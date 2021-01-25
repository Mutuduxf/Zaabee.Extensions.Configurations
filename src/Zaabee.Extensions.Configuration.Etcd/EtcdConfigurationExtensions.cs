using Microsoft.Extensions.Configuration;

namespace Zaabee.Extensions.Configuration.Etcd
{
    public static class EtcdConfigurationExtensions
    {
        public static IConfigurationBuilder AddEtcd(this IConfigurationBuilder builder, string connectionString,
            int port = 2379, string caCert = "", string clientCert = "", string clientKey = "",
            bool publicRootCa = false)
        {
            builder.Add(
                new EtcdConfigurationSource(connectionString, port, caCert, clientCert, clientKey, publicRootCa));
            return builder;
        }
    }
}