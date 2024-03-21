using MdxLib.ModelFormats.Mdx;

namespace Zeng.mdx.parsers.w3x.w3i
{
    public class UpgradeAvailabilityChange
    {
        public uint playerFlags = 0;
        public string id = "\0\0\0\0";
        public int levelAffected = 0;
        public int availability = 0;

        public void load(CLoader stream)
        {
            this.playerFlags = stream.ReadUInt32();
            this.id = stream.ReadString(4);
            this.levelAffected = stream.ReadInt32();
            this.availability = stream.ReadInt32();
        }
    }
}
