using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace Proxy.Primitives.xUnit
{
    [ExcludeFromCodeCoverage]
    public class ProxyTests
    {
        private readonly Uri availProxyUri = new Uri("http://212.8.40.78:3131");
        private readonly Uri notAvailProxyUri = new Uri("http://109.87.40.23:60083");

        [Fact]
        public void Check_Available_Proxy()
        {
            var expectedProxyString = $"{availProxyUri};online;";
            var proxy = new Proxy(availProxyUri);
            proxy.PerformTest();

            Assert.Equal(ProxyType.HTTP, proxy.Type);
            Assert.Equal(expectedProxyString, proxy.ToString());
            Assert.Equal("online", proxy.Status);
        }

        [Fact]
        public void Check_NotAvailable_Proxy()
        {
            var proxy = new Proxy(notAvailProxyUri);
            proxy.PerformTest();

            Assert.Equal(ProxyType.HTTP, proxy.Type);
            Assert.Equal("offline", proxy.Status);
        }
    }
}
