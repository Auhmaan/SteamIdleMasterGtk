using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace IdleMaster.ControlEntities
{
	public class UserWebClient : WebClient
	{
		//Fields
		private readonly CookieContainer cookieContainer;

		//Constructors
		public UserWebClient()
		{
			CookieContainer cookieContainer = new CookieContainer();

			Uri uri = new Uri("http://steamcommunity.com");

			cookieContainer.Add(new Cookie("sessionid", UserSettings.Cookies.SessionId) { Domain = uri.Host });
			cookieContainer.Add(new Cookie("steamLoginSecure", UserSettings.Cookies.LoginSecure) { Domain = uri.Host });

			string cookieMachineAuth = "steamMachineAuth";

			if (UserSettings.Cookies.LoginSecure != null && UserSettings.Cookies.LoginSecure.Length > 17)
			{
				cookieMachineAuth = $"steamMachineAuth{UserSettings.Cookies.LoginSecure.Substring(0, 17)}";
			}

			cookieContainer.Add(new Cookie(cookieMachineAuth, UserSettings.Cookies.MachineAuth) { Domain = uri.Host });
			cookieContainer.Add(new Cookie("steamRememberLogin", UserSettings.Cookies.RememberLogin) { Domain = uri.Host });
			cookieContainer.Add(new Cookie("steamparental", UserSettings.Cookies.Parental) { Domain = uri.Host });

			this.cookieContainer = cookieContainer;
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

			using (UserWebClient client = new UserWebClient())
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
				(webRequest as HttpWebRequest).CookieContainer = cookieContainer;
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
					UserSettings.Cookies.Clear();
				}
			}

			return webResponse;
		}
	}
}