
using UnityEngine;
using UnityEngine.Rendering;
using Zeng.war3.runtimes.datas;

namespace Zeng.war3.runtimes.views
{
    [ExecuteInEditMode]
    public class MapCliffs : MonoBehaviour
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

        public int subMeshIndex = 0;
        public Material material;

        public Material[] instanceMaterials;
        public ComputeBuffer[] locationBufferList;
        public ComputeBuffer[] textureIndexBuffList;
        private CliffsData cliffsData;
        private CliffsItemData[] cliffs;

        private void Gen()
        {
            if (mapData == null) return;

            cliffsData = mapData.cliffsData;
            cliffs = mapData.cliffsData.cliffs;


#if UNITY_EDITOR
            if (material == null)
            {
                material = UnityEditor.AssetDatabase.LoadAssetAtPath<Material>("Assets/Zeng/war3/runtimes/views/MapCliffs.mat");
            }
#endif

            // 地图大小和偏移
            material.SetVector("u_size_offset", new Vector4(mapData.mapSize.x, mapData.mapSize.y, mapData.centerOffset.x, mapData.centerOffset.y));

            // 高度贴图
            material.SetTexture("u_heightMap", mapData.cliffsData.cliffHeightTexture);

            // 贴图数组
            material.SetTexture($"u_cliffTexturesArray", mapData.cliffsData.cliffTexturesArray);


            if (locationBufferList != null)
            {
                for (int i = 0; i < locationBufferList.Length; i++)
                {
                    if (locationBufferList[i] != null) locationBufferList[i].Dispose();
                }
            }

            if (textureIndexBuffList != null)
            {
                for (int i = 0; i < textureIndexBuffList.Length; i++)
                {
                    if (textureIndexBuffList[i] != null) textureIndexBuffList[i].Dispose();
                }
            }

            if (instanceMaterials != null)
            {
                for (int i = 0; i < instanceMaterials.Length; i++)
                {
                    if (instanceMaterials[i] != null) {
                        if (Application.isEditor)
                        {
                            DestroyImmediate(instanceMaterials[i]);
                        }
                        else {
                            Destroy(instanceMaterials[i]);
                        }
                    }
                }
            }

            instanceMaterials = new Material[cliffs.Length];
            locationBufferList = new ComputeBuffer[cliffs.Length];
            textureIndexBuffList = new ComputeBuffer[cliffs.Length];


            for (int i = 0; i < cliffs.Length; i++)
            {
                CliffsItemData cliffItem = cliffs[i];
                Material instanceMaterial = Material.Instantiate(material);
                instanceMaterials[i] = instanceMaterial;

                // locationBuffer
                ComputeBuffer locationBuffer = new ComputeBuffer(cliffItem.locations.Length, 4);
                locationBuffer.SetData(cliffItem.locations);
                instanceMaterial.SetBuffer("locationBuffer", locationBuffer);
                locationBufferList[i] = locationBuffer;

                // textureIndexBuff
                ComputeBuffer textureBuff = new ComputeBuffer(cliffItem.texturesIndex.Length, 4);
                textureBuff.SetData(cliffItem.texturesIndex);
                instanceMaterial.SetBuffer("textureIndexBuff", textureBuff);
                textureIndexBuffList[i] = textureBuff;
            }



        }


        public ShadowCastingMode castShadows = ShadowCastingMode.Off;
        public bool receiveShadows = false;
        private void LateUpdate()
        {
            if (mapData == null) return;

            // Render
            for (int i = 0; i < cliffs.Length; i++)
            {
                Graphics.DrawMeshInstancedProcedural(cliffsData.meshs[i], subMeshIndex, instanceMaterials[i], new Bounds(Vector3.zero, new Vector3(100.0f, 100.0f, 100.0f)), cliffsData.instanceCounts[i]);
            }

        }
    }
}
