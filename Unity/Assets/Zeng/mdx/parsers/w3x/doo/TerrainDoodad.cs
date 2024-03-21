

using System;
using Zeng.mdx.commons;

namespace Zeng.mdx.parsers.w3x.doo
{
    /**
     * 地形装饰物。
     *
     * 这种类型的装饰物与悬崖非常相似。
     * 它使用地形的高度，并受地面高度图的影响。
     * 一旦放置，它无法在世界编辑器中以任何方式进行操作。
     * 实际上，唯一改变它的方法是通过改变周围的悬崖来移除它。
     */
    [Serializable]
    public class TerrainDoodad
    {
        public string id = "\0\0\0\0";
        public uint u1 = 0;
        public uint[] location = new uint[2];

        public void Load(BinaryStream stream, int version)
        {
            id = stream.ReadBinary(4);
            u1 = stream.ReadUint32();
            stream.ReadUint32Array(location);
        }

        public void Save(BinaryStream stream, int version)
        {
            stream.WriteBinary(id);
            stream.WriteUint32(u1);
            stream.WriteUint32Array(location);
        }
    }

}
