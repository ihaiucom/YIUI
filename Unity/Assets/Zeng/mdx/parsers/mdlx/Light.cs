using Zeng.mdx.commons;

namespace Zeng.mdx.parsers.mdx
{

    /**
     * A light.
     */
    public class Light : GenericObject
    {

        public enum LightType
        {
            None = -1,
            Omnidirectional = 0,
            Directional = 1,
            Ambient = 2
        }

        public LightType type = LightType.None;
        public float[] attenuation = new float[2];
        public float[] color = new float[3];
        public float intensity = 0;
        public float[] ambientColor = new float[3];
        public float ambientIntensity = 0;

        public Light() : base(0x200)
        {
        }

        public override void ReadMdx(BinaryStream stream, int version)
        {
            int start = stream.Index;
            int size = (int)stream.ReadUint32();

            base.ReadMdx(stream, version);

            type = (LightType)stream.ReadUint32();
            stream.ReadFloat32Array(attenuation);
            stream.ReadFloat32Array(color);
            intensity = stream.ReadFloat32();
            stream.ReadFloat32Array(ambientColor);
            ambientIntensity = stream.ReadFloat32();

            ReadAnimations(stream, size - (stream.Index - start));
        }

        public override void WriteMdx(BinaryStream stream, int version)
        {
            stream.WriteUint32((uint)GetByteLength());

            base.WriteMdx(stream, version);

            stream.WriteUint32((uint)type);
            stream.WriteFloat32Array(attenuation);
            stream.WriteFloat32Array(color);
            stream.WriteFloat32(intensity);
            stream.WriteFloat32Array(ambientColor);
            stream.WriteFloat32(ambientIntensity);

            WriteNonGenericAnimationChunks(stream);
        }

        public override int GetByteLength(int version = 0)
        {
            return 48 + base.GetByteLength(version);
        }
    }

}
