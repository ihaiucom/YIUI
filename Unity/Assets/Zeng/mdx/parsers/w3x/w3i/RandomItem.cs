using MdxLib.ModelFormats.Mdx;

namespace Zeng.mdx.parsers.w3x.w3i
{
    public class RandomItem
    {
        public int chance = 0;
        public string id = "\0\0\0\0";

        public void load(CLoader stream)
        {
            this.chance = stream.ReadInt32();
            this.id = stream.ReadString(4);
        }
    }
}
