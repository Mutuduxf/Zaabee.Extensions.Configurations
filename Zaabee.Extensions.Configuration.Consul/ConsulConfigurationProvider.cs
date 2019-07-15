using System.Net;
using System.Text;
using Consul;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Zaabee.Extensions.Configuration.Consul
{
    public class ConsulConfigurationProvider : ConfigurationProvider
    {
        private readonly ConsulClient _consulClient;
        private readonly string _folder;
        private readonly string _key;

        public ConsulConfigurationProvider(ConsulClient consulClient, string folder = "/", string key = null)
        {
            _folder = folder?.Trim();
            _key = key?.Trim();
            _consulClient = consulClient;
        }

        public override void Load()
        {
            if (!string.IsNullOrEmpty(_folder))
            {
                var result = _consulClient.KV.List(_folder).Result;
                if (result == null || result.StatusCode != HttpStatusCode.OK || result.Response == null) return;
                foreach (var item in result.Response)
                    SetKvPair(item);
            }

            if (!string.IsNullOrEmpty(_key))
            {
                var result = _consulClient.KV.Get(_key).Result;
                if (result == null || result.StatusCode != HttpStatusCode.OK || result.Response == null) return;
                SetKvPair(result.Response);
            }
        }

        private void SetKvPair(KVPair kvPair)
        {
            var value = Encoding.UTF8.GetString(kvPair.Value);
            var kvLst = JsonConvert.DeserializeObject<JObject>(value);
            foreach (var keyValuePair in kvLst)
            {
                switch (keyValuePair.Value)
                {
                    case JObject jObject:
                        foreach (var valuePair in jObject)
                            Set($"{keyValuePair.Key}:{valuePair.Key}", valuePair.Value.ToString());
                        break;
                    case JValue jValue:
                        Set(keyValuePair.Key, jValue.ToString());
                        break;
                }
            }
        }
    }
}