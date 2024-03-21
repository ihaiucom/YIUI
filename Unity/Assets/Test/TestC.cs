using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using Zeng.mdx.commons;
using Zeng.mdx.imports.w3x;
using Zeng.mdx.parsers.mdx;

public class TestC : MonoBehaviour
{
    public string filepath = "Assets/(2)BootyBay.w3m";
    // Start is called before the first frame update
    void Start()
    {
        //ImportW3X();
        StartCoroutine(DownloadMdxTextures());

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ImportW3X()
    {
        Debug.Log(filepath);
        StartCoroutine(W3xImport.I.OnImportAsset(filepath));
    }

    public int downloadMdxStartIndex = 0;
    public int index = 0;
    public IEnumerator  DownloadMdxTextures()
    {
        string root = MdxUnityResPathDefine.Root;

        List<string> fileList = SearchFiles(root, "mdx");
        Debug.Log(fileList.Count);

        for(int  i = downloadMdxStartIndex, l = fileList.Count; i < l; i ++)
        {
            index = i;
            string path = fileList[i];
            Debug.Log(i + " " + path);
            yield return downloadMdxTexture(path, i);
        }
    }

    Dictionary<int, string> replaceableIds = new Dictionary<int, string> {

        {1, "TeamColor/TeamColor00" },
        {2, "TeamGlow/TeamGlow00"},
        {11, "Cliff/Cliff0"},
        {21, ""}, // Used by all cursor models (HumanCursor, OrcCursor, UndeadCursor, NightElfCursor)
        {31, "LordaeronTree/LordaeronSummerTree"},
        {32, "AshenvaleTree/AshenTree"},
        {33, "BarrensTree/BarrensTree"},
        {34, "NorthrendTree/NorthTree"},
        {35, "Mushroom/MushroomTree"},
        {36, "RuinsTree/RuinsTree"},
        {37, "OutlandMushroomTree/MushroomTree"},

    };

    IEnumerator downloadMdxTexture(string path, int i)
    {
        Model model = new Model();
        try {
            model.Load(path);
        }
        catch (Exception e) {
            Debug.LogError(i + " " + path + ", " + e);
        }
        foreach (var texture in model.textures) {
            if (!string.IsNullOrEmpty(texture.path)) { 
                yield return loadTexture(texture.path);
            }
            else if(texture.replaceableId != 0 && replaceableIds.ContainsKey(texture.replaceableId) && !string.IsNullOrEmpty(replaceableIds[texture.replaceableId]))
            {
                string tpath = $"ReplaceableTextures/{ replaceableIds[texture.replaceableId]}.blp";
                yield return loadTexture(tpath);
            }
        }
    }


    private IEnumerator loadTexture(string path)
    {

        if (path.IndexOf(",") != -1)
        {
            string[] arr = path.Split(",");
            foreach (var itemPath in arr)
            {
                yield return loadTexture(itemPath);
            }
            yield break;
        }

        string url = UrlUtils.localOrHive(path);
        string savePath = MdxUnityResPathDefine.GetPath(path);

        UnityEngine.Debug.Log(savePath);
        if (System.IO.File.Exists(savePath))
        {
            yield break;
        }

        IEnumerator it = MdxUnityTextureManager.I.Load(url, savePath, path);
        yield return it;


        yield return null;

    }

    // Function to recursively search for files with a specific extension in a given directory and its subdirectories
    public List<string> SearchFiles(string directory, string extension)
    {
        List<string> filePaths = new List<string>();

        // Get all files in the current directory with the specified extension
        string[] files = Directory.GetFiles(directory, $"*.{extension}");
        filePaths.AddRange(files);

        // Recursively search for files in subdirectories
        string[] subdirectories = Directory.GetDirectories(directory);
        foreach (string subdirectory in subdirectories)
        {
            filePaths.AddRange(SearchFiles(subdirectory, extension));
        }

        return filePaths;
    }

}
