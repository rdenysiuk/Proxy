using System;
using System.Net;

namespace Proxy.Primitives
{
    public class Proxy
    {
        #region Properties

        public Uri Address { get; private set; }
        public ProxyType Type { get; private set; }

        public string Status
        {
            get
            {
                return this._working switch
                {
                    true => "online",
                    false => "offline",
                    _ => "unknown"
                };
            }
        }

        public Exception Exception { get; private set; }
        public int RequestTime { get; private set; }

        #endregion

        private bool? _working;
        const string DynDnsLink = "http://checkip.dyndns.org/";

        #region .ctor
        public Proxy(Uri address)
        {
            Address = address;
            Type = (ProxyType) Enum.Parse(typeof(ProxyType), address.Scheme, true);
        }
        #endregion

        public void PerformTest()
        {
            this._working = Proxy.TestProxy(this);
        }

        public override string ToString()
        {
            return $"{this.Address};{this.Status};{this.Exception?.Message}";
        }

        #region Private methods

        private static bool TestProxy(Proxy proxy)
        {
            bool result = false;
            if (proxy.Exception == null)
                result = PerformTestRequest(proxy);

            return result;
        }

        private static bool PerformTestRequest(Proxy proxy)
        {
            bool result = false;
            var request = (HttpWebRequest) WebRequest.Create(DynDnsLink);
            request.Proxy = new WebProxy(proxy.Address);
            request.UserAgent =
                "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/39.0.2171.95 Safari/537.36";
            request.Timeout = 15000;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls |
                                                   SecurityProtocolType.Tls11 |
                                                   SecurityProtocolType.Tls12;

            request.Method = "GET";
            var now = DateTime.Now;
            try
            {
                var response = request.GetResponse();
                var milisec = (DateTime.Now - now).TotalMilliseconds;
                proxy.RequestTime = Convert.ToInt32(milisec);
                result = true;
            }
            catch (Exception ex)
            {
                proxy.RequestTime = 0;
                proxy.Exception = new ApplicationException(proxy.Address.ToString(), ex);
            }

            return result;
        }
        #endregion
    }
}