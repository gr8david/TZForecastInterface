using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TZForecastInterface
{
    class Utils
    {
        public static bool DEBUG = Properties.Settings.Default.DEBUG;
        public enum MsgLevel
        {
            Info,
            Warning,
            Error,
            Critical,
            Debug,
            SectionEnd
        };

        public static int GenerateCSV(DataTable dt, string p_outFile, bool p_append)
        {
            StringBuilder sb = new StringBuilder();
            int recs = 0;
            try
            {
                int count = 1;
                int totalColumns = dt.Columns.Count;
                foreach (DataColumn dr in dt.Columns)
                {
                    sb.Append(dr.ColumnName);

                    if (count != totalColumns)
                    {
                        sb.Append(",");
                    }

                    count++;
                }

                sb.AppendLine();

                string value = String.Empty;
                foreach (DataRow dr in dt.Rows)
                {
                    for (int x = 0; x < totalColumns; x++)
                    {
                        if (dr[x].GetType() == typeof(System.DateTime))
                        {
                            DateTime tmpdate = Convert.ToDateTime(dr[x]);
                            value = tmpdate.ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            value = dr[x].ToString();
                        }


                        if (value.Contains(",") || value.Contains("\""))
                        {
                            value = '"' + value.Replace("\"", "\"\"") + '"';
                        }

                        sb.Append(value);

                        if (x != (totalColumns - 1))
                        {
                            sb.Append(",");
                        }
                    }
                    recs += 1;
                    sb.AppendLine();
                }
            }
            catch (Exception ex)
            {
                Utils.LogMessage(Utils.MsgLevel.Debug, String.Format("Exception in Utils.GenerateCSV - {0}", ex.Message) , false, true);

                return 1;
            }

            //Now we can write the output file.
            if (p_append == true)
            {
                File.AppendAllText(p_outFile, sb.ToString());
            }
            else
            {
                File.WriteAllText(p_outFile, sb.ToString());
            }
            return recs;
        }

        public static void LogMessage(MsgLevel p_level, string p_msg, bool showmsg, bool logmsg)
        {
            StreamWriter log;
            FileStream fileStream = null;
            DirectoryInfo logDirInfo = null;
            FileInfo logFileInfo;
            if (logmsg)
            {
                /*
                 * if (p_level == MsgLevel.Debug && DEBUG == true)
                {
                    return;
                }
                else*/
                {
                    string logFilePath = Properties.Settings.Default.LogFolder;
                    logFilePath = logFilePath + "\\" + Global.CompanyID + "_FcstLog-" + System.DateTime.Today.ToString("dd-MM-yyyy") + ".txt";
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
                        case MsgLevel.Info:
                            p_msg = "Info:     " + p_msg;
                            break;
                        case MsgLevel.Warning:
                            p_msg = "Warning:  " + p_msg;
                            break;
                        case MsgLevel.Error:
                            p_msg = "Error:    " + p_msg;
                            break;
                        case MsgLevel.Critical:
                            p_msg = "Critical: " + p_msg;
                            break;
                        case MsgLevel.Debug:
                            p_msg = "Debug:    " + p_msg;
                            break;
                        case MsgLevel.SectionEnd:
                            p_msg = "======================================================================================";
                            break;
                    }
                    p_msg = DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString() + " " + p_msg;
                    log.WriteLine(p_msg);
                    log.Close();
                }
            }

            if (showmsg)
            {
                MessageDisplay MsgBox = new MessageDisplay(p_msg);
                MsgBox.ShowDialog();
                //MessageBox.Show(p_msg);
            }


        }

        public class TimerLogger : IDisposable
        {
            public TimerLogger(string p_message)
            {
                this.message = p_message;
                this.timer = new Stopwatch();
                this.timer.Start();
                string msgstr = String.Format("Starting {0}. ", message);
                Utils.LogMessage(Utils.MsgLevel.Debug, msgstr, false, true);

            }

            string message;
            Stopwatch timer;

            public void Dispose()
            {
                this.timer.Stop();
                var ms = this.timer.ElapsedMilliseconds;

                string msgstr = String.Format("Completed {0} in {1} seconds. ", message, (ms / Convert.ToDecimal(1000)).ToString("N3"));
                Utils.LogMessage(Utils.MsgLevel.Debug, msgstr, false, true);

            }


        }
        public static void LogMsg(MsgLevel p_level, string p_Company, string p_Plant, string p_PartNum, string p_msg, bool showmsg, bool logmsg)
        {
            StreamWriter log;
            FileStream fileStream = null;
            DirectoryInfo logDirInfo = null;
            FileInfo logFileInfo;
            if (logmsg)
            {
                /*
                 * if (p_level == MsgLevel.Debug && DEBUG == true)
                {
                    return;
                }
                else*/
                {
                    string logFilePath = Properties.Settings.Default.LogFolder;
                    logFilePath = logFilePath + "\\" + Global.CompanyID + "_TestLog-" + System.DateTime.Today.ToString("dd-MM-yyyy") + ".txt";
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

                    string level = "";

                    switch (p_level)
                    {
                        case MsgLevel.Info:
                            level = "Info";
                            break;
                        case MsgLevel.Warning:
                            level = "Warning";
                            break;
                        case MsgLevel.Error:
                            level = "Error";
                            break;
                        case MsgLevel.Critical:
                            level = "Critical";
                            break;
                        case MsgLevel.Debug:
                            level = "Debug";
                            break;
                        case MsgLevel.SectionEnd:
                            level = "SectionBreak";
                            break;
                        
                    }
                    if (level == "SectionBreak")
                    {
                        p_msg = DateTime.Now.ToString() + "," + "," + "," + "," + "," + "##########################################";
                    }
                    else
                    {
                        p_msg = DateTime.Now.ToString() + "," + level + "," + p_Company + "," + p_Plant + "," + p_PartNum + "," + p_msg;
                    }
                    log.WriteLine(p_msg);
                    log.Close();
                }
            }

            if (showmsg)
            {
                MessageDisplay MsgBox = new MessageDisplay(p_msg);
                MsgBox.ShowDialog();
                //MessageBox.Show(p_msg);
            }


        }

    }
}
