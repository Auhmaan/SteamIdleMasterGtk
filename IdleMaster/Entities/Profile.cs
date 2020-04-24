using HtmlAgilityPack;
using IdleMaster.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Xml;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace IdleMaster.Entities
{
    public class Profile
    {
        //Properties
        public string Url { get; private set; }

        public string Username { get; private set; }

        public string Avatar { get; private set; }

        public List<Badge> Badges { get; private set; }

        public Badge CurrentBadge { get; private set; }

        public bool HasBadges
        {
            get
            {
                return Badges != null && Badges.Any();
            }
        }

        public bool IsIdling
        {
            get
            {
                return CurrentBadge != null;
            }
        }

        public bool IsPaused
        {
            get
            {
                return CurrentBadge != null && !CurrentBadge.IsIdling;
            }
        }

        //Methods
        public void LoadProfile()
        {
            string steamId64 = WebUtility.UrlDecode(Settings.Default.CookieLoginSecure)?.Split('|').First();

            Url = $"https://steamcommunity.com/profiles/{steamId64}";

            string xmlUrl = $"{Url}/?xml=1";
            XmlDocument xmlDocument = new XmlDocument();

            using (WebClient webClient = new WebClient())
            {
                string xml = webClient.DownloadString(xmlUrl);
                xmlDocument.LoadXml(xml);
            }

            XmlNode steamId = xmlDocument.SelectSingleNode("//steamID");
            Username = WebUtility.HtmlDecode(steamId?.InnerText);

            XmlNode avatarMedium = xmlDocument.SelectSingleNode("//avatarMedium");
            Avatar = avatarMedium?.InnerText;

            LoadBadges();
        }

        public void LoadBadges()
        {
            Badges = new List<Badge>();

            string profileLink = $"{Url}/badges";
            int totalPages = 1;
            int currentPage = 0;

            do
            {
                currentPage++;

                string response = CookieClient.GetHttp($"{profileLink}?p={currentPage}");

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

                HtmlNodeCollection pages = document.DocumentNode.SelectNodes("//a[@class='pagelink']");

                string href = pages?.Last().Attributes["href"]?.Value;
                string maxPages = href?.Split('=').Last();
                int.TryParse(maxPages, out totalPages);
            }
            while (currentPage < totalPages);
        }

        private bool ProcessBadgesOnPage(HtmlDocument document)
        {
            HtmlNodeCollection openBadges = document.DocumentNode.SelectNodes("//span[@class='progress_info_bold']");

            if (openBadges == null)
            {
                return false;
            }

            HtmlNodeCollection droppableBadges = document.DocumentNode.SelectNodes("//span[text()='PLAY']/ancestor::div[@class='badge_row is_link']");

            if (droppableBadges != null)
            {
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
            }

            HtmlNodeCollection allBadges = document.DocumentNode.SelectNodes("//div[@class='badge_row is_link']");

            if (allBadges.Count > openBadges.Count)
            {
                return false;
            }

            return true;
        }

        public void StartIdlingBadges()
        {
            if (!HasBadges)
            {
                return;
            }

            CurrentBadge = Badges.First();
            CurrentBadge.StartIdle();
        }

        public void PauseIdlingBadges()
        {
            if (!IsIdling)
            {
                return;
            }

            CurrentBadge.StopIdle();
        }

        public void ResumeIdlingBadges()
        {
            if (!IsIdling)
            {
                return;
            }

            CurrentBadge.StartIdle();
        }

        public void StopIdlingBadges()
        {
            if (!IsIdling)
            {
                return;
            }

            CurrentBadge.StopIdle();
            CurrentBadge = null;
        }

        public void CheckIdlingStatus(bool removeCurrentBadgeIfFinished = false)
        {
            if (!IsIdling)
            {
                return;
            }

            HtmlDocument document = new HtmlDocument();
            string response = CookieClient.GetHttp($"{Url}/gamecards/{CurrentBadge.AppId}");

            document.LoadHtml(response);

            HtmlNode cardsNode = document.DocumentNode.SelectSingleNode(".//span[@class='progress_info_bold']");
            string cards = cardsNode?.InnerText.Split(' ').First();

            HtmlNode playtimeNode = document.DocumentNode.SelectSingleNode(".//div[@class='badge_title_stats_playtime']");
            string playtime = WebUtility.HtmlDecode(playtimeNode?.InnerText).Trim().Split(' ').First();

            CurrentBadge.UpdateStats(cards, playtime);

            if (!CurrentBadge.HasDrops)
            {
                IdleNextBadge(removeCurrentBadgeIfFinished);
            }
        }

        private void IdleNextBadge(bool removeCurrentBadge = false)
        {
            if (!IsIdling)
            {
                return;
            }

            CurrentBadge.StopIdle();

            int nextBadgeIndex = Badges.IndexOf(CurrentBadge) + 1;

            if (removeCurrentBadge)
            {
                Badges.Remove(CurrentBadge);
                nextBadgeIndex--;
            }

            CurrentBadge = Badges[nextBadgeIndex];
            CurrentBadge.StartIdle();
        }
    }
}