using Proxy.Primitives.Abstraction;
using System;
using System.Text.RegularExpressions;

namespace Proxy.Checker.App.Logic
{
    public class ProxyParse : IProxyParse
    {
        private string[] patterns = new string[]
        {
                @"(?<ip>\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3})\:(?<port>\d*)",
                //@"(?<scheme>http|socks5)"
        };

        public Uri Parse(string input)
        {
            string scheme = "", ip = "", port = "";
            foreach (var pattern in patterns)
            {
                foreach (Match match in Regex.Matches(input, pattern, RegexOptions.IgnoreCase))
                {
                    if (match.Groups["ip"].Success)
                    {
                        ip = match.Groups["ip"].Value;
                        port = match.Groups["port"].Value;
                    }
                    else if (match.Groups["scheme"].Success)
                        scheme = match.Value;
                }

            }
            if (string.IsNullOrEmpty(ip) && string.IsNullOrEmpty(port))
                return null;
            else
            {
                var uriBuilder = new UriBuilder();
                uriBuilder.Scheme = string.IsNullOrEmpty(scheme) ? "http" : scheme;
                uriBuilder.Host = ip;
                uriBuilder.Port = int.Parse(port);

                return uriBuilder.Uri;
            }
        }
    }
}
