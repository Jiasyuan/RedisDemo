using System.Collections.Generic;
using RedisDemo.Repository.DTO;

namespace RedisDemo.WebAPI.Model
{
    public class ECommerceResponse
    {
        /// <summary>
        /// Data From
        /// </summary>
        public string DataFrom { get; set; }

        /// <summary>
        /// Result List
        /// </summary>
        public List<EcommerceDto> Result { get; set; }
    }
}
