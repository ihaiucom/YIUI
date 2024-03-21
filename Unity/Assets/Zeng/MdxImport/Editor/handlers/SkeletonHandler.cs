using MdxLib.Model;
using MdxLib.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.XR;
using Zeng.Mdx;
using static PlasticGui.PlasticTableColumn;

namespace Zeng.MdxImport.mdx.handlers
{
    public class SkeletonHandler
    {
        public ModelHandler modelHandler;
        private CModel cmodel;
        private GameObject gameObject;
        private MdxImportSettings settings;

        public GameObject skeleton;
        public SortedDictionary<int, GameObject> bones = new SortedDictionary<int, GameObject>();

        public SkeletonHandler(ModelHandler modelHandler)
        {
            this.modelHandler = modelHandler;
            cmodel = modelHandler.cmodel;
            gameObject = modelHandler.gameObject;
            settings = modelHandler.settings;
        }

        public void ImportSkeleton()
        {
            skeleton = new GameObject("Skeleton");
            skeleton.transform.SetParent(gameObject.transform);

            // Create the bones.
            // Bones reference geosets with geoanims, if there aren't geoanims, then bones will act as helpers.
            CObjectContainer<CBone> cbones = cmodel.Bones;
            for (int i = 0; i < cbones.Count; i++)
            {
                CBone cbone = cbones.Get(i);
                GameObject bone = new GameObject(cbone.Name);
                MdxUtils.SetBillboarded(bone, cbone);

                // Pivot points are the positions of each object.
                CVector3 cpivot = cbone.PivotPoint;

                // Set the bone position.
                // MDX/MDL up axis is Z.
                // Unity up axis is Y.
                bone.transform.position = new Vector3(-cpivot.Y, cpivot.Z, cpivot.X);

                bones[cbone.NodeId] = bone;
            }

            // Create the helpers.
            // Helpers are only used for doing transformations to their children.
            CObjectContainer<CHelper> chelpers = cmodel.Helpers;
            for (int i = 0; i < chelpers.Count; i++)
            {
                CHelper chelper = chelpers.Get(i);
                GameObject helper = new GameObject(chelper.Name);
                MdxUtils.SetBillboarded(helper, chelper);

                // Pivot points are the positions of each object.
                CVector3 cpivot = chelper.PivotPoint;

                // Set the helper position.
                // MDX/MDL up axis is Z.
                // Unity up axis is Y.
                helper.transform.position = new Vector3(-cpivot.Y, cpivot.Z, cpivot.X);

                bones[chelper.NodeId] = helper;
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
            for (int i = 0; i < cbones.Count; i++)
            {
                CBone cbone = cbones.Get(i);

                GameObject bone = bones[cbone.NodeId];
                if (bones.ContainsKey(cbone.Parent.NodeId))
                {
                    GameObject parent = bones[cbone.Parent.NodeId];
                    bone.transform.SetParent(parent.transform);
                }
                else
                {
                    bone.transform.SetParent(skeleton.transform);
                }
            }

            // Set the helpers' parents.
            for (int i = 0; i < chelpers.Count; i++)
            {
                CHelper chelper = chelpers.Get(i);

                GameObject helper = bones[chelper.NodeId];
                if (bones.ContainsKey(chelper.Parent.NodeId))
                {
                    GameObject parent = bones[chelper.Parent.NodeId];
                    helper.transform.SetParent(parent.transform);
                }
                else
                {
                    helper.transform.SetParent(skeleton.transform);
                }
            }

            Matrix4x4[] bindposes = bones.Values.Select(go => go.transform.worldToLocalMatrix * gameObject.transform.localToWorldMatrix).ToArray();
            Transform[] meshBones = bones.Values.ToArray().Select(go => go.transform).ToArray();


            List<GeosetHandler> geosetHandlers = modelHandler.geosetHandlers;
            for (int i = 0; i < geosetHandlers.Count; i++)
            {
                GeosetHandler geosetHandler = geosetHandlers[i];

                Mesh mesh = geosetHandler.mesh;
                mesh.bindposes = bindposes;

                SkinnedMeshRenderer skinMeshRender = geosetHandler.renderer;
                skinMeshRender.bones = meshBones;
                skinMeshRender.rootBone = skeleton.transform;

                geosetHandler.ImportWeights();


            }
        }



    }
}
