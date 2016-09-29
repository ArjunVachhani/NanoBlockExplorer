using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NanoBlockExplorer.Data
{
    public class TransactionOutput
    {
        public string TxId { get; set; }
        public int VOut { get; set; }
        public string Address { get; set; }
        public long Satoshi { get; set; }
        public string Script { get; set; }
        public string Type { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
