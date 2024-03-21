
using System.Collections.Generic;
using UnityEngine;
using Zeng.mdx.handlers.w3x;

namespace Zeng.mdx.runtimes
{
    [ExecuteInEditMode]
    public class MdxMapTerrainDoodad : MonoBehaviour
    {
        [SerializeField]
        public War3MapViewerMap mapData;

        // 装饰物
        [SerializeField]
        public List<TerrainDoodad> terrainDoodads = new List<TerrainDoodad>();
        [SerializeField]
        public List<GameObject> instances = new List<GameObject>();
        [SerializeField]
        public Dictionary<string, GameObject> models = new Dictionary<string, GameObject>();
        [SerializeField]
        public Dictionary<string, int> modeUseMap = new Dictionary<string, int>();


        public float u_tileSize_default = 128.0f;
        public float u_tileSize = 1.0f;

        public Transform root;

        void Start()
        {
            Gen();
        }

        private void OnEnable()
        {
            Gen();
        }

        public void Gen()
        {
            if (mapData == null) return;

            //if(root != null && instances.Count == doodads.Count && instances[0] != null)
            //{
            //    return;
            //}

            if (root == null) {
                root = new GameObject("TerrainDoodads").transform;
                root.SetParent(transform, false);
            }

            for (int i = 0; i < instances.Count; i++)
            {
                if (!Application.isEditor) {
                    GameObject.Destroy(instances[i]);
                }
                else
                {
                    GameObject.DestroyImmediate(instances[i]);
                }
            }
            instances.Clear();

            LoadModes();

            float tileScale = u_tileSize / u_tileSize_default;
            for (int i = 0; i < terrainDoodads.Count; i++)
            {
                TerrainDoodad doodad = terrainDoodads[i];
                if (!models.ContainsKey(doodad.modePath))
                {
                    continue;
                }

                GameObject prefab = models[doodad.modePath];
                if (prefab == null) 
                {
                    continue;
                }

                GameObject go = GameObject.Instantiate(prefab);
                Vector3 pos = new Vector3(doodad.doodad.location[0], doodad.doodad.location[1], 0);
                pos = pos * 128 + new Vector3(mapData.centerOffset[0], mapData.centerOffset[1]) +new Vector3(1, 1, 0) * 128;
                pos = pos.VecToU3d();


                go.transform.position = pos * tileScale;
                go.transform.localEulerAngles = new Vector3(0, doodad.row.GetNumber("fixedRot") * Mathf.PI / 180, 0);
                go.transform.localScale = Vector3.one * tileScale;
                go.transform.SetParent(root, false);
                //Animator animator = go.GetComponent<Animator>();
                //if (animator != null) {
                //}
                instances.Add(go);

            }
        }

        [ContextMenu("LoadModes")]
        public void LoadModes()
        {
            for (int i = 0; i < terrainDoodads.Count; i++)
            {
                var doodad = terrainDoodads[i];
                if (!models.ContainsKey(doodad.modePath))
                {
                    GameObject prefab = MdxRuntimeUtils.LoadPrefabByMdxPath(doodad.modePath);
                    if (prefab != null)
                    {
                        models.Add(doodad.modePath, prefab);
                    }
                }

            }
        }

        public void Print()
        {
            foreach (var kvp in modeUseMap) { 
                Debug.Log(kvp.Key + " ： " + kvp.Value);
            }

            for (int i = 0; i < terrainDoodads.Count; i++)
            {
                var doodad = terrainDoodads[i];
                if (!models.ContainsKey(doodad.modePath))
                {
                    Debug.Log("没有预设:" + doodad.modePath);
                }

            }
        }
    }
}
