
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.GraphicsBuffer;

namespace Zeng.Mdx.Runtimes
{
    [RequireComponent(typeof(MdxParticleSystem2))]
    public class MdxParticleSystem2Render : MonoBehaviour
    {
        public MdxParticleSystem2 systems;
        public Mesh mesh;
        public Mesh billboardedMesh;
        public Material material;


        void Start()
        {
            Reset();
        }

        private void OnEnable()
        {
            Reset();
        }

        private void Reset()
        {
            systems = GetComponent<MdxParticleSystem2>();

            if (mesh == null)
            {
                mesh = CreateQuad();
                billboardedMesh = CreateQuad();
            }

            if (billboardedMesh == null)
            {
                billboardedMesh = CreateQuad2();
            }

            if (props == null)
            {
                props = new MaterialPropertyBlock();
            }

            var blendArr = FilterModeBlendUtils.emitterFilterMode(systems.filterMode);
            systems.blendSrc = blendArr[0];
            systems.blendDst = blendArr[1];

            if (material == null)
            {
                material = Material.Instantiate(systems.material);


                //material.mainTexture = systems.texture;
            }

            if (material != null) {
                material.SetInt("_SrcBlend", (int)systems.blendSrc);
                material.SetInt("_DstBlend", (int)systems.blendDst);
            }

            a_health = new float[systems.maxCount];
            a_facing = new float[systems.maxCount];

            if (systems.texture != null)
            {
                props.SetTexture("_MainTex", systems.texture);
            }


            // 模式
            props.SetInt("u_filterMode", (int)systems.filterMode);
            props.SetInt("u_emitter", 0);

            // Shared
            props.SetFloat("u_lifeSpan", systems.lifeSpan);
            props.SetFloat("u_columns", systems.columns);
            props.SetFloat("u_rows", systems.rows);



            // Shared Array
            props.SetVectorArray("u_colors", systems.colors);
            props.SetVectorArray("u_intervals", systems.intervals);

            // Particle2
            props.SetFloat("u_timeMiddle", systems.timeMiddle);
            props.SetInt("u_teamColored", systems.teamColored);
            props.SetFloatArray("u_scaling", systems.scaling);
        }

        private Vector3 transformQuat(Vector3 a, Quaternion q)
        {
            Vector3 qvec = new Vector3(q.x, q.y, q.z);
            Vector3 uv = Vector3.Cross(qvec, a);
            Vector3 uuv = Vector3.Cross(qvec, uv);
            uv *= q.w * 2;
            uuv *= 2;
            Vector3 o = a + uv + uuv;
            return o;
        }
        public Vector3[] vertices;
        private void UpdateMeshBillboarded()
        {
            Vector3[] vertices = new Vector3[4];

            vertices[0] = new Vector3(-1, -1, 0); // 左下
            vertices[1] = new Vector3(-1, 1, 0); // 左上
            vertices[2] = new Vector3(1, 1, 0);  // 右上
            vertices[3] = new Vector3(1, -1, 0); // 右下


            //vertices[0] = new Vector3(-1, 0, -1); // 左下
            //vertices[1] = new Vector3(-1, 0, 1); // 左上
            //vertices[2] = new Vector3(1, 0, 1);  // 右上
            //vertices[3] = new Vector3(1, 0, -1); // 右下

            for (int i = 0; i < 4; i++)
            {
                //Vector3 targetDirection = Camera.main.transform.position - vertices[i];
                //vertices[i] = Vector3.RotateTowards(Vector3.forward, targetDirection, 1, 0.0f);
                vertices[i] = transformQuat(vertices[i], Camera.main.transform.rotation);
            }
            this.vertices = vertices;
            billboardedMesh.vertices = vertices;
        }

