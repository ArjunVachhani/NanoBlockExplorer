using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NanoBlockExplorer.Data
{
    public class BlockNode
    {
        public string BlockHash { get; set; }
        public int NodeId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
