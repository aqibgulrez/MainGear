using SCE.MainGear.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SCE.MainGear.admin
{
    public partial class BTOItems : System.Web.UI.Page
    {
        MainGearDataContext lContext = new MainGearDataContext();

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadBTOItems();
            }
        }

        protected void lbtnExport_Click(object sender, EventArgs e)
        {
            var lProducts = from p in lContext.Products
                            where p.IsMaster == false || p.IsMaster == null
                            orderby p.DisplayOrder
                            select new { p.ProductID, p.Title };

            var lSections = from p in lContext.Sections
                            where p.IsMaster == false || p.IsMaster == null
                            orderby p.DisplayOrder
                            select new
                            {
                                p.SectionID,                                
                                p.PricingID,                                
                                p.ProductID,
                                p.Title
                            };

            var lSubSections = from p in lContext.SubSections
                               where p.IsMaster == false || p.IsMaster == null
                               orderby p.DisplayOrder
                               select new
                               {
                                   p.SubSectionID,
                                   p.Title,
                                   p.SectionID
                               };

            var lSectionItems = from p in lContext.SectionItems
                                where p.IsMaster == false || p.IsMaster == null
                                orderby p.DisplayOrder
                                select new
                                {
                                    p.SectionItemID,
                                    p.SubSectionID,
                                    p.BTOItemID
                                };

            var lBTOItems = from p in lContext.BTOItems
                            select new
                            {
                                p.ApplicationSpecificImage,
                                p.Brand,
                                p.BTOItemID,
                                p.CreatedOn,
                                p.Description,
                                p.GeneralImage,
                                p.MainCategory,
                                p.ManufacturerPartNo,
                                p.PartNo,
                                p.ProductTitle,
                                p.SectionCategory,
                                p.Specification,
                                p.SubCategory,
                                p.WebPrice,
                                p.IsActive
                            };

            StringBuilder lItems = new StringBuilder("");
            lItems.AppendLine("BTOItemID,ProductTitle,Brand,WebPrice,PartNo,ManufacturerPartNo,GeneralImage,ApplicationSpecificImage,MainCategory,SubCategory,SectionCategory,Description,Specification,CreatedOn,IsActive,Configuarations");

            foreach (var lBTOItem in lBTOItems)
            {
                // build configuaration string
                string lConfiguaration = string.Empty;
                var lFilteredSectionItems = lSectionItems.Where(x => x.BTOItemID == lBTOItem.BTOItemID);

                foreach (var lSectionItem in lFilteredSectionItems)
                {
                    var lFilteredSubSections = lSubSections.Where(x => x.SubSectionID == lSectionItem.SubSectionID);

                    foreach (var lSubSection in lFilteredSubSections)
                    {
                        var lFilteredSections = lSections.Where(x => x.SectionID == lSubSection.SectionID);

                        foreach (var lSection in lFilteredSections)
                        {
                            var lFilteredProducts = lProducts.Where(x => x.ProductID == lSection.ProductID);

                            foreach (var lProduct in lFilteredProducts)
                            {
                                if (lConfiguaration.Trim() != "")
                                    lConfiguaration += lConfiguaration.Trim() + "|";

                                lConfiguaration += lProduct.Title.Trim().Replace(",", "'") + " (Section: " + lSection.Title.Trim().Replace(",", "'") + ")";
                            }
                        }
                    }
                }

                // build final string
                lItems.AppendLine(lBTOItem.BTOItemID.ToString() + "," + lBTOItem.ProductTitle.Trim().Replace(",", "'") + "," + lBTOItem.Brand.Trim().Replace(",", "'") + "," + lBTOItem.WebPrice.Trim().Replace(",", "'") + "," + lBTOItem.PartNo.Trim().Replace(",", "'") + "," + lBTOItem.ManufacturerPartNo.Trim().Replace(",", "'") + "," + lBTOItem.GeneralImage.Trim().Replace(",", "'") + "," + lBTOItem.ApplicationSpecificImage.Trim().Replace(",", "'") + "," + lBTOItem.MainCategory.Trim().Replace(",", "'") + "," + lBTOItem.SubCategory.Trim().Replace(",", "'") + "," + lBTOItem.SectionCategory.Trim().Replace(",", "'") + "," + lBTOItem.Description.Trim().Replace(",", "'") + "," + lBTOItem.Specification.Trim().Replace(",", "'") + "," + lBTOItem.CreatedOn.ToString() + "," + lBTOItem.IsActive.ToString() + "," + lConfiguaration.Trim().Replace(",", "'"));
            }

            if(lItems.ToString().Trim() != "")
            {
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=BTOItems.csv");
                Response.Charset = Encoding.UTF8.EncodingName;
                Response.ContentType = "application/text";
                Response.ContentEncoding = System.Text.Encoding.Unicode;
                Response.Output.Write(lItems.ToString().Trim());
                Response.Flush();
                Response.End();
            }
        }
        
        protected void rptrBTOItems_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            //this needs to be set again on post back
            CheckBox chk = (CheckBox)e.Item.FindControl("chkIsActive");
            HiddenField lhdnBTOItemID = (HiddenField)e.Item.FindControl("hdnBTOItemID");

            Action<object, EventArgs> handler = (s, args) => CheckChanged(Convert.ToInt32(lhdnBTOItemID.Value));
            chk.CheckedChanged += new EventHandler(handler);
        }

        private void CheckChanged(int pBTOItemID)
        {
            BTOItem lBTOItem = lContext.BTOItems.SingleOrDefault(p => p.BTOItemID == pBTOItemID);

            if (lBTOItem != null)
            {
                if (Convert.ToBoolean(lBTOItem.IsActive))
                    lBTOItem.IsActive = false;
                else
                    lBTOItem.IsActive = true;

                lContext.SubmitChanges();
                LoadBTOItems();
            }
        }

        protected void lbtnSearch_Click(object sender, EventArgs e)
        {
            LoadBTOItems();
        }

        #endregion

        #region Functions

        private void LoadBTOItems()
        {
            var lBTOItems = from p in lContext.BTOItems
                            select new
                            {
                                p.ApplicationSpecificImage,
                                p.Brand,
                                p.BTOItemID,
                                p.CreatedOn,
                                p.Description,
                                p.GeneralImage,
                                p.MainCategory,
                                p.ManufacturerPartNo,
                                p.PartNo,
                                p.ProductTitle,
                                p.SectionCategory,
                                p.Specification,
                                p.SubCategory,
                                p.WebPrice,
                                p.IsActive
                            };

            if (drpActive.SelectedIndex != 0)
            {
                if (drpActive.SelectedItem.Value == "1") // active
                    lBTOItems = lBTOItems.Where(x => x.IsActive == true);
                else
                    lBTOItems = lBTOItems.Where(x => x.IsActive == false);
            }

            if (txtSearchText.Text.Trim() != "")
            {
                lBTOItems = lBTOItems.Where(x => x.ProductTitle.Contains(txtSearchText.Text.Trim()));
            }

            rptrBTOItems.DataSource = lBTOItems;
            rptrBTOItems.DataBind();
        }
        
        #endregion

    }
}