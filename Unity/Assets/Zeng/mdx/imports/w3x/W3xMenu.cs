using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using Unity.EditorCoroutines.Editor;
using Zeng.mdx.imports.mdx;

namespace Zeng.mdx.imports.w3x {

    public class W3xMenu
    {

        [MenuItem("Assets/w3x Import")]
        static void W3xImport()
        {
            string filepath = AssetDatabase.GetAssetPath(Selection.activeObject);
            Debug.Log(filepath);
            EditorCoroutineUtility.StartCoroutine(w3x.W3xImport.I.OnImportAsset(filepath), w3x.W3xImport.I);

        }

        [MenuItem("Assets/w3x Import", true)]
        static bool ValidateW3xImport()
        {
            if (Selection.activeObject == null) return false;
            string filepath = AssetDatabase.GetAssetPath(Selection.activeObject);
            string ext = Path.GetExtension(filepath).Trim().ToLower();
            return ext == ".w3x" || ext == ".w3m";
        }


        [MenuItem("Assets/mdx Import")]
        static void MdxImport()
        {
            string filepath = AssetDatabase.GetAssetPath(Selection.activeObject);
            Debug.Log(filepath);
            EditorCoroutineUtility.StartCoroutine(MdxModelImport.I.OnImportAsset(filepath), MdxModelImport.I);

        }

        [MenuItem("Assets/mdx Import", true)]
        static bool ValidateMdxImport()
        {
            if (Selection.activeObject == null) return false;
            string filepath = AssetDatabase.GetAssetPath(Selection.activeObject);
            string ext = Path.GetExtension(filepath).Trim().ToLower();
            return ext == ".mdx";
        }

        [MenuItem("Assets/mdx Import Dir")]
        static void MdxImportDir()
        {
            string filepath = AssetDatabase.GetAssetPath(Selection.activeObject);
            string dir = Path.GetDirectoryName(filepath);
            string[] files = Directory.GetFiles(dir, "*.mdx", SearchOption.AllDirectories);
            Debug.Log("files.Length =" + files.Length);
            EditorCoroutineUtility.StartCoroutine(MdxModelImport.I.OnImportAssets(files), MdxModelImport.I);
        }
    }
}
