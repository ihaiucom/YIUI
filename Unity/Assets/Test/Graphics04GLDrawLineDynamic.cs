using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graphics04GLDrawLineDynamic : MonoBehaviour
{
    private Material glMat;
    private List<Vector3> points = new List<Vector3>();

    private void SetMaterialPass()
    {
        if (glMat == null)
        {
            glMat = new Material(Shader.Find("Hidden/Internal-Colored"));
        }

        glMat.SetPass(0);
    }

    private void OnRenderObject()
    {
        if (points.Count != 0)
        {
            SetMaterialPass();
            DrawLines();
        }
    }

    private void DrawLines()
    {
        GL.PushMatrix();
        GL.LoadPixelMatrix();

        GL.Begin(GL.LINES);
        GL.Color(Color.red);

        for (int i = 0; i < points.Count - 1; i++)
        {
            GL.Vertex(points[i]);
            GL.Vertex(points[i + 1]);
        }

        GL.Vertex(points[points.Count - 1]);
        GL.Vertex(Input.mousePosition);

        GL.End();
        GL.PopMatrix();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            points.Add(Input.mousePosition);
        }
    }
}

