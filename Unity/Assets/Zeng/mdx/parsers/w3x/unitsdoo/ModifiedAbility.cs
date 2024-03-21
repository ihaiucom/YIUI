
using Zeng.mdx.commons;

namespace Zeng.mdx.parsers.w3x.unitsdoo
{
    /**
     * A modified ability.
     */
    public class ModifiedAbility
    {
        private string id = "\0\0\0\0";
        private int activeForAutocast = 0;
        private int heroLevel = 1;

        public void Load(BinaryStream stream)
        {
            id = stream.ReadBinary(4);
            activeForAutocast = stream.ReadInt32();
            heroLevel = stream.ReadInt32();
        }

        public void Save(BinaryStream writer)
        {
            writer.WriteBinary(id);
            writer.WriteInt32(activeForAutocast);
            writer.WriteInt32(heroLevel);
        }
    }
}