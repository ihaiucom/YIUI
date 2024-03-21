
using Zeng.mdx.commons;

namespace Zeng.mdx.parsers.mdx
{
    /**
     * A popcorn particle emitter.
     * References a pkfx file, which is used by the PopcornFX runtime.
     *
     * @since 900
     */
    public class ParticleEmitterPopcorn : GenericObject
    {
        public enum Flags
        {
            Unshaded = 0x8000,
            SortPrimsFarZ = 0x10000,
            Unfogged = 0x40000
        }

        public float lifeSpan = 0;
        public float emissionRate = 0;
        public float speed = 0;
        public float[] color = new float[3];
        public float alpha = 1;
        public uint replaceableId = 0;
        public string path = "";
        public string animationVisiblityGuide = "";

        public override void ReadMdx(BinaryStream stream, int version)
        {
            int start = stream.Index;
            uint size = stream.ReadUint32();

            base.ReadMdx(stream, version);

            this.lifeSpan = stream.ReadFloat32();
            this.emissionRate = stream.ReadFloat32();
            this.speed = stream.ReadFloat32();
            stream.ReadFloat32Array(this.color);
            this.alpha = stream.ReadFloat32();
            this.replaceableId = stream.ReadUint32();
            this.path = stream.Read(260);
            this.animationVisiblityGuide = stream.Read(260);

            this.ReadAnimations(stream, (int)(size - (stream.Index - start)));

            UnityEngine.Debug.Log($"ParticleEmitterPopcorn: path={path}");
        }

        public override void WriteMdx(BinaryStream stream, int version)
        {
            stream.WriteUint32((uint)this.GetByteLength());

            base.WriteMdx(stream, version);

            stream.WriteFloat32(this.lifeSpan);
            stream.WriteFloat32(this.emissionRate);
            stream.WriteFloat32(this.speed);
            stream.WriteFloat32Array(this.color);
            stream.WriteFloat32(this.alpha);
            stream.WriteUint32(this.replaceableId);
            stream.Skip(260 - stream.Write(this.path));
            stream.Skip(260 - stream.Write(this.animationVisiblityGuide));

            this.WriteNonGenericAnimationChunks(stream);
        }

        public override int GetByteLength(int version = 0)
        {
            return 556 + base.GetByteLength(version);
        }
    }

}
