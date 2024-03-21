using MdxLib.Model;
using MdxLib.ModelFormats;
using MdxLib.Primitives;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;

namespace Zeng.MdxImport.mdx.handlers
{
    public class ModelHandler
    {
        private string path;
        public MdxImportSettings settings;


        public CModel cmodel;
        public GameObject gameObject { get; private set; }
        public SkeletonHandler skeletonHandler { get; private set; }
        public SequenceHandler sequenceHandler { get; private set; }
        public EventHandler eventHandler { get; private set; }
        public AttachmentHandler attachmentHandler { get; private set; }
        public ParticleEmitterHandler particleEmitterHandler { get; private set; }
        public ParticleEmitters2Handler particleEmitters2Handler { get; private set; }
        public CollisionShapesHandler collisionShapesHandler { get; private set; }
        public List<GeosetHandler> geosetHandlers = new List<GeosetHandler>();
        public List<MaterailHandler> materailHandlers = new List<MaterailHandler>();
        public List<LayerHandler> layerHandlers = new List<LayerHandler>();
        public List<TextureHandler> textureHandlers = new List<TextureHandler>();
        

        public ModelHandler()
        {

        }


        public IEnumerator Import(string path, MdxImportSettings settings)
        {
            this.path = path;
            this.settings = settings;


            gameObject = new GameObject();
            gameObject.name = Path.GetFileNameWithoutExtension(path);

            ReadFile();
            Geosets();
            yield return Textures();
            Materials();
            Skeletons();

            particleEmitters2Handler = new ParticleEmitters2Handler(this);
            particleEmitters2Handler.Imports();

            Sequences();

            attachmentHandler = new AttachmentHandler(this);
            attachmentHandler.Imports();

            eventHandler = new EventHandler(this);
            eventHandler.Imports();

            particleEmitterHandler = new ParticleEmitterHandler(this);
            particleEmitterHandler.Imports();

            collisionShapesHandler = new CollisionShapesHandler(this);
            collisionShapesHandler.Imports();


            yield return gameObject;
        }

        public void SaveUnityFile(string savePath = "Assets/Prefabs/")
        {
            string directoryPath = Path.GetDirectoryName(path).Replace('\\', '/');

            string fileName = Path.GetFileNameWithoutExtension(path);
            string saveDirPath = savePath + fileName + "/";
            if (!Directory.Exists(saveDirPath))
            {
                Directory.CreateDirectory(saveDirPath);
            }

            // Mesh
            for(int i = 0; i < geosetHandlers.Count; i ++)
            {
                GeosetHandler handler = geosetHandlers[i];
                handler.SaveUnityFile(saveDirPath, fileName);
            }

            // Materail
            string directory = saveDirPath + "/Materials/";
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            for (int i = 0; i < materailHandlers.Count; i ++)
            {
                MaterailHandler handler = materailHandlers[i];
                handler.SaveUnityFile(directory, geosetHandlers);

            }

            // Clip
            sequenceHandler.SaveUnityFile(saveDirPath, fileName, gameObject);




            // Prefab
            string prefabPath = saveDirPath + fileName + ".prefab";
            PrefabUtility.SaveAsPrefabAsset(gameObject, prefabPath);

        }



        private void Sequences()
        {
            sequenceHandler = new SequenceHandler(this);
            sequenceHandler.ImportAnimations();
        }


        private void Skeletons()
        {
            skeletonHandler = new SkeletonHandler(this);
            skeletonHandler.ImportSkeleton();
        }


        private IEnumerator Textures()
        {
            for (int i = 0; i < cmodel.Textures.Count; i++)
            {
                CTexture tex = cmodel.Textures[i];

                TextureHandler textureHandler = new TextureHandler(i, tex);
                textureHandlers.Add(textureHandler);
                yield return textureHandler.Load();
            }

        }

        private void Materials()
        {
            int layerId = 0;
            for (int i = 0; i < cmodel.Materials.Count; i++)
            {
                CMaterial cmaterial = cmodel.Materials.Get(i);
                if (cmaterial.ContainsTextures(settings.excludeByTexture))
                {
                    continue;
                }

                MaterailHandler materailHandler = new MaterailHandler(i, cmaterial);
                for (int j = 0; j < cmaterial.Layers.Count; j++)
                {
                    CMaterialLayer clayer = cmaterial.Layers[j];

                    LayerHandler clayerHandler = new LayerHandler(layerId++, clayer, materailHandler);
                    layerHandlers.Add(clayerHandler);
                    materailHandler.layers.Add(clayerHandler);
                }

                materailHandlers.Add(materailHandler);
                materailHandler.Gen();
            }

            for (int i = 0; i < geosetHandlers.Count; i++)
            {
                GeosetHandler geosetHandler = geosetHandlers[i];
                geosetHandler.SetMaterail(materailHandlers);

            }
        }

        private void Geosets()
        {
            geosetHandlers = new List<GeosetHandler>();
            for (int i = 0; i < cmodel.Geosets.Count; i++)
            {
                CGeoset cgeoset = cmodel.Geosets.Get(i);
                if (settings.excludeGeosets.Contains(i) || cgeoset.ContainsTextures(settings.excludeByTexture))
                {
                    continue;
                }

                GeosetHandler handler = new GeosetHandler(i, cgeoset);
                geosetHandlers.Add(handler);
                handler.Gen();
                handler.gameObject.transform.SetParent(gameObject.transform);
            }
        }


        private void ReadFile()
        {
            //try
            {
                cmodel = new CModel();
                using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    string extension = Path.GetExtension(path);
                    if (extension.Equals(".mdx"))
                    {
                        CMdx cmdx = new CMdx();
                        cmdx.Load(path, stream, cmodel);
                    }
                    else if (extension.Equals(".mdl"))
                    {
                        CMdl cmdl = new CMdl();
                        cmdl.Load(path, stream, cmodel);
                    }
                    else
                    {
                        throw new IOException("Invalid file extension.");
                    }
                }
            }
            //catch (Exception e)
            //{
            //    cmodel = null;
            //    Debug.LogError(e.Message);
            //}
        }


        public string GetPath(GameObject bone)
        {
            if (!bone)
            {
                return "";
            }

            GameObject skeleton = skeletonHandler.skeleton;
            string path = bone.name;
            while (bone.transform.parent != skeleton.transform && bone.transform.parent != null)
            {
                bone = bone.transform.parent.gameObject;
                path = bone.name + "/" + path;
            }

            path = skeleton.name + "/" + path;

            return path;
        }

        public GeosetHandler FindGoeset(CGeoset cgeoset)
        {
            for(int i = 0; i < geosetHandlers.Count; i ++)
            {
                if (geosetHandlers[i].cgeoset.ObjectId == cgeoset.ObjectId)
                {
                    return geosetHandlers[i];
                }
            }

            return null;
        }


        public GameObject CreateGameObject(string name, CVector3 pivot, int parentId)
        {
            GameObject gameObject = new GameObject(name);

            // Set the position.
            // MDX/MDL up axis is Z.
            // Unity up axis is Y.
            gameObject.transform.position = new Vector3(-pivot.Y, pivot.Z, pivot.X);

            GameObject skeleton = skeletonHandler.skeleton;
            SortedDictionary<int, GameObject> bones = skeletonHandler.bones;
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
