using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NanoBlockExplorer.Data
{
    public class TransactionOutputRepository : Repository
    {
        public TransactionOutputRepository(UnitOfWork uow) : base(uow)
        {

        }

        public void Add(TransactionOutput output)
        {
            Execute(@"INSERT INTO [TransactionOutput] ([TxId] ,[VOut] ,[Address] ,[Satoshi] ,[Script] ,[Type] ,[CreatedOn])
                        VALUES(@TxId, @VOut, @Address, @Satoshi, @Script, @Type, @CreatedOn)", output);
        }

        public List<TransactionOutput> GetTransactionOutputsFor(string txid)
        {
            return Query<TransactionOutput>("SELECT * FROM [TransactionOutput] WHERE [TxId] = @TxId", new { TxId = txid }).ToList();
        }
    }
}
