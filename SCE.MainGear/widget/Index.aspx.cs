using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SCE.MainGear.DAL;
using System.Web.UI.HtmlControls;
using Microsoft.VisualBasic.FileIO;
using System.IO.Compression;
using SCE.MainGear.com.shoppingcartelite.api;
using System.Configuration;
using System.IO;

namespace SCE.MainGear.admin
{
    public partial class Index : System.Web.UI.Page
    {
        MainGearDataContext lContext = new MainGearDataContext();
        
        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadProducts();
                Session["ProductStepsViewList"] = null;
            }
            else
            {
                //RecreateSteps();
            }

            if (Convert.ToBoolean(Session["IsMaster"]))
                ulButtons.Attributes["style"] = "display:none;";
            else
                ulButtons.Attributes["style"] = "display:block;";
        }

        protected void rptrProducts_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "del")
            {
                SCE.MainGear.DAL.Product lProduct = lContext.Products.SingleOrDefault(p => p.ProductID == int.Parse(e.CommandArgument.ToString()));

                if (lProduct != null)
                {
                    lContext.Products.DeleteOnSubmit(lProduct);
                    lContext.SubmitChanges();

                    // delete product steps
                    lContext.ExecuteCommand("DELETE FROM ProductSteps WHERE ProductID = " + lProduct.ProductID.ToString());
                    lContext.SubmitChanges();

                    // delete product instruction
                    lContext.ExecuteCommand("DELETE FROM ProductInstructions WHERE ProductID = " + lProduct.ProductID.ToString());
                    lContext.SubmitChanges();

                    // delete all sections
                    var lSections = from p in lContext.Sections
                                    where p.ProductID == lProduct.ProductID
                                    select new { p.SectionID };

                    foreach (var lSection in lSections)
                    {
                        long lSectionIDToDelete = lSection.SectionID;
                        lContext.ExecuteCommand("DELETE FROM Sections WHERE SectionID = " + lSectionIDToDelete.ToString());
                        lContext.SubmitChanges();

                        // delete section instruction
                        lContext.ExecuteCommand("DELETE FROM SectionInstructions WHERE SectionID = " + lSectionIDToDelete.ToString());
                        lContext.SubmitChanges();

                        // delete all subsections
                        var lSubSections = from p in lContext.SubSections
                                        where p.SectionID == lSection.SectionID
                                        select new { p.SubSectionID };

                        foreach(var lSubSection in lSubSections)
                        {
                            long lSubSectionIDToDelete = lSubSection.SubSectionID;
                            lContext.ExecuteCommand("DELETE FROM SubSections WHERE SectionID = " + lSubSectionIDToDelete.ToString());
                            lContext.SubmitChanges();

                            // delete all section items
                            var lSectionItems = from p in lContext.SectionItems
                                                where p.SubSectionID == lSubSectionIDToDelete
                                                select new { p.SectionItemID };

                            foreach(var lSectionItem in lSectionItems)
                            {
                                long lSectionItemIDToDelete = lSectionItem.SectionItemID;
                                lContext.ExecuteCommand("DELETE FROM SectionItems WHERE SectionItemID = " + lSectionItemIDToDelete.ToString());
                                lContext.SubmitChanges();

                                // delete all item filters
                                lContext.ExecuteCommand("DELETE FROM ItemFilters WHERE SectionItemID = " + lSectionItemIDToDelete.ToString());
                                lContext.SubmitChanges();

                                // delete all item tags
                                lContext.ExecuteCommand("DELETE FROM ItemTags WHERE SectionItemID = " + lSectionItemIDToDelete.ToString());
                                lContext.SubmitChanges();
                            }
                        }
                    }

                    LoadProducts();

                    ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$.toaster({ message : 'Product deleted.', priority : 'success' });", true);
                }
                else
                {
                    ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$.toaster({ message : 'Deletion failed.', priority : 'danger' });", true);
                }
            }
            else if (e.CommandName == "section")
            {
                Response.Redirect("Sections.aspx?_=" + e.CommandArgument.ToString());
            }
            else if (e.CommandName == "instruction")
            {
                LoadInstructions(e.CommandArgument.ToString());
                txtInstructionTitle.Focus();
                ClientScript.RegisterStartupScript(this.Page.GetType(), "modal", "<script type='text/javascript'>$( document ).ready(function() { $('#instModal').modal('show')});</script>", false);
            }
            else if (e.CommandName == "edit")
            {
                txtTitle.Focus();
                hdnMode.Value = "edit";
                SCE.MainGear.DAL.Product lProduct = lContext.Products.SingleOrDefault(p => p.ProductID == int.Parse(e.CommandArgument.ToString()));

                if (lProduct != null)
                {
                    txtTitle.Text = lProduct.Title;
                    hdnProductID.Value = lProduct.ProductID.ToString();
                    // load product steps
                    var _steps = (from p in lContext.ProductSteps
                                  where p.ProductID == lProduct.ProductID
                                  orderby p.DisplayOrder
                                  select new { p.ProductStepID, p.Title, p.CreatedOn, p.CreatedBy, p.ModifiedOn, p.ModifiedBy, p.DisplayOrder, p.ProductID }).ToList();

                    if (_steps != null)
                    {
                        List<ProductStepsView> lProductStepsViews = new List<ProductStepsView>();
                        int i = 1;
                        bool isRemovable = false;
 
                        foreach (var lProductStep in _steps)
                        {
                            if (!isRemovable)
                            {
                                lProductStepsViews.Add(new ProductStepsView(lProductStep.Title, i, false, lProductStep.ProductStepID, "block", "Step " + i, false));
                                isRemovable = true;
                            }
                            else
                                lProductStepsViews.Add(new ProductStepsView(lProductStep.Title, i, false, lProductStep.ProductStepID, "block", "Step " + i, true));

                            i += 1;
                        }
                        
                        Session["ProductStepsViewList"] = lProductStepsViews;
                        rptrSteps.DataSource = lProductStepsViews;
                        rptrSteps.DataBind();
                    }

                    ClientScript.RegisterStartupScript(this.Page.GetType(), "modal", "<script type='text/javascript'>$( document ).ready(function() { $('#myModal').modal('show')});</script>", false);
                }
            }
        }

        protected void lbtnAddSteps_Click(object sender, EventArgs e)
        {
            //CreateStep();
            List<ProductStepsView> lProductStepsViews = (List<ProductStepsView>)Session["ProductStepsViewList"];

            for (int i = 0; i < rptrSteps.Items.Count; i++)
            {
                TextBox ltxtTitle = (TextBox)rptrSteps.Items[i].FindControl("txtStep");

                if (ltxtTitle.Text.Trim() != "")
                {
                    lProductStepsViews[i].Title = ltxtTitle.Text.Trim();
                }
            }
            
            lProductStepsViews.Add(new ProductStepsView("", (lProductStepsViews.Where(x => x.IsDeleted == false).Max(x => x.StepNumber) + 1), false, 0, "block", "Step " + (lProductStepsViews.Where(x => x.IsDeleted == false).Max(x => x.StepNumber) + 1).ToString(), true));
            rptrSteps.DataSource = lProductStepsViews;
            rptrSteps.DataBind();
            Session["ProductStepsViewList"] = lProductStepsViews;
            ClientScript.RegisterStartupScript(this.Page.GetType(), "modal", "<script type='text/javascript'>$( document ).ready(function() { $('#myModal').modal('show')});</script>", false);
        }

        protected void lbtnAddProduct_Click(object sender, EventArgs e)
        {
            ResetControls();
            txtTitle.Focus();
            List<ProductStepsView> lProductStepsViews = new List<ProductStepsView>();
            lProductStepsViews = new List<ProductStepsView>();
            lProductStepsViews.Add(new ProductStepsView("", 1, false, 0, "block", "Step 1", false));
            lProductStepsViews.Add(new ProductStepsView("", 2, false, 0, "block", "Step 2", true));
            lProductStepsViews.Add(new ProductStepsView("", 3, false, 0, "block", "Step 3", true));
            lProductStepsViews.Add(new ProductStepsView("", 4, false, 0, "block", "Step 4", true));
            Session["ProductStepsViewList"] = lProductStepsViews;
            rptrSteps.DataSource = lProductStepsViews;
            rptrSteps.DataBind();
            ClientScript.RegisterStartupScript(this.Page.GetType(), "modal", "<script type='text/javascript'>$( document ).ready(function() { $('#myModal').modal('show')});</script>", false);
        }
        
        protected void btnSaveProduct_Click(object sender, EventArgs e)
        {
            SaveProduct();
            ResetControls();
            LoadProducts();
            ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$.toaster({ message : 'Product saved.', priority : 'success' });", true);

        }

        protected void btnSaveInstructions_Click(object sender, EventArgs e)
        {
            ProductInstruction lProductInst = lContext.ProductInstructions.SingleOrDefault(p => p.ProductID == int.Parse(hdnProductIDForInst.Value));

            if (lProductInst != null && lProductInst.ProductInstructionID > 0) // instruction already exists
            {
                lProductInst.Title = txtInstructionTitle.Text.Trim();
                lProductInst.ShortDescription = txtShortDesc.Text.Trim();
                lProductInst.LongDescription = HttpUtility.HtmlDecode(txtLongDesc.Value.Trim());
                lProductInst.ModifiedBy = ((AdminUser)Session["AdminUser"]).UserID;
                lProductInst.ModifiedOn = DateTime.Now;
                lContext.SubmitChanges();
            }
            else // add new
            {
                lProductInst = new ProductInstruction();
                lProductInst.ProductID = int.Parse(hdnProductIDForInst.Value);
                lProductInst.Title = txtInstructionTitle.Text.Trim();
                lProductInst.ShortDescription = txtShortDesc.Text.Trim();
                lProductInst.LongDescription = HttpUtility.HtmlDecode(txtLongDesc.Value.Trim());
                lProductInst.CreatedBy = ((AdminUser)Session["AdminUser"]).UserID;
                lProductInst.CreatedOn = DateTime.Now;
                lContext.ProductInstructions.InsertOnSubmit(lProductInst);
                lContext.SubmitChanges();
            }

            ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$.toaster({ message : 'Instruction saved.', priority : 'success' });", true);
        }

        protected void lbtnRefresh_Click(object sender, EventArgs e)
        {
            FetchBTOItems();
            FetchWholesales();
            ResetControls();
            LoadProducts();
        }

        protected void btnSaveAndNextProduct_Click(object sender, EventArgs e)
        {
            long lProductID = SaveProduct();
            Response.Redirect("Sections.aspx?_=" + lProductID.ToString());
        }

        protected void rptrSteps_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if(e.CommandName == "remove")
            {
                List<ProductStepsView> lProductStepsViews = (List<ProductStepsView>)Session["ProductStepsViewList"];

                for (int i = 0; i < rptrSteps.Items.Count; i++)
                {
                    TextBox ltxtTitle = (TextBox)rptrSteps.Items[i].FindControl("txtStep");

                    if (ltxtTitle.Text.Trim() != "")
                    {
                        lProductStepsViews[i].Title = ltxtTitle.Text.Trim();
                    }
                }

                if(lProductStepsViews != null)
                {
                    if (lProductStepsViews.Where(x => x.StepNumber == Convert.ToInt32(e.CommandArgument) && x.IsDeleted == false).SingleOrDefault() != null)
                    {                        
                        lProductStepsViews.Where(x => x.StepNumber == Convert.ToInt32(e.CommandArgument) && x.IsDeleted == false).SingleOrDefault().Display = "none";
                        lProductStepsViews.Where(x => x.StepNumber == Convert.ToInt32(e.CommandArgument) && x.IsDeleted == false).SingleOrDefault().IsDeleted = true;
                        lProductStepsViews = ReorderSteps(lProductStepsViews);
                        Session["ProductStepsViewList"] = lProductStepsViews;
                        rptrSteps.DataSource = lProductStepsViews;
                        rptrSteps.DataBind();
                        ClientScript.RegisterStartupScript(this.Page.GetType(), "modal", "<script type='text/javascript'>$( document ).ready(function() { $('#myModal').modal('show')});</script>", false);
                    }
                    else
                        ClientScript.RegisterStartupScript(this.Page.GetType(), "modal", "<script type='text/javascript'>$( document ).ready(function() { $.toaster({ message : 'Unable to remove this step.', priority : 'danger' }); $('#myModal').modal('show')});</script>", false);
                }
                else
                    ClientScript.RegisterStartupScript(this.Page.GetType(), "modal", "<script type='text/javascript'>$( document ).ready(function() { $.toaster({ message : 'Unable to remove this step.', priority : 'danger' }); $('#myModal').modal('show')});</script>", false);
            }
        }

        private List<ProductStepsView> ReorderSteps(List<ProductStepsView> pProductStepsViews)
        {
            int lStepNum = 2;

            for(int i = 1; i < pProductStepsViews.Count; i++)
            {
                if (pProductStepsViews[i].IsDeleted == false)
                {
                    pProductStepsViews[i].StepNumber = lStepNum;
                    pProductStepsViews[i].StepNumberDisplay = "Step " + (lStepNum).ToString();
                    lStepNum += 1;
                }
            }

            return pProductStepsViews;
        }

        protected void rptrProducts_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (Convert.ToBoolean(Session["IsMaster"]))
                    ((LinkButton)e.Item.FindControl("lbtnDelProduct")).Visible = false;
            }
        }

        #endregion

        #region Functions
        
        private long SaveProduct()
        {
            SCE.MainGear.DAL.Product lProduct = new SCE.MainGear.DAL.Product();

            if (hdnMode.Value == "add") // add new product
            {
                lProduct.Title = txtTitle.Text.Trim();
                lProduct.CreatedBy = ((AdminUser)Session["AdminUser"]).UserID;
                lProduct.CreatedOn = DateTime.Now;
                lProduct.IsMaster = Convert.ToBoolean(Session["IsMaster"]);

                int lDisplayOrder = 1;

                try
                {
                    lDisplayOrder = lContext.Products.OrderByDescending(u => u.DisplayOrder).FirstOrDefault().DisplayOrder;
                }
                catch
                { }

                lProduct.DisplayOrder = lDisplayOrder + 1;
                lContext.Products.InsertOnSubmit(lProduct);
                lContext.SubmitChanges();
            }
            else // update product
            {
                lProduct = lContext.Products.SingleOrDefault(p => p.ProductID == int.Parse(hdnProductID.Value.Trim()));

                if (lProduct != null && lProduct.ProductID > 0)
                {
                    lProduct.Title = txtTitle.Text.Trim();
                    lProduct.ModifiedBy = ((AdminUser)Session["AdminUser"]).UserID;
                    lProduct.ModifiedOn = DateTime.Now;
                    lProduct.IsMaster = Convert.ToBoolean(Session["IsMaster"]);
                    lContext.SubmitChanges();
                }
            }

            if (lProduct.ProductID > 0) // save steps
            {
                List<ProductStepsView> lProductStepsViews = (List<ProductStepsView>)Session["ProductStepsViewList"];

                for (int i = 0; i < rptrSteps.Items.Count; i++)
                {
                    TextBox ltxtTitle = (TextBox)rptrSteps.Items[i].FindControl("txtStep");

                    if (ltxtTitle.Text.Trim() != "")
                    {
                        if (lProductStepsViews[i].ProductStepID == 0 && lProductStepsViews[i].IsDeleted == false) // add new
                        {
                            ProductStep lProductStep = new ProductStep();
                            lProductStep.ProductID = lProduct.ProductID;
                            lProductStep.Title = ltxtTitle.Text.Trim();
                            lProductStep.CreatedBy = ((AdminUser)Session["AdminUser"]).UserID;
                            lProductStep.CreatedOn = DateTime.Now;
                            lProductStep.DisplayOrder = lProductStepsViews[i].StepNumber;
                            lContext.ProductSteps.InsertOnSubmit(lProductStep);
                            lContext.SubmitChanges();
                        }
                        else if (lProductStepsViews[i].ProductStepID > 0 && lProductStepsViews[i].IsDeleted == false) // update
                        {
                            ProductStep lProductStep = lContext.ProductSteps.SingleOrDefault(p => p.ProductStepID == lProductStepsViews[i].ProductStepID);

                            if (lProductStep != null && lProductStep.ProductID > 0)
                            {
                                lProductStep.Title = ltxtTitle.Text.Trim();
                                lProductStep.ModifiedBy = ((AdminUser)Session["AdminUser"]).UserID;
                                lProductStep.ModifiedOn = DateTime.Now;
                                lProductStep.DisplayOrder = lProductStepsViews[i].StepNumber;
                                lContext.SubmitChanges();
                            }
                        }
                        else if (lProductStepsViews[i].ProductStepID > 0 && lProductStepsViews[i].IsDeleted == true)
                        {
                            lContext.ExecuteCommand("DELETE FROM ProductSteps WHERE ProductStepID = " + lProductStepsViews[i].ProductStepID.ToString());
                            lContext.SubmitChanges();
                        }
                    }
                }
            }

            return lProduct.ProductID;
        }

        private void FetchWholesales()
        {
            try
            {
                com.shoppingcartelite.api.sceApi api = new com.shoppingcartelite.api.sceApi();
                AuthHeaderAPI header = new AuthHeaderAPI();
                header.ApiKey = ConfigurationManager.AppSettings["SceApiKey"];
                header.ApiSecretKey = ConfigurationManager.AppSettings["SceApiSecretKey"];
                header.ApiAccessKey = ConfigurationManager.AppSettings["SceApiAccessKey"];
                //header.StoreID = "maingearPublic"; header.StoreID = "maingear"; header.StoreID = "ecommerceventure";
                api.AuthHeaderAPIValue = header;
                int lTotalItems = 0;

                WDLevel[] lWDLevels = api.GetWDLevels(1);

                foreach (var lWDLevel in lWDLevels)
                {
                    foreach (var lProduct in lWDLevel.Products)
                    {
                        try
                        {
                            //WriteToFile("Wholesale Part No: " + lProduct.PartNo.ToString() + " --- Discount: " + lProduct.Discount.ToString());
                            Wholesale lWholesale = new Wholesale();

                            if (lProduct.PartNo != null && lProduct.PartNo.Trim() != "")
                            {
                                lWholesale = lContext.Wholesales.FirstOrDefault(p => p.PartNumber.Trim().ToLower() == lProduct.PartNo.Trim().ToLower());
                            }

                            if (lWholesale != null && lWholesale.WholesaleID > 0) // update
                            {
                                lWholesale.PartNumber = lProduct.PartNo == null ? "" : lProduct.PartNo;
                                lWholesale.ProductID = lProduct.ProdID.ToString();
                                lWholesale.Brand = "";
                                lWholesale.Discount = lProduct.Discount == null ? "0" : lProduct.Discount.ToString();
                                lWholesale.DiscountType = lProduct.DiscountType.ToString();
                                lWholesale.LevelName = "";
                                lWholesale.ManufacturePartNumber = "";
                                lWholesale.Price = "";
                                lWholesale.Title = "";
                                lContext.SubmitChanges();
                                lTotalItems += 1;

                            }
                            else // add new
                            {
                                lWholesale = new Wholesale();
                                lWholesale.PartNumber = lProduct.PartNo == null ? "" : lProduct.PartNo;
                                lWholesale.ProductID = lProduct.ProdID == null ? "" : lProduct.ProdID.ToString();
                                lWholesale.Brand = "";
                                lWholesale.Discount = lProduct.Discount == null ? "0" : lProduct.Discount.ToString();
                                lWholesale.DiscountType = lProduct.DiscountType.ToString();
                                lWholesale.LevelName = "";
                                lWholesale.ManufacturePartNumber = "";
                                lWholesale.Price = "";
                                lWholesale.Title = "";
                                lWholesale.CreatedOn = DateTime.Now;
                                lContext.Wholesales.InsertOnSubmit(lWholesale);
                                lContext.SubmitChanges();
                                lTotalItems += 1;
                            }
                        }
                        catch (Exception ex)
                        {
                            //this.WriteToFile("Wholesale MESSAGE => " + ex.Message + "\n\n INNER EXCEPTION => " + ex.InnerException + "\n\n STACK TRACE => " + ex.StackTrace);
                        }
                    }
                }

                //this.WriteToFile(lTotalItems.ToString() + " Wholesale Items imported successfully.");
            }
            catch (Exception ex)
            {
                //this.WriteToFile("Wholesale MESSAGE => " + ex.Message + "\n\n INNER EXCEPTION => " + ex.InnerException + "\n\n STACK TRACE => " + ex.StackTrace);
            }
        }

        private void FetchBTOItems()
        {
            try
            {
                //int lSize = Convert.ToInt32(ConfigurationManager.AppSettings["ProductArraySize"]);
                //this.WriteToFile("Array Size: " + lSize.ToString());
                //string[] lProductsIDs = new string[lSize];
                string lRootPath = Server.MapPath("");
                com.shoppingcartelite.api.sceApi api = new com.shoppingcartelite.api.sceApi();
                AuthHeaderAPI header = new AuthHeaderAPI();
                header.ApiKey = ConfigurationManager.AppSettings["SceApiKey"];
                header.ApiSecretKey = ConfigurationManager.AppSettings["SceApiSecretKey"];
                header.ApiAccessKey = ConfigurationManager.AppSettings["SceApiAccessKey"];
                //header.StoreID = "maingearPublic"; header.StoreID = "maingear"; header.StoreID = "ecommerceventure";
                api.AuthHeaderAPIValue = header;

                //for (int i = 0; i < lProductsIDs.Length; i++)
                //{
                //    lProductsIDs[i] = (i + 1).ToString();
                //}

                byte[] zip = api.GetFullProductsExport(",");

                if (zip != null)
                {
                    if (!Directory.Exists(lRootPath + "\\imports"))
                        Directory.CreateDirectory(lRootPath + "\\imports");

                    System.IO.DirectoryInfo di = new DirectoryInfo(lRootPath + "\\imports");

                    foreach (FileInfo file in di.GetFiles())
                    {
                        file.Delete();
                    }

                    string startPath = lRootPath + "\\imports\\";
                    string zipPath = lRootPath + "\\imports\\" + System.Guid.NewGuid().ToString() + ".zip";
                    string extractPath = lRootPath + "\\imports\\";

                    File.WriteAllBytes(zipPath, zip);
                    System.IO.Compression.ZipFile.ExtractToDirectory(zipPath, extractPath);

                    // delete all rows from BTOItem table
                    //lContext.ExecuteCommand("TRUNCATE TABLE BTOItems");

                    // parse csv
                    using (TextFieldParser parser = new TextFieldParser(lRootPath + "\\imports\\newSceExport1.csv"))
                    {
                        parser.TextFieldType = FieldType.Delimited;
                        parser.SetDelimiters(",");
                        bool lIsFirstRow = true;
                        int lTotalItems = 0;

                        while (!parser.EndOfData)
                        {
                            try
                            {
                                string[] fields = parser.ReadFields();

                                if (!lIsFirstRow)
                                {
                                    //Processing row
                                    BTOItem lBTOItem = lContext.BTOItems.FirstOrDefault(p => p.ProductID.Trim().ToLower() + "-" + p.PartNo.Trim().ToLower() == fields[1].Trim().ToLower() + "-" + fields[147].Trim().ToLower());

                                    if (lBTOItem != null && lBTOItem.BTOItemID > 0) // update existing item
                                    {
                                        // 1, 4, 14, 158, 147, 148, 27, 152, 29, 39, 50, 15, 76
                                        lBTOItem.ProductID = fields[1];
                                        lBTOItem.ProductTitle = fields[4];
                                        lBTOItem.Brand = fields[14];
                                        lBTOItem.WebPrice = fields[158];
                                        lBTOItem.PartNo = fields[147];
                                        lBTOItem.ManufacturerPartNo = fields[148];
                                        lBTOItem.GeneralImage = fields[27];
                                        lBTOItem.ApplicationSpecificImage = fields[152];
                                        lBTOItem.MainCategory = fields[29];
                                        lBTOItem.SubCategory = fields[39];
                                        lBTOItem.SectionCategory = fields[50];
                                        lBTOItem.Description = fields[15];
                                        lBTOItem.Specification = fields[76];
                                        lBTOItem.IsActive = true;
                                        lContext.SubmitChanges();
                                    }
                                    else // add new
                                    {
                                        lBTOItem = new BTOItem();
                                        // 1, 4, 14, 158, 147, 148, 27, 152, 29, 39, 50, 15, 76
                                        lBTOItem.ProductID = fields[1];
                                        lBTOItem.ProductTitle = fields[4];
                                        lBTOItem.Brand = fields[14];
                                        lBTOItem.WebPrice = fields[158];
                                        lBTOItem.PartNo = fields[147];
                                        lBTOItem.ManufacturerPartNo = fields[148];
                                        lBTOItem.GeneralImage = fields[27];
                                        lBTOItem.ApplicationSpecificImage = fields[152];
                                        lBTOItem.MainCategory = fields[29];
                                        lBTOItem.SubCategory = fields[39];
                                        lBTOItem.SectionCategory = fields[50];
                                        lBTOItem.Description = fields[15];
                                        lBTOItem.Specification = fields[76];
                                        lBTOItem.CreatedOn = DateTime.Now;
                                        lBTOItem.IsActive = true;
                                        lContext.BTOItems.InsertOnSubmit(lBTOItem);
                                        lContext.SubmitChanges();
                                    }

                                    lTotalItems += 1;
                                }
                                else
                                    lIsFirstRow = false;
                            }
                            catch (Exception ex)
                            {
                                //this.WriteToFile("BTO Item MESSAGE => " + ex.Message + "\n\n INNER EXCEPTION => " + ex.InnerException + "\n\n STACK TRACE => " + ex.StackTrace);
                            }
                        }

                        //this.WriteToFile(lTotalItems.ToString() + " BTO Items imported successfully.");
                    }
                }
            }
            catch (Exception ex)
            {
                //this.WriteToFile("BTO Item MESSAGE => " + ex.Message + "\n\n INNER EXCEPTION => " + ex.InnerException + "\n\n STACK TRACE => " + ex.StackTrace);
            }
        }

        private void LoadInstructions(string pProductID)
        {
            hdnProductIDForInst.Value = pProductID;
            ProductInstruction lProductInst = lContext.ProductInstructions.SingleOrDefault(p => p.ProductID == int.Parse(pProductID));

            if (lProductInst != null && lProductInst.ProductInstructionID > 0) // instruction already exists
            {
                txtInstructionTitle.Text = lProductInst.Title;
                txtShortDesc.Text = lProductInst.ShortDescription;
                txtLongDesc.Value = lProductInst.LongDescription;
            }
            else
            {
                txtInstructionTitle.Text = string.Empty;
                txtShortDesc.Text = string.Empty;
                txtLongDesc.Value = string.Empty;
            }
        }

        //private void RecreateSteps()
        //{
        //    int lTotalSteps = Convert.ToInt32(hdnTotalSteps.Value);

        //    if (lTotalSteps > 4)
        //    {
        //        for (int i = 5; i <= lTotalSteps; i++)
        //        {
        //            HtmlGenericControl lMainDiv = new HtmlGenericControl("DIV");
        //            lMainDiv.Attributes.Add("class", "form-group");
        //            lMainDiv.Attributes.Add("id", "divStep" + i.ToString());

        //            HtmlGenericControl lLabel = new HtmlGenericControl("LABEL");
        //            lLabel.Attributes.Add("class", "control-label col-xs-2");
        //            lLabel.InnerHtml = "Step " + i.ToString();
        //            lLabel.Attributes.Add("id", "lblStep" + i.ToString());

        //            HtmlGenericControl lInnerDiv = new HtmlGenericControl("DIV");
        //            lInnerDiv.Attributes.Add("class", "col-xs-8");
        //            lInnerDiv.Attributes.Add("id", "divInnerStep" + i.ToString());

        //            TextBox lTextBox = new TextBox();
        //            lTextBox.ID = "txtStep" + i.ToString();
        //            lTextBox.CssClass = "form-control col-md-7 col-xs-12";
        //            lTextBox.Attributes.Add("placeholder", "Step " + i.ToString());

        //            HiddenField lHiddenField = new HiddenField();
        //            lHiddenField.ID = "hdnStep" + i.ToString();

        //            lInnerDiv.Controls.Add(lTextBox);
        //            lInnerDiv.Controls.Add(lHiddenField);
        //            lMainDiv.Controls.Add(lLabel);
        //            lMainDiv.Controls.Add(lInnerDiv);
        //            pnlSteps.Controls.Add(lMainDiv);
        //        }
        //    }
        //}

        private void LoadProducts()
        {
            var lProducts = from p in lContext.Products
                            where p.IsMaster == Convert.ToBoolean(Session["IsMaster"])
                            orderby p.DisplayOrder
                            select new { p.ProductID, p.Title, p.CreatedOn, p.CreatedBy, p.ModifiedOn, p.ModifiedBy, p.DisplayOrder, p.IsMaster };

            rptrProducts.DataSource = lProducts;
            rptrProducts.DataBind();
        }

        //private void CreateStep()
        //{
        //    int lTotalSteps = Convert.ToInt32(hdnTotalSteps.Value);
        //    int lNewStepNumber = lTotalSteps + 1;

        //    HtmlGenericControl lClearDiv = new HtmlGenericControl("DIV");
        //    lClearDiv.Attributes.Add("class", "clearfix");
            
        //    HtmlGenericControl lMainDiv = new HtmlGenericControl("DIV");
        //    lMainDiv.Attributes.Add("class", "form-group");
        //    lMainDiv.Attributes.Add("id", "divStep" + lNewStepNumber.ToString());

        //    HtmlGenericControl lLabel = new HtmlGenericControl("LABEL");
        //    lLabel.Attributes.Add("class", "control-label col-xs-2");
        //    lLabel.InnerHtml = "Step " + lNewStepNumber.ToString();
        //    lLabel.Attributes.Add("id", "lblStep" + lNewStepNumber.ToString());

        //    HtmlGenericControl lInnerDiv = new HtmlGenericControl("DIV");
        //    lInnerDiv.Attributes.Add("class", "col-xs-8");
        //    lInnerDiv.Attributes.Add("id", "divInnerStep" + lNewStepNumber.ToString());

        //    TextBox lTextBox = new TextBox();
        //    lTextBox.ID = "txtStep" + lNewStepNumber.ToString();
        //    lTextBox.CssClass = "form-control col-md-7 col-xs-12";
        //    lTextBox.Attributes.Add("placeholder", "Step " + lNewStepNumber.ToString());

        //    HiddenField lHiddenField = new HiddenField();
        //    lHiddenField.ID = "hdnStep" + lNewStepNumber.ToString();

        //    lInnerDiv.Controls.Add(lTextBox);
        //    lInnerDiv.Controls.Add(lHiddenField);
        //    lMainDiv.Controls.Add(lLabel);
        //    lMainDiv.Controls.Add(lInnerDiv);
        //    pnlSteps.Controls.Add(lClearDiv);
        //    pnlSteps.Controls.Add(lMainDiv);

        //    hdnTotalSteps.Value = lNewStepNumber.ToString();
        //}

        private void ResetControls()
        {
            txtTitle.Text = string.Empty;
            hdnMode.Value = "add";
            hdnProductID.Value = "";
            txtInstructionTitle.Text = string.Empty;
            txtLongDesc.InnerText = string.Empty;
            txtShortDesc.Text = string.Empty;
            hdnProductIDForInst.Value = string.Empty;
            Session["ProductStepsViewList"] = null;
        }

        #endregion       

    }

    public class ProductStepsView
    {
        private long _ProductStepID;

        public long ProductStepID
        {
            get { return _ProductStepID; }
            set { _ProductStepID = value; }
        }
        private string _Title;

        public string Title
        {
            get { return _Title; }
            set { _Title = value; }
        }
        private long _ProductID;

        public long ProductID
        {
            get { return _ProductID; }
            set { _ProductID = value; }
        }
        private int _DisplayOrder;

        public int DisplayOrder
        {
            get { return _DisplayOrder; }
            set { _DisplayOrder = value; }
        }
        private System.DateTime _CreatedOn;

        public System.DateTime CreatedOn
        {
            get { return _CreatedOn; }
            set { _CreatedOn = value; }
        }
        private long _CreatedBy;

        public long CreatedBy
        {
            get { return _CreatedBy; }
            set { _CreatedBy = value; }
        }
        private System.Nullable<System.DateTime> _ModifiedOn;

        public System.Nullable<System.DateTime> ModifiedOn
        {
            get { return _ModifiedOn; }
            set { _ModifiedOn = value; }
        }
        private System.Nullable<long> _ModifiedBy;

        public System.Nullable<long> ModifiedBy
        {
            get { return _ModifiedBy; }
            set { _ModifiedBy = value; }
        }
        private bool _IsDeleted;

        public bool IsDeleted
        {
            get { return _IsDeleted; }
            set { _IsDeleted = value; }
        }
        private int _StepNumber;

        public int StepNumber
        {
            get { return _StepNumber; }
            set { _StepNumber = value; }
        }

        public ProductStepsView(string pTitle, int pStepNumber, bool pIsDeleted, long pProductStepID, string pDisplay, string pStepNumberDisplay, bool pIsRemovable)
		{
            this.StepNumber = pStepNumber;
            this.IsDeleted = pIsDeleted;
            this.ProductStepID = pProductStepID;
            this.Display = pDisplay;
            this.StepNumberDisplay = pStepNumberDisplay;
            this.IsRemovable = pIsRemovable;
            this.Title = pTitle;
		}

        private string _Display;

        public string Display
        {
            get { return _Display; }
            set { _Display = value; }
        }

        private string _StepNumberDisplay;

        public string StepNumberDisplay
        {
            get { return _StepNumberDisplay; }
            set { _StepNumberDisplay = value; }
        }
        private bool _IsRemovable;

        public bool IsRemovable
        {
            get { return _IsRemovable; }
            set { _IsRemovable = value; }
        }
    }
}