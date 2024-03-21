using MdxLib.ModelFormats.Mdx;

namespace Zeng.mdx.parsers.w3x.w3i
{
    public class Force
    {
        public uint flags = 0;
        public uint playerMasks = 0;
        public string name = "";

        public void load(CLoader stream)
        {
            this.flags = stream.ReadUInt32();
            this.playerMasks = stream.ReadUInt32();
            this.name = stream.ReadNull();
        }
    }
}
