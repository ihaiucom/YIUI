using System;
using Zeng.mdx.commons;
namespace Zeng.mdx.parsers.w3x.unitsdoo
{
    /**
     * war3mapUnits.doo - the units and items file.
     */
    public class War3MapUnitsDoo
    {
        public int version = 8;
        public int subversion = 11;
        public Unit[] units = new Unit[] { };

        public void Load(byte[] buffer, int buildVersion)
        {
            BinaryStream stream = new BinaryStream(buffer);

            if (stream.ReadBinary(4) != "W3do")
            {
                throw new Exception("Not a valid war3mapUnits.doo buffer");
            }

            version = stream.ReadInt32();
            subversion = (int)stream.ReadUint32();

            int unitsLength = stream.ReadInt32();
            units = new Unit[unitsLength];
            for (int i = 0, l = unitsLength; i < l; i++)
            {
                Unit unit = new Unit();

                unit.Load(stream, version, subversion, buildVersion);

                units[i] = unit;
            }
        }

    }
}