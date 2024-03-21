
using Zeng.mdx.commons;

namespace Zeng.mdx.parsers.mdx
{

    /**
     * A uint animation.
     */
    public class UintAnimation : Animation
    {
        public override float[] ReadMdxValue(BinaryStream stream)
        {
            uint[] arr = stream.ReadUint32Array(1);
            float[] result = new float[arr.Length];
            for(int i = 0; i < arr.Length; i ++)
            {
                result[i] = arr[i];
            }
            return result;
        }

        public override void WriteMdxValue(BinaryStream stream, float[] value)
        {
            stream.WriteUint32((uint)value[0]);
        }

    }

}
