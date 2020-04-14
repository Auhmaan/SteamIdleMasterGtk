namespace IdleMaster
{
    partial class frmMain
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
            this.components = new System.ComponentModel.Container();
            this.tmrSteamStatus = new System.Windows.Forms.Timer(this.components);
            this.tmrCookieStatus = new System.Windows.Forms.Timer(this.components);
            this.tmrLoadBadges = new System.Windows.Forms.Timer(this.components);
            this.tmrIdle = new System.Windows.Forms.Timer(this.components);
            this.lnkLogin = new System.Windows.Forms.LinkLabel();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnPause = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.lblSteam = new System.Windows.Forms.Label();
            this.lblLogin = new System.Windows.Forms.Label();
            this.lsbGames = new System.Windows.Forms.ListBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tmrSteamStatus
            // 
            this.tmrSteamStatus.Interval = 500;
            this.tmrSteamStatus.Tick += new System.EventHandler(this.tmrSteamStatus_Tick);
            // 
            // tmrCookieStatus
            // 
            this.tmrCookieStatus.Interval = 500;
            this.tmrCookieStatus.Tick += new System.EventHandler(this.tmrCookieStatus_Tick);
            // 
            // tmrLoadBadges
            // 
            this.tmrLoadBadges.Interval = 500;
            this.tmrLoadBadges.Tick += new System.EventHandler(this.tmrLoadBadges_Tick);
            // 
            // tmrIdle
            // 
            this.tmrIdle.Interval = 5000;
            this.tmrIdle.Tick += new System.EventHandler(this.tmrIdle_Tick);
            // 
            // lnkLogin
            // 
            this.lnkLogin.AutoSize = true;
            this.lnkLogin.Location = new System.Drawing.Point(12, 9);
            this.lnkLogin.Name = "lnkLogin";
            this.lnkLogin.Size = new System.Drawing.Size(33, 13);
            this.lnkLogin.TabIndex = 0;
            this.lnkLogin.TabStop = true;
            this.lnkLogin.Text = "Login";
            this.lnkLogin.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkLogin_LinkClicked);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(15, 304);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 1;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnPause
            // 
            this.btnPause.Location = new System.Drawing.Point(96, 304);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(75, 23);
            this.btnPause.TabIndex = 1;
            this.btnPause.Text = "Pause";
            this.btnPause.UseVisualStyleBackColor = true;
            this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(177, 304);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 1;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // lblSteam
            // 
            this.lblSteam.AutoSize = true;
            this.lblSteam.Location = new System.Drawing.Point(12, 31);
            this.lblSteam.Name = "lblSteam";
            this.lblSteam.Size = new System.Drawing.Size(37, 13);
            this.lblSteam.TabIndex = 2;
            this.lblSteam.Text = "Steam";
            // 
            // lblLogin
            // 
            this.lblLogin.AutoSize = true;
            this.lblLogin.Location = new System.Drawing.Point(12, 53);
            this.lblLogin.Name = "lblLogin";
            this.lblLogin.Size = new System.Drawing.Size(33, 13);
            this.lblLogin.TabIndex = 2;
            this.lblLogin.Text = "Login";
            // 
            // lsbGames
            // 
            this.lsbGames.FormattingEnabled = true;
            this.lsbGames.Location = new System.Drawing.Point(15, 73);
            this.lsbGames.Name = "lsbGames";
            this.lsbGames.Size = new System.Drawing.Size(237, 225);
            this.lsbGames.TabIndex = 3;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(177, 44);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 4;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(304, 339);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.lsbGames);
            this.Controls.Add(this.lblLogin);
            this.Controls.Add(this.lblSteam);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnPause);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.lnkLogin);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Idle Master";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer tmrSteamStatus;
        private System.Windows.Forms.Timer tmrCookieStatus;
        private System.Windows.Forms.Timer tmrLoadBadges;
        private System.Windows.Forms.Timer tmrIdle;
        private System.Windows.Forms.LinkLabel lnkLogin;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnPause;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Label lblSteam;
        private System.Windows.Forms.Label lblLogin;
        private System.Windows.Forms.ListBox lsbGames;
        private System.Windows.Forms.Button btnRefresh;
    }
}