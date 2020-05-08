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

        public bool IsIdling { get; private set; }

        public bool IsPaused { get; private set; }

        public int FirstIdlingIndex { get; private set; }

        //Constructors
        public Library()
        {
            Games = new List<Game>();
        }

        //Methods
        public void StartIdling()
        {
            if (!HasGames || IsIdling)
            {
                return;
            }

            FirstIdlingIndex = 0;

            foreach (Game game in Games.Skip(FirstIdlingIndex).Take(UserSettings.GamesToIdle))
            {
                game.StartIdling();
            }

            IsIdling = true;
        }

        public void PauseIdling()
        {
            if (IsPaused)
            {
                return;
            }

            foreach (Game game in Games.Where(x => x.IsIdling))
            {
                game.StopIdling();
            }

            IsPaused = true;
        }

        public void ResumeIdling()
        {
            if (!IsPaused)
            {
                return;
            }

            foreach (Game game in Games.Skip(FirstIdlingIndex).Take(UserSettings.GamesToIdle))
            {
                game.StartIdling();
            }

            IsPaused = false;
        }

        public void SkipIdling()
        {
            if (!IsIdling)
            {
                return;
            }

            Games[FirstIdlingIndex].StopIdling();
            Games[FirstIdlingIndex + UserSettings.GamesToIdle].StartIdling();

            FirstIdlingIndex++;
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

            IsIdling = false;
            IsPaused = false;
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
            foreach (Game game in Games.Where(x => x.CurrentIdleStyle == IdleStyle.Fast))
            {
                game.StartIdling();
            }
        }

        public void StopFastIdling()
        {
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