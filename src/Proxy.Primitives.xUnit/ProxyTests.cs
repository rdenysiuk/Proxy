using Moq;
using Proxy.Logic.Astraction;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Proxy.Primitives.xUnit
{
    [ExcludeFromCodeCoverage]
    public class ProxyTests
    {
        private readonly Uri availProxyUri = new Uri("http://1.1.1.1:3128");
        private readonly Uri notAvailProxyUri = new Uri("http://1.1.1.1:3127");

        private Mock<IHttpClient> moqHttpClient;

        public ProxyTests()
        {
            moqHttpClient = new Mock<IHttpClient>();
        }

        [Fact]
        public async Task Check_Available_Proxy()
        {
            var expectedProxyString = $"{availProxyUri};online;";

            moqHttpClient
                .Setup(a => a.SendAsync(It.IsAny<HttpClientHandler>(), It.IsAny<HttpRequestMessage>()))
                .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK });

            var proxy = new ProxyState(availProxyUri, moqHttpClient.Object);
            await proxy.PerformTest();

            Assert.Equal(ProxyType.HTTP, proxy.Type);
            Assert.Equal(expectedProxyString, proxy.ToString());
            Assert.Equal("online", proxy.Status);
        }

        [Fact]
        public async Task Check_NotAvailable_Proxy()
        {
            moqHttpClient
                .Setup(a => a.SendAsync(It.IsAny<HttpClientHandler>(), It.IsAny<HttpRequestMessage>()))
                .Throws(new HttpRequestException());

            var proxy = new ProxyState(notAvailProxyUri, moqHttpClient.Object);
            await proxy.PerformTest();

            Assert.Equal(ProxyType.HTTP, proxy.Type);
            Assert.Equal("offline", proxy.Status);
        }
    }
}
