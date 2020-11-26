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
        private readonly IRedLockHelper _redLockHelper;
        private readonly ConnectionMultiplexer _redisConnection;

        public RedisCacheHelper(IRedisConnectionMultiplexer redisConnectionMultiplexer, IRedLockHelper redLockHelper)
        {
            this._redisConnectionMultiplexer = redisConnectionMultiplexer;
            this._redLockHelper = redLockHelper;
            this._redisConnection = _redisConnectionMultiplexer.GteRedisConnection();
        }


        public List<T> GetCache<T>(string key, Func<IEnumerable<T>> dataAccessProvider)
        {
            List<T> result;
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
            Tuple<string, List<T>> resultTuple;
            List<T> result;
            var cacheDb = _redisConnection.GetDatabase();
            var redisValue = cacheDb.StringGet(key);
            if (redisValue.IsNullOrEmpty)
            {
                result = dataAccessProvider()?.ToList();
                resultTuple = Tuple.Create("Data From DB", result);
                ListSet<T>(key, result);
            }
            else
            {
                result = JsonSerializer.Deserialize<List<T>>(redisValue);
                resultTuple = Tuple.Create("Data From Redis", result);
            }
            return resultTuple;
        }

        public async System.Threading.Tasks.Task<Tuple<string, List<T>>> GetCacheTupleRedLockAsync<T>(string key, Func<IEnumerable<T>> dataAccessProvider)
        {
            Tuple<string, List<T>> resultTuple = null;
            
            var resource = $"lockkey_{key}";//resource lock key
            var expiry = TimeSpan.FromSeconds(30);//lock object 失效時間
            using (var redLock = await _redLockHelper.GetRedLockFactory().CreateLockAsync(resource, expiry)) 
            {
                // 確定取得 lock 所有權
                if (redLock.IsAcquired)
                {
                    List<T> result;
                    var cacheDb = _redisConnection.GetDatabase();
                    var redisValue = cacheDb.StringGet(key);
                    if (redisValue.IsNullOrEmpty)
                    {
                        result = dataAccessProvider()?.ToList();
                        resultTuple = Tuple.Create("Data From DB", result);
                        ListSet<T>(key, result);
                    }
                    else
                    {
                        result = JsonSerializer.Deserialize<List<T>>(redisValue);
                        resultTuple = Tuple.Create("Data From Redis", result?.ToList());
                    }

                }
            }
            // the lock is automatically released at the end of the using block

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
