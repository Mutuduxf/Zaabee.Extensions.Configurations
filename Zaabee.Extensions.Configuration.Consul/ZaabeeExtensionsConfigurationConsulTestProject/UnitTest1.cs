using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Xunit;
using Zaabee.Extensions.Configuration.Consul;

namespace ZaabeeExtensionsConfigurationConsulTestProject
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var configBuilder = new ConfigurationBuilder()
                .AddConsulFolder(c =>
                {
                    c.Address = new Uri("http://192.168.5.229:8500");
                    c.Datacenter = "dc1";
                    c.WaitTime = TimeSpan.FromSeconds(30);
                }, folder: "/dev")
                .AddConsulFile(c =>
                {
                    c.Address = new Uri("http://192.168.5.229:8500");
                    c.Datacenter = "dc1";
                    c.WaitTime = TimeSpan.FromSeconds(30);
                }, key: "/dev/Redis");
            var config = configBuilder.Build();
            var hostCfg = config.GetSection("LogSystemHosts").Get<List<string>>();
            var mongoCfgUrl = config.GetSection("LogMongo").Get<string>();
            var mongoCfgDbName = config.GetSection("LogMongoDbName").Get<string>();
        }
    }
}