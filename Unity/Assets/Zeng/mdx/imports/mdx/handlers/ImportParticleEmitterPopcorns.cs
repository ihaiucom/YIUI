
using System.Collections.Generic;
using UnityEngine;
using Zeng.mdx.parsers.mdx;

namespace Zeng.mdx.imports.mdx
{
    public class ImportParticleEmitterPopcorns
    {
        public ImportModel importModel;
        public Model cmodel;

        public SortedDictionary<int, GameObject> nodes = new SortedDictionary<int, GameObject>();

        public ImportParticleEmitterPopcorns(ImportModel importModel, Model cmodel)
        {
            this.importModel = importModel;
            this.cmodel = cmodel;
        
        }


        public void ToUnity()
        {
            for (int i = 0; i < cmodel.particleEmittersPopcorn.Count; i++)
            {
                ParticleEmitterPopcorn cparticleEmitter = cmodel.particleEmittersPopcorn[i];

                Vector3 pivot = new Vector3();
                if (cparticleEmitter.objectId < cmodel.pivotPoints.Count)
                {
                    var cpivot = cmodel.pivotPoints[cparticleEmitter.objectId];
                    pivot.x = cpivot[0];
                    pivot.y = cpivot[1];
                    pivot.z = cpivot[2];
                    pivot = pivot.VecToU3d();
                }

                GameObject go = importModel.CreateGameObject($"ParticleEmitterPopcorn_{cparticleEmitter.name}" , pivot, cparticleEmitter.parentId);
                nodes.Add(cparticleEmitter.objectId, go);


                if (!string.IsNullOrEmpty(cparticleEmitter.path))
                {
                    Debug.Log($"ParticleEmitterPopcorn_{cparticleEmitter.name}, {cparticleEmitter.path}");
                }
            }
        }

    }
}
