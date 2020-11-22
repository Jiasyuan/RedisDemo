using System;
using System.Collections.Generic;
using System.Text;

namespace RedisDemo.Repository.Common.Interface
{
    public interface IRedisCacheHelper
    {
        List<T> GetCache<T>(string key, Func<IEnumerable<T>> dataAccessProvider);
        Tuple<string, List<T>> GetCacheTuple<T>(string key, Func<IEnumerable<T>> dataAccessProvider);
    }
}
