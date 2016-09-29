using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NanoBlockExplorer.Data
{
    public class BlockNodeRepository : Repository
    {
        public BlockNodeRepository(UnitOfWork uow) : base(uow)
        {

        }

        public void Add(BlockNode blockNode)
        {
            Execute(@"INSERT INTO [Block_Node]([BlockHash],[NodeId],[CreatedOn])
                            VALUES(@BlockHash, @NodeId, @CreatedOn)", blockNode);
        }

        public string GetBestBlockHash(int nodeId)
        {
            return Query<string>(@"SELECT TOP 1 [Block].[Hash] FROM [Block] INNER JOIN [Block_Node] ON [Block].[Hash] = [Block_Node].[BlockHash] WHERE [NodeId] = NodeId ORDER BY [Height] DESC", new { NodeId = nodeId }).FirstOrDefault();
        }

        public bool Exists(string blockHash, int nodeId)
        {
            return Query<BlockNode>("SELECT TOP 1 * FROM [Block_Node] WHERE [BlockHash] = @BlockHash AND [NodeId]= @NodeId", new { BlockHash = blockHash, NodeId = nodeId }).Any();
        }
    }
}
