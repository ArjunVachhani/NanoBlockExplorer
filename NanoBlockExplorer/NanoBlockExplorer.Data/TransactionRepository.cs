using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NanoBlockExplorer.Data
{
    public class TransactionRepository : Repository
    {
        public TransactionRepository(UnitOfWork uow) : base(uow) { }

        public void Add(Transaction transaction)
        {
            Execute(@"INSERT INTO [Transaction] ([TxId] ,[Size] ,[Version] ,[LockTime] ,[Time] ,[CreatedOn])
                        VALUES(@TxId, @Size, @Version, @LockTime, @Time, @CreatedOn)", transaction);
        }

        public Transaction GetById(string txid)
        {
            return Query<Transaction>("SELECT TOP 1 * FROM [Transaction] WHERE [TxId] = @TxId", new { TxId = txid }).FirstOrDefault();
        }
        
        public bool Exists(string txid)
        {
            return Query<Transaction>("SELECT TOP 1 * FROM [Transaction] WHERE [TxId] = @TxId", new { TxId = txid }).Any();
        }

        public List<Transaction> GetTransactionsForBlockHash(string blockHash)
        {
            return Query<Transaction>("SELECT [Transaction].* FROM [Transaction] INNER JOIN [BlockTransaction] ON [Transaction].[TxId] = [BlockTransaction].[TxId] WHERE [BlockTransaction].[BlockHash] = @BlockHash", new { BlockHash = blockHash }).ToList();
        }
    }
}
