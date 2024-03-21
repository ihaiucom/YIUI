using Zeng.mdx.commons;

namespace Zeng.mdx.parsers.mdx
{
    /**
     * A bone.
     */
    public class Bone : GenericObject
    {
        public int geosetId = -1;
        public int geosetAnimationId = -1;

        public Bone() : base(0x100)
        {
        }

        public override void ReadMdx(BinaryStream stream, int version)
        {
            base.ReadMdx(stream, version);

            this.geosetId = stream.ReadInt32();
            this.geosetAnimationId = stream.ReadInt32();
        }

        public override void WriteMdx(BinaryStream stream, int version)
        {
            base.WriteMdx(stream, version);

            stream.WriteInt32(this.geosetId);
            stream.WriteInt32(this.geosetAnimationId);
        }



        public override int GetByteLength(int version = 0)
        {
            return 8 + base.GetByteLength(version);
        }
    }

}
