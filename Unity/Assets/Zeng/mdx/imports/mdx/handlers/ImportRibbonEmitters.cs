
using System.Collections.Generic;
using UnityEngine;
using Zeng.mdx.parsers.mdx;

namespace Zeng.mdx.imports.mdx
{
    public class ImportRibbonEmitters
    {
        public ImportModel importModel;
        public Model cmodel;

        public SortedDictionary<int, GameObject> nodes = new SortedDictionary<int, GameObject>();

        public ImportRibbonEmitters(ImportModel importModel, Model cmodel)
        {
            this.importModel = importModel;
            this.cmodel = cmodel;
        
        }


        public void ToUnity()
        {
            for (int i = 0; i < cmodel.ribbonEmitters.Count; i++)
            {
                RibbonEmitter cparticleEmitter = cmodel.ribbonEmitters[i];

                Vector3 pivot = new Vector3();
                if (cparticleEmitter.objectId < cmodel.pivotPoints.Count)
                {
                    var cpivot = cmodel.pivotPoints[cparticleEmitter.objectId];
                    pivot.x = cpivot[0];
                    pivot.y = cpivot[1];
                    pivot.z = cpivot[2];
                    pivot = pivot.VecToU3d();
                }

                GameObject go = importModel.CreateGameObject($"RibbonEmitter_{cparticleEmitter.name}" , pivot, cparticleEmitter.parentId);
                nodes.Add(cparticleEmitter.objectId, go);


            }
        }

    }
}
