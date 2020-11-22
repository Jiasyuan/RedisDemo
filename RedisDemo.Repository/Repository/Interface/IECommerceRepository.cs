using System;
using System.Collections.Generic;
using RedisDemo.Repository.DTO;

namespace RedisDemo.Repository.Repository.Interface
{
    public interface IECommerceRepository
    {
        Tuple<string, List<EcommerceDto>> GetECommerce();
    }
}
