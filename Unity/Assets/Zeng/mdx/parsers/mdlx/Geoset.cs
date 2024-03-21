

using System.Collections.Generic;
using Zeng.mdx.commons;

namespace Zeng.mdx.parsers.mdx
{

    /**
     * A geoset.
     */
    public class Geoset: IMdxDynamicObject
    {
        public float[] vertices = new float[0];
        public float[] normals = new float[0];
        public uint[] faceTypeGroups = new uint[0];
        public uint[] faceGroups = new uint[0];
        public ushort[] faces = new ushort[0];
        public byte[] vertexGroups = new byte[0];
        public uint[] matrixGroups = new uint[0];
        public uint[] matrixIndices = new uint[0];
        public uint materialId = 0;
        public uint selectionGroup = 0;
        public uint selectionFlags = 0;
        /** 
         * @since 900
         */
        public int lod = -1;
        /** 
         * @since 900
         */
        public string lodName = "";
        public Extent extent = new Extent();
        public List<Extent> sequenceExtents = new List<Extent>();
        /** 
         * @since 900
         */
        public float[] tangents = new float[0];
        /**
         * An array of bone indices and weights.
         * Every 8 consecutive elements describe the following:
         *    [B0, B1, B2, B3, W0, W1, W2, W3]
         * Where:
         *     Bn is a bone index.
         *     Wn is a weight, which can be normalized with Wn/255.
         *
         * @since 900
         */
        public byte[] skin = new byte[0];
        public List<float[]> uvSets = new List<float[]>();

        public void ReadMdx(BinaryStream stream, int version)
        {
            stream.ReadUint32(); // Don't care about the size.

            stream.Skip(4); // VRTX
            vertices = stream.ReadFloat32Array((int)(stream.ReadUint32() * 3));
            stream.Skip(4); // NRMS
            normals = stream.ReadFloat32Array((int)(stream.ReadUint32() * 3));
            stream.Skip(4); // PTYP
            faceTypeGroups = stream.ReadUint32Array((int)stream.ReadUint32());
            stream.Skip(4); // PCNT
            faceGroups = stream.ReadUint32Array((int)stream.ReadUint32());
            stream.Skip(4); // PVTX
            faces = stream.ReadUint16Array((int)stream.ReadUint32());
            stream.Skip(4); // GNDX
            vertexGroups = stream.ReadUint8Array((int)stream.ReadUint32());
            stream.Skip(4); // MTGC
            matrixGroups = stream.ReadUint32Array((int)stream.ReadUint32());
            stream.Skip(4); // MATS
            matrixIndices = stream.ReadUint32Array((int)stream.ReadUint32());
            materialId = stream.ReadUint32();
            selectionGroup = stream.ReadUint32();
            selectionFlags = stream.ReadUint32();

            if (version > 800)
            {
                lod = stream.ReadInt32();
                lodName = stream.Read(80);
            }

            extent.ReadMdx(stream);

            for (int i = 0, l = (int)stream.ReadUint32(); i < l; i++)
            {
                Extent extent = new Extent();

                extent.ReadMdx(stream);

                sequenceExtents.Add(extent);
            }

            // Non-reforged models that come with reforged are saved with version >800, however they don't have TANG and SKIN.
            if (version > 800)
            {
                if (stream.ReadBinary(4) == "TANG")
                {
                    tangents = stream.ReadFloat32Array((int)(stream.ReadUint32() * 4));
                }
                else
                {
                    stream.Skip(-4);
                }

                if (stream.ReadBinary(4) == "SKIN")
                {
                    skin = stream.ReadUint8Array((int)stream.ReadUint32());
                }
                else
                {
                    stream.Skip(-4);
                }
            }

            stream.Skip(4); // UVAS

            for (int i = 0, l = (int)stream.ReadUint32(); i < l; i++)
            {
                stream.Skip(4); // UVBS
                uvSets.Add(stream.ReadFloat32Array((int)(stream.ReadUint32() * 2)));
            }
        }

        public void WriteMdx(BinaryStream stream, int version)
        {
            stream.WriteUint32((uint)GetByteLength(version));
            stream.WriteBinary("VRTX");
            stream.WriteUint32((uint)(vertices.Length / 3));
            stream.WriteFloat32Array(vertices);
            stream.WriteBinary("NRMS");
            stream.WriteUint32((uint)(normals.Length / 3));
            stream.WriteFloat32Array(normals);
            stream.WriteBinary("PTYP");
            stream.WriteUint32((uint)faceTypeGroups.Length);
            stream.WriteUint32Array(faceTypeGroups);
            stream.WriteBinary("PCNT");
            stream.WriteUint32((uint)faceGroups.Length);
            stream.WriteUint32Array(faceGroups);
            stream.WriteBinary("PVTX");
            stream.WriteUint32((uint)faces.Length);
            stream.WriteUint16Array(faces);
            stream.WriteBinary("GNDX");
            stream.WriteUint32((uint)vertexGroups.Length);
            stream.WriteUint8Array(vertexGroups);
            stream.WriteBinary("MTGC");
            stream.WriteUint32((uint)matrixGroups.Length);
            stream.WriteUint32Array(matrixGroups);
            stream.WriteBinary("MATS");
            stream.WriteUint32((uint)matrixIndices.Length);
            stream.WriteUint32Array(matrixIndices);
            stream.WriteUint32(materialId);
            stream.WriteUint32(selectionGroup);
            stream.WriteUint32(selectionFlags);

            if (version > 800)
            {
                stream.WriteInt32(lod);
                stream.Skip(80 - stream.Write(lodName));
            }

            extent.WriteMdx(stream);

            stream.WriteUint32((uint)sequenceExtents.Count);

            foreach (var sequenceExtent in sequenceExtents)
            {
                sequenceExtent.WriteMdx(stream);
            }

            if (version > 800)
            {
                if (tangents.Length > 0)
                {
                    stream.WriteBinary("TANG");
                    stream.WriteUint32((uint)(tangents.Length / 4));
                    stream.WriteFloat32Array(tangents);
                }

                if (skin.Length > 0)
                {
                    stream.WriteBinary("SKIN");
                    stream.WriteUint32((uint)skin.Length);
                    stream.WriteUint8Array(skin);
                }
            }

            stream.WriteBinary("UVAS");
            stream.WriteUint32((uint)uvSets.Count);

            foreach (var uvSet in uvSets)
            {
                stream.WriteBinary("UVBS");
                stream.WriteUint32((uint)(uvSet.Length / 2));
                stream.WriteFloat32Array(uvSet);
            }
        }

        public int GetByteLength(int version)
        {
            int size = 120 + vertices.Length * 4 + normals.Length * 4 + faceTypeGroups.Length * 4 + faceGroups.Length * 4 + faces.Length * 2 + vertexGroups.Length + matrixGroups.Length * 4 + matrixIndices.Length * 4 + sequenceExtents.Count * 28;

            if (version > 800)
            {
                size += 84;

                if (tangents.Length > 0)
                {
                    size += 8 + tangents.Length * 4;
                }

                if (skin.Length > 0)
                {
                    size += 8 + skin.Length;
                }
            }

            foreach (var uvSet in uvSets)
            {
                size += 8 + uvSet.Length * 4;
            }

            return size;
        }


    }
}
