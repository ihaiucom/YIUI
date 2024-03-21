using Zeng.mdx.commons;

namespace Zeng.mdx.parsers.mdx
{
    public interface IMdxDynamicObject
    {
        void ReadMdx(BinaryStream stream, int version);
        void WriteMdx(BinaryStream stream, int version);
        int GetByteLength(int version = 0);
    }
}
