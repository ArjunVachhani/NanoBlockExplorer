using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NanoBlockExplorer.Data
{
    public class Transaction
    {
        public string TxId { get; set; }
        public int Size { get; set; }
        public long Version { get; set; }
        public long LockTime { get; set; }
        public long Time { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
