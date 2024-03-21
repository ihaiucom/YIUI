
using Zeng.mdx.commons;

namespace Zeng.mdx.parsers.mdx
{
    /**
     * A sequence.
     */
    public class Sequence : IMdxStaticObject
    {
        public string name = "";
        public uint[] interval = new uint[2];
        public float moveSpeed = 0;
        public uint nonLooping = 0;
        public float rarity = 0;
        public uint syncPoint = 0;
        public Extent extent = new Extent();

        public void ReadMdx(BinaryStream stream)
        {
            this.name = stream.Read(80);
            stream.ReadUint32Array(this.interval);
            this.moveSpeed = stream.ReadFloat32();
            this.nonLooping = stream.ReadUint32();
            this.rarity = stream.ReadFloat32();
            this.syncPoint = stream.ReadUint32();
            this.extent.ReadMdx(stream);
        }

        public void WriteMdx(BinaryStream stream)
        {
            stream.Skip(80 - stream.Write(this.name));
            stream.WriteUint32Array(this.interval);
            stream.WriteFloat32(this.moveSpeed);
            stream.WriteUint32(this.nonLooping);
            stream.WriteFloat32(this.rarity);
            stream.WriteUint32(this.syncPoint);
            this.extent.WriteMdx(stream);
        }

    }

}
