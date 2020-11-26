using System.Linq;
using Microsoft.AspNetCore.Mvc;
using RedisDemo.Repository.DTO;
using RedisDemo.Repository.Repository.Interface;
using RedisDemo.WebAPI.Helper;
using RedisRedisDemo.Contract;

namespace RedisDemo.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ECommerceController : ControllerBase
    {
        private IECommerceRepository _eCommerceRepository;

        public ECommerceController(IECommerceRepository eCommerceRepository)
        {
            this._eCommerceRepository = eCommerceRepository;
        }

        [HttpGet]
        public ECommerceResponse GetECommerce()
        {
            var resultTuple = _eCommerceRepository.GetECommerce();
            var result = MapperHelper.MapperProperties<EcommerceDto, EcommerceModel>(resultTuple.Item2);
            return new ECommerceResponse()
            {
                DataFrom = resultTuple.Item1,
                Result = result.ToList()
            };
        }
    }
}
