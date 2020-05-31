using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace IdleMaster.ControlEntities
{
    public class CookieClient : WebClient
    {
        //Properties
        public CookieContainer CookieContainer { get; set; }

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

            CookieContainer = cookieContainer;
            Encoding = Encoding.UTF8;
        }

        //Methods
        public static async Task<string> GetHttp(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                return null;
            }

            string content;

            using (CookieClient client = new CookieClient())
            {
                content = await client.DownloadStringTaskAsync(url);
            }

            return content;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest webRequest = base.GetWebRequest(address);

            if (webRequest is HttpWebRequest)
            {
                (webRequest as HttpWebRequest).CookieContainer = CookieContainer;
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