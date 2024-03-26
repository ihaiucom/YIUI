/*------------------------------------------------------------------------------
|
| COPYRIGHT (C) 2018 - 2026 All Right Reserved
|
| FILE NAME  : \Assets\CLineActionEditor\ActionEngine\Res\ResourceMgr.cs
| AUTHOR     : https://supercline.com/
| PURPOSE    : 
|
| SPEC       : resource name format[/gamedata/xx.prefab], and case-insensitive
|
| MODIFICATION HISTORY
| 
| Ver	   Date			   By			   Details
| -----    -----------    -------------   ----------------------
| 1.0	   2018-9-10        SuperCLine           Created
|
+-----------------------------------------------------------------------------*/


namespace SuperCLine.ActionEngine
{
    using System.IO;
    using System.Collections.Generic;
    using UnityEngine;

    public sealed class ResourceMgr : Singleton<ResourceMgr>
    {
        private Dictionary<string, string> m_FileHash = new Dictionary<string, string>();
        //TODO: compatible with old version.
        private Dictionary<string, string> m_FileHash_v1 = new Dictionary<string, string>();

        public static string resRoot
        {
            get { return "Assets/CLineActionEditor/Demo/Resource"; }
        }

        private static string _gamedataPath;
        public static string gamedataPath
        {
            get
            {
                if (string.IsNullOrEmpty(_gamedataPath))
                {
                    var dataPath = Application.dataPath.Replace("Assets", resRoot);
                    _gamedataPath = string.Format("{0}/GameData/", dataPath);
                }

                return _gamedataPath;
            }
        }

        public override void Init()
        {
            BuildResourceMap();
        }

        public override void Destroy()
        {
            
        }

        public UnityEngine.Object LoadObject(string fileName, System.Type type)
        {
            string asset = string.Empty;
            if (FindResource(fileName, out asset))
            {
                UnityEngine.Object obj = UnityEditor.AssetDatabase.LoadAssetAtPath(asset, type);
                if (null == obj)
                {
                    LogMgr.Instance.Logf(ELogType.ELT_ERROR, "ResourceMgr", "Failed to load asset: {0}", fileName);
                }
                return obj;
            }
            else
            {
                LogMgr.Instance.Logf(ELogType.ELT_ERROR, "ResourceMgr", "The resource '{0}' is not exist! ", fileName);
                return null;
            }
        }

        public T LoadObject<T>(string fileName) where T : UnityEngine.Object
        {
            string asset = string.Empty;
            if (FindResource(fileName, out asset))
            {
                T obj = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(asset);
                if (null == obj)
                {
                    LogMgr.Instance.Logf(ELogType.ELT_ERROR, "ResourceMgr", "Failed to load asset: {0}", fileName);
                }
                return obj;
            }
            else
            {
                LogMgr.Instance.Logf(ELogType.ELT_ERROR, "ResourceMgr", "The resource '{0}' is not exist! ", fileName);
                return null;
            }
        }

        public string FormatResourceName(string name)
        {
            return name.Replace(resRoot, "");
        }

        private void BuildResourceMap()
        {
            string abRoot = Path.Combine(Directory.GetCurrentDirectory(), resRoot);
            if (!Directory.Exists(abRoot))
                LogMgr.Instance.Log(ELogType.ELT_ERROR, "ResourceMgr", "The directory of resource is not exist!");

            string[] guids = UnityEditor.AssetDatabase.FindAssets("t:Texture t:TextAsset t:GameObject t:Shader t:Font", new string[] { resRoot });
            for (int i=0; i<guids.Length; ++i)
            {
                string assetPath = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[i]);
                if (File.Exists(assetPath))
                {
                    string fileName = assetPath.Replace(resRoot, "");
                    m_FileHash[fileName] = assetPath;
                    m_FileHash_v1[fileName.ToLower()] = assetPath;
                }
            }
        }

        public bool FindResource(string name, out string asset)
        {
            if (m_FileHash_v1.TryGetValue(name, out asset))
                return true;

            if (m_FileHash.TryGetValue(name, out asset))
                return true;

            return false;
        }


        public string[] GetFiles(string path)
        {
            List<string> fileList = new List<string>();

            using (var itr = m_FileHash.GetEnumerator())
            {
                while (itr.MoveNext())
                {
                    if (itr.Current.Key.Contains(path))
                    {
                        fileList.Add(itr.Current.Key);
                    }
                }
            }

            return fileList.ToArray();
        }

    }

}
