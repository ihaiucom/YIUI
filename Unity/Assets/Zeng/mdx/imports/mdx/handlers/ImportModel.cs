using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Zeng.mdx.commons;
using Zeng.mdx.parsers.mdx;
using CTexture = Zeng.mdx.parsers.mdx.Texture;
using Texture = UnityEngine.Texture;
using CMaterial = Zeng.mdx.parsers.mdx.Material;
using Material = UnityEngine.Material;
using MdxLib.Primitives;

namespace Zeng.mdx.imports.mdx
{
    public class ImportModel
    {
        public string assetPath;
        public Model model;


        public string saveDirPath;
        public string savePrefabPath;
        public string name;

        public ImportSetting importSetting = new ImportSetting();
        public GameObject gameObject;

        public ImportModel(Model model, string assetPath)
        {
            this.model = model;
            this.assetPath = assetPath;


            string filePath = assetPath.Replace("\\", "/");
            filePath = filePath.Substring(filePath.IndexOf("Assets/") + "Assets/".Length);
            name = Path.GetFileName(filePath).Replace(".mdx", "");
            if (filePath.IndexOf("mdx_res/") != -1)
            {
                filePath = filePath.Substring(filePath.IndexOf("mdx_res/") + "mdx_res/".Length);
                savePrefabPath = "Assets/MdxPrefabs/" + filePath.Replace(".mdx", ".prefab");
                saveDirPath = Path.GetDirectoryName(savePrefabPath).Replace("\\", "/");
            }
            else
            {
                savePrefabPath = $"Assets/MdxPrefabs/{name}/{name}.prefab";
                saveDirPath = Path.GetDirectoryName(savePrefabPath).Replace("\\", "/");
            }
            Debug.Log(filePath);
            Debug.Log(savePrefabPath);
            Debug.Log(saveDirPath);
        }

        public List<ImportGeoset> geosets = new List<ImportGeoset>();
        public List<ImportMeshRender> meshRenders = new List<ImportMeshRender>();
        public List<Texture> textures = new List<Texture>();
        public List<List<ImportMaterail>> importMaterials = new List<List<ImportMaterail>>();
        public List<List<Material>> materials = new List<List<Material>>();
        public ImportSkeletons importSkeletons;
        public ImportAttachments importAttachments;
        public ImportEvents importEvents;
        public ImportSequences importSequences;
        public ImportParticleEmitters importParticleEmitters;
        public ImportParticleEmitter2s importParticleEmitter2s;
        public ImportParticleEmitterPopcorns importParticleEmitterPopcorns;
        public ImportRibbonEmitters importRibbonEmitters;

        public IEnumerator ToUnity()
        {
            gameObject = new GameObject(name);

            // Mesh
            for (int i = 0; i < model.geosets.Count; i++)
            {
                Geoset geoset = model.geosets[i];
                ImportGeoset importGeoset = new ImportGeoset(this, geoset, i);
                importGeoset.ToUnity();
                if (model.bones.Count == 0 && model.helpers.Count == 0) importGeoset.Save();
                geosets.Add(importGeoset);
            }

            // Texture
            for(int i = 0; i < model.textures.Count; i ++)
            {
                Texture texture = null;

                CTexture ctexture = model.textures[i];
                string texturePath = ctexture.TexturePath;
                if(!string.IsNullOrEmpty(texturePath))
                {
                    texturePath = MdxUnityResPathDefine.GetPath(texturePath);
                    if (System.IO.File.Exists(texturePath))
                    {
                        texture = AssetDatabase.LoadAssetAtPath<Texture>(texturePath);
                    }
                    else
                    {
                        string url = UrlUtils.localOrHive(ctexture.TexturePath);
                        string savePath = MdxUnityResPathDefine.GetPath(ctexture.TexturePath);
                        IEnumerator itor = MdxUnityTextureManager.I.Load(url, savePath, ctexture.TexturePath);
                        yield return itor;
                        if (itor.Current is Texture)
                        {
                            texture = (Texture)itor.Current;
                        }
                    }
                }
                textures.Add(texture);
            }

            // Master
            for(int i = 0; i < model.materials.Count; i ++)
            {
                CMaterial cmaterial  = model.materials[i];
                materials.Add(new List<Material>());
                importMaterials.Add(new List<ImportMaterail>());
                for (int j = 0; j < cmaterial.layers.Count; j ++)
                {
                    Layer clayer = cmaterial.layers[j];

                    ImportMaterail importMaterail = new ImportMaterail(this, cmaterial, clayer, i, j);
                    importMaterail.ToUnity();
                    importMaterail.Save();
                    materials[i].Add(importMaterail.material);
                    importMaterials[i].Add(importMaterail);

                }
            }

            // Render
            for (int i = 0; i < model.geosets.Count; i++)
            {
                Geoset geoset = model.geosets[i];
                ImportGeoset importGeoset = geosets[i];
                List<ImportMaterail> importMaterailList = importMaterials[(int)geoset.materialId];
                for(int j = 0; j < importMaterailList.Count; j ++)
                {
                    ImportMaterail importMaterail = importMaterailList[j];
                    ImportMeshRender importMeshRender = new ImportMeshRender(this, importGeoset, importMaterail);
                    importMeshRender.gameObject.transform.parent = gameObject.transform;
                    meshRenders.Add(importMeshRender);
                }
            }

            // Skeletons
            importSkeletons = new ImportSkeletons(this, model);

            // Attachments
            importAttachments = new ImportAttachments(this, model);
            importAttachments.ToUnity();

            // Events
            importEvents = new ImportEvents(this, model);
            importEvents.ToUnity();

            // ParticleEmitters
            importParticleEmitters = new ImportParticleEmitters(this, model);
            importParticleEmitters.ToUnity();

            // ImportParticleEmitter2s
            importParticleEmitter2s = new ImportParticleEmitter2s(this, model);
            importParticleEmitter2s.ToUnity();

            // ImportParticleEmitterPopcorns
            importParticleEmitterPopcorns = new ImportParticleEmitterPopcorns(this, model);
            importParticleEmitterPopcorns.ToUnity();

            // ImportRibbonEmitters
            importRibbonEmitters = new ImportRibbonEmitters(this, model); 
            importRibbonEmitters.ToUnity();


            // Sequences
            importSequences = new ImportSequences(this, model);
            importSequences.ToUnity();
            importSequences.Save();



            PrefabUtility.SaveAsPrefabAsset(gameObject, savePrefabPath);

            yield return importAttachments.LoadOthers();
            yield return importEvents.LoadOthers();

        }



        public string GetPath(GameObject bone)
        {
            if (!bone)
            {
                return "";
            }

            GameObject skeleton = importSkeletons.skeleton;
            string path = bone.name;
            while (bone.transform.parent != skeleton.transform && bone.transform.parent != null)
            {
                bone = bone.transform.parent.gameObject;
                path = bone.name + "/" + path;
            }

            path = skeleton.name + "/" + path;

            return path;
        }

        public GameObject CreateGameObject(string name, Vector3 pivot, int parentId)
        {
            GameObject gameObject = new GameObject(name);

            // Set the position.
            // MDX/MDL up axis is Z.
            // Unity up axis is Y.
            gameObject.transform.position = pivot;

            GameObject skeleton = importSkeletons.skeleton;
            SortedDictionary<int, GameObject> bones = importSkeletons.bones;
            // Set the parent.
            if (bones.ContainsKey(parentId))
            {
                GameObject parent = bones[parentId];
                gameObject.transform.SetParent(parent.transform);
            }
            else
            {
                gameObject.transform.SetParent(skeleton.transform);
            }

            return gameObject;
        }

    }
}
