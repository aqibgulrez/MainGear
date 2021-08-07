using SCE.MainGear.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SCE.MainGear.widget
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        MainGearDataContext lContext = new MainGearDataContext();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnChange_Click(object sender, EventArgs e)
        {
            if (txtOldPassword.Text.Trim() != String.Empty && txtNewPassword.Text.Trim() != String.Empty && txtConfirmPassword.Text.Trim() != string.Empty)
            {
                if (txtNewPassword.Text.Trim() == txtConfirmPassword.Text.Trim())
                {
                    string lHashPassword = Helper.CalculateMD5Hash(txtOldPassword.Text.Trim().ToLower());
                    AdminUser lAdminUser = lContext.AdminUsers.SingleOrDefault(p => p.Username == ((AdminUser)Session["AdminUser"]).Username.Trim().ToLower() && p.Password == lHashPassword);

                    if (lAdminUser != null)
                    {
                        lAdminUser.Password = Helper.CalculateMD5Hash(txtNewPassword.Text.Trim().ToLower());
                        lContext.SubmitChanges();
                        ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$.toaster({ message : 'Password changed successfully.', priority : 'success' }); window.setTimeout(function(){ window.location.href = 'index.aspx' }, 3000);", true);
                    }
                    else
                        ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$.toaster({ message : 'Invalid username or password.', priority : 'danger' });", true);
                }
                else
                    ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$.toaster({ message : 'Passwords do not match.', priority : 'danger' });", true);
            }
            else
            {
                if (txtOldPassword.Text.Trim() == String.Empty)
                    ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$.toaster({ message : 'Old password required.', priority : 'danger' });", true);

                if (txtNewPassword.Text.Trim() == String.Empty)
                    ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$.toaster({ message : 'New password required.', priority : 'danger' });", true);

                if (txtConfirmPassword.Text.Trim() == String.Empty)
                    ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$.toaster({ message : 'Confirm password required.', priority : 'danger' });", true);
            }
        }
        
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("index.aspx");
        }
    }
}