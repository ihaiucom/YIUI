using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Graphics07DrawMeshToTarget : MonoBehaviour
{
    public DrawLocation location = DrawLocation.ONGUI;
    public bool toTarget = false;
    public Color clearColor = Color.blue;
    public Material mat;
    public RenderTexture target;
    public int triCount = 10;
    public float radius = 20;

    CommandBuffer drawBuffer;

    void Draw()
    {
        if (toTarget)
        {
            DrawToTarget();
        }
        else
        {
            DrawToScreen();
        }
    }

    private void DrawToScreen()
    {
        drawBuffer.Clear();
        drawBuffer.ClearRenderTarget(true, true, clearColor);
        Graphics.ExecuteCommandBuffer(drawBuffer);

        drawBuffer.DrawMesh(Graphics00Mesh.Instance.GetMesh(triCount, radius), Matrix4x4.identity, mat);
        Graphics.ExecuteCommandBuffer(drawBuffer);
    }

    private void DrawToTarget()
    {
        GL.PushMatrix();
        Graphics.SetRenderTarget(target);
        GL.LoadPixelMatrix(0, target.width, 0, target.height);

        drawBuffer.Clear();
        drawBuffer.ClearRenderTarget(true, true, clearColor);
        Graphics.ExecuteCommandBuffer(drawBuffer);

        drawBuffer.DrawMesh(Graphics00Mesh.Instance.GetMesh(triCount, radius), Matrix4x4.identity, mat);
        Graphics.ExecuteCommandBuffer(drawBuffer);

        GL.PopMatrix();
    }

    private void Start()
    {
        drawBuffer = new CommandBuffer() { name = "DrawMesh buffer" };
    }

    private void OnGUI()
    {
        if (location != DrawLocation.ONGUI) return;

        if (Event.current.type.Equals(EventType.Repaint))
        {
            Draw();
        }
    }

    private void Update()
    {
        if (location != DrawLocation.UPDATE) return;

        Draw();
    }

    private void OnPostRender()
    {
        if (location != DrawLocation.POSTRENDER) return;

        Draw();
    }
}

