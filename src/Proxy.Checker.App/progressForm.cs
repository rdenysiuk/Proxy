using System.Windows.Forms;

namespace Proxy.Checker.App
{
    public partial class progressForm : Form
    {
        public progressForm()
        {
            InitializeComponent();
        }

        public void SetProgress(int progress)
        {
            progressBar.Value = progress;
        }
    }
}
