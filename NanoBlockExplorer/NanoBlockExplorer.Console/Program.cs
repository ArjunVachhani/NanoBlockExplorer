using BitcoinLib.Responses;
using BitcoinLib.Services.Coins.Bitcoin;
using NanoBlockExplorer.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NanoBlockExplorer.Console
{
    class Program
    {
        static int NodeId
        {
            get
            {
                return int.Parse(ConfigurationManager.AppSettings["NodeId"]);
            }
        }

        static void Main(string[] args)
        {
            //SyncFromDbLastBlock();
            //SyncFromStart();
            //SyncFromBlockHash("00000000a967199a2fad0877433c93df785a8d8ce062e5f9b451cd1397bdbf62");
            SyncFromDbLastBlockForwardOnly();
        }

        static void SyncFromDbLastBlock()
        {
            while (true)
            {
                try
                {
                    System.Console.WriteLine("Checking how many blocks behind.");
                    var unsyncedBlocks = GetUnsyncedBlockHash();
                    if (unsyncedBlocks.Count == 0)
                    {
                        System.Console.WriteLine("DB is up to date.");
                        Thread.Sleep(1000);
                    }
                    else
                    {
                        System.Console.WriteLine("Blocks to sync : " + unsyncedBlocks.Count);
                        System.Console.WriteLine(unsyncedBlocks.Count + " Blocks to sync");
                        var i = 0;
                        foreach (var blockHash in unsyncedBlocks)
                        {
                            System.Console.WriteLine("Syncing : " + blockHash + ", Remaining : " + (unsyncedBlocks.Count - i));
                            SyncBlock(blockHash);
                            i++;
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine(ex.ToString());
                    Thread.Sleep(1000);
                }
            }
        }

        static void SyncFromDbLastBlockForwardOnly()
        {
            string latestBlockInDb = GetBestBlockHashOfNode();
            SyncFromBlockHash(latestBlockInDb);
        }

        static List<string> GetUnsyncedBlockHash()
        {
            BitcoinService service = GetBitcoinService();
            string blockHash = service.GetBestBlockHash();
            string latestBlockInDb = GetBestBlockHashOfNode();
            List<string> unsyncedBlockHash = new List<string>();
            while (!blockHash.Equals(latestBlockInDb, StringComparison.OrdinalIgnoreCase) && !blockHash.Equals(GetGenesisBlockHash(), StringComparison.OrdinalIgnoreCase))
            {
                unsyncedBlockHash.Add(blockHash);
                var blockResponse = service.GetBlock(blockHash, true);
                blockHash = blockResponse.PreviousBlockHash;
            }
            unsyncedBlockHash.Reverse();
            return unsyncedBlockHash;
        }

        static void SyncFromStart()
        {
            string blockHashToSync = GetGenesisBlockHash();
            SyncFromBlockHash(blockHashToSync);
        }

        static void SyncFromBlockHash(string blockHashToSync)
        {
            BitcoinService service = GetBitcoinService();
            while (!string.IsNullOrWhiteSpace(blockHashToSync))
            {
                var block = service.GetBlock(blockHashToSync, true);
                System.Console.WriteLine("Syncing : " + block.Hash + ", Height : " + block.Height);
                SyncBlock(block);
                blockHashToSync = block.NextBlockHash;
            }

        }

        static void SyncBlock(string blockHash)
        {
            if (string.IsNullOrWhiteSpace(blockHash))
                return;
            BitcoinService service = GetBitcoinService();
            var block = service.GetBlock(blockHash, true);
            SyncBlock(block);
        }

        static void SyncBlock(GetBlockResponse block)
        {
            if (block == null)
                return;
            BitcoinService service = GetBitcoinService();
            if (BlockExist(block.Hash))
            {
                AssertBlock(block);//throws exception and blocks normal flow
                MarkBlockReceived(block.Hash);
            }
            else
            {
                //add one by one
                if (block.Tx != null)
                {
                    using (var db = GetDatabase())
                    {
                        db.BeginTransaction();
                        var dbBlock = new Block
                        {
                            Bits = block.Bits,
                            CreatedOn = DateTime.UtcNow,
                            Difficulty = Convert.ToDecimal(block.Difficulty),
                            Hash = block.Hash,
                            Height = block.Height,
                            MerkelRootHash = block.MerkleRoot,
                            Nounce = long.Parse(block.Nonce),
                            PreviousBlockHash = block.PreviousBlockHash,
                            Size = block.Size,
                            Time = block.Time,
                            Version = block.Version
                        };
                        db.Blocks.Add(dbBlock);

                        foreach (var txid in block.Tx)
                        {
                            if (db.Transactions.Exists(txid))
                            {
                                AssertTransaction(txid);
                                var blockTransaction = new BlockTransaction
                                {
                                    BlockHash = block.Hash,
                                    TxId = txid,
                                    CreatedOn = DateTime.Now
                                };
                                db.BlockTransaction.Add(blockTransaction);
                            }
                            else
                            {
                                var tx = service.GetRawTransaction(txid, 1);
                                var transaction = new Transaction
                                {
                                    TxId = tx.TxId,
                                    LockTime = tx.LockTime,
                                    Time = tx.Time,
                                    Version = tx.Version,
                                    CreatedOn = DateTime.UtcNow,
                                };
                                db.Transactions.Add(transaction);
                                var blockTransaction = new BlockTransaction
                                {
                                    BlockHash = block.Hash,
                                    TxId = txid,
                                    CreatedOn = DateTime.Now
                                };
                                db.BlockTransaction.Add(blockTransaction);
                                foreach (var vin in tx.Vin)
                                {
                                    var input = new TransactionInput
                                    {
                                        TxId = tx.TxId,
                                        InputTxId = vin.TxId,
                                        InputVOut = string.IsNullOrWhiteSpace(vin.CoinBase) ? int.Parse(vin.Vout) : default(int?), //coinbase transaction does not have previous transction
                                        CreatedOn = DateTime.UtcNow
                                    };
                                    db.TransactionInputs.Add(input);
                                }

                                foreach (var vout in tx.Vout)
                                {
                                    var output = new TransactionOutput
                                    {
                                        TxId = tx.TxId,
                                        Satoshi = (long)(vout.Value * 100000000),
                                        Script = vout.ScriptPubKey.Asm,
                                        Address = vout.ScriptPubKey.Addresses != null ? vout.ScriptPubKey.Addresses.FirstOrDefault() : null,
                                        Type = vout.ScriptPubKey.Type,
                                        VOut = vout.N,
                                        CreatedOn = DateTime.UtcNow
                                    };
                                    db.TransactionOutputs.Add(output);
                                }
                            }
                        }
                        db.BlockNodes.Add(new BlockNode { BlockHash = block.Hash, NodeId = NodeId, CreatedOn = DateTime.UtcNow });
                        db.Commit();
                    }
                }
            }
        }

        static void AssertBlock(GetBlockResponse block)
        {
            BitcoinService service = GetBitcoinService();
            using (var db = GetDatabase())
            {
                Block dbBlock = db.Blocks.GetByHash(block.Hash);
                if (dbBlock.Height != block.Height || !dbBlock.MerkelRootHash.Equals(block.MerkleRoot, StringComparison.OrdinalIgnoreCase) || !dbBlock.PreviousBlockHash.Equals(block.PreviousBlockHash, StringComparison.OrdinalIgnoreCase))
                    throw new Exception("Block details do not match");
                var transactions = db.Transactions.GetTransactionsForBlockHash(block.Hash);
                if (block.Tx.Count == transactions.Count)//both has same no of transaction
                {
                    var allTxAinB = block.Tx.All(x => transactions.Any(y => y.TxId.Equals(x, StringComparison.OrdinalIgnoreCase)));
                    var allTxBinA = transactions.All(x => block.Tx.Any(y => y.Equals(x.TxId, StringComparison.OrdinalIgnoreCase)));
                    if (allTxAinB && allTxAinB) //txid match
                    {
                        foreach (var txid in block.Tx)
                        {
                            try
                            {
                                AssertTransaction(txid);
                            }
                            catch (Exception ex)
                            {
                                throw new Exception("Transaction check failed for Block : " + block.Hash, ex);
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("Transaction do not match. for block hash : " + block.Hash);
                    }
                }
                else
                {
                    throw new Exception("block has different no of transactions. for block hash : " + block.Hash);
                }
            }
        }

        static void AssertTransaction(string txid)
        {
            BitcoinService service = GetBitcoinService();
            using (var db = GetDatabase())
            {
                var tx = service.GetRawTransaction(txid, 1);
                List<TransactionInput> inputs = db.TransactionInputs.GetTransactionInputsFor(txid);
                var allInputAinB = tx.Vin.Where(x => string.IsNullOrWhiteSpace(x.CoinBase)).All(x => inputs.Any(y => x.TxId.Equals(y.InputTxId, StringComparison.OrdinalIgnoreCase) && int.Parse(x.Vout) == y.InputVOut));
                var allInputBinA = inputs.All(x => x.InputVOut == null || tx.Vin.Any(y => x.InputTxId.Equals(y.TxId, StringComparison.OrdinalIgnoreCase) && x.InputVOut == int.Parse(y.Vout)));

                List<TransactionOutput> outputs = db.TransactionOutputs.GetTransactionOutputsFor(txid);
                var allOutputAinB = tx.Vout.All(x => outputs.Any(y => (long)(x.Value * 100000000) == y.Satoshi && x.N == y.VOut && x.ScriptPubKey.Addresses == null || x.ScriptPubKey.Addresses.Any(z => y.Address.Equals(z, StringComparison.OrdinalIgnoreCase))));
                var allOutputBinA = outputs.All(x => tx.Vout.Any(y => x.Satoshi == (long)(y.Value * 100000000) && x.VOut == y.N && (y.ScriptPubKey.Addresses == null || y.ScriptPubKey.Addresses.Any(z => z.Equals(x.Address, StringComparison.OrdinalIgnoreCase)))));
                if (allInputAinB == false || allInputBinA == false || allOutputAinB == false || allOutputBinA == false)
                    throw new Exception("input outputs do not match for  : " + txid);
            }
        }

        static BitcoinService GetBitcoinService()
        {
            return new BitcoinService(true);
        }

        static string GetGenesisBlockHash()
        {
            if (ConfigurationManager.AppSettings["Environment"] == "testnet")
                return "000000000933ea01ad0ee984209779baaec3ced90fa3f408719526f8d77f4943";
            else
                return "000000000019d6689c085ae165831e934ff763ae46a2a6c172b3f1b60a8ce26f";
        }

        static string GetBestBlockHashOfNode()
        {
            using (var db = GetDatabase())
                return db.BlockNodes.GetBestBlockHash(NodeId);
        }

        static void MarkBlockReceived(string blockHash)
        {
            using (var db = GetDatabase())
            {
                if (!db.BlockNodes.Exists(blockHash, NodeId))
                    db.BlockNodes.Add(new BlockNode { BlockHash = blockHash, NodeId = NodeId, CreatedOn = DateTime.UtcNow });
            }
        }

        static bool BlockExist(string blockHash)
        {
            using (var db = GetDatabase())
                return db.Blocks.Exists(blockHash);
        }

        static List<Transaction> GetTransactions(string blockHash)
        {
            using (var db = GetDatabase())
                return db.Transactions.GetTransactionsForBlockHash(blockHash);
        }

        static Database GetDatabase()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["BlockExplorerDB"].ConnectionString;
            return new Database(connectionString);
        }
    }
}
