
using Zeng.mdx.commons;

namespace Zeng.mdx.parsers.mdx
{

    /**
     * A uint animation.
     */
    public class FloatAnimation : Animation
    {
        public override float[] ReadMdxValue(BinaryStream stream)
        {
            float[] result = stream.ReadFloat32Array(1);
            return result;
        }

        public override void WriteMdxValue(BinaryStream stream, float[] value)
        {
            stream.WriteFloat32(value[0]);
        }

    }

}
