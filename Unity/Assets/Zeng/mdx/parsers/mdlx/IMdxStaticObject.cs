using Zeng.mdx.commons;

namespace Zeng.mdx.parsers.mdx
{
    public interface IMdxStaticObject
    {
        void ReadMdx(BinaryStream stream);
        void WriteMdx(BinaryStream stream);
    }
}
