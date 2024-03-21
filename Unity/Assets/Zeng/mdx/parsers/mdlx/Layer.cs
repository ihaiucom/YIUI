
using Zeng.mdx.commons;

namespace Zeng.mdx.parsers.mdx
{

    public class Layer : AnimatedObject
    {
        public enum FilterMode
        {
            None = 0,
            Transparent = 1,
            Blend = 2,
            Additive = 3,
            AddAlpha = 4,
            Modulate = 5,
            Modulate2x = 6
        }

        public enum Flags
        {
            None = 0x0,
            Unshaded = 0x1,
            SphereEnvMap = 0x2,
            TwoSided = 0x10,
            Unfogged = 0x20,
            NoDepthTest = 0x40,
            NoDepthSet = 0x80,
            Unlit = 0x100
        }


        public FilterMode filterMode = FilterMode.None;
        public Flags flags = Flags.None;
        public int textureId = -1;
        public int textureAnimationId = -1;
        public uint coordId = 0;
        public float alpha = 1;
        /** 
         * @since 900
         */
        public float emissiveGain = 1;
        /** 
         * @since 1000
         */
        public float[] fresnelColor = new float[] { 1, 1, 1 };
        /** 
         * @since 1000
         */
        public float fresnelOpacity = 0;
        /** 
         * @since 1000
         */
        public float fresnelTeamColor = 0;


        //-------------------
        public bool unshaded = false;
        public bool sphereEnvironmentMap = false;
        public bool twoSided = false;
        public bool unfogged = false;
        public bool noDepthTest = false;
        public bool noDepthSet = false;

        public void setFlags()
        {

            int flags = (int)this.flags;

            this.unshaded = (flags & (int)Flags.Unshaded) != 0;
            this.sphereEnvironmentMap = (flags & (int)Flags.SphereEnvMap) != 0; 
            this.twoSided = (flags & (int)Flags.TwoSided) != 0; 
            this.unfogged = (flags & (int)Flags.Unfogged) != 0; 
            this.noDepthTest = (flags & (int)Flags.NoDepthTest) != 0; 
            this.noDepthSet = (flags & (int)Flags.NoDepthSet) != 0; 
        }

        // Layer
        public Animation AnimTextureID { get { return animationMap.ContainsKey("KMTF") ? animationMap["KMTF"] : null; } }
        public Animation AnimAlpha { get { return animationMap.ContainsKey("KMTA") ? animationMap["KMTA"] : null; } }
        public Animation AnimEmissiveGain { get { return animationMap.ContainsKey("KMTE") ? animationMap["KMTE"] : null; } }
        public Animation AnimFresnelColor { get { return animationMap.ContainsKey("KFC3") ? animationMap["KFC3"] : null; } }
        public Animation AnimFresnelOpacity { get { return animationMap.ContainsKey("KFCA") ? animationMap["KFCA"] : null; } }
        public Animation AnimFresnelTeamColor { get { return animationMap.ContainsKey("KFTC") ? animationMap["KFTC"] : null; } }

        public void ReadMdx(BinaryStream stream, int version)
        {
            int start = stream.Index;
            uint size = stream.ReadUint32();

            filterMode = (FilterMode)stream.ReadUint32();
            flags = (Flags)stream.ReadUint32();
            textureId = stream.ReadInt32();
            textureAnimationId = stream.ReadInt32();
            coordId = stream.ReadUint32();
            alpha = stream.ReadFloat32();

            // Note that even though these fields were introduced in versions 900 and 1000 separately, the game does not offer backwards compatibility.
            if (version > 800)
            {
                emissiveGain = stream.ReadFloat32();
                stream.ReadFloat32Array(fresnelColor);
                fresnelOpacity = stream.ReadFloat32();
                fresnelTeamColor = stream.ReadFloat32();
            }

            ReadAnimations(stream, (int)(size - (stream.Index - start)));

            setFlags();
        }

        public void WriteMdx(BinaryStream stream, int version)
        {
            stream.WriteUint32((uint)GetByteLength(version));
            stream.WriteUint32((uint)filterMode);
            stream.WriteUint32((uint)flags);
            stream.WriteInt32(textureId);
            stream.WriteInt32(textureAnimationId);
            stream.WriteUint32(coordId);
            stream.WriteFloat32(alpha);

            // See note above in readMdx.
            if (version > 800)
            {
                stream.WriteFloat32(emissiveGain);
                stream.WriteFloat32Array(fresnelColor);
                stream.WriteFloat32(fresnelOpacity);
                stream.WriteFloat32(fresnelTeamColor);
            }

            WriteAnimations(stream);
        }

        public override int GetByteLength(int version)
        {
            int size = 28 + base.GetByteLength(version);

            // See note above in readMdx.
            if (version > 800)
            {
                size += 24;
            }

            return size;
        }



    }
}
