using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SCE.MainGear.DAL;
using SCE.MainGear.com.shoppingcartelite.api;
using System.IO;
using Microsoft.VisualBasic.FileIO;

namespace SCE.MainGear.admin
{
    public partial class Login : System.Web.UI.Page
    {
        MainGearDataContext lContext = new MainGearDataContext();

        protected void Page_Load(object sender, EventArgs e)
        {
            txtUsername.Focus();

            if (!IsPostBack)
            {
                try
                {
                    //string[] lProductsIDs = new string[40];
                    //com.shoppingcartelite.api.sceApi api = new com.shoppingcartelite.api.sceApi();
                    //AuthHeaderAPI header = new AuthHeaderAPI();
                    //header.ApiKey = "uyvfdubu4wi1jdvj5p4wqz";
                    //header.ApiSecretKey = "kvjncnasprrxkiwczrhbxs";
                    //header.ApiAccessKey = "KoYBmg+yJKqCMBcOc0188VwTqVpxQygfIXFOelapY5w=";
                    //api.AuthHeaderAPIValue = header;
                    
                    //for (int i = 0; i < 40; i++)
                    //{
                    //    lProductsIDs[i] = (i + 1).ToString();
                    //}

                    //byte[] zip = api.GetProductsExport(",", lProductsIDs);

                    //if (zip != null)
                    //{
                    //    System.IO.DirectoryInfo di = new DirectoryInfo(Server.MapPath("../files/imports/"));

                    //    foreach (FileInfo file in di.GetFiles())
                    //    {
                    //        file.Delete();
                    //    }

                    //    string startPath = Server.MapPath("../files/imports/");
                    //    string zipPath = Server.MapPath("../files/imports/" + System.Guid.NewGuid().ToString() + ".zip");
                    //    string extractPath = Server.MapPath("../files/imports/");

                    //    File.WriteAllBytes(zipPath, zip);
                    //    System.IO.Compression.ZipFile.ExtractToDirectory(zipPath, extractPath);

                    //    // delete all rows from BTOItem table
                    //    //lContext.ExecuteCommand("TRUNCATE TABLE BTOItems");

                    //    // parse csv
                    //    using (TextFieldParser parser = new TextFieldParser(Server.MapPath("../files/imports/newSceExport1.csv")))
                    //    {
                    //        parser.TextFieldType = FieldType.Delimited;
                    //        parser.SetDelimiters(",");
                    //        bool lIsFirstRow = true;

                    //        while (!parser.EndOfData)
                    //        {
                    //            string[] fields = parser.ReadFields();

                    //            if (!lIsFirstRow)
                    //            {
                    //                //Processing row

                    //                BTOItem lBTOItem = new BTOItem();
                    //                // 4, 14, 158, 147, 148, 27, 152, 29, 39, 50, 15, 76
                    //                lBTOItem.ProductTitle = fields[4];
                    //                lBTOItem.Brand = fields[14];
                    //                lBTOItem.WebPrice = fields[158];
                    //                lBTOItem.PartNo = fields[147];
                    //                lBTOItem.ManufacturerPartNo = fields[148];
                    //                lBTOItem.GeneralImage = fields[27];
                    //                lBTOItem.ApplicationSpecificImage = fields[152];
                    //                lBTOItem.MainCategory = fields[29];
                    //                lBTOItem.SubCategory = fields[39];
                    //                lBTOItem.SectionCategory = fields[50];
                    //                lBTOItem.Description = fields[15];
                    //                lBTOItem.Specification = fields[76];
                    //                lBTOItem.CreatedOn = DateTime.Now;
                    //                lContext.BTOItems.InsertOnSubmit(lBTOItem);
                    //            }
                    //            else
                    //                lIsFirstRow = false;
                    //        }

                    //        lContext.SubmitChanges();
                    //    }
                    //}
                }
                catch(Exception ex)
                {
                    string err = ex.Message;
                }
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text.Trim() != String.Empty && txtPassword.Text.Trim() != String.Empty)
            {
                string lHashPassword = Helper.CalculateMD5Hash(txtPassword.Text.Trim().ToLower());
                AdminUser lAdminUser = lContext.AdminUsers.SingleOrDefault(p => p.Username == txtUsername.Text.Trim().ToLower() && p.Password == lHashPassword);

                if (lAdminUser != null)
                {
                    Session.Add("AdminUser", lAdminUser);

                    if (Request.QueryString["returnurl"] != null)
                        Response.Redirect(Request.QueryString["returnurl"]);
                    else
                        Response.Redirect("Index.aspx");
                }
                else
                    ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$.toaster({ message : 'Invalid username or password.', priority : 'danger' });", true);
            }
        }

        //protected void lbtnChangePass_Click(object sender, EventArgs e)
        //{
        //    Response.Redirect("ChangePassword.aspx");
        //}
    }
}