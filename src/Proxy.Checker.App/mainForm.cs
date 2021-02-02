using Proxy.Checker.App.Logic;
using Proxy.Primitives.Abstraction;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proxy.Checker.App
{
    public partial class mainForm : Form
    {
        private readonly IProxyParse _proxyParse;
        private ProxyDataModel _proxyDataModel = new ProxyDataModel();
        private progressForm _progressForm = null;

        public mainForm()
        {
            InitializeComponent();
            _proxyParse = new ProxyParse();
            ProxyDataGridView.DataSource = _proxyDataModel.Table;
        }

        #region Events
        private void mainForm_Shown(object sender, EventArgs e)
        {
            TestButtonEnable();
            exportButton.Enabled = _proxyDataModel.Proxies.Count != 0;
        }
        private void addProxyButton_Click(object sender, EventArgs e)
        {
            var addProxyForm = new AddProxyForm(_proxyParse);
            if (addProxyForm.ShowDialog() == DialogResult.OK)
            {
                var proxies = addProxyForm.GetProxies();
                if (proxies.Count() > 0)
                {
                    _proxyDataModel.AddRange(proxies);
                    ConfigWidthGrid();
                    Log($"Added {proxies.Count()} proxy. Total {_proxyDataModel.Proxies.Count}");
                    TestButtonEnable();
                }
            }
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            _proxyDataModel.Clear();
        }

        private void testButton_Click(object sender, EventArgs e)
        {
            _progressForm = new progressForm();
            
            TestProxies();

            _progressForm.ShowDialog();
        }

        #endregion

        private void TestProxies()
        {
            var totalProxies = _proxyDataModel.Proxies.Count;

            Task.Run(() => UpdateProgressBar());
        }

        private void UpdateProgressBar()
        {
            //_progressForm.SetProgress(progressValue);
            _progressForm.SetProgress(25);
            Thread.Sleep(1000);
            _progressForm.SetProgress(50);
            Thread.Sleep(1000);
            _progressForm.SetProgress(75);
            Thread.Sleep(1000);
            _progressForm.SetProgress(100);
            Thread.Sleep(1000);
            _progressForm.Close();
        }

        /// <summary>
        /// Log information
        /// </summary>
        /// <param name="message">Logging message</param>
        /// <param name="isNewLine">Need to start from new line</param>
        private void Log(string message, bool isNewLine = false)
        {
            if (isNewLine) txtLog.AppendText(Environment.NewLine);
            txtLog.AppendText(message);
        }

        private void ConfigWidthGrid()
        {
            ProxyDataGridView.Columns[0].Width = 130;
            ProxyDataGridView.Columns[1].Width = 50;
            ProxyDataGridView.Columns[2].Width = 40;
            ProxyDataGridView.Columns[3].Width = 130;
        }

        private void TestButtonEnable()
        {
            testButton.Enabled = _proxyDataModel.Proxies.Count != 0;
        }
    }
}
