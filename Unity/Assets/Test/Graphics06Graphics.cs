using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public enum DrawLocation
{
    ONGUI,
    POSTRENDER,
    UPDATE
}

public class Graphics06Graphics : MonoBehaviour
{
    public DrawLocation location = DrawLocation.ONGUI;
    public bool toTarget = false;
    public Texture mainTexture;
    public RenderTexture target;
    public Color clearColor = Color.red;

    CommandBuffer clearBuffer;

    void Draw()
    {
        if (toTarget)
        {
            DrawTextureToTarget();
        }
        else
        {
            DrawTexture();
        }
    }

    public void DrawTexture()
    {
        GL.PushMatrix();
        //GL.LoadPixelMatrix();
        GL.LoadPixelMatrix(0, Screen.width, Screen.height, 0);
        Graphics.DrawTexture(new Rect(0, 0, 200, 100), mainTexture);
        GL.PopMatrix();
    }

    public void DrawTextureToTarget()
    {
        Graphics.SetRenderTarget(target);
        clearBuffer.Clear();
        clearBuffer.ClearRenderTarget(true, true, clearColor);
        Graphics.ExecuteCommandBuffer(clearBuffer);

        GL.PushMatrix();
        GL.LoadPixelMatrix(0, target.width, target.height, 0);
        //GL.LoadPixelMatrix(0, target.width, 0, target.height);
        Graphics.DrawTexture(new Rect(0, 0, target.width, target.height), mainTexture);
        GL.PopMatrix();
    }

    private void Start()
    {
        clearBuffer = new CommandBuffer() { name = "Clear Buffer" };
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
        //如果此时绘制到屏幕上，则不会看到绘制的结果
        Draw();
    }

    private void OnPostRender()
    {
        if (location != DrawLocation.POSTRENDER) return;

        Draw();
    }
}

