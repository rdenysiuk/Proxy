using Proxy.Primitives.Abstraction;
using System;
using System.Windows.Forms;

namespace Proxy.Checker.App
{
    public partial class AddProxyForm : Form
    {
        //private readonly IProxyParse _proxyParse;
        
        //public AddProxyForm(IProxyParse proxyParse)
        public AddProxyForm()
        {
            InitializeComponent();
            //_proxyParse = proxyParse;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void txtProxyList_Leave(object sender, EventArgs e)
        {
            var proxyList = txtProxyList.Text.Substring(0, 10);
            txtProxyList.Text = proxyList;
        }
    }
}
