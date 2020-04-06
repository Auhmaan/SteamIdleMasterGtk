using HtmlAgilityPack;
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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace IdleMaster
{
    public partial class frmMain : Form
    {
        private Statistics statistics = new Statistics();

        public List<Badge> Badges;
        public Badge CurrentBadge;

        public int GamesRemaining
        {
            get
            {
                return Badges.Count();
            }
        }
        public int CardsRemaining
        {
            get
            {
                return Badges.Sum(x => x.RemainingCards);
            }
        }

        public bool IsCookieReady;
        public bool IsSteamReady;
        public int TimeLeft = 900;
        public int ReloadCount = 0;

        public frmMain()
        {
            InitializeComponent();
            Badges = new List<Badge>();
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
                string language;

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
            Badges.Clear();

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

        private void UpdateStateInfo()
        {
            //Update totals
            if (ReloadCount == 0)
            {
                lblIdle.Text = $"{GamesRemaining} {localization.strings.games_left_to_idle}, {Badges.Count(x => x.IsIdling)} {localization.strings.idle_now}.";
                lblDrops.Text = $"{CardsRemaining} {localization.strings.card_drops_remaining}";
                lblIdle.Visible = GamesRemaining > 0;
                lblDrops.Visible = CardsRemaining > 0;
            }
        }

        private void UpdateIdleProcesses()
        {
            foreach (Badge badge in Badges.Where(x => !Equals(x, CurrentBadge)))
            {
                if (badge.HoursPlayed >= 2 && badge.IsIdling)
                {
                    badge.StopIdle();
                }

                if (badge.HoursPlayed < 2 && Badges.Count(x => x.IsIdling) < 30)
                {
                    badge.Idle();
                }
            }

            RefreshGamesStateListView();

            if (!Badges.Any(x => x.IsIdling))
            {
                NextIdle();
            }

            UpdateStateInfo();
        }

        private void RefreshGamesStateListView()
        {
            lsvGamesState.Items.Clear();

            foreach (Badge badge in Badges.Where(x => x.IsIdling))
            {
                ListViewItem line = new ListViewItem(badge.Name);
                line.SubItems.Add(badge.HoursPlayed.ToString());
                lsvGamesState.Items.Add(line);
            }
        }

        ////////////////////////////////////////BADGES////////////////////////////////////////

        private async Task CheckCardDrops(Badge badge)
        {
            if (!await badge.CanDropCards())
            {
                NextIdle();
            }
            else
            {
                //Resets the clock based on the number of remaining drops
                TimeLeft = badge.RemainingCards == 1 ? 300 : 900;
            }

            lblCurrentRemaining.Text = $"{badge.RemainingCards} {localization.strings.card_drops_remaining}";
            pbIdle.Value = pbIdle.Maximum - badge.RemainingCards;
            lblHoursPlayed.Text = "{badge.HoursPlayed} {localization.strings.hrs_on_record}";
            UpdateStateInfo();
        }

        private async Task LoadBadgesAsync()
        {
            try
            {
                string profileLink = $"{Settings.Default.myProfileURL}/badges";
                int currentPage = 0;
                int totalPages = 1;

                do
                {
                    currentPage++;
                    string response = await CookieClient.GetHttpAsync($"{profileLink}?p={currentPage}");

                    if (string.IsNullOrWhiteSpace(response))
                    {
                        ResetClientStatus();
                        return;
                    }

                    HtmlDocument document = new HtmlDocument();
                    document.LoadHtml(response);

                    //Process badges in the current page, if the current page has no droppable badges the process stops
                    if (!ProcessBadgesOnPage(document))
                    {
                        break;
                    }

                    lblDrops.Text = $"{localization.strings.reading_badge_page} {currentPage}/{totalPages}, {localization.strings.please_wait}";

                    //If the current page is the first one, calculate how many pages exist
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

            SortBadges(Settings.Default.sort);

            ptbReadingPage.Visible = false;
            UpdateStateInfo();

            if (CardsRemaining == 0)
            {
                IdleComplete();
            }
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

                Badge existingBadge = Badges.FirstOrDefault(x => x.AppId == appId);

                if (existingBadge != null)
                {
                    existingBadge.UpdateStats(cards, playtime);
                    continue;
                }

                Badge newBadge = new Badge(appId, name, cards, playtime);
                Badges.Add(newBadge);
            }

            HtmlNodeCollection allBadges = document.DocumentNode.SelectNodes("//div[contains(@class,'is_link')]");

            if (allBadges.Count > droppableBadges.Count)
            {
                return false;
            }

            return true;
        }

        private void SortBadges(string method)
        {
            lblDrops.Text = localization.strings.sorting_results;

            switch (method)
            {
                case "mostcards":
                    Badges = Badges.OrderByDescending(x => x.RemainingCards).ToList();
                    break;
                case "leastcards":
                    Badges = Badges.OrderBy(x => x.RemainingCards).ToList();
                    break;
                case "mostvalue":
                    try
                    {
                        string query = string.Format("http://api.enhancedsteam.com/market_data/average_card_prices/im.php?appids={0}", string.Join(",", Badges.Select(x => x.AppId)));
                        string json = new WebClient() { Encoding = Encoding.UTF8 }.DownloadString(query);
                        EnhancedSteamHelper convertedJson = JsonConvert.DeserializeObject<EnhancedSteamHelper>(json);

                        foreach (Avg price in convertedJson.AvgValues)
                        {
                            Badge badge = Badges.SingleOrDefault(x => x.AppId == price.AppId);

                            if (badge != null)
                            {
                                badge.AveragePrice = price.AvgPrice;
                            }
                        }

                        Badges = Badges.OrderByDescending(x => x.AveragePrice).ToList();
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

        ////////////////////////////////////////IDLE////////////////////////////////////////

        private void StartIdle()
        {
            //Kill all existing processes before starting any new ones
            //This prevents rogue processes from interfering with idling time and slowing card drops
            try
            {
                string username = WindowsIdentity.GetCurrent().Name;

                foreach (Process process in Process.GetProcessesByName("steam-idle"))
                {
                    ManagementObjectSearcher searcher = new ManagementObjectSearcher($"SELECT * FROM Win32_Process WHERE PROCESSID = {process.Id}");
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

            if (!Badges.Any())
            {
                IdleComplete();
                UpdateStateInfo();
                return;
            }

            statistics.SetRemainingCards(CardsRemaining);
            tmrStatistics.Enabled = true;
            tmrStatistics.Start();

            IEnumerable<Badge> multi = null;

            switch (Settings.Default.IdleStyle)
            {
                case IdleType.Single:
                    StartSoloIdle(Badges.First());
                    break;
                case IdleType.Multiple:
                    multi = Badges.Where(x => x.HoursPlayed < 2);

                    if (multi.Count() >= 2)
                    {
                        StartMultipleIdle();
                    }
                    else
                    {
                        StartSoloIdle(Badges.First());
                    }

                    break;
                case IdleType.Both:
                    multi = Badges.Where(x => x.HoursPlayed >= 2);

                    if (multi.Count() > 0)
                    {
                        StartSoloIdle(multi.First());
                    }
                    else
                    {
                        StartMultipleIdle();
                    }

                    break;
            }

            UpdateStateInfo();
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
                picApp.Load($"http://cdn.akamai.steamstatic.com/steam/apps/{CurrentBadge.AppId}/header_292x136.jpg");
                picApp.Visible = true;
            }
            catch (Exception ex)
            {
                Logger.Exception(ex, $"frmMain -> StartIdle -> load pic, for id = {CurrentBadge.AppId}");
            }

            //Update label controls
            lblCurrentRemaining.Text = $"{CurrentBadge.RemainingCards} {localization.strings.card_drops_remaining}";
            lblCurrentStatus.Text = localization.strings.currently_ingame;
            lblHoursPlayed.Visible = true;
            lblHoursPlayed.Text = $"{CurrentBadge.HoursPlayed} {localization.strings.hrs_on_record}";

            //Set progress bar values and show the footer
            pbIdle.Maximum = CurrentBadge.RemainingCards;
            pbIdle.Value = 0;
            ssFooter.Visible = true;

            //Start the animated "working" gif
            ptbIdleStatus.Image = Resources.imgSpin;

            //Start the timer that will check if drops remain
            tmrCardDropCheck.Enabled = true;

            //Reset the timer
            TimeLeft = CurrentBadge.RemainingCards == 1 ? 300 : 900;

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
                Badges.ForEach(x => x.StopIdle());
            }
            catch (Exception ex)
            {
                Logger.Exception(ex, "frmMain -> StopIdle");
            }
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

            if (Badges.Any())
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

                bool isMultipleIdle = Badges.Any(x => !Equals(x, CurrentBadge) && x.IsIdling);

                if (isMultipleIdle)
                {
                    await LoadBadgesAsync();
                    UpdateIdleProcesses();

                    isMultipleIdle = Badges.Any(x => x.HoursPlayed < 2 && x.IsIdling);

                    if (isMultipleIdle)
                    {
                        TimeLeft = 360;
                    }
                }

                //Check if user is authenticated and if any badge left to idle
                //There should be check for IsCookieReady, but property is set in timer tick, so it could take some time to be set
                tmrCardDropCheck.Enabled = !string.IsNullOrWhiteSpace(Settings.Default.sessionid) && IsSteamReady && Badges.Any() && TimeLeft != 0;
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
            statistics.IncreaseMinutesIdled();
            statistics.CheckCardRemaining(CardsRemaining);
        }

        ////////////////////////////////////////CLICKS////////////////////////////////////////

        private void btnSkip_Click(object sender, EventArgs e)
        {
            if (!IsSteamReady)
            {
                return;
            }

            StopIdle();
            Badges.RemoveAll(x => Equals(x, CurrentBadge));
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
                Badges.Clear();
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

            if (Settings.Default.blacklist.Cast<string>().Any(x => x == CurrentBadge.AppId))
            {
                btnSkip.PerformClick();
            }
        }

        private void blacklistCurrentGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings.Default.blacklist.Add(CurrentBadge.AppId);
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