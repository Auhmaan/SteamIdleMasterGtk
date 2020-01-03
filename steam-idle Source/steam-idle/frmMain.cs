using System;
using System.Windows.Forms;

namespace steam_idle
{
    public partial class frmMain : Form
    {
        public frmMain(long appId)
        {
            InitializeComponent();
            ptbSteamApp.Load($"http://cdn.akamai.steamstatic.com/steam/apps/{appId}/header_292x136.jpg");
        }

        private void frmMain_Load(object sender, EventArgs e)
        {

        }
    }
}