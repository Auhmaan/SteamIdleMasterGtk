using IdleMaster.ControlEntities;
using IdleMaster.Entities;
using IdleMaster.Enums;
using IdleMaster.Forms;
using IdleMaster.Properties;
using IdleMaster.UserControls;
using Steamworks;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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

        private async void frmMain_Load(object sender, EventArgs e)
        {
            CopyResources();

            CheckSteamStatus();
            tmrSteamStatus.Start();

            await LoadProfile();
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

                ptbAvatar.ImageLocation = null;
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
                            Tag = game.AppId,
                            Text = game.Name
                        };

                        item.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = $"{game.HoursPlayed}h" });
                        item.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = game.RemainingCards.ToString() });
                        item.SubItems.Add(new ListViewItem.ListViewSubItem());

                        lsvGames.Items.Add(item);
                    }
                }
            }

            if (uiProfile == "status")
            {
                foreach (ListViewItem item in lsvGames.Items)
                {
                    Game game = _profile.Library.Games.FirstOrDefault(x => item.Tag.ToString() == x.AppId);

                    if (game == null)
                    {
                        continue;
                    }

                    string idleStatus = null;

                    if (!_profile.Library.IsPaused)
                    {
                        switch (game.Status)
                        {
                            case GameStatus.FastIdling:
                                idleStatus = "Fast Idling";
                                break;
                            case GameStatus.Restarting:
                                idleStatus = "Restarting";
                                break;
                            case GameStatus.NormalIdling:
                                idleStatus = "Idling";
                                break;
                            case GameStatus.Finished:
                                idleStatus = "Finished";
                                break;
                        }
                    }
                    else
                    {
                        int index = _profile.Library.Games.IndexOf(game);
                        int firstIndex = _profile.Library.FirstIdlingIndex;
                        int lastIndex = _profile.Library.FirstIdlingIndex + UserSettings.GamesToIdle;

                        if (firstIndex <= index && index < lastIndex)
                        {
                            idleStatus = "Paused";
                        }
                    }

                    item.SubItems[3].Text = idleStatus;
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
                btnSkip.Enabled = false;
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

        private void ShowWaitingAnimation()
        {
            ucLoading userControl = new ucLoading
            {
                Name = "ucLoading",
                Location = new Point(12, 27)
            };

            Controls.Add(userControl);
            userControl.BringToFront();
        }

        private void HideWaitingAnimation()
        {
            Controls.RemoveByKey("ucLoading");
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

        private async Task Login()
        {
            frmBrowser form = new frmBrowser();
            form.ShowDialog();

            await LoadProfile();
        }

        private void Logout()
        {
            StopIdle();
            _profile = null;

            UserSettings.ClearCookies();

            UpdateUserInterface("logout");
        }

        ////////////////////////////////////////PROFILE////////////////////////////////////////

        private async Task LoadProfile()
        {
            ShowWaitingAnimation();

            if (!IsLoggedIn)
            {
                UpdateUserInterface("logout");
                HideWaitingAnimation();
                return;
            }

            _profile = new Profile();
            await _profile.LoadProfile();

            UpdateUserInterface("login");
            UpdateUserInterface("list");

            HideWaitingAnimation();
        }

        private async Task RefreshList()
        {
            if (!IsLoggedIn)
            {
                UpdateUserInterface("logout");
                return;
            }

            await _profile.LoadGames();
            UpdateUserInterface("list");
        }

        ////////////////////////////////////////IDLE////////////////////////////////////////

        private void StartIdle()
        {
            if (!_isSteamRunning)
            {
                return;
            }

            if (UserSettings.FastIdleEnabled)
            {
                DialogResult result = MessageBox.Show($"Fast idle is enabled.{Environment.NewLine}This option will restart your games multiple times so it is recommended you first set yourself as invisible or offline on your friends window before idling to avoid annoying others.{Environment.NewLine}Do you wish to start idling now?", "Attention", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (result == DialogResult.No)
                {
                    return;
                }
            }

            _profile.Library.StartIdling();

            UpdateUserInterface("start");
            UpdateUserInterface("status");

            tmrNormalIdleStatus.Start();
            tmrFastIdleStop.Start();
        }

        private void PauseIdle()
        {
            if (!_isSteamRunning)
            {
                return;
            }

            _profile.Library.PauseIdling();

            tmrNormalIdleStatus.Stop();
            tmrFastIdleStart.Stop();
            tmrFastIdleStop.Stop();

            UpdateUserInterface("pause");
            UpdateUserInterface("status");
        }

        private void ResumeIdle()
        {
            if (!_isSteamRunning)
            {
                return;
            }

            _profile.Library.ResumeIdling();

            UpdateUserInterface("resume");
            UpdateUserInterface("status");

            tmrNormalIdleStatus.Start();
            tmrFastIdleStop.Start();
        }

        private void SkipIdle()
        {
            if (!_isSteamRunning)
            {
                return;
            }

            _profile.Library.SkipIdling();

            UpdateUserInterface("resume");
            UpdateUserInterface("status");
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
            UpdateUserInterface("status");
        }

        private async Task CheckNormalIdleStatus()
        {
            tmrNormalIdleStatus.Stop();

            if (!_isSteamRunning)
            {
                return;
            }

            await _profile.Library.CheckIdlingStatus(GameStatus.NormalIdling);
            UpdateUserInterface("status");

            tmrNormalIdleStatus.Start();
        }

        private async Task FastIdleStart()
        {
            tmrFastIdleStart.Stop();

            if (!_isSteamRunning)
            {
                return;
            }

            _profile.Library.StartFastIdling();
            await _profile.Library.CheckIdlingStatus(GameStatus.FastIdling);
            UpdateUserInterface("status");

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
            UpdateUserInterface("status");

            tmrFastIdleStart.Start();
        }

        ////////////////////////////////////////TIMERS////////////////////////////////////////

        private async void tmrNormalIdleStatus_Tick(object sender, EventArgs e)
        {
            await CheckNormalIdleStatus();
        }

        private async void tmrFastIdleStart_Tick(object sender, EventArgs e)
        {
            await FastIdleStart();
        }

        private void tmrFastIdleStop_Tick(object sender, EventArgs e)
        {
            FastIdleStop();
        }

        ////////////////////////////////////////CONTROLS////////////////////////////////////////

        private async void lnkSession_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (!IsLoggedIn)
            {
                await Login();
            }
            else
            {
                Logout();
            }
        }

        private async void btnRefresh_Click(object sender, EventArgs e)
        {
            await RefreshList();
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

        private void tsiSettings_Click(object sender, EventArgs e)
        {
            if (IsLoggedIn && _profile.Library.IsIdling)
            {
                MessageBox.Show("You must not be idling games to access the settings.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            frmSettings form = new frmSettings();
            form.ShowDialog();
        }
    }
}