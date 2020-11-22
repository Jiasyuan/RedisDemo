using Microsoft.AspNetCore.Mvc;
using RedisDemo.Repository.Repository.Interface;
using RedisDemo.WebAPI.Model;

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

        public ECommerceResponse GetECommerce()
        {
            var resultTuple = _eCommerceRepository.GetECommerce();
            return new ECommerceResponse()
            {
                DataFrom = resultTuple.Item1,
                Result = resultTuple.Item2
            };
        }
    }
}
