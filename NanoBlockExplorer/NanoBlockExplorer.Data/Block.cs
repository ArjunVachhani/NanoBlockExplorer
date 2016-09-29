using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NanoBlockExplorer.Data
{
    public class Block
    {
        public string Hash { get; set; }
        public string PreviousBlockHash { get; set; }
        public string MerkelRootHash { get; set; }
        public int Version { get; set; }
        public int Time { get; set; }
        public string Bits { get; set; }
        public long Nounce { get; set; }
        public int Size { get; set; }
        public int Height { get; set; }
        public decimal Difficulty { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
