
using UnityEngine;
using UnityEngine.Rendering;
using Zeng.war3.runtimes.datas;

namespace Zeng.war3.runtimes.views
{
    [ExecuteInEditMode]
    public class MapGround : MonoBehaviour
    {
        [SerializeField]
        public MapData mapData;


        void Start()
        {
            Gen();
        }

        private void OnEnable()
        {
            Gen();
        }

        public Mesh mesh;
        public int subMeshIndex = 0;
        public Material material;
        public Material instanceMaterial;

        //private ComputeBuffer cornerHeightsBuffer; // 高度数据Buffer
        private ComputeBuffer cornerTexturesBuffer; // 贴图索引
        private ComputeBuffer cornerVariationsBuffer; // 贴图Cell UV 索引
        private void Gen()
        {
            if (mapData == null) return;

            if (mesh == null)
            {
                mesh = GenMesh();
            }


#if UNITY_EDITOR
            if (material == null)
            {
                material = UnityEditor.AssetDatabase.LoadAssetAtPath<Material>("Assets/Zeng/war3/runtimes/views/MapGround.mat");
            }
#endif

            if (instanceMaterial == null)
            {
                instanceMaterial = Material.Instantiate<Material>(material);
            }

            // 地图大小和偏移
            instanceMaterial.SetVector("u_size_offset", new Vector4(mapData.mapSize.x, mapData.mapSize.y, mapData.centerOffset.x, mapData.centerOffset.y));

            // 高度贴图
            instanceMaterial.SetTexture("u_heightMap", mapData.groundData.cornerHeightTexture);

            // 地面贴图数组
            //instanceMaterial.SetTexture($"u_tilesets", mapData.groupData.tilesetTexturesArray);
            // u_extended
            float[] u_extended = new float[15];
            for (int i = 0, l = Mathf.Min(mapData.groundData.tilesetTextures.Length, 15); i < l; i++)
            {
                float isExtended = mapData.groundData.tilesetTextures[i] != null && mapData.groundData.tilesetTextures[i].width > mapData.groundData.tilesetTextures[i].height ? 0.125f : 0.25f;
                u_extended[i] = isExtended;

                instanceMaterial.SetTexture($"u_tileset_{i}", mapData.groundData.tilesetTextures[i]);
            }
            instanceMaterial.SetFloatArray("u_extended", u_extended);

            // cornerHeightsBuffer
            //if (cornerHeightsBuffer != null)
            //    cornerHeightsBuffer.Release();
            //cornerHeightsBuffer = new ComputeBuffer(mapData.groupData.cornerHeights.Length, 4);
            //cornerHeightsBuffer.SetData(mapData.groupData.cornerHeights);
            //instanceMaterial.SetBuffer("cornerHeightsBuffer", cornerHeightsBuffer);


            // cornerTexturesBuffer
            if (cornerTexturesBuffer != null)
            {
                cornerTexturesBuffer.Release();
            }
            cornerTexturesBuffer = new ComputeBuffer(mapData.groundData.cornerTextures.Length, 4);
            cornerTexturesBuffer.SetData(mapData.groundData.cornerTextures);
            instanceMaterial.SetBuffer("cornerTexturesBuffer", cornerTexturesBuffer);


            // cornerVariationsBuffer
            if (cornerVariationsBuffer != null)
            {
                cornerVariationsBuffer.Release();
            }
            cornerVariationsBuffer = new ComputeBuffer(mapData.groundData.cornerVariations.Length, 4);
            cornerVariationsBuffer.SetData(mapData.groundData.cornerVariations);
            instanceMaterial.SetBuffer("cornerVariationsBuffer", cornerVariationsBuffer);


        }

        private Mesh GenMesh()
        {
            Mesh mesh = new Mesh();
            Vector3[] vertices = new Vector3[4];

            vertices[0] = new Vector3(0, 0, 0); // 左下
            vertices[1] = new Vector3(1, 0, 0); // 右下
            vertices[2] = new Vector3(0, 0, 1);  // 左上
            vertices[3] = new Vector3(1, 0, 1); // 右上

            mesh.vertices = vertices;

            //=======================================

            int[] tri = new int[6];

            //  Lower left triangle.
            tri[0] = 2;
            tri[1] = 1;
            tri[2] = 0;

            //  Upper right triangle.   
            tri[3] = 2;
            tri[4] = 3;
            tri[5] = 1;

            mesh.triangles = tri;


            return mesh;
        }


        public ShadowCastingMode castShadows = ShadowCastingMode.Off;
        public bool receiveShadows = false;
        private void LateUpdate()
        {
            if (mapData == null) return;
            // Render
            Graphics.DrawMeshInstancedProcedural(mesh, subMeshIndex, instanceMaterial, new Bounds(Vector3.zero, new Vector3(100.0f, 100.0f, 100.0f)), mapData.instanceCount);
        }
    }
}
