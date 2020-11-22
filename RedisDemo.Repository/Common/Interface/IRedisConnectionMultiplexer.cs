using System;
using System.Collections.Generic;
using System.Text;
using StackExchange.Redis;

namespace RedisDemo.Repository.Common.Interface
{
    public interface IRedisConnectionMultiplexer
    {
        ConnectionMultiplexer GteRedisConnection();
    }
}
