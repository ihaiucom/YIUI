using MdxLib.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Windows;

namespace Zeng.MdxImport.mdx.handlers
{
    public class MaterailHandler
    {
        public int index;
        public CMaterial cmaterial;

        public List<LayerHandler> layers = new List<LayerHandler>();
        public List<Material> layerMasters = new List<Material>();
        public Material material
        {
            get
            {
                if (layerMasters.Count > 0)
                {
                    return layerMasters[0];
                }
                else
                {
                    return null;
                }
            }

            set
            {
                layerMasters[0] = value;
            }
        }



        public MaterailHandler(int index, CMaterial cmaterial)
        {
            this.index = index;
            this.cmaterial = cmaterial;


        }


        public void SaveUnityFile(string directory, List<GeosetHandler> geosetHandlers)
        {

            string materialPath = directory + material.name + ".mat";
            AssetDatabase.CreateAsset(material, materialPath);
            AssetDatabase.SaveAssets();

            Material materialAsset = AssetDatabase.LoadAssetAtPath<Material>(materialPath);

            for (int i = 0; i < geosetHandlers.Count; i++)
            {
                GeosetHandler geosetHandler = geosetHandlers[i];
                Material[] sharedMaterials = geosetHandler.renderer.sharedMaterials;
                for (int j = 0; j < sharedMaterials.Length; j++)
                {
                    Debug.Log("SaveUnityFile: " + sharedMaterials[j].name + ", " + material.name);
                    if (sharedMaterials[j].name.Contains(material.name))
                    {
                        sharedMaterials[j] = materialAsset;
                    }
                }
                geosetHandler.renderer.sharedMaterials = sharedMaterials;
            }

            material = materialAsset;
        }

        public void Gen()
        {

            for (int j = 0; j < cmaterial.Layers.Count; j++)
            {
                CMaterialLayer clayer = cmaterial.Layers[j];
                Shader shader = MdxUtils.GetShader(cmaterial, clayer);
                Material material = new Material(shader);
                material.name = cmaterial.Model.Name + "_" + index.ToString() + "_" + j.ToString();
                layerMasters.Add(material);

                if (clayer?.Texture?.Object != null)
                {

                    CTexture tex = clayer.Texture.Object;
                    Texture texture = MdxTextureManager.I.Get(tex);
                    Debug.Log($"SetTexture j={j}, material={material.name}, ObjectId={tex.ObjectId}, ReplaceableId={tex.ReplaceableId}, FileName={tex.FileName}, path={MdxTextureManager.I.GetPath(tex)}, texture={texture}");
                    if (texture)
                    {
                        material.SetTexture("_MainTex", texture);
                        if (!string.IsNullOrEmpty(tex.FileName) && cmaterial.Layers.Count > 0)
                        {
                            if (string.IsNullOrEmpty(cmaterial.Layers[0]?.Texture?.Object?.FileName))
                            {
                                layerMasters[0].SetTexture("_MainTex", texture);
                            }
                        }
                    }
                }






                material.SetColor("_TeamColor", Color.red);
                material.SetFloat("_filterMode", (float)clayer.FilterMode);
                material.SetInt("_Cull", (clayer.TwoSided) ? (int)UnityEngine.Rendering.CullMode.Off : (int)UnityEngine.Rendering.CullMode.Back);

                if (cmaterial.Layers.Count == 1 && clayer.FilterMode > EMaterialLayerFilterMode.Transparent)
                {
                    BlendMode[] blendMode1s = MdxUtils.GetBinde(clayer.FilterMode);
                    material.SetOverrideTag("RenderType", "Transparent");
                    material.renderQueue = (int)RenderQueue.Transparent;
                    material.SetFloat("_BlendOp", (float)UnityEngine.Rendering.BlendOp.Add);
                    material.SetFloat("_SrcBlend", (float)blendMode1s[0]);
                    material.SetFloat("_DstBlend", (float)blendMode1s[1]);
                    material.SetFloat("_ZWrite", 0);
                    material.SetColor("_vertexColor", new Color(1f, 0.3f, 0, 0.5f));
                }
                else
                {
                    if (cmaterial.Layers.Count == 2)
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

        }
    }
}
