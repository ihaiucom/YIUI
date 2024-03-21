
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Zeng.mdx.parsers.mdx;
using System.IO;
using System;

namespace Zeng.mdx.imports.mdx
{
    public class ImportGeoset
    {
        public ImportModel importModel;

        public Geoset cgeoset;
        public int index;

        public GeosetAnimation cgeosetAnimation;

        public string meshPath;
        public Mesh mesh;

        public ImportGeoset(ImportModel importModel, Geoset geoset, int index)
        {
            this.importModel = importModel;
            this.cgeoset = geoset;
            this.index = index;
            if (importModel.saveDirPath.Contains("Cliffs")) {
                meshPath = $"{importModel.saveDirPath}/mesh_{importModel.name}.asset";
            }
            else
            {
                meshPath = $"{importModel.saveDirPath}/mesh_{index}.asset";
            }

            foreach(var geosetAnimation in importModel.model.geosetAnimations)
            {
                if (geosetAnimation.geosetId == index)
                {
                    this.cgeosetAnimation = geosetAnimation;
                }
            }
        }

        public void ToUnity()
        {

            mesh = new Mesh();



            // Vertices.
            List<Vector3> vertices = new List<Vector3>();
            for (int i = 0, l = cgeoset.vertices.Length; i < l; i += 3)
            {
                Vector3 v = new Vector3();
                v.x = cgeoset.vertices[i + 0];
                v.y = cgeoset.vertices[i + 1];
                v.z = cgeoset.vertices[i + 2];
                v = v.VecToU3d();
                vertices.Add(v);
            }
            // Triangles.
            List<int> triangles = new List<int>();
            for (int i = 0, l = cgeoset.faces.Length; i < l; i += 3)
            {
                triangles.Add(cgeoset.faces[i + 0]);
                triangles.Add(cgeoset.faces[i + 2]);
                triangles.Add(cgeoset.faces[i + 1]);
            }

            // Normals.
            List<Vector3> normals = new List<Vector3>();
            for (int i = 0, l = cgeoset.normals.Length; i < l; i += 3)
            {
                Vector3 v = new Vector3();
                v.x = cgeoset.normals[i + 0];
                v.y = cgeoset.normals[i + 1];
                v.z = cgeoset.normals[i + 2];
                v = v.VecToU3d();
                normals.Add(v);
            }
            // UVs.
            List<Vector2> uvs = new List<Vector2>();
            if (cgeoset.uvSets.Count > 0) {
                for (int i = 0, l = cgeoset.uvSets[0].Length; i < l; i += 2)
                {
                    Vector2 v = new Vector2();
                    v.x = cgeoset.uvSets[0][i + 0];
                    v.y = cgeoset.uvSets[0][i + 1];
                    v = v.UvToU3d();
                    uvs.Add(v);
                }
            }

            // UVs.
            List<Vector2> uvs2 = new List<Vector2>();
            if (cgeoset.uvSets.Count > 1)
            {
                for (int i = 0, l = cgeoset.uvSets[1].Length; i < l; i += 2)
                {
                    Vector2 v = new Vector2();
                    v.x = cgeoset.uvSets[1][i + 0];
                    v.y = cgeoset.uvSets[1][i + 1];
                    v = v.UvToU3d();
                    uvs2.Add(v);
                }
            }


            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.normals = normals.ToArray();
            mesh.uv = uvs.ToArray();
            if (uvs2.Count > 0) mesh.uv2 = uvs2.ToArray();

        }


        public void ImportWeights(ImportSkeletons importSkeletons)
        {
            List<BoneWeight> weights = new List<BoneWeight>();

            uint[] matrixIndices = cgeoset.matrixIndices;
            List<uint[]> matrixGroups = new List<uint[]>();

            int offset = 0;
            int maxBones = 4;
            foreach (int size in cgeoset.matrixGroups)
            {
                matrixGroups.Add(matrixIndices[offset..(offset + size)]);
                offset += size;
            }


            byte[] vertexGroups = cgeoset.vertexGroups;
            int vertices = cgeoset.vertices.Length / 3;
            for (int i = 0; i < vertices; i++)
            {
                int matrixGroupsIndex = vertexGroups[i];
                if (matrixGroupsIndex < matrixGroups.Count) {
                    uint[] matrixGroup = matrixGroups[matrixGroupsIndex];
                    BoneWeight weight = new BoneWeight();

                    for (int j = 0; j < matrixGroup.Length; j ++)
                    {
                        int objectId = (int)matrixGroup[j];
                        int boneIndex = importSkeletons.GetIndex(objectId);
                        //Debug.Log($"{i} matrixGroupsIndex={matrixGroupsIndex}, objectId={objectId}, boneIndex={boneIndex}");
                        switch (j) {
                            case 0:
                                {
                                    weight.boneIndex0 = boneIndex;
                                    weight.weight0 = 1f / matrixGroup.Length;
                                    break;
                                }
                            case 1:
                                {
                                    weight.boneIndex1 = boneIndex;
                                    weight.weight1 = 1f / matrixGroup.Length;
                                    break;
                                }
                            case 2:
                                {
                                    weight.boneIndex2 = boneIndex;
                                    weight.weight2 = 1f / matrixGroup.Length;
                                    break;
                                }
                            case 3:
                                {
                                    weight.boneIndex3 = boneIndex;
                                    weight.weight3 = 1f / matrixGroup.Length;
                                    break;
                                }
                            default:
                                {
                                    throw new Exception("Invalid number of bones " + j + " when skining.");
                                }
                        }
                    }
                    weights.Add(weight);
                }
                else
                {
                    Debug.LogError($"ImportWeights {matrixGroupsIndex} >=  {matrixGroups.Count}");
                }



            }

            //Debug.Log($"{meshPath}， weights.Count={weights.Count}， vertices={vertices}， mesh.vertices.Length={mesh.vertices.Length}");
            mesh.boneWeights = weights.ToArray();
        }


        public void Save()
        {
            //Debug.Log(meshPath);
            if (!Directory.Exists(Path.GetDirectoryName(meshPath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(meshPath));
            }

            if (File.Exists(meshPath)) {
                File.Delete(meshPath);
            }

            AssetDatabase.CreateAsset(mesh, meshPath);
            AssetDatabase.SaveAssets();

            mesh = AssetDatabase.LoadAssetAtPath<Mesh>(meshPath);
        }
    }
}
