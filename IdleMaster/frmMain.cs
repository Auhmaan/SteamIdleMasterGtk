using IdleMaster.Entities;
using IdleMaster.Properties;
using Steamworks;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Management;
using System.Security.Principal;
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
                return !string.IsNullOrWhiteSpace(Settings.Default.CookieSessionId) && !string.IsNullOrWhiteSpace(Settings.Default.CookieLoginSecure);
            }
        }

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            CopyResources();
            KillPendingProcesses();

            CheckSteamStatus();
            tmrSteamStatus.Start();

            LoadProfile();
        }

        ////////////////////////////////////////METHODS////////////////////////////////////////

        private void CopyResources()
        {
            if (File.Exists("steam_api.dll") == false)
            {
                CopyResource("IdleMaster.Resources.steam_api64.dll", $@"{Environment.CurrentDirectory}\steam_api.dll");
            }

            if (File.Exists("CSteamworks.dll") == false)
            {
                CopyResource("IdleMaster.Resources.Steamworks.NET.dll", $@"{Environment.CurrentDirectory}\CSteamworks.dll");
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

        private void KillPendingProcesses()
        {
            string windowsUser = WindowsIdentity.GetCurrent().Name;

            foreach (Process process in Process.GetProcessesByName("steam-idle"))
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher($"SELECT * FROM Win32_Process WHERE PROCESSID = {process.Id}");
                ManagementObjectCollection processList = searcher.Get();

                foreach (ManagementObject managementObject in processList)
                {
                    string[] argList = new string[] { null, null };

                    int.TryParse(managementObject.InvokeMethod("GetOwner", argList).ToString(), out int returnValue);

                    if (returnValue != 0)
                    {
                        continue;
                    }

                    if ($"{argList[1]}\\{argList[0]}" != windowsUser)
                    {
                        continue;
                    }

                    process.Kill();
                }
            }
        }

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

        private void UpdateUserInterface(string uiProfile = "logout")
        {
            if (uiProfile == "logout")
            {
                lnkSession.Text = "Login";

                ptbAvatar.Image = null;
                lblUsername.Text = null;

                btnRefresh.Enabled = false;
                lsvBadges.Enabled = false;
                lsvBadges.Items.Clear();

                btnStart.Enabled = false;
                btnPause.Enabled = false;
                btnStop.Enabled = false;
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

                btnStart.Enabled = _profile.HasBadges && _isSteamRunning;
                btnPause.Enabled = false;
                btnStop.Enabled = false;
            }

            if (uiProfile == "list")
            {
                lsvBadges.Enabled = _profile.HasBadges;
                lsvBadges.Items.Clear();

                foreach (Badge badge in _profile.Badges)
                {
                    ListViewItem item = new ListViewItem
                    {
                        Text = badge.Name
                    };

                    item.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = $"{badge.HoursPlayed}h" });
                    item.SubItems.Add(new ListViewItem.ListViewSubItem() { Text = badge.RemainingCards.ToString() });

                    lsvBadges.Items.Add(item);
                }
            }

            if (uiProfile == "start")
            {
                ptbAvatar.ImageLocation = _profile.Avatar;
                lblUsername.Text = _profile.Username;

                btnRefresh.Enabled = false;
                lsvBadges.Enabled = true;

                btnStart.Enabled = false;
                btnPause.Enabled = true;
                btnStop.Enabled = true;

                btnPause.BackgroundImage = Resources.Pause;
            }

            if (uiProfile == "pause")
            {
                ptbAvatar.ImageLocation = _profile.Avatar;
                lblUsername.Text = _profile.Username;

                btnRefresh.Enabled = false;
                lsvBadges.Enabled = true;

                btnStart.Enabled = false;
                btnPause.Enabled = true;
                btnStop.Enabled = true;

                btnPause.BackgroundImage = Resources.Resume;
            }

            if (uiProfile == "stop")
            {
                ptbAvatar.ImageLocation = _profile.Avatar;
                lblUsername.Text = _profile.Username;

                btnRefresh.Enabled = true;
                lsvBadges.Enabled = true;

                btnStart.Enabled = _profile.HasBadges && _isSteamRunning;
                btnPause.Enabled = false;
                btnStop.Enabled = false;

                btnPause.BackgroundImage = Resources.Pause;
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
            KillPendingProcesses();

            Settings.Default.CookieSessionId = string.Empty;
            Settings.Default.CookieLoginSecure = string.Empty;
            Settings.Default.CookieParental = string.Empty;
            Settings.Default.Save();

            UpdateUserInterface("logout");
        }

        ////////////////////////////////////////PROFILE////////////////////////////////////////

        private void RefreshList()
        {
            _profile.LoadBadges();
            UpdateUserInterface("list");
        }

        ////////////////////////////////////////IDLE////////////////////////////////////////

        private void tmrIdleStatus_Tick(object sender, EventArgs e)
        {
            CheckIdleStatus();
        }

        private void StartIdle()
        {
            if (!_isSteamRunning)
            {
                return;
            }

            _profile.StartIdlingBadges();
            CheckIdleStatus();

            UpdateUserInterface("start");
        }

        private void PauseIdle()
        {
            if (!_isSteamRunning)
            {
                return;
            }

            if (_profile.CurrentBadge.IsIdling)
            {
                _profile.PauseIdlingBadges();
                tmrIdleStatus.Stop();

                UpdateUserInterface("pause");
                UpdateUserInterface("list");
            }
            else
            {
                _profile.ResumeIdlingBadges();
                CheckIdleStatus();

                UpdateUserInterface("start");
            }
        }

        private void StopIdle()
        {
            if (!_isSteamRunning)
            {
                return;
            }

            _profile.StopIdlingBadges();
            tmrIdleStatus.Stop();

            UpdateUserInterface("stop");
            UpdateUserInterface("list");
        }

        private void CheckIdleStatus()
        {
            tmrIdleStatus.Stop();

            if (!_isSteamRunning)
            {
                return;
            }

            _profile.CheckIdlingStatus(true);
            UpdateUserInterface("list");
            tmrIdleStatus.Start();
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

        private void btnPause_Click(object sender, EventArgs e)
        {
            PauseIdle();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            StopIdle();
        }

        ////////////////////////////////////////DESIGN////////////////////////////////////////

        private void lsvBadges_ColumnWidthChanging(object sender, ColumnWidthChangingEventArgs e)
        {
            e.NewWidth = lsvBadges.Columns[e.ColumnIndex].Width;
            e.Cancel = true;
        }

        private void lsvBadges_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Add ||
                e.KeyCode == Keys.Subtract)
            {
                e.SuppressKeyPress = true;
            }
        }
    }
}