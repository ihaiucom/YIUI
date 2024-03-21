using MdxLib.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using static PlasticGui.PlasticTableColumn;
using static UnityEngine.Mesh;

namespace Zeng.MdxImport.mdx.handlers
{
    public class GeosetHandler
    {
        public int index;
        public CGeoset cgeoset;

        public GameObject gameObject;
        public Mesh mesh;
        public MeshFilter meshFilter;
        public SkinnedMeshRenderer renderer;



        public GeosetHandler(int index, CGeoset cgeoset)
        {
            this.index = index;
            this.cgeoset = cgeoset;
        }


        public void SaveUnityFile(string saveDirPath, string fileName)
        {
            string meshPath = saveDirPath + gameObject.name + ".asset";

            AssetDatabase.CreateAsset(mesh, meshPath);
            AssetDatabase.SaveAssets();

            renderer.sharedMesh = meshFilter.sharedMesh = AssetDatabase.LoadAssetAtPath<Mesh>(meshPath);
        }

        public void Gen()
        {
            gameObject = new GameObject();
            gameObject.name = $"mesh_{index}";
            meshFilter = gameObject.AddComponent<MeshFilter>();
            renderer = gameObject.AddComponent<SkinnedMeshRenderer>();


            mesh = new Mesh();


            // Vertices.
            List<Vector3> vertices = new List<Vector3>();
            for (int j = 0; j < cgeoset.Vertices.Count; j++)
            {
                // MDX/MDL up axis is Z.
                // Unity up axis is Y.
                CGeosetVertex cvertex = cgeoset.Vertices.Get(j);
                Vector3 vertex = new Vector3(-cvertex.Position.Y, cvertex.Position.Z, cvertex.Position.X);
                vertices.Add(vertex);
            }

            // Triangles.
            List<int> triangles = new List<int>();
            for (int j = 0; j < cgeoset.Faces.Count; j++)
            {
                // MDX/MDL coordinate system is anti-clockwise.
                // Unity coordinate system is clockwise.
                CGeosetFace cface = cgeoset.Faces.Get(j);
                triangles.Add(cface.Vertex1.ObjectId);
                triangles.Add(cface.Vertex3.ObjectId); // Swap the order of the vertex 2 and 3.
                triangles.Add(cface.Vertex2.ObjectId);
            }

            // Normals.
            List<Vector3> normals = new List<Vector3>();
            for (int j = 0; j < cgeoset.Vertices.Count; j++)
            {
                // MDX/MDL up axis is Z.
                // Unity up axis is Y.
                CGeosetVertex cvertex = cgeoset.Vertices.Get(j);
                Vector3 normal = new Vector3(-cvertex.Normal.Y, cvertex.Normal.Z, cvertex.Normal.X);
                normals.Add(normal);
            }

            // UVs.
            List<Vector2> uvs = new List<Vector2>();
            for (int j = 0; j < cgeoset.Vertices.Count; j++)
            {
                // MDX/MDL texture coordinate origin is at top left.
                // Unity texture coordinate origin is at bottom left.
                CGeosetVertex cvertex = cgeoset.Vertices.Get(j);
                Vector2 uv = new Vector2(cvertex.TexturePosition.X, Mathf.Abs(cvertex.TexturePosition.Y - 1)); // Vunity = abs(Vmdx - 1)
                uvs.Add(uv);
            }

            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.normals = normals.ToArray();
            mesh.uv = uvs.ToArray();

            meshFilter.sharedMesh = mesh;
            renderer.sharedMesh = mesh;
        }

        public void SetMaterail(List<MaterailHandler> materailHandlers)
        {
            CMaterial cMaterial = cgeoset.Material.Object;

            List<Material> materials = new List<Material>();
            for (int i = 0; i < materailHandlers.Count; i++)
            {
                MaterailHandler materailHandler = materailHandlers[i];
                if (materailHandler.cmaterial.ObjectId == cMaterial.ObjectId)
                {
                    Debug.Log($"GeosetHandler {index}, cMaterial.ObjectId={cMaterial.ObjectId}, {i}, {materailHandler.material.name}");
                    Debug.Log(materailHandler.material);
                    materials.Add(materailHandler.material);
                }
            }

            renderer.sharedMaterials = materials.ToArray();
        }

        public void ImportWeights()
        {
            List<BoneWeight> weights = new List<BoneWeight>();

            CObjectContainer<CGeosetVertex> cvertices = cgeoset.Vertices;
            for (int j = 0; j < cvertices.Count; j++)
            {
                CGeosetVertex cvertex = cvertices.Get(j);

                // Check if the vertex belongs to a triangle.
                // Mesh combines discard vertices that don't belong to any triangle. To avoid the error "Mesh.boneWeights is out of bounds" (more weights than vertices), these weights are ignored.
                bool hasTriangle = false;
                foreach (CGeosetFace cface in cgeoset.Faces)
                {
                    if (cvertex.ObjectId == cface.Vertex1.ObjectId || cvertex.ObjectId == cface.Vertex2.ObjectId || cvertex.ObjectId == cface.Vertex3.ObjectId)
                    {
                        hasTriangle = true;
                        break;
                    }
                }
                if (!hasTriangle)
                {
                    continue;
                }

                BoneWeight weight = new BoneWeight();

                // Group.
                // A vertex group reference a group (of matrices).
                CGeosetGroup cgroup = cvertex.Group.Object; // Vertex group reference.

                // Matrices.
                // A matrix reference an object. The bone weights are evenly distributed, each weight is 1/N.
                CObjectContainer<CGeosetGroupNode> cmatrices = cgroup.Nodes;
                for (int k = 0; k < cmatrices.Count; k++)
                {
                    CGeosetGroupNode cmatrix = cmatrices.Get(k);
                    switch (k)
                    {
                        case 0:
                            {
                                weight.boneIndex0 = cmatrix.Node.NodeId;
                                weight.weight0 = 1f / cmatrices.Count;
                                break;
                            }
                        case 1:
                            {
                                weight.boneIndex1 = cmatrix.Node.NodeId;
                                weight.weight1 = 1f / cmatrices.Count;
                                break;
                            }
                        case 2:
                            {
                                weight.boneIndex2 = cmatrix.Node.NodeId;
                                weight.weight2 = 1f / cmatrices.Count;
                                break;
                            }
                        case 3:
                            {
                                weight.boneIndex3 = cmatrix.Node.NodeId;
                                weight.weight3 = 1f / cmatrices.Count;
                                break;
                            }
                        default:
                            {
                                throw new Exception("Invalid number of bones " + k + " when skining.");
                            }
                    }
                }

                weights.Add(weight);
            }

            mesh.boneWeights = weights.ToArray();
        }



    }
}
