using MdxLib.Animator;
using System.Collections.Generic;
using Zeng.mdx.commons;

namespace Zeng.mdx.parsers.mdx
{
    public enum InterpolationType {
        DontInterp = 0,
        Linear = 1,
        Hermite = 2,
        Bezier = 3,
    }

    /**
     * An animation.
     */
    public abstract class Animation
    {
        public string name = "";
        public InterpolationType interpolationType = InterpolationType.DontInterp;
        public int globalSequenceId = -1;
        public List<int> frames = new List<int>();
        public List<float[]> values = new List<float[]>();
        public List<float[]> inTans = new List<float[]>();
        public List<float[]> outTans = new List<float[]>();

        public abstract float[] ReadMdxValue(BinaryStream stream);
        public abstract void WriteMdxValue(BinaryStream stream, float[] value);

        public void ReadMdx(BinaryStream stream, string name)
        {
            List<int> frames = this.frames;
            List<float[]> values = this.values;
            List<float[]> inTans = this.inTans;
            List<float[]> outTans = this.outTans;
            int tracksCount = (int)stream.ReadUint32();
            int interpolationType = (int)stream.ReadUint32();

            this.name = name;
            this.interpolationType = (InterpolationType)interpolationType;
            this.globalSequenceId = stream.ReadInt32();

            for (int i = 0; i < tracksCount; i++)
            {
                frames.Add(stream.ReadInt32());
                values.Add(ReadMdxValue(stream));

                if (interpolationType > 1)
                {
                    inTans.Add(ReadMdxValue(stream));
                    outTans.Add(ReadMdxValue(stream));
                }
            }
        }

        public void WriteMdx(BinaryStream stream)
        {
            uint interpolationType = (uint)this.interpolationType;
            List<int> frames = this.frames;
            List<float[]> values = this.values;
            List<float[]> inTans = this.inTans;
            List<float[]> outTans = this.outTans;
            int tracksCount = frames.Count;

            stream.WriteBinary(this.name);
            stream.WriteUint32((uint)tracksCount);
            stream.WriteUint32(interpolationType);
            stream.WriteInt32(this.globalSequenceId);

            for (int i = 0; i < tracksCount; i++)
            {
                stream.WriteInt32(frames[i]);
                WriteMdxValue(stream, values[i]);

                if (interpolationType > (uint)InterpolationType.Linear)
                {
                    WriteMdxValue(stream, inTans[i]);
                    WriteMdxValue(stream, outTans[i]);
                }
            }
        }



        public int GetByteLength()
        {
            int tracksCount = this.frames.Count;
            int size = 16;

            if (tracksCount > 0)
            {
                int bytesPerValue = this.values[0].Length;
                int valuesPerTrack = 1;

                if (this.interpolationType > InterpolationType.Linear)
                {
                    valuesPerTrack = 3;
                }

                size += (4 + valuesPerTrack * bytesPerValue) * tracksCount;
            }

            return size;
        }


    }
}
