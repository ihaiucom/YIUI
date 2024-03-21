using MdxLib.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Zeng.MdxImport.mdx.handlers
{
    public class TextureHandler
    {
        public int index;
        public CTexture ctexture;
        public Texture texture = null;
        public TextureHandler(int index, CTexture ctexture)
        {
            this.index = index;
            this.ctexture = ctexture;
        }

        public IEnumerator Load()
        {
            CTexture tex = ctexture;
            IEnumerator it = MdxTextureManager.I.Load(tex);
            yield return it;

            Debug.Log($"ImportTextures i:{index},  FileName={tex.FileName},path={MdxTextureManager.I.GetPath(tex)}, Current={it.Current}");
            if (it.Current != null && it.Current is Texture)
            {
                texture = (Texture)it.Current;
                Debug.Log(texture);
            }

            Debug.Log($"ImportTextures i:{index}, ObjectId={tex.ObjectId}, ReplaceableId={tex.ReplaceableId}, FileName={tex.FileName},path={MdxTextureManager.I.GetPath(tex)}, texture={texture}");

        }
    }
}
