
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zeng.mdx.commons;
using Zeng.mdx.parsers.mdx;

namespace Zeng.mdx.imports.mdx
{
    public class ImportAttachments
    {
        public ImportModel importModel;
        public Model cmodel;

        public SortedDictionary<int, GameObject> nodes = new SortedDictionary<int, GameObject>();

        public ImportAttachments(ImportModel importModel, Model cmodel)
        {
            this.importModel = importModel;
            this.cmodel = cmodel;
        
        }


        public void ToUnity()
        {
            for (int i = 0; i < cmodel.attachments.Count; i++)
            {
                Attachment cattachment = cmodel.attachments[i];

                Vector3 pivot = new Vector3();
                if (cattachment.objectId < cmodel.pivotPoints.Count)
                {
                    var cpivot = cmodel.pivotPoints[cattachment.objectId];
                    pivot.x = cpivot[0];
                    pivot.y = cpivot[1];
                    pivot.z = cpivot[2];
                    pivot = pivot.VecToU3d();
                }

                GameObject go = importModel.CreateGameObject($"Attachment_{cattachment.attachmentId}_{cattachment.name}" , pivot, cattachment.parentId);
                nodes.Add(cattachment.objectId, go);


            }
        }

        public IEnumerator LoadOthers()
        {
            for (int i = 0; i < cmodel.attachments.Count; i++)
            {
                Attachment cattachment = cmodel.attachments[i];

                if (!string.IsNullOrEmpty(cattachment.path))
                {
                    Debug.Log($"Attachment_{cattachment.attachmentId}_{cattachment.name}, {cattachment.path}");

                    yield return MdxModelImport.I.OnImportAsset(MdxUnityResPathDefine.GetPath(cattachment.path));
                }
            }
        }
    }
}
