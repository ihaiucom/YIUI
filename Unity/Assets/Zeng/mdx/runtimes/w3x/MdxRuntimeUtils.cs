
using UnityEngine;

namespace Zeng.mdx.runtimes
{
    public static class MdxRuntimeUtils
    {
        public static string GetPrefabPath(string mdxPath )
        {
            string prefabPath = mdxPath.Replace("\\", "/").Replace(".mdx", ".prefab");
            prefabPath = $"Assets/MdxPrefabs/{prefabPath}";
            return prefabPath;

        }

        public static GameObject LoadPrefabByMdxPath(string mdxPath)
        {
            string prefabPath = GetPrefabPath(mdxPath);
            return LoadPrefab(prefabPath);
        }

        public static GameObject LoadPrefab(string prefabPath)
        {
            GameObject go = null;
#if UNITY_EDITOR
            go =  UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
#endif
            if (go == null)
            {
                Debug.Log($"MdxRuntimeUtils LoadPrefab 预设文件不存在 {prefabPath}");
            }
            return go;
        }


        public static Vector3 ToU3d(this float[] v)
        {
            return new Vector3(-v[1], v[2], v[0]);
        }

        public static Vector3 VecToU3d(this Vector3 v)
        {
            return new Vector3(-v.y, v.z, v.x);
        }

        public static Vector2 UvToU3d(this Vector2 v)
        {
            return new Vector2(v.x, Mathf.Abs(v.y - 1));
        }


        public static Quaternion QuaternionToU3d(this Quaternion v)
        {
            return new Quaternion(-v.y, v.z, v.x, -v.w);
        }
    }
}
