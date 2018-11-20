using System;
using System.Collections.Generic;
using System.Configuration;

using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.Odbc;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using Erp.BO;
//using Erp.Proxy.BO;


namespace TZForecastInterface
{
    public partial class MainForm : Form
    {
        public class POSugg
        {
            public string Company {get; set; }
            public string Plant { get; set; }
            public string PartNum {get; set; }
            public DateTime DueDate { get; set; }
            public decimal RelQty { get; set; }
            public string ClassID { get; set; }
            public DateTime ShipDate { get; set; }
            public int ForePeriod { get; set; }
            
        }
        public enum WeekDay
        {
            [StringValue("Monday")] Monday = 2,
            [StringValue("Tuesday")] Tuesday = 3,
            [StringValue("Wednesday")] Wednesday = 4,
            [StringValue("Thursday")] Thursday = 5,
            [StringValue("Friday")] Friday = 6,
            [StringValue("Saturday")] Saturday = 7,
            [StringValue("Sunday")] Sunday = 1
        }
        public bool Debug = true;
        

        //public string CompanyID;
        

        //private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        //public SqlConnection CustomAppsConn = new SqlConnection(Properties.Settings.Default.CustomAppsConnStr);
        public SqlConnection ERPConn = new SqlConnection(Properties.Settings.Default.ERPConnStr);
        public DataTable dtForecastMethods = new DataTable();
        public dsPartPlantFcstMethods dsPPData = new dsPartPlantFcstMethods();
        public dsDaysOfWeek dsDOW = new dsDaysOfWeek();
        
        public MainForm()
        {
            InitializeComponent();
        }

        

        private void MainForm_Load(object sender, EventArgs e)
        {

            // Load combobox with available Companies. Currently loaded a fixed list but this could load from the ERP database
            BindingList<Company> comboItems = new BindingList<Company>();
            comboItems.Add(new Company { CompanyID = "TZNZ", Name = "temperzone New Zealand Ltd" });
            comboItems.Add(new Company { CompanyID = "TZA", Name = "temperzone Australia Pty Ltd" });


            cbCompany.DataSource = comboItems;
            cbCompany.DisplayMember = "Name";
            cbCompany.ValueMember = "CompanyID";
            cbCompany.SelectedIndex = 0;

            
            // Setup default forecast period
            Period DefaultPeriod = new Period((DateTime.Today.Year * 100) + DateTime.Today.Month);
            tbForecastPeriod.Text = DefaultPeriod.PeriodValue.ToString();


           
        }

  
        private void btnGo_Click(object sender, EventArgs e)
        {
            // Set cursor as hourglass
            Cursor.Current = Cursors.WaitCursor;

            // Clear UI fields as required.
            tbSmartforecastsMsg.Clear();
            tbCustEngMsg.Clear();
            tbUsageMsg.Clear();
            tbInterCoyMsg.Clear();

            // Set-up variables to determine which actions are required.
            bool ProcessSmartforecasts = cbProcessSmartforecasts.Checked;
            bool ProcessCustEng = cbProcessCustEng.Checked;
            bool ProcessUsage = cbProcessUsage.Checked;
            bool ProcessIC = cbProcessIC.Checked;

            
            // Set the global period values
            Period FirstPeriod = new Period(Convert.ToInt32(tbForecastPeriod.Text));
            Period LastPeriod = new Period(FirstPeriod.PeriodValue);
            LastPeriod.PeriodAdd(11);  // generate forecasts for next 12 months

            // Get the CompanyID from user selection
            //CompanyID = ((Company)cbCompany.SelectedItem).CompanyID;
            Global.CompanyID = ((Company)cbCompany.SelectedItem).CompanyID;

            switch(Global.CompanyID)
            {
                case "TZA":
                    Global.MainPlant = "SYDNEY";
                    Global.ICCustNum = 130;
                    Global.CustEngCustNum = 2689;
                    Global.UsageCustNum = 3199;     //Created  19 Nov 2018
                    break;
                case "TZNZ":
                    Global.MainPlant = "AUCK";
                    Global.ICCustNum = 2050;
                    Global.CustEngCustNum = 2425;
                    Global.UsageCustNum = 2802;
                    break;
            }

            Global.Debug = true;

            // Reset message textboxes
            tbCustEngMsg.BackColor = Color.White;
            tbCustEngMsg.ForeColor = Color.Black;
            tbUsageMsg.BackColor = Color.White;
            tbUsageMsg.ForeColor = Color.Black;
            tbSmartforecastsMsg.BackColor = Color.White;
            tbSmartforecastsMsg.ForeColor = Color.Black;

            // Log a start message
            string msg = String.Format("Start of Process");
            Utils.LogMessage(Utils.MsgLevel.Info, msg, false, true);
            msg = String.Format("Selection parameters: Company = {0}  Smartforecasts = {1}  Custom Eng. = {2}  Usage = {3}  Inter-Company = {4}   ", ((Company)cbCompany.SelectedItem).CompanyID, cbProcessSmartforecasts.CheckState, cbProcessCustEng.CheckState, cbProcessUsage.CheckState, cbProcessIC.CheckState);
            Utils.LogMessage(Utils.MsgLevel.Debug, msg, false, true);

            GetCommonData(FirstPeriod, LastPeriod);
            // Run the requested interfaces

            
            // Gert a data table forecast tyopes and methods from PartPlant table for selected company. We will need to refer to this later.
            dsPPData = GetPartPlantFcstMethods(Global.CompanyID);

            // Process each select forecast type
            if (ProcessSmartforecasts == true)
            {
                int SMF_RecsWritten = 0;
                //Read the CSV file and build a dsSummaryFcst dataset.
                dsSummaryFcst dsSMFSummary = GetSMFFcst(Global.CompanyID);
                if (dsSMFSummary == null)
                {
                    Utils.LogMessage(Utils.MsgLevel.Error,"No Smartforecasts summary records to be processed.", true, true);
                    return;
                }

                // Build a dsDetailFcst dataset. This contains the individual forecast records to be uploaded to Epicor
                // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                // LastPeriod can be varied based on number of future forecast periods required.
                // TZA will require 13 period
                //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                if (Global.CompanyID == "TZA")
                    LastPeriod.PeriodAdd(1);  // 
                dsDetailFcst dsSMFDetail = CalculateForecasts(dsSMFSummary, FirstPeriod.PeriodValue, LastPeriod.PeriodValue);
                if (dsSMFSummary == null)
                {
                    Utils.LogMessage(Utils.MsgLevel.Error, "No Smartforecast detail records to be processed.", true, true);
                    return;
                }

                // Now we can write the detail records to CSV file.
                string tmpPath = String.Format("{0}\\{1}_SmartForecasts_{2}.csv", Properties.Settings.Default.OutputFolder, Global.CompanyID, DateTime.Today.ToString("dd-MM-yyy"));
                SMF_RecsWritten = Utils.GenerateCSV(dsSMFDetail.dtDetailFcst,  tmpPath);

                tbSmartforecastsMsg.Text = string.Format(" {0} records processed with {1} warnings and {2} errors", SMF_RecsWritten, 0, 0);

            }

            if (ProcessCustEng == true)
            {
                // Get the CustNum for the CustEng customer
                int CustEngRecsWritten = 0;
                int CustEngWarnings = 0;
                int CustEngErrors = 0;
          
                
                dsSummaryFcst dsCustEngSummary = GetCustEngFcst(Global.CompanyID, cbCustEngSupressCP.Enabled, nuCustEngPeriodsToAvg.Value);
                // Now we call the forecast calculating function
                dsDetailFcst dsCustEngDetail = CalculateForecasts(dsCustEngSummary,FirstPeriod.PeriodValue, LastPeriod.PeriodValue);
                //WriteToCSV(dsCustEngDetail.dtDetailFcst, @"c:\temp\CustEngFcst.csv");
                string tmpPath = String.Format("{0}\\{1}_CustEngFcst_{2}.csv", Properties.Settings.Default.OutputFolder, Global.CompanyID, DateTime.Today.ToString("dd-MM-yyy"));
                CustEngRecsWritten = Utils.GenerateCSV(dsCustEngDetail.dtDetailFcst, tmpPath);
                tbCustEngMsg.Text = string.Format(" {0} records processed with {1} warnings and {2} errors", CustEngRecsWritten, CustEngWarnings, CustEngErrors);
                if (CustEngErrors > 0)
                {
                    tbCustEngMsg.BackColor = Color.Red;
                    tbCustEngMsg.ForeColor = Color.White;
                }
                else if (CustEngWarnings > 0)
                {
                    tbCustEngMsg.BackColor = Color.Yellow;
                    tbCustEngMsg.ForeColor = Color.Black;
                }
                else
                {
                    tbCustEngMsg.BackColor = Color.White;
                    tbCustEngMsg.ForeColor = Color.Black;
                }


            }

            if (ProcessUsage == true)
            {
                int UsageRecsWritten = 0;
                int UsageWarnings = 0;
                int UsageErrors = 0;

                dsSummaryFcst dsUsageFcstSummary = GetUsageFcst(Global.CompanyID, cbUsageSupressCP.Enabled, nuUsagePeriodsToAvg.Value);
                dsDetailFcst dsUsageDetail = CalculateForecasts(dsUsageFcstSummary, FirstPeriod.PeriodValue, LastPeriod.PeriodValue);
                string tmpPath = String.Format("{0}\\{1}_UsageFcst_{2}.csv", Properties.Settings.Default.OutputFolder, Global.CompanyID, DateTime.Today.ToString("dd-MM-yyy"));
                UsageRecsWritten = Utils.GenerateCSV(dsUsageDetail.dtDetailFcst, tmpPath);
                tbUsageMsg.Text = string.Format(" {0} records processed with {1} warnings and {2} errors", UsageRecsWritten, 0, 0);
                if (UsageErrors > 0)
                {
                    tbUsageMsg.BackColor = Color.Red;
                    tbUsageMsg.ForeColor = Color.White;
                }
                else if (UsageWarnings > 0)
                {
                    tbUsageMsg.BackColor = Color.Yellow;
                    tbUsageMsg.ForeColor = Color.Black;
                }
                else
                {
                    tbUsageMsg.BackColor = Color.White;
                    tbUsageMsg.ForeColor = Color.Black;
                }
            }

            if (ProcessIC == true)
            {

                int ICRecsWritten = 0;
                int ICWarnings = 0;
                int ICErrors = 0;

                // Get PO Suggestions from each partner company site

                // Accumulate into planning sites

                // Calculate forecasts

                dsSummaryFcst dsICSummary = GetICFcst(Global.CompanyID, LastPeriod.LastDate);

                // Calculate forecasts and generate CSV for 
                dsDetailFcst dsICDetail = CalculateForecasts(dsICSummary, FirstPeriod.PeriodValue, LastPeriod.PeriodValue);

                /* 25 Sep 2018
                 * At this stage we have forecasts calculated for TZM and HIT PartClasses.
                 * We need to add to this component part forecasts and sales orders.
                 * Then this needs to be grouped by Company, Plant, PartNum, ForeDate as there can be duplicates and Epicor
                 * won't allow duplicates
                 */
                dsDetailFcst dstmpDetailFcst = new dsDetailFcst();
                dstmpDetailFcst = (dsDetailFcst)dsICDetail.Copy();

                foreach (DataRow dr in dsICSummary.dtICPOSuggDtl)
                {
                    dsDetailFcst.dtDetailFcstRow row = dstmpDetailFcst.dtDetailFcst.NewdtDetailFcstRow();
                    row.Company = dr["Company"].ToString();
                    row.Plant = dr["Plant"].ToString();
                    row.PartNum = dr["PartNum"].ToString();
                    row.CustNum = Global.ICCustNum;
                    row.ForeDate = Convert.ToDateTime(dr["ForeDate"]);
                    row.ForeQty = Convert.ToInt32(dr["ForeQty"]);
                    
                    dstmpDetailFcst.dtDetailFcst.AdddtDetailFcstRow(row);

                }

                var query = (from tt in dstmpDetailFcst.dtDetailFcst
                             group tt by new
                             {
                                 tt.Company,
                                 tt.Plant,
                                 tt.PartNum,
                                 tt.CustNum,
                                 tt.ForeDate
                             } into g
                             select new
                             {
                                 Company = (System.String)g.Key.Company,
                                 Plant = (System.String)g.Key.Plant,
                                 PartNum = (System.String)g.Key.PartNum,
                                 CustNum = (System.Int32)g.Key.CustNum,
                                 ForeDate = (System.DateTime)g.Key.ForeDate,
                                 ForeQty = (System.Decimal)g.Sum(p => p.ForeQty)
                             }).ToList();

                /* Messy stuff */

                dsDetailFcst dsICFcst = new dsDetailFcst();

                foreach (var r in query)
                {
                  

                    dsDetailFcst.dtDetailFcstRow detailRow = dsICFcst.dtDetailFcst.NewdtDetailFcstRow();
                    detailRow.Company = Global.CompanyID;
                    detailRow.Plant = Global.MainPlant;
                    detailRow.PartNum = r.PartNum.ToString();
                    detailRow.CustNum = Global.ICCustNum;
                    detailRow.ForeDate = Convert.ToDateTime(r.ForeDate);
                    detailRow.ForeQty = Convert.ToInt32(r.ForeQty);
                 
                    dsICFcst.dtDetailFcst.AdddtDetailFcstRow(detailRow);
                }









                /* ######################*/

                // Generate CSV for all IC summarized Forecasts
                string tmpPath;
                /*
                tmpPath = String.Format("{0}\\{1}_ICFcsts_{2}.csv", Properties.Settings.Default.OutputFolder, Global.CompanyID, DateTime.Today.ToString("dd-MM-yyy"));
                ICRecsWritten = Utils.GenerateCSV(dsICFcst.dtDetailFcst, tmpPath);
                */

                tmpPath = String.Format("{0}\\{1}_ICFcstUnits_{2}.csv", Properties.Settings.Default.OutputFolder, Global.CompanyID, DateTime.Today.ToString("dd-MM-yyy"));
                //int ICRecsWritten = Utils.GenerateCSV(dsICDetail.dtDetailFcst, tmpPath);
                ICRecsWritten = Utils.GenerateCSV(dsICDetail.dtDetailFcst  , tmpPath);

                // Now generate CSV for non-TZM and HIT IC PO Suggestions
                tmpPath = String.Format("{0}\\{1}_ICFcst_Components{2}.csv", Properties.Settings.Default.OutputFolder, Global.CompanyID, DateTime.Today.ToString("dd-MM-yyy"));
                ICRecsWritten += Utils.GenerateCSV(dsICSummary.dtICPOSuggDtl, tmpPath);

                // Finally add forecasts to offset existing Sales Orders to Intercompany partner
                //dsDetailFcst dsICSalesOrders = CalculateSOForecasts()

                tbInterCoyMsg.Text = string.Format(" {0} records propcessed with {1} warnings and {2} errors", ICRecsWritten, 0, 0);

                if (ICErrors > 0)
                {
                    tbInterCoyMsg.BackColor = Color.Red;
                    tbInterCoyMsg.ForeColor = Color.White;
                }
                else if (ICWarnings > 0)
                {
                    tbInterCoyMsg.BackColor = Color.Yellow;
                    tbInterCoyMsg.ForeColor = Color.Black;
                }
                else
                {
                    tbInterCoyMsg.BackColor = Color.White;
                    tbInterCoyMsg.ForeColor = Color.Black;
                }
            }

            

            Cursor.Current = Cursors.Default;
            msg = String.Format("Process Completed");
            Utils.LogMessage(Utils.MsgLevel.Info, msg, true, true);
            
            Utils.LogMessage(Utils.MsgLevel.SectionEnd, "", false, true);
        }

