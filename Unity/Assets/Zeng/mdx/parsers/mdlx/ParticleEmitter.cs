using Zeng.mdx.commons;

namespace Zeng.mdx.parsers.mdx
{


    /**
     * A particle emitter.
     */
    public class ParticleEmitter : GenericObject
    {

        public enum Flags
        {
            EmitterUsesMDL = 0x8000,
            EmitterUsesTGA = 0x10000
        }

        public float emissionRate = 0;
        public float gravity = 0;
        public float longitude = 0;
        public float latitude = 0;
        public string path = "";
        public float lifeSpan = 0;
        public float speed = 0;

        public ParticleEmitter() : base(0x1000)
        {
        }

        public override void ReadMdx(BinaryStream stream, int version)
        {
            int start = stream.Index;
            int size = (int)stream.ReadUint32();

            base.ReadMdx(stream, version);

            this.emissionRate = stream.ReadFloat32();
            this.gravity = stream.ReadFloat32();
            this.longitude = stream.ReadFloat32();
            this.latitude = stream.ReadFloat32();
            this.path = stream.Read(260);
            this.lifeSpan = stream.ReadFloat32();
            this.speed = stream.ReadFloat32();

            this.ReadAnimations(stream, size - (stream.Index - start));

            UnityEngine.Debug.Log($"ParticleEmitter: path={path}");
        }

        public override void WriteMdx(BinaryStream stream, int version)
        {
            stream.WriteUint32((uint)this.GetByteLength());

            base.WriteMdx(stream, version);

            stream.WriteFloat32(this.emissionRate);
            stream.WriteFloat32(this.gravity);
            stream.WriteFloat32(this.longitude);
            stream.WriteFloat32(this.latitude);
            stream.Skip(260 - stream.Write(this.path));
            stream.WriteFloat32(this.lifeSpan);
            stream.WriteFloat32(this.speed);

            this.WriteNonGenericAnimationChunks(stream);
        }

        public override int GetByteLength(int version = 0)
        {
            return 288 + base.GetByteLength(version);
        }
    }
}
