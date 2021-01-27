using Proxy.Checker.App.Logic;
using Proxy.Primitives.Abstraction;
using System;
using System.Windows.Forms;

namespace Proxy.Checker.App
{
    public partial class mainForm : Form
    {
        private readonly IProxyParse _proxyParse;
        private ProxyDataModel _proxyDataModel = new ProxyDataModel();

        public mainForm()
        {
            InitializeComponent();
            _proxyParse = new ProxyParse();
            ProxyDataGridView.DataSource = _proxyDataModel.Table;
        }
        
        #region Events
        private void addProxyButton_Click(object sender, EventArgs e)
        {
            var addProxyForm = new AddProxyForm(_proxyParse);
            if (addProxyForm.ShowDialog() == DialogResult.OK)
            {
                var proxies = addProxyForm.GetProxies();
                _proxyDataModel.AddRange(proxies);
                ConfigWidthGrid();
            }
        } 
        #endregion

        
        private void ConfigWidthGrid()
        {
            ProxyDataGridView.Columns[0].Width = 130;
            ProxyDataGridView.Columns[1].Width = 50;
            ProxyDataGridView.Columns[2].Width = 40;
            ProxyDataGridView.Columns[3].Width = 130;
        }
    }
}
