using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GLExample1 : MonoBehaviour
{
    // Draws a triangle under an already drawn triangle
    public Material mat;

    void OnPostRender()
    {
        if (!mat)
        {
            Debug.LogError("Please Assign a material on the inspector");
            return;
        }
        GL.PushMatrix();
        mat.SetPass(0);
        GL.LoadOrtho();
        GL.Color(Color.red);
        GL.Begin(GL.TRIANGLES);
        GL.Vertex3(0.25f, 0.1351f, 0);
        GL.Vertex3(0.25f, 0.3f, 0);
        GL.Vertex3(0.5f, 0.3f, 0);
        GL.End();

        GL.Color(Color.yellow);
        GL.Begin(GL.TRIANGLES);
        GL.Vertex3(0.5f, 0.25f, -1);
        GL.Vertex3(0.5f, 0.1351f, -1);
        GL.Vertex3(0.1f, 0.25f, -1);
        GL.End();

        GL.PopMatrix();
    }
}