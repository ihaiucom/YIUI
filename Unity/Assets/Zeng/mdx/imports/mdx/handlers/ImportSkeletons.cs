
using Zeng.mdx.parsers.mdx;
using System.Collections.Generic;
using UnityEngine;
using Zeng.Mdx;
using System.Linq;
using UnityEditor;

namespace Zeng.mdx.imports.mdx
{
    public class ImportSkeletons
    {
        public ImportModel importModel;
        public Model cmodel;

        public GameObject skeleton;
        public SortedDictionary<int, GameObject> bones = new SortedDictionary<int, GameObject>();
        public SortedDictionary<int, int> boneObjectId2Index = new SortedDictionary<int, int>();


        public ImportSkeletons(ImportModel importModel, Model cmodel)
        {
            this.importModel = importModel;
            this.cmodel = cmodel;

            skeleton = new GameObject("Skeleton");
            skeleton.transform.SetParent(importModel.gameObject.transform);

            
            // bones
            for (int i = 0; i < cmodel.bones.Count; i ++)
            {
                Bone cbone = cmodel.bones[i];

                Vector3 pivot = new Vector3();
                if (cbone.objectId < cmodel.pivotPoints.Count)
                {
                    var cpivot = cmodel.pivotPoints[cbone.objectId];
                    pivot.x = cpivot[0];
                    pivot.y = cpivot[1];
                    pivot.z = cpivot[2];
                    pivot = pivot.VecToU3d();
                }

                GameObject bone = new GameObject(cbone.name);
                bone.transform.position = pivot;
                SetBillboarded(bone, cbone);
                bones[cbone.objectId] = bone;
            }

            // helpers
            for (int i = 0; i < cmodel.helpers.Count; i++)
            {
                Helper chelper = cmodel.helpers[i];

                Vector3 pivot = new Vector3();
                if (chelper.objectId < cmodel.pivotPoints.Count)
                {
                    var cpivot = cmodel.pivotPoints[chelper.objectId];
                    pivot.x = cpivot[0];
                    pivot.y = cpivot[1];
                    pivot.z = cpivot[2];
                    pivot = pivot.VecToU3d();
                }

                GameObject bone = new GameObject(chelper.name);
                bone.transform.position = pivot;
                SetBillboarded(bone, chelper);
                bones[chelper.objectId] = bone;
            }



            if (bones.Count == 0)
            {
                return;
            }
            else
            {
                // Add the root bone.
                bones.Add(bones.Keys.Max() + 1, skeleton);

            }

            // Set the bones' parents.
            for (int i = 0; i < cmodel.bones.Count; i++)
            {
                Bone cbone = cmodel.bones[i];

                GameObject bone = bones[cbone.objectId];
                if (bones.ContainsKey(cbone.parentId))
                {
                    GameObject parent = bones[cbone.parentId];
                    bone.transform.SetParent(parent.transform);
                }
                else
                {
                    bone.transform.SetParent(skeleton.transform);
                }
            }


            // Set the helpers' parents.
            for (int i = 0; i < cmodel.helpers.Count; i++)
            {
                Helper chelper = cmodel.helpers[i];

                GameObject helper = bones[chelper.objectId];
                if (bones.ContainsKey(chelper.parentId))
                {
                    GameObject parent = bones[chelper.parentId];
                    helper.transform.SetParent(parent.transform);
                }
                else
                {
                    helper.transform.SetParent(skeleton.transform);
                }
            }


            Matrix4x4[] bindposes = bones.Values.Select(go => go.transform.worldToLocalMatrix * importModel.gameObject.transform.localToWorldMatrix).ToArray();
            Transform[] meshBones = bones.Values.ToArray().Select(go => go.transform).ToArray();
            this.meshBones = meshBones;

            //Debug.Log("bindposes.Length= " + bindposes.Length);

            for (int i = 0; i < importModel.geosets.Count; i++)
            {
                ImportGeoset importGeoset = importModel.geosets[i];

                Mesh mesh = importGeoset.mesh;
                mesh.bindposes = bindposes;
                importGeoset.ImportWeights(this);
                importGeoset.Save();
            }


            for (int i = 0; i < importModel.meshRenders.Count; i++)
            {
                ImportMeshRender importMeshRender = importModel.meshRenders[i];

                importMeshRender.ResetMesh();
                SkinnedMeshRenderer skinMeshRender = importMeshRender.renderer;
                skinMeshRender.bones = meshBones;
                skinMeshRender.rootBone = skeleton.transform;
            }
        }
        Transform[] meshBones;
        public int GetIndex(int cObjectId)
        {
            if (bones.ContainsKey(cObjectId)) {

                int index = ArrayUtility.IndexOf(meshBones, bones[cObjectId].transform);
                return index;
            }
            else
            {
                Debug.Log("获取骨骼索引没找到 cObjectId="+ cObjectId);
                return 0;
            }
        }


        public static void SetBillboarded(GameObject bone, GenericObject cbone)
        {
            if (cbone.billboarded || cbone.billboardedX || cbone.billboardedY || cbone.billboardedZ)
            {
                NodeBillboarded nodeBillboarded = bone.AddComponent<NodeBillboarded>();
                nodeBillboarded.Billboarded = cbone.billboarded;
                nodeBillboarded.BillboardedLockY = cbone.billboardedX;
                nodeBillboarded.BillboardedLockZ = cbone.billboardedY;
                nodeBillboarded.BillboardedLockX = cbone.billboardedZ;
            }
        }

    }
}
