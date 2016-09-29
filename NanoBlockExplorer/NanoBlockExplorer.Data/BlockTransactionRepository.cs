using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NanoBlockExplorer.Data
{
    public class BlockTransactionRepository : Repository
    {
        public BlockTransactionRepository(UnitOfWork uow) : base(uow)
        {

        }

        public void Add(BlockTransaction blockTransactionn)
        {
            Execute(@"INSERT INTO [BlockTransaction] ([BlockHash] ,[TxId] ,[CreatedOn]) 
		                VALUES (@BlockHash ,@TxId ,@CreatedOn)", blockTransactionn);
        }
    }
}
