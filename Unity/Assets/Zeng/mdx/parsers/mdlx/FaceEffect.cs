
using Zeng.mdx.commons;

namespace Zeng.mdx.parsers.mdx
{
    /**
     * A face effect.
     */
    public class FaceEffect : IMdxStaticObject
    {
        public string type = "";
        public string path = "";

        public void ReadMdx(BinaryStream stream)
        {
            type = stream.Read(80);
            path = stream.Read(260);
        }

        public void WriteMdx(BinaryStream stream)
        {
            stream.Skip(80 - stream.Write(type));
            stream.Skip(260 - stream.Write(path));
        }
    }

}
