using IdleMaster.Properties;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;

namespace IdleMaster
{
    public class Profile
    {
        //Methods
        public static string GetSteamId()
        {
            string steamid = WebUtility.UrlDecode(Settings.Default.CookieLoginSecure);
            return steamid?.Split('|').First();
        }

        public static string GetSteamUrl()
        {
            return $"https://steamcommunity.com/profiles/{GetSteamId()}";
        }

        public static string GetUsername()
        {
            string steamUrl = GetSteamUrl();
            string userName = $"User {GetSteamId()}";

            WebClient webClient = new WebClient
            {
                Encoding = Encoding.UTF8
            };

            string xml = webClient.DownloadString($"{steamUrl}/?xml=1");

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);

            XmlNode xmlNode = xmlDocument.SelectSingleNode("//steamID");

            if (xmlNode != null)
            {
                userName = WebUtility.HtmlDecode(xmlNode.InnerText);
            }

            return userName;
        }
    }
}