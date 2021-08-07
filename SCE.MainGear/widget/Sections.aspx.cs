using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SCE.MainGear.DAL;
using System.Web.Services;

namespace SCE.MainGear.admin
{
    public partial class Sections : System.Web.UI.Page
    {
        MainGearDataContext lContext = new MainGearDataContext();

        #region Events
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["_"] != null)
                {
                    Product lProduct = lContext.Products.SingleOrDefault(p => p.ProductID == int.Parse(Request.QueryString["_"]));
                    ltrlProductTitle.Text = lProduct.Title;
                    LoadSections();

                    if (Convert.ToBoolean(Session["IsMaster"]))
                        liAddFromMaster.Attributes["style"] = "display:none;";
                    else
                    {
                        liAddFromMaster.Attributes["style"] = "display:block;";
                        LoadMasterDropdown();
                    }
                }
                else
                    Response.Redirect("index.aspx");
            }
        }
        
        protected void lbtnAddSection_Click(object sender, EventArgs e)
        {
            Response.Redirect("addsection.aspx?__=" + Request.QueryString["_"]);
        }

        protected void lbtnAddSectionFromMaster_Click(object sender, EventArgs e)
        {
            if (drpMasterSections.Items.Count > 0)
                drpMasterSections.SelectedIndex = 0;
            else
                drpMasterSections.Items.Add(new ListItem("No master section available"));

            ClientScript.RegisterStartupScript(this.Page.GetType(), "modal", "<script type='text/javascript'>$( document ).ready(function() { $('#masterModal').modal('show')});</script>", false);
        }

        protected void rptrSections_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "del")
            {
                Section lSection = lContext.Sections.SingleOrDefault(p => p.SectionID == int.Parse(e.CommandArgument.ToString()));

                if (lSection != null)
                {
                    lContext.Sections.DeleteOnSubmit(lSection);
                    lContext.SubmitChanges();

                    // delete section instruction
                    lContext.ExecuteCommand("DELETE FROM SectionInstructions WHERE SectionID = " + lSection.SectionID.ToString());
                    lContext.SubmitChanges();

                    // delete all subsections
                    var lSubSections = from p in lContext.SubSections
                                       where p.SectionID == lSection.SectionID
                                       select new { p.SubSectionID };

                    foreach (var lSubSection in lSubSections)
                    {
                        long lSubSectionIDToDelete = lSubSection.SubSectionID;
                        lContext.ExecuteCommand("DELETE FROM SubSections WHERE SectionID = " + lSubSectionIDToDelete.ToString());
                        lContext.SubmitChanges();

                        // delete all section items
                        var lSectionItems = from p in lContext.SectionItems
                                            where p.SubSectionID == lSubSectionIDToDelete
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
                    }

                    LoadSections();

                    ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$.toaster({ message : 'Section deleted.', priority : 'success' });", true);
                }
                else
                {
                    ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$.toaster({ message : 'Deletion failed.', priority : 'danger' });", true);
                }
            }
            else if (e.CommandName == "edit")
            {
                Response.Redirect("addsection.aspx?_=" + e.CommandArgument.ToString() + "&__=" + Request.QueryString["_"]);
            }
            else if (e.CommandName == "instruction")
            {
                LoadInstructions(e.CommandArgument.ToString());
                txtInstructionTitle.Focus();
                ClientScript.RegisterStartupScript(this.Page.GetType(), "modal", "<script type='text/javascript'>$( document ).ready(function() { $('#instModal').modal('show')});</script>", false);
            }
        }

        protected void btnSaveInstructions_Click(object sender, EventArgs e)
        {
            SectionInstruction lSectionInst = lContext.SectionInstructions.SingleOrDefault(p => p.SectionID == int.Parse(hdnSectionIDForInst.Value));

            if (lSectionInst != null && lSectionInst.SectionInstructionID > 0) // instruction already exists
            {
                lSectionInst.Title = txtInstructionTitle.Text.Trim();
                lSectionInst.ShortDescription = txtShortDesc.Text.Trim();
                lSectionInst.LongDescription = HttpUtility.HtmlDecode(txtLongDesc.Value.Trim());
                lSectionInst.ModifiedBy = ((AdminUser)Session["AdminUser"]).UserID;
                lSectionInst.ModifiedOn = DateTime.Now;
                lContext.SubmitChanges();
            }
            else // add new
            {
                lSectionInst = new SectionInstruction();
                lSectionInst.SectionID = int.Parse(hdnSectionIDForInst.Value);
                lSectionInst.Title = txtInstructionTitle.Text.Trim();
                lSectionInst.ShortDescription = txtShortDesc.Text.Trim();
                lSectionInst.LongDescription = HttpUtility.HtmlDecode(txtLongDesc.Value.Trim());
                lSectionInst.CreatedBy = ((AdminUser)Session["AdminUser"]).UserID;
                lSectionInst.CreatedOn = DateTime.Now;
                lContext.SectionInstructions.InsertOnSubmit(lSectionInst);
                lContext.SubmitChanges();
            }

            ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$.toaster({ message : 'Instruction saved.', priority : 'success' });", true);
        }

        protected void btnAddFromMaster_Click(object sender, EventArgs e)
        {
            // fetch master section
            Section lMasterSection = lContext.Sections.SingleOrDefault(p => p.SectionID == int.Parse(drpMasterSections.SelectedItem.Value));

            if (lMasterSection != null && lMasterSection.SectionID > 0)
            {
                CloneSection(lMasterSection);
                LoadSections();
                ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$.toaster({ message : 'Master section copied.', priority : 'success' });", true);
            }
        }
                
        #endregion

        #region Functions

        private void CloneSection(Section lMasterSection)
        {
            // copy master section to new section of curent product
            Section lNewSection = new Section();
            lNewSection.Title = lMasterSection.Title;
            lNewSection.RenderModeID = lMasterSection.RenderModeID;
            lNewSection.ProductStepID = lMasterSection.ProductStepID;
            lNewSection.PricingID = lMasterSection.PricingID;
            lNewSection.IsRequired = lMasterSection.IsRequired;
            lNewSection.ImageName = lMasterSection.ImageName;
            lNewSection.Description = lMasterSection.Description;
            lNewSection.CreatedBy = ((AdminUser)Session["AdminUser"]).UserID;
            lNewSection.CreatedOn = DateTime.Now;
            lNewSection.IsMaster = false;

            long lDisplayOrder = 1;

            try
            {
                lDisplayOrder = lContext.Sections.Where(x => x.ProductID == Convert.ToInt32(Request.QueryString["_"])).OrderByDescending(u => u.DisplayOrder).FirstOrDefault().DisplayOrder;
            }
            catch
            { }

            lNewSection.DisplayOrder = lDisplayOrder + 1;
            lNewSection.ModifiedBy = null;
            lNewSection.ModifiedOn = null;
            lNewSection.ProductID = Convert.ToInt32(Request.QueryString["_"]);
            lContext.Sections.InsertOnSubmit(lNewSection);
            lContext.SubmitChanges();

            // add section instruction to newly added section
            SectionInstruction lMasterSectionInst = lContext.SectionInstructions.SingleOrDefault(p => p.SectionID == int.Parse(drpMasterSections.SelectedItem.Value));

            if (lMasterSectionInst != null && lMasterSectionInst.SectionInstructionID > 0)
            {
                SectionInstruction lNewSectionInstruction = new SectionInstruction();
                lNewSectionInstruction.LongDescription = lMasterSectionInst.LongDescription;
                lNewSectionInstruction.ShortDescription = lMasterSectionInst.ShortDescription;
                lNewSectionInstruction.Title = lMasterSectionInst.Title;
                lNewSectionInstruction.SectionID = lNewSection.SectionID;
                lNewSectionInstruction.ModifiedBy = null;
                lNewSectionInstruction.ModifiedOn = null;
                lNewSectionInstruction.CreatedBy = ((AdminUser)Session["AdminUser"]).UserID;
                lNewSectionInstruction.CreatedOn = DateTime.Now;
                lContext.SectionInstructions.InsertOnSubmit(lNewSectionInstruction);
                lContext.SubmitChanges();
            }

            // add sub section to newly added section
            var lMasterSubSections = from p in lContext.SubSections
                                     where p.SectionID == Convert.ToInt32(drpMasterSections.SelectedItem.Value)
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
                                         p.DisplayOrder,
                                         p.IsMaster
                                     };

            foreach (var lMasterSubSection in lMasterSubSections)
            {
                SubSection lNewSubSection = new SubSection();
                lNewSubSection.Title = lMasterSubSection.Title;
                lNewSubSection.SectionID = lNewSection.SectionID;
                lNewSubSection.CreatedBy = ((AdminUser)Session["AdminUser"]).UserID;
                lNewSubSection.CreatedOn = DateTime.Now;
                lNewSubSection.IsMaster = false;
                lNewSubSection.ModifiedBy = null;
                lNewSubSection.ModifiedOn = null;

                lDisplayOrder = 1;

                try
                {
                    lDisplayOrder = lContext.SubSections.Where(x => x.SectionID == lNewSection.SectionID).OrderByDescending(u => u.DisplayOrder).FirstOrDefault().DisplayOrder;
                }
                catch
                { }

                lNewSubSection.DisplayOrder = lDisplayOrder + 1;
                lContext.SubSections.InsertOnSubmit(lNewSubSection);
                lContext.SubmitChanges();

                // fetch section items for each subsection and add them with newly added subsection ID
                var lMasterSectionItems = from p in lContext.SectionItems
                                          where p.SubSectionID == Convert.ToInt32(lMasterSubSection.SubSectionID)
                                          orderby p.DisplayOrder
                                          select new
                                          {
                                              p.SectionItemID,
                                              p.SubSectionID,
                                              p.DisplayOrder,
                                              p.BTOItemID,
                                              p.IsQuantity,
                                              p.IsQuantityRequired,
                                              p.ProcessingTime,
                                              p.QuantityIncrement,
                                              p.RecommendationID,
                                              p.WholesaleID,
                                              p.DisplayMode,
                                              Tags = lContext.ItemTags.Where(t => t.SectionItemID == p.SectionItemID),
                                              Filters = lContext.ItemFilters.Where(f => f.SectionItemID == p.SectionItemID),
                                              Options = lContext.SectionItemOptions.Where(sio => sio.SectionItemID == p.SectionItemID)
                                          };

                foreach (var lMasterSectionItem in lMasterSectionItems)
                {
                    SectionItem lNewSectionItem = new SectionItem();
                    lNewSectionItem.CreatedBy = ((AdminUser)Session["AdminUser"]).UserID;
                    lNewSectionItem.CreatedOn = DateTime.Now;
                    lNewSectionItem.IsDefault = false;
                    lNewSectionItem.IsMaster = false;
                    lNewSectionItem.ModifiedBy = null;
                    lNewSectionItem.ModifiedOn = null;
                    lNewSectionItem.SubSectionID = lNewSubSection.SubSectionID;
                    lNewSectionItem.BTOItemID = lMasterSectionItem.BTOItemID;
                    lNewSectionItem.DisplayMode = lMasterSectionItem.DisplayMode;
                    lNewSectionItem.IsQuantity = lMasterSectionItem.IsQuantity;
                    lNewSectionItem.IsQuantityRequired = lMasterSectionItem.IsQuantityRequired;
                    lNewSectionItem.ProcessingTime = lMasterSectionItem.ProcessingTime;
                    lNewSectionItem.QuantityIncrement = lMasterSectionItem.QuantityIncrement;
                    lNewSectionItem.RecommendationID = lMasterSectionItem.RecommendationID;
                    lNewSectionItem.WholesaleID = lMasterSectionItem.WholesaleID;
                    lDisplayOrder = 1;

                    try
                    {
                        lDisplayOrder = lContext.SectionItems.Where(x => x.SubSectionID == lNewSubSection.SubSectionID).OrderByDescending(u => u.DisplayOrder).FirstOrDefault().DisplayOrder;
                    }
                    catch
                    { }

                    lNewSectionItem.DisplayOrder = lDisplayOrder + 1;
                    lContext.SectionItems.InsertOnSubmit(lNewSectionItem);
                    lContext.SubmitChanges();


                    // copy lMasterSectionItem's tags
                    foreach (var Tag in lMasterSectionItem.Tags)
                    {
                        ItemTag lNewTag = new ItemTag();
                        lNewTag.SectionItemID = lNewSectionItem.SectionItemID;
                        lNewTag.TagID = Tag.TagID;
                        lNewTag.CreatedOn = DateTime.Now;
                        lContext.ItemTags.InsertOnSubmit(lNewTag);
                    }


                    // copy lMasterSectionItem's Filters
                    foreach (var Filter in lMasterSectionItem.Filters)
                    {
                        ItemFilter lNewFilter = new ItemFilter();
                        lNewFilter.SectionItemID = lNewSectionItem.SectionItemID;
                        lNewFilter.SectionName = Filter.SectionName;
                        lNewFilter.SectionName = Filter.SectionName;
                        lNewFilter.Tag = Filter.Tag;
                        lNewFilter.FilterType = Filter.FilterType;
                        lNewFilter.CreatedOn = DateTime.Now;
                        lNewFilter.CreatedBy = ((AdminUser)Session["AdminUser"]).UserID;
                        lContext.ItemFilters.InsertOnSubmit(lNewFilter);
                    }


                    // copy lMasterSectionItem's Options
                    foreach (var SectionOption in lMasterSectionItem.Options)
                    {
                        SectionItemOption lNewSectionItemOption = new SectionItemOption();
                        lNewSectionItemOption.SectionItemID = lNewSectionItem.SectionItemID;
                        lNewSectionItemOption.BTOItemID = SectionOption.BTOItemID;
                        lContext.SectionItemOptions.InsertOnSubmit(lNewSectionItemOption);
                    }
                }
            }
        }

        private void LoadMasterDropdown()
        {
            var lMasterSections = from n in lContext.Sections where n.IsMaster == true select n;
            drpMasterSections.DataSource = lMasterSections;
            drpMasterSections.DataTextField = "Title";
            drpMasterSections.DataValueField = "SectionID";
            drpMasterSections.DataBind();
        }

        private void LoadInstructions(string pProductID)
        {
            hdnSectionIDForInst.Value = pProductID;
            SectionInstruction lSectionInst = lContext.SectionInstructions.SingleOrDefault(p => p.SectionID == int.Parse(pProductID));

            if (lSectionInst != null && lSectionInst.SectionInstructionID > 0) // instruction already exists
            {
                txtInstructionTitle.Text = lSectionInst.Title;
                txtShortDesc.Text = lSectionInst.ShortDescription;
                txtLongDesc.Value = lSectionInst.LongDescription;
            }
            else
            {
                txtInstructionTitle.Text = string.Empty;
                txtShortDesc.Text = string.Empty;
                txtLongDesc.Value = string.Empty;
            }
        }
        
        private void LoadSections()
        {
            var lSections = from p in lContext.Sections
                            where p.ProductID == Convert.ToInt32(Request.QueryString["_"])
                            orderby p.DisplayOrder
                            select new
                            {
                                p.SectionID,
                                p.Description,
                                p.ImageName,
                                p.IsRequired,
                                p.PricingID,
                                p.ProductStepID,
                                p.RenderModeID,
                                p.ProductID,
                                p.Title,
                                p.CreatedOn,
                                p.CreatedBy,
                                p.ModifiedOn,
                                p.ModifiedBy,
                                p.DisplayOrder,
                                p.IsMaster
                            };
            rptrProducts.DataSource = lSections;
            rptrProducts.DataBind();
        }
                
        [WebMethod]
        public static string ChangeDisplayOrder(string lIdToReposition, string lNewPosition)
        {
            try
            {
                MainGearDataContext lContext = new MainGearDataContext();

                if (lIdToReposition != null && lIdToReposition.Trim() != "")
                {
                    Section lSectionnewPosition = lContext.Sections.SingleOrDefault(p => p.SectionID == int.Parse(lIdToReposition.Split('-')[0]));
                    Section lSectionOldPosition = lContext.Sections.SingleOrDefault(p => p.DisplayOrder == int.Parse(lNewPosition) + 1);

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

        #endregion
    }
}