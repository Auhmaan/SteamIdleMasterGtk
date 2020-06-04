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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
			this.tmrSteamStatus = new System.Windows.Forms.Timer(this.components);
			this.lnkSession = new System.Windows.Forms.LinkLabel();
			this.lblSteam = new System.Windows.Forms.Label();
			this.lblUsername = new System.Windows.Forms.Label();
			this.lsvGames = new System.Windows.Forms.ListView();
			this.colGame = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colHoursPlayed = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colRemainingCards = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.colStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.tmrNormalIdleStatus = new System.Windows.Forms.Timer(this.components);
			this.tmrFastIdleStart = new System.Windows.Forms.Timer(this.components);
			this.tmrFastIdleStop = new System.Windows.Forms.Timer(this.components);
			this.mnsMainMenu = new System.Windows.Forms.MenuStrip();
			this.tsiSettings = new System.Windows.Forms.ToolStripMenuItem();
			this.btnSkip = new System.Windows.Forms.Button();
			this.btnPauseResume = new System.Windows.Forms.Button();
			this.ptbAvatar = new System.Windows.Forms.PictureBox();
			this.btnRefresh = new System.Windows.Forms.Button();
			this.btnStop = new System.Windows.Forms.Button();
			this.btnStart = new System.Windows.Forms.Button();
			this.tsiBlacklist = new System.Windows.Forms.ToolStripMenuItem();
			this.mnsMainMenu.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.ptbAvatar)).BeginInit();
			this.SuspendLayout();
			// 
			// tmrSteamStatus
			// 
			this.tmrSteamStatus.Interval = 500;
			this.tmrSteamStatus.Tick += new System.EventHandler(this.tmrSteamStatus_Tick);
			// 
			// lnkSession
			// 
			this.lnkSession.AutoSize = true;
			this.lnkSession.Location = new System.Drawing.Point(403, 33);
			this.lnkSession.Name = "lnkSession";
			this.lnkSession.Size = new System.Drawing.Size(33, 13);
			this.lnkSession.TabIndex = 1;
			this.lnkSession.TabStop = true;
			this.lnkSession.Text = "Login";
			this.lnkSession.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkSession_LinkClicked);
			// 
			// lblSteam
			// 
			this.lblSteam.AutoSize = true;
			this.lblSteam.Location = new System.Drawing.Point(9, 33);
			this.lblSteam.Name = "lblSteam";
			this.lblSteam.Size = new System.Drawing.Size(37, 13);
			this.lblSteam.TabIndex = 0;
			this.lblSteam.Text = "Steam";
			// 
			// lblUsername
			// 
			this.lblUsername.AutoSize = true;
			this.lblUsername.Location = new System.Drawing.Point(79, 62);
			this.lblUsername.Name = "lblUsername";
			this.lblUsername.Size = new System.Drawing.Size(55, 13);
			this.lblUsername.TabIndex = 2;
			this.lblUsername.Text = "Username";
			// 
			// lsvGames
			// 
			this.lsvGames.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colGame,
            this.colHoursPlayed,
            this.colRemainingCards,
            this.colStatus});
			this.lsvGames.FullRowSelect = true;
			this.lsvGames.GridLines = true;
			this.lsvGames.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.lsvGames.HideSelection = false;
			this.lsvGames.Location = new System.Drawing.Point(12, 132);
			this.lsvGames.MultiSelect = false;
			this.lsvGames.Name = "lsvGames";
			this.lsvGames.Size = new System.Drawing.Size(431, 197);
			this.lsvGames.TabIndex = 7;
			this.lsvGames.UseCompatibleStateImageBehavior = false;
			this.lsvGames.View = System.Windows.Forms.View.Details;
			this.lsvGames.ColumnWidthChanging += new System.Windows.Forms.ColumnWidthChangingEventHandler(this.lsvGames_ColumnWidthChanging);
			this.lsvGames.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.lsvGames_ItemSelectionChanged);
			this.lsvGames.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lsvGames_KeyDown);
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
			// tmrNormalIdleStatus
			// 
			this.tmrNormalIdleStatus.Interval = 60000;
			this.tmrNormalIdleStatus.Tick += new System.EventHandler(this.tmrNormalIdleStatus_Tick);
			// 
			// tmrFastIdleStart
			// 
			this.tmrFastIdleStart.Interval = 5000;
			this.tmrFastIdleStart.Tick += new System.EventHandler(this.tmrFastIdleStart_Tick);
			// 
			// tmrFastIdleStop
			// 
			this.tmrFastIdleStop.Interval = 10000;
			this.tmrFastIdleStop.Tick += new System.EventHandler(this.tmrFastIdleStop_Tick);
			// 
			// mnsMainMenu
			// 
			this.mnsMainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsiSettings,
            this.tsiBlacklist});
			this.mnsMainMenu.Location = new System.Drawing.Point(0, 0);
			this.mnsMainMenu.Name = "mnsMainMenu";
			this.mnsMainMenu.Size = new System.Drawing.Size(455, 24);
			this.mnsMainMenu.TabIndex = 8;
			this.mnsMainMenu.Text = "menuStrip1";
			// 
			// tsiSettings
			// 
			this.tsiSettings.Name = "tsiSettings";
			this.tsiSettings.Size = new System.Drawing.Size(61, 20);
			this.tsiSettings.Text = "Settings";
			this.tsiSettings.Click += new System.EventHandler(this.tsiSettings_Click);
			// 
			// btnSkip
			// 
			this.btnSkip.BackgroundImage = global::IdleMaster.Properties.Resources.Skip;
			this.btnSkip.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.btnSkip.Location = new System.Drawing.Point(372, 94);
			this.btnSkip.Name = "btnSkip";
			this.btnSkip.Size = new System.Drawing.Size(32, 32);
			this.btnSkip.TabIndex = 10;
			this.btnSkip.UseVisualStyleBackColor = true;
			this.btnSkip.Click += new System.EventHandler(this.btnSkip_Click);
			// 
			// btnPauseResume
			// 
			this.btnPauseResume.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnPauseResume.BackgroundImage")));
			this.btnPauseResume.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.btnPauseResume.Location = new System.Drawing.Point(334, 94);
			this.btnPauseResume.Name = "btnPauseResume";
			this.btnPauseResume.Size = new System.Drawing.Size(32, 32);
			this.btnPauseResume.TabIndex = 9;
			this.btnPauseResume.UseVisualStyleBackColor = true;
			this.btnPauseResume.Click += new System.EventHandler(this.btnPauseResume_Click);
			// 
			// ptbAvatar
			// 
			this.ptbAvatar.Location = new System.Drawing.Point(12, 62);
			this.ptbAvatar.Name = "ptbAvatar";
			this.ptbAvatar.Size = new System.Drawing.Size(64, 64);
			this.ptbAvatar.TabIndex = 5;
			this.ptbAvatar.TabStop = false;
			// 
			// btnRefresh
			// 
			this.btnRefresh.BackgroundImage = global::IdleMaster.Properties.Resources.Refresh;
			this.btnRefresh.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.btnRefresh.Location = new System.Drawing.Point(82, 94);
			this.btnRefresh.Name = "btnRefresh";
			this.btnRefresh.Size = new System.Drawing.Size(32, 32);
			this.btnRefresh.TabIndex = 3;
			this.btnRefresh.UseVisualStyleBackColor = true;
			this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
			// 
			// btnStop
			// 
			this.btnStop.BackgroundImage = global::IdleMaster.Properties.Resources.Stop;
			this.btnStop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.btnStop.Location = new System.Drawing.Point(410, 94);
			this.btnStop.Name = "btnStop";
			this.btnStop.Size = new System.Drawing.Size(33, 32);
			this.btnStop.TabIndex = 6;
			this.btnStop.UseVisualStyleBackColor = true;
			this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
			// 
			// btnStart
			// 
			this.btnStart.BackgroundImage = global::IdleMaster.Properties.Resources.Play;
			this.btnStart.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.btnStart.Location = new System.Drawing.Point(296, 94);
			this.btnStart.Name = "btnStart";
			this.btnStart.Size = new System.Drawing.Size(32, 32);
			this.btnStart.TabIndex = 4;
			this.btnStart.UseVisualStyleBackColor = true;
			this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
			// 
			// tsiBlacklist
			// 
			this.tsiBlacklist.Name = "tsiBlacklist";
			this.tsiBlacklist.Size = new System.Drawing.Size(62, 20);
			this.tsiBlacklist.Text = "Blacklist";
			this.tsiBlacklist.Click += new System.EventHandler(this.tsiBlacklist_Click);
			// 
			// frmMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(455, 341);
			this.Controls.Add(this.btnSkip);
			this.Controls.Add(this.btnPauseResume);
			this.Controls.Add(this.lsvGames);
			this.Controls.Add(this.lblUsername);
			this.Controls.Add(this.ptbAvatar);
			this.Controls.Add(this.btnRefresh);
			this.Controls.Add(this.lblSteam);
			this.Controls.Add(this.btnStop);
			this.Controls.Add(this.btnStart);
			this.Controls.Add(this.lnkSession);
			this.Controls.Add(this.mnsMainMenu);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "frmMain";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Idle Master";
			this.Load += new System.EventHandler(this.frmMain_Load);
			this.mnsMainMenu.ResumeLayout(false);
			this.mnsMainMenu.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.ptbAvatar)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer tmrSteamStatus;
        private System.Windows.Forms.LinkLabel lnkSession;
        private System.Windows.Forms.Button btnStart;
		private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Label lblSteam;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.PictureBox ptbAvatar;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.ListView lsvGames;
        private System.Windows.Forms.ColumnHeader colGame;
        private System.Windows.Forms.ColumnHeader colHoursPlayed;
        private System.Windows.Forms.ColumnHeader colRemainingCards;
        private System.Windows.Forms.Timer tmrNormalIdleStatus;
        private System.Windows.Forms.Timer tmrFastIdleStart;
        private System.Windows.Forms.ColumnHeader colStatus;
        private System.Windows.Forms.Timer tmrFastIdleStop;
        private System.Windows.Forms.MenuStrip mnsMainMenu;
        private System.Windows.Forms.Button btnPauseResume;
        private System.Windows.Forms.Button btnSkip;
        private System.Windows.Forms.ToolStripMenuItem tsiSettings;
		private System.Windows.Forms.ToolStripMenuItem tsiBlacklist;
	}
}