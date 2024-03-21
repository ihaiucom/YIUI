using MdxLib.ModelFormats.Mdx;

namespace Zeng.mdx.parsers.w3x.w3i
{
    public class Player
    {
        public int id = 0;
        public int type = 0;
        public int race = 0;
        public int isFixedStartPosition = 0;
        public string name = "";
        public float[] startLocation = new float[2];
        public uint allyLowPriorities = 0;
        public uint allyHighPriorities = 0;
        public byte[] unknown1 = new byte[8];

        public void load(CLoader stream, int version)
        {
            this.id = stream.ReadInt32();
            this.type = stream.ReadInt32();
            this.race = stream.ReadInt32();
            this.isFixedStartPosition = stream.ReadInt32();
            this.name = stream.ReadNull();
            stream.readFloat32Array(this.startLocation);
            this.allyLowPriorities = stream.ReadUInt32();
            this.allyHighPriorities = stream.ReadUInt32();
            if (version > 30)
            {
                stream.readUint8Array(this.unknown1);
            }
        }
    }
}
