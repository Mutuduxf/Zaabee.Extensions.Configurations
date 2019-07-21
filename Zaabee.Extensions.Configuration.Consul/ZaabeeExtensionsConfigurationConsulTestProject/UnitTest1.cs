using System;
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
                .AddConsul(c =>
                {
                    c.Address = new Uri("http://192.168.78.152:8500");
                    c.Datacenter = "dc1";
                    c.WaitTime = TimeSpan.FromSeconds(30);
                }, folder: "/dev")
                .AddConsul(c =>
                {
                    c.Address = new Uri("http://192.168.78.152:8500");
                    c.Datacenter = "dc1";
                    c.WaitTime = TimeSpan.FromSeconds(30);
                }, folder: "/prod", key: "redis");
            var config = configBuilder.Build();
            var a = config.GetSection("a").Get<string>();
            var b = config.GetSection("b").Get<string>();
            var redisConn = config.GetSection("redisConn").Get<string>();
        }
    }
}