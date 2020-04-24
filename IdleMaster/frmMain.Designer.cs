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
			this.lnkLogin = new System.Windows.Forms.LinkLabel();
			this.btnStart = new System.Windows.Forms.Button();
			this.btnPause = new System.Windows.Forms.Button();
			this.btnStop = new System.Windows.Forms.Button();
			this.lblSteam = new System.Windows.Forms.Label();
			this.btnRefresh = new System.Windows.Forms.Button();
			this.btnResume = new System.Windows.Forms.Button();
			this.ptbAvatar = new System.Windows.Forms.PictureBox();
			this.lblUsername = new System.Windows.Forms.Label();
			this.lsvBadges = new System.Windows.Forms.ListView();
			this.colGame = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colHoursPlayed = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colRemainingCards = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.lnkLogout = new System.Windows.Forms.LinkLabel();
			this.tmrIdleStatus = new System.Windows.Forms.Timer(this.components);
			this.btnSkip = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.ptbAvatar)).BeginInit();
			this.SuspendLayout();
			// 
			// tmrSteamStatus
			// 
			this.tmrSteamStatus.Interval = 500;
			this.tmrSteamStatus.Tick += new System.EventHandler(this.tmrSteamStatus_Tick);
			// 
			// lnkLogin
			// 
			this.lnkLogin.AutoSize = true;
			this.lnkLogin.Location = new System.Drawing.Point(9, 9);
			this.lnkLogin.Name = "lnkLogin";
			this.lnkLogin.Size = new System.Drawing.Size(33, 13);
			this.lnkLogin.TabIndex = 0;
			this.lnkLogin.TabStop = true;
			this.lnkLogin.Text = "Login";
			this.lnkLogin.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkLogin_LinkClicked);
			// 
			// btnStart
			// 
			this.btnStart.Location = new System.Drawing.Point(12, 320);
			this.btnStart.Name = "btnStart";
			this.btnStart.Size = new System.Drawing.Size(75, 23);
			this.btnStart.TabIndex = 1;
			this.btnStart.Text = "Start";
			this.btnStart.UseVisualStyleBackColor = true;
			this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
			// 
			// btnPause
			// 
			this.btnPause.Location = new System.Drawing.Point(174, 320);
			this.btnPause.Name = "btnPause";
			this.btnPause.Size = new System.Drawing.Size(75, 23);
			this.btnPause.TabIndex = 1;
			this.btnPause.Text = "Pause";
			this.btnPause.UseVisualStyleBackColor = true;
			this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
			// 
			// btnStop
			// 
			this.btnStop.Location = new System.Drawing.Point(336, 320);
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
			this.lblSteam.Location = new System.Drawing.Point(9, 31);
			this.lblSteam.Name = "lblSteam";
			this.lblSteam.Size = new System.Drawing.Size(37, 13);
			this.lblSteam.TabIndex = 2;
			this.lblSteam.Text = "Steam";
			// 
			// btnRefresh
			// 
			this.btnRefresh.Location = new System.Drawing.Point(82, 88);
			this.btnRefresh.Name = "btnRefresh";
			this.btnRefresh.Size = new System.Drawing.Size(75, 23);
			this.btnRefresh.TabIndex = 4;
			this.btnRefresh.Text = "Refresh";
			this.btnRefresh.UseVisualStyleBackColor = true;
			this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
			// 
			// btnResume
			// 
			this.btnResume.Location = new System.Drawing.Point(255, 320);
			this.btnResume.Name = "btnResume";
			this.btnResume.Size = new System.Drawing.Size(75, 23);
			this.btnResume.TabIndex = 1;
			this.btnResume.Text = "Resume";
			this.btnResume.UseVisualStyleBackColor = true;
			this.btnResume.Click += new System.EventHandler(this.btnResume_Click);
			// 
			// ptbAvatar
			// 
			this.ptbAvatar.Location = new System.Drawing.Point(12, 47);
			this.ptbAvatar.Name = "ptbAvatar";
			this.ptbAvatar.Size = new System.Drawing.Size(64, 64);
			this.ptbAvatar.TabIndex = 5;
			this.ptbAvatar.TabStop = false;
			// 
			// lblUsername
			// 
			this.lblUsername.AutoSize = true;
			this.lblUsername.Location = new System.Drawing.Point(82, 47);
			this.lblUsername.Name = "lblUsername";
			this.lblUsername.Size = new System.Drawing.Size(55, 13);
			this.lblUsername.TabIndex = 6;
			this.lblUsername.Text = "Username";
			// 
			// lsvBadges
			// 
			this.lsvBadges.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colGame,
            this.colHoursPlayed,
            this.colRemainingCards,
            this.colStatus});
			this.lsvBadges.FullRowSelect = true;
			this.lsvBadges.GridLines = true;
			this.lsvBadges.HideSelection = false;
			this.lsvBadges.Location = new System.Drawing.Point(12, 117);
			this.lsvBadges.Name = "lsvBadges";
			this.lsvBadges.Size = new System.Drawing.Size(431, 197);
			this.lsvBadges.TabIndex = 8;
			this.lsvBadges.UseCompatibleStateImageBehavior = false;
			this.lsvBadges.View = System.Windows.Forms.View.Details;
			// 
			// colGame
			// 
			this.colGame.Text = "Game";
			this.colGame.Width = 150;
			// 
			// colHoursPlayed
			// 
			this.colHoursPlayed.Text = "Hours Played";
			this.colHoursPlayed.Width = 100;
			// 
			// colRemainingCards
			// 
			this.colRemainingCards.Text = "Remaining Cards";
			this.colRemainingCards.Width = 100;
			// 
			// colStatus
			// 
			this.colStatus.Text = "Status";
			// 
			// lnkLogout
			// 
			this.lnkLogout.AutoSize = true;
			this.lnkLogout.Location = new System.Drawing.Point(47, 9);
			this.lnkLogout.Name = "lnkLogout";
			this.lnkLogout.Size = new System.Drawing.Size(40, 13);
			this.lnkLogout.TabIndex = 0;
			this.lnkLogout.TabStop = true;
			this.lnkLogout.Text = "Logout";
			this.lnkLogout.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkLogout_LinkClicked);
			// 
			// tmrIdleStatus
			// 
			this.tmrIdleStatus.Interval = 5000;
			this.tmrIdleStatus.Tick += new System.EventHandler(this.tmrIdleStatus_Tick);
			// 
			// btnSkip
			// 
			this.btnSkip.Location = new System.Drawing.Point(93, 320);
			this.btnSkip.Name = "btnSkip";
			this.btnSkip.Size = new System.Drawing.Size(75, 23);
			this.btnSkip.TabIndex = 1;
			this.btnSkip.Text = "Skip";
			this.btnSkip.UseVisualStyleBackColor = true;
			this.btnSkip.Click += new System.EventHandler(this.btnSkip_Click);
			// 
			// frmMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(454, 355);
			this.Controls.Add(this.lsvBadges);
			this.Controls.Add(this.lblUsername);
			this.Controls.Add(this.ptbAvatar);
			this.Controls.Add(this.btnRefresh);
			this.Controls.Add(this.lblSteam);
			this.Controls.Add(this.btnSkip);
			this.Controls.Add(this.btnResume);
			this.Controls.Add(this.btnStop);
			this.Controls.Add(this.btnPause);
			this.Controls.Add(this.btnStart);
			this.Controls.Add(this.lnkLogout);
			this.Controls.Add(this.lnkLogin);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "frmMain";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Idle Master";
			this.Load += new System.EventHandler(this.frmMain_Load);
			((System.ComponentModel.ISupportInitialize)(this.ptbAvatar)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer tmrSteamStatus;
        private System.Windows.Forms.LinkLabel lnkLogin;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnPause;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Label lblSteam;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnResume;
        private System.Windows.Forms.PictureBox ptbAvatar;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.ListView lsvBadges;
        private System.Windows.Forms.ColumnHeader colGame;
        private System.Windows.Forms.ColumnHeader colHoursPlayed;
        private System.Windows.Forms.ColumnHeader colRemainingCards;
        private System.Windows.Forms.LinkLabel lnkLogout;
        private System.Windows.Forms.Timer tmrIdleStatus;
		private System.Windows.Forms.ColumnHeader colStatus;
		private System.Windows.Forms.Button btnSkip;
	}
}