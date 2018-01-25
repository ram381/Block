using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Block
{
    /// <summary>
    /// credits to Sylvain Saurel
    /// </summary>
    public partial class Block : Form
    {
        public Block()
        {
            InitializeComponent();
            Blockchain blockchain = new Blockchain(4);
            blockchain.addBlock(blockchain.newBlock("second block"));
            blockchain.addBlock(blockchain.newBlock("third block"));
            Console.WriteLine("Blockchain valid ?  " + blockchain.isBlockChainValid());
            Console.WriteLine(blockchain);
        }
    }   

    public class Bloock
    {
        public Bloock(int index, long timestamp, string previoushash, string data)
        {
            this.index = index;
            this.timestamp = timestamp;
            this.previoushash = previoushash;
            this.data = data;
            this.nonce = 0;
            hash = Bloock.CalculateHash(this);
        }


        public string toString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("Block #").Append(index).Append(" [previousHash : ").Append(previoushash).Append(", ").
            Append("timestamp : ").Append(new DateTime(timestamp)).Append(", ").Append("data : ").Append(data).Append(", ").
            Append("hash : ").Append(hash).Append("]");
            return builder.ToString();
        }

        public void mineBlock(int diffifulty)
        {
            nonce = 0;
            int num = 0;
            while (!hash.Substring(0, diffifulty).Equals(num.ToString().PadLeft(diffifulty, '0')))
            {
                nonce++;
                hash = Bloock.CalculateHash(this);
            }
        }

        public static string CalculateHash(Bloock block)
        {
            if (block != null)
            {
                return sha256(block.str());
            }
            return null;
        }

        public string calHash()
        {
            return CalculateHash(this);
        }
        static string sha256(string randomString)
        {
            System.Security.Cryptography.SHA256Managed crypt = new System.Security.Cryptography.SHA256Managed();
            System.Text.StringBuilder hash = new System.Text.StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(randomString), 0, Encoding.UTF8.GetByteCount(randomString));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
        }


        public string str()
        {
            return index + timestamp + Previoushash + data + nonce;
        }

        private int index;
        private long timestamp;
        private string previoushash;
        private string data;
        private string hash;
        private int nonce;

        public int Index { get => index; set => index = value; }
        public long Timestamp { get => timestamp; set => timestamp = value; }
        public string Previoushash { get => previoushash; set => previoushash = value; }
        public string Data { get => data; set => data = value; }
        public string Hash { get => hash; set => hash = value; }
    }
    public class Blockchain
    {
        public Blockchain(int difficulty)
        {
            this.difficulty = difficulty;
            blocks = new List<Bloock>();
            // create the first block
            Bloock b = new Bloock(0, DateTime.UtcNow.Millisecond, null, "genesis block");
            b.mineBlock(difficulty);
            blocks.Add(b);
        }

        public int getDifficulty()
        {
            return difficulty;
        }


        private int difficulty;
        private List<Bloock> blocks;


        public Bloock latestBloock()
        {
            return blocks.Take(blocks.Count).FirstOrDefault();
        }


        public Bloock newBlock(String data)
        {
            Bloock latestBlock = latestBloock();
            return new Bloock(latestBlock.Index + 1, System.DateTime.UtcNow.Millisecond, latestBlock.Hash, data);

        }

        public void addBlock(Bloock b)
        {
            if (b != null)
            {
                b.mineBlock(difficulty);
                blocks.Add(b);
            }
        }

        public bool isFirstBlockValid()
        {
            Bloock firstBlock = blocks[0];

            if (firstBlock.Index != 0)
            {
                return false;
            }

            if (firstBlock.Previoushash != null)
            {
                return false;
            }

            if (firstBlock.Hash == null ||
                 !firstBlock.calHash().Equals(firstBlock.Hash))
            {
                return false;
            }

            return true;
        }

        public String toString()
        {
            StringBuilder builder = new StringBuilder();
            foreach (var item in blocks)
            {
                builder.Append(item).Append("\n");
            }
            return builder.ToString();
        }
        public bool isValidNewBlock(Bloock newBlock, Bloock previousBlock)
        {
            if (newBlock != null && previousBlock != null)
            {
                if (previousBlock.Index + 1 != newBlock.Index)
                {
                    return false;
                }

                if (newBlock.Previoushash == null ||
                    !newBlock.Previoushash.Equals(previousBlock.Hash))
                {
                    return false;
                }

                if (newBlock.Hash == null ||
                    !newBlock.calHash().Equals(newBlock.Hash))
                {
                    return false;
                }

                return true;
            }
            return false;
        }

        public bool isBlockChainValid()
        {
            if (!isFirstBlockValid())
            {
                return false;
            }

            for (int i = 1; i < blocks.Count; i++)
            {
                Bloock currentBlock = blocks[i];
                Bloock prevBlock = blocks[i - 1];
                if (!isValidNewBlock(currentBlock, prevBlock))
                {
                    return false;
                }
            }
            return true;
        }
    }
}



