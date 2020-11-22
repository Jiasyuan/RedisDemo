using System;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using RedisDemo.Repository.Common.Interface;
using RedisDemo.Repository.DTO;

namespace RedisDemo.Repository.Repository
{
    public class EcommerceRepository : IDisposable
    {
        private readonly IDatabaseConnectionHelper _databaseConnectionHelper;

        public EcommerceRepository(IDatabaseConnectionHelper databaseConnectionHelper)
        {
            this._databaseConnectionHelper = databaseConnectionHelper;
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
            return null;
        }

        /// <summary>
        /// Get ECommerce From DataBase
        /// </summary>
        /// <returns></returns>
        private Tuple<string, List<EcommerceDto>> GetECommerceFromDb()
        {
            string sqlCommand = @"SELECT [ecommerce_Id]
                                  ,[en_name]
                                  ,[ch_name]
                              FROM [dbo].[ecommerce] with(nolock)";
            List<EcommerceDto> ECommerceList = new List<EcommerceDto>();
            using (var conn = _databaseConnectionHelper.Create())
            {
                var result = conn.Query<EcommerceDto>(sqlCommand);
                if (result != null && result.Count() > 0)
                    ECommerceList = result.ToList();
            }
            Tuple<string, List<EcommerceDto>> resultTuple = Tuple.Create("Data From DB", ECommerceList);

            return resultTuple;
        }

        private Tuple<string, List<EcommerceDto>> GetECommerceFromRedis()
        {
            List<EcommerceDto> ECommerceList = new List<EcommerceDto>();
            Tuple<string, List<EcommerceDto>> resultTuple = Tuple.Create("Data From Redis", ECommerceList);

            return resultTuple;
        }
    }
}
