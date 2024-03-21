
using Zeng.mdx.commons;
namespace Zeng.mdx.parsers.w3x.unitsdoo
{
    /**
     * An inventory item. 库存
     */
    public class InventoryItem
    {
        public int slot = 0;
        public string id = "\0\0\0\0";

        public void Load(BinaryStream stream)
        {
            slot = stream.ReadInt32();
            id = stream.ReadBinary(4);
        }

        public void Save(BinaryStream stream)
        {
            stream.WriteInt32(slot);
            stream.WriteBinary(id);
        }
    }
}
