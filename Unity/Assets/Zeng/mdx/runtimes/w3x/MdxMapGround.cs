

using UnityEngine.Rendering;
using UnityEngine;
using Zeng.mdx.handlers.w3x;
using Material = UnityEngine.Material;

namespace Zeng.mdx.runtimes
{
    [ExecuteInEditMode]
    public class MdxMapGround : MonoBehaviour
    {
        [SerializeField]
        public War3MapViewerMap mapData;
        public Texture2D u_heightMap;

        public Mesh mesh;
        public int subMeshIndex = 0;
        public Material material;
        public Material instanceMaterial;

        public int instanceCount = 0;

        public float u_tileSize_default = 128.0f;
        public float u_tileSize = 1.0f;

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
                material = UnityEditor.AssetDatabase.LoadAssetAtPath<Material>("Assets/Zeng/mdx/runtimes/w3x/MdxMapGround.mat");
            }
#endif

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

        private ComputeBuffer cornerHeightsBuffer;
        private ComputeBuffer cornerTexturesBuffer;
        private ComputeBuffer cornerVariationsBuffer;
        private ComputeBuffer positionBuffer;
        public float[] cornerHeightsBufferArr;
        public float[] cornerTexturesBufferArr;
        public float[] cornerVariationsBufferArr;
        public float min = 0;
        public float max = 0;

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


            instanceMaterial.SetTexture($"u_tilesets", tilesetTexturesArray);
            instanceMaterial.SetFloat("u_baseTileset", tilesetCount > 15 ? 15 : 0);

            // u_extended
            float[] u_extended = new float[15];
            for (int i = 0, l = Mathf.Min(tilesetCount, 15); i < l; i++)
            {
                float isExtended = mapData.tilesetTextures[i] != null && mapData.tilesetTextures[i].width > mapData.tilesetTextures[i].height ? 0.125f : 0.25f;
                u_extended[i] = isExtended;

                instanceMaterial.SetTexture($"u_tileset_{i}", mapData.tilesetTextures[i]);
            }
            instanceMaterial.SetFloatArray("u_extended", u_extended);





            // cornerTexturesBuffer
            if (cornerTexturesBuffer != null)
            {
                cornerTexturesBuffer.Release();
            }

            cornerTexturesBufferArr = new float[mapData.cornerTextures.Length];
            for (int i = 0; i < cornerTexturesBufferArr.Length; i++)
            {
                cornerTexturesBufferArr[i] = mapData.cornerTextures[i];
            }
            cornerTexturesBuffer = new ComputeBuffer(mapData.cornerTextures.Length, 4);
            cornerTexturesBuffer.SetData(cornerTexturesBufferArr);
            instanceMaterial.SetBuffer("cornerTexturesBuffer", cornerTexturesBuffer);


            // cornerVariationsBuffer
            if (cornerVariationsBuffer != null)
            {
                cornerVariationsBuffer.Release();
            }

            cornerVariationsBufferArr = new float[mapData.cornerVariations.Length];
            for (int i = 0; i < cornerVariationsBufferArr.Length; i++)
            {
                cornerVariationsBufferArr[i] = mapData.cornerVariations[i];
            }
            cornerVariationsBuffer = new ComputeBuffer(mapData.cornerVariations.Length, 4);
            cornerVariationsBuffer.SetData(cornerVariationsBufferArr);
            instanceMaterial.SetBuffer("cornerVariationsBuffer", cornerVariationsBuffer);



            int size = columns * rows;
            // cornerHeightsBuffer
            if (cornerHeightsBuffer != null)
                cornerHeightsBuffer.Release();
            cornerHeightsBuffer = new ComputeBuffer(mapData.cornerHeights.Length, 4);

            cornerHeightsBufferArr = mapData.cornerHeights;
            for (int i = 0; i < mapData.cornerHeights.Length; i++)
            {
                float v = mapData.cornerHeights[i];
                min = Mathf.Min(min, v);
                max = Mathf.Max(max, v);
            }
            cornerHeightsBuffer.SetData(mapData.cornerHeights);
            instanceMaterial.SetBuffer("cornerHeightsBuffer", cornerHeightsBuffer);
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
