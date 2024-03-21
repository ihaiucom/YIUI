
using Zeng.mdx.commons;

namespace Zeng.mdx.parsers.mdx
{
    /**
     * An extent.
     */
    public class Extent
    {
        public float boundsRadius = 0;
        public float[] min = new float[3];
        public float[] max = new float[3];

        public void ReadMdx(BinaryStream stream)
        {
            boundsRadius = stream.ReadFloat32();
            stream.ReadFloat32Array(min);
            stream.ReadFloat32Array(max);
        }

        public void WriteMdx(BinaryStream stream)
        {
            stream.WriteFloat32(boundsRadius);
            stream.WriteFloat32Array(min);
            stream.WriteFloat32Array(max);
        }
    }

}
