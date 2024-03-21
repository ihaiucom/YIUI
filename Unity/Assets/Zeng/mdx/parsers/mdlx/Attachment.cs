using Zeng.mdx.commons;

namespace Zeng.mdx.parsers.mdx
{


    /**
     * An attachment.
     */
    public class Attachment : GenericObject
    {
        public string path = "";
        public int attachmentId = 0;

        // Attachment
        public Animation Visibility { get { return animationMap.ContainsKey("KATV") ? animationMap["KATV"] : null; } }

        public Attachment() : base(0x800)
        {
        }

        public override void ReadMdx(BinaryStream stream, int version)
        {
            int start = stream.Index;
            int size = (int)stream.ReadUint32();

            base.ReadMdx(stream, version);

            this.path = stream.Read(260);
            this.attachmentId = stream.ReadInt32();

            this.ReadAnimations(stream, size - (stream.Index - start));
        }

        public override void WriteMdx(BinaryStream stream, int version)
        {
            stream.WriteUint32((uint)this.GetByteLength(version));

            base.WriteMdx(stream, version);

            stream.Skip(260 - stream.Write(this.path));
            stream.WriteInt32(this.attachmentId);

            this.WriteNonGenericAnimationChunks(stream);
        }

        public override int GetByteLength(int version = 0)
        {
            return 268 + base.GetByteLength(version);
        }
    }
}
