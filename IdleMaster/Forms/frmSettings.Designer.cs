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
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblSimultaneous1 = new System.Windows.Forms.Label();
            this.rdbMultiIdle = new System.Windows.Forms.RadioButton();
            this.grbIdleBehavior = new System.Windows.Forms.GroupBox();
            this.chkFastIdle = new System.Windows.Forms.CheckBox();
            this.lblSimultaneous2 = new System.Windows.Forms.Label();
            this.rdbSingleIdle = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.nudGamesToIdle)).BeginInit();
            this.grbIdleBehavior.SuspendLayout();
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
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(93, 148);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(174, 148);
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
            this.rdbMultiIdle.Size = new System.Drawing.Size(67, 17);
            this.rdbMultiIdle.TabIndex = 6;
            this.rdbMultiIdle.TabStop = true;
            this.rdbMultiIdle.Text = "Multi Idle";
            this.rdbMultiIdle.UseVisualStyleBackColor = true;
            this.rdbMultiIdle.CheckedChanged += new System.EventHandler(this.rdbMultiIdle_CheckedChanged);
            // 
            // grbIdleBehavior
            // 
            this.grbIdleBehavior.Controls.Add(this.chkFastIdle);
            this.grbIdleBehavior.Controls.Add(this.lblSimultaneous2);
            this.grbIdleBehavior.Controls.Add(this.rdbSingleIdle);
            this.grbIdleBehavior.Controls.Add(this.lblSimultaneous1);
            this.grbIdleBehavior.Controls.Add(this.rdbMultiIdle);
            this.grbIdleBehavior.Controls.Add(this.nudGamesToIdle);
            this.grbIdleBehavior.Location = new System.Drawing.Point(12, 12);
            this.grbIdleBehavior.Name = "grbIdleBehavior";
            this.grbIdleBehavior.Size = new System.Drawing.Size(237, 130);
            this.grbIdleBehavior.TabIndex = 7;
            this.grbIdleBehavior.TabStop = false;
            this.grbIdleBehavior.Text = "Idle behavior";
            // 
            // chkFastIdle
            // 
            this.chkFastIdle.AutoSize = true;
            this.chkFastIdle.Location = new System.Drawing.Point(6, 107);
            this.chkFastIdle.Name = "chkFastIdle";
            this.chkFastIdle.Size = new System.Drawing.Size(98, 17);
            this.chkFastIdle.TabIndex = 8;
            this.chkFastIdle.Text = "Enable fast idle";
            this.chkFastIdle.UseVisualStyleBackColor = true;
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
            this.rdbSingleIdle.Size = new System.Drawing.Size(74, 17);
            this.rdbSingleIdle.TabIndex = 5;
            this.rdbSingleIdle.TabStop = true;
            this.rdbSingleIdle.Text = "Single Idle";
            this.rdbSingleIdle.UseVisualStyleBackColor = true;
            // 
            // frmSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(261, 183);
            this.Controls.Add(this.grbIdleBehavior);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
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
            this.grbIdleBehavior.ResumeLayout(false);
            this.grbIdleBehavior.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NumericUpDown nudGamesToIdle;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblSimultaneous1;
        private System.Windows.Forms.RadioButton rdbMultiIdle;
        private System.Windows.Forms.GroupBox grbIdleBehavior;
        private System.Windows.Forms.Label lblSimultaneous2;
        private System.Windows.Forms.RadioButton rdbSingleIdle;
        private System.Windows.Forms.CheckBox chkFastIdle;
    }
}