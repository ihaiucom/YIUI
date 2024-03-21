
using System.Collections.Generic;
using Zeng.mdx.commons;

namespace Zeng.mdx.parsers.w3x.doo
{
    /**
     * A random item set.
     */
    public class RandomItemSet
    {
        public List<RandomItem> items = new List<RandomItem>();

        public void Load(BinaryStream stream)
        {
            int l = (int)stream.ReadUint32();
            for (int i = 0; i < l; i++)
            {
                RandomItem item = new RandomItem();
                item.Load(stream);
                items.Add(item);
            }
        }

        public void Save(BinaryStream stream)
        {
            stream.WriteUint32((uint)items.Count);
            foreach (RandomItem item in items)
            {
                item.Save(stream);
            }
        }

        public int GetByteLength()
        {
            return 4 + items.Count * 8;
        }
    }

}
