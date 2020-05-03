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
                return Badges.Any(x => x.IsNormalIdling || x.IsFastIdling);
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

            Badges.ForEach(x => x.StartIdling());
        }

        public void StopIdlingBadges()
        {
            if (!IsIdling)
            {
                return;
            }

            Badges.ForEach(x => x.StopIdling());
        }

        public void CheckNormalIdlingStatus()
        {
            if (!IsIdling)
            {
                return;
            }

            foreach (Badge badge in Badges.Where(x => x.IsNormalIdling))
            {
                HtmlDocument document = new HtmlDocument();
                string response = CookieClient.GetHttp($"{Url}/gamecards/{badge.AppId}");

                document.LoadHtml(response);

                HtmlNode cardsNode = document.DocumentNode.SelectSingleNode(".//span[@class='progress_info_bold']");
                string cards = cardsNode?.InnerText.Split(' ').First();

                HtmlNode playtimeNode = document.DocumentNode.SelectSingleNode(".//div[@class='badge_title_stats_playtime']");
                string playtime = WebUtility.HtmlDecode(playtimeNode?.InnerText).Trim().Split(' ').First();

                badge.UpdateStats(cards, playtime);

                if (!badge.HasDrops)
                {
                    badge.StopIdling();
                }
            }

            Badges.RemoveAll(x => x.IsNormalIdling && !x.HasDrops);
        }

        public void StartFastIdlingBadges()
        {
            if (!HasBadges)
            {
                return;
            }

            foreach (Badge badge in Badges.Where(x => x.IsFastIdling).ToList())
            {
                badge.StartIdling();
            }
        }

        public void StopFastIdlingBadges()
        {
            if (!IsIdling)
            {
                return;
            }

            foreach (Badge badge in Badges.Where(x => x.IsFastIdling).ToList())
            {
                badge.StopIdling();
            }
        }

        public void CheckFastIdlingStatus()
        {
            if (!IsIdling)
            {
                return;
            }

            foreach (Badge badge in Badges.Where(x => x.IsFastIdling))
            {
                HtmlDocument document = new HtmlDocument();
                string response = CookieClient.GetHttp($"{Url}/gamecards/{badge.AppId}");

                document.LoadHtml(response);

                HtmlNode cardsNode = document.DocumentNode.SelectSingleNode(".//span[@class='progress_info_bold']");
                string cards = cardsNode?.InnerText.Split(' ').First();

                HtmlNode playtimeNode = document.DocumentNode.SelectSingleNode(".//div[@class='badge_title_stats_playtime']");
                string playtime = WebUtility.HtmlDecode(playtimeNode?.InnerText).Trim().Split(' ').First();

                badge.UpdateStats(cards, playtime);

                if (!badge.HasDrops)
                {
                    badge.StopIdling();
                }
            }

            Badges.RemoveAll(x => x.IsFastIdling && !x.HasDrops);
        }
    }
}