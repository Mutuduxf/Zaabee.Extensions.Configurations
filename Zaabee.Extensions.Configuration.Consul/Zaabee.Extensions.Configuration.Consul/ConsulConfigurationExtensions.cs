using System;
using System.Net.Http;
using Consul;
using Microsoft.Extensions.Configuration;

namespace Zaabee.Extensions.Configuration.Consul
{
    public static class ConsulConfigurationExtensions
    {
        public static IConfigurationBuilder AddConsulFolder(this IConfigurationBuilder builder,
            Action<ConsulClientConfiguration> configOverride = null, Action<HttpClient> clientOverride = null,
            Action<HttpClientHandler> handlerOverride = null, string folder = "/")
        {
            builder.Add(new ConsulConfigurationSource(configOverride, clientOverride, handlerOverride, folder));
            return builder;
        }

        public static IConfigurationBuilder AddConsulFile(this IConfigurationBuilder builder,
            Action<ConsulClientConfiguration> configOverride = null, Action<HttpClient> clientOverride = null,
            Action<HttpClientHandler> handlerOverride = null, string key = null)
        {
            builder.Add(new ConsulConfigurationSource(configOverride, clientOverride, handlerOverride, key: key));
            return builder;
        }
    }
}