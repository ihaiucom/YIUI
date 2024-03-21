using Codice.Client.Common;
using MdxLib.Model;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Zeng.MdxImport
{
    public class MdxTextureManager
    {
        private static MdxTextureManager _I;
        public static MdxTextureManager I
        {
            get
            {
                if (_I == null)
                {
                    _I = new MdxTextureManager();
                }
                return _I;
            }
        }

        private string GetServerUrl(string path)
        {
            path = path.ToLower().Replace('\\', '/').Replace(".blp", ".dds");
            string url = $"https://www.hiveworkshop.com/assets/wc3/war3.w3mod/{path}";
            //string url = $"https://www.hiveworkshop.com/casc-contents?path={path}";
            return url;
        }

        public string GetPath(CTexture tex)
        {
            string path = tex.FileName;
            int replaceableId = tex.ReplaceableId;
            if (string.IsNullOrEmpty(path) && replaceableId != 0)
            {
                path = $"ReplaceableTextures\\{Replaceableids.ids[replaceableId]}.blp";
            }

            return path;
        }

        private string GetSavePath(string path)
        {
            string savePath = $"Assets/Textures/{path.Replace(".blp", ".dds")}";
            return savePath;
        }


        private Dictionary<string, Texture> textures = new Dictionary<string, Texture>();

        public Texture Get(CTexture tex)
        {
            if (tex == null) return null;
            string path = GetPath(tex);
            if (textures.ContainsKey(path))
            {
                return textures[path];
            }
            return null;
        }

        public IEnumerator Load(CTexture tex)
        {
            Debug.Log("Load£º" + tex.ReplaceableId + ", " + tex.ObjectId + "," + tex.FileName + ", " + tex.WrapWidth + ", " + tex.WrapHeight);

            string path = GetPath(tex);
            string url = GetServerUrl(path);
            string savePath = GetSavePath(path);
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
                IEnumerator itor = LoadServer(tex);
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

        public IEnumerator LoadServer(CTexture tex)
        {
            string path = GetPath(tex);
            IEnumerator itor = LoadServer(path);
            yield return itor;
            yield return itor.Current;
        }


        public IEnumerator LoadServer(string path)
        {
            string url = GetServerUrl(path);
            string savePath = GetSavePath(path);
            Debug.Log($"LoadServer <color=green> {url}</color>");

            IEnumerator itor = null;
            itor = MdxDownload.I.Load(url, savePath);
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

        public IEnumerator LoadTeamTextures()
        {


            int teams = 28;
            string ext = "blp";

            for (int i = 0; i < teams; i++)
            {
                string id = i.ToString().PadLeft(2, '0');
                string end = $"{id}.{ext}";
                string color = $"ReplaceableTextures\\TeamColor\\TeamColor{end}";
                string glow = $"ReplaceableTextures\\TeamGlow\\TeamGlow{end}";

                yield return LoadServer(color);
                yield return LoadServer(glow);
            }

        }
    }

}
