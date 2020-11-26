using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using RedisRedisDemo.Contract;
using RedisRedisDemo.Test.Library;
using System.Linq;

namespace RedisRedisDemo.Test.UseSingleServer
{
    class Program
    {
        static void Main(string[] args)
        {
            string host = "";
            string methoud = "ECommerce";
            ConcurrentStack<ECommerceResponse> resultConcurrentStack = new ConcurrentStack<ECommerceResponse>();
            Parallel.For(1, 10, (i, state) =>
            {
                resultConcurrentStack.Push(ApiHelper.Api<ECommerceResponse>(host, EnumContentType.json, EnumApiMethodType.Get, methoud, "", ""));
            });
            var resultList = resultConcurrentStack.ToList();
            int fromDB = resultList.Where(w => w.DataFrom == "Data From DB").Count();
            int fromRedis = resultList.Where(w => w.DataFrom == "Data From Redis").Count();

            Console.WriteLine($"Data From DB :{fromDB}筆");
            Console.WriteLine($"Data From Redis :{fromRedis}筆");
            Console.Read();
        }
    }
}
