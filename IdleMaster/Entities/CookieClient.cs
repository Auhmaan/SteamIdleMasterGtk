using IdleMaster.Properties;
using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace IdleMaster.Entities
{
    public class CookieClient : WebClient
    {
        //Properties
        public CookieContainer Cookie { get; set; }

        //Constructors
        public CookieClient()
        {
            CookieContainer cookieContainer = new CookieContainer();

            Uri uri = new Uri("http://steamcommunity.com");

            cookieContainer.Add(new Cookie("sessionid", Settings.Default.CookieSessionId) { Domain = uri.Host });
            cookieContainer.Add(new Cookie("steamLoginSecure", Settings.Default.CookieLoginSecure) { Domain = uri.Host });

            string cookieMachineAuth = "steamMachineAuth";

            if (Settings.Default.CookieLoginSecure != null && Settings.Default.CookieLoginSecure.Length > 17)
            {
                cookieMachineAuth = $"steamMachineAuth{Settings.Default.CookieLoginSecure.Substring(0, 17)}";
            }

            cookieContainer.Add(new Cookie(cookieMachineAuth, Settings.Default.CookieMachineAuth) { Domain = uri.Host });
            cookieContainer.Add(new Cookie("steamRememberLogin", Settings.Default.CookieRememberLogin) { Domain = uri.Host });
            cookieContainer.Add(new Cookie("steamparental", Settings.Default.CookieParental) { Domain = uri.Host });

            Cookie = cookieContainer;
            Encoding = Encoding.UTF8;
        }

        //Methods
        public static string GetHttp(string url)
        {
            string content = null;
            CookieClient client = new CookieClient();

            for (int count = 3; count > 0; count--)
            {
                content = client.DownloadString(url);

                if (!string.IsNullOrWhiteSpace(content))
                {
                    break;
                }
            }

            return content;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest webRequest = base.GetWebRequest(address);

            if (webRequest is HttpWebRequest)
            {
                (webRequest as HttpWebRequest).CookieContainer = Cookie;
            }

            return webRequest;
        }

        protected override WebResponse GetWebResponse(WebRequest webRequest, IAsyncResult result)
        {
            WebResponse webResponse = base.GetWebResponse(webRequest);

            CookieCollection cookieCollection = (webResponse as HttpWebResponse).Cookies;

            if (cookieCollection.Count > 0)
            {
                if (cookieCollection["steamLoginSecure"] != null && cookieCollection["steamLoginSecure"].Value == "deleted")
                {
                    Settings.Default.CookieSessionId = null;
                    Settings.Default.CookieLoginSecure = null;
                    Settings.Default.CookieParental = null;
                    Settings.Default.CookieMachineAuth = null;
                    Settings.Default.CookieRememberLogin = null;
                    Settings.Default.Save();
                }
            }

            return webResponse;
        }
    }
}