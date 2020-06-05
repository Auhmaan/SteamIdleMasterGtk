using System.Drawing;
using System.Windows.Forms;

namespace IdleMaster.UserControls
{
    public partial class ucLoading : UserControl
    {
        public ucLoading()
        {
            InitializeComponent();
        }

        private void ucLoading_Load(object sender, System.EventArgs e)
        {
            Name = "ucLoading";
            Location = new Point(12, 27);
            BringToFront();
        }
    }
}