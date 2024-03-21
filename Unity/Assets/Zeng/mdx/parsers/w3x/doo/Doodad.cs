using System;
using System.Collections.Generic;
using Zeng.mdx.commons;

namespace Zeng.mdx.parsers.w3x.doo
{
    /**
     * A doodad. 摆设
     */
    [Serializable]
    public class Doodad
    {
        public string id = "\0\0\0\0";
        public int variation = 0;
        public float[] location = new float[3];
        public float angle = 0;
        public float[] scale = new float[] { 1, 1, 1 };
        /**
         * @since Game version 1.32
         */
        public string skin = "\0\0\0\0";
        public byte flags = 0;
        public byte life = 0;
        public int itemTable = -1;
        public List<RandomItemSet> itemSets = new List<RandomItemSet>();
        public int editorId = 0;
        public byte[] u1 = new byte[8];

        public void Load(BinaryStream stream, int version, int buildVersion)
        {
            id = stream.ReadBinary(4);
            variation = stream.ReadInt32();
            stream.ReadFloat32Array(location);
            angle = stream.ReadFloat32();
            stream.ReadFloat32Array(scale);

            if (buildVersion > 131)
            {
                skin = stream.ReadBinary(4);
            }

            flags = stream.ReadUint8();
            life = stream.ReadUint8();

            if (version > 7)
            {
                itemTable = (int)stream.ReadUint32();

                for (int i = 0, l = (int)stream.ReadUint32(); i < l; i++)
                {
                    RandomItemSet itemSet = new RandomItemSet();

                    itemSet.Load(stream);

                    itemSets.Add(itemSet);
                }
            }

            editorId = stream.ReadInt32();
        }

        public void Save(BinaryStream stream, int version, int buildVersion)
        {
            stream.WriteBinary(id);
            stream.WriteInt32(variation);
            stream.WriteFloat32Array(location);
            stream.WriteFloat32(angle);
            stream.WriteFloat32Array(scale);

            if (buildVersion > 131)
            {
                stream.WriteBinary(skin);
            }

            stream.WriteUint8(flags);
            stream.WriteUint8(life);

            if (version > 7)
            {
                stream.WriteUint32((uint)itemTable);
                stream.WriteUint32((uint)itemSets.Count);

                foreach (RandomItemSet itemSet in itemSets)
                {
                    itemSet.Save(stream);
                }
            }

            stream.WriteInt32(editorId);
        }

        public int GetByteLength(int version, int buildVersion)
        {
            int size = 42;

            if (buildVersion > 131)
            {
                size += 4;
            }

            if (version > 7)
            {
                size += 8;

                foreach (RandomItemSet itemSet in itemSets)
                {
                    size += itemSet.GetByteLength();
                }
            }

            return size;
        }
    }


}
