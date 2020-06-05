using HtmlAgilityPack;
using IdleMaster.ControlEntities;
using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
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

        public Library Library { get; private set; } = new Library();

        //Methods
        public async Task LoadProfile()
        {
            string steamId64 = WebUtility.UrlDecode(UserSettings.Cookies.LoginSecure)?.Split('|').First();

            UserSettings.ProfileUrl = $"https://steamcommunity.com/profiles/{steamId64}";
            Url = UserSettings.ProfileUrl;

            string xmlUrl = $"{Url}/?xml=1";
            XmlDocument xmlDocument = new XmlDocument();

            using (WebClient webClient = new WebClient())
            {
                string xml = await webClient.DownloadStringTaskAsync(xmlUrl);
                xmlDocument.LoadXml(xml);
            }

            XmlNode steamId = xmlDocument.SelectSingleNode("//steamID");
            Username = WebUtility.HtmlDecode(steamId?.InnerText);

            XmlNode avatarMedium = xmlDocument.SelectSingleNode("//avatarMedium");
            Avatar = avatarMedium?.InnerText;

            await LoadGames();
        }

        public async Task LoadGames()
        {
            string profileLink = $"{Url}/badges";
            int totalPages = 1;
            int currentPage = 0;

            do
            {
                currentPage++;

                string response = await UserWebClient.GetHttp($"{profileLink}?p={currentPage}");

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

                    Game existingBadge = Library.Games.FirstOrDefault(x => x.AppId == appId);

                    int.TryParse(cards, out int remainingCards);
                    double.TryParse(playtime, NumberStyles.Any, new NumberFormatInfo(), out double hoursPlayed);

                    if (existingBadge != null)
                    {
                        existingBadge.UpdateStats(remainingCards, hoursPlayed);
                        continue;
                    }

                    Library.Games.Add(new Game(appId, name, remainingCards, hoursPlayed));
                }
            }

            HtmlNodeCollection allBadges = document.DocumentNode.SelectNodes("//div[@class='badge_row is_link']");

            if (allBadges.Count > openBadges.Count)
            {
                return false;
            }

            return true;
        }
    }
}