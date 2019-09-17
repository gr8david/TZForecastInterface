using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ice.Core;
using Erp.Common;
using Erp.BO;
using Erp.Proxy.BO;

namespace TZForecastInterface
{
    public partial class EpicorInterface : Form
    {
        public EpicorInterface()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void btn_Go_Click(object sender, EventArgs e)
        {


            try
            {
                Ice.Core.Session obj = new Ice.Core.Session("dplayne", "xaser11", Ice.Core.Session.LicenseType.Default, @"c:\epicor\ERP10_2_SND1.sysconfig");
                if (obj != null)
                {
                    MessageBox.Show("Session Valid");

                    obj.CompanyID = "TZNZ";
                    //Erp.Contracts.ForecastSvcContract hForecast = null;
                    var ForecastBO = Ice.Lib.Framework.WCFServiceSupport.CreateImpl<Erp.Proxy.BO.ForecastImpl>(obj, Erp.Proxy.BO.ForecastImpl.UriPath);

                    if (ForecastBO != null)
                    {
                        ForecastBO.ClearForecasts(cbAllSites.Checked, dtFromDate.Value);
                    }
                    obj.Dispose();
                    obj = null;
                }
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }




        }

        private void EpicorInterface_Load(object sender, EventArgs e)
        {
            lb_CompanyID.Text = Global.CompanyID;

            dtFromDate.Value = Global.ForecastPeriod.FirstDate;

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            // Process att checked tasks

            if (cbClearFcst.Checked == true)
            {
                int result = ClearForecasts(Global.ForecastPeriod.FirstDate, cbAllSites.Checked);
                if (result != 0)
                {
                    MessageBox.Show("Clear Forecasts failed. Execption: " + result.ToString());
                    return;
                }

                else
                {
                    // Display sucess message
                    MessageBox.Show("Clear Forecasts completed");

                }
            }

            if (cbSmartFcsts.Checked == true)
            {
                int result = LoadForecasts(tbSmartForecastsFile.Text);

            }

            if (cbCustEngFcsts.Checked == true)
            {
                int result = LoadForecasts(tbCustEngFile.Text);
            }

            if (cbUsageFcsts.Checked == true)
            {
                int result = LoadForecasts(tbUsageFile.Text);
            }

            if (cbManualFcsts.Checked == true)
            {
                int result = LoadForecasts(tbManualFile.Text);
            }

            if (cbICFcsts.Checked == true)
            {
                int result = LoadForecasts(tbICFile.Text);
            }
        }

