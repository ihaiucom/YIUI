

namespace Zeng.mdx.parsers.mpq
{
    public class MqlInflate
    {
        public static byte[] inflate(byte[] bytes)
        {
            return GZipUtils.Decompress(bytes);
        }
    }
}
