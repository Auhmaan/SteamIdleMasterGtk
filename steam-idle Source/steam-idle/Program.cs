using Steamworks;
using System;
using System.Windows.Forms;

namespace steam_idle
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            long.TryParse(args[0], out long appId);
            Environment.SetEnvironmentVariable("SteamAppId", AppId);

            if (!SteamAPI.Init())
            {
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain(appId));
        }
    }
}