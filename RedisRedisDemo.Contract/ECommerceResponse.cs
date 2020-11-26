using System.Collections.Generic;

namespace RedisRedisDemo.Contract
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
        public List<EcommerceModel> Result { get; set; }
    }
}
