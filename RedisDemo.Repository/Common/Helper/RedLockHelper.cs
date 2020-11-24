using System;
using System.Collections.Generic;
using RedisDemo.Repository.Common.Interface;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using StackExchange.Redis;

namespace RedisDemo.Repository.Common.Helper
{
    public class RedLockHelper : IRedLockHelper, IDisposable
    {
        private readonly IRedisConnectionMultiplexer _redisConnectionMultiplexer;
        private ConnectionMultiplexer _redisConnection;
        private bool _disposedValue = false;
        #region IDisposable Support
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    return;
                }

                _disposedValue = true;
            }
        }

        ~RedLockHelper() => Dispose(false);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
        public RedLockHelper(IRedisConnectionMultiplexer redisConnectionMultiplexer)
        {
            this._redisConnectionMultiplexer = redisConnectionMultiplexer;
            this._redisConnection = _redisConnectionMultiplexer.GteRedisConnection();
        }

        public RedLockFactory GetRedLockFactory()
        {
            var multiplexers = new List<RedLockMultiplexer>
            {
                _redisConnection
            };
            return RedLockFactory.Create(multiplexers);
        }
    }
}
