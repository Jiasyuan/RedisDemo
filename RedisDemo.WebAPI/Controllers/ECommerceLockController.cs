using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RedisDemo.Repository.DTO;
using RedisDemo.Repository.Repository.Interface;
using RedisDemo.WebAPI.Helper;
using RedisRedisDemo.Contract;

namespace RedisDemo.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ECommerceLockController : ControllerBase
    {
        private IECommerceRepository _eCommerceRepository;

        public ECommerceLockController(IECommerceRepository eCommerceRepository)
        {
            this._eCommerceRepository = eCommerceRepository;
        }

        [HttpGet]
        public async Task<ECommerceResponse> GetECommerce()
        {
            var resultTuple = await _eCommerceRepository.GetECommerceLock();
            var result = MapperHelper.MapperProperties<EcommerceDto, EcommerceModel>(resultTuple.Item2);
            return new ECommerceResponse()
            {
                DataFrom = resultTuple.Item1,
                Result = result.ToList()
            };
        }
    }
}
