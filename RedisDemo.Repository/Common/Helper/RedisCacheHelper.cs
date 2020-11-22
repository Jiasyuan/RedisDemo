using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using RedisDemo.Repository.Common.Interface;
using StackExchange.Redis;

namespace RedisDemo.Repository.Common.Helper
{
    public class RedisCacheHelper : IRedisCacheHelper
    {
        private readonly IRedisConnectionMultiplexer _redisConnectionMultiplexer;
        private ConnectionMultiplexer _redisConnection;

        public RedisCacheHelper(IRedisConnectionMultiplexer redisConnectionMultiplexer)
        {
            this._redisConnectionMultiplexer = redisConnectionMultiplexer;
            this._redisConnection = _redisConnectionMultiplexer.GteRedisConnection();
        }


        public List<T> GetCache<T>(string key, Func<IEnumerable<T>> dataAccessProvider)
        {
            List<T> result = null;
            var cacheDb = _redisConnection.GetDatabase();
            var redisValue = cacheDb.StringGet(key);
            if (redisValue.IsNullOrEmpty)
            {
                result = dataAccessProvider()?.ToList();
                if (result != null)
                    ListSet(key, result);
            }
            else
            {
                result = JsonSerializer.Deserialize<List<T>>(redisValue);
            }
            return result;
        }

        public Tuple<string, List<T>> GetCacheTuple<T>(string key, Func<IEnumerable<T>> dataAccessProvider)
        {
            Tuple<string, List<T>> resultTuple = null;
            IEnumerable<T> result = null;
            var cacheDb = _redisConnection.GetDatabase();
            var redisValue = cacheDb.StringGet(key);
            if (redisValue.IsNullOrEmpty)
            {
                result = dataAccessProvider();
                resultTuple = Tuple.Create("Data From DB", result?.ToList());
                ListSet<T>(key, result?.ToList());
            }
            else
            {
                result = JsonSerializer.Deserialize<List<T>>(redisValue);
                resultTuple = Tuple.Create("Data From Redis", result?.ToList());
            }
            return resultTuple;
        }


        private void ListSet<T>(string key, List<T> value)
        {
            var cacheDb = _redisConnection.GetDatabase();
            cacheDb.StringSet(key, JsonSerializer.Serialize(value));
        }




        private void InsertBy_ListRightPush(string key, IEnumerable<string> input)
        {
            foreach (var entity in input)
            {
                _redisConnection.GetDatabase().ListRightPush(key, entity);
            }
        }

        private void InsertBy_CreateBatch(string key, IEnumerable<string> input)
        {
            var batch = _redisConnection.GetDatabase().CreateBatch();

            foreach (var entity in input)
            {
                _redisConnection.GetDatabase().ListRightPush(key, entity);
            }

            batch.Execute();
        }
    }

}
