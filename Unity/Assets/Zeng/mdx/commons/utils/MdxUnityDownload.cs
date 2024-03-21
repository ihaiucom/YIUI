
using System.Collections;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace Zeng.mdx.commons
{
    public class MdxUnityDownload
    {
        private static MdxUnityDownload _I;
        public static MdxUnityDownload I
        {
            get
            {
                if (_I == null)
                {
                    _I = new MdxUnityDownload();
                }
                return _I;
            }
        }

        public IEnumerator Load(string url, string savePath)
        {
            UnityWebRequest request = UnityWebRequest.Get(url);
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Download Error:" + request.error + "\n" + url);
                yield return false;
            }
            else
            {
                //下载完成后执行的回调
                if (request.isDone)
                {
                    byte[] results = request.downloadHandler.data;
                    Save(savePath, results);
                    yield return true;
                }

            }
        }

        public void Save(string filePath, byte[] data)
        {
            string dir = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            File.WriteAllBytes(filePath, data);

            AssetDatabase.Refresh();
        }

    }
}
