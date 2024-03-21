using System.Collections;
using Zeng.mdx.handlers.w3x;

namespace Zeng.mdx.imports.w3x
{
    public class W3xImport
    {

        private static W3xImport _I;
        public static W3xImport I
        {
            get
            {
                if (_I == null)
                {
                    _I = new W3xImport();
                }
                return _I;
            }
        }

        public IEnumerator OnImportAsset(string assetPath)
        {
            //War3Map war3Map = new War3Map();
            //war3Map.ReadFile(assetPath);
            //yield return war3Map.SaveUnity(savePath);



            //War3MapViewerMap map = new War3MapViewerMap();
            //map.load(assetPath);
            //yield return null;

            War3MapViewer view = new War3MapViewer();
            yield return view.Init();
            UnityEngine.Debug.Log("~~~~~~~~~~下载资源完成");
            yield return view.loadMap(assetPath);
            UnityEngine.Debug.Log("~~~~~~~~~~loadMap完成");

            ImportMap importMap = new ImportMap(view, assetPath);
            yield return importMap.ToUnity();
            UnityEngine.Debug.Log("~~~~~~~~~~ToUnity完成");



        }
    }
}