        public dsSummaryFcst GetCustEngFcst(string p_CompanyID, Boolean p_SupressCurrentPeriod, Decimal p_NumPeriodsToAvg)
        {

            // 1. Get the historical usage of parts in Custom Eng jobs. Call Stored Procedure dbo.csp_DMGetCustEngUsage
            //string query = "Select period from dbo.PartUsage where partnum = '162-255-001'"; ;
            var watch = System.Diagnostics.Stopwatch.StartNew();
            Utils.LogMessage(Utils.MsgLevel.Info, "Getting Custom Eng Job usage history",false, true);
            Utils.LogMessage(Utils.MsgLevel.Info, string.Format("     Company = {0}, Period = {1}, Periods To Average = {2}" ,p_CompanyID , tbForecastPeriod.Text, nuCustEngPeriodsToAvg.Value),false, true);
            // the code that you want to measure comes here
            DataTable dtCustEngHistory = new DataTable();
            try
            {



                using (var con = new SqlConnection(Properties.Settings.Default.CustomAppsConnStr))
                using (var cmd = new SqlCommand("dbo.csp_DM_GetCustEngUsage", con))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 6000;
                    cmd.Parameters.AddWithValue("@Company", p_CompanyID);
                    cmd.Parameters.AddWithValue("@ForePeriod", Convert.ToInt32(tbForecastPeriod.Text));
                    cmd.Parameters.AddWithValue("@PeriodsToAvg", p_NumPeriodsToAvg);
                    cmd.Parameters.AddWithValue("@CheckRpt", 0);

                    da.Fill(dtCustEngHistory);
                }
            }
            catch (Exception ex)
            {
                Utils.LogMessage(Utils.MsgLevel.Error,"     GetCustEngFcst 1. {0}" + ex.Message,false, true);
            }

            finally
            {
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                Utils.LogMessage(Utils.MsgLevel.Info, string.Format("Completed Getting Custom Eng Job usage history. Retrieved {0} records in {1} seconds", dtCustEngHistory.Rows.Count.ToString(), ((decimal)elapsedMs /1000).ToString("N3")),false, true);
            }

            // Now we want to build a dsSummaryFcst from dtCustEngHistory
            // The 
            dsSummaryFcst summary = new dsSummaryFcst();
            foreach (DataRow row in dtCustEngHistory.Rows)
            {
                Utils.LogMessage(Utils.MsgLevel.Debug, string.Format("     Processing {0}, {1}, {2}, {3}, {4}, {5:0}, {6:N0}, {7:0.00} ", row["Company"], row["Plant"].ToString(), row["PartNum"], row["CustEngFcstMethod_c"].ToString(), row["FirstPeriod"].ToString(), row["NumPeriods"], row["Qty"], row["AvgMonthUse"]), false, true);

                Period tmpPeriod = new Period(Convert.ToInt32(tbForecastPeriod.Text));
                int tmpNumFcsts = 12;
                if (p_SupressCurrentPeriod)
                {
                    tmpPeriod.PeriodAdd(1);
                    tmpNumFcsts = 11;
                }


                // Get Forecast Method
                // Find Company, Plant, PartNum in dsPartPlantMethods - if not found log error
                string querystr = String.Format("Company = '{0}' AND Plant = '{1}' AND PartNum = '{2}'", row[0], row[1], row[2]);
                DataRow[] rowPartPlantFcstMethods = dsPPData.dtPartPlantFcstMethods.Select(querystr);

                if (rowPartPlantFcstMethods.Length < 1)
                {
                    // Error couldn't find a PartPlant record
                }

                // Find Forecast Method parameters
                string qry_ForecastMethods = String.Format("Company = '{0}' AND Key1 = '{1}'", rowPartPlantFcstMethods[0]["Company"], rowPartPlantFcstMethods[0]["CustEngFcstMethod"]);
                DataRow[] rowForecastMethods = dtForecastMethods.Select(qry_ForecastMethods);

                // If the Avg. Monthly Usage is less than 0.75 then we ignore this part
                if (Convert.ToDouble(row["AvgMonthUse"]) < 0.75)
                {
                    if (Debug)
                    {
                        Utils.LogMessage(Utils.MsgLevel.Warning, string.Format("     Average Monthly Usage is less than 0.75. No forecasts will be created for this part."), false, true);
                        continue;
                    }
                }

                for (int periodcount = 0; periodcount < tmpNumFcsts; periodcount++)
                {
                    dsSummaryFcst.dtSummaryFcstRow summaryRow = summary.dtSummaryFcst.NewdtSummaryFcstRow();
                    summaryRow.Company = row["Company"].ToString();
                    summaryRow.Plant = row["Plant"].ToString();
                    summaryRow.PartNum = row["PartNum"].ToString();
                    summaryRow.CustNum = Global.CustEngCustNum;
                    summaryRow.ForePeriod = tmpPeriod.PeriodValue;
                    summaryRow.ForeQty = Convert.ToInt32(row["AvgMonthUse"]);
                    summaryRow.NumFcsts = Convert.ToInt16(rowForecastMethods[0]["Number01"]);
                    summaryRow.FirstDayOfMonth = Convert.ToInt16(rowForecastMethods[0]["Number02"]);
                    summaryRow.DayOfWeek = Convert.ToInt16(StringEnum.Parse(typeof(WeekDay), Convert.ToString(rowForecastMethods[0]["ShortChar01"]), true));

                    summary.dtSummaryFcst.AdddtSummaryFcstRow(summaryRow);

                    tmpPeriod.PeriodAdd(1);
                }
                
                


            }

            return summary;


        }

        public dsSummaryFcst GetUsageFcst(string p_CompanyID, Boolean p_SupressCurrentPeriod, Decimal p_NumPeriodsToAvg)
        {

            // 1. Get the historical usage of parts in Custom Eng jobs. Call Stored Procedure dbo.csp_DMGetCustEngUsage
            
            var watch = System.Diagnostics.Stopwatch.StartNew();
            Utils.LogMessage(Utils.MsgLevel.Info, "Getting Misc. Usage history", false, true);
            Utils.LogMessage(Utils.MsgLevel.Info, string.Format("     Company = {0}, Period = {1}, Periods To Average = {2}", p_CompanyID, tbForecastPeriod.Text, nuCustEngPeriodsToAvg.Value), false, true);
            
            DataTable dtUsageHistory = new DataTable();
            try
            {
                using (var con = new SqlConnection(Properties.Settings.Default.CustomAppsConnStr))
                using (var cmd = new SqlCommand("dbo.csp_DM_GetMiscUsage", con))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 6000;
                    cmd.Parameters.AddWithValue("@Company", p_CompanyID);
                    cmd.Parameters.AddWithValue("@ForePeriod", Convert.ToInt32(tbForecastPeriod.Text));
                    cmd.Parameters.AddWithValue("@PeriodsToAvg", p_NumPeriodsToAvg);
                    
                    da.Fill(dtUsageHistory);
                }
            }
            catch (Exception ex)
            {
                Utils.LogMessage(Utils.MsgLevel.Error, "     GetUsageFcst 1. {0}" + ex.Message, false, true);
            }

