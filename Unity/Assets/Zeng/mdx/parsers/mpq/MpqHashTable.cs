using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zeng.mdx.commons;

namespace Zeng.mdx.parsers.mpq
{
    public class MpqHashTable
    {
        public MpqCrypto c;
        public List<MpqHash> entries;

        public MpqHashTable(MpqCrypto c)
        {
            this.c = c;
            entries = new List<MpqHash>();
        }

        public void clear()
        {
            this.entries.Clear();
        }

        public void addEmpties(int howMany)
        {
            for(int i = 0; i < howMany; i ++)
            {
                this.entries.Add(new MpqHash());
            }
        }

        public void load(byte[] bytes)
        {
            int entriesCount = bytes.Length / 16;
            byte[] decryBytes = this.c.decryptBlock(bytes, MpqConstants.HASH_TABLE_KEY);
            UInt32[] uint32array = BytesUtils.Uint32Array(decryBytes);

            int offset = 0;

            // Clear the table and add the needed empties.
            this.clear();
            this.addEmpties(entriesCount);

            for(int i = 0; i < this.entries.Count; i ++)
            {
                MpqHash hash = this.entries[i];

                UInt32[] itemBytes = new UInt32[4];
                Array.Copy(uint32array, offset, itemBytes, 0, 4);
                hash.load(itemBytes);

                offset += 4;
            }
        }

        public MpqHash get(string name)
        {
            int offset = (int)(c.hash(name, MpqConstants.HASH_TABLE_INDEX) & (entries.Count - 1));
            uint nameA = c.hash(name, MpqConstants.HASH_NAME_A);
            uint nameB = c.hash(name, MpqConstants.HASH_NAME_B);

            for (int i = 0, l = entries.Count; i < l; i++)
            {
                MpqHash hash = entries[(i + offset) % l];

                if (nameA == hash.nameA && nameB == hash.nameB)
                {
                    return hash;
                }
                else if (hash.blockIndex == 0xFFFFFFFF)
                {
                    return null;
                }
            }

            return null;
        }



    }
}
