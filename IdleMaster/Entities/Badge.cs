using HtmlAgilityPack;
using IdleMaster.Properties;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace IdleMaster
{
    public class Badge
    {
        //Fields
        private Process idleProcess;

        //Properties
        public string AppId { get; set; }

        public string Name { get; set; }

        public int RemainingCards { get; set; }

        public double HoursPlayed { get; set; }

        public double AveragePrice { get; set; }

        public bool IsIdling
        {
            get
            {
                return idleProcess != null;
            }
        }

        //Constructors
        public Badge(string appId, string name, string remaining, string hours)
        {
            AppId = appId;
            Name = name;
            UpdateStats(remaining, hours);
        }

        //Methods
        public Process StartIdle()
        {
            if (IsIdling)
            {
                return idleProcess;
            }

            idleProcess = Process.Start(new ProcessStartInfo("steam-idle.exe", AppId)
            {
                WindowStyle = ProcessWindowStyle.Hidden
            });

            return idleProcess;
        }

        public void StopIdle()
        {
            if (!IsIdling)
            {
                return;
            }

            idleProcess.Kill();
        }

        public void UpdateStats(string remaining, string hours)
        {
            int.TryParse(remaining, out int remainingCards);
            double.TryParse(hours, NumberStyles.Any, new NumberFormatInfo(), out double hoursPlayed);

            RemainingCards = remainingCards;
            HoursPlayed = hoursPlayed;
        }

        public async Task<bool> CanDropCards()
        {
            HtmlDocument document = new HtmlDocument();
            string response = await CookieClient.GetHttpAsync($"{Settings.Default.myProfileURL}/gamecards/{AppId}");

            document.LoadHtml(response);

            HtmlNode cardsNode = document.DocumentNode.SelectSingleNode(".//span[@class='progress_info_bold']");
            string cards = cardsNode?.InnerText.Split(' ').First();

            HtmlNode playtimeNode = document.DocumentNode.SelectSingleNode(".//div[@class='badge_title_stats_playtime']");
            string playtime = WebUtility.HtmlDecode(playtimeNode?.InnerText).Trim().Split(' ').First();

            UpdateStats(cards, playtime);

            return RemainingCards != 0;
        }
    }
}