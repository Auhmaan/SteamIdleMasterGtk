using IdleMaster.Properties;
using System;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace IdleMaster
{
    public partial class frmBrowser : Form
    {
        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool InternetSetOption(int hInternet, int dwOption, string lpBuffer, int dwBufferLength);

        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool InternetSetCookie(string lpszUrlName, string lpszCookieName, string lpszCookieData);

        [DllImport("wininet.dll", SetLastError = true)]
        public static extern bool InternetGetCookieEx(string url, string cookieName, StringBuilder cookieData, ref int size, int dwFlags, IntPtr lpReserved);

        public frmBrowser()
        {
            InitializeComponent();
        }

        private void frmBrowser_Load(object sender, EventArgs e)
        {
            //Remove any existing session state data
            InternetSetOption(0, 42, null, 0);

            //Delete Steam cookie data from the browser control
            InternetSetCookie("http://steamcommunity.com", "sessionid", ";expires=Mon, 01 Jan 0001 00:00:00 GMT");
            InternetSetCookie("http://steamcommunity.com", "steamLoginSecure", ";expires=Mon, 01 Jan 0001 00:00:00 GMT");
            InternetSetCookie("http://steamcommunity.com", "steamRememberLogin", ";expires=Mon, 01 Jan 0001 00:00:00 GMT");

            wbSteam.Navigate("https://steamcommunity.com/login", "_self", null, "User-Agent: Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko");
        }

        private void wbAuth_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            wbSteam.Document.Body.Style = "overflow:hidden";

            if (wbSteam.Url.AbsoluteUri == "https://steamcommunity.com/login" ||
                wbSteam.Url.AbsoluteUri == "https://steamcommunity.com/my/goto")
            {
                dynamic htmldoc = wbSteam.Document.DomDocument;
                dynamic idElement = htmldoc.GetElementById("global_header");

                if (!object.ReferenceEquals(idElement, DBNull.Value))
                {
                    idElement.parentNode.removeChild(idElement);
                }
            }

            if (wbSteam.Url.AbsoluteUri == "https://steamcommunity.com/my/goto")
            {
                Height = 370;
            }

            //Only gather the cookie information after reaching the profile page
            if (!wbSteam.Url.AbsoluteUri.StartsWith("https://steamcommunity.com/id/"))
            {
                return;
            }

            Height = 520;

            CookieContainer container = GetUriCookieContainer(wbSteam.Url);
            CookieCollection cookies = container.GetCookies(wbSteam.Url);

            foreach (Cookie cookie in cookies)
            {
                switch (cookie.Name)
                {
                    case "sessionid":
                        Settings.Default.CookieSessionId = cookie.Value;
                        break;
                    case "steamLoginSecure":
                        Settings.Default.CookieLoginSecure = cookie.Value;
                        Settings.Default.myProfileURL = SteamProfile.GetSteamUrl();
                        break;
                    case "steamMachineAuth":
                        Settings.Default.CookieMachineAuth = cookie.Value;
                        break;
                    case "steamRememberLogin":
                        Settings.Default.CookieRememberLogin = cookie.Value;
                        break;
                    case "steamparental":
                        Settings.Default.CookieParental = cookie.Value;
                        break;
                }
            }

            //Save all of the data to the program settings file
            Settings.Default.Save();

            Close();
        }

        private CookieContainer GetUriCookieContainer(Uri uri)
        {
            //First, create a null cookie container
            CookieContainer cookies = null;

            //Determine the size of the cookie
            int datasize = 8192 * 16;
            StringBuilder cookieData = new StringBuilder(datasize);

            //Call InternetGetCookieEx from wininet.dll
            if (!InternetGetCookieEx(uri.ToString(), null, cookieData, ref datasize, 0x2000, IntPtr.Zero))
            {
                if (datasize < 0)
                {
                    return null;
                }

                //Allocate stringbuilder large enough to hold the cookie
                cookieData = new StringBuilder(datasize);
                if (!InternetGetCookieEx(uri.ToString(), null, cookieData, ref datasize, 0x2000, IntPtr.Zero))
                {
                    return null;
                }
            }

            //If the cookie contains data, add it to the cookie container
            if (cookieData.Length > 0)
            {
                cookies = new CookieContainer();
                cookies.SetCookies(uri, cookieData.ToString().Replace(';', ','));
            }

            //Return the cookie container
            return cookies;
        }
    }
}