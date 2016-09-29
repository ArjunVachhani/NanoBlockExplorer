using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NanoBlockExplorer.Data
{
    public class Database : IDisposable
    {
        private UnitOfWork UOW { get; set; }

        public Database(string connectionString)
        {
            UOW = new UnitOfWork(connectionString);
            Blocks = new BlockRepository(UOW);
            BlockNodes = new BlockNodeRepository(UOW);
            BlockTransaction = new BlockTransactionRepository(UOW);
            Transactions = new TransactionRepository(UOW);
            TransactionInputs = new TransactionInputRepository(UOW);
            TransactionOutputs = new TransactionOutputRepository(UOW);
        }

        public BlockRepository Blocks { get; private set; }

        public BlockNodeRepository BlockNodes { get; private set; }

        public BlockTransactionRepository BlockTransaction { get; private set; }

        public TransactionRepository Transactions { get; private set; }

        public TransactionInputRepository TransactionInputs { get; private set; }

        public TransactionOutputRepository TransactionOutputs { get; private set; }

        public void Dispose()
        {
            if (UOW != null)
            {
                UOW.Dispose();
            }
        }

        public void BeginTransaction()
        {
            UOW.BeginTransaction();
        }

        public void Commit()
        {
            UOW.CommitTransaction();
        }

        public void Rollback()
        {
            UOW.RollbackTransaction();
        }
    }
}
