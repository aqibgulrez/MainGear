using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using SCE.MainGear.API.Models;

namespace SCE.MainGear.API
{
    public partial class ProductForm : System.Web.UI.Page
    {
        

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        [System.Web.Services.WebMethod(BufferResponse = false), ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public static string GetProductJSON(string ProductID)
        {
            MainGearWidgetEntities db_Context = new MainGearWidgetEntities();
            long lProductID;
            try
            {
                lProductID = Convert.ToInt64(ProductID);
            }
            catch
            {
                return null;
            }


            Product lProduct = db_Context.Products.SingleOrDefault(p => p.ProductID == lProductID);
            if(lProduct == null) return null;


            lProduct.Steps = db_Context.ProductSteps.Where(ps => ps.ProductID == lProductID).ToList();
            lProduct.Sections = db_Context.Sections.Where(s => s.ProductID == lProductID).ToList();
            lProduct.Instructions =  db_Context.ProductInstructions.Where(pi => pi.ProductID == lProductID).ToList();

            foreach(Section section in lProduct.Sections){
                section.Instructions = db_Context.SectionInstructions.Where(si => si.SectionID == section.SectionID).ToList();
                section.SubSections = db_Context.SubSections.Where(ss => ss.SectionID == section.SectionID).ToList();
                foreach(SubSection subSection in section.SubSections){
                    subSection.SubSectionItems = db_Context.SectionItems.Where(si => si.SubSectionID == subSection.SubSectionID).ToList();
                    foreach(SectionItem sectionItem in subSection.SubSectionItems){
                        sectionItem.ItemFilters = db_Context.ItemFilters.Where(itf => itf.SectionItemID == sectionItem.SectionItemID).ToList();
                        
                        sectionItem.ItemTags = db_Context.ItemTags.Where(it => it.SectionItemID == sectionItem.SectionItemID).ToList();
                        foreach (ItemTag tag in sectionItem.ItemTags)
                        {
                            tag.TagText = db_Context.Tags.Single(t => t.TagID == tag.TagID).TagText;
                        }

                        var SectionItemOptions = db_Context.SectionItemOptions.Where(sio => sio.SectionItemID == sectionItem.SectionItemID);
                        if (SectionItemOptions.Count() > 0)
                        {
                            foreach (SectionItemOption option in SectionItemOptions)
                            {
                                var optionVm = new SectionItemOptionViewModel();
                                var OptionsBTOItem = db_Context.BTOItems.Single(i => i.BTOItemID == option.BTOItemID);

                                optionVm.SectionItemOptionID = option.SectionItemOptionID;
                                optionVm.OptionName = OptionsBTOItem.PrimaryOptionTitle;
                                optionVm.OptionChoice = OptionsBTOItem.PrimaryOptionValue;
                                optionVm.SectionItemID = sectionItem.SectionItemID;

                                sectionItem.Options.Add(optionVm);
                            }
                        }

                        sectionItem.WholeSale = db_Context.Wholesales.SingleOrDefault(ws => ws.WholesaleID == sectionItem.WholesaleID);
                        sectionItem.BTOItem = db_Context.BTOItems.SingleOrDefault(bi => bi.BTOItemID == sectionItem.BTOItemID);
                    }
                }
            }

            return JsonConvert.SerializeObject(lProduct, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            });
        }


