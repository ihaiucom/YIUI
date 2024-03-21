
using System;
using System.Collections;
using System.IO;
using MdxLib.ModelFormats.Mdx;
using UnityEngine;
using Zeng.mdx.parsers.mpq;
using Zeng.mdx.parsers.w3x.imp;

namespace Zeng.mdx.parsers.w3x {

    public class War3Map
    {
        public int u1 = 0;
        public string name = "";
        public int flags = 0;
        public int maxPlayers = 0;

        public MpqArchive archive = new MpqArchive();
        public War3MapImp imports = new War3MapImp();

        public bool isReadonly = false;




        public void ReadFile(string path)
        {
            try
            {
                using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    Load(stream, false, path);
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }

        public void Load(Stream Stream, bool isReadonly = false, string name = "")
        {
            Stream.Position = 0;
            MemoryStream ms = new MemoryStream();
            Stream.CopyTo(ms);
            byte[] bytes = ms.ToArray();
            load(bytes, isReadonly, name);
            ms.Dispose();

        }

        public void load(byte[] buffer, bool isReadonly = false, string name = "" )
        {

            MemoryStream ms = new MemoryStream(buffer);
            ms.Position = 0;
            CLoader cloader = new CLoader(name, ms);
            if (cloader.ReadString(4) == "HM3W")
            {
                this.u1 = cloader.ReadInt32();
                this.name = cloader.ReadNull();
                this.flags = cloader.ReadInt32();
                this.maxPlayers = cloader.ReadInt32();
            }

            this.isReadonly = isReadonly;
            this.archive.load(buffer, ms, cloader, isReadonly);

            // Read in the imports file if there is one.
            this.readImports();
            ms.Dispose();
        }

        private void readImports()
        {

            MpqFile file = this.archive.get("war3map.imp");

            if (file != null)
            {
                byte[] buffer = file.arrayBuffer();

                if (buffer != null)
                {
                    this.imports.load(buffer);
                }
            }
        }

        public MpqFile get(string name)
        {
            return this.archive.get(name);
        }


#if UNITY_EDITOR
        public IEnumerator SaveUnity(string savePath = "Assets/Prefabs/")
        {

            yield return null;
        }
#endif

    }

}