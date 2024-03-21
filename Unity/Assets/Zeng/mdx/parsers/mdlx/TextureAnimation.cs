using System;
using Zeng.mdx.commons;

namespace Zeng.mdx.parsers.mdx
{

    /**
     * A texture animation.
     */
    public class TextureAnimation : AnimatedObject, IMdxDynamicObject
    {
        public void ReadMdx(BinaryStream stream, int version)
        {
            uint size = stream.ReadUint32();

            ReadAnimations(stream, (int)(size - 4));
        }

        public void WriteMdx(BinaryStream stream, int version)
        {
            stream.WriteUint32((uint)GetByteLength());
            WriteAnimations(stream);
        }


        public override int GetByteLength(int version = 0)
        {
            return 4 + base.GetByteLength(version);
        }
    }

}