        [System.Web.Services.WebMethod(BufferResponse = false), ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public static string PutProductJSON(string lProduct)
        {
            MainGearWidgetEntities db_Context = new MainGearWidgetEntities();
            string ProductJson = lProduct.ToString();
            try {
                using (var sr = new StringReader(lProduct.ToString()))
                using (var jr = new JsonTextReader(sr))
                {
                    var js = new JsonSerializer();
                    var product = js.Deserialize<Product>(jr);

                    //SaveProduct(product);

                    return JsonConvert.SerializeObject(product);
                }
            }
            catch (Exception ex) {
                return ex.Message;
            }
        }


        [System.Web.Services.WebMethod(BufferResponse = false), ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = false)]
        public static string PlaceOrder(string ProductOrderJSON)
        {
            MainGearWidgetEntities db_Context = new MainGearWidgetEntities();
            //// test Product Order
            //ProductOrderVM testOrder = new ProductOrderVM();
            //testOrder.TotalPrice = 899.99m;
            //testOrder.EstimatedShippingDate = DateTime.Now.AddDays(3);
            //testOrder.Items = new List<ProductOrderItemVM> { 
            //    new ProductOrderItemVM{
            //        SectionItemID = 3,
            //        Quantity= 3,
            //        ItemPrice = 3.00M,
            //    },
            //    new ProductOrderItemVM{
            //        SectionItemID = 4,
            //        Quantity= 4,
            //        ItemPrice = 4.00M,
            //    },
            //    new ProductOrderItemVM{
            //        SectionItemID = 5,
            //        Quantity= 5,
            //        ItemPrice = 5.00M,
            //    }
            //};
            //return JsonConvert.SerializeObject(testOrder);

           
            try
            {
                using (var sr = new StringReader(ProductOrderJSON))
                using (var jr = new JsonTextReader(sr))
                {
                    var js = new JsonSerializer();
                    var ProductOrderViewModel = js.Deserialize<ProductOrderVM>(jr);

                    //Save Product Order
                    SaveProductOrder(ProductOrderViewModel);

                    return JsonConvert.SerializeObject(ProductOrderViewModel);
                }
                 
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }



        private static void SaveProduct(Product product)
        {
            MainGearWidgetEntities db_Context = new MainGearWidgetEntities();
            using (var dbContextTransaction = db_Context.Database.BeginTransaction())
            {
                try
                {
                    db_Context.Products.Add(product);
                    db_Context.SaveChanges();
                    var pid = product.ProductID;
                    if (product.Steps.Count > 0)
                    {
                        foreach (ProductStep step in product.Steps) { step.ProductID = pid; }
                        db_Context.ProductSteps.AddRange(product.Steps);
                    }
                    if (product.Sections.Count > 0)
                    {
                        foreach (Section section in product.Sections)
                        {
                            section.ProductID = pid;
                            db_Context.Sections.Add(section);
                            db_Context.SaveChanges();
                            var sid = section.SectionID;
                                    
                            if (section.SubSections.Count > 0) { 
                                foreach (SubSection subSection in section.SubSections)
                                {
                                    subSection.SectionID = sid;
                                    db_Context.SubSections.Add(subSection);
                                    db_Context.SaveChanges();
                                    var ssid = section.SectionID;
                                            
                                    if (subSection.SubSectionItems.Count > 0)
                                    {
                                        foreach (SectionItem sectionItem in subSection.SubSectionItems)
                                        {
                                            sectionItem.SubSectionID = ssid;
                                            db_Context.SectionItems.Add(sectionItem);
                                            db_Context.SaveChanges();
                                            var ssiid = sectionItem.SectionItemID;


                                            if (sectionItem.ItemFilters.Count > 0)
                                            {
                                                foreach (ItemFilter filter in sectionItem.ItemFilters) { filter.SectionItemID = ssiid; }
                                                db_Context.ItemFilters.AddRange(sectionItem.ItemFilters);
                                                       
                                            }
                                            if (sectionItem.ItemTags.Count > 0)
                                            {
                                                foreach (ItemTag tag in sectionItem.ItemTags) { tag.SectionItemID = ssiid; }
                                                db_Context.ItemTags.AddRange(sectionItem.ItemTags);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    dbContextTransaction.Commit();
                }
                catch (Exception ex)
                {
                    dbContextTransaction.Rollback();
                    throw ex;
                }
            }
        }
        private static void SaveProductOrder(ProductOrderVM ProductOrderViewModel)
        {
            MainGearWidgetEntities db_Context = new MainGearWidgetEntities();
            using (var transaction = db_Context.Database.BeginTransaction())
            {
                try
                {
                    ProductOrder Order = new ProductOrder();
                    Order.ShippingDate = ProductOrderViewModel.EstimatedShippingDate;
                    Order.TotalPrice = ProductOrderViewModel.TotalPrice;
                    Order.PlacedOn = DateTime.Now;
                    Order.OrderStatus = "Pending";

                    db_Context.ProductOrders.Add(Order);
                    db_Context.SaveChanges();
                    long oid = Order.OrderID;

                    foreach (ProductOrderItemVM orderItemVM in ProductOrderViewModel.Items)
                    {

                        // check if item with specific ID actually exist or not
                        // if yes then go on adding
                        //if no then throw exception
                        SectionItem lSectionItem = db_Context.SectionItems.SingleOrDefault(i => i.SectionItemID == orderItemVM.SectionItemID);
                        if (lSectionItem != null)
                        {

                            // build prodcut order item from Order item View Model
                            OrderItem lOrderItem = new API.OrderItem();
                            lOrderItem.OrderID = oid;
                            lOrderItem.SectionItemID = orderItemVM.SectionItemID;
                            lOrderItem.BTOItemID = lSectionItem.BTOItemID;
                            lOrderItem.WholesaleID = lSectionItem.WholesaleID;
                            lOrderItem.ItemPrice = orderItemVM.ItemPrice;
                            lOrderItem.Quantity = orderItemVM.Quantity;

                            db_Context.OrderItems.Add(lOrderItem);
                        }
                        else
                        {
                            throw new InvalidDataException("Invalid Item ID");
                        }
                    }
                    db_Context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }

            }
        }
        private static OrderItem BuildOrderItem(ProductOrderItemVM orderItemVM, long orderID, SectionItem sectionItem){
            // build prodcut order from VMs
            OrderItem OrderItem = new API.OrderItem();
            OrderItem.OrderID = orderID;
            OrderItem.SectionItemID = orderItemVM.SectionItemID;
            OrderItem.BTOItemID = sectionItem.BTOItemID;
            OrderItem.WholesaleID = sectionItem.WholesaleID;
            OrderItem.ItemPrice = orderItemVM.ItemPrice;
            OrderItem.Quantity = orderItemVM.Quantity;

            return OrderItem;
        }
    }
    public class UserResults
    {
        public Product Product { get; set; }
    }
}