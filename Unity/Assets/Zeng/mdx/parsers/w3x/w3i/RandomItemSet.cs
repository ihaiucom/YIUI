using MdxLib.ModelFormats.Mdx;
using System.Collections.Generic;


namespace Zeng.mdx.parsers.w3x.w3i
{
    public class RandomItemSet
    {
        public List<RandomItem> items = new List<RandomItem>();

        public void load(CLoader stream)
        {
            for (int i = 0, l = stream.ReadInt32(); i < l; i++)
            {
                RandomItem item = new RandomItem();

                item.load(stream);

                items.Add(item);
            }
        }
    }
}
