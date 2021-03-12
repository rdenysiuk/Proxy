using Proxy.Logic.Astraction;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Proxy.Logic
{
    public class ProxyHttpClient : IHttpClient
    {
        public async Task<HttpResponseMessage> SendAsync(HttpClientHandler clientHandler, HttpRequestMessage request)
        {
            using (var httpClient = new HttpClient(clientHandler))
            {

                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
                httpClient.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate");
                return await httpClient.SendAsync(request);
            }
        }
    }
}
