using UnityEngine;
using UnityEditor;
using log4net.Util;

namespace Zeng.Mdx
{
    [CustomEditor(typeof(NodeBillboarded))]
    public class NodeBillboardedEditor : Editor
    {

        void OnSceneGUI()
        {
            NodeBillboarded nodeBillboarded = (NodeBillboarded)target;
            var transform = nodeBillboarded.transform;
            var Camera = UnityEditor.SceneView.lastActiveSceneView.camera;
            transform.LookAt(transform.position - Camera.transform.forward, Camera.transform.up);
            //Vector3 angles = transform.localEulerAngles;
            //angles += nodeBillboarded.localEulerAnglesOffset;
            //transform.localEulerAngles = angles;

            //var _mainCamera = UnityEditor.SceneView.lastActiveSceneView.camera;
            //var p = lookAtCamera.transform.position - _mainCamera.transform.position;
            //p = lookAtCamera.transform.position + p;
            //var up = _mainCamera.transform.right;
            //lookAtCamera.transform.LookAt(p, up);
        }

    }
}