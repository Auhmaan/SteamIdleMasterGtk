using IdleMaster.ControlEntities;
using System;
using System.Windows.Forms;

namespace IdleMaster.Forms
{
    public partial class frmSettings : Form
    {
        public frmSettings()
        {
            InitializeComponent();
        }

        private void frmSettings_Load(object sender, EventArgs e)
        {
            nudGamesToIdle.Value = UserSettings.GamesToIdle;
        }

        ////////////////////////////////////////METHODS////////////////////////////////////////

        private void ApplySettings()
        {
            int.TryParse(nudGamesToIdle.Value.ToString(), out int gamesToIdle);
            UserSettings.GamesToIdle = gamesToIdle;
        }

        ////////////////////////////////////////CONTROLS////////////////////////////////////////

        private void btnOk_Click(object sender, EventArgs e)
        {
            ApplySettings();
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            ApplySettings();
        }
    }
}