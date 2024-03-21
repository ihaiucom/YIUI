using System;
using Zeng.mdx.commons;

namespace Zeng.mdx.parsers.w3x.doo
{
    /**
     * war3map.doo - the doodad and destructible file.
     */
    public class War3MapDoo
    {
        public int version = 0;
        public byte[] u1 = new byte[4];
        public Doodad[] doodads = new Doodad[0];
        public byte[] u2 = new byte[4];
        public TerrainDoodad[] terrainDoodads = new TerrainDoodad[0];

        public void Load(byte[] buffer, int buildVersion)
        {
            BinaryStream stream = new BinaryStream(buffer);

            if (stream.ReadBinary(4) != "W3do")
            {
                throw new Exception("Not a valid war3map.doo buffer");
            }

            version = stream.ReadInt32();
            stream.ReadUint8Array(u1);

            int doodadCount = stream.ReadInt32();
            doodads = new Doodad[doodadCount];

            for (int i = 0; i < doodadCount; i++)
            {
                Doodad doodad = new Doodad();
                doodad.Load(stream, version, buildVersion);
                doodads[i] = doodad;
            }

            stream.ReadUint8Array(u2);

            int terrainDoodadCount = stream.ReadInt32();
            terrainDoodads = new TerrainDoodad[terrainDoodadCount];

            for (int i = 0; i < terrainDoodadCount; i++)
            {
                TerrainDoodad terrainDoodad = new TerrainDoodad();
                terrainDoodad.Load(stream, version);
                terrainDoodads[i] = terrainDoodad;
            }
        }

        public byte[] Save(int buildVersion)
        {
            BinaryStream stream = new BinaryStream(new byte[GetByteLength(buildVersion)]);

            stream.WriteBinary("W3do");
            stream.WriteInt32(version);
            stream.WriteUint8Array(u1);
            stream.WriteUint32((uint)doodads.Length);

            foreach (Doodad doodad in doodads)
            {
                doodad.Save(stream, version, buildVersion);
            }

            stream.WriteUint8Array(u2);
            stream.WriteUint32((uint)terrainDoodads.Length);

            foreach (TerrainDoodad terrainDoodad in terrainDoodads)
            {
                terrainDoodad.Save(stream, version);
            }

            return stream.Uint8array;
        }

        public int GetByteLength(int buildVersion)
        {
            int size = 24 + terrainDoodads.Length * 16;

            foreach (Doodad doodad in doodads)
            {
                size += doodad.GetByteLength(version, buildVersion);
            }

            return size;
        }
    }
}
