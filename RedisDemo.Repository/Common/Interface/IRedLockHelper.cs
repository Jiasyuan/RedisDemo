using RedLockNet.SERedis;

namespace RedisDemo.Repository.Common.Interface
{
    public interface IRedLockHelper
    {
        RedLockFactory GetRedLockFactory();
    }
}
