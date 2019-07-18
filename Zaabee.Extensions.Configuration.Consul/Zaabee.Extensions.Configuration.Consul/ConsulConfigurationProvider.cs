using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Consul;
using Microsoft.Extensions.Configuration;

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
            if (!string.IsNullOrEmpty(_folder)) LoadDir();
            if (!string.IsNullOrEmpty(_key)) LoadFile();
        }

        private void LoadDir()
        {
            var result = _consulClient.KV.List(_folder).Result;
            if (result == null || result.StatusCode != HttpStatusCode.OK || result.Response == null) return;
            foreach (var item in result.Response.Where(p => p.Value != null))
            {
                var json = Encoding.UTF8.GetString(item.Value);
                using (var memoryStream = new MemoryStream())
                using (var streamWriter = new StreamWriter(memoryStream))
                {
                    streamWriter.Write(json);
                    streamWriter.Flush();
                    memoryStream.Position = 0;
                    SetKvPair(memoryStream);
                }
            }
        }

        private void LoadFile()
        {
            var result = _consulClient.KV.Get(_key).Result;
            if (result == null || result.StatusCode != HttpStatusCode.OK || result.Response?.Value == null) return;
            var json = Encoding.UTF8.GetString(result.Response.Value);
            using (var memoryStream = new MemoryStream())
            using (var streamWriter = new StreamWriter(memoryStream))
            {
                streamWriter.Write(json);
                streamWriter.Flush();
                memoryStream.Position = 0;
                SetKvPair(memoryStream);
            }
        }

        private void SetKvPair(Stream stream)
        {
            var parser = new JsonConfigurationObjectParser();
            foreach (var keyValuePair in parser.Parse(stream))
                Set(keyValuePair.Key, keyValuePair.Value);
        }
    }
}