
using Zeng.mdx.commons;

namespace Zeng.mdx.parsers.mdx
{

    /**
     * A uint animation.
     */
    public class Vector3Animation : Animation
    {
        public override float[] ReadMdxValue(BinaryStream stream)
        {
            float[] result = stream.ReadFloat32Array(3);
            return result;
        }

        public override void WriteMdxValue(BinaryStream stream, float[] value)
        {
            stream.WriteFloat32Array(value);
        }

    }

}
