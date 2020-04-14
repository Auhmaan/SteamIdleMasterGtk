namespace steam_idle
{
    partial class frmMain
    {
        ///<summary>
        ///Required designer variable.
        ///</summary>
        private System.ComponentModel.IContainer components = null;

        ///<summary>
        ///Clean up any resources being used.
        ///</summary>
        ///<param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        ///<summary>
        ///Required method for Designer support - do not modify
        ///the contents of this method with the code editor.
        ///</summary>
        private void InitializeComponent()
        {
            this.ptbSteamApp = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.ptbSteamApp)).BeginInit();
            this.SuspendLayout();
            // 
            // ptbSteamApp
            // 
            this.ptbSteamApp.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ptbSteamApp.Location = new System.Drawing.Point(-1, 0);
            this.ptbSteamApp.Name = "ptbSteamApp";
            this.ptbSteamApp.Size = new System.Drawing.Size(292, 136);
            this.ptbSteamApp.TabIndex = 0;
            this.ptbSteamApp.TabStop = false;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(291, 136);
            this.Controls.Add(this.ptbSteamApp);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMain";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            ((System.ComponentModel.ISupportInitialize)(this.ptbSteamApp)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox ptbSteamApp;
    }
}