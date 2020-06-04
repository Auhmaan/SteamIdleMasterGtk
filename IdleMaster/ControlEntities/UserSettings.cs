using IdleMaster.Properties;

namespace IdleMaster.ControlEntities
{
    public static class UserSettings
    {
        //Properties
        public static UserCookies Cookies { get; } = new UserCookies();

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

        public static bool FastIdleEnabled
        {
            get
            {
                return Settings.Default.FastIdleEnabled;
            }
            set
            {
                Settings.Default.FastIdleEnabled = value;
                Settings.Default.Save();
            }
        }
    }
}