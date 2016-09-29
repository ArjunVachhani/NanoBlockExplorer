using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace NanoBlockExplorer.Data
{
    public abstract class Repository
    {
        private IDbConnection DbConnection
        {
            get
            {
                return UOW.DbConnection;
            }
        }

        private IDbTransaction DbTransactoin
        {
            get
            {
                return UOW.DbTransaction;
            }
        }

        private UnitOfWork UOW { get; set; }

        public Repository(UnitOfWork uow)
        {
            if (uow == null)
                throw new NullReferenceException("transaction should not be null.");
            UOW = uow;
        }

        public List<T> Query<T>(string sql, object param = null)
        {
            var result = DbConnection.Query<T>(sql, param, DbTransactoin).ToList();
            ReleaseResources();
            return result;
        }

        public T FirstOrDefault<T>(string sql, object param = null)
        {
            T result = DbConnection.Query<T>(sql, param, DbTransactoin).FirstOrDefault();
            ReleaseResources();
            return result;
        }

        public void Execute(string sql, object param = null)
        {
            DbConnection.Execute(sql, param, DbTransactoin);
            ReleaseResources();
        }

        private void ReleaseResources()
        {
            UOW.ReleaseResources();
        }
    }
}
