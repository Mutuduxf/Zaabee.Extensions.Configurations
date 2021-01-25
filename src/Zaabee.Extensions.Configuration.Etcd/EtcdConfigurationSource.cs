using dotnet_etcd;
using Microsoft.Extensions.Configuration;

namespace Zaabee.Extensions.Configuration.Etcd
{
    public class EtcdConfigurationSource : IConfigurationSource
    {
        private readonly EtcdClient _etcdClient;

        public EtcdConfigurationSource(string connectionString, int port = 2379, string caCert = "",
            string clientCert = "", string clientKey = "", bool publicRootCa = false)
        {
            _etcdClient = new EtcdClient(connectionString, port, caCert, clientCert, clientKey, publicRootCa);
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder) =>
            new EtcdConfigurationProvider(_etcdClient);
    }
}