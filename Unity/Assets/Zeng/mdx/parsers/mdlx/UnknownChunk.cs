using Zeng.mdx.commons;

namespace Zeng.mdx.parsers.mdx
{
    /**
     * An unknown chunk.
     */
    public class UnknownChunk: IMdxDynamicObject
    {
        public string tag;
        public byte[] chunk;

        public UnknownChunk(BinaryStream stream, int size, string tag)
        {
            this.tag = tag;
            this.chunk = stream.ReadUint8Array(new byte[size]);
        }

        public void ReadMdx(BinaryStream stream, int version)
        {
            throw new System.NotImplementedException();
        }

        public void WriteMdx(BinaryStream stream, int version)
        {
            stream.WriteBinary(this.tag);
            stream.WriteUint32((uint)this.chunk.Length);
            stream.WriteUint8Array(this.chunk);
        }



        public int GetByteLength(int version = 0)
        {
            return 8 + this.chunk.Length;
        }
    }

}
