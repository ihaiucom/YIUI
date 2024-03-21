using MdxLib.ModelFormats.Mdx;

namespace Zeng.mdx.parsers.w3x.w3i
{
    public class RandomItemTable
    {
        public int id = 0;
        public string name = "";
        public RandomItemSet[] sets = new RandomItemSet[] { };

        public void load(CLoader stream)
        {
            this.id = stream.ReadInt32();
            this.name = stream.ReadNull();

            this.sets = new RandomItemSet[stream.ReadUInt32()];
            for (int i = 0, l = sets.Length; i < l; i++)
            {
                RandomItemSet set = new RandomItemSet();

                set.load(stream);

                this.sets[i] = set;
            }
        }
    }
}
