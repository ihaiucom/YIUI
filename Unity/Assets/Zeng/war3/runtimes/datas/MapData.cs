
using System;
using UnityEngine;

namespace Zeng.war3.runtimes.datas
{
    /// <summary>
    /// 地形数据
    /// </summary>
    public class MapData : ScriptableObject
    {
        // 地图大小
        public Vector2Int mapSize; // int columns = this.mapSize.x; int rows = this.mapSize.y;
        // 地图中心点偏移
        public Vector2 centerOffset;
        // 瓦片 实例数量
        public int instanceCount; // instanceCount = （columns - 1） * （rows -1）

        // 地形--地面数据
        [SerializeField]
        public GroundData groundData;

        // 地形--悬崖数据
        [SerializeField]
        public CliffsData cliffsData;

        // 地形--水
        [SerializeField]
        public WaterData waterData;

    }

    /// <summary>
    /// 地形--地面数据
    /// </summary>
    [Serializable]
    public class GroundData
    {

        // 地面 对应瓦片 高度数据
        public float[] cornerHeights; // cornerHeights = new float[columns * rows];
        // 地面 对应瓦片 高度贴图
        public Texture cornerHeightTexture;
        // 地面 对应瓦片 贴图索引
        public float[] cornerTextures; // new float[instanceCount * 4]; 
        // 地面 对应瓦片 贴图对应uv索引
        public float[] cornerVariations; // new float[instanceCount * 4]; 
        // 地面 贴图列表
        public Texture[] tilesetTextures;
        // 地面 贴图列表 创建的数组贴图
        public Texture2DArray tilesetTexturesArray;
    }

    /// <summary>
    /// 地形--悬崖数据
    /// </summary>
    [Serializable]
    public class CliffsData
    {

        // 高度数据
        public float[] cliffHeights; // cornerHeights = new float[columns * rows];
        // 高度贴图
        public Texture cliffHeightTexture;
        // 贴图列表
        public Texture[] cliffTextures;
        // 贴图列表 创建的数组贴图
        public Texture2DArray cliffTexturesArray;
        // 实例预设列表
        public CliffsItemData[] cliffs;
        // CliffsItemData.path对应的Mesh文件
        public Mesh[] meshs;
        // CliffsItemData.path对应的实例数量
        public int[] instanceCounts;
    }


    /// <summary>
    /// 地形--悬崖实例预设数据
    /// </summary>
    [Serializable]
    public class CliffsItemData
    {
        public string path;
        // 贴图索引
        public int[] texturesIndex;
        // 位置(x,y,z)
        public float[] locations;
    }

    /// <summary>
    /// 地形--水数据
    /// </summary>
    [Serializable]
    public class WaterData 
    {

        // 是否是水面
        public float[] waterFlags;

        public float waterIncreasePerFrame = 0.25f;
        // 贴图列表
        public Texture[] waterTextures;
        // 贴图列表 创建的数组贴图
        public Texture2DArray waterTexturesArray;

        // 高度偏移
        public float waterHeightOffset = 0;
        // 高度数据
        public float[] waterHeights; // cornerHeights = new float[columns * rows];
        // 高度贴图
        public Texture waterHeightTexture;


        public float u_minDepth = 10f / 128f; //  0.078125 // 10 / 128
        public float u_deepLevel = 64f / 128f; // 0.5    // 64 / 128
        public float u_maxDepth = 72f / 128f; // 0.5625   // 72 / 128


        public Color u_minDeepColor;
        public Color u_maxDeepColor;
        public Color u_minShallowColor;
        public Color u_maxShallowColor;

    }
}
