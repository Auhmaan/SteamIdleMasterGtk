using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Management;
using System.Security.Principal;
using System.Windows.Forms;

namespace IdleMaster
{
    static class Program
    {
        ///<summary>
        ///The main entry point for the application.
        ///</summary>
        [STAThread]
        static void Main()
        {
            KillPendingProcesses();

            //Set the browser emulation version for embedded browser control
            try
            {
                RegistryKey ie_root = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION");
                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION", true);
                string programName = Path.GetFileName(Environment.GetCommandLineArgs()[0]);
                key.SetValue(programName, 10001, RegistryValueKind.DWord);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());

            KillPendingProcesses();
        }

        static void KillPendingProcesses()
        {
            string windowsUser = WindowsIdentity.GetCurrent().Name;

            foreach (Process process in Process.GetProcessesByName("steam-idle"))
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher($"SELECT * FROM Win32_Process WHERE PROCESSID = {process.Id}");
                ManagementObjectCollection processList = searcher.Get();

                foreach (ManagementObject managementObject in processList)
                {
                    string[] argList = new string[] { null, null };

                    int.TryParse(managementObject.InvokeMethod("GetOwner", argList).ToString(), out int returnValue);

                    if (returnValue != 0)
                    {
                        continue;
                    }

                    if ($"{argList[1]}\\{argList[0]}" != windowsUser)
                    {
                        continue;
                    }

                    process.Kill();
                }
            }
        }
    }
}