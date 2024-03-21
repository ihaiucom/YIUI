using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class MapIcon
{
    public Image image;
    public bool isShow = false;
    public int spIndex = 0;
    public Vector3 pos;
    public Vector2 key;
    [SerializeField]
    public List<MapNode> parentNodes = new List<MapNode>();
}

[Serializable]
public class MapNode
{
    public Vector2 key;
    public Vector2 pos;
    public bool isShow = false;
    [SerializeField]
    public List<Vector2> iconKeys = new List<Vector2>();
    public void Init(int x,int y)
    {
        iconKeys.Add(new Vector2(x, y));
        iconKeys.Add(new Vector2(x + 1, y));
        iconKeys.Add(new Vector2(x, y + 1));
        iconKeys.Add(new Vector2(x + 1, y + 1));
    }
}

public class TileMap : MonoBehaviour
{
    public Sprite[] sps;
    [SerializeField]
    public int imageSize = 32;//64*64组成一个小块地图\
    public Canvas canvas;

    [SerializeField]
    public Dictionary<Vector2, MapNode> mapNodeDic;
    [SerializeField]
    public Dictionary<Vector2, MapIcon> mapIconDic;
    [SerializeField]
    public List<Vector2> listIconKeys;
    [SerializeField]
    public List<Vector2> listNodeKeys;
    bool isNeedRefresh = false;

    [SerializeField]
    public Vector2 temp = Vector2.zero;
    private void Start()
    {
        OnInitMap();
    }

    void OnInitMap()
    {
        mapNodeDic = new Dictionary<Vector2, MapNode>();
        mapIconDic = new Dictionary<Vector2, MapIcon>();

        int ScreenWidth = (int)Screen.width;
        int ScreenHeight = (int)Screen.height;
        int widNum = ScreenWidth / imageSize;
        int heightNum = ScreenHeight / imageSize;
        //640 / 2 - imageSize/2
        int xSize = -ScreenWidth / 2 - imageSize / 2;
        int ySize = -ScreenHeight / 2 - imageSize / 2;
        Vector2 originPos1 = new Vector2(xSize + imageSize / 2, ySize + imageSize / 2);
        Vector2 originPos2 = new Vector3(xSize + imageSize, ySize + imageSize);
        
        for (int i = 0; i < widNum; i++)
        {
            for(int j = 0; j < heightNum; j++)
            {
                MapIcon mapIcon = new MapIcon();
                mapIcon.key = new Vector2(i, j);
                temp.x = originPos1.x + i * imageSize;
                temp.y = originPos1.y + j * imageSize;
                mapIcon.pos = temp;
                mapIconDic[mapIcon.key] = mapIcon;
            }
        }
        listIconKeys = new List<Vector2>(mapIconDic.Keys);

        for (int i = 0; i < widNum - 1; i++)
        {
            for (int j = 0; j < heightNum - 1; j++)
            {
                MapNode mapNode = new MapNode();
                mapNode.key = new Vector2(i, j);
                temp.x = originPos2.x + i * imageSize;
                temp.y = originPos2.y + j * imageSize;
                mapNode.pos = temp;
                mapNode.Init(i, j);
                mapNodeDic[mapNode.key] = mapNode;
                for(int z=0;z< mapNode.iconKeys.Count; z++)
                {
                    mapIconDic[mapNode.iconKeys[z]].parentNodes.Add(mapNode);
                }
            }
        }
        listNodeKeys = new List<Vector2>(mapNodeDic.Keys);
    }
    /// <summary>
    /// 监听点击
    /// </summary>
    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, canvas.worldCamera, out temp);
            float dis = 100000;
            MapNode mapNode = null;
            for (int i = 0; i < listNodeKeys.Count; i++)
            {
                float _dis = Vector2.Distance(mapNodeDic[listNodeKeys[i]].pos, temp);
                if (_dis < dis)
                {
                    dis = _dis;
                    mapNode = mapNodeDic[listNodeKeys[i]];
                }
            }
            if (mapNode!=null)
            {
                mapNode.isShow = true;
                RefreshMapData(mapNode);
            }
        }
    }
    /// <summary>
    /// 刷新地图数据
    /// </summary>
    /// <param name="mapNode"></param>
    void RefreshMapData(MapNode mapNode)
    {
        if (mapNode != null)
        {
            for(int i=0;i< mapNode.iconKeys.Count; i++)
            {
                Vector2 _key = mapNode.iconKeys[i];
                mapIconDic[mapNode.iconKeys[i]].isShow = true;
                int tempSp = 0;
                Vector2 pos = mapIconDic[mapNode.iconKeys[i]].pos;
                MapNode tempMapNode;
                for (int j = 0;j< mapIconDic[mapNode.iconKeys[i]].parentNodes.Count; j++)
                {
                    tempMapNode = mapIconDic[mapNode.iconKeys[i]].parentNodes[j];
                    if (!tempMapNode.isShow)
                        continue;
                    temp = tempMapNode.pos;
                    if (temp.x < pos.x && temp.y > pos.y)
                        tempSp += 2;
                    if (temp.x > pos.x && temp.y > pos.y)
                        tempSp += 1;
                    if (temp.x < pos.x && temp.y < pos.y)
                        tempSp += 8;
                    if (temp.x > pos.x && temp.y < pos.y)
                        tempSp += 4;
                }
                mapIconDic[mapNode.iconKeys[i]].spIndex = tempSp;
            }
            isNeedRefresh = true;
        }
    }

    /// <summary>
    /// 负责地图刷新
    /// </summary>
    private void LateUpdate()
    {
        if (isNeedRefresh)
        {
            for (int i = 0; i < listIconKeys.Count; i++)
            {
                if (mapIconDic[listIconKeys[i]].isShow)
                {
                    Image img = mapIconDic[listIconKeys[i]].image;
                    if (img == null)
                    {
                        mapIconDic[listIconKeys[i]].image = GetImageFromPool(mapIconDic[listIconKeys[i]].spIndex);
                        img = mapIconDic[listIconKeys[i]].image;
                        img.transform.localPosition = mapIconDic[listIconKeys[i]].pos;
                    }
                    mapIconDic[listIconKeys[i]].image.sprite = sps[mapIconDic[listIconKeys[i]].spIndex];
                }
            }
            isNeedRefresh = false;
        }
    }


    /// <summary>
    /// 获取image组件
    /// </summary>
    /// <param name="sp"></param>
    /// <returns></returns>
    private Image GetImageFromPool(int sp)
    {
        GameObject obj = new GameObject("image");
        obj.transform.parent = transform;
        Image image = obj.AddComponent<Image>();
        RectTransform rect = image.transform as RectTransform;
        rect.sizeDelta = new Vector2(imageSize, imageSize);
        //image.sprite = sps[sp];
        return image;
    }
}
