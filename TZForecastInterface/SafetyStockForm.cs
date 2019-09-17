using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TZForecastInterface
{
    public partial class SafetyStockForm : Form
    {
        public SafetyStockForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Global.Debug = true;
            // First we call the stored procedure to get the data we need
            // This will return a dsSafetyStock dataset.

            dsSafetyStock dsSafetyStockData = new dsSafetyStock();

            var watch = System.Diagnostics.Stopwatch.StartNew();
            Utils.LogMessage(Utils.MsgLevel.SectionEnd, "", false, true);
            Utils.LogMessage(Utils.MsgLevel.Info, "Getting Safety Stock data", false, true);
            Utils.LogMessage(Utils.MsgLevel.Info, string.Format("     Company = {0}", Global.CompanyID), false, true);
            try
            {
                using (var con = new SqlConnection(Properties.Settings.Default.CustomAppsConnStr))
                using (var cmd = new SqlCommand("dbo.csp_DM_GetSafetyStockData", con))
                using (var da = new SqlDataAdapter(cmd))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 6000;
                    cmd.Parameters.AddWithValue("@Company", Global.CompanyID);
                    cmd.Parameters.AddWithValue("@FcstPeriod", n_FcstPeriod.Value);
                    cmd.Parameters.AddWithValue("@NumPeriods", n_NumPeriods.Value);


                    da.Fill(dsSafetyStockData.dtSafetyStock);
                }
            }
            catch (Exception ex)
            {
                Utils.LogMessage(Utils.MsgLevel.Error, "     SafetyStockData 1. {0}" + ex.Message, false, true);
            }

            finally
            {
                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;
                Utils.LogMessage(Utils.MsgLevel.Info, string.Format("Completed Getting Safety Stock data. Retrieved {0} records in {1} seconds", dsSafetyStockData.dtSafetyStock.Rows.Count.ToString(), ((decimal)elapsedMs / 1000).ToString("N3")), false, true);
            }
            // Now we process each record and build the database for output to CSV file

            decimal t_MonthlyForecast = 0;
            decimal t_MonthlyUsage = 0;
            decimal t_ICMonthlyForecast = 0;
            decimal t_ICMonthlyUsage = 0;

            int t_SSQty = 0;
            int t_ICSSQty = 0;

            //dsDetailSafetyStock dsSafety = new dsDetailSafetyStock();
            dsDetailSafetyStock dsDetail = new dsDetailSafetyStock();


            foreach (dsSafetyStock.dtSafetyStockRow row in dsSafetyStockData.dtSafetyStock.Rows)
            {
                Utils.LogMessage(Utils.MsgLevel.Debug, string.Format("     Processing {0}, {1}, {2} ", row.Company, row.Plant, row.PartNum), false, true);

                t_SSQty = 0;
                t_ICSSQty = 0;

                /*
                if (row.PartNum == "012-000-060")
                {
                    MessageBox.Show("Stop");
                }
                */


                if (row.SafetyStock == true)
                {
                    switch (row.Basis)
                    {
                        case "SF":      //Sales Forecast
                            if (row.Forecast_Qty <= 0 || row.Forecast_Months <= 0)
                            {
                                // We cannot determine a viable monthly forecast so throw a warning
                                Utils.LogMessage(Utils.MsgLevel.Warning, string.Format("Cannot determine a viable average monthly Forecast: {0}, {1}, {2}, Total Forecast = {3} Forecast Months = {4}", row.Company, row.Plant, row.PartNum, row.Forecast_Qty, row.Forecast_Months), false, true);
                            }
                            else
                            {
                                t_MonthlyForecast = row.Forecast_Qty / row.Forecast_Months;
                                if (row.LeadTimeMultiplier > 0)
                                {
                                    if (row.LeadTime > 0)
                                    {
                                        t_SSQty = Convert.ToInt32((t_MonthlyForecast * 12 / 52) * Convert.ToDecimal((row.LeadTime) / 5) * row.LeadTimeMultiplier);
                                    }
                                    else
                                    {
                                        // Throw error
                                        Utils.LogMessage(Utils.MsgLevel.Debug, string.Format("Error: Leadtime = 0  {0}, {1}, {2} ", row.Company, row.Plant, row.PartNum), false, true);
                                    }
                                }
                                else
                                {
                                    if (row.WeeksSafety > 0)
                                    {
                                        t_SSQty = Convert.ToInt32((t_MonthlyForecast * 12 / 52) * row.WeeksSafety);
                                        if (Global.Debug)
                                        {
                                            Utils.LogMessage(Utils.MsgLevel.Debug, string.Format("{0}, {1}, {2}, Monthly Forecast = {3}, Weeks Safety = {4}, Safety Stock = {5}", row.Company, row.Plant, row.PartNum, Convert.ToInt32(t_MonthlyForecast * 12 / 52), row.WeeksSafety, t_SSQty), false, true);
                                        }
                                    }
                                    else
                                    {
                                        // Throw error
                                        Utils.LogMessage(Utils.MsgLevel.Debug, string.Format("Error: Weeks Safety = 0  {0}, {1}, {2} ", row.Company, row.Plant, row.PartNum), false, true);

                                    }
                                }
                            }

                            break;
                        case "US":      //Usage

                            if (row.Usage_Qty <= 0 || row.Usage_Months <= 0)
                            {
                                // We cannot determine a viable monthly forecast so throw a warning
                                Utils.LogMessage(Utils.MsgLevel.Warning, string.Format("Cannot determine a viable average monthly Usage: {0}, {1}, {2}, Total Usage = {3} Usage Months = {4}", row.Company, row.Plant, row.PartNum, row.Usage_Qty, row.Usage_Months), false, true);
                            }
                            else
                            {
                                t_MonthlyUsage = row.Usage_Qty / row.Usage_Months;
                                if (row.LeadTimeMultiplier > 0)
                                {
                                    if (row.LeadTime > 0)
                                    {
                                        t_SSQty = Convert.ToInt32((t_MonthlyUsage * 12 / 52) * Convert.ToDecimal((row.LeadTime) / 5) * row.LeadTimeMultiplier);
                                    }
                                    else
                                    {
                                        // Throw error
                                        Utils.LogMessage(Utils.MsgLevel.Debug, string.Format("Error: Leadtime = 0  {0}, {1}, {2} ", row.Company, row.Plant, row.PartNum), false, true);
                                    }
                                }
                                else
                                {
                                    if (row.WeeksSafety > 0)
                                    {
                                        t_SSQty = Convert.ToInt32((t_MonthlyUsage * 12 / 52) * row.WeeksSafety);
                                    }
                                    else
                                    {
                                        // Throw error
                                        Utils.LogMessage(Utils.MsgLevel.Debug, string.Format("Error: Weeks Safety = 0  {0}, {1}, {2} ", row.Company, row.Plant, row.PartNum), false, true);

                                    }
                                }
                            }
                            break;
                        case "IC":      //Inter-company Partner's Sales Forecast

                            // Throw error
                            Utils.LogMessage(Utils.MsgLevel.Debug, string.Format("Error: Inter-company Partner's Sales Forecast does is not sensible here.  {0}, {1}, {2} ", row.Company, row.Plant, row.PartNum), false, true);

                            break;
                    }
                }

                if (row.ICSafetyStock == true)
                {
                    switch (row.ICBasis)
                    {
                        case "SF":      //Sales Forecast

                            // Throw error
                            Utils.LogMessage(Utils.MsgLevel.Debug, string.Format("Error: Sales Forecast method is not available for Inter-comoany safety Stock calculation  {0}, {1}, {2} ", row.Company, row.Plant, row.PartNum), false, true);
                            break;
                        case "US":      //Usage

                            // Throw error
                            Utils.LogMessage(Utils.MsgLevel.Debug, string.Format("Error: Sales Forecast method is not available for Inter-comoany safety Stock calculation  {0}, {1}, {2} ", row.Company, row.Plant, row.PartNum), false, true);


                            break;
                        case "IC":      //Inter-company Partner's Sales Forecast

                            if (row.ICForecast_Qty <= 0 || row.ICForecast_Months <= 0)
                            {
                                // We cannot determine a viable monthly forecast so throw a warning
                                Utils.LogMessage(Utils.MsgLevel.Warning, string.Format("Cannot determine a viable average monthly IC Forecast: {0}, {1}, {2}, Total IC forecast = {3} IC Forecast Months = {4}", row.Company, row.Plant, row.PartNum, row.ICForecast_Qty, row.ICForecast_Months), false, true);
                            }
                            else
                            {
                                t_ICMonthlyForecast = row.ICForecast_Qty / row.ICForecast_Months;
                                if (row.ICLeadTimeMultiplier > 0)
                                {
                                    if (row.ICLeadTime > 0)
                                    {
                                        t_ICSSQty = Convert.ToInt32((t_ICMonthlyForecast * 12 / 52) * Convert.ToDecimal((row.ICLeadTime) / 5) * row.ICLeadTimeMultiplier);
                                    }
                                    else
                                    {
                                        // Throw error
                                        Utils.LogMessage(Utils.MsgLevel.Debug, string.Format("Error: Leadtime = 0  {0}, {1}, {2} ", row.Company, row.Plant, row.PartNum), false, true);
                                    }
                                }
                                else
                                {
                                    if (row.ICWeeksSafety > 0)
                                    {
                                        t_ICSSQty = Convert.ToInt32((t_ICMonthlyForecast * 12 / 52) * row.ICWeeksSafety);
                                    }
                                    else
                                    {
                                        // Throw error
                                        Utils.LogMessage(Utils.MsgLevel.Debug, string.Format("Error: Weeks Safety = 0  {0}, {1}, {2} ", row.Company, row.Plant, row.PartNum), false, true);

                                    }
                                }// Throw error
                            }
                            break;
                    }
                }
                // Now we can write  a record for update of the SafetyQtyin Epicor
                 dsDetailSafetyStock.dtDetailSafetyStockRow detailRow = dsDetail.dtDetailSafetyStock.NewdtDetailSafetyStockRow();
                detailRow.Company = row.Company;
                detailRow.Plant = row.Plant;
                detailRow.PartNum = row.PartNum;
                detailRow.SafetyQty = t_SSQty + t_ICSSQty;
                
                dsDetail.dtDetailSafetyStock.AdddtDetailSafetyStockRow(detailRow);

                if (Global.Debug)
                {
                    Utils.LogMessage(Utils.MsgLevel.Info, string.Format("Safety Stock for  {0}, {1}, {2} = {3}   Inter-company = {4}  Total = {5}", row.Company, row.Plant, row.PartNum, t_SSQty, t_ICSSQty, (t_SSQty + t_ICSSQty)), false, true);
                }

                
            }

            // Write to CSV file
            string tmpPath = String.Format("{0}\\{1}_SafetyStock_{2}_{3}.csv", Properties.Settings.Default.OutputFolder, Global.CompanyID, Global.ForecastPeriod.PeriodValue, DateTime.Today.ToString("dd-MM-yyy"));
            int SSRecsWritten = Utils.GenerateCSV(dsDetail.dtDetailSafetyStock, tmpPath, false);
            //tbsafetyStockMsg.Text = string.Format(" {0} records processed with {1} warnings and {2} errors", CustEngRecsWritten, CustEngWarnings, CustEngErrors);


            Utils.LogMessage(Utils.MsgLevel.Info, string.Format("Safety Stock processing completed"), true, true);

        }


        private void SafetyStockForm_Load(object sender, EventArgs e)
        {
            // Load combobox with available Companies. Currently loaded a fixed list but this could load from the ERP database
            BindingList<Company> comboItems = new BindingList<Company>();
            comboItems.Add(new Company { CompanyID = "TZNZ", Name = "temperzone New Zealand Ltd" });
            comboItems.Add(new Company { CompanyID = "TZA", Name = "temperzone Australia Pty Ltd" });


            cbCompany.DataSource = comboItems;
            cbCompany.DisplayMember = "Name";
            cbCompany.ValueMember = "CompanyID";
            cbCompany.SelectedIndex = 0;



            Global.CompanyID = ((Company)cbCompany.SelectedItem).CompanyID;
        }

        private void cbCompany_SelectedIndexChanged(object sender, EventArgs e)
        {
            Global.CompanyID = ((Company)cbCompany.SelectedItem).CompanyID;
        }
    }
}
