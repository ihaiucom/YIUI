using Zeng.mdx.commons;

namespace Zeng.mdx.parsers.mdx
{


    /**
     * A particle emitter type 2.
     */
    public class ParticleEmitter2 : GenericObject
    {
        public enum Flags
        {
            Unshaded = 0x8000,
            SortPrimsFarZ = 0x10000,
            LineEmitter = 0x20000,
            Unfogged = 0x40000,
            ModelSpace = 0x80000,
            XYQuad = 0x100000
        }

        public enum FilterMode
        {
            Blend = 0,
            Additive = 1,
            Modulate = 2,
            Modulate2x = 3,
            AlphaKey = 4
        }

        public enum HeadOrTail
        {
            Head = 0,
            Tail = 1,
            Both = 2
        }

        public float speed = 0;
        public float variation = 0; // 加速度
        public float latitude = 0;
        public float gravity = 0;
        public float lifeSpan = 0;
        public float emissionRate = 0;
        public float width = 0;
        public float length = 0;
        public FilterMode filterMode = FilterMode.Blend;
        public uint rows = 0;
        public uint columns = 0;
        public HeadOrTail headOrTail = HeadOrTail.Head;
        public float tailLength = 0;
        public float timeMiddle = 0;
        public float[][] segmentColors = new float[3][] { new float[3], new float[3], new float[3] };
        public byte[] segmentAlphas = new byte[3];
        public float[] segmentScaling = new float[3];
        public uint[][] headIntervals = new uint[2][] { new uint[3], new uint[3] };
        public uint[][] tailIntervals = new uint[2][] { new uint[3], new uint[3] };
        public int textureId = -1;
        public uint squirt = 0;
        public int priorityPlane = 0;
        public uint replaceableId = 0;

        public bool Head {
            get { return headOrTail == HeadOrTail.Head || headOrTail == HeadOrTail.Both; }
        }

        public bool Tail
        {
            get { return headOrTail == HeadOrTail.Tail || headOrTail == HeadOrTail.Both; }
        }

        public string ReplaceablePath {
            get { return Texture.GetTexturePath(null, (int)replaceableId); }
        }


        // ParticleEmitter2
        public Animation AnimSpeed { get { return animationMap.ContainsKey("KP2S") ? animationMap["KP2S"] : null; } }
        public Animation AnimVariation { get { return animationMap.ContainsKey("KP2R") ? animationMap["KP2R"] : null; } }
        public Animation AnimLatitude { get { return animationMap.ContainsKey("KP2L") ? animationMap["KP2L"] : null; } }
        public Animation AnimGravity { get { return animationMap.ContainsKey("KP2G") ? animationMap["KP2G"] : null; } }
        public Animation AnimEmissionRate { get { return animationMap.ContainsKey("KP2E") ? animationMap["KP2E"] : null; } }
        public Animation AnimWidth { get { return animationMap.ContainsKey("KP2N") ? animationMap["KP2N"] : null; } }
        public Animation AnimLength { get { return animationMap.ContainsKey("KP2W") ? animationMap["KP2W"] : null; } }
        public Animation AnimVisibility { get { return animationMap.ContainsKey("KP2V") ? animationMap["KP2V"] : null; } }

        public override void ReadMdx(BinaryStream stream, int version)
        {
            uint start = (uint)stream.Index;
            uint size = stream.ReadUint32();

            base.ReadMdx(stream, version);

            speed = stream.ReadFloat32();
            variation = stream.ReadFloat32();
            latitude = stream.ReadFloat32();
            gravity = stream.ReadFloat32();
            lifeSpan = stream.ReadFloat32();
            emissionRate = stream.ReadFloat32();
            width = stream.ReadFloat32();
            length = stream.ReadFloat32();
            filterMode = (FilterMode)stream.ReadUint32();
            rows = stream.ReadUint32();
            columns = stream.ReadUint32();
            headOrTail = (HeadOrTail)stream.ReadUint32();
            tailLength = stream.ReadFloat32();
            timeMiddle = stream.ReadFloat32();
            stream.ReadFloat32Array(segmentColors[0]);
            stream.ReadFloat32Array(segmentColors[1]);
            stream.ReadFloat32Array(segmentColors[2]);
            stream.ReadUint8Array(segmentAlphas);
            stream.ReadFloat32Array(segmentScaling);
            stream.ReadUint32Array(headIntervals[0]);
            stream.ReadUint32Array(headIntervals[1]);
            stream.ReadUint32Array(tailIntervals[0]);
            stream.ReadUint32Array(tailIntervals[1]);
            textureId = stream.ReadInt32();
            squirt = stream.ReadUint32();
            priorityPlane = stream.ReadInt32();
            replaceableId = stream.ReadUint32();

            ReadAnimations(stream, (int)(size - (stream.Index - start)));
        }

        public override void WriteMdx(BinaryStream stream, int version)
        {
            stream.WriteUint32((uint)GetByteLength());

            base.WriteMdx(stream, version);

            stream.WriteFloat32(speed);
            stream.WriteFloat32(variation);
            stream.WriteFloat32(latitude);
            stream.WriteFloat32(gravity);
            stream.WriteFloat32(lifeSpan);
            stream.WriteFloat32(emissionRate);
            stream.WriteFloat32(width);
            stream.WriteFloat32(length);
            stream.WriteUint32((uint)filterMode);
            stream.WriteUint32(rows);
            stream.WriteUint32(columns);
            stream.WriteUint32((uint)headOrTail);
            stream.WriteFloat32(tailLength);
            stream.WriteFloat32(timeMiddle);
            stream.WriteFloat32Array(segmentColors[0]);
            stream.WriteFloat32Array(segmentColors[1]);
            stream.WriteFloat32Array(segmentColors[2]);
            stream.WriteUint8Array(segmentAlphas);
            stream.WriteFloat32Array(segmentScaling);
            stream.WriteUint32Array(headIntervals[0]);
            stream.WriteUint32Array(headIntervals[1]);
            stream.WriteUint32Array(tailIntervals[0]);
            stream.WriteUint32Array(tailIntervals[1]);
            stream.WriteInt32(textureId);
            stream.WriteUint32(squirt);
            stream.WriteInt32(priorityPlane);
            stream.WriteUint32(replaceableId);

            WriteNonGenericAnimationChunks(stream);
        }

        public override int GetByteLength(int version = 0)
        {
            return 175 + base.GetByteLength(version);
        }


    }
}
