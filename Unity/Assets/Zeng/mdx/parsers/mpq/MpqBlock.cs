using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zeng.mdx.parsers.mpq
{
    public class MpqBlock
    {
        public uint offset = 0;
        public uint compressedSize = 0;
        public uint normalSize = 0;
        public uint flags = 0;


        public MpqBlock() {
        
        }

        public void load(UInt32[] bytes)
        {
            this.offset = bytes[0];
            this.compressedSize = bytes[1];
            this.normalSize = bytes[2];
            this.flags = bytes[3];
        }
    }
}
