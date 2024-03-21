using MdxLib.Model;
using UnityEngine;
using UnityEngine.Rendering;

namespace Zeng.Mdx.Runtimes
{
    public class MdxParticleSystem2 : MonoBehaviour, IEmitterParameter
    {
        // 最大粒子数量
        public int maxCount = 100;
        // 【keyframe】是否可见
        public float visibility = 1;
        // 【keyframe】随机位置范围 x
        public float width = 1;
        // 【keyframe】随机位置范围 y
        public float length = 1;
        // 【keyframe】缩放
        public float speed = 1;
        // 【keyframe】纬度, 旋转Y轴角度
        public float latitude = 0;
        // 【keyframe】重力
        public float gravity = 0;
        // 【keyframe】粒子发射速度
        public float emissionRate = 1;
        public int emissionRateKey = -1;
        // 【keyframe】随机缩放
        public float variation = 0;

        // 是否是喷射
        public bool squirt = false;
        // 生命周期时间
        public float lifeSpan = 1.0f;
        // 生命周期 中间值, 0->1->0
        public float timeMiddle = 0.5f;
        // 贴图
        public string texturePath;
        public Texture texture;
        // 序列帧贴图 列数
        public int columns = 1;
        // 序列帧贴图 行数
        public int rows = 1;
        // 序列帧贴图 一格的uv宽度
        public float cellWidth = 1;
        // 序列帧贴图 一格的uv高度
        public float cellHeight = 1;
        // 是否使用队伍颜色
        public int teamColored = 0;
        // 贴图ID 1 teamColors[teamColored], 2 teamGlows[teamColored], > 2 贴图
        public int replaceableId = 0;
        public int textureId = 0;



        // 拖尾长度
        public float tailLength = 0;

        // 非拖尾的
        public bool head = true;
        // 是否拖尾
        public bool tail = true;

        public Vector4[] colors = new Vector4[3];
        public float[] scaling = new float[3] { 0.5f, 1.0f, 0.5f };
        // [headIntervals[0] HeadLife, headIntervals[1] HeadDecay,tailIntervals[0] TailLife,  tailIntervals[1] TailDecay]
        public Vector4[] intervals = new Vector4[4];
        // 是否限制旋转X轴
        public bool lineEmitter;
        // 是否是模型空间
        public bool modelSpace = true;
        // 是否 xy平面朝向，用于vectors
        public bool xZQuad = false;




        public EParticleEmitter2FilterMode filterMode = EParticleEmitter2FilterMode.Additive;
        public BlendMode blendSrc = (BlendMode)5;
        public BlendMode blendDst = (BlendMode)1;
        public int priorityPlane = 3500;

        public bool allowParticleSpawn = true;

        public float timeScale = 1.0f;
        public float TimeScale
        {
            get { return timeScale; }
            set { timeScale = value; }
        }


        public EmittedObjectUpdater emittedObjectUpdater
        {
            get
            {
                return EmittedObjectUpdater.I;
            }
        }

        public Material material;
        public ParticleEmitter2 emitter;
        public int debug_alive = 0;
        public int debug_objectsCount = 0;

        private void Start()
        {
            Reset();
        }

        private void OnEnable()
        {
            Reset();
        }

        private void Reset()
        {
            cellWidth = 1.0f / columns;
            cellHeight = 1.0f / rows;

            if (emitter == null)
            {
                emitter = new ParticleEmitter2(this);
            }
        }

        private void Update()
        {
            //if (this.visibility > 0)
            //{
            //}
            emitter.update(Time.deltaTime);

            debug_alive = emitter.alive;
            debug_objectsCount = emitter.objects.Count;
        }

    }
}
