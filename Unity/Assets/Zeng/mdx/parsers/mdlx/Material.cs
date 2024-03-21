using System;
using System.Collections.Generic;
using Zeng.mdx.commons;
namespace Zeng.mdx.parsers.mdx
{


    /// <summary>
    /// A material.
    /// </summary>
    public class Material : IMdxDynamicObject
    {
        public enum MaterailFlags
        {
            None = 0x0,
            ConstantColor = 0x1,
            TwoSided = 0x2,
            SortPrimsNearZ = 0x8,
            SortPrimsFarZ = 0x10,
            FullResolution = 0x20
        }


        public int priorityPlane = 0;
        public MaterailFlags flags = MaterailFlags.None;
        /// <summary>
        /// Since 900
        /// </summary>
        public string shader = "";
        public List<Layer> layers = new List<Layer>();

        public void ReadMdx(BinaryStream stream, int version)
        {
            stream.ReadUint32(); // Don't care about the size.

            priorityPlane = stream.ReadInt32();
            flags = (MaterailFlags)stream.ReadUint32();

            if (version > 800)
            {
                shader = stream.Read(80);
            }

            stream.Skip(4); // LAYS

            for (int i = 0, l = (int)stream.ReadUint32(); i < l; i++)
            {
                Layer layer = new Layer();

                layer.ReadMdx(stream, version);

                layers.Add(layer);
            }
        }

        public void WriteMdx(BinaryStream stream, int version)
        {
            stream.WriteUint32((uint)GetByteLength(version));
            stream.WriteInt32(priorityPlane);
            stream.WriteUint32((uint)flags);

            if (version > 800)
            {
                stream.Skip(80 - stream.Write(shader));
            }

            stream.WriteBinary("LAYS");
            stream.WriteUint32((uint)layers.Count);

            foreach (Layer layer in layers)
            {
                layer.WriteMdx(stream, version);
            }
        }

        public int GetByteLength(int version)
        {
            int size = 20;

            if (version > 800)
            {
                size += 80;
            }

            foreach (Layer layer in layers)
            {
                size += layer.GetByteLength(version);
            }

            return size;
        }
    }

}
