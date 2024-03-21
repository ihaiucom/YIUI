using System;
using System.IO;
using Zeng.mdx.commons;

namespace Zeng.mdx.parsers.mdx
{

    /**
     * An event object.
     */
    public class EventObject : GenericObject
    {
        public class EventObjectType {
            public const string SPN = "SPN"; // Splats\\SpawnData.slk
            public const string FPT = "FPT"; // Splats\\SplatData.slk
            public const string SPL = "SPL";   // Splats\\SplatData.slk
            public const string UBR = "UBR";  // Splats\\UberSplatData.slk
            public const string SND = "SND";  // UI\\SoundInfo\\AnimSounds.slk

        }


        public int globalSequenceId = -1;
        public uint[] tracks = new uint[0];

        public string type;
        public string id;

        public EventObject() : base(0x400)
        {
        }

        public override void ReadMdx(BinaryStream stream, int version)
        {
            base.ReadMdx(stream, version);

            stream.Skip(4); // KEVT

            uint count = stream.ReadUint32();

            globalSequenceId = stream.ReadInt32();
            tracks = stream.ReadUint32Array((int)count);

            // TODO ZF
            type = name.Substring(0, 3);
            id = name.Substring(4);

            UnityEngine.Debug.Log($"EventObject: type={type}, id={id}");
        }

        public override void WriteMdx(BinaryStream stream, int version)
        {
            base.WriteMdx(stream, version);
            stream.WriteBinary("KEVT");
            stream.WriteUint32((uint)tracks.Length);
            stream.WriteInt32(globalSequenceId);
            stream.WriteUint32Array(tracks);
        }

        public override int GetByteLength(int version = 0)
        {
            return 12 + tracks.Length * sizeof(uint) + base.GetByteLength(version);
        }
    }

}
