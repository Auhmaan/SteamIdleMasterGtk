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
        private bool? _isSteamRunning = null;

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

            UpdateUserInterface("reset");

            CheckSteamStatus();
            tmrSteamStatus.Start();

            LoadProfile();
        }

        ////////////////////////////////////////METHODS////////////////////////////////////////

        private void CopyResources()
        {
            if (File.Exists("steam_api.dll") == false)
            {
                CopyResource("IdleMaster.Resources.steam_api.dll", $@"{Environment.CurrentDirectory}\steam_api.dll");
            }

            if (File.Exists("CSteamworks.dll") == false)
            {
                CopyResource("IdleMaster.Resources.CSteamworks.dll", $@"{Environment.CurrentDirectory}\CSteamworks.dll");
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
                UpdateUserInterface("login");
                return;
            }

            _profile = new Profile();
            _profile.LoadProfile();

            UpdateUserInterface("login");
            UpdateUserInterface("profile");
            UpdateUserInterface("list");
            UpdateUserInterface("idle");
        }

        private void UpdateUserInterface(string pattern = "all")
        {
            if (pattern == "reset")
            {
                lnkLogin.Enabled = false;
                lnkLogout.Enabled = false;

                lblSteam.Text = null;

                ptbAvatar.Image = null;
                lblUsername.Text = null;

                btnRefresh.Enabled = false;
                lsvBadges.Enabled = false;
                lsvBadges.Items.Clear();

                btnStart.Enabled = false;
                btnPause.Enabled = false;
                btnResume.Enabled = false;
                btnStop.Enabled = false;
            }

            if (pattern == "all" ||
               pattern == "login")
            {
                lnkLogin.Enabled = !IsLoggedIn;
                lnkLogout.Enabled = IsLoggedIn;
            }

            if (pattern == "all" ||
                pattern == "steam")
            {
                lblSteam.Text = $"Steam is {(!_isSteamRunning.Value ? "not " : "")}running";
                lblSteam.ForeColor = _isSteamRunning.Value ? Color.Green : Color.Red;
            }

            if (IsLoggedIn)
            {
                if (pattern == "all" ||
                   pattern == "profile")
                {
                    ptbAvatar.ImageLocation = _profile.Avatar;
                    lblUsername.Text = _profile.Username;
                    btnRefresh.Enabled = true;
                }

                if (pattern == "all" ||
                    pattern == "list")
                {
                    lsvBadges.Enabled = true;
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

                if (pattern == "all" ||
                    pattern == "idle")
                {
                    if (_profile.HasBadges)
                    {
                        btnStart.Enabled = !_profile.IsIdling;
                        btnPause.Enabled = _profile.IsIdling && !_profile.IsPaused;
                        btnResume.Enabled = _profile.IsIdling && _profile.IsPaused;
                        btnStop.Enabled = _profile.IsIdling;
                    }
                }
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
            UpdateUserInterface("steam");
        }

        ////////////////////////////////////////IDLE////////////////////////////////////////

        private void tmrIdleStatus_Tick(object sender, EventArgs e)
        {
            CheckIdleStatus();
        }

        private void StartIdle()
        {
            _profile.StartIdlingBadges();

            if (!_profile.IsIdling)
            {
                return;
            }

            CheckIdleStatus();
        }

        private void PauseIdle()
        {
            _profile.PauseIdlingBadges();
            tmrIdleStatus.Stop();
        }

        private void ResumeIdle()
        {
            _profile.ResumeIdlingBadges();
            CheckIdleStatus();
        }

        private void StopIdle()
        {
            _profile.StopIdlingBadges();
        }

        private void CheckIdleStatus()
        {
            tmrIdleStatus.Stop();
            _profile.CheckIdlingStatus(true);
            tmrIdleStatus.Start();
        }

        ////////////////////////////////////////CONTROLS////////////////////////////////////////

        private void lnkLogin_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmBrowser form = new frmBrowser();
            form.ShowDialog();

            LoadProfile();
        }

        private void lnkLogout_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            StopIdle();
            _profile = null;
            KillPendingProcesses();

            Settings.Default.CookieSessionId = string.Empty;
            Settings.Default.CookieLoginSecure = string.Empty;
            Settings.Default.CookieParental = string.Empty;
            Settings.Default.Save();

            UpdateUserInterface("reset");
            UpdateUserInterface("login");
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            _profile.LoadBadges();
            UpdateUserInterface("list");
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            StartIdle();
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            PauseIdle();
        }

        private void btnResume_Click(object sender, EventArgs e)
        {
            ResumeIdle();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            StopIdle();
        }
    }
}