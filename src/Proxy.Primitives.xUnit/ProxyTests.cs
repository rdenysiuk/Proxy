using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Xunit;

namespace Proxy.Primitives.xUnit
{
    [ExcludeFromCodeCoverage]
    public class ProxyTests
    {
        private readonly Uri availProxyUri = new Uri("http://176.241.129.113:3128");
        private readonly Uri notAvailProxyUri = new Uri("http://176.241.129.113:3127");

        [Fact]
        public async Task Check_Available_Proxy()
        {
            var expectedProxyString = $"{availProxyUri};online;";
            var proxy = new ProxyState(availProxyUri);
            await proxy.PerformTest();

            Assert.Equal(ProxyType.HTTP, proxy.Type);
            Assert.Equal(expectedProxyString, proxy.ToString());
            Assert.Equal("online", proxy.Status);
        }

        [Fact]
        public async Task Check_NotAvailable_Proxy()
        {
            var proxy = new ProxyState(notAvailProxyUri);
            await proxy.PerformTest();

            Assert.Equal(ProxyType.HTTP, proxy.Type);
            Assert.Equal("offline", proxy.Status);
        }
    }
}
