

using MdxLib.Model;
using UnityEngine;
using UnityEngine.Rendering;
using Zeng.Mdx;

namespace Zeng.MdxImport.mdx.handlers
{
    public class MdxUtils
    {
        public static bool IsForceKeyFrame(string nodeName)
        {
            if (!nodeName.Contains("Plane") && !nodeName.Contains("Glow"))
            {
                return true;
            }
            return false;
        }

        public static void SetBillboarded(GameObject bone, INode cbone)
        {
            if (cbone.Billboarded || cbone.BillboardedLockX || cbone.BillboardedLockY || cbone.BillboardedLockZ)
            {
                NodeBillboarded nodeBillboarded = bone.AddComponent<NodeBillboarded>();
                nodeBillboarded.Billboarded = cbone.Billboarded;
                nodeBillboarded.BillboardedLockY = cbone.BillboardedLockX;
                nodeBillboarded.BillboardedLockZ = cbone.BillboardedLockY;
                nodeBillboarded.BillboardedLockX = cbone.BillboardedLockZ;
            }
        }

        public static Shader GetShader(CMaterial cmaterial, CMaterialLayer clayer)
        {
            Shader shader = null;
            if (clayer.FilterMode > EMaterialLayerFilterMode.Transparent)
            {
                shader = Shader.Find("Zeng/Mdx/sd_transparent");
            }
            else
            {
                shader = Shader.Find("Zeng/Mdx/sd_opaqua");
            }

            return shader;
        }

        public static BlendMode[] GetBinde(EMaterialLayerFilterMode filterMode)
        {
            BlendMode[] arr = new BlendMode[2] { BlendMode.SrcAlpha, BlendMode.OneMinusSrcAlpha };
            switch (filterMode)
            {
                case EMaterialLayerFilterMode.Blend:
                    arr[0] = BlendMode.SrcAlpha;
                    arr[1] = BlendMode.OneMinusSrcAlpha;
                    break;
                case EMaterialLayerFilterMode.Additive:
                    arr[0] = BlendMode.SrcAlpha;
                    arr[1] = BlendMode.One;
                    break;
                case EMaterialLayerFilterMode.AdditiveAlpha:
                    arr[0] = BlendMode.SrcAlpha;
                    arr[1] = BlendMode.One;
                    break;
                case EMaterialLayerFilterMode.Modulate:
                    arr[0] = BlendMode.Zero;
                    arr[1] = BlendMode.SrcColor;
                    break;
                case EMaterialLayerFilterMode.Modulate2x:
                    arr[0] = BlendMode.DstColor;
                    arr[1] = BlendMode.SrcColor;
                    break;
            }
            return arr;
        }
    }
}
