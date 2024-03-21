using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using Zeng.MdxImport;
using UnityEditor.Animations;

namespace Zeng.MdxImport
{
    public class EditorMdxImport
    {
        private static EditorMdxImport _I;
        public static EditorMdxImport I
        {
            get
            {
                if (_I == null)
                {
                    _I = new EditorMdxImport();
                }
                return _I;
            }
        }

        // General.
        public bool importAttachments = false;
        public bool importEvents = false;
        public bool importParticleEmitters = false;
        public bool importCollisionShapes = false;
        public List<int> excludeGeosets = new List<int>();
        public List<string> excludeByTexture = new List<string>() { "gutz.blp" };

        // Materials.
        public bool importMaterials = true;
        public bool addMaterialsToAsset = true;

        // Animations.
        public bool importAnimations = true;
        public bool addAnimationsToAsset = true;
        public bool importTangents = true;
        public float frameRate = 960;
        public List<string> excludeAnimations = new List<string>() { "Decay Bone", "Decay Flesh" };


        public IEnumerator OnImportAsset(string assetPath, string savePath = "Assets/Prefabs/")
        {
            string directoryPath = Path.GetDirectoryName(assetPath).Replace('\\', '/');

            MdxModelImport model = new MdxModelImport();
            MdxImportSettings settings = new MdxImportSettings()
            {
                importAttachments = importAttachments,
                importEvents = importEvents,
                importParticleEmitters = importParticleEmitters,
                importCollisionShapes = importCollisionShapes,
                excludeGeosets = excludeGeosets,
                excludeByTexture = excludeByTexture,
                importMaterials = importMaterials,
                importAnimations = importAnimations,
                importTangents = importTangents,
                frameRate = frameRate,
                excludeAnimations = excludeAnimations
            };
            yield return model.Import(assetPath, settings);

            string fileName = Path.GetFileNameWithoutExtension(assetPath);
            string saveDirPath = savePath + fileName + "/";
            if (!Directory.Exists(saveDirPath))
            {
                Directory.CreateDirectory(saveDirPath);
            }



            string meshPath = saveDirPath + fileName + ".asset";
            if (model.mesh != null)
            {
                AssetDatabase.CreateAsset(model.mesh, meshPath);
                AssetDatabase.SaveAssets();

                MeshFilter meshFilter = model.gameObject.GetComponent<MeshFilter>();
                meshFilter.mesh = AssetDatabase.LoadAssetAtPath<Mesh>(meshPath);

            }

            if (importMaterials)
            {

                string directory = saveDirPath + "/Materials/";
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                Material[] materials = null;
                MeshRenderer renderer = model.gameObject.GetComponent<MeshRenderer>();
                if (renderer != null)
                {
                    materials = renderer.materials;
                }

                for (int i = 0; i < model.materials.Count; i++)
                {
                    Material material = model.materials[i];


                    string materialPath = directory + material.name + ".mat";
                    AssetDatabase.CreateAsset(material, materialPath);
                    AssetDatabase.SaveAssets();

                    Material materialAsset = AssetDatabase.LoadAssetAtPath<Material>(materialPath);

                    if (materials != null)
                    {
                        for (var m = 0; m < materials.Length; m++)
                        {
                            if (materials[i] == material)
                            {
                                materials[i] = materialAsset;
                                Debug.Log(materialAsset);
                            }
                        }
                    }
                }

                if (renderer != null)
                {
                    renderer.sharedMaterials = materials;
                }
            }

            if (importAnimations)
            {
                string directory = saveDirPath + "/Animations/";

                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                foreach (AnimationClip clip in model.clips)
                {
                    AssetDatabase.CreateAsset(clip, directory + clip.name + ".anim");
                    AssetDatabase.SaveAssets();
                }

                string animationControllerPath = saveDirPath + fileName + ".controller";
                AnimatorController animatorController = AnimatorControllerUtils.Create(animationControllerPath, directory);

                Animator animator = model.gameObject.AddComponent<Animator>();
                animator.runtimeAnimatorController = animatorController;
            }

            string prefabPath = saveDirPath + fileName + ".prefab";
            PrefabUtility.SaveAsPrefabAsset(model.gameObject, prefabPath);
        }


    }

}