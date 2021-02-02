using Proxy.Primitives.Abstraction;
using Proxy.Primitives;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;

namespace Proxy.Checker.App
{
    public partial class AddProxyForm : Form
    {
        private readonly IProxyParse _proxyParse;

        public AddProxyForm(IProxyParse proxyParse)
        {
            InitializeComponent();
            _proxyParse = proxyParse;
        }

        public IEnumerable<ProxyState> GetProxies()
        {
            var proxies = new List<ProxyState>();
            using (var sr = new StringReader(txtProxyList.Text))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    var uri = _proxyParse.Parse(line);
                    if (uri != null)
                        proxies.Add(new ProxyState(uri));
                }
            }
            return proxies;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}