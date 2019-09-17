namespace TZForecastInterface
{
    partial class EpicorInterface
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cbAllSites = new System.Windows.Forms.CheckBox();
            this.dtFromDate = new System.Windows.Forms.DateTimePicker();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tbManualFile = new System.Windows.Forms.TextBox();
            this.tbUsageFile = new System.Windows.Forms.TextBox();
            this.tbCustEngFile = new System.Windows.Forms.TextBox();
            this.tbSmartForecastsFile = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cbClearFcst = new System.Windows.Forms.CheckBox();
            this.cbSmartFcsts = new System.Windows.Forms.CheckBox();
            this.cbManualFcsts = new System.Windows.Forms.CheckBox();
            this.cbCustEngFcsts = new System.Windows.Forms.CheckBox();
            this.cbUsageFcsts = new System.Windows.Forms.CheckBox();
            this.lb_CompanyID = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tbICFile = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cbICFcsts = new System.Windows.Forms.CheckBox();
            this.btnProcess = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbAllSites
            // 
            this.cbAllSites.AutoSize = true;
            this.cbAllSites.Checked = true;
            this.cbAllSites.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbAllSites.Location = new System.Drawing.Point(1211, 46);
            this.cbAllSites.Name = "cbAllSites";
            this.cbAllSites.Size = new System.Drawing.Size(28, 27);
            this.cbAllSites.TabIndex = 1;
            this.cbAllSites.UseVisualStyleBackColor = true;
            // 
            // dtFromDate
            // 
            this.dtFromDate.Location = new System.Drawing.Point(637, 38);
            this.dtFromDate.Name = "dtFromDate";
            this.dtFromDate.Size = new System.Drawing.Size(386, 31);
            this.dtFromDate.TabIndex = 2;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tbManualFile);
            this.panel1.Controls.Add(this.tbUsageFile);
            this.panel1.Controls.Add(this.tbCustEngFile);
            this.panel1.Controls.Add(this.tbSmartForecastsFile);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.cbAllSites);
            this.panel1.Controls.Add(this.cbClearFcst);
            this.panel1.Controls.Add(this.dtFromDate);
            this.panel1.Controls.Add(this.cbSmartFcsts);
            this.panel1.Controls.Add(this.cbManualFcsts);
            this.panel1.Controls.Add(this.cbCustEngFcsts);
            this.panel1.Controls.Add(this.cbUsageFcsts);
            this.panel1.Location = new System.Drawing.Point(71, 106);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1459, 345);
            this.panel1.TabIndex = 3;
            // 
            // tbManualFile
            // 
            this.tbManualFile.Location = new System.Drawing.Point(637, 259);
            this.tbManualFile.Name = "tbManualFile";
            this.tbManualFile.Size = new System.Drawing.Size(784, 31);
            this.tbManualFile.TabIndex = 17;
            this.tbManualFile.Text = "\\\\TZ-AKL-SRS1\\TZForecastInterface\\Output\\TZNZ_ManualFcst_201903_190319.csv";
            // 
            // tbUsageFile
            // 
            this.tbUsageFile.Location = new System.Drawing.Point(637, 201);
            this.tbUsageFile.Name = "tbUsageFile";
            this.tbUsageFile.Size = new System.Drawing.Size(784, 31);
            this.tbUsageFile.TabIndex = 16;
            this.tbUsageFile.Text = "\\\\TZ-AKL-SRS1\\TZForecastInterface\\Output\\TZNZ_UsageFcst_201903_190319.csv";
            // 
            // tbCustEngFile
            // 
            this.tbCustEngFile.Location = new System.Drawing.Point(637, 152);
            this.tbCustEngFile.Name = "tbCustEngFile";
            this.tbCustEngFile.Size = new System.Drawing.Size(784, 31);
            this.tbCustEngFile.TabIndex = 15;
            this.tbCustEngFile.Text = "\\\\TZ-AKL-SRS1\\TZForecastInterface\\Output\\TZNZ_CustEngFcst_201903_190319.csv";
            // 
            // tbSmartForecastsFile
            // 
            this.tbSmartForecastsFile.Location = new System.Drawing.Point(637, 97);
            this.tbSmartForecastsFile.Name = "tbSmartForecastsFile";
            this.tbSmartForecastsFile.Size = new System.Drawing.Size(784, 31);
            this.tbSmartForecastsFile.TabIndex = 14;
            this.tbSmartForecastsFile.Text = "\\\\TZ-AKL-SRS1\\TZForecastInterface\\Output\\TZNZ_Smartforecasts_201903_190319.csv";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(504, 259);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(127, 25);
            this.label6.TabIndex = 13;
            this.label6.Text = "Source File:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(504, 152);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(127, 25);
            this.label5.TabIndex = 12;
            this.label5.Text = "Source File:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(504, 204);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(127, 25);
            this.label4.TabIndex = 11;
            this.label4.Text = "Source File:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(504, 100);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(127, 25);
            this.label1.TabIndex = 6;
            this.label1.Text = "Source File:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1092, 44);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 25);
            this.label3.TabIndex = 4;
            this.label3.Text = "All Sites:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(504, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(118, 25);
            this.label2.TabIndex = 3;
            this.label2.Text = "From Date:";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // cbClearFcst
            // 
            this.cbClearFcst.AutoSize = true;
            this.cbClearFcst.Checked = true;
            this.cbClearFcst.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbClearFcst.Location = new System.Drawing.Point(38, 42);
            this.cbClearFcst.Name = "cbClearFcst";
            this.cbClearFcst.Size = new System.Drawing.Size(262, 29);
            this.cbClearFcst.TabIndex = 6;
            this.cbClearFcst.Text = "Clear current forecasts";
            this.cbClearFcst.UseVisualStyleBackColor = true;
            // 
            // cbSmartFcsts
            // 
            this.cbSmartFcsts.AutoSize = true;
            this.cbSmartFcsts.Checked = true;
            this.cbSmartFcsts.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbSmartFcsts.Location = new System.Drawing.Point(38, 100);
            this.cbSmartFcsts.Name = "cbSmartFcsts";
            this.cbSmartFcsts.Size = new System.Drawing.Size(239, 29);
            this.cbSmartFcsts.TabIndex = 7;
            this.cbSmartFcsts.Text = "Add SmartForecasts";
            this.cbSmartFcsts.UseVisualStyleBackColor = true;
            // 
            // cbManualFcsts
            // 
            this.cbManualFcsts.AutoSize = true;
            this.cbManualFcsts.Checked = true;
            this.cbManualFcsts.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbManualFcsts.Location = new System.Drawing.Point(38, 258);
            this.cbManualFcsts.Name = "cbManualFcsts";
            this.cbManualFcsts.Size = new System.Drawing.Size(260, 29);
            this.cbManualFcsts.TabIndex = 10;
            this.cbManualFcsts.Text = "Add Manual Forecasts";
            this.cbManualFcsts.UseVisualStyleBackColor = true;
            // 
            // cbCustEngFcsts
            // 
            this.cbCustEngFcsts.AutoSize = true;
            this.cbCustEngFcsts.Checked = true;
            this.cbCustEngFcsts.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbCustEngFcsts.Location = new System.Drawing.Point(38, 148);
            this.cbCustEngFcsts.Name = "cbCustEngFcsts";
            this.cbCustEngFcsts.Size = new System.Drawing.Size(383, 29);
            this.cbCustEngFcsts.TabIndex = 8;
            this.cbCustEngFcsts.Text = "Add Custom Engineering Forecasts";
            this.cbCustEngFcsts.UseVisualStyleBackColor = true;
            // 
            // cbUsageFcsts
            // 
            this.cbUsageFcsts.AutoSize = true;
            this.cbUsageFcsts.Checked = true;
            this.cbUsageFcsts.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbUsageFcsts.Location = new System.Drawing.Point(38, 200);
            this.cbUsageFcsts.Name = "cbUsageFcsts";
            this.cbUsageFcsts.Size = new System.Drawing.Size(318, 29);
            this.cbUsageFcsts.TabIndex = 9;
            this.cbUsageFcsts.Text = "Add Usage Based Forecasts";
            this.cbUsageFcsts.UseVisualStyleBackColor = true;
            // 
            // lb_CompanyID
            // 
            this.lb_CompanyID.AutoSize = true;
            this.lb_CompanyID.Location = new System.Drawing.Point(329, 38);
            this.lb_CompanyID.Name = "lb_CompanyID";
            this.lb_CompanyID.Size = new System.Drawing.Size(0, 25);
            this.lb_CompanyID.TabIndex = 5;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.tbICFile);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.cbICFcsts);
            this.panel2.Location = new System.Drawing.Point(71, 514);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1459, 120);
            this.panel2.TabIndex = 6;
            // 
            // tbICFile
            // 
            this.tbICFile.Location = new System.Drawing.Point(637, 42);
            this.tbICFile.Name = "tbICFile";
            this.tbICFile.Size = new System.Drawing.Size(784, 31);
            this.tbICFile.TabIndex = 19;
            this.tbICFile.Text = "\\\\TZ-AKL-SRS1\\TZForecastInterface\\Output\\TZNZ_ICFcst_201903_190319.csv";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(495, 45);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(127, 25);
            this.label7.TabIndex = 18;
            this.label7.Text = "Source File:";
            this.label7.Click += new System.EventHandler(this.label7_Click);
            // 
            // cbICFcsts
            // 
            this.cbICFcsts.AutoSize = true;
            this.cbICFcsts.Checked = true;
            this.cbICFcsts.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbICFcsts.Location = new System.Drawing.Point(38, 40);
            this.cbICFcsts.Name = "cbICFcsts";
            this.cbICFcsts.Size = new System.Drawing.Size(329, 29);
            this.cbICFcsts.TabIndex = 0;
            this.cbICFcsts.Text = "Add Inter-Company Forecasts";
            this.cbICFcsts.UseVisualStyleBackColor = true;
            // 
            // btnProcess
            // 
            this.btnProcess.Location = new System.Drawing.Point(1302, 666);
            this.btnProcess.Name = "btnProcess";
            this.btnProcess.Size = new System.Drawing.Size(228, 50);
            this.btnProcess.TabIndex = 7;
            this.btnProcess.Text = "Process";
            this.btnProcess.UseVisualStyleBackColor = true;
            this.btnProcess.Click += new System.EventHandler(this.btnProcess_Click);
            // 
            // EpicorInterface
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1770, 772);
            this.Controls.Add(this.btnProcess);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.lb_CompanyID);
            this.Controls.Add(this.panel1);
            this.Name = "EpicorInterface";
            this.Text = "Epicor Update";
            this.Load += new System.EventHandler(this.EpicorInterface_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.CheckBox cbAllSites;
        private System.Windows.Forms.DateTimePicker dtFromDate;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lb_CompanyID;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cbClearFcst;
        private System.Windows.Forms.CheckBox cbSmartFcsts;
        private System.Windows.Forms.CheckBox cbManualFcsts;
        private System.Windows.Forms.CheckBox cbCustEngFcsts;
        private System.Windows.Forms.CheckBox cbUsageFcsts;
        private System.Windows.Forms.TextBox tbManualFile;
        private System.Windows.Forms.TextBox tbUsageFile;
        private System.Windows.Forms.TextBox tbCustEngFile;
        private System.Windows.Forms.TextBox tbSmartForecastsFile;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox tbICFile;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox cbICFcsts;
        private System.Windows.Forms.Button btnProcess;
    }
}