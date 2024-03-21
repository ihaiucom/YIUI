
using System.IO;

namespace Zeng.mdx.commons
{
    public class UrlUtils
    {
        public static string localOrHive(string src, object _params = null)
        {
            src = src.ToLower();

            string ext =  Path.GetExtension(src).ToLower();
            if (ext == ".blp") {
                src = Path.ChangeExtension(src, ".dds");
                //return $"https://www.hiveworkshop.com/assets/wc3/war3.w3mod/{src}";
            }


            string url = $"https://www.hiveworkshop.com/casc-contents?path={src}";
            //UnityEngine.Debug.Log(url); 
            return url;
        }
    }
}
