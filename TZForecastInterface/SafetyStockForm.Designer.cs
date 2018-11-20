namespace TZForecastInterface
{
    partial class SafetyStockForm
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
            this.btnSSProcess = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cbCompany = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.n_FcstPeriod = new System.Windows.Forms.NumericUpDown();
            this.n_NumPeriods = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.n_FcstPeriod)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.n_NumPeriods)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSSProcess
            // 
            this.btnSSProcess.Location = new System.Drawing.Point(1224, 694);
            this.btnSSProcess.Name = "btnSSProcess";
            this.btnSSProcess.Size = new System.Drawing.Size(110, 84);
            this.btnSSProcess.TabIndex = 0;
            this.btnSSProcess.Text = "Process";
            this.btnSSProcess.UseVisualStyleBackColor = true;
            this.btnSSProcess.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(90, 100);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 25);
            this.label1.TabIndex = 1;
            this.label1.Text = "Company:";
            // 
            // cbCompany
            // 
            this.cbCompany.FormattingEnabled = true;
            this.cbCompany.Location = new System.Drawing.Point(230, 97);
            this.cbCompany.Name = "cbCompany";
            this.cbCompany.Size = new System.Drawing.Size(299, 33);
            this.cbCompany.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(129, 260);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 25);
            this.label2.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(129, 318);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(0, 25);
            this.label3.TabIndex = 4;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(216, 199);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(150, 29);
            this.checkBox1.TabIndex = 5;
            this.checkBox1.Text = "checkBox1";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(216, 260);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(150, 29);
            this.checkBox2.TabIndex = 6;
            this.checkBox2.Text = "checkBox2";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(626, 410);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(162, 25);
            this.label4.TabIndex = 7;
            this.label4.Text = "Forecast period";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(488, 471);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(300, 25);
            this.label5.TabIndex = 8;
            this.label5.Text = "Number of Periods to Average";
            // 
            // n_FcstPeriod
            // 
            this.n_FcstPeriod.Location = new System.Drawing.Point(807, 414);
            this.n_FcstPeriod.Maximum = new decimal(new int[] {
            202412,
            0,
            0,
            0});
            this.n_FcstPeriod.Minimum = new decimal(new int[] {
            201801,
            0,
            0,
            0});
            this.n_FcstPeriod.Name = "n_FcstPeriod";
            this.n_FcstPeriod.Size = new System.Drawing.Size(120, 31);
            this.n_FcstPeriod.TabIndex = 9;
            this.n_FcstPeriod.Value = new decimal(new int[] {
            201810,
            0,
            0,
            0});
            // 
            // n_NumPeriods
            // 
            this.n_NumPeriods.Location = new System.Drawing.Point(807, 469);
            this.n_NumPeriods.Name = "n_NumPeriods";
            this.n_NumPeriods.Size = new System.Drawing.Size(120, 31);
            this.n_NumPeriods.TabIndex = 10;
            // 
            // SafetyStockForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1571, 867);
            this.Controls.Add(this.n_NumPeriods);
            this.Controls.Add(this.n_FcstPeriod);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbCompany);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSSProcess);
            this.Name = "SafetyStockForm";
            this.Text = "Safety Stock";
            this.Load += new System.EventHandler(this.SafetyStockForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.n_FcstPeriod)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.n_NumPeriods)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSSProcess;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbCompany;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown n_FcstPeriod;
        private System.Windows.Forms.NumericUpDown n_NumPeriods;
    }
}