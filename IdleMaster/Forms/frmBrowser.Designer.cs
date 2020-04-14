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
            this.wbSteam = new System.Windows.Forms.WebBrowser();
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
            // frmBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(991, 481);
            this.Controls.Add(this.wbSteam);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmBrowser";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Please Login to Steam";
            this.Load += new System.EventHandler(this.frmBrowser_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private WebBrowser wbSteam;
    }
}