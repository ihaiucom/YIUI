using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zeng.mdx.commons;

namespace Zeng.mdx.parsers.mpq
{
    public class MpqBlockTable
    {
        public MpqCrypto c;
        public List<MpqBlock> entries;

        public MpqBlockTable(MpqCrypto c)
        {
            this.c = c;
            this.entries = new List<MpqBlock>();
        }

        public void clear() { 
            this.entries.Clear();
        }

        public void addEmpties(int howMany)
        {
            for (int i = 0; i < howMany; i++)
            {
                this.entries.Add(new MpqBlock());
            }
        }

        public void load(byte[] bytes)
        {
            int entriesCount = bytes.Length / 16;
            byte[] decryBytes = this.c.decryptBlock(bytes, MpqConstants.BLOCK_TABLE_KEY);
            UInt32[] uint32array = BytesUtils.Uint32Array(decryBytes);

            int offset = 0;

            // Clear the table and add the needed empties.
            this.clear();
            this.addEmpties(entriesCount);


            for (int i = 0; i < this.entries.Count; i++)
            {
                MpqBlock block = this.entries[i];

                UInt32[] itemBytes = new UInt32[4];
                Array.Copy(uint32array, offset, itemBytes, 0, 4);
                block.load(itemBytes);

                offset += 4;
            }

        }
    }
}
