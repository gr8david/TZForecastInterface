namespace TZForecastInterface
{
    partial class MainForm
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
            this.cbCompany = new System.Windows.Forms.ComboBox();
            this.btnGo = new System.Windows.Forms.Button();
            this.cbProcessSmartforecasts = new System.Windows.Forms.CheckBox();
            this.cbProcessCustEng = new System.Windows.Forms.CheckBox();
            this.cbProcessUsage = new System.Windows.Forms.CheckBox();
            this.tbForecastPeriod = new System.Windows.Forms.TextBox();
            this.lbForecastPeriod = new System.Windows.Forms.Label();
            this.nuCustEngPeriodsToAvg = new System.Windows.Forms.NumericUpDown();
            this.cbProcessIC = new System.Windows.Forms.CheckBox();
            this.tbImportFile = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbSmartforecastsMsg = new System.Windows.Forms.TextBox();
            this.tbCustEngMsg = new System.Windows.Forms.TextBox();
            this.tbUsageMsg = new System.Windows.Forms.TextBox();
            this.tbInterCoyMsg = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.utlilitiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label2 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.button2 = new System.Windows.Forms.Button();
            this.bgw1 = new System.ComponentModel.BackgroundWorker();
            this.cbCustEngSupressCP = new System.Windows.Forms.CheckBox();
            this.nuUsagePeriodsToAvg = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cbUsageSupressCP = new System.Windows.Forms.CheckBox();
            this.cbICSupressCP = new System.Windows.Forms.CheckBox();
            this.cbSMFSupressCP = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.nuCustEngFcstMnths = new System.Windows.Forms.NumericUpDown();
            this.nuUsageFcstMnths = new System.Windows.Forms.NumericUpDown();
            this.nnuICFcstMnths = new System.Windows.Forms.NumericUpDown();
            this.nuSMFFcstMnths = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.nuCustEngPeriodsToAvg)).BeginInit();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nuUsagePeriodsToAvg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nuCustEngFcstMnths)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nuUsageFcstMnths)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nnuICFcstMnths)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nuSMFFcstMnths)).BeginInit();
            this.SuspendLayout();
            // 
            // cbCompany
            // 
            this.cbCompany.FormattingEnabled = true;
            this.cbCompany.Items.AddRange(new object[] {
            "emperzone Australia Pty Ltd",
            "temperzone New Zealand Ltd"});
            this.cbCompany.Location = new System.Drawing.Point(371, 143);
            this.cbCompany.Margin = new System.Windows.Forms.Padding(10, 3, 3, 3);
            this.cbCompany.Name = "cbCompany";
            this.cbCompany.Size = new System.Drawing.Size(365, 33);
            this.cbCompany.TabIndex = 0;
            this.cbCompany.Text = "Temperzone Ltd";
            // 
            // btnGo
            // 
            this.btnGo.BackColor = System.Drawing.Color.Navy;
            this.btnGo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGo.ForeColor = System.Drawing.Color.White;
            this.btnGo.Location = new System.Drawing.Point(855, 611);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(144, 44);
            this.btnGo.TabIndex = 1;
            this.btnGo.Text = "Go";
            this.btnGo.UseVisualStyleBackColor = false;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // cbProcessSmartforecasts
            // 
            this.cbProcessSmartforecasts.AutoSize = true;
            this.cbProcessSmartforecasts.Location = new System.Drawing.Point(73, 462);
            this.cbProcessSmartforecasts.Name = "cbProcessSmartforecasts";
            this.cbProcessSmartforecasts.Size = new System.Drawing.Size(274, 29);
            this.cbProcessSmartforecasts.TabIndex = 2;
            this.cbProcessSmartforecasts.Text = "Smartforecasts Demand";
            this.cbProcessSmartforecasts.UseVisualStyleBackColor = true;
            // 
            // cbProcessCustEng
            // 
            this.cbProcessCustEng.AutoSize = true;
            this.cbProcessCustEng.Location = new System.Drawing.Point(73, 271);
            this.cbProcessCustEng.Name = "cbProcessCustEng";
            this.cbProcessCustEng.Size = new System.Drawing.Size(253, 29);
            this.cbProcessCustEng.TabIndex = 3;
            this.cbProcessCustEng.Text = "Custom Eng. Demand";
            this.cbProcessCustEng.UseVisualStyleBackColor = true;
            // 
            // cbProcessUsage
            // 
            this.cbProcessUsage.AutoSize = true;
            this.cbProcessUsage.Location = new System.Drawing.Point(73, 336);
            this.cbProcessUsage.Name = "cbProcessUsage";
            this.cbProcessUsage.Size = new System.Drawing.Size(259, 29);
            this.cbProcessUsage.TabIndex = 4;
            this.cbProcessUsage.Text = "Usage Based Demand";
            this.cbProcessUsage.UseVisualStyleBackColor = true;
            // 
            // tbForecastPeriod
            // 
            this.tbForecastPeriod.Location = new System.Drawing.Point(371, 208);
            this.tbForecastPeriod.Name = "tbForecastPeriod";
            this.tbForecastPeriod.Size = new System.Drawing.Size(221, 31);
            this.tbForecastPeriod.TabIndex = 5;
            this.tbForecastPeriod.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // lbForecastPeriod
            // 
            this.lbForecastPeriod.AutoSize = true;
            this.lbForecastPeriod.Location = new System.Drawing.Point(177, 214);
            this.lbForecastPeriod.Name = "lbForecastPeriod";
            this.lbForecastPeriod.Size = new System.Drawing.Size(170, 25);
            this.lbForecastPeriod.TabIndex = 6;
            this.lbForecastPeriod.Text = "Forecast Period:";
            // 
            // nuCustEngPeriodsToAvg
            // 
            this.nuCustEngPeriodsToAvg.Location = new System.Drawing.Point(1217, 271);
            this.nuCustEngPeriodsToAvg.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.nuCustEngPeriodsToAvg.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nuCustEngPeriodsToAvg.Name = "nuCustEngPeriodsToAvg";
            this.nuCustEngPeriodsToAvg.Size = new System.Drawing.Size(74, 31);
            this.nuCustEngPeriodsToAvg.TabIndex = 8;
            this.nuCustEngPeriodsToAvg.Value = new decimal(new int[] {
            12,
            0,
            0,
            0});
            // 
            // cbProcessIC
            // 
            this.cbProcessIC.AutoSize = true;
            this.cbProcessIC.Location = new System.Drawing.Point(73, 399);
            this.cbProcessIC.Name = "cbProcessIC";
            this.cbProcessIC.Size = new System.Drawing.Size(266, 29);
            this.cbProcessIC.TabIndex = 9;
            this.cbProcessIC.Text = "Inter-company Demand";
            this.cbProcessIC.UseVisualStyleBackColor = true;
            // 
            // tbImportFile
            // 
            this.tbImportFile.BackColor = System.Drawing.Color.White;
            this.tbImportFile.Location = new System.Drawing.Point(371, 523);
            this.tbImportFile.Name = "tbImportFile";
            this.tbImportFile.Size = new System.Drawing.Size(628, 31);
            this.tbImportFile.TabIndex = 10;
            this.tbImportFile.TextChanged += new System.EventHandler(this.tbImportFile_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(38, 526);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 25);
            this.label1.TabIndex = 11;
            // 
            // tbSmartforecastsMsg
            // 
            this.tbSmartforecastsMsg.BackColor = System.Drawing.Color.White;
            this.tbSmartforecastsMsg.Location = new System.Drawing.Point(371, 460);
            this.tbSmartforecastsMsg.Name = "tbSmartforecastsMsg";
            this.tbSmartforecastsMsg.Size = new System.Drawing.Size(628, 31);
            this.tbSmartforecastsMsg.TabIndex = 12;
            // 
            // tbCustEngMsg
            // 
            this.tbCustEngMsg.BackColor = System.Drawing.Color.White;
            this.tbCustEngMsg.Location = new System.Drawing.Point(371, 271);
            this.tbCustEngMsg.Name = "tbCustEngMsg";
            this.tbCustEngMsg.Size = new System.Drawing.Size(628, 31);
            this.tbCustEngMsg.TabIndex = 13;
            // 
            // tbUsageMsg
            // 
            this.tbUsageMsg.BackColor = System.Drawing.Color.White;
            this.tbUsageMsg.Location = new System.Drawing.Point(371, 334);
            this.tbUsageMsg.Name = "tbUsageMsg";
            this.tbUsageMsg.Size = new System.Drawing.Size(628, 31);
            this.tbUsageMsg.TabIndex = 14;
            // 
            // tbInterCoyMsg
            // 
            this.tbInterCoyMsg.BackColor = System.Drawing.Color.White;
            this.tbInterCoyMsg.Location = new System.Drawing.Point(371, 397);
            this.tbInterCoyMsg.Name = "tbInterCoyMsg";
            this.tbInterCoyMsg.Size = new System.Drawing.Size(628, 31);
            this.tbInterCoyMsg.TabIndex = 15;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.Navy;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(285, 611);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(316, 46);
            this.button1.TabIndex = 16;
            this.button1.Text = "Test Epicor Forecast Update";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.CornflowerBlue;
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.utlilitiesToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1517, 40);
            this.menuStrip1.TabIndex = 17;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(64, 36);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(164, 38);
            this.toolStripMenuItem1.Text = "Save";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(161, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(164, 38);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(157, 36);
            this.optionsToolStripMenuItem.Text = "Safety Stock";
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
            // 
            // utlilitiesToolStripMenuItem
            // 
            this.utlilitiesToolStripMenuItem.Name = "utlilitiesToolStripMenuItem";
            this.utlilitiesToolStripMenuItem.Size = new System.Drawing.Size(112, 36);
            this.utlilitiesToolStripMenuItem.Text = "Utlilities";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(238, 146);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(109, 25);
            this.label2.TabIndex = 18;
            this.label2.Text = "Company:";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.Navy;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.ForeColor = System.Drawing.Color.White;
            this.button2.Location = new System.Drawing.Point(80, 519);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(267, 44);
            this.button2.TabIndex = 22;
            this.button2.Text = "Smartforecasts File...";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // cbCustEngSupressCP
            // 
            this.cbCustEngSupressCP.AutoSize = true;
            this.cbCustEngSupressCP.Checked = true;
            this.cbCustEngSupressCP.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbCustEngSupressCP.Location = new System.Drawing.Point(1374, 269);
            this.cbCustEngSupressCP.Name = "cbCustEngSupressCP";
            this.cbCustEngSupressCP.Size = new System.Drawing.Size(28, 27);
            this.cbCustEngSupressCP.TabIndex = 23;
            this.cbCustEngSupressCP.UseVisualStyleBackColor = true;
            // 
            // nuUsagePeriodsToAvg
            // 
            this.nuUsagePeriodsToAvg.Location = new System.Drawing.Point(1217, 334);
            this.nuUsagePeriodsToAvg.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.nuUsagePeriodsToAvg.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nuUsagePeriodsToAvg.Name = "nuUsagePeriodsToAvg";
            this.nuUsagePeriodsToAvg.Size = new System.Drawing.Size(74, 31);
            this.nuUsagePeriodsToAvg.TabIndex = 24;
            this.nuUsagePeriodsToAvg.Value = new decimal(new int[] {
            12,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(1193, 155);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(113, 56);
            this.label4.TabIndex = 25;
            this.label4.Text = "Periods to Average";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(1312, 146);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(155, 75);
            this.label5.TabIndex = 26;
            this.label5.Text = "Supress Current Period Forecast";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cbUsageSupressCP
            // 
            this.cbUsageSupressCP.AutoSize = true;
            this.cbUsageSupressCP.Location = new System.Drawing.Point(1374, 334);
            this.cbUsageSupressCP.Name = "cbUsageSupressCP";
            this.cbUsageSupressCP.Size = new System.Drawing.Size(28, 27);
            this.cbUsageSupressCP.TabIndex = 27;
            this.cbUsageSupressCP.UseVisualStyleBackColor = true;
            // 
            // cbICSupressCP
            // 
            this.cbICSupressCP.AutoSize = true;
            this.cbICSupressCP.Location = new System.Drawing.Point(1374, 397);
            this.cbICSupressCP.Name = "cbICSupressCP";
            this.cbICSupressCP.Size = new System.Drawing.Size(28, 27);
            this.cbICSupressCP.TabIndex = 28;
            this.cbICSupressCP.UseVisualStyleBackColor = true;
            // 
            // cbSMFSupressCP
            // 
            this.cbSMFSupressCP.AutoSize = true;
            this.cbSMFSupressCP.Location = new System.Drawing.Point(1374, 460);
            this.cbSMFSupressCP.Name = "cbSMFSupressCP";
            this.cbSMFSupressCP.Size = new System.Drawing.Size(28, 27);
            this.cbSMFSupressCP.TabIndex = 29;
            this.cbSMFSupressCP.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(1017, 146);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(146, 85);
            this.label3.TabIndex = 30;
            this.label3.Text = "Number of Forecast Months";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // nuCustEngFcstMnths
            // 
            this.nuCustEngFcstMnths.Location = new System.Drawing.Point(1055, 269);
            this.nuCustEngFcstMnths.Maximum = new decimal(new int[] {
            24,
            0,
            0,
            0});
            this.nuCustEngFcstMnths.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nuCustEngFcstMnths.Name = "nuCustEngFcstMnths";
            this.nuCustEngFcstMnths.Size = new System.Drawing.Size(74, 31);
            this.nuCustEngFcstMnths.TabIndex = 31;
            this.nuCustEngFcstMnths.Value = new decimal(new int[] {
            12,
            0,
            0,
            0});
            // 
            // nuUsageFcstMnths
            // 
            this.nuUsageFcstMnths.Location = new System.Drawing.Point(1055, 336);
            this.nuUsageFcstMnths.Maximum = new decimal(new int[] {
            24,
            0,
            0,
            0});
            this.nuUsageFcstMnths.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nuUsageFcstMnths.Name = "nuUsageFcstMnths";
            this.nuUsageFcstMnths.Size = new System.Drawing.Size(74, 31);
            this.nuUsageFcstMnths.TabIndex = 32;
            this.nuUsageFcstMnths.Value = new decimal(new int[] {
            12,
            0,
            0,
            0});
            // 
            // nnuICFcstMnths
            // 
            this.nnuICFcstMnths.Location = new System.Drawing.Point(1055, 399);
            this.nnuICFcstMnths.Maximum = new decimal(new int[] {
            24,
            0,
            0,
            0});
            this.nnuICFcstMnths.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nnuICFcstMnths.Name = "nnuICFcstMnths";
            this.nnuICFcstMnths.Size = new System.Drawing.Size(74, 31);
            this.nnuICFcstMnths.TabIndex = 33;
            this.nnuICFcstMnths.Value = new decimal(new int[] {
            12,
            0,
            0,
            0});
            // 
            // nuSMFFcstMnths
            // 
            this.nuSMFFcstMnths.Location = new System.Drawing.Point(1055, 462);
            this.nuSMFFcstMnths.Maximum = new decimal(new int[] {
            24,
            0,
            0,
            0});
            this.nuSMFFcstMnths.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nuSMFFcstMnths.Name = "nuSMFFcstMnths";
            this.nuSMFFcstMnths.Size = new System.Drawing.Size(74, 31);
            this.nuSMFFcstMnths.TabIndex = 34;
            this.nuSMFFcstMnths.Value = new decimal(new int[] {
            12,
            0,
            0,
            0});
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ClientSize = new System.Drawing.Size(1517, 727);
            this.Controls.Add(this.nuSMFFcstMnths);
            this.Controls.Add(this.nnuICFcstMnths);
            this.Controls.Add(this.nuUsageFcstMnths);
            this.Controls.Add(this.nuCustEngFcstMnths);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbSMFSupressCP);
            this.Controls.Add(this.cbICSupressCP);
            this.Controls.Add(this.cbUsageSupressCP);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.nuUsagePeriodsToAvg);
            this.Controls.Add(this.cbCustEngSupressCP);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tbInterCoyMsg);
            this.Controls.Add(this.tbUsageMsg);
            this.Controls.Add(this.tbCustEngMsg);
            this.Controls.Add(this.tbSmartforecastsMsg);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbImportFile);
            this.Controls.Add(this.cbProcessIC);
            this.Controls.Add(this.nuCustEngPeriodsToAvg);
            this.Controls.Add(this.lbForecastPeriod);
            this.Controls.Add(this.tbForecastPeriod);
            this.Controls.Add(this.cbProcessUsage);
            this.Controls.Add(this.cbProcessCustEng);
            this.Controls.Add(this.cbProcessSmartforecasts);
            this.Controls.Add(this.btnGo);
            this.Controls.Add(this.cbCompany);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Temperzone Forecast Interface";
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nuCustEngPeriodsToAvg)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nuUsagePeriodsToAvg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nuCustEngFcstMnths)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nuUsageFcstMnths)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nnuICFcstMnths)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nuSMFFcstMnths)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbCompany;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.CheckBox cbProcessSmartforecasts;
        private System.Windows.Forms.CheckBox cbProcessCustEng;
        private System.Windows.Forms.CheckBox cbProcessUsage;
        private System.Windows.Forms.TextBox tbForecastPeriod;
        private System.Windows.Forms.Label lbForecastPeriod;
        private System.Windows.Forms.NumericUpDown nuCustEngPeriodsToAvg;
        private System.Windows.Forms.CheckBox cbProcessIC;
        private System.Windows.Forms.TextBox tbImportFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbSmartforecastsMsg;
        private System.Windows.Forms.TextBox tbCustEngMsg;
        private System.Windows.Forms.TextBox tbUsageMsg;
        private System.Windows.Forms.TextBox tbInterCoyMsg;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button button2;
        private System.ComponentModel.BackgroundWorker bgw1;
        private System.Windows.Forms.CheckBox cbCustEngSupressCP;
        private System.Windows.Forms.NumericUpDown nuUsagePeriodsToAvg;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox cbUsageSupressCP;
        private System.Windows.Forms.CheckBox cbICSupressCP;
        private System.Windows.Forms.CheckBox cbSMFSupressCP;
        private System.Windows.Forms.ToolStripMenuItem utlilitiesToolStripMenuItem;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nuCustEngFcstMnths;
        private System.Windows.Forms.NumericUpDown nuUsageFcstMnths;
        private System.Windows.Forms.NumericUpDown nnuICFcstMnths;
        private System.Windows.Forms.NumericUpDown nuSMFFcstMnths;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    }
}

