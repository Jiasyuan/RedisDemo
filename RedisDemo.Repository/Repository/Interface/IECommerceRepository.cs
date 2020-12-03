using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RedisDemo.Repository.DTO;

namespace RedisDemo.Repository.Repository.Interface
{
    public interface IECommerceRepository
    {
        /// <summary>
        /// Get All E-Commerce
        /// </summary>
        /// <returns></returns>
        Tuple<string, List<EcommerceDto>> GetECommerce();

        /// <summary>
        /// Get All E-Commerce RedLock
        /// </summary>
        /// <returns></returns>
        Task<Tuple<string, List<EcommerceDto>>> GetECommerceLock();
    }
}
