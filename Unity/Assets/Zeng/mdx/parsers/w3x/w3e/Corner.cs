using MdxLib.ModelFormats.Mdx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zeng.mdx.parsers.w3x.w3e
{
    public class Corner
    {
        public float groundHeight = 0;
        public float waterHeight = 0;
        public int mapEdge = 0;
        public int ramp = 0;
        public int blight = 0;
        public int water = 0;
        public int boundary = 0;
        public int groundTexture = 0;
        public int cliffVariation = 0;
        public int groundVariation = 0;
        public int cliffTexture = 0;
        public int layerHeight = 0;

        public void load(CLoader stream)
        {
            this.groundHeight = 1f * (stream.ReadInt16() - 8192) / 512;

            int waterAndEdge = stream.ReadInt16();

            this.waterHeight = (1f * (waterAndEdge & 0x3FFF) - 8192) / 512;
            this.mapEdge = waterAndEdge & 0x4000;

            int textureAndFlags = stream.ReadByte();

            this.ramp = textureAndFlags & 0b00010000;
            this.blight = textureAndFlags & 0b00100000;
            this.water = textureAndFlags & 0b01000000;
            this.boundary = textureAndFlags & 0b10000000;

            this.groundTexture = textureAndFlags & 0b00001111;

            int variation = stream.ReadByte();

            this.cliffVariation = (variation & 0b11100000) >> 5;
            this.groundVariation = variation & 0b00011111;

            int cliffTextureAndLayer = stream.ReadByte();

            this.cliffTexture = (cliffTextureAndLayer & 0b11110000) >> 4;
            this.layerHeight = cliffTextureAndLayer & 0b00001111;
        }
    }
}
