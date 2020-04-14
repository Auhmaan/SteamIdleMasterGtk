using HtmlAgilityPack;
using IdleMaster.Properties;
using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace IdleMaster
{
    public class CookieClient : WebClient
    {
        //Properties
        public CookieContainer Cookie { get; set; }

        public Uri ResponseUri { get; set; }

        //Constructors
        public CookieClient()
        {
            Cookie = GenerateCookies();
            Encoding = Encoding.UTF8;
        }

        //Methods
        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest request = base.GetWebRequest(address);

            if (request is HttpWebRequest)
            {
                (request as HttpWebRequest).CookieContainer = Cookie;
            }

            return request;
        }

        protected override WebResponse GetWebResponse(WebRequest request, IAsyncResult result)
        {
            WebResponse baseResponse = base.GetWebResponse(request);

            CookieCollection cookies = (baseResponse as HttpWebResponse).Cookies;

            if (cookies.Count > 0)
            {
                if (cookies["steamLoginSecure"] != null && cookies["steamLoginSecure"].Value == "deleted")
                {
                    Settings.Default.CookieSessionId = null;
                    Settings.Default.CookieLoginSecure = null;
                    Settings.Default.CookieParental = null;
                    Settings.Default.CookieMachineAuth = null;
                    Settings.Default.CookieRememberLogin = null;
                    Settings.Default.Save();
                }
            }

            ResponseUri = baseResponse.ResponseUri;

            return baseResponse;
        }

        public static CookieContainer GenerateCookies()
        {
            CookieContainer cookies = new CookieContainer();
            Uri target = new Uri("http://steamcommunity.com");
            cookies.Add(new Cookie("sessionid", Settings.Default.CookieSessionId) { Domain = target.Host });
            cookies.Add(new Cookie("steamLoginSecure", Settings.Default.CookieLoginSecure) { Domain = target.Host });
            cookies.Add(new Cookie("steamparental", Settings.Default.CookieParental) { Domain = target.Host });
            cookies.Add(new Cookie("steamRememberLogin", Settings.Default.CookieRememberLogin) { Domain = target.Host });
            cookies.Add(new Cookie(GetSteamMachineAuthCookieName(), Settings.Default.CookieMachineAuth) { Domain = target.Host });

            return cookies;
        }

        public static string GetSteamMachineAuthCookieName()
        {
            if (Settings.Default.CookieLoginSecure != null && Settings.Default.CookieLoginSecure.Length > 17)
            {
                return string.Format("steamMachineAuth{0}", Settings.Default.CookieLoginSecure.Substring(0, 17));
            }

            return "steamMachineAuth";
        }

        public static async Task<string> GetHttpAsync(string url, int count = 3)
        {
            while (true)
            {
                CookieClient client = new CookieClient();
                string content = null;

                content = await client.DownloadStringTaskAsync(url);

                if (!string.IsNullOrWhiteSpace(content) || count == 0)
                {
                    return content;
                }

                count--;
            }
        }

        public static async Task<bool> IsLogined()
        {
            string response = await GetHttpAsync(Settings.Default.myProfileURL);

            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(response);

            return document.DocumentNode.SelectSingleNode("//a[@class=\"global_action_link\"]") == null;
        }
    }
}