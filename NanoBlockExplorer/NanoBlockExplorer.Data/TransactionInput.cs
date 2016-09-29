using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NanoBlockExplorer.Data
{
    public class TransactionInput
    {
        public string TxId { get; set; }
        public string InputTxId { get; set; }
        public int? InputVOut { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
