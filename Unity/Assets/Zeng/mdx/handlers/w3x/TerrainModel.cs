

namespace Zeng.mdx.handlers.w3x
{
    public class TerrainModel
    {
        public War3MapViewerMap map;
        public CliffItem cliffItem;
        public string mdxPath;
        public byte[] mdx;

        public TerrainModel(War3MapViewerMap map, CliffItem cliffItem, string mdxPath, byte[] mdx) {
            this.map = map;
            this.cliffItem = cliffItem;
            this.mdxPath = mdxPath;
            this.mdx = mdx;
               
        }
    }
}
