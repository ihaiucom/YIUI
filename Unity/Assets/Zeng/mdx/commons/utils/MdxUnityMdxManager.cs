using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Zeng.mdx.commons
{
    public class MdxUnityMdxManager
    {
        private static MdxUnityMdxManager _I;
        public static MdxUnityMdxManager I
        {
            get
            {
                if (_I == null)
                {
                    _I = new MdxUnityMdxManager();
                }
                return _I;
            }
        }

        private Dictionary<string, byte[]> caches = new Dictionary<string, byte[]>();

        public byte[] Get(string path)
        {
            if (caches.ContainsKey(path))
            {
                return caches[path];
            }
            return null;
        }

        public IEnumerator Load(string url, string savePath, string path)
        {
            if (caches.ContainsKey(path))
            {
                yield return caches[path];
            }

            if (File.Exists(savePath))
            {
                byte[] mdx = File.ReadAllBytes(savePath);
                caches[path] = mdx;
                yield return mdx;
            }
            else
            {
                IEnumerator itor = LoadServer(url, savePath, path);
                yield return itor;
                if (itor.Current is byte[])
                {
                    byte[] mdx = (byte[])itor.Current;
                    caches[path] = mdx;

                    Debug.Log($"[MdxTextureManager] Load Server: {url}, mdx={mdx} ");
                    yield return mdx;
                }
                else
                {
                    Debug.LogError($"[MdxTextureManager] Load Server: {url}, mdx=null ");
                    yield return null;
                }
            }

        }

        public IEnumerator LoadServer(string url, string savePath, string path)
        {
            Debug.Log($"LoadServer <color=green> {url}</color>");

            IEnumerator itor = null;
            itor = MdxUnityDownload.I.Load(url, savePath);
            yield return itor;
            bool isOk = (bool)itor.Current;

            Debug.Log($"LoadServer isOk={isOk} {url}");
            if (isOk)
            {
                byte[] mdx = File.ReadAllBytes(savePath);
                caches[path] = mdx;
                yield return mdx;
            }
            else
            {
                yield return null;
            }
        }
    }
}
