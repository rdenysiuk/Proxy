using Proxy.Logic;
using Proxy.Logic.Astraction;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Proxy.Primitives
{
    public class ProxyState
    {
        #region Properties
        public Uri Address { get; private set; }
        public ProxyType Type { get; private set; }

        public string Status
        {
            get
            {
                switch (_working)
                {
                    case true:
                        return "online";
                    case false:
                        return "offline";
                    default:
                        return "unknown";
                };
            }
        }

        public Exception Exception { get; private set; }
        
        [Obsolete]
        public int RequestTime { get; private set; }

        #endregion

        private bool? _working;
        const string DynDnsLink = "http://checkip.dyndns.org/";

        private readonly IHttpClient _httpClient;

        #region .ctor
        public ProxyState(Uri address, IHttpClient httpClient)
        {
            Initial(address);
            _httpClient = httpClient;
        }
        public ProxyState(Uri address)
            : this(address, new ProxyHttpClient())
        {
            Initial(address);
        }

        private void Initial(Uri address)
        {
            Address = address ?? throw new ArgumentNullException(nameof(address));
            Type = (ProxyType)Enum.Parse(typeof(ProxyType), address.Scheme, true);
        }
        #endregion

        /// <summary>
        ///     Perform the check proxy availability  
        /// </summary>
        /// <returns></returns>
        public async Task PerformTest()
        {
            this._working = await PerformTestRequest(this, _httpClient);
        }

        public override string ToString()
        {
            return $"{this.Address};{this.Status};{this.Exception?.Message}";
        }

        #region Private methods
        private static async Task<bool> PerformTestRequest(ProxyState proxy, IHttpClient httpClient)
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
                var requestMessage = new HttpRequestMessage(HttpMethod.Get, DynDnsLink);
                var responseMessage = await httpClient.SendAsync(clientHandler, requestMessage);
                responseMessage.EnsureSuccessStatusCode();
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