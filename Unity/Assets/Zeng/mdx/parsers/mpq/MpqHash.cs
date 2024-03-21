using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zeng.mdx.commons;

namespace Zeng.mdx.parsers.mpq
{
    public class MpqHash
    {
        public uint nameA = 0xFFFFFFFF;
        public uint nameB = 0xFFFFFFFF;
        public uint locale = 0xFFFF;
        public uint platform = 0xFFFF;
        public uint blockIndex = MpqConstants.HASH_ENTRY_EMPTY;

        public void load(UInt32[] bytes)
        {
            uint localePlatform = bytes[2];

            this.nameA = bytes[0];
            this.nameB = bytes[1];
            this.locale = localePlatform & 0x0000FFFF;
            this.platform = (uint)BytesUtils.RightMove((int)localePlatform, 16);
            this.blockIndex = bytes[3];
        }
    }
}
