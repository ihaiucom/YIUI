using MdxLib.ModelFormats.Mdx;

namespace Zeng.mdx.parsers.w3x.w3i
{
    public class RandomUnit
    {
        public int chance = 0;
        public string[] ids = new string[0];

        public void load(CLoader stream, int positions)
        {
            this.chance = stream.ReadInt32();

            for (int i = 0; i < positions; i++)
            {
                this.ids[i] = stream.ReadString(4);
            }
        }
    }
}
