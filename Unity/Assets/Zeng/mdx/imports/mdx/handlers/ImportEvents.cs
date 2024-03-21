
using MdxLib.Model;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using UnityEngine;
using Zeng.mdx.commons;
using Zeng.mdx.parsers;
using Zeng.mdx.parsers.mdx;

namespace Zeng.mdx.imports.mdx
{
    public class ImportEvents
    {
        public ImportModel importModel;
        public Model cmodel;

        public SortedDictionary<int, GameObject> nodes = new SortedDictionary<int, GameObject>();

        public ImportEvents(ImportModel importModel, Model cmodel)
        {
            this.importModel = importModel;
            this.cmodel = cmodel;
        
        }


        public void ToUnity()
        {
            for (int i = 0; i < cmodel.eventObjects.Count; i++)
            {
                EventObject cevent = cmodel.eventObjects[i];

                Vector3 pivot = new Vector3();
                if (cevent.objectId < cmodel.pivotPoints.Count)
                {
                    var cpivot = cmodel.pivotPoints[cevent.objectId];
                    pivot.x = cpivot[0];
                    pivot.y = cpivot[1];
                    pivot.z = cpivot[2];
                    pivot = pivot.VecToU3d();
                }

                GameObject go = importModel.CreateGameObject($"Event_{cevent.name}" , pivot, cevent.parentId);
                nodes.Add(cevent.objectId, go);


            }
        }

        public IEnumerator LoadOthers()
        {
            for (int i = 0; i < cmodel.eventObjects.Count; i++)
            {
                EventObject cevent = cmodel.eventObjects[i];
                if (cevent.type == EventObject.EventObjectType.SPN) {
                    MappedDataRow row = MdxModelImport.I.SpawnData.GetRow(cevent.id);
                    if (row != null) {
                        string itemPath = $"{row.GetString("Model").Replace(".mdl", ".mdx")}";
                        itemPath = MdxUnityResPathDefine.GetPath(itemPath);
                        yield return MdxModelImport.I.OnImportAsset(itemPath);
                    }
                }
            }
        }
    }
}
