using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SCE.MainGear.DAL;
using System.IO;
using System.Web.Services;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Data;

namespace SCE.MainGear.admin
{
    public partial class addsection : System.Web.UI.Page
    {
        MainGearDataContext lContext = new MainGearDataContext();
        static IEnumerable<BTOItem> lBTOItems;
        static IQueryable<Recommendation> lRecommendations;
        static IQueryable<Wholesale> lWholesales;
        
        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Convert.ToBoolean(Session["IsMaster"]))
                {
                    divSteps.Attributes.Add("style", "display:none;");
                }

                List<SectionItem> lSectionItems = new List<SectionItem>();
                Session["SectionItems"] = lSectionItems;

                List<Int32> lItems = new List<Int32>();
                lItems.Add(1);
                lItems.Add(2);
                lItems.Add(3);
                rptrBTOItems.DataSource = lItems;
                rptrBTOItems.DataBind();
                Session["RepeaterDS"] = lItems;

                LoadDropdowns();

                if (Request.QueryString["_"] != null) // sectionID
                {
                    LoadSection();
                    LoadSubSections();
                    ltrlAddSectionBreadcrumb.Text = "Update Section";
                }
                else
                    ltrlAddSectionBreadcrumb.Text = "Add Section";

                txtTitle.Focus();
            }
        }

        protected void btnAddSubHeading_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["_"] != null)
            {
                ResetBTOItems(false);
                txtSubHeading.Focus();
                ClientScript.RegisterStartupScript(this.Page.GetType(), "modal", "<script type='text/javascript'>$( document ).ready(function() { $('#subHeadingModal').modal('show'); });</script>", false);
            }
            else
            {
                ClientScript.RegisterStartupScript(this.Page.GetType(), "modal", "<script type='text/javascript'>$( document ).ready(function() { $.toaster({ message : 'Please save section before adding sub heading.', priority : 'danger' }); });</script>", false);
            }
        }

        protected void lbtnSections_Click(object sender, EventArgs e)
        {
            Response.Redirect("Sections.aspx?_=" + Request.QueryString["__"]);
        }

        protected void rptrBTOItems_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                try
                {
                    //HtmlInputText txt = (HtmlInputText)e.Item.FindControl("txtTest");
                    //HtmlSelect drp = (HtmlSelect)e.Item.FindControl("drp");
                    //((TextBox)e.Item.FindControl("txtHidden")).Visible = false;
                    
                    LoadBTODropdowns((DropDownList)e.Item.FindControl("ddlBTOItem"), (DropDownList)e.Item.FindControl("drpRecommendations"), (DropDownList)e.Item.FindControl("drpWholesale"));
                    ((DropDownList)e.Item.FindControl("ddlBTOItem")).SelectedIndexChanged += ldrpBTOItems_SelectedIndexChanged;
                }
                catch (Exception ex) { string str = ex.Message; }
            }
        }

        protected void ldrpBTOItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            BTOItem lBTOItem = lContext.BTOItems.SingleOrDefault(x => x.BTOItemID == Convert.ToInt32(((DropDownList)sender).SelectedItem.Value));

            if (lBTOItem != null && lBTOItem.BTOItemID > 0)
            {
                string lBTOProdID = lBTOItem.ProductID, lBTOPartNumber = lBTOItem.PartNo;
                RepeaterItem lRptrItem = (RepeaterItem)(((DropDownList)sender).NamingContainer);

                if (lRptrItem != null)
                {
                    LoadWholesaleDropdown(lBTOPartNumber, lBTOProdID, (DropDownList)(lRptrItem.FindControl("drpWholesale")));
                }
            }

            ClientScript.RegisterStartupScript(this.Page.GetType(), "modal", "<script type='text/javascript'>$( document ).ready(function() { modalCustom('BTOModal'); });</script>", false);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

            long lSectionID = SaveSection();

            if (lSectionID > 0)
                Response.Redirect("addsection.aspx?__=" + Request.QueryString["__"] + "&_=" + lSectionID.ToString());
            else
                ClientScript.RegisterStartupScript(this.Page.GetType(), "modal", "<script type='text/javascript'>$( document ).ready(function() { $.toaster({ message : 'There was some error.', priority : 'danger' }); });</script>", false);
        }

        protected void btnSaveAndNext_Click(object sender, EventArgs e)
        {
            long lSectionID = SaveSection();

            if (lSectionID > 0)
                Response.Redirect("Sections.aspx?_=" + Request.QueryString["__"]);
            else
                ClientScript.RegisterStartupScript(this.Page.GetType(), "modal", "<script type='text/javascript'>$( document ).ready(function() { $.toaster({ message : 'There was some error.', priority : 'danger' }); });</script>", false);
        }

        protected void btnCancelSection_Click(object sender, EventArgs e)
        {
            Response.Redirect("sections.aspx?_=" + Request.QueryString["__"]);
        }

        protected void rptrSubSections_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "additem")
            {
                SubSection lSubSection = lContext.SubSections.SingleOrDefault(p => p.SubSectionID == Convert.ToInt32(e.CommandArgument));

                if (lSubSection != null)
                {
                    ResetBTOItems(false);
                    hdnSubSectionID.Value = lSubSection.SubSectionID.ToString();
                    hdnEditSectionItemID.Value = "0";
                    ClientScript.RegisterStartupScript(this.Page.GetType(), "modal", "<script type='text/javascript'>$( document ).ready(function() { modalCustom('BTOModal'); });</script>", false);
                }
            }
            else if (e.CommandName == "delsub")
            {
                SubSection lSubSection = lContext.SubSections.SingleOrDefault(p => p.SubSectionID == int.Parse(e.CommandArgument.ToString()));

                if (lSubSection != null)
                {
                    lContext.SubSections.DeleteOnSubmit(lSubSection);
                    lContext.SubmitChanges();

                    // delete all section items
                    var lSectionItems = from p in lContext.SectionItems
                                        where p.SubSectionID == lSubSection.SubSectionID
                                        select new { p.SectionItemID };

                    foreach (var lSectionItem in lSectionItems)
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
                    LoadSubSections();

                    ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$.toaster({ message : 'Sub Heading deleted.', priority : 'success' });", true);
                }
                else
                {
                    ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$.toaster({ message : 'Deletion failed.', priority : 'danger' });", true);
                }
            }
        }

        protected void rptrSubSections_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HiddenField hdnSubSectionID = (HiddenField)(e.Item.FindControl("hdnSubSectionID"));
                Repeater rptrSectionItems = (Repeater)(e.Item.FindControl("rptrSectionItems"));

                var lSectionItems = from p in lContext.SectionItems
                                   where p.SubSectionID == Convert.ToInt32(hdnSubSectionID.Value)
                                   orderby p.DisplayOrder
                                   select new
                                   {
                                       p.SectionItemID,
                                       p.SubSectionID,                                       
                                       p.CreatedOn,
                                       p.CreatedBy,
                                       p.ModifiedOn,
                                       p.ModifiedBy,
                                       p.DisplayOrder,
                                       ProductTitle = lContext.BTOItems.SingleOrDefault(q => q.BTOItemID == p.BTOItemID).ProductTitle, //(from q in lContext.BTOItems where q.BTOItemID == p.BTOItemID select q.ProductTitle).FirstOrDefault(),
                                       WholesaleTitle = lContext.Wholesales.SingleOrDefault(q => q.WholesaleID == p.WholesaleID).Discount.ToString() + " " + lContext.Wholesales.SingleOrDefault(q => q.WholesaleID == p.WholesaleID).DiscountType //(from r in lContext.Wholesales where r.WholesaleID == p.WholesaleID select r.Title)
                                   };
                rptrSectionItems.DataSource = lSectionItems;
                rptrSectionItems.DataBind();
            }

        }
        
        protected void btnSaveSubSection_Click1(object sender, EventArgs e)
        {
            if (Request.QueryString["_"] != null)
            {
                SubSection lSubSection = new SubSection();
                lSubSection.Title = txtSubHeading.Text.Trim();
                lSubSection.SectionID = Convert.ToInt32(Request.QueryString["_"]);
                long lDisplayOrder = 1;

                try
                {
                    lDisplayOrder = lContext.SubSections.OrderByDescending(u => u.DisplayOrder).FirstOrDefault().DisplayOrder;
                }
                catch
                { }

                lSubSection.DisplayOrder = lDisplayOrder;
                lSubSection.CreatedOn = DateTime.Now;
                lSubSection.CreatedBy = ((AdminUser)Session["AdminUser"]).UserID;
                lSubSection.IsMaster = Convert.ToBoolean(Session["IsMaster"]);
                lContext.SubSections.InsertOnSubmit(lSubSection);
                lContext.SubmitChanges();
                LoadSubSections();
                ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$.toaster({ message : 'Sub Heading added.', priority : 'success' });", true);
            }
            else
                ClientScript.RegisterStartupScript(this.Page.GetType(), "modal", "<script type='text/javascript'>$( document ).ready(function() { $.toaster({ message : 'Please save section before adding items.', priority : 'danger' }); });</script>", false);
        }

        protected void btnSaveItem_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["_"] != null)
            {

                List<SectionItemWithOptions> listSectionItemWithOptions = new List<SectionItemWithOptions>();
                
                List<SectionItem> lSectionItems = new List<SectionItem>();
                List<SectionItemOption> SectionItemOptions = new List<SectionItemOption>();
                long lDisplayOrder = 1;
                long lSubSectionID = 0;

                // loop thorugh items
                foreach (RepeaterItem lBTOItem in rptrBTOItems.Items)
                {
                    AddSectionItem(
                        (DropDownList)lBTOItem.FindControl("ddlBTOItem"), 
                        (RadioButton)lBTOItem.FindControl("rbtnDisplayModeCheck"), 
                        (RadioButton)lBTOItem.FindControl("rbtnIsDefault"), 
                        (CheckBox)lBTOItem.FindControl("chkIsQuantity"), 
                        (CheckBox)lBTOItem.FindControl("chkIsQuantityRequired"), 
                        (TextBox)lBTOItem.FindControl("txtProcessingTime"), 
                        (TextBox)lBTOItem.FindControl("txtQuantityIncrement"), 
                        (DropDownList)lBTOItem.FindControl("drpRecommendations"),
                        (TextBox)lBTOItem.FindControl("BTOItemSelectedWholesaleID"),
                        (TextBox)lBTOItem.FindControl("txtBTOItemOptionsSelectedOptions"), 
                        (HiddenField)lBTOItem.FindControl("hdnItemID"), 
                        (TextBox)lBTOItem.FindControl("txtHidden"), 
                        (HtmlTable)lBTOItem.FindControl("tblFilters"),
                        ref listSectionItemWithOptions);
                }

                if (hdnSubSectionID.Value != "" && hdnSubSectionID.Value != "0")
                    lSubSectionID = Convert.ToInt32(hdnSubSectionID.Value);
                else // add new subsection
                {
                    SubSection lSubSection = new SubSection();
                    lSubSection.Title = "";
                    lSubSection.SectionID = Convert.ToInt32(Request.QueryString["_"]);
                    lDisplayOrder = 1;

                    try
                    {
                        lDisplayOrder = lContext.SubSections.OrderByDescending(u => u.DisplayOrder).FirstOrDefault().DisplayOrder;
                    }
                    catch
                    { }

                    lSubSection.DisplayOrder = lDisplayOrder;
                    lSubSection.CreatedOn = DateTime.Now;
                    lSubSection.CreatedBy = ((AdminUser)Session["AdminUser"]).UserID;
                    lSubSection.IsMaster = Convert.ToBoolean(Session["IsMaster"]);
                    lContext.SubSections.InsertOnSubmit(lSubSection);
                    lContext.SubmitChanges();
                    lSubSectionID = lSubSection.SubSectionID;
                }

                // save into DB
                foreach (SectionItemWithOptions lSectionItemWithOptions in listSectionItemWithOptions)
                {
                    if (lSectionItemWithOptions.SectionItem.SectionItemID > 0) // update existing
                    {
                        SectionItem lSectionItemForUpdate = lContext.SectionItems.SingleOrDefault(p => p.SectionItemID == lSectionItemWithOptions.SectionItem.SectionItemID);

                        if (lSectionItemForUpdate != null)
                        {
                            lSectionItemForUpdate.BTOItemID = lSectionItemWithOptions.SectionItem.BTOItemID;
                            lSectionItemForUpdate.DisplayMode = lSectionItemWithOptions.SectionItem.DisplayMode;
                            lSectionItemForUpdate.DisplayOrder = lSectionItemWithOptions.SectionItem.DisplayOrder;
                            lSectionItemForUpdate.IsDefault = lSectionItemWithOptions.SectionItem.IsDefault;
                            lSectionItemForUpdate.IsQuantity = lSectionItemWithOptions.SectionItem.IsQuantity;
                            lSectionItemForUpdate.IsQuantityRequired = lSectionItemWithOptions.SectionItem.IsQuantityRequired;
                            lSectionItemForUpdate.ModifiedBy = ((AdminUser)Session["AdminUser"]).UserID;
                            lSectionItemForUpdate.ModifiedOn = DateTime.Now;
                            lSectionItemForUpdate.ProcessingTime = lSectionItemWithOptions.SectionItem.ProcessingTime;
                            lSectionItemForUpdate.QuantityIncrement = lSectionItemWithOptions.SectionItem.QuantityIncrement;
                            lSectionItemForUpdate.RecommendationID = lSectionItemWithOptions.SectionItem.RecommendationID;
                            lSectionItemForUpdate.WholesaleID = lSectionItemWithOptions.SectionItem.WholesaleID;
                            lSectionItemForUpdate.IsMaster = Convert.ToBoolean(Session["IsMaster"]);
                            lContext.SubmitChanges();
                            SaveTags(lSectionItemWithOptions.SectionItem.TagText, lSectionItemForUpdate.SectionItemID);
                            SaveItemFilters(lSectionItemWithOptions.SectionItem.ItemFilters, lSectionItemForUpdate.SectionItemID);
                            SaveItemOptions(FetchItemOptions(txtBTOItemOptionsSelectedOptionsEdit), lSectionItemForUpdate.SectionItemID, "Edit");
                        }
                    }
                    else // insert new
                    {
                        lSectionItemWithOptions.SectionItem.SubSectionID = lSubSectionID;
                        lSectionItemWithOptions.SectionItem.IsMaster = Convert.ToBoolean(Session["IsMaster"]);
                        lContext.SectionItems.InsertOnSubmit(lSectionItemWithOptions.SectionItem);
                        lContext.SubmitChanges();
                        SaveTags(lSectionItemWithOptions.SectionItem.TagText, lSectionItemWithOptions.SectionItem.SectionItemID);
                        SaveItemFilters(lSectionItemWithOptions.SectionItem.ItemFilters, lSectionItemWithOptions.SectionItem.SectionItemID);
                        SaveItemOptions(lSectionItemWithOptions.SectionItemOptions, lSectionItemWithOptions.SectionItem.SectionItemID, "Add");
                    }                    
                }

                lSectionItems = new List<SectionItem>();
                Session["SectionItems"] = lSectionItems;
                LoadSubSections();
                ClientScript.RegisterStartupScript(this.Page.GetType(), "modal", "<script type='text/javascript'>$( document ).ready(function() { $.toaster({ message : 'Items saved.', priority : 'success' }); });</script>", false);
            }
            else
                ClientScript.RegisterStartupScript(this.Page.GetType(), "modal", "<script type='text/javascript'>$( document ).ready(function() { $.toaster({ message : 'Please save section before adding items.', priority : 'danger' }); });</script>", false);
        }

        private void rptrSectionItems_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "edit")
            {
                SectionItem lSectionItemForUpdate = lContext.SectionItems.SingleOrDefault(p => p.SectionItemID == Convert.ToInt32(e.CommandArgument));

                if (lSectionItemForUpdate != null)
                {
                    chkEditIsDefault.Checked = lSectionItemForUpdate.IsDefault;
                    drpEditBTOItems.ClearSelection();
                    drpEditWholesales.ClearSelection();
                    drpEditRecommendations.ClearSelection();

                    ListItem liBTO = drpEditBTOItems.Items.FindByValue(lSectionItemForUpdate.BTOItemID.ToString());

                    if (liBTO != null)
                        liBTO.Selected = true;

                    BTOItem lBTOItem = lContext.BTOItems.SingleOrDefault(x => x.BTOItemID == Convert.ToInt32(drpEditBTOItems.SelectedItem.Value));

                    if (lBTOItem != null && lBTOItem.BTOItemID > 0)
                    {
                        string lBTOProdID = lBTOItem.ProductID, lBTOPartNumber = lBTOItem.PartNo;
                        LoadWholesaleDropdown(lBTOPartNumber, lBTOProdID, drpEditWholesales);
                        //ClientScript.RegisterStartupScript(this.Page.GetType(), "modal", "<script type='text/javascript'>$( document ).ready(function() { modalCustom('BTOModalEdit'); });</script>", false);
                    }

                    ListItem liWhole = drpEditWholesales.Items.FindByValue(lSectionItemForUpdate.WholesaleID.ToString());

                    if (liWhole != null)
                        drpEditWholesales.ClearSelection();
                        liWhole.Selected = true;

                    if (drpEditWholesales.SelectedIndex != 0)
                    {
                        try
                        {
                            var webPrice = Convert.ToDecimal(lBTOItem.WebPrice);
                            var Wholesale = Convert.ToDecimal(drpEditWholesales.SelectedItem.Text.Split(' ')[0].Replace("%", ""));
                            var percentwholeSale = Wholesale / 100 * webPrice;
                            var discountPrice = webPrice - percentwholeSale;
                            var RoundedWholeSale = Math.Round(discountPrice, 2);

                            txtEditPrice.Text = RoundedWholeSale.ToString();

                            
                            //txtEditPrice.Text = Math.Round(
                            //    Convert.ToDecimal(lBTOItem.WebPrice) - (
                            //    (Convert.ToDecimal(drpEditWholesales.SelectedItem.Text.Split(' ')[0].Replace("%", "")) / 100) 
                            //    * Convert.ToDecimal(lBTOItem.WebPrice)), 2).ToString();
                        }
                        catch { }
                    }
                    // handle display mode edit 
                    if (lSectionItemForUpdate.DisplayMode == "Checkbox")
                    {
                        rbtnEditDisplayModeCheckbox.Checked = true;
                        rbtnEditDisplayModeRadio.Checked = false;
                    }
                    else if (lSectionItemForUpdate.DisplayMode == "Radio")
                    {
                        rbtnEditDisplayModeCheckbox.Checked = false;
                        rbtnEditDisplayModeRadio.Checked = true;
                    }

                    // start SectionItemOption Edit Handle
                    lstbxBTOItemOptionsEdit.Items.Clear();
                    var BTOItemOptions = lContext.BTOItems
                                            .Where(btoItem => btoItem.ProductID == lBTOItem.ProductID)
                                            .Select(item => new
                                            {
                                                Text = item.PrimaryOptionValue,
                                                Value = item.BTOItemID,
                                                Selected = lContext.SectionItemOptions.Any(sio => sio.BTOItemID == item.BTOItemID && sio.SectionItemID == lSectionItemForUpdate.SectionItemID)
                                            });
                    if (BTOItemOptions.Count() > 1)
                    {
                        ltrlBTOItemOptionTitleEdit.Text = lBTOItem.PrimaryOptionTitle;
                        lstbxBTOItemOptionsEdit.Items.Clear();
                        string hdntxt = string.Empty;
                        bool flagFirstTime = true;
                        foreach (var item in BTOItemOptions)
                        {
                            var listItem = new ListItem();
                            listItem.Text = item.Text;
                            listItem.Value = item.Value.ToString();
                            listItem.Selected = item.Selected;
                            lstbxBTOItemOptionsEdit.Items.Add(listItem);
                            if (item.Selected)
                            {
                                if (flagFirstTime)
                                {
                                    hdntxt += item.Value;
                                    flagFirstTime = false;
                                }
                                else hdntxt += "," + item.Value;
                            }
                        }
                        //lstbxBTOItemOptionsEdit.Items.Add(new ListItem("All", "100"));
                        txtBTOItemOptionsSelectedOptionsEdit.Text = hdntxt;
                    }
                    // end SectionItemOption Edit Handle

                    ListItem liRec = drpEditRecommendations.Items.FindByValue(lSectionItemForUpdate.RecommendationID.ToString());

                    if (liRec != null)
                        liRec.Selected = true;

                    chkEditIsQuantity.Checked = lSectionItemForUpdate.IsQuantity;
                    chkEditIsQuantityRequired.Checked = lSectionItemForUpdate.IsQuantityRequired;
                    txtEditProcessingTime.Text = lSectionItemForUpdate.ProcessingTime.ToString();
                    txtEditQuantityIncrement.Text = lSectionItemForUpdate.QuantityIncrement.ToString();
                    hdnEditSectionItemID.Value = lSectionItemForUpdate.SectionItemID.ToString();

                    // load tags
                    var lItemTags = from p in lContext.ItemTags
                                    where p.SectionItemID == Convert.ToInt32(e.CommandArgument)
                                    select new
                                    {
                                        p.CreatedOn,
                                        p.ItemTagID,
                                        p.SectionItemID,
                                        p.TagID,
                                        TagText = lContext.Tags.FirstOrDefault(x => x.TagID == p.TagID).TagText
                                    };

                    string lTagText = "['"; // ['tag 1', 'tag 2']

                    foreach (var lTag in lItemTags)
                    {
                        if (lTagText.Trim() != "['")
                            lTagText += "', '";

                        lTagText += lTag.TagText;
                    }

                    lTagText += "']";

                    // load item filters
                    var lItemFilters = from p in lContext.ItemFilters
                                       where p.SectionItemID == Convert.ToInt32(e.CommandArgument)
                                       select new
                                       {
                                           p.FilterType,
                                           p.ItemFilterID,
                                           p.SectionName,
                                           p.Tag,
                                           p.CreatedOn,
                                           p.CreatedBy,

                                       };

                    HtmlTableRowCollection lRows = tblEditFilters.Rows;
                    //lRows.RemoveAt(0);
                    //lRows.RemoveAt(lRows.Count - 1);

                    int i = 1;
                    if (lItemFilters.Count() > 0)
                    {
                        foreach (var lItemFilter in lItemFilters)
                        {
                            ((TextBox)lRows[i].FindControl("txtFilterRowNo" + (i).ToString())).Text = "visible";
                            lRows[i].Attributes.CssStyle["display"] = "block";

                            try
                            {
                                ((HtmlInputText)lRows[i].FindControl("txtSectionName" + (i).ToString())).Value = lItemFilter.SectionName;
                                ((HiddenField)lRows[i].FindControl("hdnItemFilterID" + (i).ToString())).Value = lItemFilter.ItemFilterID.ToString();
                            }
                            catch { }

                            try
                            {
                                HtmlSelect ldrpTags = (HtmlSelect)lRows[i].FindControl("drpTags" + (i).ToString());

                                if (ldrpTags != null)
                                {
                                    ldrpTags.Items.Clear();
                                    foreach (var lTag in lItemTags)
                                    {
                                        ldrpTags.Items.Add(lTag.TagText);
                                    }

                                    ldrpTags.Items.FindByText(lItemFilter.Tag).Selected = true;
                                    ((HiddenField)lRows[i].FindControl("hdnSelectedTag" + (i))).Value = lItemFilter.Tag;
                                }
                            }
                            catch { }

                            HtmlSelect ldrpFilterType = (HtmlSelect)lRows[i].FindControl("drpFilterTypes" + (i).ToString());


                            for(int j=0; j<ldrpFilterType.Items.Count;j++){
                                ldrpFilterType.Items[j].Selected = false;
                            }
                            ldrpFilterType.Items.FindByText(lItemFilter.FilterType).Selected = true;
                            i += 1;
                        }
                    }
                    for (int j = i; j <= 10; j++)
                    {
                        if (j <= 3)
                        {
                            lRows[j].Attributes.CssStyle["display"] = "block";
                            ((TextBox)lRows[j].FindControl("txtFilterRowNo" + (j).ToString())).Text = "visible";
                        }
                        else
                        {
                            lRows[j].Attributes.CssStyle["display"] = "none";
                            ((TextBox)lRows[j].FindControl("txtFilterRowNo" + (j).ToString())).Text = "hidden";
                        }
                        ((HtmlInputText)lRows[j].FindControl("txtSectionName" + (j).ToString())).Value = "";
                        ((HiddenField)lRows[j].FindControl("hdnItemFilterID" + (j).ToString())).Value = "";
                        ((HtmlSelect)lRows[j].FindControl("drpTags" + (j).ToString())).Items.Clear();
                        HtmlSelect ldrpFilterType = (HtmlSelect)lRows[j].FindControl("drpFilterTypes" + (j).ToString());
                        for (int k = 0; k < ldrpFilterType.Items.Count; k++)
                        {
                            ldrpFilterType.Items[k].Selected = false;
                        }
                        ldrpFilterType.Items.FindByText("Contains Disable").Selected = true;

                    }

                    ClientScript.RegisterStartupScript(this.Page.GetType(), "modal", "<script type='text/javascript'>$( document ).ready(function() { $('#tagBoxBTOEdit').tagging( 'add', " + lTagText + " ); modalCustom('BTOModalEdit'); });</script>", false);
                }
            }
            else if (e.CommandName == "del")
            {
                SectionItem lSectionItem = lContext.SectionItems.SingleOrDefault(p => p.SectionItemID == int.Parse(e.CommandArgument.ToString()));

                if (lSectionItem != null)
                {
                    lContext.SectionItems.DeleteOnSubmit(lSectionItem);
                    lContext.SubmitChanges();

                    long lSectionItemIDToDelete = lSectionItem.SectionItemID;
                    
                    // delete all item filters
                    lContext.ExecuteCommand("DELETE FROM ItemFilters WHERE SectionItemID = " + lSectionItemIDToDelete.ToString());
                    lContext.SubmitChanges();

                    // delete all item tags
                    lContext.ExecuteCommand("DELETE FROM ItemTags WHERE SectionItemID = " + lSectionItemIDToDelete.ToString());
                    lContext.SubmitChanges();

                    // delete all sectionitemOptions
                    lContext.SectionItemOptions.DeleteAllOnSubmit(lContext.SectionItemOptions.Where(o => o.SectionItemID == lSectionItem.SectionItemID));
                    lContext.SubmitChanges();

                    LoadSubSections();

                    ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$.toaster({ message : 'Item deleted.', priority : 'success' });", true);
                }
                else
                {
                    ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$.toaster({ message : 'Deletion failed.', priority : 'danger' });", true);
                }
            }
        }

        protected void btnAddNewItem_Click(object sender, EventArgs e)
        {
            List<Int32> lItems = (List<Int32>)Session["RepeaterDS"];

            if (lItems == null)
                lItems = new List<int>();

            lItems.Add(rptrBTOItems.Items.Count + 1);
            rptrBTOItems.DataSource = lItems;
            rptrBTOItems.DataBind();
            Session["RepeaterDS"] = lItems;
            ClientScript.RegisterStartupScript(this.Page.GetType(), "modal", "<script type='text/javascript'>$( document ).ready(function() { modalCustom('BTOModal'); });</script>", false);
        }

        protected void btnAddItemFromMain_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["_"] != null)
            {
                ResetBTOItems(false);
                ClientScript.RegisterStartupScript(this.Page.GetType(), "modal", "<script type='text/javascript'>$( document ).ready(function() { modalCustom('BTOModal'); });</script>", false);
            }
            else
            {
                ClientScript.RegisterStartupScript(this.Page.GetType(), "modal", "<script type='text/javascript'>$( document ).ready(function() { $.toaster({ message : 'Please save section before adding items.', priority : 'danger' }); });</script>", false);
            }
        }

        protected void rptrSubSections_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                ((Repeater)e.Item.FindControl("rptrSectionItems")).ItemCommand += new RepeaterCommandEventHandler(rptrSectionItems_ItemCommand);
            }
        }

        protected void btnBTOItemEdit_Click(object sender, EventArgs e)
        {
            string tags = hdnTags.Value;

            if (hdnEditSectionItemID.Value != "" && hdnEditSectionItemID.Value != "0") // update
            {
                SectionItem lSectionItemForUpdate = lContext.SectionItems.SingleOrDefault(p => p.SectionItemID == Convert.ToInt32(hdnEditSectionItemID.Value));

                if (lSectionItemForUpdate != null)
                {
                    lSectionItemForUpdate.BTOItemID = Convert.ToInt32(drpEditBTOItems.SelectedItem.Value);

                    if (rbtnEditDisplayModeCheckbox.Checked)
                        lSectionItemForUpdate.DisplayMode = "Checkbox";
                    else
                        lSectionItemForUpdate.DisplayMode = "Radio";

                    lSectionItemForUpdate.IsDefault = chkEditIsDefault.Checked;
                    lSectionItemForUpdate.IsQuantity = chkEditIsQuantity.Checked;
                    lSectionItemForUpdate.IsQuantityRequired = chkEditIsQuantityRequired.Checked;
                    lSectionItemForUpdate.ModifiedBy = ((AdminUser)Session["AdminUser"]).UserID;
                    lSectionItemForUpdate.ModifiedOn = DateTime.Now;
                    lSectionItemForUpdate.ProcessingTime = ((txtEditProcessingTime.Text.Trim() == "") ? 0 : Convert.ToInt32(txtEditProcessingTime.Text.Trim()));
                    lSectionItemForUpdate.QuantityIncrement = ((txtEditQuantityIncrement.Text.Trim() == "") ? 0 : Convert.ToInt32(txtEditQuantityIncrement.Text.Trim()));
                    lSectionItemForUpdate.RecommendationID = Convert.ToInt32(drpEditRecommendations.SelectedItem.Value);
                    lSectionItemForUpdate.WholesaleID = Convert.ToInt32(drpEditWholesales.SelectedItem.Value);
                    lSectionItemForUpdate.IsMaster = Convert.ToBoolean(Session["IsMaster"]);
                    lContext.SubmitChanges();

                    SaveItemOptions(FetchItemOptions(txtBTOItemOptionsSelectedOptionsEdit), lSectionItemForUpdate.SectionItemID, "Edit");

                    SaveTags(hdnTags.Value.Trim(), lSectionItemForUpdate.SectionItemID);
                    var Itemfilters = FetchItemFilters(tblEditFilters);
                
                    SaveItemFilters(Itemfilters, lSectionItemForUpdate.SectionItemID);
                    LoadSubSections();
                }
            }
            else if (hdnSubSectionID.Value != "" && hdnSubSectionID.Value != "0") // add new
            {
                SectionItem lSectionItem = new SectionItem();
                lSectionItem.SubSectionID = Convert.ToInt32(hdnSubSectionID.Value.Trim());
                lSectionItem.BTOItemID = Convert.ToInt32(drpEditBTOItems.SelectedItem.Value);
                lSectionItem.IsDefault = chkEditIsDefault.Checked;
                lSectionItem.IsQuantity = chkEditIsQuantity.Checked;
                lSectionItem.IsQuantityRequired = chkEditIsQuantityRequired.Checked;
                lSectionItem.CreatedBy = ((AdminUser)Session["AdminUser"]).UserID;
                lSectionItem.CreatedOn = DateTime.Now;
                lSectionItem.ProcessingTime = ((txtEditProcessingTime.Text.Trim() == "") ? 0 : Convert.ToInt32(txtEditProcessingTime.Text.Trim()));
                lSectionItem.QuantityIncrement = ((txtEditQuantityIncrement.Text.Trim() == "") ? 0 : Convert.ToInt32(txtEditQuantityIncrement.Text.Trim()));
                lSectionItem.RecommendationID = Convert.ToInt32(drpEditRecommendations.SelectedItem.Value);
                lSectionItem.WholesaleID = Convert.ToInt32(drpEditWholesales.SelectedItem.Value);
                lSectionItem.IsMaster = Convert.ToBoolean(Session["IsMaster"]);

                if (rbtnEditDisplayModeCheckbox.Checked)
                    lSectionItem.DisplayMode = "Checkbox";
                else
                    lSectionItem.DisplayMode = "Radio";

                long lDisplayOrder = 1;

                try
                {
                    lDisplayOrder = lContext.SectionItems.OrderByDescending(u => u.DisplayOrder).FirstOrDefault().DisplayOrder;
                }
                catch
                { }
                
                lSectionItem.DisplayOrder = lDisplayOrder;
                lContext.SectionItems.InsertOnSubmit(lSectionItem);
                lContext.SubmitChanges();

                SaveItemOptions(FetchItemOptions(txtBTOItemOptionsSelectedOptionsEdit), lSectionItem.SectionItemID, "Add");

                SaveTags(hdnTags.Value.Trim(), lSectionItem.SectionItemID);
                SaveItemFilters(FetchItemFilters(tblEditFilters), lSectionItem.SectionItemID);
                LoadSubSections();
            }
        }

        protected void drpEditBTOItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            BTOItem lBTOItem = lContext.BTOItems.SingleOrDefault(x => x.BTOItemID == Convert.ToInt32(drpEditBTOItems.SelectedItem.Value));

            if (lBTOItem != null && lBTOItem.BTOItemID > 0)
            {
                string lBTOProdID = lBTOItem.ProductID, lBTOPartNumber = lBTOItem.PartNo;
                LoadWholesaleDropdown(lBTOPartNumber, lBTOProdID, drpEditWholesales);
                ClientScript.RegisterStartupScript(this.Page.GetType(), "modal", "<script type='text/javascript'>$( document ).ready(function() { modalCustom('BTOModalEdit'); });</script>", false);
            }
        }

        #endregion

        #region Functions

        private void SaveItemOptions(List<SectionItemOption> pOptions, long pSectionItemID, string CommandType = "Add")
        {
            
            if (CommandType == "Edit")
            {
                // delete the previous ones
                var PreviousOptions = lContext.SectionItemOptions.Where(o => o.SectionItemID == pSectionItemID);
                if (PreviousOptions.Count() > 0)
                {
                    lContext.SectionItemOptions.DeleteAllOnSubmit(PreviousOptions);
                }
            }
            
            // Insert The new Ones
            foreach (SectionItemOption option in pOptions)
            {
                option.SectionItemID = pSectionItemID;
                lContext.SectionItemOptions.InsertOnSubmit(option);
            }
            lContext.SubmitChanges();
        }

        private void SaveItemFilters(List<ItemFilter> pItemFilters, long pSectionItemID)
        {
            lContext.ItemFilters.DeleteAllOnSubmit(lContext.ItemFilters.Where(i => i.SectionItemID == pSectionItemID));

            foreach (ItemFilter lItemFilter in pItemFilters)
            {
                lItemFilter.SectionItemID = pSectionItemID;
                lContext.ItemFilters.InsertOnSubmit(lItemFilter);
            }
            lContext.SubmitChanges();
        }

        private void SaveTags(string pTags, long pSectionItemID)
        {
            if (pTags.Trim() != string.Empty)
            {
                string[] lTagArray = pTags.Split(',');

                if (lTagArray.Length > 0)
                {
                    foreach (string lTagText in lTagArray)
                    {
                        Tag lTag = lContext.Tags.SingleOrDefault(p => p.TagText == lTagText.Trim().ToLower());

                        if (lTag != null && lTag.TagID > 0)
                        {
                            ItemTag lEsixtingItemTag = lContext.ItemTags.SingleOrDefault(p => p.TagID == lTag.TagID && p.SectionItemID == pSectionItemID);

                            if (lEsixtingItemTag == null || lEsixtingItemTag.ItemTagID <= 0)
                            {
                                ItemTag lItemTag = new ItemTag();
                                lItemTag.TagID = lTag.TagID;
                                lItemTag.SectionItemID = pSectionItemID;
                                lItemTag.CreatedOn = DateTime.Now;
                                lContext.ItemTags.InsertOnSubmit(lItemTag);
                                lContext.SubmitChanges();
                            }
                        }
                        else
                        {
                            lTag = new Tag();
                            lTag.TagText = lTagText.Trim().ToLower();
                            lTag.CreatedOn = DateTime.Now;
                            lContext.Tags.InsertOnSubmit(lTag);
                            lContext.SubmitChanges();

                            ItemTag lItemTag = new ItemTag();
                            lItemTag.TagID = lTag.TagID;
                            lItemTag.SectionItemID = pSectionItemID;
                            lItemTag.CreatedOn = DateTime.Now;
                            lContext.ItemTags.InsertOnSubmit(lItemTag);
                            lContext.SubmitChanges();
                        }
                    }
                }
            }
        }
        
        private void AddSectionItem(
            DropDownList pddlBTOItem, 
            RadioButton prbtnDisplayModeCheck, 
            RadioButton prbtnIsDefault, 
            CheckBox pchkIsQuantity, 
            CheckBox pchkIsQuantityRequired, 
            TextBox ptxtProcessingTime, 
            TextBox ptxtQuantityIncrement, 
            DropDownList pdrpRecommendations,
            TextBox BTOItemSelectedWholesaleID,
            TextBox txtItemOptions, 
            HiddenField phdnSectionItemID, 
            TextBox ptxtTags, HtmlTable 
            ptblFilters, 
            ref List<SectionItemWithOptions> plistSectionItemsWithOptions)
        {
            if (pddlBTOItem.SelectedItem.Text != "Please select item")
            {
                SectionItem lSectionItem = new SectionItem();
                List<ItemFilter> lItemFilters = new List<ItemFilter>();
                long lDisplayOrder = 1;

                if (hdnSubSectionID.Value != "" && hdnSubSectionID.Value != "0")
                    lSectionItem.SubSectionID = Convert.ToInt32(hdnSubSectionID.Value);

                if (phdnSectionItemID.Value != "" && phdnSectionItemID.Value != "0")
                    lSectionItem.SectionItemID = Convert.ToInt32(phdnSectionItemID.Value);

                lSectionItem.BTOItemID = Convert.ToInt32(pddlBTOItem.SelectedItem.Value);
                lSectionItem.CreatedBy = ((AdminUser)Session["AdminUser"]).UserID;
                lSectionItem.CreatedOn = DateTime.Now;
                lSectionItem.TagText = ptxtTags.Text.Trim();

                if (prbtnDisplayModeCheck.Checked)
                    lSectionItem.DisplayMode = "Checkbox";
                else
                    lSectionItem.DisplayMode = "Radio";

                lDisplayOrder = 1;

                try
                {
                    lDisplayOrder = lContext.SectionItems.OrderByDescending(u => u.DisplayOrder).FirstOrDefault().DisplayOrder;
                }
                catch
                { }

                lSectionItem.DisplayOrder = lDisplayOrder;
                lSectionItem.IsDefault = prbtnIsDefault.Checked;
                lSectionItem.IsQuantity = pchkIsQuantity.Checked;
                lSectionItem.IsQuantityRequired = pchkIsQuantityRequired.Checked;
                lSectionItem.ProcessingTime = ((ptxtProcessingTime.Text.Trim() == "") ? 0 : Convert.ToInt32(ptxtProcessingTime.Text.Trim()));
                lSectionItem.QuantityIncrement = ((ptxtQuantityIncrement.Text.Trim() == "") ? 0 : Convert.ToInt32(ptxtQuantityIncrement.Text.Trim()));
                lSectionItem.RecommendationID = Convert.ToInt32(pdrpRecommendations.SelectedItem.Value);

                if (BTOItemSelectedWholesaleID != null && !string.IsNullOrWhiteSpace(BTOItemSelectedWholesaleID.Text))
                {
                    lSectionItem.WholesaleID = Convert.ToInt32(BTOItemSelectedWholesaleID.Text);
                }
            
                lSectionItem.ItemFilters = FetchItemFilters(ptblFilters);

                SectionItemWithOptions lobj = new SectionItemWithOptions();
                lobj.SectionItem = lSectionItem;
                lobj.SectionItemOptions = FetchItemOptions(txtItemOptions);
                plistSectionItemsWithOptions.Add(lobj);
            }
        }

        private List<SectionItemOption> FetchItemOptions(TextBox pOptionsAsPlainTextField)
        {
            var lOptions = new List<SectionItemOption>();

            if (pOptionsAsPlainTextField != null && !string.IsNullOrWhiteSpace(pOptionsAsPlainTextField.Text))
            {
                foreach (string index in pOptionsAsPlainTextField.Text.Split(','))
                {
                    SectionItemOption objSectionItemOption = new SectionItemOption();
                    objSectionItemOption.BTOItemID = Convert.ToInt64(index);
                    lOptions.Add(objSectionItemOption);
                }
            }
            return lOptions;
        }
        private List<ItemFilter> FetchItemFilters(HtmlTable ptblFilters)
        {
            List<ItemFilter> lItemFilters = new List<ItemFilter>();

            // get item filters
            if (ptblFilters != null) // min 3 rows => first label, last meta
            {
                try
                {
                    HtmlTableRowCollection lRows = ptblFilters.Rows;
                    lRows.RemoveAt(0);
                    lRows.RemoveAt(lRows.Count - 1);
                    int i = 1;

                    foreach (HtmlTableRow lRow in lRows)
                    {
                        if (((TextBox)lRows[i].FindControl("txtFilterRowNo" + (i).ToString())).Text == "visible")
                        {
                            ItemFilter lItemFilter = new ItemFilter();
                            try
                            {
                                HtmlInputText ltxtSectionName = (HtmlInputText)lRow.FindControl("txtSectionName" + i.ToString());
                                if (ltxtSectionName != null && ltxtSectionName.Value.Trim() != "")
                                {
                                    HiddenField hdnItemFilterID = (HiddenField)lRow.FindControl("hdnItemFilterID" + i.ToString());
                                    if (hdnItemFilterID != null && !string.IsNullOrWhiteSpace(hdnItemFilterID.Value) && hdnItemFilterID.Value != "0")
                                    {
                                        lItemFilter.ItemFilterID = Convert.ToInt64(hdnItemFilterID.Value);
                                    }
                                    lItemFilter.SectionName = ltxtSectionName.Value;
                                    HiddenField lhdnTag = (HiddenField)lRow.FindControl("hdnSelectedTag" + i.ToString());
                                    HtmlSelect ldrpFilterTypes = (HtmlSelect)lRow.FindControl("drpFilterTypes" + i.ToString());
                                    lItemFilter.FilterType = ldrpFilterTypes.Items[ldrpFilterTypes.SelectedIndex].Text;
                                    lItemFilter.Tag = lhdnTag.Value;
                                    lItemFilter.CreatedBy = ((AdminUser)Session["AdminUser"]).UserID;
                                    lItemFilter.CreatedOn = DateTime.Now;
                                    lItemFilters.Add(lItemFilter);
                                }
                            }
                            catch { }
                        }
                        i++;
                    }
                }
                catch (Exception ex) { string err = ex.Message; }
            }

            return lItemFilters;
        }

        private long SaveSection()
        {
            long lSectionID = 0;

            if (Request.QueryString["_"] == null) // add new section
            {
                Section lSection = new Section();
                lSection.IsRequired = chkIsRequired.Checked;
                lSection.PricingID = Convert.ToInt32(drpPricings.SelectedItem.Value);
                lSection.ProductID = Convert.ToInt32(Request.QueryString["__"]);
                lSection.ProductStepID = drpProductSteps.SelectedItem == null ? 0 : Convert.ToInt32(drpProductSteps.SelectedItem.Value);
                lSection.RenderModeID = Convert.ToInt32(drpRenderModes.SelectedItem.Value);
                lSection.Title = txtTitle.Text.Trim();
                lSection.CreatedBy = ((AdminUser)Session["AdminUser"]).UserID;
                lSection.CreatedOn = DateTime.Now;
                lSection.Description = txtaDescription.Text.Trim();
                lSection.IsMaster = Convert.ToBoolean(Session["IsMaster"]);

                int lDisplayOrder = 1;

                try
                {
                    lDisplayOrder = lContext.Products.OrderByDescending(u => u.DisplayOrder).FirstOrDefault().DisplayOrder;
                }
                catch
                { }

                lSection.DisplayOrder = lDisplayOrder + 1;
                lContext.Sections.InsertOnSubmit(lSection);
                lContext.SubmitChanges();
                lSectionID = lSection.SectionID;

                // save file
                try
                {
                    string lFileName = fuImage.PostedFile.FileName;

                    if (lFileName != null && lFileName != "")
                    {
                        //lSection = lContext.Sections.SingleOrDefault(p => p.SectionID == lSection.SectionID);

                        if (lSection != null && lSection.SectionID > 0)
                        {
                            if (!Directory.Exists(Server.MapPath("../files/sections")))
                            {
                                Directory.CreateDirectory(Server.MapPath("../files/sections"));
                            }

                            string guid = System.Guid.NewGuid().ToString();
                            fuImage.PostedFile.SaveAs(Server.MapPath("../files/sections/" + guid + ".jpg"));
                            lSection.ImageName = guid + ".jpg";
                            lContext.SubmitChanges();
                        }
                    }
                }
                catch { }
            }
            else // update
            {
                Section lSection = lContext.Sections.SingleOrDefault(p => p.SectionID == int.Parse(Request.QueryString["_"]));
                lSection.IsRequired = chkIsRequired.Checked;
                lSection.PricingID = Convert.ToInt32(drpPricings.SelectedItem.Value);
                lSection.ProductID = Convert.ToInt32(Request.QueryString["__"]);
                lSection.ProductStepID = drpProductSteps.SelectedItem == null ? 0 : Convert.ToInt32(drpProductSteps.SelectedItem.Value);
                lSection.RenderModeID = Convert.ToInt32(drpRenderModes.SelectedItem.Value);
                lSection.Title = txtTitle.Text.Trim();
                lSection.ModifiedBy = ((AdminUser)Session["AdminUser"]).UserID;
                lSection.ModifiedOn = DateTime.Now;
                lSection.Description = txtaDescription.Text.Trim();
                lSection.IsMaster = Convert.ToBoolean(Session["IsMaster"]);
                lContext.SubmitChanges();
                lSectionID = lSection.SectionID;
                
                // save file
                try
                {
                    string lFileName = fuImage.PostedFile.FileName;

                    if (lFileName != null && lFileName != "")
                    {
                        //lSection = lContext.Sections.SingleOrDefault(p => p.SectionID == lSection.SectionID);

                        if (lSection != null && lSection.SectionID > 0)
                        {
                            if (!Directory.Exists(Server.MapPath("../files/sections")))
                            {
                                Directory.CreateDirectory(Server.MapPath("../files/sections"));
                            }

                            string guid = System.Guid.NewGuid().ToString();
                            fuImage.PostedFile.SaveAs(Server.MapPath("../files/sections/" + guid + ".jpg"));
                            lSection.ImageName = guid + ".jpg";
                            lContext.SubmitChanges();
                        }
                    }
                }
                catch { }
            }

            return lSectionID;
        }

        private void LoadSection()
        {
            Section lSection = lContext.Sections.SingleOrDefault(p => p.SectionID == int.Parse(Request.QueryString["_"]));

            if (lSection != null)
            {
                txtTitle.Text = lSection.Title;
                txtaDescription.Text = lSection.Description;

                ListItem liPricing = drpPricings.Items.FindByValue(lSection.PricingID.ToString());

                if (liPricing != null)
                    liPricing.Selected = true;

                ListItem liRenderMode = drpRenderModes.Items.FindByValue(lSection.RenderModeID.ToString());

                if (liRenderMode != null)
                    liRenderMode.Selected = true;

                ListItem liProductSteps = drpProductSteps.Items.FindByValue(lSection.ProductStepID.ToString());

                if (liProductSteps != null)
                    liProductSteps.Selected = true;

                chkIsRequired.Checked = lSection.IsRequired;
                imgSection.ImageUrl = "../files/sections/" + lSection.ImageName;
            }
        }

        private void LoadDropdowns()
        {
            var lPricings = from n in lContext.Pricings select n;
            drpPricings.DataSource = lPricings;
            drpPricings.DataTextField = "Title";
            drpPricings.DataValueField = "PricingID";
            drpPricings.DataBind();

            var lRenderModes = from o in lContext.RenderModes select o;
            drpRenderModes.DataSource = lRenderModes;
            drpRenderModes.DataTextField = "Title";
            drpRenderModes.DataValueField = "RenderModeID";
            drpRenderModes.DataBind();

            var lProductSteps = from p in lContext.ProductSteps
                                where p.ProductID == Convert.ToInt32(Request.QueryString["__"])
                                orderby p.DisplayOrder
                                select new { p.ProductStepID, DisplayTitle = "Step " + p.DisplayOrder + " " + p.Title };
            drpProductSteps.DataSource = lProductSteps;
            drpProductSteps.DataTextField = "DisplayTitle";
            drpProductSteps.DataValueField = "ProductStepID";
            drpProductSteps.DataBind();

            lBTOItems = lContext.BTOItems.ToList();
            lRecommendations = from o in lContext.Recommendations select o;
            lWholesales = from o in lContext.Wholesales select o;

            lBTOItems = (lBTOItems.ToList()).DistinctBy(i => i.ProductID);
            foreach (var lBTOItem in lBTOItems)
            {
                lBTOItem.Description = lBTOItem.ProductTitle + " (Price: " + lBTOItem.WebPrice + ")";
            }
            
            foreach(var lWholesale in lWholesales)
            {
                lWholesale.Title = lWholesale.Discount + " " + lWholesale.DiscountType;
            }

            //SqlConnection lConn = new SqlConnection(lContext.Connection.ConnectionString);
            //SqlDataAdapter lDa = new SqlDataAdapter("Select WholesaleID, Discount + ' ' + DiscountType AS WholesaleText FROM Wholesales", lConn);
            //lConn.Open();
            //lDa.Fill(lDs);
            //lConn.Close();

            //LoadBTODropdowns(ddlBTOItem1, drpRecommendations1, drpWholesale1);
            //LoadBTODropdowns(ddlBTOItem2, drpRecommendations2, drpWholesale2);
            //LoadBTODropdowns(ddlBTOItem3, drpRecommendations3, drpWholesale3);
            LoadBTODropdowns(drpEditBTOItems, drpEditRecommendations, drpEditWholesales);
        }

        private void LoadBTODropdowns(DropDownList pdrpBTOItem, DropDownList pdrpRecommendations, DropDownList pdrpWholesale)
        {
            pdrpBTOItem.DataSource = lBTOItems;
            pdrpBTOItem.DataTextField = "Description";
            pdrpBTOItem.DataValueField = "BTOItemID";
            pdrpBTOItem.DataBind();
            pdrpBTOItem.Items.Insert(0, new ListItem("Please select item"));
            
            pdrpRecommendations.DataSource = lRecommendations;
            pdrpRecommendations.DataTextField = "Title";
            pdrpRecommendations.DataValueField = "RecommendationID";
            pdrpRecommendations.DataBind();

            //BTOItem lBTOItem = lContext.BTOItems.SingleOrDefault(x => x.BTOItemID == Convert.ToInt32(pdrpBTOItem.SelectedItem.Value));

            //if (lBTOItem != null && lBTOItem.BTOItemID > 0)
            //{
            //    string lBTOProdID = lBTOItem.ProductID, lBTOPartNumber = lBTOItem.PartNo;
            //    LoadWholesaleDropdown(lBTOPartNumber, lBTOProdID, pdrpWholesale);
            //}
            //else
            //{
                pdrpWholesale.Items.Add(new ListItem("No wholesale found", "0"));
            //}
        }

        private void LoadSubSections()
        {
            var lSubSections = from p in lContext.SubSections
                               where p.SectionID == Convert.ToInt32(Request.QueryString["_"])
                               orderby p.DisplayOrder
                               select new
                               {
                                   p.SubSectionID,
                                   p.SectionID,
                                   p.Title,
                                   p.CreatedOn,
                                   p.CreatedBy,
                                   p.ModifiedOn,
                                   p.ModifiedBy,
                                   p.DisplayOrder
                               };
            rptrSubSections.DataSource = lSubSections;
            rptrSubSections.DataBind();
        }

        private void ResetBTOItems(bool isEdit)
        {
            List<Int32> lItems = new List<int>();
            lItems.Add(1);
            lItems.Add(2);
            lItems.Add(3);
            rptrBTOItems.DataSource = lItems;
            rptrBTOItems.DataBind();
            Session["RepeaterDS"] = lItems;

            if (!isEdit)
                hdnSubSectionID.Value = "0";

            txtSubHeading.Text = "";
        }

        [WebMethod]
        public static string ChangeDisplayOrder(string lIdToReposition, string lNewPosition)
        {
            try
            {
                MainGearDataContext lContext = new MainGearDataContext();

                if (lIdToReposition != null && lIdToReposition.Trim() != "")
                {
                    SectionItem lSectionnewPosition = lContext.SectionItems.SingleOrDefault(p => p.SubSectionID == int.Parse(lIdToReposition.Split('-')[0]) && p.SectionItemID == int.Parse(lIdToReposition.Split('-')[1]));
                    SectionItem lSectionOldPosition = lContext.SectionItems.SingleOrDefault(p => p.SubSectionID == int.Parse(lIdToReposition.Split('-')[0]) && p.DisplayOrder == int.Parse(lNewPosition) + 1);

                    if (int.Parse(lIdToReposition.Split('-')[1]) < (int.Parse(lNewPosition) + 1)) // going down
                    {
                        lSectionnewPosition.DisplayOrder = int.Parse(lNewPosition) + 1;
                        lSectionOldPosition.DisplayOrder = lSectionOldPosition.DisplayOrder - 1;
                    }
                    else // going up
                    {
                        lSectionnewPosition.DisplayOrder = int.Parse(lNewPosition) + 1;
                        lSectionOldPosition.DisplayOrder = lSectionOldPosition.DisplayOrder + 1;
                    }

                    lContext.SubmitChanges();
                    return "1";
                }
                else
                    return "0";
            }
            catch
            { return "0"; }
        }
        
        private bool LoadWholesaleDropdown(string pBTOPartNumber, string pBTOProdID, DropDownList pdrpEditWholesales)
        {
            string lBTOProdID = pBTOProdID, lBTOPartNumber = pBTOPartNumber;
            IQueryable<Wholesale> lWholesalesFiltered = lWholesales.Where(x => x.ProductID == lBTOProdID);
            pdrpEditWholesales.Items.Clear();


            List<ListItem> Items = new List<ListItem>();
            if (lWholesalesFiltered.Count() > 0)
            {
                Items.Add(new ListItem("Please select wholesale", "0"));
                BuildWholeSaleItems(lWholesalesFiltered.ToList(), ref Items);
            }
            else
            {
                Items.Add(new ListItem("No wholesale found", "0"));
            }
            Items.First().Selected = true;
            pdrpEditWholesales.Items.AddRange(Items.ToArray());
            return true;
        }

        private static void BuildWholeSaleItems(List<Wholesale> Wholesales, ref List<ListItem> Items){
            string lDiscountType = "";
            foreach (var lWholesaleItem in Wholesales)
            {
                if (lWholesaleItem.DiscountType.ToLower() == "percentprice")
                    lDiscountType = "%";

                ListItem lItem = new ListItem();
                lItem.Text = lWholesaleItem.Discount + lDiscountType + " off Web Price";
                lItem.Value = lWholesaleItem.WholesaleID.ToString();
                Items.Add(lItem);
            }
        }

        [WebMethod]
        public static List<ListItem> GetWholesales(string pBTOItemID)
        {
            List<ListItem> lListItems = new List<ListItem>();

            try
            {
                MainGearDataContext lContext = new MainGearDataContext();
                BTOItem lBTOItem = lContext.BTOItems.SingleOrDefault(x => x.BTOItemID == Convert.ToInt64(pBTOItemID));

                if (lBTOItem != null && lBTOItem.BTOItemID > 0)
                {
                    string lBTOProdID = lBTOItem.ProductID, lBTOPartNumber = lBTOItem.PartNo;
                    IQueryable<Wholesale> lWholesalesFiltered = lWholesales.Where(x => x.ProductID == lBTOProdID);

                    BuildWholeSaleItems(lWholesalesFiltered.ToList(), ref lListItems);
                }
            }
            catch
            { }

            return lListItems;
        }

        [WebMethod]
        public static string GetBTOItemPrice(string pBTOItemID)
        {
            string lPrice = "";

            try
            {
                MainGearDataContext lContext = new MainGearDataContext();
                BTOItem lBTOItem = lContext.BTOItems.SingleOrDefault(x => x.BTOItemID == Convert.ToInt32(pBTOItemID));

                if (lBTOItem != null && lBTOItem.BTOItemID > 0)
                {
                    lPrice = lBTOItem.WebPrice;
                }
            }
            catch
            { }

            return lPrice;
        }
           
        [WebMethod]
        public static BTOItemOptionsList GetBTOItemOptions(string pBTOItemID)
        {
            BTOItemOptionsList List = new BTOItemOptionsList();
            List.Options = new List<ListItem>();

            try
            {
                MainGearDataContext lContext = new MainGearDataContext();
                BTOItem lBTOItem = lContext.BTOItems.SingleOrDefault(x => x.BTOItemID == Convert.ToInt64(pBTOItemID));


                if (lBTOItem != null && lBTOItem.BTOItemID > 0)
                {

                    IQueryable<BTOItem> GroupedBTOItems = lContext.BTOItems.Where(o => o.ProductID == lBTOItem.ProductID);
                    if (GroupedBTOItems.Count() > 1)
                    {
                        List.OptionsTitle = GroupedBTOItems.FirstOrDefault().PrimaryOptionTitle;
                        foreach (var BTOItem in GroupedBTOItems)
                        {
                            List.Options.Add(new ListItem(BTOItem.PrimaryOptionValue, BTOItem.BTOItemID.ToString()));
                        }
                        //List.Options.Add(new ListItem("All", "100"));
                    }

                }
            }
            catch
            { }

            return List;
        }



        
        #endregion
    }
    public static class IEnumrableExtension
    {
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
    }
    public class BTOItemOptionsList{
        public string OptionsTitle { get; set; }
        public  List<ListItem> Options { get; set; }
    }

    public class SectionItemWithOptions
    {
        public SectionItem SectionItem { get; set; }
        public List<SectionItemOption> SectionItemOptions { get; set; }
    }
    
}