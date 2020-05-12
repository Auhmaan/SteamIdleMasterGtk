using IdleMaster.Properties;

namespace IdleMaster.ControlEntities
{
    public static class UserSettings
    {
        //Properties
        public static string CookieSessionId
        {
            get
            {
                return Settings.Default.CookieSessionId;
            }
            set
            {
                Settings.Default.CookieSessionId = value;
                Settings.Default.Save();
            }
        }

        public static string CookieLoginSecure
        {
            get
            {
                return Settings.Default.CookieLoginSecure;
            }
            set
            {
                Settings.Default.CookieLoginSecure = value;
                Settings.Default.Save();
            }
        }

        public static string CookieMachineAuth
        {
            get
            {
                return Settings.Default.CookieMachineAuth;
            }
            set
            {
                Settings.Default.CookieMachineAuth = value;
                Settings.Default.Save();
            }
        }

        public static string CookieRememberLogin
        {
            get
            {
                return Settings.Default.CookieRememberLogin;
            }
            set
            {
                Settings.Default.CookieRememberLogin = value;
                Settings.Default.Save();
            }
        }

        public static string CookieParental
        {
            get
            {
                return Settings.Default.CookieParental;
            }
            set
            {
                Settings.Default.CookieParental = value;
                Settings.Default.Save();
            }
        }

        public static string ProfileUrl
        {
            get
            {
                return Settings.Default.ProfileUrl;
            }
            set
            {
                Settings.Default.ProfileUrl = value;
            }
        }

        public static int GamesToIdle
        {
            get
            {
                return Settings.Default.GamesToIdle;
            }
            set
            {
                if (value >= 1 || value <= 30)
                {
                    Settings.Default.GamesToIdle = value;
                    Settings.Default.Save();
                }
            }
        }

        //Methods
        public static void ClearCookies()
        {
            Settings.Default.CookieSessionId = "";
            Settings.Default.CookieLoginSecure = "";
            Settings.Default.CookieParental = "";
            Settings.Default.CookieMachineAuth = "";
            Settings.Default.CookieRememberLogin = "";
            Settings.Default.Save();
        }
    }
}