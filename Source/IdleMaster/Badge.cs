using HtmlAgilityPack;
using IdleMaster.Properties;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IdleMaster
{
    public class Badge
    {
        public string AppId { get; set; }

        public string Name { get; set; }

        public int RemainingCard { get; set; }

        public double HoursPlayed { get; set; }

        public double AveragePrice { get; set; }

        private Process idleProcess;

        public bool InIdle
        {
            get
            {
                return idleProcess != null && !idleProcess.HasExited;
            }
        }

        public Badge(string appId, string name, string remaining, string hours)
        {
            AppId = appId;
            Name = name;
            UpdateStats(remaining, hours);
        }

        public Process Idle()
        {
            if (InIdle)
            {
                return idleProcess;
            }

            idleProcess = Process.Start(new ProcessStartInfo("steam-idle.exe", AppId) { WindowStyle = ProcessWindowStyle.Hidden });

            return idleProcess;
        }

        public void StopIdle()
        {
            if (InIdle)
            {
                idleProcess.Kill();
            }
        }

        public async Task<bool> CanCardDrops()
        {
            try
            {
                HtmlDocument document = new HtmlDocument();
                string response = await CookieClient.GetHttpAsync($"{Settings.Default.myProfileURL}/gamecards/{AppId})");

                //Response should be empty. User should be unauthorised.
                if (string.IsNullOrWhiteSpace(response))
                {
                    return false;
                }

                document.LoadHtml(response);

                HtmlNode hoursNode = document.DocumentNode.SelectSingleNode("//div[@class=\"badge_title_stats_playtime\"]");
                string hours = Regex.Match(hoursNode.InnerText, @"[0-9\.,]+").Value;

                HtmlNode cardNode = hoursNode.ParentNode.SelectSingleNode(".//span[@class=\"progress_info_bold\"]");
                string cards = cardNode == null ? string.Empty : Regex.Match(cardNode.InnerText, @"[0-9]+").Value;

                UpdateStats(cards, hours);
                return RemainingCard != 0;
            }
            catch (Exception ex)
            {
                Logger.Exception(ex, "Badge -> CanCardDrops, for id = " + AppId);
            }

            return false;
        }

        public void UpdateStats(string remaining, string hours)
        {
            RemainingCard = string.IsNullOrWhiteSpace(remaining) ? 0 : int.Parse(remaining);
            HoursPlayed = string.IsNullOrWhiteSpace(hours) ? 0 : double.Parse(hours, new NumberFormatInfo());
        }

        public override bool Equals(object obj)
        {
            Badge badge = obj as Badge;
            return badge != null && Equals(AppId, badge.AppId);
        }

        public override int GetHashCode()
        {
            return AppId.GetHashCode();
        }

        public override string ToString()
        {
            return string.IsNullOrWhiteSpace(Name) ? AppId : Name;
        }
    }
}