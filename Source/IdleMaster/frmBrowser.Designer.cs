using System.ComponentModel;
using System.Windows.Forms;

namespace IdleMaster
{
    partial class frmBrowser
    {
        ///<summary>
        ///Required designer variable.
        ///</summary>
        private IContainer components = null;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBrowser));
            this.wbSteam = new System.Windows.Forms.WebBrowser();
            this.lblSaving = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // wbSteam
            // 
            this.wbSteam.AllowWebBrowserDrop = false;
            this.wbSteam.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wbSteam.Location = new System.Drawing.Point(0, 0);
            this.wbSteam.MinimumSize = new System.Drawing.Size(20, 20);
            this.wbSteam.Name = "wbSteam";
            this.wbSteam.ScriptErrorsSuppressed = true;
            this.wbSteam.Size = new System.Drawing.Size(991, 481);
            this.wbSteam.TabIndex = 0;
            this.wbSteam.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.wbAuth_DocumentCompleted);
            // 
            // lblSaving
            // 
            this.lblSaving.AutoSize = true;
            this.lblSaving.Location = new System.Drawing.Point(34, 11);
            this.lblSaving.Name = "lblSaving";
            this.lblSaving.Size = new System.Drawing.Size(180, 13);
            this.lblSaving.TabIndex = 1;
            this.lblSaving.Text = "Idle Master is saving your information";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::IdleMaster.Properties.Resources.imgSpin;
            this.pictureBox1.Location = new System.Drawing.Point(13, 9);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(16, 16);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // frmBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(991, 481);
            this.Controls.Add(this.wbSteam);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lblSaving);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmBrowser";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Please Login to Steam";
            this.Load += new System.EventHandler(this.frmBrowser_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private WebBrowser wbSteam;
        private Label lblSaving;
        private PictureBox pictureBox1;
    }
}