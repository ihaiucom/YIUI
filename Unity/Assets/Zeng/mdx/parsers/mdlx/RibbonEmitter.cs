
using Zeng.mdx.commons;

namespace Zeng.mdx.parsers.mdx
{

    /**
     * A ribbon emitter.
     */
    public class RibbonEmitter : GenericObject
    {
        public float heightAbove = 0;
        public float heightBelow = 0;
        public float alpha = 0;
        public float[] color = new float[3];
        public float lifeSpan = 0;
        public uint textureSlot = 0;
        public uint emissionRate = 0;
        public uint rows = 0;
        public uint columns = 0;
        public int materialId = 0;
        public float gravity = 0;

        public RibbonEmitter() : base(0x4000)
        {
        }

        public override void ReadMdx(BinaryStream stream, int version)
        {
            int start = stream.Index;
            uint size = stream.ReadUint32();

            base.ReadMdx(stream, version);

            heightAbove = stream.ReadFloat32();
            heightBelow = stream.ReadFloat32();
            alpha = stream.ReadFloat32();
            stream.ReadFloat32Array(color);
            lifeSpan = stream.ReadFloat32();
            textureSlot = stream.ReadUint32();
            emissionRate = stream.ReadUint32();
            rows = stream.ReadUint32();
            columns = stream.ReadUint32();
            materialId = stream.ReadInt32();
            gravity = stream.ReadFloat32();

            ReadAnimations(stream, (int)(size - (stream.Index - start)));
        }

        public override void WriteMdx(BinaryStream stream, int version)
        {
            stream.WriteUint32((uint)GetByteLength());

            base.WriteMdx(stream, version);

            stream.WriteFloat32(heightAbove);
            stream.WriteFloat32(heightBelow);
            stream.WriteFloat32(alpha);
            stream.WriteFloat32Array(color);
            stream.WriteFloat32(lifeSpan);
            stream.WriteUint32(textureSlot);
            stream.WriteUint32(emissionRate);
            stream.WriteUint32(rows);
            stream.WriteUint32(columns);
            stream.WriteInt32(materialId);
            stream.WriteFloat32(gravity);

            WriteNonGenericAnimationChunks(stream);
        }

        public override int GetByteLength(int version = 0)
        {
            return 56 + base.GetByteLength(version);
        }
    }

}
