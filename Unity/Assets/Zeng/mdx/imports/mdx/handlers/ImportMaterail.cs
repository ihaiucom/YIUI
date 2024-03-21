using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using Zeng.mdx.parsers.mdx;
using CMaterial = Zeng.mdx.parsers.mdx.Material;
using Material = UnityEngine.Material;

namespace Zeng.mdx.imports.mdx
{
    public class ImportMaterail
    {
        public ImportModel importModel;

        public CMaterial cmaterial;
        public Layer clayer;
        public int materailIndex;
        public int layerIndex;
        public string materailPath;

        public Material material;
        public string name;



        public ImportMaterail(ImportModel importModel, CMaterial cmaterial, Layer clayer, int materailIndex, int layerIndex)
        {
            this.importModel = importModel;
            this.cmaterial = cmaterial;
            this.clayer = clayer;
            this.materailIndex = materailIndex;
            this.layerIndex = layerIndex;
            name = $"{importModel.name}_{materailIndex}_{layerIndex}";
            materailPath = $"{importModel.saveDirPath}/Materials/{name}.mat";
        }

        public void ToUnity()
        {
            Shader shader = GetShader(cmaterial, clayer);
            material = new Material(shader);
            material.name = name;

            UnityEngine.Texture texture = importModel.textures[clayer.textureId];
            material.SetTexture("_MainTex", texture);


            material.SetColor("_TeamColor", Color.red);
            material.SetFloat("_filterMode", (float)clayer.filterMode);
            material.SetInt("_Cull", clayer.twoSided ? (int)UnityEngine.Rendering.CullMode.Off : (int)UnityEngine.Rendering.CullMode.Back);

            if(clayer.filterMode > Layer.FilterMode.Transparent)
            {
                BlendMode[] blendMode1s = GetBinde(clayer.filterMode);

                material.SetOverrideTag("RenderType", "Transparent");
                material.renderQueue = (int)RenderQueue.Transparent;
                material.SetFloat("_BlendOp", (float)UnityEngine.Rendering.BlendOp.Add);
                material.SetFloat("_SrcBlend", (float)blendMode1s[0]);
                material.SetFloat("_DstBlend", (float)blendMode1s[1]);
                material.SetFloat("_ZWrite", 0);
                material.SetColor("_vertexColor", new Color(1f, 1f, 1, 1f));
                //material.SetColor("_vertexColor", new Color(1f, 0.3f, 0, 0.5f));
            }
            else
            {
                if (cmaterial.layers.Count == 2)
                {
                    material.renderQueue = (int)RenderQueue.AlphaTest;
                    material.SetOverrideTag("RenderType", "TransparentCutout");
                }
                else
                {
                    material.renderQueue = (int)RenderQueue.Geometry;
                    material.SetOverrideTag("RenderType", "Opaque");
                }

                material.SetFloat("_BlendOp", (float)BlendOp.Add);
                material.SetFloat("_SrcBlend", (float)BlendMode.One);
                material.SetFloat("_DstBlend", (float)BlendMode.Zero);
                material.SetFloat("_ZWrite", 1);
            }


        }

        public void Save()
        {
            //Debug.Log(materailPath);
            if (!Directory.Exists(Path.GetDirectoryName(materailPath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(materailPath));
            }

            AssetDatabase.CreateAsset(material, materailPath);
            AssetDatabase.SaveAssets();
            material =  AssetDatabase.LoadAssetAtPath<UnityEngine.Material>(materailPath);


        }

        public static Shader GetShader(CMaterial cmaterial, Layer clayer)
        {
            Shader shader = null;
            if (clayer.filterMode > Layer.FilterMode.Transparent)
            {
                shader = Shader.Find("Zeng/Mdx/sd_transparent");
            }
            else
            {
                shader = Shader.Find("Zeng/Mdx/sd_opaqua");
            }

            return shader;
        }

        public static BlendMode[] GetBinde(Layer.FilterMode filterMode)
        {
            BlendMode[] arr = new BlendMode[2] { BlendMode.SrcAlpha, BlendMode.OneMinusSrcAlpha };
            switch (filterMode)
            {
                case Layer.FilterMode.Blend:
                    arr[0] = BlendMode.SrcAlpha;
                    arr[1] = BlendMode.OneMinusSrcAlpha;
                    break;
                case Layer.FilterMode.Additive:
                    arr[0] = BlendMode.SrcAlpha;
                    arr[1] = BlendMode.One;
                    break;
                case Layer.FilterMode.AddAlpha:
                    arr[0] = BlendMode.SrcAlpha;
                    arr[1] = BlendMode.One;
                    break;
                case Layer.FilterMode.Modulate:
                    arr[0] = BlendMode.Zero;
                    arr[1] = BlendMode.SrcColor;
                    break;
                case Layer.FilterMode.Modulate2x:
                    arr[0] = BlendMode.DstColor;
                    arr[1] = BlendMode.SrcColor;
                    break;
            }
            return arr;
        }
    }
}
