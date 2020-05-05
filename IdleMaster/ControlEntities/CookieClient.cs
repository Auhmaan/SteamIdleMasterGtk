using System;
using System.Net;
using System.Text;

namespace IdleMaster.ControlEntities
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

            cookieContainer.Add(new Cookie("sessionid", UserSettings.CookieSessionId) { Domain = uri.Host });
            cookieContainer.Add(new Cookie("steamLoginSecure", UserSettings.CookieLoginSecure) { Domain = uri.Host });

            string cookieMachineAuth = "steamMachineAuth";

            if (UserSettings.CookieLoginSecure != null && UserSettings.CookieLoginSecure.Length > 17)
            {
                cookieMachineAuth = $"steamMachineAuth{UserSettings.CookieLoginSecure.Substring(0, 17)}";
            }

            cookieContainer.Add(new Cookie(cookieMachineAuth, UserSettings.CookieMachineAuth) { Domain = uri.Host });
            cookieContainer.Add(new Cookie("steamRememberLogin", UserSettings.CookieRememberLogin) { Domain = uri.Host });
            cookieContainer.Add(new Cookie("steamparental", UserSettings.CookieParental) { Domain = uri.Host });

            Cookie = cookieContainer;
            Encoding = Encoding.UTF8;
        }

        //Methods
        public static string GetHttp(string url)
        {
            string content = null;

            using (CookieClient client = new CookieClient())
            {
                for (int count = 3; count > 0; count--)
                {
                    content = client.DownloadString(url);

                    if (!string.IsNullOrWhiteSpace(content))
                    {
                        break;
                    }
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
                    UserSettings.ClearCookies();
                }
            }

            return webResponse;
        }
    }
}