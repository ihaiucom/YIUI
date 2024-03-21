

using System.IO;

namespace Zeng.mdx.commons
{
    public class MdxUnityResPathDefine
    {
        public static string Root = "Assets/Zeng/mdx_res/";

        public static string GetPath(string path)
        {
            string ext = Path.GetExtension(path).ToLower();
            if (ext == ".blp")
            {
                path = Path.ChangeExtension(path, ".dds");
            }

            return Root + path;
        }
    }
}
