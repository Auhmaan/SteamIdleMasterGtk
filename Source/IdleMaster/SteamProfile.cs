using IdleMaster.Properties;
using System;
using System.Net;
using System.Text;
using System.Xml;

namespace IdleMaster
{
    internal class SteamProfile
    {
        internal static string GetSteamId()
        {
            string steamid = WebUtility.UrlDecode(Settings.Default.steamLogin);
            int index = steamid.IndexOfAny(new[] { '|' }, 0);
            return index != -1 ? steamid.Remove(index) : steamid;
        }

        internal static string GetSteamUrl()
        {
            return "https://steamcommunity.com/profiles/" + GetSteamId();
        }

        internal static string GetSignedAs()
        {
            string steamUrl = GetSteamUrl();
            string userName = $"User {GetSteamId()}";

            try
            {
                string xmlRaw = new WebClient() { Encoding = Encoding.UTF8 }.DownloadString($"{steamUrl}/?xml=1");

                XmlDocument xml = new XmlDocument();
                xml.LoadXml(xmlRaw);

                XmlNode xmlNode = xml.SelectSingleNode("//steamID");

                if (xmlNode != null)
                {
                    userName = WebUtility.HtmlDecode(xmlNode.InnerText);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Logger.Exception(ex, $"frmMain -> GetSignedAs, for steamUrl = {steamUrl}");
            }

            return $"{localization.strings.signed_in_as} {userName}";
        }
    }
}