using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NanoBlockExplorer.Data
{
    public class BlockTransaction
    {
        public string BlockHash { get; set; }
        public string TxId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
