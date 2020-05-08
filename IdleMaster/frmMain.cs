using IdleMaster.ControlEntities;
using IdleMaster.Entities;
using IdleMaster.Forms;
using IdleMaster.Properties;
using Steamworks;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace IdleMaster
{
    public partial class frmMain : Form
    {
        private bool _isSteamRunning;

        private Profile _profile;

        private bool IsLoggedIn
        {
            get
            {
                return !string.IsNullOrWhiteSpace(UserSettings.CookieSessionId) && !string.IsNullOrWhiteSpace(UserSettings.CookieLoginSecure);
            }
        }

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            CopyResources();

            CheckSteamStatus();
            tmrSteamStatus.Start();

            LoadProfile();
        }

        ////////////////////////////////////////METHODS////////////////////////////////////////

        private void CopyResources()
        {
            if (File.Exists("steam_api64.dll") == false)
            {
                CopyResource("IdleMaster.Resources.steam_api64.dll", $@"{Environment.CurrentDirectory}\steam_api64.dll");
            }

            if (File.Exists("Steamworks.NET.dll") == false)
            {
                CopyResource("IdleMaster.Resources.Steamworks.NET.dll", $@"{Environment.CurrentDirectory}\Steamworks.NET.dll");
            }

            if (File.Exists("steam-idle.exe") == false)
            {
                CopyResource("IdleMaster.Resources.steam-idle.exe", $@"{Environment.CurrentDirectory}\steam-idle.exe");
            }
        }

        private void CopyResource(string resourceName, string file)
        {
            using (Stream resource = GetType().Assembly.GetManifestResourceStream(resourceName))
            {
                if (resource == null)
                {
                    return;
                }

                using (Stream output = File.OpenWrite(file))
                {
                    resource.CopyTo(output);
                }
            }
        }

        private void UpdateUserInterface(string uiProfile)
        {
            if (uiProfile == "logout")
            {
                lnkSession.Text = "Login";

                ptbAvatar.Image = null;
                lblUsername.Text = null;

                btnRefresh.Enabled = false;
                lsvGames.Enabled = false;
                lsvGames.Items.Clear();

                btnStart.Enabled = false;
                btnPauseResume.Enabled = false;
                btnSkip.Enabled = false;
                btnStop.Enabled = false;

                btnPauseResume.BackgroundImage = Resources.Pause;
            }

            if (!IsLoggedIn)
            {
                return;
            }

            if (uiProfile == "login")
            {
                lnkSession.Text = "Logout";

                ptbAvatar.ImageLocation = _profile.Avatar;
                lblUsername.Text = _profile.Username;

                btnRefresh.Enabled = true;
                lsvGames.Enabled = _profile.Library.HasGames;

                btnStart.Enabled = _profile.Library.HasGames && _isSteamRunning;
                btnPauseResume.Enabled = false;
                btnSkip.Enabled = false;
                btnStop.Enabled = false;

                btnPauseResume.BackgroundImage = Resources.Pause;
            }

            if (uiProfile == "list")
            {
                lsvGames.Enabled = _profile.Library.HasGames;
                lsvGames.Items.Clear();

                if (_profile.Library.HasGames)
                {
                    foreach (Game game in _profile.Library.Games)
                    {
                        ListViewItem item = new ListViewItem
                        {
                            Text = game.Name
                        };

                        item.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = $"{game.HoursPlayed}h" });
                        item.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = game.RemainingCards.ToString() });

                        string idleStatus = null;

                        if (_profile.Library.IsIdling)
                        {
                            if (_profile.Library.IsPaused)
                            {
                                int index = _profile.Library.Games.IndexOf(game);
                                int firstIndex = _profile.Library.FirstIdlingIndex;
                                int lastIndex = _profile.Library.FirstIdlingIndex + UserSettings.GamesToIdle;

                                if (firstIndex <= index && index < lastIndex)
                                {
                                    idleStatus = "Paused";
                                }
                            }

                            if (game.IsIdling)
                            {
                                idleStatus = "Idling";
                            }
                        }

                        item.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = idleStatus });

                        lsvGames.Items.Add(item);
                    }
                }
            }

            if (uiProfile == "start")
            {
                ptbAvatar.ImageLocation = _profile.Avatar;
                lblUsername.Text = _profile.Username;

                btnRefresh.Enabled = false;
                lsvGames.Enabled = _profile.Library.HasGames;

                btnStart.Enabled = false;
                btnPauseResume.Enabled = true;
                btnSkip.Enabled = _profile.Library.FirstIdlingIndex + UserSettings.GamesToIdle < _profile.Library.Games.Count;
                btnStop.Enabled = true;

                btnPauseResume.BackgroundImage = Resources.Pause;
            }

            if (uiProfile == "pause")
            {
                ptbAvatar.ImageLocation = _profile.Avatar;
                lblUsername.Text = _profile.Username;

                btnRefresh.Enabled = false;
                lsvGames.Enabled = _profile.Library.HasGames;

                btnStart.Enabled = false;
                btnPauseResume.Enabled = true;
                btnSkip.Enabled = _profile.Library.FirstIdlingIndex + UserSettings.GamesToIdle < _profile.Library.Games.Count;
                btnStop.Enabled = true;

                btnPauseResume.BackgroundImage = Resources.Resume;
            }

            if (uiProfile == "resume")
            {
                ptbAvatar.ImageLocation = _profile.Avatar;
                lblUsername.Text = _profile.Username;

                btnRefresh.Enabled = false;
                lsvGames.Enabled = _profile.Library.HasGames;

                btnStart.Enabled = false;
                btnPauseResume.Enabled = true;
                btnSkip.Enabled = _profile.Library.FirstIdlingIndex + UserSettings.GamesToIdle < _profile.Library.Games.Count;
                btnStop.Enabled = true;

                btnPauseResume.BackgroundImage = Resources.Pause;
            }

            if (uiProfile == "stop")
            {
                ptbAvatar.ImageLocation = _profile.Avatar;
                lblUsername.Text = _profile.Username;

                btnRefresh.Enabled = true;
                lsvGames.Enabled = _profile.Library.HasGames;

                btnStart.Enabled = _profile.Library.HasGames && _isSteamRunning;
                btnPauseResume.Enabled = false;
                btnSkip.Enabled = false;
                btnStop.Enabled = false;

                btnPauseResume.BackgroundImage = Resources.Pause;
            }
        }

        ////////////////////////////////////////STATUS////////////////////////////////////////

        private void tmrSteamStatus_Tick(object sender, EventArgs e)
        {
            CheckSteamStatus();
        }

        private void CheckSteamStatus()
        {
            _isSteamRunning = SteamAPI.IsSteamRunning();

            lblSteam.Text = $"Steam is {(!_isSteamRunning ? "not " : "")}running";
            lblSteam.ForeColor = _isSteamRunning ? Color.Green : Color.Red;
        }

        ////////////////////////////////////////SESSION////////////////////////////////////////

        private void Login()
        {
            frmBrowser form = new frmBrowser();
            form.ShowDialog();

            LoadProfile();
        }

        private void Logout()
        {
            StopIdle();
            _profile = null;

            UserSettings.ClearCookies();

            UpdateUserInterface("logout");
        }

        ////////////////////////////////////////PROFILE////////////////////////////////////////

        private void LoadProfile()
        {
            if (!IsLoggedIn)
            {
                UpdateUserInterface("logout");
                return;
            }

            _profile = new Profile();
            _profile.LoadProfile();

            UpdateUserInterface("login");
            UpdateUserInterface("list");
        }

        private void RefreshList()
        {
            if (!IsLoggedIn)
            {
                UpdateUserInterface("logout");
                return;
            }

            _profile.LoadGames();
            UpdateUserInterface("list");
        }

        ////////////////////////////////////////IDLE////////////////////////////////////////

        private void StartIdle()
        {
            if (!_isSteamRunning)
            {
                return;
            }

            _profile.Library.StartIdling();

            UpdateUserInterface("start");
            UpdateUserInterface("list");

            tmrNormalIdleStatus.Start();
            //tmrFastIdleStop.Start();
        }

        private void PauseIdle()
        {
            if (!_isSteamRunning)
            {
                return;
            }

            _profile.Library.PauseIdling();

            UpdateUserInterface("pause");
            UpdateUserInterface("list");
        }

        private void ResumeIdle()
        {
            if (!_isSteamRunning)
            {
                return;
            }

            _profile.Library.ResumeIdling();

            UpdateUserInterface("resume");
            UpdateUserInterface("list");
        }

        private void SkipIdle()
        {
            if (!_isSteamRunning)
            {
                return;
            }

            _profile.Library.SkipIdling();

            UpdateUserInterface("resume");
            UpdateUserInterface("list");
        }

        private void StopIdle()
        {
            if (!_isSteamRunning)
            {
                return;
            }

            _profile.Library.StopIdling();

            tmrNormalIdleStatus.Stop();
            tmrFastIdleStart.Stop();
            tmrFastIdleStop.Stop();

            UpdateUserInterface("stop");
            UpdateUserInterface("list");
        }

        private void CheckNormalIdleStatus()
        {
            tmrNormalIdleStatus.Stop();

            if (!_isSteamRunning)
            {
                return;
            }

            _profile.Library.CheckNormalIdlingStatus();
            UpdateUserInterface("list");

            tmrNormalIdleStatus.Start();
        }

        private void FastIdleStart()
        {
            tmrFastIdleStart.Stop();

            if (!_isSteamRunning)
            {
                return;
            }

            _profile.Library.StartFastIdling();
            _profile.Library.CheckFastIdlingStatus();
            UpdateUserInterface("list");

            tmrFastIdleStop.Start();
        }

        private void FastIdleStop()
        {
            tmrFastIdleStop.Stop();

            if (!_isSteamRunning)
            {
                return;
            }

            _profile.Library.StopFastIdling();
            UpdateUserInterface("list");

            tmrFastIdleStart.Start();
        }

        ////////////////////////////////////////TIMERS////////////////////////////////////////

        private void tmrNormalIdleStatus_Tick(object sender, EventArgs e)
        {
            CheckNormalIdleStatus();
        }

        private void tmrFastIdleStart_Tick(object sender, EventArgs e)
        {
            FastIdleStart();
        }

        private void tmrFastIdleStop_Tick(object sender, EventArgs e)
        {
            FastIdleStop();
        }

        ////////////////////////////////////////CONTROLS////////////////////////////////////////

        private void lnkSession_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (!IsLoggedIn)
            {
                Login();
            }
            else
            {
                Logout();
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshList();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            StartIdle();
        }

        private void btnPauseResume_Click(object sender, EventArgs e)
        {
            if (!_profile.Library.IsPaused)
            {
                PauseIdle();
            }
            else
            {
                ResumeIdle();
            }
        }

        private void btnSkip_Click(object sender, EventArgs e)
        {
            SkipIdle();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            StopIdle();
        }

        ////////////////////////////////////////DESIGN////////////////////////////////////////

        private void lsvGames_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            e.NewWidth = lsvGames.Columns[e.ColumnIndex].Width;
            e.Cancel = true;
        }

        private void lsvGames_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Add ||
                e.KeyCode == Keys.Subtract)
            {
                e.SuppressKeyPress = true;
            }
        }

        private void lsvGames_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
            {
                e.Item.Selected = false;
            }
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmSettings form = new frmSettings();
            form.ShowDialog();
        }
    }
}