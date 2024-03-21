using MdxLib.ModelFormats.Mdx;
using System.IO;

namespace Zeng.mdx.parsers.w3x.imp
{
    public class Import
    {
        public int isCustom = 0;
        public string path = "";

        public void load(MemoryStream ms, CLoader cloader)
        {
            this.isCustom = cloader.ReadInt8();
            this.path = cloader.ReadNull();
        }
    }
}
