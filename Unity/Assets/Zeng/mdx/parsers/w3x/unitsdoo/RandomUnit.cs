
using Zeng.mdx.commons;

namespace Zeng.mdx.parsers.w3x.unitsdoo
{
    /**
     * A random unit.
     */
    public class RandomUnit
    {
        private string id = "\0\0\0\0";
        private int chance = 0;

        public void Load(BinaryStream reader)
        {
            id = reader.ReadBinary(4);
            chance = reader.ReadInt32();
        }

        public void Save(BinaryStream writer)
        {
            writer.WriteBinary(id);
            writer.WriteInt32(chance);
        }
    }
}