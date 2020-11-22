using System;
using System.Collections.Generic;
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
    }
}
