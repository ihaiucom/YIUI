using MdxLib.ModelFormats.Mdx;


namespace Zeng.mdx.parsers.w3x.w3i
{
    public class RandomUnitTable
    {

        public int id = 0;
        public string name = "";
        public int positions = 0;
        public int[] columnTypes = new int[0];
        public RandomUnit[] units = new RandomUnit[0];

        public void load(CLoader stream)
        {
            this.id = stream.ReadInt32();
            this.name = stream.ReadNull();
            this.positions = stream.ReadInt32();
            this.columnTypes = new int[this.positions];
            this.columnTypes = stream.readInt32Array(this.columnTypes);

            this.units = new RandomUnit[stream.ReadUInt32()];
            for (int i = 0, l = units.Length; i < l; i++)
            {
                RandomUnit unit = new RandomUnit();

                unit.load(stream, this.positions);

                this.units[i] = unit;
            }
        }
    }
}
