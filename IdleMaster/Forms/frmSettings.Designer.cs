namespace IdleMaster.Forms
{
    partial class frmSettings
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
            this.nudGamesToIdle = new System.Windows.Forms.NumericUpDown();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblSimultaneous1 = new System.Windows.Forms.Label();
            this.rdbMultiIdle = new System.Windows.Forms.RadioButton();
            this.grbIdleFormat = new System.Windows.Forms.GroupBox();
            this.lblSimultaneous2 = new System.Windows.Forms.Label();
            this.rdbSingleIdle = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.nudGamesToIdle)).BeginInit();
            this.grbIdleFormat.SuspendLayout();
            this.SuspendLayout();
            // 
            // nudGamesToIdle
            // 
            this.nudGamesToIdle.Enabled = false;
            this.nudGamesToIdle.Location = new System.Drawing.Point(45, 69);
            this.nudGamesToIdle.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.nudGamesToIdle.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.nudGamesToIdle.Name = "nudGamesToIdle";
            this.nudGamesToIdle.Size = new System.Drawing.Size(35, 20);
            this.nudGamesToIdle.TabIndex = 0;
            this.nudGamesToIdle.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudGamesToIdle.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(174, 123);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 23);
            this.btnApply.TabIndex = 1;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(12, 123);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(93, 123);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblSimultaneous1
            // 
            this.lblSimultaneous1.AutoSize = true;
            this.lblSimultaneous1.Enabled = false;
            this.lblSimultaneous1.Location = new System.Drawing.Point(6, 71);
            this.lblSimultaneous1.Name = "lblSimultaneous1";
            this.lblSimultaneous1.Size = new System.Drawing.Size(33, 13);
            this.lblSimultaneous1.TabIndex = 4;
            this.lblSimultaneous1.Text = "Up to";
            // 
            // rdbMultiIdle
            // 
            this.rdbMultiIdle.AutoSize = true;
            this.rdbMultiIdle.Location = new System.Drawing.Point(6, 51);
            this.rdbMultiIdle.Name = "rdbMultiIdle";
            this.rdbMultiIdle.Size = new System.Drawing.Size(75, 17);
            this.rdbMultiIdle.TabIndex = 6;
            this.rdbMultiIdle.TabStop = true;
            this.rdbMultiIdle.Text = "Multi Idling";
            this.rdbMultiIdle.UseVisualStyleBackColor = true;
            this.rdbMultiIdle.CheckedChanged += new System.EventHandler(this.rdbMultiIdle_CheckedChanged);
            // 
            // grbIdleFormat
            // 
            this.grbIdleFormat.Controls.Add(this.lblSimultaneous2);
            this.grbIdleFormat.Controls.Add(this.rdbSingleIdle);
            this.grbIdleFormat.Controls.Add(this.lblSimultaneous1);
            this.grbIdleFormat.Controls.Add(this.rdbMultiIdle);
            this.grbIdleFormat.Controls.Add(this.nudGamesToIdle);
            this.grbIdleFormat.Location = new System.Drawing.Point(12, 12);
            this.grbIdleFormat.Name = "grbIdleFormat";
            this.grbIdleFormat.Size = new System.Drawing.Size(237, 105);
            this.grbIdleFormat.TabIndex = 7;
            this.grbIdleFormat.TabStop = false;
            this.grbIdleFormat.Text = "Idling format";
            // 
            // lblSimultaneous2
            // 
            this.lblSimultaneous2.AutoSize = true;
            this.lblSimultaneous2.Enabled = false;
            this.lblSimultaneous2.Location = new System.Drawing.Point(86, 71);
            this.lblSimultaneous2.Name = "lblSimultaneous2";
            this.lblSimultaneous2.Size = new System.Drawing.Size(109, 13);
            this.lblSimultaneous2.TabIndex = 7;
            this.lblSimultaneous2.Text = "games simultaneously";
            // 
            // rdbSingleIdle
            // 
            this.rdbSingleIdle.AutoSize = true;
            this.rdbSingleIdle.Location = new System.Drawing.Point(6, 28);
            this.rdbSingleIdle.Name = "rdbSingleIdle";
            this.rdbSingleIdle.Size = new System.Drawing.Size(82, 17);
            this.rdbSingleIdle.TabIndex = 5;
            this.rdbSingleIdle.TabStop = true;
            this.rdbSingleIdle.Text = "Single Idling";
            this.rdbSingleIdle.UseVisualStyleBackColor = true;
            // 
            // frmSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(261, 158);
            this.Controls.Add(this.grbIdleFormat);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnApply);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSettings";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.frmSettings_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudGamesToIdle)).EndInit();
            this.grbIdleFormat.ResumeLayout(false);
            this.grbIdleFormat.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NumericUpDown nudGamesToIdle;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblSimultaneous1;
        private System.Windows.Forms.RadioButton rdbMultiIdle;
        private System.Windows.Forms.GroupBox grbIdleFormat;
        private System.Windows.Forms.Label lblSimultaneous2;
        private System.Windows.Forms.RadioButton rdbSingleIdle;
    }
}