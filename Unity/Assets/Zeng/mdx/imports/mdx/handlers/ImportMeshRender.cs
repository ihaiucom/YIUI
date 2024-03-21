
using System;
using UnityEngine;
using Zeng.mdx.parsers.mdx;
using static UnityEngine.Mesh;

namespace Zeng.mdx.imports.mdx
{
    public class ImportMeshRender
    {
        public ImportModel importModel;

        public ImportGeoset importGeoset;
        public ImportMaterail importMaterail;


        public GameObject gameObject;
        public MeshFilter meshFilter;
        public SkinnedMeshRenderer renderer;

        public ImportMeshRender(ImportModel importModel, ImportGeoset importGeoset, ImportMaterail importMaterail)
        {
            this.importModel = importModel;
            this.importGeoset = importGeoset;
            this.importMaterail = importMaterail;


            gameObject = new GameObject();
            gameObject.name = $"mesh_{importGeoset.index}_{importMaterail.name}";
            meshFilter = gameObject.AddComponent<MeshFilter>();
            renderer = gameObject.AddComponent<SkinnedMeshRenderer>();

            Mesh mesh = importGeoset.mesh;
            meshFilter.sharedMesh = mesh;
            renderer.sharedMesh = mesh;
            renderer.material = importMaterail.material;
        }

        public void ResetMesh()
        {
            Mesh mesh = importGeoset.mesh;
            meshFilter.sharedMesh = mesh;
            renderer.sharedMesh = mesh;
        }
    }
}
