using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace Zeng.mdx.commons
{
    public class MdxUnityTextureManager
    {
        private static MdxUnityTextureManager _I;
        public static MdxUnityTextureManager I
        {
            get
            {
                if (_I == null)
                {
                    _I = new MdxUnityTextureManager();
                }
                return _I;
            }
        }

        private Dictionary<string, Texture> textures = new Dictionary<string, Texture>();

        public Texture Get(string path)
        {
            if (textures.ContainsKey(path))
            {
                return textures[path];
            }
            return null;
        }

        public IEnumerator Load(string url, string savePath, string path)
        {
            if (textures.ContainsKey(path))
            {
                yield return textures[path];
            }

            if (File.Exists(savePath))
            {
                Texture texture = AssetDatabase.LoadAssetAtPath<Texture>(savePath);
                if (texture)
                {
                    textures[path] = texture;
                }

                yield return texture;
            }
            else
            {
                IEnumerator itor = LoadServer(url, savePath, path);
                yield return itor;
                if (itor.Current is Texture)
                {
                    Texture texture = (Texture)itor.Current;
                    textures[path] = texture;

                    Debug.Log($"[MdxTextureManager] Load Server: {url}, texture={texture} ");
                    yield return texture;
                }
                else
                {
                    Debug.LogError($"[MdxTextureManager] Load Server: {url}, texture=null ");
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
                Texture texture = AssetDatabase.LoadAssetAtPath<Texture>(savePath);
                if (texture)
                {
                    textures[path] = texture;
                }
                Debug.Log(texture);
                yield return texture;
            }
            else
            {
                yield return null;
            }
        }
    }
}
