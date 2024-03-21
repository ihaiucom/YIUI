using Zeng.mdx.commons;

namespace Zeng.mdx.parsers.mdx
{

    /**
     * A geoset animation.
     */
    public class GeosetAnimation : AnimatedObject, IMdxDynamicObject
    {
        public float alpha = 1;
        public uint flags = 0;
        public float[] color = new float[] { 1, 1, 1 };
        public int geosetId = -1;



        // GeosetAnimation
        public Animation Alpha { get { return animationMap.ContainsKey("KGAO") ? animationMap["KGAO"] : null; } }
        public Animation Color { get { return animationMap.ContainsKey("KGAC") ? animationMap["KGAC"] : null; } }

        public void ReadMdx(BinaryStream stream, int version)
        {
            uint size = stream.ReadUint32();

            alpha = stream.ReadFloat32();
            flags = stream.ReadUint32();
            stream.ReadFloat32Array(color);
            geosetId = stream.ReadInt32();

            ReadAnimations(stream, (int)(size - 28));
        }

        public void WriteMdx(BinaryStream stream, int version)
        {
            stream.WriteUint32((uint)GetByteLength());
            stream.WriteFloat32(alpha);
            stream.WriteUint32(flags);
            stream.WriteFloat32Array(color);
            stream.WriteInt32(geosetId);

            WriteAnimations(stream);
        }

        public override int GetByteLength(int version = 0)
        {
            return 28 + base.GetByteLength(version);
        }
    }

}
