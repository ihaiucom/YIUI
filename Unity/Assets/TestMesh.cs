using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMesh : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    [ContextMenu("Test")]
    public void Test()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = new Vector3[3] { new Vector3(-4, 0, 0), new Vector3(4, 0, 0), new Vector3(7, 8, 9) };
        //mesh.normals = new Vector3[3] { new Vector3(0, 0, 1), new Vector3(0, 0, 1), new Vector3(0, 0, 1) };
        mesh.colors = new Color[3] { Color.red, Color.green, Color.blue };
        mesh.triangles = new int[3] { 2, 1, 0 };

        MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
        if (meshFilter == null)
        {
            meshFilter = gameObject.AddComponent<MeshFilter>();
        }
        meshFilter.mesh = mesh;

        UnityEditor.AssetDatabase.CreateAsset(mesh, "Assets/test_mesh.asset");

    }
}
