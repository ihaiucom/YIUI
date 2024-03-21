using System.Collections;
using System.Diagnostics;
using Unity.EditorCoroutines.Editor;
using Zeng.mdx.commons;
using Zeng.mdx.parsers;
using Zeng.mdx.parsers.mdx;

namespace Zeng.mdx.imports.mdx
{
    public class MdxModelImport
    {

        private static MdxModelImport _I;
        public static MdxModelImport I
        {
            get
            {
                if (_I == null)
                {
                    _I = new MdxModelImport();
                }
                return _I;
            }
        }

        public MappedData SpawnData = new MappedData();
        public MdxModelImport() {
            loadBaseFile("Splats\\SpawnData.slk", SpawnData);
        }

        public IEnumerator OnImportAssets(string[] files)
        {
            for (int i = 0; i < files.Length; i++)
            {
                UnityEngine.Debug.Log(i + " " + files[i]);
                yield return OnImportAsset(files[i]);
            }
        }

        public IEnumerator OnImportAsset(string assetPath)
        {
            Model model = new Model();
            model.Load(assetPath);

            ImportModel imodel = new ImportModel(model, assetPath);
            yield return imodel.ToUnity();
        }


        private void loadBaseFile(string path, MappedData mappedData)
        {
            string savePath = MdxUnityResPathDefine.GetPath(path);
            if (System.IO.File.Exists(savePath))
            {
                //UnityEngine.Debug.Log(savePath);
                string buffer = System.IO.File.ReadAllText(savePath);
                mappedData.Load(buffer);
            }
        }


    }
}