            finally
            {
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                Utils.LogMessage(Utils.MsgLevel.Info, string.Format("Completed Getting Misc. Usage history. Retrieved {0} records in {1} seconds", dtUsageHistory.Rows.Count.ToString(), ((decimal)elapsedMs / 1000).ToString("N3")), false, true);
            }

            // Now we want to build a dsSummaryFcst from dtCustEngHistory
            dsSummaryFcst summary = new dsSummaryFcst();
            foreach (DataRow row in dtUsageHistory.Rows)
            {
                Utils.LogMessage(Utils.MsgLevel.Debug, string.Format("     Processing {0}, {1}, {2}, {3}, {4}, {5:0}, {6:N0}, {7:0.00} ", row["Company"], row["Plant"].ToString(), row["PartNum"], row["UsageFcstMethod_c"].ToString(), row["FirstPeriod"].ToString(), row["NumPeriods"], row["Qty"], row["AvgMonthUse"]), false, true);

              
                Period tmpPeriod = new Period(Convert.ToInt32(tbForecastPeriod.Text));
                int tmpNumFcsts = 12;
                if (p_SupressCurrentPeriod)
                {
                    tmpPeriod.PeriodAdd(1);
                    tmpNumFcsts = 11;
                }

                // Get Forecast Method
                // Find Company, Plant, PartNum in dsPartPlantMethods - if not found log error
                string querystr = String.Format("Company = '{0}' AND Plant = '{1}' AND PartNum = '{2}'", row[0], row[1], row[2]);
                DataRow[] rowPartPlantFcstMethods = dsPPData.dtPartPlantFcstMethods.Select(querystr);

                if (rowPartPlantFcstMethods.Length < 1)
                {
                    // Error couldn't find a PartPlant record
                }

                // Find Forecast Method parameters
                string qry_ForecastMethods = String.Format("Company = '{0}' AND Key1 = '{1}'", rowPartPlantFcstMethods[0]["Company"], rowPartPlantFcstMethods[0]["UsageFcstMethod"]);
                DataRow[] rowForecastMethods = dtForecastMethods.Select(qry_ForecastMethods);

                for (int periodcount = 0; periodcount < tmpNumFcsts; periodcount++)
                {
                    dsSummaryFcst.dtSummaryFcstRow summaryRow = summary.dtSummaryFcst.NewdtSummaryFcstRow();
                    summaryRow.Company = row["Company"].ToString();
                    summaryRow.Plant = row["Plant"].ToString();
                    summaryRow.PartNum = row["PartNum"].ToString();
                    summaryRow.CustNum = Global.UsageCustNum;                  
                    summaryRow.ForePeriod = tmpPeriod.PeriodValue;
                    summaryRow.ForeQty = Convert.ToInt32(Convert.ToDecimal(row["Qty"]) / Convert.ToDecimal(row["NumPeriods"])) ;
                    summaryRow.NumFcsts = Convert.ToInt16(rowForecastMethods[0]["Number01"]);
                    summaryRow.FirstDayOfMonth = Convert.ToInt16(rowForecastMethods[0]["Number02"]);
                    summaryRow.DayOfWeek = Convert.ToInt16(StringEnum.Parse(typeof(WeekDay), Convert.ToString(rowForecastMethods[0]["ShortChar01"]), true));

                    summary.dtSummaryFcst.AdddtSummaryFcstRow(summaryRow);

                    tmpPeriod.PeriodAdd(1);
                }


            }

