

using UnityEngine.Rendering;
using UnityEngine;
using Zeng.mdx.handlers.w3x;
using Material = UnityEngine.Material;

namespace Zeng.mdx.runtimes
{
    [ExecuteInEditMode]
    public class MdxMapWater : MonoBehaviour
    {
        [SerializeField]
        public War3MapViewerMap mapData;
        public Texture2D u_heightMap;
        public Texture2D u_waterHeightMap;

        public Mesh mesh;
        public int subMeshIndex = 0;
        public Material material;
        public Material instanceMaterial;

        public int instanceCount = 0;

        public float u_tileSize_default = 128.0f;
        public float u_tileSize = 1.0f;

        public float waterIncreasePerFrame = 0.25f;
        public int waterTexturCount;
        public float waterTime = 0;
        public int waterTextureIndex;

        public Color u_minDeepColor;
        public Color u_maxDeepColor;
        public Color u_minShallowColor;
        public Color u_maxShallowColor;

        // Start is called before the first frame update
        void Start()
        {
            Gen();
        }

        private void OnEnable()
        {
            Gen();
        }

        // Update is called once per frame
        void Update()
        {
            waterTime += waterIncreasePerFrame;
            if (waterTime > waterTexturCount) {
                waterTime = waterTime - waterTexturCount;
            }
            waterTextureIndex = Mathf.FloorToInt(waterTime);

            if (instanceMaterial != null)
            {
                instanceMaterial.SetInt("u_tilesetsIndex", waterTextureIndex);

            }
        }

        public void Gen()
        {

            if (mapData == null) return;

            instanceCount = mapData.instanceCount;
            if (mesh == null)
            {
                mesh = GenMesh();
            }

#if UNITY_EDITOR
            if (material == null)
            {
                material = UnityEditor.AssetDatabase.LoadAssetAtPath<Material>("Assets/Zeng/mdx/runtimes/w3x/MdxMapWater.mat");
            }
#endif
            material.SetColor("u_minDeepColor", u_minDeepColor);
            material.SetColor("u_maxDeepColor", u_maxDeepColor);
            material.SetColor("u_minShallowColor", u_minShallowColor);
            material.SetColor("u_maxShallowColor", u_maxShallowColor);

            if (instanceMaterial == null)
            {
                instanceMaterial = Material.Instantiate<Material>(material);
            }
            GenMateialBuffers();
        }

        public Mesh GenMesh()
        {
            Mesh mesh = new Mesh();
            Vector3[] vertices = new Vector3[4];

            vertices[0] = new Vector3(0, 0, 0); // 左下
            vertices[1] = new Vector3(1, 1, 0); // 右下
            vertices[2] = new Vector3(0, 2, 1);  // 左上
            vertices[3] = new Vector3(1, 3, 1); // 右上

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

        private ComputeBuffer waterFlagsBuffer;
        public float[] waterFlagsBufferArr;

        public float[] u_offset;

        public Texture2DArray tilesetTexturesArray;
        public void GenMateialBuffers()
        {

            int columns = mapData.columns;
            int rows = mapData.rows;
            int tilesetCount = mapData.tilesetTextures.Count;

            instanceMaterial.SetFloat("u_tileSize", u_tileSize);
            instanceMaterial.SetFloatArray("u_size", new float[] { mapData.mapSize[0], mapData.mapSize[1] });

            u_offset = new float[] { mapData.centerOffset[0] / u_tileSize_default * u_tileSize, mapData.centerOffset[1] / u_tileSize_default * u_tileSize };
            instanceMaterial.SetFloatArray("u_offset", u_offset);
            instanceMaterial.SetTexture("u_heightMap", u_heightMap);
            instanceMaterial.SetTexture("u_waterHeightMap", u_waterHeightMap);
            instanceMaterial.SetFloat("u_offsetHeight", mapData.waterHeightOffset);
            instanceMaterial.SetTexture($"u_tilesets", tilesetTexturesArray);

            




            // cornerTexturesBuffer
            if (waterFlagsBuffer != null)
            {
                waterFlagsBuffer.Release();
            }

            waterFlagsBufferArr = new float[mapData.waterFlags.Length];
            for (int i = 0; i < waterFlagsBufferArr.Length; i++)
            {
                waterFlagsBufferArr[i] = mapData.waterFlags[i];
            }
            waterFlagsBuffer = new ComputeBuffer(mapData.waterFlags.Length, 4);
            waterFlagsBuffer.SetData(waterFlagsBufferArr);
            instanceMaterial.SetBuffer("waterFlagsBuffer", waterFlagsBuffer);


        }


        private MaterialPropertyBlock props;
        public ShadowCastingMode castShadows = ShadowCastingMode.Off;
        public bool receiveShadows = false;
        private void LateUpdate()
        {

            // Render
            Graphics.DrawMeshInstancedProcedural(mesh, subMeshIndex, instanceMaterial, new Bounds(Vector3.zero, new Vector3(100.0f, 100.0f, 100.0f)), instanceCount);
        }
    }
}
