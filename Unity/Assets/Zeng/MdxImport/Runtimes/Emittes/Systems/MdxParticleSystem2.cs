using MdxLib.Model;
using UnityEngine;
using UnityEngine.Rendering;

namespace Zeng.Mdx.Runtimes
{
    public class MdxParticleSystem2 : MonoBehaviour, IEmitterParameter
    {
        // �����������
        public int maxCount = 100;
        // ��keyframe���Ƿ�ɼ�
        public float visibility = 1;
        // ��keyframe�����λ�÷�Χ x
        public float width = 1;
        // ��keyframe�����λ�÷�Χ y
        public float length = 1;
        // ��keyframe������
        public float speed = 1;
        // ��keyframe��γ��, ��תY��Ƕ�
        public float latitude = 0;
        // ��keyframe������
        public float gravity = 0;
        // ��keyframe�����ӷ����ٶ�
        public float emissionRate = 1;
        public int emissionRateKey = -1;
        // ��keyframe���������
        public float variation = 0;

        // �Ƿ�������
        public bool squirt = false;
        // ��������ʱ��
        public float lifeSpan = 1.0f;
        // �������� �м�ֵ, 0->1->0
        public float timeMiddle = 0.5f;
        // ��ͼ
        public string texturePath;
        public Texture texture;
        // ����֡��ͼ ����
        public int columns = 1;
        // ����֡��ͼ ����
        public int rows = 1;
        // ����֡��ͼ һ���uv���
        public float cellWidth = 1;
        // ����֡��ͼ һ���uv�߶�
        public float cellHeight = 1;
        // �Ƿ�ʹ�ö�����ɫ
        public int teamColored = 0;
        // ��ͼID 1 teamColors[teamColored], 2 teamGlows[teamColored], > 2 ��ͼ
        public int replaceableId = 0;
        public int textureId = 0;



        // ��β����
        public float tailLength = 0;

        // ����β��
        public bool head = true;
        // �Ƿ���β
        public bool tail = true;

        public Vector4[] colors = new Vector4[3];
        public float[] scaling = new float[3] { 0.5f, 1.0f, 0.5f };
        // [headIntervals[0] HeadLife, headIntervals[1] HeadDecay,tailIntervals[0] TailLife,  tailIntervals[1] TailDecay]
        public Vector4[] intervals = new Vector4[4];
        // �Ƿ�������תX��
        public bool lineEmitter;
        // �Ƿ���ģ�Ϳռ�
        public bool modelSpace = true;
        // �Ƿ� xyƽ�泯������vectors
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
