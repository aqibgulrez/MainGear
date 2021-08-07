using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Configuration;
using SCE.MainGear.DAL;
using Microsoft.VisualBasic.FileIO;
using System.IO.Compression;
using SCE.MainGear.Service.com.shoppingcartelite.api;

namespace SCE.MainGear.Service
{
    public partial class BTOItemService : ServiceBase
    {
        private Timer lTimer;
        MainGearDataContext lContext = new MainGearDataContext();
        string lRootPath = "";

        public BTOItemService()
        {
            InitializeComponent();

        }

        protected override void OnStart(string[] args)
        {
            lRootPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            this.WriteToFile("BTO Items Service started {0}");
            this.WriteToFile("Going to fetch BTO items..");
            FetchBTOItems();
            this.WriteToFile("Going to fetch wholesale items..");
            FetchWholesales();
            this.ScheduleService();
        }

        protected override void OnStop()
        {
            this.WriteToFile("BTO Items Service stopped {0}");
            this.lTimer.Dispose();
        }

        public void ScheduleService()
        {
            try
            {
                lTimer = new Timer(new TimerCallback(SchedularCallback));
                string mode = ConfigurationManager.AppSettings["Mode"].ToUpper();
                this.WriteToFile("BTO Item Service Mode: " + mode + " {0}");

                //Set the Default Time.
                DateTime scheduledTime = DateTime.MinValue;

                if (mode == "DAILY")
                {
                    //Get the Scheduled Time from AppSettings.
                    scheduledTime = DateTime.Parse(System.Configuration.ConfigurationManager.AppSettings["ScheduledTime"]);
                    if (DateTime.Now > scheduledTime)
                    {
                        //If Scheduled Time is passed set Schedule for the next day.
                        scheduledTime = scheduledTime.AddDays(1);
                    }
                }

                if (mode.ToUpper() == "INTERVAL")
                {
                    //Get the Interval in Minutes from AppSettings.
                    int intervalMinutes = Convert.ToInt32(ConfigurationManager.AppSettings["IntervalMinutes"]);

                    //Set the Scheduled Time by adding the Interval to Current Time.
                    scheduledTime = DateTime.Now.AddMinutes(intervalMinutes);
                    if (DateTime.Now > scheduledTime)
                    {
                        //If Scheduled Time is passed set Schedule for the next Interval.
                        scheduledTime = scheduledTime.AddMinutes(intervalMinutes);
                    }
                }

                TimeSpan timeSpan = scheduledTime.Subtract(DateTime.Now);
                string schedule = string.Format("{0} day(s) {1} hour(s) {2} minute(s) {3} seconds(s)", timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);

                this.WriteToFile("BTO Items Service scheduled to run after: " + schedule + " {0}");

                //Get the difference in Minutes between the Scheduled and Current Time.
                int dueTime = Convert.ToInt32(timeSpan.TotalMilliseconds);

                //Change the Timer's Due Time.
                lTimer.Change(dueTime, Timeout.Infinite);
            }
            catch (Exception ex)
            {
                WriteToFile("BTO Items Service Error on: {0} " + ex.Message + ex.StackTrace);

                //Stop the Windows Service.
                using (System.ServiceProcess.ServiceController serviceController = new System.ServiceProcess.ServiceController("SimpleService"))
                {
                    serviceController.Stop();
                }
            }
        }

        private void SchedularCallback(object e)
        {
            this.WriteToFile("Going to fetch BTO items..");
            FetchBTOItems();
            this.WriteToFile("Going to fetch wholesale items..");
            FetchWholesales();
            this.WriteToFile("BTO Items Service Log: {0}");
            this.ScheduleService();
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
                            this.WriteToFile("Wholesale MESSAGE => " + ex.Message + "\n\n INNER EXCEPTION => " + ex.InnerException + "\n\n STACK TRACE => " + ex.StackTrace);
                        }
                    }
                }

                this.WriteToFile(lTotalItems.ToString() + " Wholesale Items imported successfully.");
            }
            catch (Exception ex)
            {
                this.WriteToFile("Wholesale MESSAGE => " + ex.Message + "\n\n INNER EXCEPTION => " + ex.InnerException + "\n\n STACK TRACE => " + ex.StackTrace);
            }
        }

        private void WriteToFile(string text)
        {
            string path = "C:\\BTOItemsServiceLog.txt";
            using (StreamWriter writer = new StreamWriter(path, true))
            {
                writer.WriteLine(string.Format(text, DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt")));
                writer.Close();
            }
        }

        private void FetchBTOItems()
        {
            try
            {
                //int lSize = Convert.ToInt32(ConfigurationManager.AppSettings["ProductArraySize"]);
                //this.WriteToFile("Array Size: " + lSize.ToString());
                //string[] lProductsIDs = new string[lSize];
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
                                    BTOItem lBTOItem = lContext.BTOItems.FirstOrDefault(p => p.PartNo.Trim().ToLower() == fields[147].Trim().ToLower());

                                    if (lBTOItem != null && lBTOItem.BTOItemID > 0) // update existing item
                                    {
                                        // 4, 14, 158, 147, 148, 27, 152, 29, 39, 50, 15, 76
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
                                        // 4, 14, 158, 147, 148, 27, 152, 29, 39, 50, 15, 76
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
                                this.WriteToFile("BTO Item MESSAGE => " + ex.Message + "\n\n INNER EXCEPTION => " + ex.InnerException + "\n\n STACK TRACE => " + ex.StackTrace);
                            }
                        }
                                                
                        this.WriteToFile(lTotalItems.ToString() + " BTO Items imported successfully.");
                    }
                }
            }
            catch (Exception ex)
            {
                this.WriteToFile("BTO Item MESSAGE => " + ex.Message + "\n\n INNER EXCEPTION => " + ex.InnerException + "\n\n STACK TRACE => " + ex.StackTrace);
            }
        }
    }
}