        private int ClearForecasts(DateTime p_FromDate, bool p_AllPlants)
        {

            try
            {
                Ice.Core.Session obj = new Ice.Core.Session("dplayne", "xaser11", Ice.Core.Session.LicenseType.Default, @"c:\epicor\ERP10_2_SND2.sysconfig");
                if (obj != null)
                {
                    MessageBox.Show("Session Valid");

                    var ForecastBO = Ice.Lib.Framework.WCFServiceSupport.CreateImpl<Erp.Proxy.BO.ForecastImpl>(obj, Erp.Proxy.BO.ForecastImpl.UriPath);
                    if (ForecastBO == null)
                    {
                        return 1;
                    }

                    ForecastBO.ClearForecasts(p_AllPlants, p_FromDate);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Execption: " + ex.Message);
                return 9;
            }

            return 0;
        }

        int LoadForecasts(string p_FilePath)
        {
            try
            {
                // Import the source file into a dataset



                Ice.Core.Session obj = new Ice.Core.Session("dplayne", "xaser11", Ice.Core.Session.LicenseType.Default, @"c:\epicor\ERP10_2_SND2.sysconfig");
                if (obj != null)
                {
                    MessageBox.Show("Session Valid");

                    var ForecastBO = Ice.Lib.Framework.WCFServiceSupport.CreateImpl<Erp.Proxy.BO.ForecastImpl>(obj, Erp.Proxy.BO.ForecastImpl.UriPath);
                    if (ForecastBO == null)
                    {
                        return 1;
                    }


                    dsDetailFcst dsNewForecasts = GetNewForecasts(p_FilePath);
                    Erp.BO.ForecastDataSet dsForecast = new Erp.BO.ForecastDataSet();
                    //int counter = 0;

                    foreach (DataRow dr in dsNewForecasts.dtDetailFcst.Rows)
                    {
                        
                        dsForecast.Tables[0].Clear();

                        DataRow drForecast = dsForecast.Tables[0].NewRow();
                        
                        drForecast["Company"]       = dr[0];
                        drForecast["CustNum"]       = dr[3];
                        drForecast["PartNum"]       = dr[2];
                        drForecast["Plant"]         = dr[1];
                        drForecast["ForeDate"]      = dr[4];
                        drForecast["Inactive"]      = false;
                        drForecast["ForeQty"]       = dr[5];
                        drForecast["ForeQtyUOM"] = "EACH"; //
                        drForecast["ConsumedQty"]   = 0;
                        drForecast["PONum"] = "";
                        drForecast["CreatedBy"] = "";
                        drForecast["AutoTransfer"]  = false;
                        drForecast["DemandReference"] = "";
                        drForecast["DemandContractNum"] = 0;
                        drForecast["DemandHeadSeq"] = 0;
                        drForecast["ScheduleNumber"] = "";
                        drForecast["ShipToNum"] = "";
                        drForecast["ParentPartNum"] = "";
                        drForecast["KitFlag"] = "";
                        //drForecast["EDIUpdateDate"] = "";
                        drForecast["SysRevID"] = 0;
                        drForecast["SysRowID"] = "00000000-0000-0000-0000-000000000000";
                        drForecast["ImportErrorMsg"] = "";
                        drForecast["BitFlag"] = 0;
                        drForecast["CustomerBTName"] = "";
                        drForecast["CustomerName"] = "";
                        drForecast["CustomerCustID"] = "";
                        drForecast["DemandContractDemandContract"] = "";
                        drForecast["PartNumSellingFactor"] = 1;
                        drForecast["PartNumTrackSerialNum"] = false;
                        drForecast["PartNumTrackDimension"] = false;
                        drForecast["PartNumSalesUM"] = "";
                        drForecast["PartNumPartDescription"] = "";
                        drForecast["PartNumTrackLots"] = false;
                        drForecast["PartNumPricePerCode"] = "E";
                        drForecast["PartNumIUM"] = "";
                        drForecast["PlantName"] = "";
                        drForecast["RowMod"] = "A";
;
                        dsForecast.Tables[0].Rows.Add(drForecast);
                        
                        try
                        {
                            ForecastBO.Update(dsForecast);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Exception: " + ex.Message);
                        }
                        
                    }
                    

                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Load SmartForecasts file failed. Exception: " + ex.Message);
                return 1;
            }

            return 0;
        }

        dsDetailFcst GetNewForecasts(string p_FilePath)
        {
            dsDetailFcst dsNewForecasts = new dsDetailFcst();

            // Read source file and load records into dsNewForecasts

            try
            {
                string[] csvRows = System.IO.File.ReadAllLines(p_FilePath);
                string[] fields = null;
                bool firstrow = true;
                foreach (string csvRow in csvRows)
                {
                    if (firstrow == false)
                    {
                        fields = csvRow.Split(',');
                        DataRow row = dsNewForecasts.dtDetailFcst.NewRow();

                        row[0] = fields[0];                     //Company
                        row[1] = fields[1];                     //Plant
                        row[2] = fields[2];                     //PartNum
                        row[3] = fields[3];                     //CustNum
                        row[4] = fields[4];                     //ForeDate
                        row[5] = fields[5];                     //ForeQty

                        if (Convert.ToInt32(row[5]) > 0)
                        {
                            dsNewForecasts.dtDetailFcst.Rows.Add(row);
                        }


                    }
                    firstrow = false;
                }
            }
            catch(Exception ex)
            {
                return null;
            }

            return dsNewForecasts;
        }
    }

}
