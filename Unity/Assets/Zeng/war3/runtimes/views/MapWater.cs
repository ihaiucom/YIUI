
using UnityEngine;
using UnityEngine.Rendering;
using Zeng.war3.runtimes.datas;

namespace Zeng.war3.runtimes.views
{
    [ExecuteInEditMode]
    public class MapWater : MonoBehaviour
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


        public int waterTexturCount;
        public float waterTime = 0;
        public int waterTextureIndex;

        private ComputeBuffer waterFlagsBuffer;
        private void Gen()
        {
            if (mapData == null) return;
            waterTexturCount = mapData.waterData.waterTextures.Length;

            if (mesh == null)
            {
                mesh = GenMesh();
            }

            mesh = GenMesh();

#if UNITY_EDITOR
            if (material == null)
            {
                material = UnityEditor.AssetDatabase.LoadAssetAtPath<Material>("Assets/Zeng/war3/runtimes/views/MapWater.mat");
            }
#endif

            if (instanceMaterial == null)
            {
                instanceMaterial = Material.Instantiate<Material>(material);
            }

            instanceMaterial = Material.Instantiate<Material>(material);
            // 地图大小和偏移
            instanceMaterial.SetVector("u_size_offset", new Vector4(mapData.mapSize.x, mapData.mapSize.y, mapData.centerOffset.x, mapData.centerOffset.y));

            // 地面高度贴图
            instanceMaterial.SetTexture("u_heightMap", mapData.groundData.cornerHeightTexture);
            // 水面高度贴图
            instanceMaterial.SetTexture("u_waterHeightMap", mapData.waterData.waterHeightTexture);
            // 高度偏移
            instanceMaterial.SetFloat("u_offsetHeight", mapData.waterData.waterHeightOffset);
            // 地面贴图数组
            instanceMaterial.SetTexture($"u_tilesets", mapData.waterData.waterTexturesArray);


            // cornerTexturesBuffer
            if (waterFlagsBuffer != null)
            {
                waterFlagsBuffer.Release();
            }

            waterFlagsBuffer = new ComputeBuffer(mapData.waterData.waterFlags.Length, 4);
            waterFlagsBuffer.SetData(mapData.waterData.waterFlags);
            instanceMaterial.SetBuffer("waterFlagsBuffer", waterFlagsBuffer);


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

            //=======================================
            Vector2[] uv = new Vector2[4];

            uv[0] = new Vector2(0, 0);
            uv[1] = new Vector2(1, 0);
            uv[2] = new Vector2(0, 1);
            uv[3] = new Vector2(1, 1);
            mesh.uv = uv;


            return mesh;
        }

        void Update()
        {
            waterTime += mapData.waterData.waterIncreasePerFrame;
            if (waterTime > waterTexturCount)
            {
                waterTime = waterTime - waterTexturCount;
            }
            waterTextureIndex = Mathf.FloorToInt(waterTime);

            if (instanceMaterial != null)
            {
                instanceMaterial.SetInt("u_tilesetsIndex", waterTextureIndex);

            }
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
