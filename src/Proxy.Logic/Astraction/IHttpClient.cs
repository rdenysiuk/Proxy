using System.Net.Http;
using System.Threading.Tasks;

namespace Proxy.Logic.Astraction
{
    public interface IHttpClient
    {
        Task<HttpResponseMessage> SendAsync(HttpClientHandler clientHandler, HttpRequestMessage request);
    }
}