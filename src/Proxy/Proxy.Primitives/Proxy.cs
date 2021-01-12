using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

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
            Type = (ProxyType)Enum.Parse(typeof(ProxyType), address.Scheme, true);
        }
        #endregion

        public async Task PerformTest()
        {
            this._working = await PerformTestRequest(this);
        }

        public override string ToString()
        {
            return $"{this.Address};{this.Status};{this.Exception?.Message}";
        }

        #region Private methods

        private static async Task<bool> PerformTestRequest(Proxy proxy)
        {
            ServicePointManager.Expect100Continue = false;
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            HttpClientHandler clientHandler = new HttpClientHandler()
            {
                AllowAutoRedirect = true,
                AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip,
                Proxy = new WebProxy(proxy.Address)
            };
                        
            try
            {
                using var httpClient = new HttpClient(clientHandler);

                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
                httpClient.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate");
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, DynDnsLink);
                HttpResponseMessage response = await httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (HttpRequestException ex)
            {
                proxy.Exception = new ApplicationException($"Error during test request. Proxy: {proxy.Address}", ex.InnerException);
                return false;
            }
        }
        #endregion
    }
}