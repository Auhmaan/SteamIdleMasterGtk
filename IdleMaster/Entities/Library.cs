using HtmlAgilityPack;
using IdleMaster.ControlEntities;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace IdleMaster.Entities
{
    public class Library
    {
        public List<Game> Games { get; private set; }

        public Library()
        {
            Games = new List<Game>();
        }

        public bool HasGames
        {
            get
            {
                return Games != null && Games.Any();
            }
        }

        public bool IsIdling
        {
            get
            {
                return Games != null && Games.Any(x => x.IsNormalIdling || x.IsFastIdling);
            }
        }

        public void StartIdling()
        {
            if (!HasGames)
            {
                return;
            }

            foreach (Game game in Games.Take(UserSettings.GamesToIdle))
            {
                game.StartIdling();
            }
        }

        public void StopIdling()
        {
            if (!IsIdling)
            {
                return;
            }

            foreach (Game game in Games.Where(x => x.IsIdling))
            {
                game.StopIdling();
            }
        }

        public void CheckNormalIdlingStatus()
        {
            if (!IsIdling)
            {
                return;
            }

            foreach (Game game in Games.Where(x => x.IsNormalIdling))
            {
                HtmlDocument document = new HtmlDocument();
                string response = CookieClient.GetHttp($"{UserSettings.ProfileUrl}/gamecards/{game.AppId}");

                document.LoadHtml(response);

                HtmlNode cardsNode = document.DocumentNode.SelectSingleNode(".//span[@class='progress_info_bold']");
                string cards = cardsNode?.InnerText.Split(' ').First();

                HtmlNode playtimeNode = document.DocumentNode.SelectSingleNode(".//div[@class='badge_title_stats_playtime']");
                string playtime = WebUtility.HtmlDecode(playtimeNode?.InnerText).Trim().Split(' ').First();

                game.UpdateStats(cards, playtime);

                if (!game.HasDrops)
                {
                    game.StopIdling();
                }
            }

            Games.RemoveAll(x => x.IsNormalIdling && !x.HasDrops);
        }

        public void StartFastIdling()
        {
            if (!HasGames)
            {
                return;
            }

            foreach (Game game in Games.Where(x => x.IsFastIdling))
            {
                game.StartIdling();
            }
        }

        public void StopFastIdling()
        {
            if (!IsIdling)
            {
                return;
            }

            foreach (Game game in Games.Where(x => x.IsFastIdling).ToList())
            {
                game.StopIdling();
            }
        }

        public void CheckFastIdlingStatus()
        {
            if (!IsIdling)
            {
                return;
            }

            foreach (Game game in Games.Where(x => x.IsFastIdling))
            {
                HtmlDocument document = new HtmlDocument();
                string response = CookieClient.GetHttp($"{UserSettings.ProfileUrl}/gamecards/{game.AppId}");

                document.LoadHtml(response);

                HtmlNode cardsNode = document.DocumentNode.SelectSingleNode(".//span[@class='progress_info_bold']");
                string cards = cardsNode?.InnerText.Split(' ').First();

                HtmlNode playtimeNode = document.DocumentNode.SelectSingleNode(".//div[@class='badge_title_stats_playtime']");
                string playtime = WebUtility.HtmlDecode(playtimeNode?.InnerText).Trim().Split(' ').First();

                game.UpdateStats(cards, playtime);

                if (!game.HasDrops)
                {
                    game.StopIdling();
                }
            }

            Games.RemoveAll(x => x.IsFastIdling && !x.HasDrops);
        }
    }
}