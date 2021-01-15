using System;
using System.Windows.Forms;

namespace Proxy.Checker.App
{
    public partial class mainForm : Form
    {
        public mainForm()
        {
            InitializeComponent();
        }

        private void addProxyButton_Click(object sender, EventArgs e)
        {
            var addProxyForm = new AddProxyForm();
            addProxyForm.ShowDialog();
        }
    }
}
