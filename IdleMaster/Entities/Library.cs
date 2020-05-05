using HtmlAgilityPack;
using IdleMaster.ControlEntities;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace IdleMaster.Entities
{
    public class Library
    {
        //Properties
        public List<Game> Games { get; private set; }

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
                return Games != null && Games.Any(x => x.IsIdling);
            }
        }

        public bool IsPaused
        {
            get
            {
                return Games != null && Games.Any(x => x.IsPaused);
            }
        }

        //Constructors
        public Library()
        {
            Games = new List<Game>();
        }

        //Methods
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

        public void PauseIdling()
        {
            if (!IsIdling)
            {
                return;
            }

            foreach (Game game in Games.Where(x => x.IsIdling))
            {
                game.PauseIdling();
            }
        }

        public void ResumeIdling()
        {
            if (!IsIdling)
            {
                return;
            }

            foreach (Game game in Games.Where(x => x.IsIdling))
            {
                game.ResumeIdling();
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

            foreach (Game game in Games.Where(x => x.CurrentIdleStyle == IdleStyle.Normal))
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

            Games.RemoveAll(x => x.CurrentIdleStyle == IdleStyle.Normal && !x.HasDrops);
        }

        public void StartFastIdling()
        {
            if (!HasGames)
            {
                return;
            }

            foreach (Game game in Games.Where(x => x.CurrentIdleStyle == IdleStyle.Fast))
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

            foreach (Game game in Games.Where(x => x.CurrentIdleStyle == IdleStyle.Fast).ToList())
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

            foreach (Game game in Games.Where(x => x.CurrentIdleStyle == IdleStyle.Fast))
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

            Games.RemoveAll(x => x.CurrentIdleStyle == IdleStyle.Fast && !x.HasDrops);
        }
    }
}