            return summary;


        }
        
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        
        private dsDetailFcst Calc_Forecasts(dsSummaryFcst p_dsSummary)
        {

            /* Here we calculate the individual forecasts to be uploaded to Epicor.
             * These are based on the period total forecasts split into separate dated forecasts based on the forecast method set for each Part/Plant record
             */
            MessageBox.Show("Stop");

            dsDetailFcst dsForecasts = new dsDetailFcst();

            // Process each record in the p_dsSummary dataset

            foreach (dsSummaryFcst.dtSummaryFcstRow SummaryFcstRow in p_dsSummary.dtSummaryFcst)
            {
                // Each record is processed based on its Forecast Method fields

                if (SummaryFcstRow.NumFcsts == 0)       // 0 means we want 1 forecast every week so we use the DayOfWeek value
                {
                    // First we need to fins the number of DayOfWeek occurrances in the period

                }


            }



            return dsForecasts;
        }

        private dsDaysOfWeek GetNumDaysOfWeek(Period FromPeriod, Period ToPeriod)
        {
            dsDaysOfWeek dsDOW = new dsDaysOfWeek();

            Period tmpPeriod = new Period(FromPeriod.PeriodValue);
            int daycount = 0;


            for (tmpPeriod.PeriodSeq = FromPeriod.PeriodSeq; tmpPeriod.PeriodSeq <= ToPeriod.PeriodSeq; tmpPeriod.PeriodAdd(1))
            {

                // First get a date for the current period
                DateTime periodStartDate = new DateTime(tmpPeriod.Year, tmpPeriod.Month, 1);
                DateTime periodEndDate = new DateTime(tmpPeriod.Year, tmpPeriod.Month, 1).AddMonths(1);
                int DaysBetween = (int)(periodEndDate - periodStartDate).TotalDays;



                for (int j = 0; j < 7; j++)
                {
                    int day = 0;
                    int tmp = (int)periodStartDate.DayOfWeek;
                    int tmp2 = j - tmp;


                    // Calc date of month for first x day
                    if (tmp2 < 0)
                    {
                        day = tmp2 + 7 + 1;
                        daycount = (DaysBetween - day) / 7 + 1;

                    }
                    if (tmp2 >= 0)
                    {
                        day = tmp2 + 1;
                        daycount = (DaysBetween - day) / 7 + 1;
                    }
                    dsDaysOfWeek.dtDaysOfWeekRow row = dsDOW.dtDaysOfWeek.NewdtDaysOfWeekRow();
                    row.Period = tmpPeriod.PeriodValue;
                    row.DayOfWeek = Convert.ToInt16(j);
                    row.DayCount = Convert.ToInt16(daycount);
                    dsDOW.dtDaysOfWeek.AdddtDaysOfWeekRow(row);
                }
            }

            //DataRow[] thisRow = dsDOW.dtDaysOfWeek.Select(String.Format("Period = '{0}' AND DayOfWeek = '{1}'", 201805, 3));
            return dsDOW;
        }

        private DateTime GetDateOfDay(Period p_period, DayOfWeek p_dayofweek)
        {
            DateTime d_date = new DateTime(p_period.Year, p_period.Month, 1);

            while (d_date.DayOfWeek != p_dayofweek)
            {
                d_date = d_date.AddDays(1);
            }
            return d_date;
        }
        private dsDetailFcst CalculateForecasts(dsSummaryFcst dsSummary, int p_FirstPeriod, int p_LastPeriod)
        {

            var functionwatch = System.Diagnostics.Stopwatch.StartNew();
            Utils.LogMessage(Utils.MsgLevel.Debug, string.Format("Called CalculateForecasts - dsSummary = p_FirstPeriod = {0}, {1} p_LastPeriod = ", p_FirstPeriod, p_LastPeriod), false, true);


            int i_numFcsts;

            int i_DayOfWeek = 0;
            DateTime d_FcstDate;

            dsDetailFcst dsDetail = new dsDetailFcst();

            foreach (dsSummaryFcst.dtSummaryFcstRow SummaryRow in dsSummary.dtSummaryFcst)
            {
                // Each record is processed based on its Forecast Method fields

                /*
                if (SummaryRow.NumFcsts == 0)
                {
                    // Get values we need to use from SummaryRow
                    Period i_Period = new Period(SummaryRow.ForePeriod);
                    // Insert error check here
                    // Period cannot be less than start period nor greater than start period + num periods
                    if (i_Period.PeriodValue < p_FirstPeriod || i_Period.PeriodValue > p_LastPeriod)
                    {
                        Utils.LogMessage(Utils.MsgLevel.Warning, String.Format("Forecast period of {0} is outside the selected range {1} to {2}", i_Period.PeriodValue, p_FirstPeriod, p_LastPeriod), false, true);
                        continue;
                    }


                    i_DayOfWeek = SummaryRow.DayOfWeek - 1;  // Need to convert to 0 = Sun .... 6 = Sat

                    // Get the number of forecasts required for this period and day of the week
                    var daysQuery = (from rowData in dsDOW.dtDaysOfWeek.AsEnumerable()
                                     where rowData.Period == i_Period.PeriodValue
                                           && rowData.DayOfWeek == i_DayOfWeek
                                     select rowData).FirstOrDefault();

                    i_numFcsts = daysQuery.DayCount;


                    
                    // Calculate the quantity fior each forecast in the month
                    int i_TotFcstQty, i_FcstQty, i_1stFcstQty;
                    i_TotFcstQty = SummaryRow.ForeQty;
                    // 23 Jun 2018 DJP Change to ROUND function
                    //i_FcstQty = i_TotFcstQty / i_numFcsts;
                    i_FcstQty = Convert.ToInt32(Math.Round(Convert.ToDecimal((i_TotFcstQty / i_numFcsts))));
                    i_1stFcstQty = i_TotFcstQty - (i_FcstQty * (i_numFcsts - 1));

                    
                    // Get the date for the first forecast
                    d_FcstDate = GetDateOfDay(i_Period, (DayOfWeek)i_DayOfWeek);
                    if (i_1stFcstQty > 0)
                    {
                        if (Debug)
                        {
                            string msg = String.Format("Processing     : {0}  {1}  {2}  {3}  {4}  {5}  {6}  {7}  {8}", SummaryRow.Company, SummaryRow.Plant, SummaryRow.PartNum, SummaryRow.CustNum, SummaryRow.ForePeriod, SummaryRow.ForeQty, SummaryRow.NumFcsts, SummaryRow.FirstDayOfMonth, ((WeekDay)SummaryRow.DayOfWeek).ToString());
                            Utils.LogMessage(Utils.MsgLevel.Debug, msg, false, true);
                        }

                        dsDetailFcst.dtDetailFcstRow detailRow = dsDetail.dtDetailFcst.NewdtDetailFcstRow();
                        detailRow.Company = SummaryRow.Company;
                        detailRow.Plant = SummaryRow.Plant;
                        detailRow.PartNum = SummaryRow.PartNum;
                        detailRow.CustNum = SummaryRow.CustNum;
                        detailRow.ForeDate = d_FcstDate;
                        detailRow.ForeQty = i_1stFcstQty;
                        dsDetail.dtDetailFcst.AdddtDetailFcstRow(detailRow);

                        if (Debug)
                        {
                            string msg = String.Format("Create forecast: {0}  {1}  {2}  {3}  {4}  {5}  ", SummaryRow.Company, SummaryRow.Plant, SummaryRow.PartNum, SummaryRow.CustNum, d_FcstDate.ToString("dd-MM-yyy"), i_1stFcstQty);
                            Utils.LogMessage(Utils.MsgLevel.Debug, msg, false, true);
                        }


                    }

                    for (int i = 1; i < i_numFcsts; i++)
                    {
                        int tmpFcstQty = i_TotFcstQty - i_1stFcstQty - (i_FcstQty * (i - 1));
                        d_FcstDate = d_FcstDate.AddDays(7);
                        if (tmpFcstQty > 0)
                        {
                            dsDetailFcst.dtDetailFcstRow detailRow = dsDetail.dtDetailFcst.NewdtDetailFcstRow();
                            detailRow.Company = SummaryRow.Company;
                            detailRow.Plant = SummaryRow.Plant;
                            detailRow.PartNum = SummaryRow.PartNum;
                            detailRow.CustNum = SummaryRow.CustNum;
                            detailRow.ForeDate = d_FcstDate;
                            detailRow.ForeQty = i_FcstQty;   // tmpFcstQty;
                            dsDetail.dtDetailFcst.AdddtDetailFcstRow(detailRow);
                            if (Debug)
                            {
                                string msg = String.Format("Create forecast: {0}  {1}  {2}  {3}  {4}  {5}  ", SummaryRow.Company, SummaryRow.Plant, SummaryRow.PartNum, SummaryRow.CustNum, d_FcstDate.ToString("dd-MM-yyy"), i_FcstQty);
                                Utils.LogMessage(Utils.MsgLevel.Debug, msg, false, true);
                            }
                        }

                    }




                }
                else
                {
                    // Get values we need to use from SummaryRow
                    Period i_Period = new Period(SummaryRow.ForePeriod);

                    i_numFcsts = SummaryRow.NumFcsts;

                    // Calculate the quantity for each forecast in the month
                    int i_TotFcstQty, i_FcstQty, i_1stFcstQty;
                    i_TotFcstQty = SummaryRow.ForeQty;
                    i_FcstQty = (int)Math.Round((decimal)i_TotFcstQty / (decimal)i_numFcsts);
                    i_1stFcstQty = i_TotFcstQty - (i_FcstQty * (i_numFcsts - 1));

                    // Get the date for the first forecast
                    d_FcstDate = new DateTime(i_Period.Year, i_Period.Month, SummaryRow.FirstDayOfMonth);
                    if (i_1stFcstQty > 0)
                    {
                        if (Debug)
                        {
                            string msg = String.Format("Processing     : {0}  {1}  {2}  {3}  {4}  {5}  {6}  {7}  {8}", SummaryRow.Company, SummaryRow.Plant, SummaryRow.PartNum, SummaryRow.CustNum, SummaryRow.ForePeriod, SummaryRow.ForeQty,SummaryRow.NumFcsts, SummaryRow.FirstDayOfMonth, ((WeekDay)SummaryRow.DayOfWeek).ToString());
                            Utils.LogMessage(Utils.MsgLevel.Debug, msg, false, true);
                        }
                        dsDetailFcst.dtDetailFcstRow detailRow = dsDetail.dtDetailFcst.NewdtDetailFcstRow();
                        detailRow.Company = SummaryRow.Company;
                        detailRow.Plant = SummaryRow.Plant;
                        detailRow.PartNum = SummaryRow.PartNum;
                        detailRow.CustNum = SummaryRow.CustNum;
                        detailRow.ForeDate = d_FcstDate;
                        detailRow.ForeQty = i_1stFcstQty;
                        dsDetail.dtDetailFcst.AdddtDetailFcstRow(detailRow);
                        if (Debug)
                        {
                            string msg = String.Format("Create forecast: {0}  {1}  {2}  {3}  {4}  {5}  ", SummaryRow.Company, SummaryRow.Plant, SummaryRow.PartNum, SummaryRow.CustNum, d_FcstDate.ToString("dd-MM-yyy"), i_1stFcstQty);
                            Utils.LogMessage(Utils.MsgLevel.Debug, msg, false, true);
                        }
                    }

                    for (int i = 1; i < i_numFcsts; i++)
                    {
                        int tmpFcstQty = i_TotFcstQty - i_1stFcstQty - (i_FcstQty * (i - 1));
                        d_FcstDate = d_FcstDate.AddDays(28 / i_numFcsts);
                        if (tmpFcstQty > 0)
                        {
                            dsDetailFcst.dtDetailFcstRow detailRow = dsDetail.dtDetailFcst.NewdtDetailFcstRow();
                            detailRow.Company = SummaryRow.Company;
                            detailRow.Plant = SummaryRow.Plant;
                            detailRow.PartNum = SummaryRow.PartNum;
                            detailRow.CustNum = SummaryRow.CustNum;
                            detailRow.ForeDate = d_FcstDate;
                            detailRow.ForeQty = tmpFcstQty;
                            dsDetail.dtDetailFcst.AdddtDetailFcstRow(detailRow);

                            string msg = String.Format("Create forecast: {0}  {1}  {2}  {3}  {4}  {5}  ", SummaryRow.Company, SummaryRow.Plant, SummaryRow.PartNum, SummaryRow.CustNum, d_FcstDate.ToString("dd-MM-yyy"), i_FcstQty);
                            Utils.LogMessage(Utils.MsgLevel.Debug, msg, false, true);
                        }

                    }

                }
                */
                if (SummaryRow.NumFcsts == 0)
                {
                    // Get values we need to use from SummaryRow
                    Period i_Period = new Period(SummaryRow.ForePeriod);
                    
                    // Period cannot be less than start period nor greater than start period + num periods
                    if (i_Period.PeriodValue < p_FirstPeriod || i_Period.PeriodValue > p_LastPeriod)
                    {
                        Utils.LogMessage(Utils.MsgLevel.Warning, String.Format("Forecast period of {0} is outside the selected range {1} to {2}", i_Period.PeriodValue, p_FirstPeriod, p_LastPeriod), false, true);
                        continue;
                    }


                    i_DayOfWeek = SummaryRow.DayOfWeek - 1;  // Need to convert to 0 = Sun .... 6 = Sat

                    // Get the number of forecasts required for this period and day of the week
                    var daysQuery = (from rowData in dsDOW.dtDaysOfWeek.AsEnumerable()
                                     where rowData.Period == i_Period.PeriodValue
                                           && rowData.DayOfWeek == i_DayOfWeek
                                     select rowData).FirstOrDefault();

                    i_numFcsts = daysQuery.DayCount;

                    // Here we need to modify the number of forecasts for the months of December and January
                    // TZNZ:  Reduce December by 1 week (last 1), reduce January by 2 weeks (first 2)
                    // TZA:   Reduce December by 1 week (last 1), reduce January by 1 week (first 1)
                    switch (Global.CompanyID)
                    {
                        case "TZA":
                            if (i_Period.Month == 12 && i_numFcsts > 1)
                                i_numFcsts = i_numFcsts - 1;
                            if (i_Period.Month == 1 && i_numFcsts > 1)
                                i_numFcsts = i_numFcsts - 1;
                            break;
                        case "TZNZ":
                            if (i_Period.Month == 12 && i_numFcsts > 1)
                                i_numFcsts = i_numFcsts - 1;
                            if (i_Period.Month == 1 && i_numFcsts > 1)
                                i_numFcsts = i_numFcsts - 2;
                            break;
                    }

                    // Calculate the quantity for each forecast in the month
                    int i_TotFcstQty, i_FcstQty, i_1stFcstQty;
                    i_TotFcstQty = SummaryRow.ForeQty;
                    
                    i_FcstQty = Convert.ToInt32(Math.Round((double)i_TotFcstQty / i_numFcsts,MidpointRounding.AwayFromZero));
                    i_1stFcstQty = i_TotFcstQty - (i_FcstQty * (i_numFcsts - 1));


                    // Get the date for the first forecast
                    d_FcstDate = GetDateOfDay(i_Period, (DayOfWeek)i_DayOfWeek);

                    switch (Global.CompanyID)
                    {
                        case "TZA":                       
                            if (i_Period.Month == 1)
                                d_FcstDate = d_FcstDate.AddDays(7);
                            break;
                        case "TZNZ":
                            if (i_Period.Month == 1)
                                d_FcstDate = d_FcstDate.AddDays(14);
                            break;
                    }
                    if (i_1stFcstQty > 0)
                    {
                        if (Debug)
                        {
                            string msg = String.Format("Processing     : {0}  {1}  {2}  {3}  {4}  {5}  {6}  {7}  {8}", SummaryRow.Company, SummaryRow.Plant, SummaryRow.PartNum, SummaryRow.CustNum, SummaryRow.ForePeriod, SummaryRow.ForeQty, SummaryRow.NumFcsts, SummaryRow.FirstDayOfMonth, ((WeekDay)SummaryRow.DayOfWeek).ToString());
                            Utils.LogMessage(Utils.MsgLevel.Debug, msg, false, true);
                        }

                        dsDetailFcst.dtDetailFcstRow detailRow = dsDetail.dtDetailFcst.NewdtDetailFcstRow();
                        detailRow.Company = SummaryRow.Company;
                        detailRow.Plant = SummaryRow.Plant;
                        detailRow.PartNum = SummaryRow.PartNum;
                        detailRow.CustNum = SummaryRow.CustNum;
                        detailRow.ForeDate = d_FcstDate;
                        detailRow.ForeQty = i_1stFcstQty;
                        dsDetail.dtDetailFcst.AdddtDetailFcstRow(detailRow);

                        if (Debug)
                        {
                            string msg = String.Format("Create forecast: {0}  {1}  {2}  {3}  {4}  {5}  ", SummaryRow.Company, SummaryRow.Plant, SummaryRow.PartNum, SummaryRow.CustNum, d_FcstDate.ToString("dd-MM-yyy"), i_1stFcstQty);
                            Utils.LogMessage(Utils.MsgLevel.Debug, msg, false, true);
                        }


                    }
                    else
                    {
                        i_1stFcstQty = 0;
                    }


                    for (int i = 1; i < i_numFcsts; i++)
                    {
                        int tmpFcstQty = i_TotFcstQty - i_1stFcstQty - (i_FcstQty * (i - 1));
                        d_FcstDate = d_FcstDate.AddDays(7);
                        if (tmpFcstQty > 0)
                        {
                            dsDetailFcst.dtDetailFcstRow detailRow = dsDetail.dtDetailFcst.NewdtDetailFcstRow();
                            detailRow.Company = SummaryRow.Company;
                            detailRow.Plant = SummaryRow.Plant;
                            detailRow.PartNum = SummaryRow.PartNum;
                            detailRow.CustNum = SummaryRow.CustNum;
                            detailRow.ForeDate = d_FcstDate;
                            detailRow.ForeQty = i_FcstQty;   // tmpFcstQty;
                            dsDetail.dtDetailFcst.AdddtDetailFcstRow(detailRow);
                            if (Debug)
                            {
                                string msg = String.Format("Create forecast: {0}  {1}  {2}  {3}  {4}  {5}  ", SummaryRow.Company, SummaryRow.Plant, SummaryRow.PartNum, SummaryRow.CustNum, d_FcstDate.ToString("dd-MM-yyy"), i_FcstQty);
                                Utils.LogMessage(Utils.MsgLevel.Debug, msg, false, true);
                            }
                        }

                    }




                }
                else
                {
                    // Get values we need to use from SummaryRow
                    Period i_Period = new Period(SummaryRow.ForePeriod);

                    i_numFcsts = SummaryRow.NumFcsts;

                    // Calculate the quantity for each forecast in the month
                    int i_TotFcstQty, i_FcstQty, i_1stFcstQty;
                    i_TotFcstQty = SummaryRow.ForeQty;
                    i_FcstQty = (int)Math.Round((decimal)i_TotFcstQty / (decimal)i_numFcsts);
                    i_1stFcstQty = i_TotFcstQty - (i_FcstQty * (i_numFcsts - 1));

                    // Get the date for the first forecast
                    d_FcstDate = new DateTime(i_Period.Year, i_Period.Month, SummaryRow.FirstDayOfMonth);
                    if (i_1stFcstQty > 0)
                    {
                        if (Debug)
                        {
                            string msg = String.Format("Processing     : {0}  {1}  {2}  {3}  {4}  {5}  {6}  {7}  {8}", SummaryRow.Company, SummaryRow.Plant, SummaryRow.PartNum, SummaryRow.CustNum, SummaryRow.ForePeriod, SummaryRow.ForeQty, SummaryRow.NumFcsts, SummaryRow.FirstDayOfMonth, ((WeekDay)SummaryRow.DayOfWeek).ToString());
                            Utils.LogMessage(Utils.MsgLevel.Debug, msg, false, true);
                        }
                        dsDetailFcst.dtDetailFcstRow detailRow = dsDetail.dtDetailFcst.NewdtDetailFcstRow();
                        detailRow.Company = SummaryRow.Company;
                        detailRow.Plant = SummaryRow.Plant;
                        detailRow.PartNum = SummaryRow.PartNum;
                        detailRow.CustNum = SummaryRow.CustNum;
                        detailRow.ForeDate = d_FcstDate;
                        detailRow.ForeQty = i_1stFcstQty;
                        dsDetail.dtDetailFcst.AdddtDetailFcstRow(detailRow);
                        if (Debug)
                        {
                            string msg = String.Format("Create forecast: {0}  {1}  {2}  {3}  {4}  {5}  ", SummaryRow.Company, SummaryRow.Plant, SummaryRow.PartNum, SummaryRow.CustNum, d_FcstDate.ToString("dd-MM-yyy"), i_1stFcstQty);
                            Utils.LogMessage(Utils.MsgLevel.Debug, msg, false, true);
                        }
                    }

                    for (int i = 1; i < i_numFcsts; i++)
                    {
                        int tmpFcstQty = i_TotFcstQty - i_1stFcstQty - (i_FcstQty * (i - 1));
                        d_FcstDate = d_FcstDate.AddDays(28 / i_numFcsts);
                        if (tmpFcstQty > 0)
                        {
                            dsDetailFcst.dtDetailFcstRow detailRow = dsDetail.dtDetailFcst.NewdtDetailFcstRow();
                            detailRow.Company = SummaryRow.Company;
                            detailRow.Plant = SummaryRow.Plant;
                            detailRow.PartNum = SummaryRow.PartNum;
                            detailRow.CustNum = SummaryRow.CustNum;
                            detailRow.ForeDate = d_FcstDate;
                            detailRow.ForeQty = tmpFcstQty;
                            dsDetail.dtDetailFcst.AdddtDetailFcstRow(detailRow);

                            if (Debug)
                            {
                                string msg = String.Format("Create forecast: {0}  {1}  {2}  {3}  {4}  {5}  ", SummaryRow.Company, SummaryRow.Plant, SummaryRow.PartNum, SummaryRow.CustNum, d_FcstDate.ToString("dd-MM-yyy"), i_FcstQty);
                                Utils.LogMessage(Utils.MsgLevel.Debug, msg, false, true);
                            }
                        }

                    }

                }



            }
            functionwatch.Stop();
            var functionelapsedMs = functionwatch.ElapsedMilliseconds;
            string msgstr = String.Format("Returning from CalculateForecasts - {0} seconds", (functionelapsedMs / 1000).ToString("N3"));
            Utils.LogMessage(Utils.MsgLevel.Info, msgstr, false, true);
            return dsDetail;
        }

        private dsPartPlantFcstMethods GetPartPlantFcstMethods(string p_CompanyID)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();


            //DataTable dtPartPlantData = new DataTable();
            dsPartPlantFcstMethods dsPPData = new dsPartPlantFcstMethods();
            try
            {
                using (var con = new SqlConnection(Properties.Settings.Default.CustomAppsConnStr))
                using (var cmd = new SqlCommand("dbo.csp_DM_GetPlanningData", con))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 6000;
                    cmd.Parameters.AddWithValue("@Company", p_CompanyID);
                    da.Fill(dsPPData.dtPartPlantFcstMethods);
                }
            }
            catch (Exception ex)
            {
                Utils.LogMessage(Utils.MsgLevel.Error, string.Format("     GetPartPlantFcstMethods 1. {0}", ex.Message), true, true);
            }

            finally
            {
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                Utils.LogMessage(Utils.MsgLevel.Info, string.Format("Completed Getting Part Plant Forecast Methods Data. Retrieved {0} records in {1} seconds", dsPPData.dtPartPlantFcstMethods.Rows.Count.ToString(), ((decimal)elapsedMs / 1000).ToString("N3")), false, true);
            }


            return dsPPData;
        }

        private dsSummaryFcst GetSMFFcst(string p_CompanyID)
        {
            
            var functionwatch = System.Diagnostics.Stopwatch.StartNew();
            if (Debug) Utils.LogMessage(Utils.MsgLevel.Debug, string.Format("Called GetSMFFcst - {0}", p_CompanyID), false, true);
            string msgstr;
            dsSummaryFcst dsSMFtmp = new dsSummaryFcst();
            dsSummaryFcst dsSMFSummary = new dsSummaryFcst();

            DataTable dtcsvdata = new DataTable();
            //dtcsvdata = ReadCsvFile(@"c:\temp\SMFDemand.csv");
            var ReadCSVwatch = System.Diagnostics.Stopwatch.StartNew();
            Utils.LogMessage(Utils.MsgLevel.Info, String.Format("Reading CSV file {0}", tbImportFile.Text), false, true);
            try
            {
                dtcsvdata = GetDataTableFromSmartforecastsCSV(p_CompanyID, tbImportFile.Text);
            }
            catch (Exception ex)
            {
                Utils.LogMessage(Utils.MsgLevel.Error, String.Format("Failed to read CSV file {0}. Eception: {1}" , tbImportFile.Text, ex.Message), true, true);
            }
            finally
            {
                var ReadCSVelapsedMs = ReadCSVwatch.ElapsedMilliseconds;
                //string CSVmsgstr = String.Format("Read {0} records from {1} in {2} seconds.", dtcsvdata.Rows.Count, tbImportFile.Text,(ReadCSVelapsedMs / Convert.ToDecimal(1000)).ToString("N3"));
                Utils.LogMessage(Utils.MsgLevel.Info, String.Format("Read {0} records from {1} in {2} seconds.", dtcsvdata.Rows.Count, tbImportFile.Text, (ReadCSVelapsedMs / Convert.ToDecimal(1000)).ToString("N3")), false, true);

            }


            // Preprocess the datatable to identify and report Inactive parts that have been included in forecasts from SmartForecasts
            //DataView view = new DataView(dtcsvdata);
            //DataTable InactiveParts = view.ToTable(true, "Company", "PartNum","Inactive");

            var watch = System.Diagnostics.Stopwatch.StartNew();

            var Parts = (from s in dtcsvdata.AsEnumerable()
                         join p in dsPPData.dtPartPlantFcstMethods on s.Field<string>("PartNum") equals p.PartNum
                         where p.Inactive == true
                         select new { p.PartNum }).Distinct();

            // log error here for inactive parts
            int inactivecount = 0;
            if (Parts != null)
            {
                foreach (var row in Parts.OrderBy(x => x.PartNum))
                {
                    inactivecount++;
                    //string msg = String.Format("Part {0} is Inactive. ", row.PartNum);
                    Utils.LogMessage(Utils.MsgLevel.Warning, String.Format("Part {0} is Inactive. ", row.PartNum),  false, true);
                }   
            }

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            //string msgstr = String.Format("Completed checking for inactive Parts. {0} inactive Parts were found in  {1} seconds." , inactivecount, (elapsedMs / Convert.ToDecimal(1000)).ToString("N3"));
            Utils.LogMessage(Utils.MsgLevel.Info, String.Format("Completed checking for inactive Parts. {0} inactive Parts were found in  {1} seconds.", inactivecount, (elapsedMs / Convert.ToDecimal(1000)).ToString("N3")), false, true);

            // Preprocess the datatable to identify and report PartPlant records that do not exist but have been included in forecasts from SmartForecasts
            //DataView view = new DataView(dtcsvdata);
            //DataTable InactiveParts = view.ToTable(true, "Company", "PartNum","Inactive");

            var ppwatch = System.Diagnostics.Stopwatch.StartNew();

            var PartPlant = (from ps in dtcsvdata.AsEnumerable()
                             join pp2 in dsPPData.dtPartPlantFcstMethods.AsEnumerable()
                                 on new
                                 {
                                     PartNum = ps.Field<string>("PartNum"),
                                     Plant = ps.Field<string>("Plant")
                                 }
                                 equals new
                                 {
                                     PartNum = pp2.Field<string>("PartNum"),
                                     Plant = pp2.Field<string>("Plant")
                                 }


                              into outer
                             from leftJoin in outer.DefaultIfEmpty()
                             //where ps.Field<int>("")
                             select new
                             {
                                 PartNum = ps.Field<string>("PartNum"),
                                 Plant = ps.Field<string>("Plant"),
                                 Valid = (leftJoin == null) ? 0 : 1,
                                 SalesFcst = (leftJoin == null) ? false : leftJoin.Field<Boolean>("SalesFcst"),
                                 SalesFcstMethod = (leftJoin == null) ? "" : leftJoin.Field<string>("SalesFcstMethod")
                             }).Distinct();

            
            int invalidcount = 0;

            
            if (PartPlant != null)
            {
                foreach (var row in PartPlant.OrderBy(x =>  x.PartNum).ThenBy(x => x.Plant))
                {
                    invalidcount++;
                    if (row.Valid == 0)
                    //string msg = String.Format("Part {0} is Inactive. ", row.PartNum);
                    Utils.LogMessage(Utils.MsgLevel.Warning, String.Format("A valid Sales Forecast planning method could not be found for {0}     - {1} ", row.PartNum, row.Plant), false, true);
                }
            }

            watch.Stop();
            var ppelapsedMs = ppwatch.ElapsedMilliseconds;
            //string msgstr = String.Format("Completed checking for inactive Parts. {0} inactive Parts were found in  {1} seconds." , inactivecount, (elapsedMs / Convert.ToDecimal(1000)).ToString("N3"));
            Utils.LogMessage(Utils.MsgLevel.Info, String.Format("Completed checking for inactive Parts. {0} inactive Parts were found in  {1} seconds.", inactivecount, (ppelapsedMs / Convert.ToDecimal(1000)).ToString("N3")), false, true);

            /*
            // Preprocess the datatable to identify and report PartPlant records that do exist but do not have a SalesFcst_c = true and/or 
            // SalesFcstMNethod_c is blank BUT have been included in forecasts from SmartForecasts
            //DataView view = new DataView(dtcsvdata);
            //DataTable InactiveParts = view.ToTable(true, "Company", "PartNum","Inactive");

            var fcwatch = System.Diagnostics.Stopwatch.StartNew();

            var PartPlantFC = (from ps in dtcsvdata.AsEnumerable()
                               join pp2 in dsPPData.dtPartPlantFcstMethods.AsEnumerable()
                                 on new
                                 {
                                     PartNum = ps.Field<string>("PartNum"),
                                     Plant = ps.Field<string>("Plant")
                                 }
                                 equals new
                                 {
                                     PartNum = pp2.Field<string>("PartNum"),
                                     Plant = pp2.Field<string>("Plant")
                                 }

                             select new
                             {
                                 PartNum = ps.Field<string>("PartNum"),
                                 Plant = ps.Field<string>("Plant")
                                
                             }).Distinct();

            // log error here for inactive parts
            int invalidfccount = 0;


            if (PartPlant != null)
            {
                foreach (var row in PartPlant.OrderBy(x => x.PartNum).ThenBy(x => x.Plant))
                {
                    invalidfccount++;
                    if (row.Valid != 0 )
                        //string msg = String.Format("Part {0} is Inactive. ", row.PartNum);
                        Utils.LogMessage(Utils.MsgLevel.Warning, String.Format("Sales Forecast and/or Planning Method not set for  {0}     - {1} ", row.PartNum, row.Plant), false, true);
                }
            }

            watch.Stop();
            var fcelapsedMs = ppwatch.ElapsedMilliseconds;
            //string msgstr = String.Format("Completed checking for inactive Parts. {0} inactive Parts were found in  {1} seconds." , inactivecount, (elapsedMs / Convert.ToDecimal(1000)).ToString("N3"));
            Utils.LogMessage(Utils.MsgLevel.Info, String.Format("Completed checking for inactive Parts. {0} inactive Parts were found in  {1} seconds.", invalidfccount, (fcelapsedMs / Convert.ToDecimal(1000)).ToString("N3")), false, true);

            */

            // First we can update the incoming data with the Planning Plant
            //msgstr = String.Format("Updating Planning Site.");
            Utils.LogMessage(Utils.MsgLevel.Info, String.Format("Updating Planning Site."), false, true);
            watch.Restart();
            int errcount = 0;



            using (new Utils.TimerLogger("Update Planning Site in GetSMFFcst"))
            {
                var result = (from csv in dtcsvdata.AsEnumerable()
                              join pp in PartPlant.AsEnumerable()
                              on new
                              {
                                  PartNum = csv.Field<string>("PartNum"),
                                  Plant = csv.Field<string>("Plant")
                              }
                              equals new
                              {
                                  pp.PartNum,
                                  pp.Plant
                              }
                              select new
                              {
                                  Company = csv.Field<string>("Company"),
                                  PartNum = csv.Field<string>("PartNum"),
                                  Plant = csv.Field<string>("Plant"),
                                  ForePeriod = csv.Field<int>("ForePeriod"),
                                  ForeQty = csv.Field<int>("ForeQty"),
                                  pp.Valid
                              });




                foreach (var r in result)
                {
                    

                }





                foreach (DataRow row in dtcsvdata.Rows)
                {
                    string querystr = String.Format("Company = '{0}' AND Plant = '{1}' AND PartNum = '{2}'", row["Company"], row["Plant"], row["PartNum"]);
                    DataRow[] rowPartPlantFcstMethods = dsPPData.dtPartPlantFcstMethods.Select(querystr);

                    if (rowPartPlantFcstMethods.Length > 0)
                    {
                        if (Convert.ToBoolean(rowPartPlantFcstMethods[0]["Inactive"]) != true)
                        {
                            if (rowPartPlantFcstMethods[0]["PlanningPlant"].ToString() != string.Empty)
                            {
                                row["Plant"] = rowPartPlantFcstMethods[0]["PlanningPlant"].ToString();
                            }
                        }
                    }
                    else
                    {
                        // No need to log error
                        //Utils.LogMessage(Utils.MsgLevel.Warning, String.Format("No Part Plant Forecast Methods found for {0} {1} {2}. ", row["Company"], row["Plant"], row["PartNum"]), false, true);
                        errcount++;
                    }

                }
            }
            watch.Stop();
            elapsedMs = watch.ElapsedMilliseconds;
            //msgstr = String.Format("Completed updating Planning Site in {0} seconds. {1} errors identified.", (elapsedMs / Convert.ToDecimal(1000)).ToString("N3"), errcount.ToString());
            Utils.LogMessage(Utils.MsgLevel.Debug, String.Format("Completed updating Planning Site in {0} seconds. {1} errors identified.", (elapsedMs / Convert.ToDecimal(1000)).ToString("N3"), errcount.ToString()), false, true);

            // Next we group and sum




            // At this point we have an untyped datatable with plant updated to Planning Plant where approriate

            // Now we need to map the SmartForecasts data to dtSummaryFcst
            msgstr = String.Format("Generating dtSummaryFcst table.");
            if (Debug) Utils.LogMessage(Utils.MsgLevel.Debug, msgstr, false, true);
            watch.Restart();
            errcount = 0;

            if (dtcsvdata.Rows.Count > 0)
            {
                Utils.LogMessage(Utils.MsgLevel.Info, "Read " + dtcsvdata.Rows.Count.ToString() + " records from " + @"y:\temp\TZA_Jul2018_2.csv", false, true);

                using (new Utils.TimerLogger("Generate dtSummary table in GetSMFFcst"))
                {
                    foreach (DataRow row in dtcsvdata.Rows)
                    {

                        // Read each row
                        // Find Company, Plant, PartNum in dsPartPlantMethods - if not found log error
                        string querystr = String.Format("Company = '{0}' AND Plant = '{1}' AND PartNum = '{2}'", row["Company"], row["Plant"], row["PartNum"]);
                        DataRow[] rowPartPlantFcstMethods = dsPPData.dtPartPlantFcstMethods.Select(querystr);

                        if (rowPartPlantFcstMethods.Length > 0)
                        {
                            if (Convert.ToBoolean(rowPartPlantFcstMethods[0]["Inactive"]) != true)
                            {
                                if (rowPartPlantFcstMethods[0][6].ToString() != String.Empty)
                                {
                                    // Find Forecast Method parameters
                                    string qry_ForecastMethods = String.Format("Company = '{0}' AND Key1 = '{1}'", rowPartPlantFcstMethods[0][0], rowPartPlantFcstMethods[0][6]);
                                    DataRow[] rowForecastMethods = dtForecastMethods.Select(qry_ForecastMethods);

                                    // Create new forecast data record with appropriate planning plant
                                    dsSummaryFcst.dtSummaryFcstRow summaryRow = dsSMFSummary.dtSummaryFcst.NewdtSummaryFcstRow();
                                    summaryRow.Company = row["Company"].ToString();
                                    summaryRow.Plant = row["Plant"].ToString();
                                    summaryRow.PartNum = row["PartNum"].ToString();
                                    summaryRow.CustNum = 0;
                                    summaryRow.ForePeriod = Convert.ToInt32(row["ForePeriod"]);
                                    summaryRow.ForeQty = Convert.ToInt32(row["ForeQty"]);
                                    summaryRow.NumFcsts = Convert.ToInt16(rowForecastMethods[0]["Number01"]);
                                    summaryRow.FirstDayOfMonth = Convert.ToInt16(rowForecastMethods[0]["Number02"]);
                                    summaryRow.DayOfWeek = Convert.ToInt16(StringEnum.Parse(typeof(WeekDay), Convert.ToString(rowForecastMethods[0]["ShortChar02"]), true));



                                    dsSMFSummary.dtSummaryFcst.AdddtSummaryFcstRow(summaryRow);
                                }
                                else
                                {
                                    //Utils.LogMessage(Utils.MsgLevel.Warning, String.Format("Part Plant Forecast Methods not valid for {0} {1} {2} ", row["Company"], row["Plant"], row["PartNum"]), false, true);
                                }
                            }
                        }
                        else
                        {
                            //Utils.LogMessage(Utils.MsgLevel.Warning, String.Format("Part Plant Forecast Methods not found for {0} {1} {2} Part may be Inactive.", row["Company"], row["Plant"], row["PartNum"]), false, true);

                        }


                        // Summarize on Company, Plant, PartNum, Period

                        // Create new dtSummary Fcst record

                    }

                }
                // Summarize the datatable

                using (new Utils.TimerLogger("Summarize dtSummary table in GetSMFFcst"))
                {

                    var Result = (from s in dsSMFSummary.dtSummaryFcst

                                  group s by new { s.Company, s.Plant, s.PartNum, s.CustNum, s.ForePeriod, s.NumFcsts, s.FirstDayOfMonth, s.DayOfWeek } into g

                                  select new { g.Key.Company, g.Key.Plant, g.Key.PartNum, g.Key.CustNum, g.Key.ForePeriod, ForeQty = g.Sum(s => s.ForeQty), g.Key.NumFcsts, g.Key.FirstDayOfMonth, g.Key.DayOfWeek }).OrderBy(i => i.Plant).ThenBy(i => i.PartNum);

                    dsSummaryFcst result = new dsSummaryFcst();



                    foreach (var item in Result)
                    {

                        result.dtSummaryFcst.Rows.Add(item.Company, item.Plant, item.PartNum, item.CustNum, item.ForePeriod, item.ForeQty, item.NumFcsts, item.FirstDayOfMonth, item.DayOfWeek);
                    }

                    functionwatch.Stop();
                    var functionelapsedMs = functionwatch.ElapsedMilliseconds;
                    msgstr = String.Format("Returning from GetSMFFcst - {0} seconds", (functionelapsedMs / 1000).ToString("N3"));
                    Utils.LogMessage(Utils.MsgLevel.Info, msgstr, false, true);
                    return result;
                }
            }

            watch.Stop();
            elapsedMs = watch.ElapsedMilliseconds;
            msgstr = String.Format("Completed generating dtSummaryFcst table in {0} seconds. ", (elapsedMs / Convert.ToDecimal(1000)).ToString("N3"), errcount.ToString());
            if (Debug) Utils.LogMessage(Utils.MsgLevel.Debug, msgstr, false, true);


            return null;
           
        }

        private dsSummaryFcst GetICFcst(string p_CompanyID, DateTime p_LastDate)
        {
            // First we get the PO Suggestions from the Iner-Company Partner

            dsSummaryFcst dsICSummary = new dsSummaryFcst();
            dsICPOSugg dsICSugg = GetICPOSuggestions(p_CompanyID, p_LastDate);


            // Next we get the existing Sales Orders so we can create forecasts to offset.
            dsSalesOrders dsICSalesOrders = GetICSalesOrders(p_CompanyID, Global.ICCustNum);


            
            /* Create new dsICSummary from dsICSugg 
             
               Rather than using the Forecast Method as set on the PartPlant_UD record we are applying standard values.
               If the ClassID is in 'TZM', 'HIT' then we use 'FIN01' otherwise we use the actual PO Suggestion less ship time - ShipDate */

            using (new Utils.TimerLogger("Generate dtSummary table in GetICFcst"))
            {

                //var groupedby = dsICSugg.dtICPOSugg.AsEnumerable().GroupBy(row => row.Field<string>("PartNum"));

        
        // Here we need to group and sum the quantity per period
        DataTable dt = new DataTable();
                var query =  (from mytable in dsICSugg.dtICPOSugg
                group mytable by new
                {
                    mytable.Company,
                    mytable.PartNum,
                    mytable.ClassID,
                    mytable.ForePeriod
                } into g
                select new
                {
                    Company = (System.String)g.Key.Company,
                    PartNum = (System.String)g.Key.PartNum,
                    ClassID = (System.String)g.Key.ClassID,
                    ForePeriod = (System.Int32)g.Key.ForePeriod,
                    RelQty = (System.Int32)g.Sum(p => p.RelQty)
                }).ToList();


                foreach (var r in query)
                {
                    if (r.ClassID.ToString() == "TZM" || r.ClassID.ToString() == "HIT")
                    {
                        string qry_ForecastMethods = String.Format("Company = '{0}' AND Key1 = '{1}'", r.Company.ToString(), "INT01");  // INT01 sets forcast method parameters
                        DataRow[] rowForecastMethods = dtForecastMethods.Select(qry_ForecastMethods);

                        // Create new forecast data record with appropriate planning plant
                        dsSummaryFcst.dtSummaryFcstRow summaryRow = dsICSummary.dtSummaryFcst.NewdtSummaryFcstRow();
                        summaryRow.Company = Global.CompanyID;
                        summaryRow.Plant = Global.MainPlant;
                        summaryRow.PartNum = r.PartNum.ToString();
                        summaryRow.CustNum = Global.ICCustNum;
                        summaryRow.ForePeriod = Convert.ToInt32(r.ForePeriod);
                        summaryRow.ForeQty = Convert.ToInt32(r.RelQty);
                        summaryRow.NumFcsts = Convert.ToInt16(rowForecastMethods[0]["Number01"]);
                        summaryRow.FirstDayOfMonth = Convert.ToInt16(rowForecastMethods[0]["Number02"]);
                        summaryRow.DayOfWeek = Convert.ToInt16(StringEnum.Parse(typeof(WeekDay), Convert.ToString(rowForecastMethods[0]["ShortChar02"]), true));
                        
                        dsICSummary.dtSummaryFcst.AdddtSummaryFcstRow(summaryRow);
                    }


                }

                // Now we need to add Forecasts to cover any existing Sales Orders

                Utils.LogMessage(Utils.MsgLevel.Debug, string.Format("Add Forecasts for existing Sales Orders."), false, true);

                foreach (DataRow row in dsICSalesOrders.dtSalesOrders.Rows)
                {
                    
                        // Create new forecast data record 
                        dsSummaryFcst.dtICPOSuggDtlRow summaryRow = dsICSummary.dtICPOSuggDtl.NewdtICPOSuggDtlRow();
                        summaryRow.Company = row["Company"].ToString();
                        summaryRow.Plant   = row["Plant"].ToString();
                        summaryRow.PartNum = row["PartNum"].ToString();
                        summaryRow.CustNum = Global.ICCustNum;
                        summaryRow.ForeDate = Convert.ToDateTime(row["ReqDate"]);
                        summaryRow.ForeQty = Convert.ToInt32(row["Qty"]);


                    Utils.LogMessage(Utils.MsgLevel.Debug, string.Format("Adding forecast for Company: {0}, Plant: {1}, PartNum: {2}, CustNum: {3},  ForeDate: {4}, ForeQty {5:0} ", summaryRow.Company, summaryRow.Plant, summaryRow.PartNum, summaryRow.CustNum.ToString(), summaryRow.ForeDate.ToString(), summaryRow.ForeQty.ToString()), false, true);
                    Utils.LogMsg(Utils.MsgLevel.Debug, summaryRow.Company, summaryRow.Plant, summaryRow.PartNum, string.Format("Adding forecast: CustNum: {0},  ForeDate: {1}, ForeQty {2:0} ", summaryRow.CustNum.ToString(), summaryRow.ForeDate.ToString(), summaryRow.ForeQty.ToString()), false, true);
                    dsICSummary.dtICPOSuggDtl.AdddtICPOSuggDtlRow(summaryRow);
                    

                }

                Utils.LogMessage(Utils.MsgLevel.Debug, string.Format("Completed adding forecasts for existing SalesOrders."), false, true);

                /*
                foreach (DataRow row in dsICSugg.dtICPOSugg.Rows)
                {
                    if (row["ClassID"].ToString() == "TZM" || row["ClassID"].ToString() == "HIT")
                    {
                        string qry_ForecastMethods = String.Format("Company = '{0}' AND Key1 = '{1}'", row["Company"].ToString(), "INT01");  // INT01 sets forcast method parameters
                        DataRow[] rowForecastMethods = dtForecastMethods.Select(qry_ForecastMethods);

                        // Create new forecast data record with appropriate planning plant
                        dsSummaryFcst.dtSummaryFcstRow summaryRow = dsICSummary.dtSummaryFcst.NewdtSummaryFcstRow();
                        summaryRow.Company = row["Company"].ToString();
                        summaryRow.Plant = row["Plant"].ToString();
                        summaryRow.PartNum = row["PartNum"].ToString();
                        summaryRow.CustNum = Global.ICCustNum;     
                        summaryRow.ForePeriod = Convert.ToInt32(row["ForePeriod"]);
                        summaryRow.ForeQty = Convert.ToInt32(row["RelQty"]);
                        summaryRow.NumFcsts = Convert.ToInt16(rowForecastMethods[0]["Number01"]);
                        summaryRow.FirstDayOfMonth = Convert.ToInt16(rowForecastMethods[0]["Number02"]);
                        summaryRow.DayOfWeek = Convert.ToInt16(StringEnum.Parse(typeof(WeekDay), Convert.ToString(rowForecastMethods[0]["ShortChar02"]), true));



                        dsICSummary.dtSummaryFcst.AdddtSummaryFcstRow(summaryRow);
                    }

                }

                */
                // Now add non-TZM  HIT PO Suggestions
                foreach (DataRow row in dsICSugg.dtICPOSugg.Rows)
                {
                    if (row["ClassID"].ToString() != "TZM" && row["ClassID"].ToString() != "HIT")
                    {
                        /*
                        string qry_ForecastMethods = String.Format("Company = '{0}' AND Key1 = '{1}'", row["Company"].ToString(), "INT01");  // INT01 sets forcast method parameters
                        DataRow[] rowForecastMethods = dtForecastMethods.Select(qry_ForecastMethods);
                        */
                // Create new forecast data record with appropriate planning plant
                dsSummaryFcst.dtICPOSuggDtlRow summaryRow = dsICSummary.dtICPOSuggDtl.NewdtICPOSuggDtlRow();
                        summaryRow.Company = Global.CompanyID;
                        summaryRow.Plant = Global.MainPlant;
                        summaryRow.PartNum = row["PartNum"].ToString();
                        summaryRow.CustNum = Global.ICCustNum; 
                        summaryRow.ForeDate = Convert.ToDateTime(row["ShipDate"]);
                        summaryRow.ForeQty = Convert.ToInt32(row["RelQty"]);
                        
                        dsICSummary.dtICPOSuggDtl.AdddtICPOSuggDtlRow(summaryRow);
                    }
                }
            }

            return dsICSummary;

        }


        public DataTable ReadCsvFile(string filename)
        {
            /* General method to read a CSV file and return a generic datatable */
            DataTable dtCsv = new DataTable();
            string Fulltext;
            if (filename != null)
            {

                using (StreamReader sr = new StreamReader(filename))
                {
                    while (!sr.EndOfStream)
                    {
                        Fulltext = sr.ReadToEnd().ToString(); //read full file text  
                        string[] rows = Fulltext.Split('\n'); //split full file text into rows  
                        for (int i = 0; i < rows.Count() - 1; i++)
                        {
                            string[] rowValues = rows[i].Split(','); //split each row with comma to get individual values  
                            {
                                if (i == 0)
                                {
                                    for (int j = 0; j < rowValues.Count(); j++)
                                    {
                                        dtCsv.Columns.Add(rowValues[j]); //add headers  
                                    }
                                }
                                else
                                {
                                    DataRow dr = dtCsv.NewRow();
                                    for (int k = 0; k < rowValues.Count(); k++)
                                    {
                                        dr[k] = rowValues[k].ToString();
                                    }
                                    dtCsv.Rows.Add(dr); //add other rows  
                                }
                            }
                        }
                    }
                }
            }
            return dtCsv;
        }

        /*
        public static void Utils.LogMessage(Utils.MsgLevel p_level, string p_msg, bool showmsg, bool logmsg)
        {
            StreamWriter log;
            FileStream fileStream = null;
            DirectoryInfo logDirInfo = null;
            FileInfo logFileInfo;
            if (logmsg)
            {
                if (p_level == Utils.MsgLevel.Debug && DEBUG == true)
                {
                    return;
                }
                else
                { 
                    string logFilePath = Properties.Settings.Default.LogFolder;
                    logFilePath = logFilePath + "Log-" + System.DateTime.Today.ToString("MM-dd-yyyy") + "." + "txt";
                    logFileInfo = new FileInfo(logFilePath);
                    logDirInfo = new DirectoryInfo(logFileInfo.DirectoryName);
                    if (!logDirInfo.Exists) logDirInfo.Create();
                    if (!logFileInfo.Exists)
                    {
                        fileStream = logFileInfo.Create();
                    }
                    else
                    {
                        fileStream = new FileStream(logFilePath, FileMode.Append);
                    }
                    log = new StreamWriter(fileStream);


                    switch (p_level)
                    {
                        case Utils.MsgLevel.Info:
                            p_msg = "    Info:   " + p_msg;
                            break;
                        case Utils.MsgLevel.Warning:
                            p_msg = " Warning: " + p_msg;
                            break;
                        case Utils.MsgLevel.Error:
                            p_msg = "   Error:    " + p_msg;
                            break;
                        case Utils.MsgLevel.Critical:
                            p_msg = "Critical:  " + p_msg;
                            break;
                        case Utils.MsgLevel.Debug:
                            p_msg = "   Debug:      " + p_msg;
                            break;
                    }
                    p_msg = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + " " + p_msg;
                    log.WriteLine(p_msg);
                    log.Close();
                }
            }

            if (showmsg)
            {
                MessageBox.Show(p_msg);
            }

            
        } */

        public static DataTable GetDataTableFromSmartforecastsCSV(string p_CompanyID, string filePath)
        {
            /* Forecast data from Smartforecasts is output to file in CSV format.
             * Field terminator is comma and line terminator is 0x0D, 0x0A
             * Data layout is expected as:
             *              PartNum,    Plant,     SalesRegion, ForecastDate, ForecastQuantity
             *      e.g.    162-255-001,Plant_AUCK,AUCK,01-01-2018,6
                            

             */

            //Create a new stopwatch to record the operation time
            var watch = System.Diagnostics.Stopwatch.StartNew();

            // Log the start of the read operation
            String msgstr = String.Format("Getting records from csv file {0}", filePath);
            Utils.LogMessage(Utils.MsgLevel.Info, msgstr,false, true);

            string p_DateForemat = "MM/DD/YYYY";
            // Create a new DataTable to be returned
            DataTable dt = new DataTable();

            try
            {
              // Add the columns to the datatable
                DataColumn dc = new DataColumn("Company", typeof(String));
                dt.Columns.Add(dc);

                dc = new DataColumn("Plant", typeof(String));
                dt.Columns.Add(dc);

                dc = new DataColumn("PartNum", typeof(String));
                dt.Columns.Add(dc);

                dc = new DataColumn("SalesRegion", typeof(String));
                dt.Columns.Add(dc);

                dc = new DataColumn("ForePeriod", typeof(int));
                dt.Columns.Add(dc);

                dc = new DataColumn("ForeQty", typeof(int));
                dt.Columns.Add(dc);
            }
            catch (Exception ex)
            {
                msgstr = String.Format("Exception creating new DataTable - {0}", ex.Message);
                Utils.LogMessage(Utils.MsgLevel.Error, msgstr, true, true);
  
                return null;
            }
           
            try
            {
                string[] csvRows = System.IO.File.ReadAllLines(filePath);
                string[] fields = null;
                bool firstrow = true;
                foreach (string csvRow in csvRows)
                {
                    if (firstrow == false)
                    {
                        fields = csvRow.Split(',');
                        DataRow row = dt.NewRow();

                        row[0] = p_CompanyID;     //Company
                        row[1] = fields[1].Substring(6);     //Plant
                        row[2] = fields[0];     //PartNum
                        row[3] = fields[2];     //SalesRegion

                        // Get ForePeriod from input string format of "dd/mm/yyyy"
                        string tmpStr = fields[3];
                        int tmpPeriod = 0;
                        switch (p_DateForemat)
                        {
                            case "DD/MM/YYYY":
                                    tmpPeriod = (Convert.ToInt16(tmpStr.Substring(6, 4)) * 100) + (Convert.ToInt16(tmpStr.Substring(3, 2)));
                                break;
                            case "MM/DD/YYYY":
                                    tmpPeriod = (Convert.ToInt16(tmpStr.Substring(6, 4)) * 100) + (Convert.ToInt16(tmpStr.Substring(0, 2)));
                                break;
                        }
                        row[4] = tmpPeriod;     //ForePeriod

                        // Get ForeQty from input string
                        int tmpForeQty = (Convert.ToInt16(fields[4]));
                        row[5] = tmpForeQty;     //ForeQty

                        if (tmpForeQty > 0)
                        {
                            dt.Rows.Add(row);
                        }
                        
                        
                    }
                    firstrow = false;
                }
            }
            catch(Exception ex)
            {
                msgstr = String.Format("Exception creating reading records from CSV - {0}", ex.Message);
                Utils.LogMessage(Utils.MsgLevel.Error, msgstr, true, true);

                return null;
            }

            finally
            {
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                //("Completed reading data from CSV. Retrieved " + dt.Rows.Count.ToString() + " records in " + elapsedMs.ToString("N0") + " milliseconds", false, true);

                msgstr = String.Format("Completed reading data from CSV. Retrieved {0} records in {1} seconds", dt.Rows.Count.ToString("N0"), ((decimal)elapsedMs / 1000).ToString("N3"));
                Utils.LogMessage(Utils.MsgLevel.Info, msgstr, false, true);
            }

            return dt;
        }

        public int WriteToCSV(DataTable p_dt, string p_outFile)
        {
            StringBuilder sb = new StringBuilder();

            IEnumerable<string> columnNames = p_dt.Columns.Cast<DataColumn>().
                                              Select(column => column.ColumnName);
            sb.AppendLine(string.Join(",", columnNames));

            foreach (DataRow row in p_dt.Rows)
            {
                IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString());
                sb.AppendLine(string.Join(",", fields));
            }

            File.WriteAllText(p_outFile, sb.ToString());

            return 0;
        }

        public dsICPOSugg GetICPOSuggestions(string p_CompanyID, DateTime p_LastDate)
        {
            dsICPOSugg dsPOSuggestions = new dsICPOSugg();
            
            var watch = System.Diagnostics.Stopwatch.StartNew();
            Utils.LogMessage(Utils.MsgLevel.Info, "Getting Inter-Company PO Suggestions", false, true);
            dsSummaryFcst dsICSummary = new dsSummaryFcst();
            DataTable dtPOSuggestions = new DataTable();

            try
            {

                using (var con = new SqlConnection(Properties.Settings.Default.CustomAppsConnStr))
                using (var cmd = new SqlCommand("dbo.csp_DM_GetICPOSuggestions", con))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 6000;
                    cmd.Parameters.AddWithValue("@Company", p_CompanyID);
                    cmd.Parameters.AddWithValue("@LastDate", p_LastDate); 


                    da.Fill(dtPOSuggestions);
                    
                }
            }
            catch (Exception ex)
            {
                Utils.LogMessage(Utils.MsgLevel.Error, "     GetICPOSuggestions 1. {0}" + ex.Message, false, true);
            }

            finally
            {
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                Utils.LogMessage(Utils.MsgLevel.Info, string.Format("Completed Getting Inter-Company PO Suggestions. Retrieved {0} records in {1} seconds", dtPOSuggestions.Rows.Count.ToString(), ((decimal)elapsedMs / 1000).ToString("N3")), false, true);
            }

            
            foreach (DataRow row in dtPOSuggestions.Rows)
            {

                dsICPOSugg.dtICPOSuggRow summaryRow = dsPOSuggestions.dtICPOSugg.NewdtICPOSuggRow();

                summaryRow.Company = row["Company"].ToString();
                summaryRow.Plant = row["Plant"].ToString();
                summaryRow.PartNum = row["PartNum"].ToString();
                summaryRow.DueDate = Convert.ToDateTime(row["DueDate"]);
                summaryRow.RelQty = Convert.ToInt32(row["RelQty"]);
                summaryRow.ClassID = row["ClassID"].ToString();
                summaryRow.ShipDate = Convert.ToDateTime(row["ShipDate"]);
                summaryRow.ForePeriod = Convert.ToInt32(row["ForePeriod"]);
                dsPOSuggestions.dtICPOSugg.AdddtICPOSuggRow(summaryRow);
            } 
        

                    return dsPOSuggestions;
        }

        public dsSalesOrders GetICSalesOrders(string p_CompanyID, int p_ICCustNum)
        { 
            dsSalesOrders dsICSalesOrders = new dsSalesOrders();

            var watch = System.Diagnostics.Stopwatch.StartNew();
            Utils.LogMessage(Utils.MsgLevel.Info, "Getting Inter-Company Sales Orders", false, true);
            DataTable dtICSalesOrders = new DataTable();

            try
            {

                using (var con = new SqlConnection(Properties.Settings.Default.CustomAppsConnStr))
                using (var cmd = new SqlCommand("dbo.csp_DM_GetICSalesOrders", con))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 6000;
                    cmd.Parameters.AddWithValue("@Company", p_CompanyID);
                    cmd.Parameters.AddWithValue("@ICCustNum", p_ICCustNum);


                    da.Fill(dtICSalesOrders);
                    
                }
}
            catch (Exception ex)
            {
                Utils.LogMessage(Utils.MsgLevel.Error, "     GetICSalesOrders 1. {0}" + ex.Message, false, true);
            }

            finally
            {
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                Utils.LogMessage(Utils.MsgLevel.Info, string.Format("Completed Getting Inter-Company SalesOrders. Retrieved {0} records in {1} seconds", dtICSalesOrders.Rows.Count.ToString(), ((decimal) elapsedMs / 1000).ToString("N3")), false, true);
            }

            
            foreach (DataRow row in dtICSalesOrders.Rows)
            {

                dsSalesOrders.dtSalesOrdersRow soRow = dsICSalesOrders.dtSalesOrders.NewdtSalesOrdersRow();

                soRow.Company = row["Company"].ToString();
                soRow.Plant = row["Plant"].ToString();
                soRow.PartNum = row["PartNum"].ToString();
                soRow.ReqDate = Convert.ToDateTime(row["ReqDate"]);
                soRow.Qty = Convert.ToInt32(row["ReqQty"]);
                soRow.ClassID = row["ClassID"].ToString();
                dsICSalesOrders.dtSalesOrders.AdddtSalesOrdersRow(soRow);
            } 
        
            
                return dsICSalesOrders;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Danger Will Robinson");
            return;
            /*
            // Ice.Core.Session session = new Ice.Core.Session("dplayne", "xaser11", "net.tcp://tz-akl-srs1/erp10");
            Ice.Core.Session session = new Ice.Core.Session("dplayne", "xaser11", Ice.Core.Session.LicenseType.Default, @"c:\temp\ERP10_2_NonSSO.sysconfig");
            session.CompanyID = "TZA";
            // References: Epicor.ServiceModel.dll, Erp.Contracts.BO.ABCCode.dll
            var forecastBO = Ice.Lib.Framework.WCFServiceSupport.CreateImpl<Erp.Proxy.BO.ForecastImpl>(session, Erp.Proxy.BO.ForecastImpl.UriPath);



            // Call the BO methods
            //var ds = forecastBO.GetByID("162-255-001", "AUCK", 0, Convert.ToDateTime("2018-05-16"), "");
            String whereClause = "custnum = 0 and foredate >= '2018-05-01'";
            bool result;
            string tmpstr;
            var ds = forecastBO.GetList(whereClause, 9999, 1, out result);

            foreach (DataRow theRow in ds.Tables["ForecastList"].Rows)
            {
                tmpstr = theRow["PartNum"] + "\t" + theRow["Plant"] + "\t" + theRow["custnum"].ToString() + "\t" + theRow["ForeDate"].ToString() + "\t" + theRow["ParentPartNum"];
                forecastBO.DeleteByID(theRow["PartNum"].ToString(), theRow["Plant"].ToString(), Convert.ToInt32(theRow["custnum"]), Convert.ToDateTime(theRow["ForeDate"]), theRow["ParentPartNum"].ToString());
            }

            forecastBO.DeleteByID("162-255-001", "AUCK", 0, Convert.ToDateTime("2018-05-16"), "");
            //var row = ds.forecastBO[0];

                //System.Console.WriteLine("CountFreq is {0}", row.CountFreq);
                //System.Console.WriteLine("CustomField_c is {0}", row["CustomField_c"]);
                //System.Console.ReadKey();          
                */
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Exiting the application

            Application.Exit();
        }

        

        private void GetCommonData(Period FirstPeriod, Period LastPeriod)
        {
            // 1. We need to lookup the Forecast methods later so we will get a datatable of these now.
            Utils.LogMessage(Utils.MsgLevel.Info, "Getting Forecast Methods." , false, true);

            var watch = System.Diagnostics.Stopwatch.StartNew();
            try
            {
                using (var con = new SqlConnection(Properties.Settings.Default.ERPConnStr))
                using (var cmd = new SqlCommand("SELECT Company, Key1, Number01, Number02, ShortChar01, ShortChar02 FROM [TZ-AKL-EDB1].ERP10.ice.UD26", con))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandTimeout = 6000;

                    da.Fill(dtForecastMethods);
                }
            }
            catch (Exception ex)
            {
                Utils.LogMessage(Utils.MsgLevel.Error, "     MainForm_Load 1." + ex.Message, true, true);
            }

            finally
            {
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                Utils.LogMessage(Utils.MsgLevel.Info, "Completed Getting Forecast Methods. Retrieved " + dtForecastMethods.Rows.Count.ToString() + " records in " + (elapsedMs / 1000).ToString("N3") + " seconds", false, true);
            }


            // Get number of days of week in each period
            //Period FromPeriod = FirstPeriod; //new Period(FirstPeriod);
            //Period ToPeriod = LastPeriod; // new Period(LastPeriod);
            // !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            // Last period needs to be the maximum defined by number of forecasts
            // TZA need 13 periods
            Period tmpPeriod = new Period(LastPeriod.PeriodValue);
            tmpPeriod.PeriodAdd(1);
            dsDOW = GetNumDaysOfWeek(FirstPeriod, tmpPeriod);
        }

        private void tbImportFile_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = Properties.Settings.Default.InputFolder;
            openFileDialog1.DefaultExt = "csv";
            openFileDialog1.CheckFileExists = true;
            openFileDialog1.CheckPathExists = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                tbImportFile.Text = openFileDialog1.FileName;
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SafetyStockForm safetystockForm = new SafetyStockForm();
            safetystockForm.Show();
        }
    }

}

//var abcCodeBO = Ice.Lib.Framework.WCFServiceSupport.CreateImpl<Erp.Proxy.BO.ABCCodeImpl>(session, Erp.Proxy.BO.ABCCodeImpl.UriPath);

/*
            var watch = System.Diagnostics.Stopwatch.StartNew();
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                Utils.LogMessage(Utils.MsgLevel.Info, "Completed Getting Forecast Methods. Retrieved " + dtForecastMethods.Rows.Count.ToString() + " records in " + (elapsedMs / 1000).ToString("N3") + " seconds", false, true);

 * 
 */
