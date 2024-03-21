

using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using Zeng.mdx.commons;
using Zeng.mdx.handlers.w3x;
using Zeng.mdx.imports.mdx;
using Zeng.mdx.parsers.mdx;
using Zeng.mdx.parsers.w3x.doo;
using Zeng.mdx.runtimes;
using Zeng.war3.runtimes.datas;
using Zeng.war3.runtimes.views;

namespace Zeng.mdx.imports.w3x
{
    public class ImportMap
    {
        public War3MapViewer viewer;
        public War3MapViewerMap mapData;
        public string assetPath;

        public string saveDirPath;
        public string savePrefabPath;
        public string name;

        public ImportMap(War3MapViewer viewer, string assetPath) {
            this.viewer = viewer;
            this.mapData = viewer.map;
            this.assetPath = assetPath;


            name = Path.GetFileName(assetPath).Replace(".w3m", "").Replace(".w3x", "");
            savePrefabPath = $"Assets/MdxPrefabs/Maps/{name}/{name}.prefab";
            saveDirPath = Path.GetDirectoryName(savePrefabPath).Replace("\\", "/");
            Debug.Log(savePrefabPath);
            Debug.Log(saveDirPath);
        }

        public GameObject gameObject;

        public IEnumerator ToUnity()
        {
            gameObject = new GameObject(name);
            MapData data = new MapData();
            data = ToMapData(data);

            // MapGround
            MapGround mapGround = gameObject.AddComponent<MapGround>();
            mapGround.mapData = data;

            // MapCliffs
            MapCliffs mapCliffs = gameObject.AddComponent<MapCliffs>();
            mapCliffs.mapData = data;

            // MapWater
            MapWater mapWater = gameObject.AddComponent<MapWater>();
            mapWater.mapData = data;


            // Save Prefab
            if (!Directory.Exists(Path.GetDirectoryName(savePrefabPath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(savePrefabPath));
            }
            PrefabUtility.SaveAsPrefabAsset(gameObject, savePrefabPath);
            yield return null;
        }

        private MapData ToMapData(MapData data)
        {
            data.mapSize = new Vector2Int(mapData.mapSize[0], mapData.mapSize[1]);
            data.centerOffset = new Vector2(mapData.centerOffset[0], mapData.centerOffset[1]);

            int columns = data.mapSize.x; 
            int rows = data.mapSize.y;
            data.instanceCount = (columns - 1) * (rows - 1);

            // Ground
            GroundData groupData = data .groundData = new GroundData();
            groupData.cornerHeights = mapData.cornerHeights;
            groupData.cornerTextures = mapData.cornerTextures.ToFloatArray();
            groupData.cornerVariations = mapData.cornerVariations.ToFloatArray();
            groupData.tilesetTextures = mapData.tilesetTextures.ToArray();

            groupData.cornerHeightTexture = groundHeightMap();
            groupData.tilesetTexturesArray = tilesetTextures();

            // Cliffs
            CliffsData cliffsData = data .cliffsData = new CliffsData();
            cliffsData.cliffHeights = mapData.cliffHeights;
            cliffsData.cliffTextures = mapData.cliffTextures.ToArray();

            cliffsData.cliffHeightTexture = cliffsHeightMap();
            cliffsData.cliffTexturesArray = cliffsTextures();

            cliffsData.cliffs = new CliffsItemData[mapData.cliffs.Count];
            cliffsData.meshs = new Mesh[mapData.cliffs.Count];
            cliffsData.instanceCounts = new int[mapData.cliffs.Count];
            for (int i = 0; i < cliffsData.cliffs.Length; i++)
            {
                var srcItem = mapData.cliffs[i];
                var item = new CliffsItemData() { 
                    path = srcItem.path,
                    locations = srcItem.locations.ToArray(),
                    texturesIndex = srcItem.textures.ToArray()
                };
                cliffsData.cliffs[i] = item;
                cliffsData.instanceCounts[i] = srcItem.textures.Count;

                string cliffsPrefabPath = MdxRuntimeUtils.GetPrefabPath(mapData.cliffs[i].path);
                GameObject cliffsGO = AssetDatabase.LoadAssetAtPath<GameObject>(cliffsPrefabPath);
                if (cliffsGO != null)
                {
                    SkinnedMeshRenderer skinnedMeshRenderer = cliffsGO.GetComponentInChildren<SkinnedMeshRenderer>();
                    if (skinnedMeshRenderer == null || skinnedMeshRenderer.sharedMesh == null)
                    {
                        Debug.LogError("请检查 SkinnedMeshRenderer " + cliffsPrefabPath);
                    }
                    else
                    {
                        cliffsData.meshs[i] = skinnedMeshRenderer.sharedMesh;
                    }
                }
                else
                {
                    Debug.LogError("没有找到 " + cliffsPrefabPath);
                }
            }

            // Water
            WaterData waterData = data.waterData = new WaterData();
            waterData.waterHeightOffset = mapData.waterHeightOffset;
            waterData.waterHeights = mapData.waterHeights;
            waterData.waterHeightTexture = waterHeightMap();
            waterData.waterFlags = mapData.waterFlags.ToFloatArray();

            waterData.waterIncreasePerFrame = mapData.waterIncreasePerFrame;
            waterData.waterTextures = mapData.waterTextures.ToArray();
            waterData.waterTexturesArray = waterTextures();


            waterData.u_minDeepColor = mapData.minDeepColor.ToColor();
            waterData.u_maxDeepColor = mapData.maxDeepColor.ToColor();
            waterData.u_minShallowColor = mapData.minShallowColor.ToColor();
            waterData.u_maxShallowColor = mapData.maxShallowColor.ToColor();



            //Save Data
            string path = $"{saveDirPath}/mapData.asset";
            AssetDatabase.CreateAsset(data, path);
            AssetDatabase.Refresh();

            data = AssetDatabase.LoadAssetAtPath<MapData>(path);
            return data;
        }

        public IEnumerator ToUnity2()
        {
            gameObject = new GameObject(name);

            // Ground
            MdxMapGround mapGround = gameObject.AddComponent<MdxMapGround>();
            mapGround.mapData = mapData;

            mapGround.u_heightMap = groundHeightMap();
            //mapGround.tilesetTexturesArray = tilesetTextures();

            // Cliffs
            MdxMapCliffs mapCliffs = gameObject.AddComponent<MdxMapCliffs>();
            mapCliffs.mapData = mapData;
            mapCliffs.u_heightMap = cliffsHeightMap();
            mapCliffs.cliffs = mapData.cliffs.ToArray();
            mapCliffs.cliffTextureArray = cliffsTextures();
            mapCliffs.meshs = new Mesh[mapCliffs.cliffs.Length];
            for (int i = 0; i < mapCliffs.cliffs.Length; i ++)
            {

                string cliffsPrefabPath = MdxRuntimeUtils.GetPrefabPath(mapCliffs.cliffs[i].path);
                GameObject cliffsGO = AssetDatabase.LoadAssetAtPath<GameObject>(cliffsPrefabPath);
                if (cliffsGO != null)
                {
                    SkinnedMeshRenderer skinnedMeshRenderer = cliffsGO.GetComponentInChildren<SkinnedMeshRenderer>();
                    if (skinnedMeshRenderer == null || skinnedMeshRenderer.sharedMesh == null)
                    {
                        Debug.LogError("请检查 SkinnedMeshRenderer " + cliffsPrefabPath);
                    }
                    else
                    {
                        mapCliffs.meshs[i] = skinnedMeshRenderer.sharedMesh;
                    }
                }
                else
                {
                    Debug.LogError("没有找到 " + cliffsPrefabPath);
                }
            }

            // Water
            MdxMapWater mapWater = gameObject.AddComponent<MdxMapWater>();
            mapWater.mapData = mapData;
            mapWater.waterIncreasePerFrame = mapData.waterIncreasePerFrame;
            mapWater.waterTexturCount = mapData.waterTextures.Count;
            mapWater.u_heightMap = groundHeightMap();
            mapWater.u_waterHeightMap = waterHeightMap();
            mapWater.tilesetTexturesArray = waterTextures();

            mapWater.u_minDeepColor = mapData.minDeepColor.ToColor();
            mapWater.u_maxDeepColor = mapData.maxDeepColor.ToColor();
            mapWater.u_minShallowColor = mapData.minShallowColor.ToColor();
            mapWater.u_maxShallowColor = mapData.maxShallowColor.ToColor();


            // Doodads
            MdxMapDoodads mapDoodads = gameObject.AddComponent<MdxMapDoodads>();
            mapDoodads.mapData = mapData;
            mapDoodads.doodads = mapData.doodads;
            yield return LoadModesDoodads(mapDoodads);

            // TerrainDoodads
            MdxMapTerrainDoodad terrainDoodads = gameObject.AddComponent<MdxMapTerrainDoodad>();
            terrainDoodads.mapData = mapData;
            terrainDoodads.terrainDoodads = mapData.terrainDoodads;
            yield return LoadModesDoodads(terrainDoodads);

            // Units
            MdxMapUnits mapUnits = gameObject.AddComponent<MdxMapUnits>();
            mapUnits.mapData = mapData;
            mapUnits.units = mapData.units;
            yield return LoadModesDoodads(mapUnits);


            mapGround.enabled = false;
            mapCliffs.enabled = false;
            mapWater.enabled = false;



            // Save
            if (!Directory.Exists(Path.GetDirectoryName(savePrefabPath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(savePrefabPath));
            }
            PrefabUtility.SaveAsPrefabAsset(gameObject, savePrefabPath);
            yield return null;
        }
        private IEnumerator LoadModesDoodads(MdxMapUnits mapDoodads)
        {
            var doodads = mapDoodads.units;
            var models = mapDoodads.models;
            var modeUseMap = mapDoodads.modeUseMap;
            for (int i = 0; i < doodads.Count; i++)
            {
                var doodad = doodads[i];
                if (!modeUseMap.ContainsKey(doodad.modePath))
                {
                    modeUseMap.Add(doodad.modePath, 1);
                }
                else
                {
                    modeUseMap[doodad.modePath] += 1;
                }

                if (!models.ContainsKey(doodad.modePath))
                {
                    GameObject prefab = MdxRuntimeUtils.LoadPrefabByMdxPath(doodad.modePath);
                    if (prefab != null)
                    {
                        models.Add(doodad.modePath, prefab);
                    }
                    else
                    {
                        Debug.Log($"MdxModelImport: {doodad.modePath}");
                        string savePath = MdxUnityResPathDefine.GetPath(doodad.modePath);
                        yield return MdxModelImport.I.OnImportAsset(savePath);
                        prefab = MdxRuntimeUtils.LoadPrefabByMdxPath(doodad.modePath);
                        models.Add(doodad.modePath, prefab);
                    }
                }
            }

            mapDoodads.Print();
        }

        private IEnumerator LoadModesDoodads(MdxMapTerrainDoodad mapDoodads)
        {
            var doodads = mapDoodads.terrainDoodads;
            var models = mapDoodads.models;
            var modeUseMap = mapDoodads.modeUseMap;
            for (int i = 0; i < doodads.Count; i++)
            {
                var doodad = doodads[i];
                if (!modeUseMap.ContainsKey(doodad.modePath))
                {
                    modeUseMap.Add(doodad.modePath, 1);
                }
                else
                {
                    modeUseMap[doodad.modePath] += 1;
                }

                if (!models.ContainsKey(doodad.modePath))
                {
                    GameObject prefab = MdxRuntimeUtils.LoadPrefabByMdxPath(doodad.modePath);
                    if (prefab != null)
                    {
                        models.Add(doodad.modePath, prefab);
                    }
                    else
                    {
                        Debug.Log($"MdxModelImport: {doodad.modePath}");
                        string savePath = MdxUnityResPathDefine.GetPath(doodad.modePath);
                        yield return MdxModelImport.I.OnImportAsset(savePath);
                        prefab = MdxRuntimeUtils.LoadPrefabByMdxPath(doodad.modePath);
                        models.Add(doodad.modePath, prefab);
                    }
                }
            }

            mapDoodads.Print();
        }

        private IEnumerator LoadModesDoodads(MdxMapDoodads mapDoodads) 
        {
            var doodads = mapDoodads.doodads;
            var models = mapDoodads.models;
            var modeUseMap = mapDoodads.modeUseMap;
            for (int i = 0; i < doodads.Count; i++)
            {
                var doodad = doodads[i];
                if (!modeUseMap.ContainsKey(doodad.modePath)) {
                    modeUseMap.Add(doodad.modePath, 1);
                }
                else
                {
                    modeUseMap[doodad.modePath] += 1;
                }

                if (!models.ContainsKey(doodad.modePath))
                {
                    GameObject prefab = MdxRuntimeUtils.LoadPrefabByMdxPath(doodad.modePath);
                    if (prefab != null)
                    {
                        models.Add(doodad.modePath, prefab);
                    }
                    else {
                        Debug.Log($"MdxModelImport: {doodad.modePath}");
                        string savePath = MdxUnityResPathDefine.GetPath(doodad.modePath);
                        yield return MdxModelImport.I.OnImportAsset(savePath);
                        prefab = MdxRuntimeUtils.LoadPrefabByMdxPath(doodad.modePath);
                        models.Add(doodad.modePath, prefab);
                    }
                }
            }

            mapDoodads.Print();
        }

        private Texture2DArray waterTextures()
        {

            // Create texture2DArray
            var sourceTextures = mapData.waterTextures;
            Texture2DArray texture2DArray = new Texture2DArray(sourceTextures[0].width, sourceTextures[0].height, sourceTextures.Count, ((Texture2D)sourceTextures[0]).format, true);

            Debug.Log($"waterTextures sourceTextures.Count={sourceTextures.Count}, sourceTextures[0].mipmapCount={sourceTextures[0].mipmapCount}");
            // Apply settings
            texture2DArray.filterMode = FilterMode.Bilinear;
            texture2DArray.wrapMode = TextureWrapMode.Repeat;

            for (int i = 0; i < sourceTextures.Count; i++)
            {
                Debug.Log($"waterTextures sourceTextures[{i}].mipmapCount={sourceTextures[i].mipmapCount}");
                for (int m = 0; m < sourceTextures[i].mipmapCount; m++)
                {
                    Debug.Log($"waterTextures srcElement={0}, srcMip={m},   dstElement={i}, dstMip={m}");
                    Graphics.CopyTexture(sourceTextures[i], 0, m, texture2DArray, i, m);
                }
            }
            // Apply our changes
            texture2DArray.Apply(false);
            //Save 
            string path = $"{saveDirPath}/waterTextures.asset";
            AssetDatabase.CreateAsset(texture2DArray, path);
            AssetDatabase.Refresh();

            texture2DArray = AssetDatabase.LoadAssetAtPath<Texture2DArray>(path);
            return texture2DArray;
        }


        private Texture2DArray cliffsTextures()
        {

            // Create texture2DArray
            var sourceTextures = mapData.cliffTextures;
            Texture2DArray texture2DArray = new Texture2DArray(sourceTextures[0].width, sourceTextures[0].height, sourceTextures.Count, ((Texture2D)sourceTextures[0]).format, true);

            Debug.Log($"cliffsTextures sourceTextures.Count={sourceTextures.Count}, sourceTextures[0].mipmapCount={sourceTextures[0].mipmapCount}");
            // Apply settings
            texture2DArray.filterMode = FilterMode.Bilinear;
            texture2DArray.wrapMode = TextureWrapMode.Repeat;

            for (int i = 0; i < sourceTextures.Count; i++)
            {
                Debug.Log($"cliffsTextures sourceTextures[{i}].mipmapCount={sourceTextures[i].mipmapCount}");
                for (int m = 0; m < sourceTextures[i].mipmapCount; m++)
                {
                    Debug.Log($"cliffsTextures srcElement={0}, srcMip={m},   dstElement={i}, dstMip={m}");
                    Graphics.CopyTexture(sourceTextures[i], 0, m, texture2DArray, i, m);
                }
            }
            // Apply our changes
            texture2DArray.Apply(false);
            //Save 
            string path = $"{saveDirPath}/cliffsTextures.asset";
            AssetDatabase.CreateAsset(texture2DArray, path);
            AssetDatabase.Refresh();

            texture2DArray = AssetDatabase.LoadAssetAtPath<Texture2DArray>(path);
            return texture2DArray;
        }


        private Texture2DArray tilesetTextures()
        {
            // Create texture2DArray
            Texture2DArray texture2DArray = mapData.tilesetTextures.CreateTexture2DArray();
            //Save 
            string path = $"{saveDirPath}/tilesetTextures.asset";
            AssetDatabase.CreateAsset(texture2DArray, path);
            AssetDatabase.Refresh();

            texture2DArray = AssetDatabase.LoadAssetAtPath<Texture2DArray>(path);
            return texture2DArray;
        }

        private Texture2D waterHeightMap()
        {
            Texture2D tex = new Texture2D(mapData.mapSize[0], mapData.mapSize[1], TextureFormat.RFloat, false);
            tex.filterMode = FilterMode.Bilinear;
            tex.wrapMode = TextureWrapMode.Clamp;

            float[] heights = mapData.waterHeights;
            float[] bytes = new float[heights.Length];

            float min = 255;
            float max = -255;
            for (int i = 0; i < bytes.Length; i++)
            {
                float v = heights[i];
                min = Mathf.Min(min, v);
                max = Mathf.Max(max, v);
            }

            Debug.Log($"waterHeightMap min={min}, max={max}");

            min = -1;
            max = 7;
            for (int i = 0; i < bytes.Length; i++)
            {
                float v = heights[i];
                v = (v - min) / (max - min);

                bytes[i] = v;
            }



            tex.SetPixelData<float>(heights, 0);
            tex.Apply();

            string path = $"{saveDirPath}/water_heightMap.asset";
            AssetDatabase.CreateAsset(tex, path);
            AssetDatabase.Refresh();
            tex = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
            return tex;
        }

        private Texture2D groundHeightMap()
        {
            Texture2D tex = new Texture2D(mapData.mapSize[0], mapData.mapSize[1], TextureFormat.RFloat, false);
            tex.filterMode = FilterMode.Bilinear;
            tex.wrapMode = TextureWrapMode.Clamp;

            float[] heights = mapData.cornerHeights;
            float[] bytes = new float[heights.Length];

            float min = float.MaxValue;
            float max = float.MinValue;
            for (int i = 0; i < bytes.Length; i++)
            {
                float v = heights[i];
                min = Mathf.Min(min, v);
                max = Mathf.Max(max, v);
            }

            Debug.Log($"groundHeightMap min={min}, max={max}");

            //min = -1;
            //max = 7;
            for (int i = 0; i < bytes.Length; i++)
            {
                float v = heights[i];
                v = (v - min) / (max - min) ;

                bytes[i] = v;
            }



            tex.SetPixelData<float>(heights, 0);
            tex.Apply();

            string path = $"{saveDirPath}/ground_heightMap.asset";
            AssetDatabase.CreateAsset(tex, path);
            AssetDatabase.Refresh();
            tex = AssetDatabase.LoadAssetAtPath<Texture2D>(path);

            return tex;
        }

        private Texture2D cliffsHeightMap()
        {
            Texture2D tex = new Texture2D(mapData.mapSize[0], mapData.mapSize[1], TextureFormat.RFloat, false);
            tex.filterMode = FilterMode.Bilinear;
            tex.wrapMode = TextureWrapMode.Clamp;

            float[] heights = mapData.cliffHeights;

            float[] bytes = new float[heights.Length];

            float min = 255;
            float max = -255;
            for (int i = 0; i < bytes.Length; i++)
            {
                float v = heights[i];
                min = Mathf.Min(min, v);
                max = Mathf.Max(max, v);
            }


            Debug.Log($"cliffsHeightMap min={min}, max={max}");

            min = -1;
            max = 7;
            for (int i = 0; i < bytes.Length; i++)
            {
                float v = heights[i];
                v = (v - min) / (max - min);

                bytes[i] = v;
            }

            tex.SetPixelData<float>(heights, 0);
            tex.Apply();

            string path = $"{saveDirPath}/cliffs_heightMap.asset";
            AssetDatabase.CreateAsset(tex, path);
            AssetDatabase.Refresh();
            tex = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
            return tex;
        }

        private void SaveTextureToFile(Texture2D texture , string fileName)
        {
            if (!Directory.Exists(Path.GetDirectoryName(fileName)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fileName));
            }

            var bytes = texture.EncodeToPNG();
            var file = File.Open(fileName, FileMode.Create);
            var binary = new BinaryWriter(file);
            binary.Write(bytes);
            file.Close();

            AssetDatabase.Refresh();
        }
    }
}
