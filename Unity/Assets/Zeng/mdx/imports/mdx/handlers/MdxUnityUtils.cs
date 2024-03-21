
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

namespace Zeng.mdx.imports.mdx
{
    public static class MdxUnityUtils
    {
        public static Vector3 VecToU3d(this Vector3 v) {
            return new Vector3(-v.y, v.z, v.x);
        }

        public static Vector2 UvToU3d(this Vector2 v)
        {
            return new Vector2(v.x, Mathf.Abs(v.y - 1));
        }


        public static Quaternion QuaternionToU3d(this Quaternion v)
        {
            return new Quaternion(-v.y, v.z, v.x, -v.w);
        }


        public static bool IsForceKeyFrame(string nodeName)
        {
            if (!nodeName.Contains("Plane") && !nodeName.Contains("Glow"))
            {
                return true;
            }
            return false;
        }

        public static Color ToColor(this float[] v) {
            return new Color(v[0] / 255f, v[1] / 255f, v[2] / 255f, v[3] / 255f);
        }

        public static Vector2Int GetMaxSize(this List<Texture> textures)
        {
            Vector2Int size = Vector2Int.zero;
            foreach (Texture tex in textures)
            {
                size.x = Mathf.Max(size.x, tex.width);
                size.y = Mathf.Max(size.y, tex.height);
            }
            return size;
        }

        public static Vector2Int GetMinSize(this List<Texture> textures)
        {
            Vector2Int size = Vector2Int.one * 2048;
            foreach (Texture tex in textures)
            {
                size.x = Mathf.Min(size.x, tex.width);
                size.y = Mathf.Min(size.y, tex.height);
            }
            return size;
        }

        public static Texture2DArray CreateTexture2DArray(this List<Texture> sourceTextures)
        {
            Vector2Int size = sourceTextures.GetMinSize();
            Texture2DArray texture2DArray = new Texture2DArray(size.x, size.y, sourceTextures.Count, ((Texture2D)sourceTextures[0]).format, true);


            Debug.Log($"CreateTexture2DArray sourceTextures.Count={sourceTextures.Count}, sourceTextures[0].mipmapCount={sourceTextures[0].mipmapCount}, {sourceTextures[0].mipMapBias}, ((Texture2D)sourceTextures[0]).format={((Texture2D)sourceTextures[0]).format}");
            // Apply settings
            texture2DArray.filterMode = FilterMode.Bilinear;
            texture2DArray.wrapMode = TextureWrapMode.Repeat;


            for (int i = 0; i < sourceTextures.Count; i++)
            {
                Debug.Log($"CreateTexture2DArray sourceTextures[{i}].mipmapCount={sourceTextures[i].mipmapCount}");
                for (int m = 0; m < sourceTextures[i].mipmapCount; m++)
                {
                    Debug.Log($"CreateTexture2DArray srcElement={0}, srcMip={m},   dstElement={i}, dstMip={m}");
                    UnityEngine.Texture src = sourceTextures[i];
                    int srcElement = 0;
                    int srcMip = m;
                    int srcX = 0;
                    int srcY = 0;
                    int srcWidth = size.x;
                    int srcHeight = size.y;
                    UnityEngine.Texture dst = texture2DArray;
                    int dstElement = i;
                    int dstMip = m;
                    int dstX = 0;
                    int dstY = 0;

                    Graphics.CopyTexture(src, srcElement, srcMip, srcX, srcY, srcWidth, srcHeight, dst, dstElement, dstMip, dstX, dstY);
                }
            }
            // Apply our changes
            texture2DArray.Apply(false);
            return texture2DArray;
        }

        public static float[] ToFloatArray(this byte[] bytes )
        {
            float[] result = new float[bytes.Length];
            for(int i = 0; i < bytes.Length; i ++)
            {
                result[i] = (float)bytes[i];

            }

            return result;
        }
    }
}
