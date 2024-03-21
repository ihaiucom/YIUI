using MdxLib.ModelFormats.Mdx;
using System.Collections.Generic;
using System.IO;

namespace Zeng.mdx.parsers.w3x.imp
{
    public class War3MapImp
    {
        public int version = 1;
        public Dictionary<string, Import> entries = new Dictionary<string, Import>();

        public void load(byte[] buffer)
        {
            MemoryStream ms = new MemoryStream(buffer);
            CLoader cloader = new CLoader("", ms);
            ms.Position = 0;
            this.version = cloader.ReadInt32();
            int len = cloader.ReadInt32();


            for (int i = 0, l = len; i < l; i++)
            {
                Import entry = new Import();
                entry.load(ms, cloader);

                if (entry.isCustom != 0)
                {
                    UnityEngine.Debug.Log("Import:" +  entry.path );
                    this.entries.Add(entry.path, entry);
                }
                else
                {
                    this.entries.Add($"war3mapimported\\{ entry.path}", entry);
                }
            }
            ms.Dispose();

        }
    }
}
