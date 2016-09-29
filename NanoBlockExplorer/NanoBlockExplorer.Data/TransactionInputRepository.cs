using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NanoBlockExplorer.Data
{
    public class TransactionInputRepository : Repository
    {
        public TransactionInputRepository(UnitOfWork uow) : base(uow)
        {

        }

        public void Add(TransactionInput input)
        {
            Execute(@"INSERT INTO [TransactionInput] ([TxId] ,[InputTxId] ,[InputVOut] ,[CreatedOn])
                        VALUES (@TxId ,@InputTxId ,@InputVOut ,@CreatedOn)", input);
        }

        public List<TransactionInput> GetTransactionInputsFor(string txid)
        {
            return Query<TransactionInput>("SELECT * FROM [TransactionInput] WHERE [TxId] = @TxId", new { TxId = txid }).ToList();
        }
    }
}
