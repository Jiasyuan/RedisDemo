using System;
using Microsoft.Extensions.Configuration;
using RedisDemo.Repository.Common.Interface;
using StackExchange.Redis;

namespace RedisDemo.Repository.Common
{
    public class RedisConnectionMultiplexer : IRedisConnectionMultiplexer
    {
        private readonly IConfiguration _configuration;
        private readonly string _cacheConnectionString;
        private readonly Lazy<ConnectionMultiplexer> _lazyConnection;

        public RedisConnectionMultiplexer(IConfiguration configuration)
        {
            this._configuration = configuration;
            this._cacheConnectionString = _configuration.GetSection("CacheConnectionSetting")
                .Value;
            _lazyConnection =
                new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(_cacheConnectionString));
        }

        public ConnectionMultiplexer GteRedisConnection()
        {
            return _lazyConnection.Value;
        }

    }
}
