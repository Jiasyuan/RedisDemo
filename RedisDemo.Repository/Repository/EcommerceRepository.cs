using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using RedisDemo.Repository.Common.Interface;
using RedisDemo.Repository.DTO;
using RedisDemo.Repository.Repository.Interface;

namespace RedisDemo.Repository.Repository
{
    public class EcommerceRepository : IDisposable, IECommerceRepository
    {
        private readonly IDatabaseConnectionHelper _databaseConnectionHelper;
        private readonly IRedisCacheHelper _redisCacheHelper;
        private Func<IEnumerable<EcommerceDto>> _getECommerceFromDb;

        public EcommerceRepository(IDatabaseConnectionHelper databaseConnectionHelper, IRedisCacheHelper redisCacheHelper)
        {
            this._databaseConnectionHelper = databaseConnectionHelper;
            this._redisCacheHelper = redisCacheHelper;
            this._getECommerceFromDb = GetECommerceFromDb;
        }

        #region IDisposable Support

        private bool _disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    return;
                }

                _disposedValue = true;
            }
        }

        ~EcommerceRepository()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        public Tuple<string, List<EcommerceDto>> GetECommerce()
        {
            return _redisCacheHelper.GetCacheTuple<EcommerceDto>("ECommerce", _getECommerceFromDb);
        }

        public async Task<Tuple<string, List<EcommerceDto>>> GetECommerceLock()
        {
            return await  _redisCacheHelper.GetCacheTupleRedLockAsync<EcommerceDto>("ECommerce", _getECommerceFromDb);
        }


        /// <summary>
        /// Get ECommerce From DataBase
        /// </summary>
        /// <returns></returns>
        protected IEnumerable<EcommerceDto> GetECommerceFromDb()
        {
            string sqlCommand = @"SELECT [ecommerce_Id]
                                  ,[en_name]
                                  ,[ch_name]
                              FROM [dbo].[ecommerce] with(nolock)";
            Thread.Sleep(1800);
            using (var conn = _databaseConnectionHelper.Create())
            {
                return conn.Query<EcommerceDto>(sqlCommand);
            }
        }
        /*
        private Tuple<string, IEnumerable<EcommerceDto>> GetECommerceFromRedis()
        {
            IEnumerable<EcommerceDto> eCommerceList =null;

            Tuple<string, IEnumerable<EcommerceDto>> resultTuple = Tuple.Create("Data From Redis", eCommerceList);

            return resultTuple;
        }*/
    }
}
