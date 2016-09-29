using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NanoBlockExplorer.Data
{
    public class BlockRepository : Repository
    {
        public BlockRepository(UnitOfWork uow) : base(uow) { }


        public void Add(Block block)
        {
            Execute(@"INSERT INTO [Block] ([Hash] ,[PreviousBlockHash] ,[MerkelRootHash] ,[Version] ,[Time] ,[Bits] ,[Nounce] ,[Size] ,[Height] ,[Difficulty] ,[CreatedOn])
                        VALUES (@Hash,@PreviousBlockHash,@MerkelRootHash,@Version,@Time,@Bits,@Nounce ,@Size,@Height ,@Difficulty,@CreatedOn) ", block);
        }

        public bool Exists(string hash)
        {
            return Query<Block>("SELECT TOP 1 * FROM [Block] WHERE [Hash] = @Hash", new { Hash = hash }).Any();
        }

        public Block GetByHash(string hash)
        {
            return Query<Block>("SELECT TOP 1 * FROM [Block] WHERE [Hash] = @Hash", new { Hash = hash }).FirstOrDefault();
        }
    }
}
