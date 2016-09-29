using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NanoBlockExplorer.Data
{
    public class UnitOfWork : IDisposable
    {
        private object lockObject = new object();
        private IDbConnection _dbConnection;
        private IDbTransaction _dbTransaction;
        private static string ConnectionString;

        public UnitOfWork(string connectionString)
        {
            ConnectionString = connectionString;
        }

        private bool CanReleaseResource
        {
            get
            {
                lock (lockObject)
                {
                    return _dbTransaction == null;
                }
            }
        }

        public IDbConnection DbConnection
        {
            get
            {
                lock (lockObject)
                {
                    if (_dbConnection == null || _dbConnection.State == ConnectionState.Closed)
                    {
                        _dbConnection = new SqlConnection(ConnectionString);
                        _dbConnection.Open();
                    }
                    return _dbConnection;
                }
            }
            private set
            {
                _dbConnection = value;
            }
        }

        public IDbTransaction DbTransaction
        {
            get
            {
                return _dbTransaction;
            }

            private set
            {
                _dbTransaction = value;
            }
        }

        public void BeginTransaction()
        {
            lock (lockObject)
            {
                _dbTransaction = DbConnection.BeginTransaction();
            }
        }

        public void CommitTransaction()
        {
            lock (lockObject)
            {
                DbTransaction.Commit();
                DbTransaction = null;
                ReleaseResources();
            }
        }

        public void RollbackTransaction()
        {
            lock (lockObject)
            {
                DbTransaction.Rollback();
                DbTransaction = null;
                ReleaseResources();
            }
        }

        public void ReleaseResources()
        {
            lock (lockObject)
            {
                if (CanReleaseResource)
                {
                    DbConnection.Dispose();
                    DbConnection = null;
                }
            }
        }

        public void Dispose()
        {
            lock (lockObject)
            {
                if (_dbTransaction != null)
                    _dbTransaction.Dispose();
                if (_dbConnection != null)
                    _dbConnection.Dispose();
            }
        }
    }
}
