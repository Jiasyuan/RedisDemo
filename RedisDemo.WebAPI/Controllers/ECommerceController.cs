using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RedisDemo.Repository.Repository.Interface;

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

        
    }
}
