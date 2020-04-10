using HtmlAgilityPack;
using IdleMaster.Properties;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace IdleMaster
{
    public partial class frmMain : Form
    {
        private bool _isSteamReady;
        private bool _isCookieReady;

        private List<Badge> _badges;
        private Badge _currentBadge;

        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            _badges = new List<Badge>();

            CopyResources();
            KillPendingProcesses();

            tmrSteamStatus.Start();
            tmrCookieStatus.Start();
            tmrLoadBadges.Start();
        }

        ////////////////////////////////////////TIMERS////////////////////////////////////////

        private void tmrSteamStatus_Tick(object sender, EventArgs e)
        {
            _isSteamReady = SteamAPI.IsSteamRunning();
            lblSteam.Text = $"Steam is {(_isSteamReady ? "on" : "off")}";
        }

        private void tmrCookieStatus_Tick(object sender, EventArgs e)
        {
            _isCookieReady = !string.IsNullOrWhiteSpace(Settings.Default.CookieSessionId) &&
                             !string.IsNullOrWhiteSpace(Settings.Default.CookieLoginSecure);

            lblLogin.Text = $"Login is {(_isCookieReady ? "on" : "off")}";
        }

        private async void tmrLoadBadges_Tick(object sender, EventArgs e)
        {
            if (!_isSteamReady || !_isCookieReady)
            {
                return;
            }

            tmrSteamStatus.Stop();
            tmrCookieStatus.Stop();
            tmrLoadBadges.Stop();

            await LoadBadgesAsync();
        }

        ////////////////////////////////////////BADGES////////////////////////////////////////

        private async Task LoadBadgesAsync()
        {
            string profileLink = $"{Settings.Default.myProfileURL}/badges";
            int totalPages = 1;
            int currentPage = 0;

            do
            {
                currentPage++;

                string response = await CookieClient.GetHttpAsync($"{profileLink}?p={currentPage}");

                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(response);

                if (!ProcessBadgesOnPage(document))
                {
                    break;
                }

                if (currentPage != 1)
                {
                    continue;
                }

                HtmlNodeCollection pages = document.DocumentNode.SelectNodes("//a[@class=\"pagelink\"]");

                string href = pages?.Last().Attributes["href"]?.Value;
                string maxPages = href?.Split('=').Last();
                int.TryParse(maxPages, out totalPages);
            }
            while (currentPage < totalPages);
        }

        private bool ProcessBadgesOnPage(HtmlDocument document)
        {
            HtmlNodeCollection droppableBadges = document.DocumentNode.SelectNodes("//span[text()='PLAY']/ancestor::div[contains(@class,'is_link')]");

            if (droppableBadges == null)
            {
                return false;
            }

            foreach (HtmlNode badge in droppableBadges)
            {
                HtmlNode urlNode = badge.SelectSingleNode(".//a[@class='badge_row_overlay']");
                string appId = urlNode?.Attributes["href"]?.Value.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries).Last();

                HtmlNode nameNode = badge.SelectSingleNode(".//div[@class='badge_title']");
                string name = WebUtility.HtmlDecode(nameNode?.FirstChild.InnerText).Trim();

                HtmlNode cardsNode = badge.SelectSingleNode(".//span[@class='progress_info_bold']");
                string cards = cardsNode?.InnerText.Split(' ').First();

                HtmlNode playtimeNode = badge.SelectSingleNode(".//div[@class='badge_title_stats_playtime']");
                string playtime = WebUtility.HtmlDecode(playtimeNode?.InnerText).Trim().Split(' ').First();

                Badge existingBadge = _badges.FirstOrDefault(x => x.AppId == appId);

                if (existingBadge != null)
                {
                    existingBadge.UpdateStats(cards, playtime);
                    continue;
                }

                Badge newBadge = new Badge(appId, name, cards, playtime);
                _badges.Add(newBadge);
            }

            HtmlNodeCollection allBadges = document.DocumentNode.SelectNodes("//div[contains(@class,'is_link')]");

            if (allBadges.Count > droppableBadges.Count)
            {
                return false;
            }

            return true;
        }

        ////////////////////////////////////////IDLE////////////////////////////////////////

        private void StartIdle()
        {
            if (!_badges.Any())
            {
                return;
            }

            if (_currentBadge == null)
            {
                _currentBadge = _badges.First();
                _currentBadge.Idle();

                tmrIdle.Enabled = true;
                return;
            }

            CheckIdleStatus();

        }

        private void PauseIdle()
        {
            tmrIdle.Stop();
        }

        private void StopIdle()
        {
            _currentBadge.StopIdle();
            _currentBadge = null;
        }

        private async void CheckIdleStatus()
        {
            tmrIdle.Enabled = false;

            bool canDropCards = await _currentBadge.CanDropCards();

            if (!canDropCards)
            {
                _currentBadge.StopIdle();
                _badges.Remove(_currentBadge);

                _currentBadge = _badges.First();
                _currentBadge.Idle();
            }

            tmrIdle.Enabled = true;
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
                    string[] argList = new string[] { string.Empty, string.Empty };

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

        ////////////////////////////////////////CONTROLS////////////////////////////////////////

        private void lnkLogin_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmBrowser form = new frmBrowser();
            form.ShowDialog();
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

        private void tmrIdle_Tick(object sender, EventArgs e)
        {
            CheckIdleStatus();
        }
    }
}