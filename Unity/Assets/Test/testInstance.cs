using UnityEngine.Rendering;
using UnityEngine;
using System.Collections.Generic;

public class testInstance : MonoBehaviour
{
    //�ݲ����õ���mesh
    Mesh mesh;
    Material mat;
    public GameObject m_prefab;
    Matrix4x4[] matrix;
    ShadowCastingMode castShadows;//��Ӱѡ��
    public int InstanceCount = 10;
    //����Ԥ���������ɺ���Ҷ����mesh���
    MeshFilter[] meshFs;
    Renderer[] renders;
    private MaterialPropertyBlock props;

    //�������������unity5.6�������Ե�Enable Instance Variants��ѡ��
    public bool turnOnInstance = true;
    void Start()
    {
        if (m_prefab == null)
            return;
        Shader.EnableKeyword("LIGHTMAP_ON");//����lightmap
                                            //Shader.DisableKeyword("LIGHTMAP_OFF");
        var mf = m_prefab.GetComponent<MeshFilter>();
        if (mf)
        {
            mesh = m_prefab.GetComponent<MeshFilter>().sharedMesh;
            mat = m_prefab.GetComponent<Renderer>().sharedMaterial;
        }
        //���һ��Ԥ���� �ɶ��mesh��ɣ�����Ҫ���ƶ��ٴ�
        if (mesh == null)
        {
            meshFs = m_prefab.GetComponentsInChildren<MeshFilter>();
        }
        if (mat == null)
        {
            renders = m_prefab.GetComponentsInChildren<Renderer>();
        }
        matrix = new Matrix4x4[InstanceCount];

        castShadows = ShadowCastingMode.On;


        props = new MaterialPropertyBlock();

        List<Vector4> colorList = new List<Vector4>();
        //�������λ��������
        for (int i = 0; i < InstanceCount; i++)
        {
            ///   random position
            float x = Random.Range(-5000, 5000);
            float y = Random.Range(-3, 3);
            float z = Random.Range(-5000, 5000);
            matrix[i] = Matrix4x4.identity;   ///   set default identity
            //����λ��
            matrix[i].SetColumn(3, new Vector4(x, 0.0f, z, 1));  /// 4th colummn: set   position
            //��������
            //matrix[i].m00   = Mathf.Max(1, x);
            //matrix[i].m11   = Mathf.Max(1, y);
            //matrix[i].m22   = Mathf.Max(1, z);
            colorList.Add(new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f)));

        }
        props.SetVectorArray("_Color", colorList);
    }
    void Update()
    {
        if (turnOnInstance)
        {
            castShadows = ShadowCastingMode.On;
            if (mesh)
                Graphics.DrawMeshInstanced(mesh, 0, mat, matrix, matrix.Length, props, castShadows, true, 0, null);
            else
            {
                for (int i = 0; i < meshFs.Length; ++i)
                {
                    Graphics.DrawMeshInstanced(meshFs[i].sharedMesh, 0, renders[i].sharedMaterial, matrix, matrix.Length, props, castShadows, true, 0, null);
                }
            }
        }
    }
}