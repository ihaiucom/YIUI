

using UnityEngine.Rendering;
using UnityEngine;
using Zeng.mdx.handlers.w3x;
using Material = UnityEngine.Material;
using System.Collections.Generic;

namespace Zeng.mdx.runtimes
{
    [ExecuteInEditMode]
    public class MdxMapCliffs : MonoBehaviour
    {
        [SerializeField]
        public War3MapViewerMap mapData;
        public Texture2D u_heightMap;

        [SerializeField]
        public CliffItem[] cliffs;

        public Texture2DArray cliffTextureArray;

        public Mesh[] meshs;


        public float u_tileSize_default = 128.0f;
        public float u_tileSize = 1.0f;

        public int subMeshIndex = 0;
        public Material material;

        public Material[] instanceMaterials;
        public int[] instanceCounts;

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

        public ComputeBuffer[] locationBufferList;
        public ComputeBuffer[] textureBuffList;
        public Vector4 u_centerOffsetPixel;

        public void Gen()
        {

            if (mapData == null) return;

            u_centerOffsetPixel = new Vector4(
                 //mapData.centerOffset[0] / u_tileSize_default * u_tileSize, mapData.centerOffset[1] / u_tileSize_default * u_tileSize, // u_centerOffset
                 mapData.centerOffset[0], mapData.centerOffset[1] , // u_centerOffset
                 1f / mapData.mapSize[0], 1f / mapData.mapSize[1] // u_pixel
                );

#if UNITY_EDITOR
            if (material == null)
            {
                material = UnityEditor.AssetDatabase.LoadAssetAtPath<Material>("Assets/Zeng/mdx/runtimes/w3x/MdxMapCliffs.mat");
            }
#endif

            material.SetFloat("u_tileSize", u_tileSize);
            material.SetTexture($"u_textures", cliffTextureArray);
            material.SetVector($"u_centerOffsetPixel", u_centerOffsetPixel);
            material.SetTexture("u_heightMap", u_heightMap);

            if (locationBufferList != null) {
                for (int i = 0; i < locationBufferList.Length; i ++) {
                    if(locationBufferList[i] != null) locationBufferList[i].Dispose();
                }
            }

            if (textureBuffList != null)
            {
                for (int i = 0; i < textureBuffList.Length; i++)
                {
                    if (textureBuffList[i] != null) textureBuffList[i].Dispose();
                }
            }

            instanceCounts = new int[cliffs.Length];
            instanceMaterials = new Material[cliffs.Length];
            locationBufferList = new ComputeBuffer[cliffs.Length];
            textureBuffList = new ComputeBuffer[cliffs.Length];


            for (int i = 0; i < cliffs.Length; i ++)
            {
                CliffItem cliffItem = cliffs[i];
                instanceCounts[i] = cliffItem.textures.Count;
                Material instanceMaterial = Material.Instantiate(material);
                instanceMaterials[i] = instanceMaterial;
                //instanceMaterial.SetFloat("u_tileSize", u_tileSize);
                //instanceMaterial.SetTexture("u_textures", cliffTextureArray);
                //instanceMaterial.SetVector("u_centerOffsetPixel", u_centerOffsetPixel);

                // locationBuffer
                ComputeBuffer locationBuffer = new ComputeBuffer(cliffItem.locations.Count, 4);
                locationBuffer.SetData(cliffItem.locations);
                instanceMaterial.SetBuffer("locationBuffer", locationBuffer);
                locationBufferList[i] = locationBuffer;

                // textureBuff
                ComputeBuffer textureBuff = new ComputeBuffer(cliffItem.textures.Count, 4);
                textureBuff.SetData(cliffItem.textures);
                instanceMaterial.SetBuffer("textureBuff", textureBuff);
                textureBuffList[i] = textureBuff;
            }

        }


        public ShadowCastingMode castShadows = ShadowCastingMode.Off;
        public bool receiveShadows = false;
        public bool debug= false;
        public int debugIndex = 0;
        private void LateUpdate()
        {
            if(mapData == null) return;

            int begin = 0;
            int length = meshs.Length;
            if (debug) {
                begin = debugIndex;
                length = Mathf.Min(length, begin + 1);
            }

            // Render
            for (int i = begin; i < length; i ++)
            {
                Graphics.DrawMeshInstancedProcedural(meshs[i], subMeshIndex, instanceMaterials[i], new Bounds(Vector3.zero, new Vector3(100.0f, 100.0f, 100.0f)), instanceCounts[i]);
            }
        }


    }
}
