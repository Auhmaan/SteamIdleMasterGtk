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
            this.lblSteamStatus = new System.Windows.Forms.Label();
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
            this.tsiBlacklist = new System.Windows.Forms.ToolStripMenuItem();
            this.tsiSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.tbcPanels = new System.Windows.Forms.TabControl();
            this.tabBadgeIdle = new System.Windows.Forms.TabPage();
            this.lblOriginalRemainingCards = new System.Windows.Forms.Label();
            this.lblDroppedCards = new System.Windows.Forms.Label();
            this.prgDroppedCards = new System.Windows.Forms.ProgressBar();
            this.ptbAvatar = new System.Windows.Forms.PictureBox();
            this.btnSkip = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnPauseResume = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.tabManualIdle = new System.Windows.Forms.TabPage();
            this.btnManualStop = new System.Windows.Forms.Button();
            this.btnManualStart = new System.Windows.Forms.Button();
            this.lblAppId = new System.Windows.Forms.Label();
            this.txtAppId = new System.Windows.Forms.TextBox();
            this.lsbManualIdle = new System.Windows.Forms.ListBox();
            this.lblSteam = new System.Windows.Forms.Label();
            this.lblGames = new System.Windows.Forms.Label();
            this.lblGamesToIdle = new System.Windows.Forms.Label();
            this.mnsMainMenu.SuspendLayout();
            this.tbcPanels.SuspendLayout();
            this.tabBadgeIdle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ptbAvatar)).BeginInit();
            this.tabManualIdle.SuspendLayout();
            this.SuspendLayout();
            // 
            // tmrSteamStatus
            // 
            this.tmrSteamStatus.Tick += new System.EventHandler(this.tmrSteamStatus_Tick);
            // 
            // lnkSession
            // 
            this.lnkSession.AutoSize = true;
            this.lnkSession.Location = new System.Drawing.Point(419, 33);
            this.lnkSession.Name = "lnkSession";
            this.lnkSession.Size = new System.Drawing.Size(33, 13);
            this.lnkSession.TabIndex = 2;
            this.lnkSession.TabStop = true;
            this.lnkSession.Text = "Login";
            this.lnkSession.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkSession_LinkClicked);
            // 
            // lblSteamStatus
            // 
            this.lblSteamStatus.AutoSize = true;
            this.lblSteamStatus.Location = new System.Drawing.Point(76, 33);
            this.lblSteamStatus.Name = "lblSteamStatus";
            this.lblSteamStatus.Size = new System.Drawing.Size(47, 13);
            this.lblSteamStatus.TabIndex = 1;
            this.lblSteamStatus.Text = "Running";
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(73, 6);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(55, 13);
            this.lblUsername.TabIndex = 0;
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
            this.lsvGames.Location = new System.Drawing.Point(6, 76);
            this.lsvGames.MultiSelect = false;
            this.lsvGames.Name = "lsvGames";
            this.lsvGames.Size = new System.Drawing.Size(431, 197);
            this.lsvGames.TabIndex = 6;
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
            this.tmrFastIdleStop.Interval = 30000;
            this.tmrFastIdleStop.Tick += new System.EventHandler(this.tmrFastIdleStop_Tick);
            // 
            // mnsMainMenu
            // 
            this.mnsMainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsiBlacklist,
            this.tsiSettings});
            this.mnsMainMenu.Location = new System.Drawing.Point(0, 0);
            this.mnsMainMenu.Name = "mnsMainMenu";
            this.mnsMainMenu.Size = new System.Drawing.Size(475, 24);
            this.mnsMainMenu.TabIndex = 0;
            this.mnsMainMenu.Text = "menuStrip1";
            // 
            // tsiBlacklist
            // 
            this.tsiBlacklist.Name = "tsiBlacklist";
            this.tsiBlacklist.Size = new System.Drawing.Size(62, 20);
            this.tsiBlacklist.Text = "Blacklist";
            this.tsiBlacklist.Click += new System.EventHandler(this.tsiBlacklist_Click);
            // 
            // tsiSettings
            // 
            this.tsiSettings.Name = "tsiSettings";
            this.tsiSettings.Size = new System.Drawing.Size(61, 20);
            this.tsiSettings.Text = "Settings";
            this.tsiSettings.Click += new System.EventHandler(this.tsiSettings_Click);
            // 
            // tbcPanels
            // 
            this.tbcPanels.Controls.Add(this.tabBadgeIdle);
            this.tbcPanels.Controls.Add(this.tabManualIdle);
            this.tbcPanels.Location = new System.Drawing.Point(12, 58);
            this.tbcPanels.Name = "tbcPanels";
            this.tbcPanels.SelectedIndex = 0;
            this.tbcPanels.Size = new System.Drawing.Size(451, 334);
            this.tbcPanels.TabIndex = 3;
            this.tbcPanels.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tbcPanels_Selecting);
            // 
            // tabBadgeIdle
            // 
            this.tabBadgeIdle.Controls.Add(this.lblGamesToIdle);
            this.tabBadgeIdle.Controls.Add(this.lblGames);
            this.tabBadgeIdle.Controls.Add(this.lblOriginalRemainingCards);
            this.tabBadgeIdle.Controls.Add(this.lblDroppedCards);
            this.tabBadgeIdle.Controls.Add(this.prgDroppedCards);
            this.tabBadgeIdle.Controls.Add(this.ptbAvatar);
            this.tabBadgeIdle.Controls.Add(this.btnSkip);
            this.tabBadgeIdle.Controls.Add(this.btnStart);
            this.tabBadgeIdle.Controls.Add(this.btnPauseResume);
            this.tabBadgeIdle.Controls.Add(this.btnStop);
            this.tabBadgeIdle.Controls.Add(this.lsvGames);
            this.tabBadgeIdle.Controls.Add(this.btnRefresh);
            this.tabBadgeIdle.Controls.Add(this.lblUsername);
            this.tabBadgeIdle.Location = new System.Drawing.Point(4, 22);
            this.tabBadgeIdle.Name = "tabBadgeIdle";
            this.tabBadgeIdle.Padding = new System.Windows.Forms.Padding(3);
            this.tabBadgeIdle.Size = new System.Drawing.Size(443, 308);
            this.tabBadgeIdle.TabIndex = 0;
            this.tabBadgeIdle.Text = "Bagde Idle";
            this.tabBadgeIdle.UseVisualStyleBackColor = true;
            // 
            // lblOriginalRemainingCards
            // 
            this.lblOriginalRemainingCards.AutoSize = true;
            this.lblOriginalRemainingCards.Location = new System.Drawing.Point(404, 284);
            this.lblOriginalRemainingCards.Name = "lblOriginalRemainingCards";
            this.lblOriginalRemainingCards.Size = new System.Drawing.Size(33, 13);
            this.lblOriginalRemainingCards.TabIndex = 8;
            this.lblOriginalRemainingCards.Text = "/ 999";
            this.lblOriginalRemainingCards.TextChanged += new System.EventHandler(this.lblOriginalRemainingCards_TextChanged);
            // 
            // lblDroppedCards
            // 
            this.lblDroppedCards.Location = new System.Drawing.Point(383, 284);
            this.lblDroppedCards.Name = "lblDroppedCards";
            this.lblDroppedCards.Size = new System.Drawing.Size(25, 13);
            this.lblDroppedCards.TabIndex = 7;
            this.lblDroppedCards.Text = "999";
            this.lblDroppedCards.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // prgDroppedCards
            // 
            this.prgDroppedCards.Location = new System.Drawing.Point(6, 279);
            this.prgDroppedCards.Name = "prgDroppedCards";
            this.prgDroppedCards.Size = new System.Drawing.Size(371, 23);
            this.prgDroppedCards.Step = 1;
            this.prgDroppedCards.TabIndex = 5;
            // 
            // ptbAvatar
            // 
            this.ptbAvatar.Location = new System.Drawing.Point(6, 6);
            this.ptbAvatar.Name = "ptbAvatar";
            this.ptbAvatar.Size = new System.Drawing.Size(64, 64);
            this.ptbAvatar.TabIndex = 5;
            this.ptbAvatar.TabStop = false;
            // 
            // btnSkip
            // 
            this.btnSkip.BackgroundImage = global::IdleMaster.Properties.Resources.Skip;
            this.btnSkip.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSkip.Location = new System.Drawing.Point(366, 38);
            this.btnSkip.Name = "btnSkip";
            this.btnSkip.Size = new System.Drawing.Size(32, 32);
            this.btnSkip.TabIndex = 4;
            this.btnSkip.UseVisualStyleBackColor = true;
            this.btnSkip.Click += new System.EventHandler(this.btnSkip_Click);
            // 
            // btnStart
            // 
            this.btnStart.BackgroundImage = global::IdleMaster.Properties.Resources.Play;
            this.btnStart.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnStart.Location = new System.Drawing.Point(290, 38);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(32, 32);
            this.btnStart.TabIndex = 2;
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnPauseResume
            // 
            this.btnPauseResume.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnPauseResume.BackgroundImage")));
            this.btnPauseResume.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPauseResume.Location = new System.Drawing.Point(328, 38);
            this.btnPauseResume.Name = "btnPauseResume";
            this.btnPauseResume.Size = new System.Drawing.Size(32, 32);
            this.btnPauseResume.TabIndex = 3;
            this.btnPauseResume.UseVisualStyleBackColor = true;
            this.btnPauseResume.Click += new System.EventHandler(this.btnPauseResume_Click);
            // 
            // btnStop
            // 
            this.btnStop.BackgroundImage = global::IdleMaster.Properties.Resources.Stop;
            this.btnStop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnStop.Location = new System.Drawing.Point(404, 38);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(33, 32);
            this.btnStop.TabIndex = 5;
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackgroundImage = global::IdleMaster.Properties.Resources.Refresh;
            this.btnRefresh.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnRefresh.Location = new System.Drawing.Point(76, 38);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(32, 32);
            this.btnRefresh.TabIndex = 1;
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // tabManualIdle
            // 
            this.tabManualIdle.Controls.Add(this.btnManualStop);
            this.tabManualIdle.Controls.Add(this.btnManualStart);
            this.tabManualIdle.Controls.Add(this.lblAppId);
            this.tabManualIdle.Controls.Add(this.txtAppId);
            this.tabManualIdle.Controls.Add(this.lsbManualIdle);
            this.tabManualIdle.Location = new System.Drawing.Point(4, 22);
            this.tabManualIdle.Name = "tabManualIdle";
            this.tabManualIdle.Padding = new System.Windows.Forms.Padding(3);
            this.tabManualIdle.Size = new System.Drawing.Size(443, 387);
            this.tabManualIdle.TabIndex = 1;
            this.tabManualIdle.Text = "Manual Idle";
            this.tabManualIdle.UseVisualStyleBackColor = true;
            // 
            // btnManualStop
            // 
            this.btnManualStop.BackgroundImage = global::IdleMaster.Properties.Resources.Stop;
            this.btnManualStop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnManualStop.Enabled = false;
            this.btnManualStop.Location = new System.Drawing.Point(405, 241);
            this.btnManualStop.Name = "btnManualStop";
            this.btnManualStop.Size = new System.Drawing.Size(32, 32);
            this.btnManualStop.TabIndex = 9;
            this.btnManualStop.UseVisualStyleBackColor = true;
            this.btnManualStop.Click += new System.EventHandler(this.btnManualStop_Click);
            // 
            // btnManualStart
            // 
            this.btnManualStart.BackgroundImage = global::IdleMaster.Properties.Resources.Play;
            this.btnManualStart.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnManualStart.Location = new System.Drawing.Point(367, 241);
            this.btnManualStart.Name = "btnManualStart";
            this.btnManualStart.Size = new System.Drawing.Size(32, 32);
            this.btnManualStart.TabIndex = 8;
            this.btnManualStart.UseVisualStyleBackColor = true;
            this.btnManualStart.Click += new System.EventHandler(this.btnManualStart_Click);
            // 
            // lblAppId
            // 
            this.lblAppId.AutoSize = true;
            this.lblAppId.Location = new System.Drawing.Point(215, 251);
            this.lblAppId.Name = "lblAppId";
            this.lblAppId.Size = new System.Drawing.Size(40, 13);
            this.lblAppId.TabIndex = 7;
            this.lblAppId.Text = "App ID";
            // 
            // txtAppId
            // 
            this.txtAppId.Location = new System.Drawing.Point(261, 248);
            this.txtAppId.Name = "txtAppId";
            this.txtAppId.Size = new System.Drawing.Size(100, 20);
            this.txtAppId.TabIndex = 6;
            // 
            // lsbManualIdle
            // 
            this.lsbManualIdle.FormattingEnabled = true;
            this.lsbManualIdle.Location = new System.Drawing.Point(6, 6);
            this.lsbManualIdle.Name = "lsbManualIdle";
            this.lsbManualIdle.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lsbManualIdle.Size = new System.Drawing.Size(431, 225);
            this.lsbManualIdle.TabIndex = 5;
            this.lsbManualIdle.SelectedIndexChanged += new System.EventHandler(this.lsbManualIdle_SelectedIndexChanged);
            // 
            // lblSteam
            // 
            this.lblSteam.AutoSize = true;
            this.lblSteam.Location = new System.Drawing.Point(9, 33);
            this.lblSteam.Name = "lblSteam";
            this.lblSteam.Size = new System.Drawing.Size(71, 13);
            this.lblSteam.TabIndex = 4;
            this.lblSteam.Text = "Steam status:";
            // 
            // lblGames
            // 
            this.lblGames.AutoSize = true;
            this.lblGames.Location = new System.Drawing.Point(114, 57);
            this.lblGames.Name = "lblGames";
            this.lblGames.Size = new System.Drawing.Size(74, 13);
            this.lblGames.TabIndex = 5;
            this.lblGames.Text = "Games to idle:";
            // 
            // lblGamesToIdle
            // 
            this.lblGamesToIdle.AutoSize = true;
            this.lblGamesToIdle.Location = new System.Drawing.Point(184, 57);
            this.lblGamesToIdle.Name = "lblGamesToIdle";
            this.lblGamesToIdle.Size = new System.Drawing.Size(25, 13);
            this.lblGamesToIdle.TabIndex = 5;
            this.lblGamesToIdle.Text = "999";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(475, 404);
            this.Controls.Add(this.tbcPanels);
            this.Controls.Add(this.lblSteamStatus);
            this.Controls.Add(this.lnkSession);
            this.Controls.Add(this.mnsMainMenu);
            this.Controls.Add(this.lblSteam);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Idle Master";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.mnsMainMenu.ResumeLayout(false);
            this.mnsMainMenu.PerformLayout();
            this.tbcPanels.ResumeLayout(false);
            this.tabBadgeIdle.ResumeLayout(false);
            this.tabBadgeIdle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ptbAvatar)).EndInit();
            this.tabManualIdle.ResumeLayout(false);
            this.tabManualIdle.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer tmrSteamStatus;
        private System.Windows.Forms.LinkLabel lnkSession;
        private System.Windows.Forms.Button btnStart;
		private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Label lblSteamStatus;
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
        private System.Windows.Forms.TabControl tbcPanels;
        private System.Windows.Forms.TabPage tabBadgeIdle;
        private System.Windows.Forms.TabPage tabManualIdle;
        private System.Windows.Forms.Label lblSteam;
        private System.Windows.Forms.Button btnManualStop;
        private System.Windows.Forms.Button btnManualStart;
        private System.Windows.Forms.Label lblAppId;
        private System.Windows.Forms.TextBox txtAppId;
        private System.Windows.Forms.ListBox lsbManualIdle;
        private System.Windows.Forms.ProgressBar prgDroppedCards;
        private System.Windows.Forms.Label lblOriginalRemainingCards;
        private System.Windows.Forms.Label lblDroppedCards;
        private System.Windows.Forms.Label lblGames;
        private System.Windows.Forms.Label lblGamesToIdle;
    }
}