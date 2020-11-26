using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using RedisRedisDemo.Contract;
using RedisRedisDemo.Test.Library;
using System.Threading;

namespace RedisRedisDemo.UseMultipleServer
{
    class Program
    {
        static void Main(string[] args)
        {
            string host1 = "";
            string host2 = "";
            string methoud = "ECommerce";
            //string methoud = "ECommerceLock";
            ConcurrentStack<ECommerceResponse> resultConcurrentStack1 = new ConcurrentStack<ECommerceResponse>();
            ConcurrentStack<ECommerceResponse> resultConcurrentStack2 = new ConcurrentStack<ECommerceResponse>();

            Parallel.Invoke(
                () =>
                {
                    //SERVER1 10次
                    Console.WriteLine($"Server 1 GO");
                    Parallel.For(1, 11, (i, state) =>
                    {
                        resultConcurrentStack1.Push(ApiHelper.Api<ECommerceResponse>(host1, EnumContentType.json, EnumApiMethodType.Get, methoud, "", ""));
                    });
                    Console.WriteLine($"Server 1 End");
                },
                () =>
                {
                    //Server2 10次
                    Console.WriteLine($"Server 2 GO");
                    Parallel.For(1, 11, (i, state) =>
                    {

                        resultConcurrentStack2.Push(ApiHelper.Api<ECommerceResponse>(host2, EnumContentType.json, EnumApiMethodType.Get, methoud, "", ""));
                    });
                    Console.WriteLine($"Server 2 End");
                });


            int fromDB1 = resultConcurrentStack1.Where(w => w.DataFrom == "Data From DB").Count();
            int fromRedis1 = resultConcurrentStack1.Where(w => w.DataFrom == "Data From Redis").Count();

            int fromDB2 = resultConcurrentStack2.Where(w => w.DataFrom == "Data From DB").Count();
            int fromRedis2 = resultConcurrentStack2.Where(w => w.DataFrom == "Data From Redis").Count();
            Console.WriteLine($"Server 1");
            Console.WriteLine($"Data From DB :{fromDB1}筆");
            Console.WriteLine($"Data From Redis :{fromRedis1}筆");
            Console.WriteLine("");
            Console.WriteLine($"Server 2");
            Console.WriteLine($"Data From DB :{fromDB2}筆");
            Console.WriteLine($"Data From Redis :{fromRedis2}筆");
            Console.Read();

        }
    }
}