        private Mesh CreateQuad()
        {
            Mesh mesh = new Mesh();
            Vector3[] vertices = new Vector3[4];

            vertices[0] = new Vector3(-1, 0, -1); // 左下
            vertices[1] = new Vector3(-1, 0, 1); // 左上
            vertices[2] = new Vector3(1, 0, 1);  // 右上
            vertices[3] = new Vector3(1, 0, -1); // 右下

            mesh.vertices = vertices;


            //=======================================
            int[] tri = new int[6];

            //  Lower left triangle.
            tri[0] = 1;
            tri[1] = 3;
            tri[2] = 0;

            //  Upper right triangle.   
            tri[3] = 1;
            tri[4] = 2;
            tri[5] = 3;

            mesh.triangles = tri;


            //=======================================
            Vector3[] normals = new Vector3[4];
            normals[0] = -Vector3.down;
            normals[1] = -Vector3.down;
            normals[2] = -Vector3.down;
            normals[3] = -Vector3.down;
            mesh.normals = normals;

            //=======================================
            Vector2[] uv = new Vector2[4];

            uv[0] = new Vector2(0, 0);
            uv[1] = new Vector2(0, 1);
            uv[2] = new Vector2(1, 1);
            uv[3] = new Vector2(1, 0);

            mesh.uv = uv;
            return mesh;
        }

        private Mesh CreateQuad2()
        {
            Mesh mesh = new Mesh();
            Vector3[] vertices = new Vector3[4];

            vertices[0] = new Vector3(-1, -1, 0); // 左下
            vertices[1] = new Vector3(-1, 1, 0); // 左上
            vertices[2] = new Vector3(1, 1, 0);  // 右上
            vertices[3] = new Vector3(1, -1, 0); // 右下

            mesh.vertices = vertices;


            //=======================================
            int[] tri = new int[6];

            //  Lower left triangle.
            tri[0] = 1;
            tri[1] = 3;
            tri[2] = 0;

            //  Upper right triangle.   
            tri[3] = 1;
            tri[4] = 2;
            tri[5] = 3;

            mesh.triangles = tri;


            //=======================================
            Vector3[] normals = new Vector3[4];
            normals[0] = -Vector3.forward;
            normals[1] = -Vector3.forward;
            normals[2] = -Vector3.forward;
            normals[3] = -Vector3.forward;
            mesh.normals = normals;

            //=======================================
            Vector2[] uv = new Vector2[4];

            uv[0] = new Vector2(0, 0);
            uv[1] = new Vector2(0, 1);
            uv[2] = new Vector2(1, 1);
            uv[3] = new Vector2(1, 0);

            mesh.uv = uv;
            return mesh;
        }

        private List<Matrix4x4> matrices = new List<Matrix4x4>();
        public float[] a_health;
        public float[] a_facing;
        private MaterialPropertyBlock props;
        public int debug_alive = 0;
        public int debug_objectsCount = 0;
        public bool useMaxCount = false;
        private void Gen()
        {
            matrices.Clear();
            //a_health.Clear();
            List<EmittedObject> objects = systems.emitter.objects;
            int alive = systems.emitter.alive;
            if (alive <= 0) return;
            int count = alive;

            if (useMaxCount)
            {
                count = Mathf.Min(alive, systems.maxCount);
            }
            else
            {
                if (a_health.Length < objects.Count)
                {
                    a_health = new float[objects.Count];
                    a_facing = new float[objects.Count];
                }
            }


            for (int i = 0; i < count; i++)
            {
                Particle2 item = objects[i] as Particle2;
                item.SettingWorldMatrix();
                a_health[i] = item.health;
                a_facing[i] = item.facing;

                Matrix4x4 worldMatrix = item.worldMatrix;
                matrices.Add(worldMatrix);
            }

            for (int i = count; i < a_health.Length; i++)
            {
                a_health[i] = 0;
            }

            //for (int i = matrices.Count; i < a_health.Length; i++)
            //{
            //    matrices.Add(Matrix4x4.zero);
            //}

            // Instances
            props.SetFloatArray("a_health", a_health);
            props.SetFloatArray("a_facing", a_facing);
        }

        private ShadowCastingMode castShadows = ShadowCastingMode.Off;
        private bool receiveShadows = false;

        private void LateUpdate()
        {
            if (systems == null || systems.emitter == null) return;

            debug_alive = systems.emitter.alive;
            debug_objectsCount = systems.emitter.objects.Count;

            Gen();
            Mesh mesh = this.mesh;
            if (!systems.xZQuad)
            {
                UpdateMeshBillboarded();
                mesh = this.billboardedMesh;
            }

            if (matrices.Count > 0)
            {
                Graphics.DrawMeshInstanced(mesh, 0, material, matrices, props, castShadows, receiveShadows, 0, null);
            }
        }
    }
}
