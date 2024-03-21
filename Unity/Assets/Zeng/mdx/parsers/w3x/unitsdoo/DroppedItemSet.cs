using System.Collections.Generic;
using Zeng.mdx.commons;
namespace Zeng.mdx.parsers.w3x.unitsdoo
{
    /**
     * A dropped item set.
     */
    public class DroppedItemSet
    {
        private List<DroppedItem> items = new List<DroppedItem>();

        public void Load(BinaryStream stream)
        {
            int count = stream.ReadInt32();

            for (int i = 0; i < count; i++)
            {
                DroppedItem item = new DroppedItem();
                item.Load(stream);
                items.Add(item);
            }
        }

        public void Save(BinaryStream stream)
        {
            stream.WriteInt32(items.Count);

            foreach (DroppedItem item in items)
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