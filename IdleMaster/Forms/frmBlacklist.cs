using IdleMaster.ControlEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace IdleMaster.Forms
{
    public partial class frmBlacklist : Form
    {
        public frmBlacklist()
        {
            InitializeComponent();
        }

        private void frmBlacklist_Load(object sender, EventArgs e)
        {
            string[] gamesBlacklist = UserSettings.GamesBlacklist?.ToArray();

            if (gamesBlacklist == null)
            {
                return;
            }

            lsbBlacklist.Items.AddRange(gamesBlacklist);
        }

        ////////////////////////////////////////METHODS////////////////////////////////////////

        private void ApplySettings()
        {
            UserSettings.GamesBlacklist = lsbBlacklist.Items.Cast<string>().ToList();
        }

        ////////////////////////////////////////CONTROLS////////////////////////////////////////

        private void lsbBlacklist_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnDelete.Enabled = lsbBlacklist.SelectedIndices.Count > 0;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtAppId.Text))
            {
                return;
            }

            int.TryParse(txtAppId.Text, out int appId);

            if (appId == 0)
            {
                txtAppId.Text = null;
                return;
            }

            if (lsbBlacklist.Items.Contains(appId))
            {
                txtAppId.Text = null;
                return;
            }

            lsbBlacklist.Items.Add(appId);
            txtAppId.Text = null;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            List<int> selectedIndices = lsbBlacklist.SelectedIndices.Cast<int>().Reverse().ToList();

            foreach (int index in selectedIndices)
            {
                lsbBlacklist.Items.RemoveAt(index);
            }
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
    }
}