using IdleMaster.Properties;

namespace IdleMaster.ControlEntities
{
    public class UserCookies
    {
        //Properties
        public string SessionId
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

        public string LoginSecure
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

        public string MachineAuth
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

        public string RememberLogin
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

        public string Parental
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

        //Methods
        public void Clear()
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