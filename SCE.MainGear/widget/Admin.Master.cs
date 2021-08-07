using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SCE.MainGear.DAL;

namespace SCE.MainGear.admin
{
    public partial class Admin : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["AdminUser"] != null)
            {
                ltrlAdminName.Text = ((AdminUser)Session["AdminUser"]).Username;

                if (Session["IsMaster"] != null && Convert.ToBoolean(Session["IsMaster"]) == true)
                {
                    liMasterProduct.Attributes["class"] = "current-page";
                    liProductList.Attributes["class"] = "";
                    liBTOItems.Attributes["class"] = "";
                }
                else
                {
                    Session["IsMaster"] = false;

                    if (HttpContext.Current.Request.Url.AbsoluteUri.ToLower().Contains("btoitems.aspx"))
                    {
                        liMasterProduct.Attributes["class"] = "";
                        liProductList.Attributes["class"] = "";
                        liBTOItems.Attributes["class"] = "current-page";
                    }
                    else
                    {
                        liMasterProduct.Attributes["class"] = "";
                        liProductList.Attributes["class"] = "current-page";
                        liBTOItems.Attributes["class"] = "";
                    }
                }
            }
            else
                Response.Redirect("login.aspx");
        }

        protected void lbtnLogout_Click(object sender, EventArgs e)
        {
            Session["AdminUser"] = null;
            Session["IsMaster"] = null;
            Response.Redirect("login.aspx");
        }

        protected void lbtnMasterProduct_Click(object sender, EventArgs e)
        {
            Session["IsMaster"] = true;
            Response.Redirect("index.aspx");
        }

        protected void lbtnProductList_Click(object sender, EventArgs e)
        {
            Session["IsMaster"] = false;
            Response.Redirect("index.aspx");
        }

        protected void lbtnBTOItems_Click(object sender, EventArgs e)
        {
            Session["IsMaster"] = false;
            Response.Redirect("BTOItems.aspx");
        }

        protected void lbtnChangePassword_Click(object sender, EventArgs e)
        {
            Response.Redirect("ChangePassword.aspx");
        }
    }
}