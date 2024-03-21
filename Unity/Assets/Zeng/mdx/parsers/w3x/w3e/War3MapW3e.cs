using MdxLib.ModelFormats.Mdx;
using System.Collections.Generic;
using System.IO;

namespace Zeng.mdx.parsers.w3x.w3e
{
    public class War3MapW3e
    {
        public int version = 0;
        public char tileset = 'A';
        public int haveCustomTileset = 0;
        public string[] groundTilesets = new string[] { };
        public string[] cliffTilesets = new string[] { };
        public int[] mapSize = new int[2];
        public float[] centerOffset = new float[2];
        public List<List<Corner>> corners = new List<List<Corner>>();

        public void load(byte[] buffer)
        {

            MemoryStream ms = new MemoryStream(buffer);
            CLoader stream = new CLoader("", ms);
            ms.Position = 0;


            if (stream.ReadString(4) != "W3E!")
            {
                return;
            }

            this.version = stream.ReadInt32();
            this.tileset = stream.ReadString(1)[0];
            this.haveCustomTileset = stream.ReadInt32();
            // this.tileset = 'F'

            this.groundTilesets = new string[stream.ReadInt32()];
            for (int i = 0, l = this.groundTilesets.Length; i < l; i++)
            {
                this.groundTilesets[i] = stream.ReadString(4);
            }

            this.cliffTilesets = new string[stream.ReadInt32()];
            for (int i = 0, l = this.cliffTilesets.Length; i < l; i++)
            {
                this.cliffTilesets[i] = stream.ReadString(4);
            }

            stream.readInt32Array(this.mapSize);
            stream.readFloat32Array(this.centerOffset);

            for (int row = 0, rows = this.mapSize[1]; row < rows; row++)
            {
                this.corners.Add(new List<Corner>());

                for (int column = 0, columns = this.mapSize[0]; column < columns; column++)
                {
                    Corner corner = new Corner();

                    corner.load(stream);

                    this.corners[row].Add(corner);
                }
            }
        }
    }
}
