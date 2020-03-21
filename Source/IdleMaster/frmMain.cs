using IdleMaster.Properties;
using Newtonsoft.Json;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace IdleMaster
{
    public partial class frmMain : Form
    {
        private Statistics statistics = new Statistics();
        public List<Badge> AllBadges { get; set; }

        public IEnumerable<Badge> CanIdleBadges
        {
            get
            {
                return AllBadges.Where(b => b.RemainingCard != 0);
            }
        }
        public int CardsRemaining
        {
            get
            {
                return CanIdleBadges.Sum(b => b.RemainingCard);
            }
        }
        public int GamesRemaining
        {
            get
            {
                return CanIdleBadges.Count();
            }
        }

        public bool IsCookieReady;
        public bool IsSteamReady;
        public int TimeLeft = 900;
        public int RetryCount = 0;
        public int ReloadCount = 0;
        public Badge CurrentBadge;

        public frmMain()
        {
            InitializeComponent();
            AllBadges = new List<Badge>();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            //Copy external references to the output directory. This allows ClickOnce install.
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

            //Update the settings, if needed. When the application updates, settings will persist.
            if (Settings.Default.updateNeeded)
            {
                Settings.Default.Upgrade();
                Settings.Default.updateNeeded = false;
                Settings.Default.Save();
            }

            //Set the interface language from the settings
            if (!string.IsNullOrWhiteSpace(Settings.Default.language))
            {
                string language = null;

                switch (Settings.Default.language)
                {
                    case "Bulgarian":
                        language = "bg";
                        break;
                    case "Chinese (Simplified, China)":
                        language = "zh-CN";
                        break;
                    case "Chinese (Traditional, China)":
                        language = "zh-TW";
                        break;
                    case "Czech":
                        language = "cs";
                        break;
                    case "Danish":
                        language = "da";
                        break;
                    case "Dutch":
                        language = "nl";
                        break;
                    case "English":
                        language = "en";
                        break;
                    case "Finnish":
                        language = "fi";
                        break;
                    case "French":
                        language = "fr";
                        break;
                    case "German":
                        language = "de";
                        break;
                    case "Greek":
                        language = "el";
                        break;
                    case "Hungarian":
                        language = "hu";
                        break;
                    case "Italian":
                        language = "it";
                        break;
                    case "Japanese":
                        language = "ja";
                        break;
                    case "Korean":
                        language = "ko";
                        break;
                    case "Norwegian":
                        language = "no";
                        break;
                    case "Polish":
                        language = "pl";
                        break;
                    case "Portuguese":
                        language = "pt-PT";
                        break;
                    case "Portuguese (Brazil)":
                        language = "pt-BR";
                        break;
                    case "Romanian":
                        language = "ro";
                        break;
                    case "Russian":
                        language = "ru";
                        break;
                    case "Spanish":
                        language = "es";
                        break;
                    case "Swedish":
                        language = "sv";
                        break;
                    case "Thai":
                        language = "th";
                        break;
                    case "Turkish":
                        language = "tr";
                        break;
                    case "Ukrainian":
                        language = "uk";
                        break;
                    default:
                        language = "en";
                        break;
                }

                Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
            }

            //Localize form elements
            fileToolStripMenuItem.Text = localization.strings.file;
            gameToolStripMenuItem.Text = localization.strings.game;
            helpToolStripMenuItem.Text = localization.strings.help;
            settingsToolStripMenuItem.Text = localization.strings.settings;
            blacklistToolStripMenuItem.Text = localization.strings.blacklist;
            exitToolStripMenuItem.Text = localization.strings.exit;
            pauseIdlingToolStripMenuItem.Text = localization.strings.pause_idling;
            resumeIdlingToolStripMenuItem.Text = localization.strings.resume_idling;
            skipGameToolStripMenuItem.Text = localization.strings.skip_current_game;
            blacklistCurrentGameToolStripMenuItem.Text = localization.strings.blacklist_current_game;
            statisticsToolStripMenuItem.Text = localization.strings.statistics;
            changelogToolStripMenuItem.Text = localization.strings.release_notes;
            officialGroupToolStripMenuItem.Text = localization.strings.official_group;
            aboutToolStripMenuItem.Text = localization.strings.about;
            lnkSignIn.Text = "(" + localization.strings.sign_in + ")";
            lnkResetCookies.Text = "(" + localization.strings.sign_out + ")";
            toolStripStatusLabel1.Text = localization.strings.next_check;
            toolStripStatusLabel1.ToolTipText = localization.strings.next_check;

            lblSteamUsername.Text = localization.strings.signed_in_as;
            lsvGamesState.Columns[0].Text = localization.strings.name;
            lsvGamesState.Columns[1].Text = localization.strings.hours;

            //Set the form height
            Graphics graphics = CreateGraphics();
            double scale = graphics.DpiY * 1.625;
            Height = Convert.ToInt32(scale);

            //Set the location of certain elements so that they scale correctly for different DPI settings
            lblGameName.Location = new Point(Convert.ToInt32(graphics.DpiX * 1.14), Convert.ToInt32(lblGameName.Location.Y));
            lnkSignIn.Location = new Point(Convert.ToInt32(graphics.DpiX * 2.35), Convert.ToInt32(lnkSignIn.Location.Y));
            lnkResetCookies.Location = new Point(Convert.ToInt32(graphics.DpiX * 2.15), Convert.ToInt32(lnkResetCookies.Location.Y));
        }

        private void frmMain_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                if (Settings.Default.minToTray)
                {
                    notifyIcon1.Visible = true;
                    Hide();
                }
            }

            if (WindowState == FormWindowState.Normal)
            {
                notifyIcon1.Visible = false;
            }
        }

        private void frmMain_FormClose(object sender, FormClosedEventArgs e)
        {
            StopIdle();
        }

        ////////////////////////////////////////METHODS////////////////////////////////////////

        private void UpdateStateInfo()
        {
            //Update totals
            if (ReloadCount == 0)
            {
                lblIdle.Text = $"{GamesRemaining} {localization.strings.games_left_to_idle}, {CanIdleBadges.Count(b => b.InIdle)} {localization.strings.idle_now}.";
                lblDrops.Text = $"{CardsRemaining} {localization.strings.card_drops_remaining}";
                lblIdle.Visible = GamesRemaining > 0;
                lblDrops.Visible = CardsRemaining > 0;
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

        private void SortBadges(string method)
        {
            lblDrops.Text = localization.strings.sorting_results;

            switch (method)
            {
                case "mostcards":
                    AllBadges = AllBadges.OrderByDescending(b => b.RemainingCard).ToList();
                    break;
                case "leastcards":
                    AllBadges = AllBadges.OrderBy(b => b.RemainingCard).ToList();
                    break;
                case "mostvalue":
                    try
                    {
                        string query = string.Format("http://api.enhancedsteam.com/market_data/average_card_prices/im.php?appids={0}", string.Join(",", AllBadges.Select(b => b.AppId)));
                        string json = new WebClient() { Encoding = Encoding.UTF8 }.DownloadString(query);
                        EnhancedSteamHelper convertedJson = JsonConvert.DeserializeObject<EnhancedSteamHelper>(json);

                        foreach (Avg price in convertedJson.AvgValues)
                        {
                            Badge badge = AllBadges.SingleOrDefault(b => b.AppId == price.AppId);

                            if (badge != null)
                            {
                                badge.AveragePrice = price.AvgPrice;
                            }
                        }

                        AllBadges = AllBadges.OrderByDescending(b => b.AveragePrice).ToList();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                    break;
                default:
                    return;
            }
        }

        private void UpdateIdleProcesses()
        {
            foreach (Badge badge in CanIdleBadges.Where(b => !Equals(b, CurrentBadge)))
            {
                if (badge.HoursPlayed >= 2 && badge.InIdle)
                {
                    badge.StopIdle();
                }

                if (badge.HoursPlayed < 2 && CanIdleBadges.Count(b => b.InIdle) < 30)
                {
                    badge.Idle();
                }
            }

            RefreshGamesStateListView();

            if (!CanIdleBadges.Any(b => b.InIdle))
            {
                NextIdle();
            }

            UpdateStateInfo();
        }

        private void StartIdle()
        {
            //Kill all existing processes before starting any new ones
            //This prevents rogue processes from interfering with idling time and slowing card drops
            try
            {
                string username = WindowsIdentity.GetCurrent().Name;

                foreach (Process process in Process.GetProcessesByName("steam-idle"))
                {
                    ManagementObjectSearcher searcher = new ManagementObjectSearcher($"Select * From Win32_Process Where ProcessID = {process.Id}");
                    ManagementObjectCollection processList = searcher.Get();

                    foreach (ManagementObject managementObject in processList)
                    {
                        string[] argList = new string[] { string.Empty, string.Empty };
                        int returnVal = Convert.ToInt32(managementObject.InvokeMethod("GetOwner", argList));

                        if (returnVal == 0)
                        {
                            if (argList[1] + "\\" + argList[0] == username)
                            {
                                process.Kill();
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            //Check if user is authenticated and if any badge left to idle
            //There should be check for IsCookieReady, but property is set in timer tick, so it could take some time to be set.
            if (string.IsNullOrWhiteSpace(Settings.Default.sessionid) || !IsSteamReady)
            {
                ResetClientStatus();
                return;
            }

            if (ReloadCount > 0)
            {
                return;
            }

            if (CanIdleBadges.Any())
            {
                statistics.setRemainingCards((uint)CardsRemaining);
                tmrStatistics.Enabled = true;
                tmrStatistics.Start();

                if (Settings.Default.OnlyOneGameIdle)
                {
                    StartSoloIdle(CanIdleBadges.First());
                }
                else
                {
                    if (Settings.Default.OneThenMany)
                    {
                        IEnumerable<Badge> multi = CanIdleBadges.Where(b => b.HoursPlayed >= 2);

                        if (multi.Count() > 0)
                        {
                            StartSoloIdle(multi.First());
                        }
                        else
                        {
                            StartMultipleIdle();
                        }
                    }
                    else
                    {
                        IEnumerable<Badge> multi = CanIdleBadges.Where(b => b.HoursPlayed < 2);

                        if (multi.Count() >= 2)
                        {
                            StartMultipleIdle();
                        }
                        else
                        {
                            StartSoloIdle(CanIdleBadges.First());
                        }
                    }
                }
            }
            else
            {
                IdleComplete();
            }

            UpdateStateInfo();
        }

        private void NextIdle()
        {
            //Stop idling the current game
            StopIdle();

            //Check if user is authenticated and if any badge left to idle
            //There should be check for IsCookieReady, but property is set in timer tick, so it could take some time to be set.
            if (string.IsNullOrWhiteSpace(Settings.Default.sessionid) || !IsSteamReady)
            {
                ResetClientStatus();
                return;
            }

            if (CanIdleBadges.Any())
            {
                //Give the user notification that the next game will start soon
                lblCurrentStatus.Text = localization.strings.loading_next;

                //Make a short but random amount of time pass
                Random rand = new Random();
                int wait = rand.Next(3, 9);
                wait = wait * 1000;

                tmrStartNext.Interval = wait;
                tmrStartNext.Enabled = true;
                return;
            }

            IdleComplete();
        }

        private void StartSoloIdle(Badge badge)
        {
            //Set the currentAppID value
            CurrentBadge = badge;

            //Place user "In game" for card drops
            CurrentBadge.Idle();

            //Update game name
            lblGameName.Visible = true;
            lblGameName.Text = CurrentBadge.Name;

            lsvGamesState.Visible = false;
            gameToolStripMenuItem.Enabled = true;

            //Update game image
            try
            {
                picApp.Load($"http://cdn.akamai.steamstatic.com/steam/apps/{CurrentBadge.StringId}/header_292x136.jpg");
                picApp.Visible = true;
            }
            catch (Exception ex)
            {
                Logger.Exception(ex, $"frmMain -> StartIdle -> load pic, for id = {CurrentBadge.AppId}");
            }

            //Update label controls
            lblCurrentRemaining.Text = $"{CurrentBadge.RemainingCard} {localization.strings.card_drops_remaining}";
            lblCurrentStatus.Text = localization.strings.currently_ingame;
            lblHoursPlayed.Visible = true;
            lblHoursPlayed.Text = $"{CurrentBadge.HoursPlayed} {localization.strings.hrs_on_record}";

            //Set progress bar values and show the footer
            pbIdle.Maximum = CurrentBadge.RemainingCard;
            pbIdle.Value = 0;
            ssFooter.Visible = true;

            //Start the animated "working" gif
            ptbIdleStatus.Image = Resources.imgSpin;

            //Start the timer that will check if drops remain
            tmrCardDropCheck.Enabled = true;

            //Reset the timer
            TimeLeft = CurrentBadge.RemainingCard == 1 ? 300 : 900;

            //Set the correct buttons on the form for pause / resume
            btnResume.Visible = false;
            btnPause.Visible = true;
            btnSkip.Visible = true;
            resumeIdlingToolStripMenuItem.Enabled = false;
            pauseIdlingToolStripMenuItem.Enabled = false;
            skipGameToolStripMenuItem.Enabled = false;

            double scale = CreateGraphics().DpiY * 3.9;
            Height = Convert.ToInt32(scale);
        }

        private void StartMultipleIdle()
        {
            UpdateIdleProcesses();

            //Update label controls
            lblCurrentRemaining.Text = localization.strings.update_games_status;
            lblCurrentStatus.Text = localization.strings.currently_ingame;

            lblGameName.Visible = false;
            lblHoursPlayed.Visible = false;
            ssFooter.Visible = true;
            gameToolStripMenuItem.Enabled = false;

            //Start the animated "working" gif
            ptbIdleStatus.Image = Resources.imgSpin;

            //Start the timer that will check if drops remain
            tmrCardDropCheck.Enabled = true;

            //Reset the timer
            TimeLeft = 360;

            //Show game
            lsvGamesState.Visible = true;
            picApp.Visible = false;
            RefreshGamesStateListView();

            //Set the correct buttons on the form for pause / resume
            btnResume.Visible = false;
            btnPause.Visible = false;
            btnSkip.Visible = false;
            resumeIdlingToolStripMenuItem.Enabled = false;
            pauseIdlingToolStripMenuItem.Enabled = false;
            skipGameToolStripMenuItem.Enabled = false;

            double scale = CreateGraphics().DpiY * 3.86;
            Height = Convert.ToInt32(scale);
        }

        private void RefreshGamesStateListView()
        {
            lsvGamesState.Items.Clear();

            foreach (Badge badge in CanIdleBadges.Where(b => b.InIdle))
            {
                ListViewItem line = new ListViewItem(badge.Name);
                line.SubItems.Add(badge.HoursPlayed.ToString());
                lsvGamesState.Items.Add(line);
            }
        }

        private void StopIdle()
        {
            try
            {
                lblGameName.Visible = false;
                picApp.Image = null;
                picApp.Visible = false;
                lsvGamesState.Visible = false;
                btnPause.Visible = false;
                btnSkip.Visible = false;
                lblCurrentStatus.Text = localization.strings.not_ingame;
                lblHoursPlayed.Visible = false;
                ptbIdleStatus.Image = null;

                //Stop the card drop check timer
                tmrCardDropCheck.Enabled = false;

                //Stop the statistics timer
                tmrStatistics.Stop();
                tmrStatistics.Enabled = false;

                //Hide the status bar
                ssFooter.Visible = false;

                //Resize the form
                Graphics graphics = CreateGraphics();
                double scale = graphics.DpiY * 1.9583;
                Height = Convert.ToInt32(scale);

                //Kill the idling process
                foreach (Badge badge in AllBadges.Where(b => b.InIdle))
                    badge.StopIdle();
            }
            catch (Exception ex)
            {
                Logger.Exception(ex, "frmMain -> StopIdle");
            }
        }

        private void IdleComplete()
        {
            //Deactivate the timer control and inform the user that the program is finished
            tmrCardDropCheck.Enabled = false;
            lblCurrentStatus.Text = localization.strings.idling_complete;

            lblGameName.Visible = false;
            btnPause.Visible = false;
            btnSkip.Visible = false;

            //Resize the form
            Graphics graphics = CreateGraphics();
            double scale = graphics.DpiY * 1.9583;
            Height = Convert.ToInt32(scale);
        }

        private async Task LoadBadgesAsync()
        {
            //Settings.Default.myProfileURL = http://steamcommunity.com/id/USER
            string profileLink = $"{Settings.Default.myProfileURL}/badges";
            List<string> pages = new List<string>() { "?p=1" };
            int pagesCount = 1;

            try
            {
                //Load Page 1 and check how many pages there are
                string pageURL = $"{profileLink}/?p=1";
                string response = await CookieClient.GetHttpAsync(pageURL);

                //Response should be empty. User should be unauthorised
                if (string.IsNullOrEmpty(response))
                {
                    RetryCount++;

                    if (RetryCount == 18)
                    {
                        ResetClientStatus();
                        return;
                    }

                    throw new Exception();
                }

                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(response);

                //If user is authenticated, check page count. If user is not authenticated, pages are different
                HtmlAgilityPack.HtmlNodeCollection pageNodes = document.DocumentNode.SelectNodes("//a[@class=\"pagelink\"]");

                if (pageNodes != null)
                {
                    pages.AddRange(pageNodes.Select(p => p.Attributes["href"].Value).Distinct());
                    pages = pages.Distinct().ToList();
                }

                string lastpage = pages.Last().ToString().Replace("?p=", "");
                pagesCount = Convert.ToInt32(lastpage);

                //Get all badges from current page
                ProcessBadgesOnPage(document);

                //Load other pages
                for (int index = 2; index <= pagesCount; index++)
                {
                    lblDrops.Text = $"{localization.strings.reading_badge_page} {index}/{pagesCount}, {localization.strings.please_wait}";

                    //Load Page 2+
                    pageURL = string.Format("{0}/?p={1}", profileLink, index);
                    response = await CookieClient.GetHttpAsync(pageURL);

                    //Response should be empty. User should be unauthorised.
                    if (string.IsNullOrWhiteSpace(response))
                    {
                        RetryCount++;

                        if (RetryCount == 18)
                        {
                            ResetClientStatus();
                            return;
                        }

                        throw new Exception();
                    }

                    document.LoadHtml(response);

                    //Get all badges from current page
                    ProcessBadgesOnPage(document);
                }
            }
            catch (Exception ex)
            {
                Logger.Exception(ex, $"Badge -> LoadBadgesAsync, for profile = {Settings.Default.myProfileURL}");

                //badge page didn't load
                ptbReadingPage.Image = null;
                ptbIdleStatus.Image = null;
                lblDrops.Text = localization.strings.badge_didnt_load.Replace("__num__", "10");
                lblIdle.Text = "";

                //Set the form height
                Graphics graphics = CreateGraphics();
                double scale = graphics.DpiY * 1.625;
                Height = Convert.ToInt32(scale);
                ssFooter.Visible = false;

                ReloadCount = 1;
                tmrBadgeReload.Enabled = true;

                return;
            }

            RetryCount = 0;
            SortBadges(Settings.Default.sort);

            ptbReadingPage.Visible = false;
            UpdateStateInfo();

            if (CardsRemaining == 0)
            {
                IdleComplete();
            }
        }

        ///<summary>
        ///Processes all badges on page
        ///</summary>
        ///<param name="document">HTML document (1 page) from x</param>
        private void ProcessBadgesOnPage(HtmlDocument document)
        {
            foreach (HtmlAgilityPack.HtmlNode badge in document.DocumentNode.SelectNodes("//div[@class=\"badge_row is_link\"]"))
            {
                string appIdNode = badge.SelectSingleNode(".//a[@class=\"badge_row_overlay\"]").Attributes["href"].Value;
                string appId = Regex.Match(appIdNode, @"gamecards/(\d+)/").Groups[1].Value;

                if (string.IsNullOrWhiteSpace(appId) || Settings.Default.blacklist.Contains(appId) || appId == "368020" || appId == "335590" || appIdNode.Contains("border=1"))
                {
                    continue;
                }

                HtmlAgilityPack.HtmlNode hoursNode = badge.SelectSingleNode(".//div[@class=\"badge_title_stats_playtime\"]");
                string hours = hoursNode == null ? string.Empty : Regex.Match(hoursNode.InnerText, @"[0-9\.,]+").Value;

                HtmlAgilityPack.HtmlNode nameNode = badge.SelectSingleNode(".//div[@class=\"badge_title\"]");
                string name = WebUtility.HtmlDecode(nameNode.FirstChild.InnerText).Trim();

                HtmlAgilityPack.HtmlNode cardNode = badge.SelectSingleNode(".//span[@class=\"progress_info_bold\"]");
                string cards = cardNode == null ? string.Empty : Regex.Match(cardNode.InnerText, @"[0-9]+").Value;

                Badge badgeInMemory = AllBadges.FirstOrDefault(b => b.StringId == appId);

                if (badgeInMemory != null)
                {
                    badgeInMemory.UpdateStats(cards, hours);
                }
                else
                {
                    AllBadges.Add(new Badge(appId, name, cards, hours));
                }
            }
        }

        private async Task CheckCardDrops(Badge badge)
        {
            if (!await badge.CanCardDrops())
            {
                NextIdle();
            }
            else
            {
                //Resets the clock based on the number of remaining drops
                TimeLeft = badge.RemainingCard == 1 ? 300 : 900;
            }

            lblCurrentRemaining.Text = $"{badge.RemainingCard} {localization.strings.card_drops_remaining}";
            pbIdle.Value = pbIdle.Maximum - badge.RemainingCard;
            lblHoursPlayed.Text = "{badge.HoursPlayed} {localization.strings.hrs_on_record}";
            UpdateStateInfo();
        }

        ///<summary>
        ///Performs reset to initial state
        ///</summary>
        private void ResetClientStatus()
        {
            //Clear the settings
            Settings.Default.sessionid = null;
            Settings.Default.steamLogin = null;
            Settings.Default.myProfileURL = null;
            Settings.Default.steamparental = null;
            Settings.Default.Save();

            //Stop the steam-idle process
            StopIdle();

            //Clear the badges list
            AllBadges.Clear();

            //Resize the form
            Graphics graphics = CreateGraphics();
            double scale = graphics.DpiY * 1.625;
            Height = Convert.ToInt32(scale);

            //Hide signed user name
            if (Settings.Default.showUsername)
            {
                lblSteamUsername.Text = string.Empty;
                lblSteamUsername.Visible = false;
            }

            //Hide spinners
            ptbReadingPage.Visible = false;

            //Hide lblDrops and lblIdle
            lblDrops.Visible = false;
            lblIdle.Visible = false;

            //Set IsCookieReady to false
            IsCookieReady = false;

            //Re-enable tmrReadyToGo
            tmrReadyToGo.Enabled = true;
        }

        ////////////////////////////////////////TICKS////////////////////////////////////////

        private void tmrCheckCookieData_Tick(object sender, EventArgs e)
        {
            bool connected = !string.IsNullOrWhiteSpace(Settings.Default.sessionid) && !string.IsNullOrWhiteSpace(Settings.Default.steamLogin);

            lblCookieStatus.Text = connected ? localization.strings.idle_master_connected : localization.strings.idle_master_notconnected;
            lblCookieStatus.ForeColor = connected ? Color.Green : Color.Black;

            ptbCookieStatus.Image = connected ? Resources.imgTrue : Resources.imgFalse;
            lnkSignIn.Visible = !connected;
            lnkResetCookies.Visible = connected;
            IsCookieReady = connected;
        }

        private void tmrCheckSteam_Tick(object sender, EventArgs e)
        {
            bool isSteamRunning = SteamAPI.IsSteamRunning() || Settings.Default.ignoreclient;

            lblSteamStatus.Text = isSteamRunning ? (Settings.Default.ignoreclient ? localization.strings.steam_ignored : localization.strings.steam_running) : localization.strings.steam_notrunning;
            lblSteamStatus.ForeColor = isSteamRunning ? Color.Green : Color.Black;
            ptbSteamStatus.Image = isSteamRunning ? Resources.imgTrue : Resources.imgFalse;

            skipGameToolStripMenuItem.Enabled = isSteamRunning;
            pauseIdlingToolStripMenuItem.Enabled = isSteamRunning;
            IsSteamReady = isSteamRunning;
        }

        private async void tmrReadyToGo_Tick(object sender, EventArgs e)
        {
            if (!IsCookieReady || !IsSteamReady)
            {
                return;
            }

            //Update the form elements
            if (Settings.Default.showUsername)
            {
                lblSteamUsername.Text = SteamProfile.GetSignedAs();
                lblSteamUsername.Visible = true;
            }

            lblDrops.Visible = true;
            lblDrops.Text = $"{localization.strings.reading_badge_page}, {localization.strings.please_wait}";
            lblIdle.Visible = false;
            ptbReadingPage.Visible = true;

            tmrReadyToGo.Enabled = false;

            await LoadBadgesAsync();

            StartIdle();
        }

        private async void tmrCardDropCheck_Tick(object sender, EventArgs e)
        {
            if (TimeLeft <= 0)
            {
                tmrCardDropCheck.Enabled = false;

                if (CurrentBadge != null)
                {
                    CurrentBadge.Idle();
                    await CheckCardDrops(CurrentBadge);
                }

                bool isMultipleIdle = CanIdleBadges.Any(b => !Equals(b, CurrentBadge) && b.InIdle);

                if (isMultipleIdle)
                {
                    await LoadBadgesAsync();
                    UpdateIdleProcesses();

                    isMultipleIdle = CanIdleBadges.Any(b => b.HoursPlayed < 2 && b.InIdle);

                    if (isMultipleIdle)
                    {
                        TimeLeft = 360;
                    }
                }

                //Check if user is authenticated and if any badge left to idle
                //There should be check for IsCookieReady, but property is set in timer tick, so it could take some time to be set
                tmrCardDropCheck.Enabled = !string.IsNullOrWhiteSpace(Settings.Default.sessionid) && IsSteamReady && CanIdleBadges.Any() && TimeLeft != 0;
            }
            else
            {
                TimeLeft = TimeLeft - 1;
                lblTimer.Text = TimeSpan.FromSeconds(TimeLeft).ToString(@"mm\:ss");
            }
        }

        private void tmrStartNext_Tick(object sender, EventArgs e)
        {
            tmrStartNext.Enabled = false;
            StartIdle();
        }

        private void tmrBadgeReload_Tick(object sender, EventArgs e)
        {
            ReloadCount = ReloadCount + 1;
            lblDrops.Text = localization.strings.badge_didnt_load.Replace("__num__", (10 - ReloadCount).ToString());

            if (ReloadCount == 10)
            {
                tmrBadgeReload.Enabled = false;
                tmrReadyToGo.Enabled = true;
                ReloadCount = 0;
            }
        }

        private void tmrStatistics_Tick(object sender, EventArgs e)
        {
            statistics.increaseMinutesIdled();
            statistics.checkCardRemaining((uint)CardsRemaining);
        }

        ////////////////////////////////////////CLICKS////////////////////////////////////////

        private void btnSkip_Click(object sender, EventArgs e)
        {
            if (!IsSteamReady)
            {
                return;
            }

            StopIdle();
            AllBadges.RemoveAll(b => Equals(b, CurrentBadge));
            StartIdle();
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            if (!IsSteamReady)
            {
                return;
            }

            //Stop the steam-idle process
            StopIdle();

            //Indicate to the user that idling has been paused
            lblCurrentStatus.Text = localization.strings.idling_paused;

            //Set the correct button visibility
            btnResume.Visible = true;
            btnPause.Visible = false;
            pauseIdlingToolStripMenuItem.Enabled = false;
            resumeIdlingToolStripMenuItem.Enabled = true;

            //Focus the resume button
            btnResume.Focus();
        }

        private void btnResume_Click(object sender, EventArgs e)
        {
            //Resume idling
            StartIdle();

            pauseIdlingToolStripMenuItem.Enabled = true;
            resumeIdlingToolStripMenuItem.Enabled = false;
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Show the form
            string previous = Settings.Default.sort;
            bool previous_behavior = Settings.Default.OnlyOneGameIdle;
            bool previous_behavior2 = Settings.Default.OneThenMany;

            frmSettings form = new frmSettings();
            form.ShowDialog();

            if (previous != Settings.Default.sort || previous_behavior != Settings.Default.OnlyOneGameIdle || previous_behavior2 != Settings.Default.OneThenMany)
            {
                StopIdle();
                AllBadges.Clear();
                tmrReadyToGo.Enabled = true;
            }

            if (Settings.Default.showUsername && IsCookieReady)
            {
                lblSteamUsername.Text = SteamProfile.GetSignedAs();
                lblSteamUsername.Visible = Settings.Default.showUsername;
            }
        }

        private void pauseIdlingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnPause.PerformClick();
        }

        private void resumeIdlingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnResume.PerformClick();
        }

        private void skipGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnSkip.PerformClick();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAbout form = new frmAbout();
            form.ShowDialog();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
        }

        private void lblCurrentRemaining_Click(object sender, EventArgs e)
        {
            if (TimeLeft > 2)
            {
                TimeLeft = 2;
            }
        }

        private void blacklistToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmBlacklist form = new frmBlacklist();
            form.ShowDialog();

            if (Settings.Default.blacklist.Cast<string>().Any(appid => appid == CurrentBadge.StringId))
            {
                btnSkip.PerformClick();
            }
        }

        private void blacklistCurrentGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings.Default.blacklist.Add(CurrentBadge.StringId);
            Settings.Default.Save();

            btnSkip.PerformClick();
        }

        private void changelogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmChangelog form = new frmChangelog();
            form.Show();
        }

        private void statisticsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmStatistics form = new frmStatistics(statistics);
            form.ShowDialog();
        }

        private void officialGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("http://steamcommunity.com/groups/idlemastery");
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void lnkSignIn_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmBrowser form = new frmBrowser();
            form.ShowDialog();
        }

        private void lblGameName_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start($"http://store.steampowered.com/app/{CurrentBadge.AppId}");
        }

        private void lnkResetCookies_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ResetClientStatus();
        }
    }
}