using System;
using System.Windows.Forms;

namespace IdleMaster.Forms
{
	public partial class frmBlacklist : Form
	{
		public frmBlacklist()
		{
			InitializeComponent();
		}

		////////////////////////////////////////METHODS////////////////////////////////////////

		private void ApplySettings()
		{

		}

		////////////////////////////////////////CONTROLS////////////////////////////////////////

		private void lsbBlacklist_SelectedIndexChanged(object sender, EventArgs e)
		{
			btnDelete.Enabled = lsbBlacklist.SelectedIndices.Count > 0;
		}

		private void btnAdd_Click(object sender, EventArgs e)
		{

		}

		private void btnRemove_Click(object sender, EventArgs e)
		{

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