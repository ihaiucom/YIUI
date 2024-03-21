using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zeng.MdxImport.mdx.handlers
{
    public class ModelImport
    {
        private static ModelImport _I;
        public static ModelImport I
        {
            get
            {
                if (_I == null)
                {
                    _I = new ModelImport();
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
        public bool importMaterials = false;
        public bool addMaterialsToAsset = true;

        // Animations.
        public bool importAnimations = false;
        public bool addAnimationsToAsset = true;
        public bool importTangents = true;
        public float frameRate = 960;
        public List<string> excludeAnimations = new List<string>() { "Decay Bone", "Decay Flesh" };


        public IEnumerator OnImportAsset(string assetPath, string savePath = "Assets/Prefabs/")
        {
            excludeByTexture.Clear();
            excludeAnimations.Clear();


            ModelHandler model = new ModelHandler();
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

            model.SaveUnityFile(savePath);


        }


    }

}