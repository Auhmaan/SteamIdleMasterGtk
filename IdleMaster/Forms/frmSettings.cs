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
            rdbSingleIdle.Checked = UserSettings.GamesToIdle == 1;
            rdbMultiIdle.Checked = UserSettings.GamesToIdle > 1;
            nudGamesToIdle.Value = UserSettings.GamesToIdle > 1 ? UserSettings.GamesToIdle : nudGamesToIdle.Minimum;
        }

        ////////////////////////////////////////METHODS////////////////////////////////////////

        private void ApplySettings()
        {
            if (rdbSingleIdle.Checked)
            {
                UserSettings.GamesToIdle = 1;
            }

            if (rdbMultiIdle.Checked)
            {
                int.TryParse(nudGamesToIdle.Value.ToString(), out int gamesToIdle);
                UserSettings.GamesToIdle = gamesToIdle;
            }
        }

        ////////////////////////////////////////CONTROLS////////////////////////////////////////

        private void rdbMultiIdle_CheckedChanged(object sender, EventArgs e)
        {
            if (!rdbMultiIdle.Checked)
            {
                nudGamesToIdle.Value = nudGamesToIdle.Minimum;
            }

            nudGamesToIdle.Enabled = rdbMultiIdle.Checked;
            lblSimultaneous1.Enabled = rdbMultiIdle.Checked;
            lblSimultaneous2.Enabled = rdbMultiIdle.Checked;
        }

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