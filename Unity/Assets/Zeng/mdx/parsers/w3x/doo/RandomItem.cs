using Zeng.mdx.commons;

namespace Zeng.mdx.parsers.w3x.doo
{
    /**
     * A random item.
     */
    public class RandomItem
    {
        public string id = "\0\0\0\0";
        public int chance = 0;

        public void Load(BinaryStream stream)
        {
            this.id = stream.ReadBinary(4);
            this.chance = stream.ReadInt32();
        }

        public void Save(BinaryStream stream)
        {
            stream.WriteBinary(this.id);
            stream.WriteInt32(this.chance);
        }
    }

}
