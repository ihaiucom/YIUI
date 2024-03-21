using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using Unity.EditorCoroutines.Editor;
using Zeng.MdxImport.mdx.handlers;

namespace Zeng.MdxImport
{
    public class EditorMdxImportMenu
    {

        [MenuItem("Assets/MdxImport MulMesh")]
        static void MdxImportMulMeshMenu()
        {
            string filepath = AssetDatabase.GetAssetPath(Selection.activeObject);



            EditorCoroutineUtility.StartCoroutine(ModelImport.I.OnImportAsset(filepath), ModelImport.I);
        }

        [MenuItem("Assets/MdxImport MulMesh", true)]
        static bool ValidateMdxImportMulMeshMenu()
        {
            if (Selection.activeObject == null) return false;
            string filepath = AssetDatabase.GetAssetPath(Selection.activeObject);
            string ext = Path.GetExtension(filepath).Trim().ToLower();
            return ext == ".mdx";
        }




        [MenuItem("Assets/MdxImport")]
        static void MdxImportMenu()
        {
            string filepath = AssetDatabase.GetAssetPath(Selection.activeObject);
            EditorCoroutineUtility.StartCoroutine(EditorMdxImport.I.OnImportAsset(filepath), EditorMdxImport.I);
        }

        [MenuItem("Assets/MdxImport", true)]
        static bool ValidateMdxImportMenu()
        {
            if (Selection.activeObject == null) return false;
            string filepath = AssetDatabase.GetAssetPath(Selection.activeObject);
            string ext = Path.GetExtension(filepath).Trim().ToLower();
            return ext == ".mdx";
        }


        [MenuItem("Assets/MdxImportTeamColorTextures")]
        static void MdxImportTeamColorTextures()
        {
            EditorCoroutineUtility.StartCoroutine(MdxTextureManager.I.LoadTeamTextures(), MdxTextureManager.I);
        }
    }

}