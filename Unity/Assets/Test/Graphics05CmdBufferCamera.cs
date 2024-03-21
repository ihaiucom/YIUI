using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public enum CameraCmdBuffer
{
    DRAW_RENDERER,
    DRAW_RENDERER_TARGET,
    DRAW_MESH,
    DRAW_MESH_TARGET
}

public class Graphics05CmdBufferCamera : MonoBehaviour
{
    public Camera mainCamera;
    public Material cmdMat;
    public Renderer cubeRenderer;
    public RenderTexture target;
    public Color clearColor = Color.red;
    public int triCount = 6;
    public float radius = 5;

    private CommandBuffer cmdBuffer;
    private Mesh mesh;

    public void DrawRenderer()
    {
        cmdBuffer.Clear();
        cmdBuffer.DrawRenderer(cubeRenderer, cmdMat);
    }

    public void DrawRendererToTarget()
    {
        cmdBuffer.Clear();
        cmdBuffer.SetRenderTarget(target);
        cmdBuffer.ClearRenderTarget(true, true, clearColor);
        cmdBuffer.DrawRenderer(cubeRenderer, cmdMat);
    }

    public void DrawMesh()
    {
        cmdBuffer.Clear();
        cmdBuffer.DrawMesh(mesh, Matrix4x4.identity, cmdMat);
    }

    public void DrawMeshToTarget()
    {
        cmdBuffer.Clear();
        cmdBuffer.SetRenderTarget(target);
        cmdBuffer.ClearRenderTarget(true, true, clearColor);

        cmdBuffer.DrawMesh(mesh, Matrix4x4.identity, cmdMat);
    }

    private void Start()
    {
        cmdBuffer = new CommandBuffer() { name = "CameraCmdBuffer" };

        mainCamera.AddCommandBuffer(CameraEvent.AfterForwardOpaque, cmdBuffer);

        if (mesh == null)
        {
            mesh = Graphics00Mesh.Instance.GetMesh(triCount, radius);
        }
    }

    private void OnValidate()
    {
        mesh = Graphics00Mesh.Instance.GetMesh(triCount, radius);
    }

    private void OnDisable()
    {
        mainCamera.RemoveAllCommandBuffers();
    }
}



//public class Graphics00Mesh
//{
//    [Range(3, 100)]
//    public int triCount = 6;
//    public float radius = 5;
//    public bool showHalf = false;

//    private static Graphics00Mesh instance;

//    public static Graphics00Mesh Instance
//    {
//        get
//        {
//            if (instance == null)
//            {
//                instance = new Graphics00Mesh();
//            }

//            return instance;
//        }
//    }

//    public Mesh GetMesh(int triCount, float radius)
//    {
//        this.triCount = triCount;
//        this.radius = radius;

//        Mesh mesh = new Mesh();
//        mesh.name = MeshName;
//        mesh.vertices = Vertices;
//        mesh.triangles = Triangles;
//        mesh.uv = Uvs;

//        return mesh;
//    }

//    protected string MeshName
//    {
//        get
//        {
//            return "Circle Mesh";
//        }
//    }

//    protected Vector3[] Vertices
//    {
//        get
//        {
//            Vector3[] vertices = new Vector3[triCount + 1];
//            vertices[0] = Vector3.zero;
//            float angleDelta = 2 * Mathf.PI / triCount;

//            for (int i = 0; i < triCount; i++)
//            {
//                float angle = angleDelta * i;
//                float x = radius * Mathf.Cos(angle);
//                float y = radius * Mathf.Sin(angle);

//                vertices[i + 1] = new Vector3(x, y, 0);
//            }

//            return vertices;
//        }
//    }

//    protected int[] Triangles
//    {
//        get
//        {
//            int[] triangles = new int[triCount * 3];

//            for (int i = 0; i < triCount; i++)
//            {
//                if (showHalf)
//                {
//                    if (i % 2 == 0) continue;
//                }

//                triangles[i * 3] = 0;
//                triangles[i * 3 + 2] = i + 1;

//                if (i + 2 > triCount)
//                {
//                    triangles[i * 3 + 1] = 1;
//                }
//                else
//                {
//                    triangles[i * 3 + 1] = i + 2;
//                }
//            }
//            return triangles;
//        }
//    }

//    protected Vector2[] Uvs
//    {
//        get
//        {
//            Vector2[] uvs = new Vector2[triCount + 1];
//            uvs[0] = new Vector2(0.5f, 0.5f);
//            float angleDelta = 2 * Mathf.PI / triCount;

//            for (int i = 0; i < triCount; i++)
//            {
//                float angle = angleDelta * i;
//                float x = Mathf.Cos(angle) * 0.5f + 0.5f;
//                float y = Mathf.Sin(angle) * 0.5f + 0.5f;

//                uvs[i + 1] = new Vector2(x, y);
//            }
//            return uvs;
//        }
//    }
//}
