using System.Collections.Generic;
using Zeng.mdx.commons;

namespace Zeng.mdx.parsers.mdx
{
    public enum GenericObjectFlags
    {
        None = 0x0,
        DontInheritTranslation = 0x1,
        DontInheritScaling = 0x2,
        DontInheritRotation = 0x4,
        Billboarded = 0x8,
        BillboardedLockX = 0x10,
        BillboardedLockY = 0x20,
        BillboardedLockZ = 0x40,
        CameraAnchored = 0x80,
    }


    /**
     * A generic object.
     *
     * The parent class for all objects that exist in the world, and may contain spatial animations.
     * This includes bones, particle emitters, and many other things.
     */
    public abstract class GenericObject : AnimatedObject, IMdxDynamicObject
    {
        public string name = "";
        public int objectId = -1;
        public int parentId = -1;
        public int flags;

        //--------------------

        public bool dontInheritTranslation;
        public bool dontInheritRotation;
        public bool dontInheritScaling;
        public bool billboarded;
        public bool billboardedX;
        public bool billboardedY;
        public bool billboardedZ;
        public bool cameraAnchored;
        public bool anyBillboarding;

        public void setFlags()
        {
            int flags = this.flags;
            this.dontInheritTranslation = (flags & (int)GenericObjectFlags.DontInheritTranslation) > 0;
            this.dontInheritRotation = (flags & (int)GenericObjectFlags.DontInheritRotation) > 0;
            this.dontInheritScaling = (flags & (int)GenericObjectFlags.DontInheritScaling) > 0;
            this.billboarded = (flags & (int)GenericObjectFlags.Billboarded) > 0;
            this.billboardedX = (flags & (int)GenericObjectFlags.BillboardedLockX) > 0;
            this.billboardedY = (flags & (int)GenericObjectFlags.BillboardedLockY) > 0;
            this.billboardedZ = (flags & (int)GenericObjectFlags.BillboardedLockZ) > 0;
            this.cameraAnchored = (flags & (int)GenericObjectFlags.CameraAnchored) > 0;
            this.anyBillboarding = this.billboarded || this.billboardedX || this.billboardedY || this.billboardedZ;
        }

        // GenericObject
        public Animation Translation { get { return animationMap.ContainsKey("KGTR") ? animationMap["KGTR"] : null; } }
        public Animation Rotation { get { return animationMap.ContainsKey("KGRT") ? animationMap["KGRT"] : null; } }
        public Animation Scaling { get { return animationMap.ContainsKey("KGSC") ? animationMap["KGSC"] : null; } }


        public GenericObject(int flags = 0)
        {
            this.flags = flags;
        }

        public virtual void ReadMdx(BinaryStream stream, int version)
        {
            int size = (int)stream.ReadUint32();

            this.name = stream.Read(80);
            this.objectId = stream.ReadInt32();
            this.parentId = stream.ReadInt32();
            this.flags = (int)stream.ReadUint32();

            this.ReadAnimations(stream, size - 96);

            setFlags();
        }

        public virtual void WriteMdx(BinaryStream stream, int version)
        {
            stream.WriteUint32((uint)this.GetGenericByteLength());
            stream.Skip(80 - stream.Write(this.name));
            stream.WriteInt32(this.objectId);
            stream.WriteInt32(this.parentId);
            stream.WriteUint32((uint)this.flags);

            foreach (Animation animation in this.eachAnimation(true))
            {
                animation.WriteMdx(stream);
            }
        }


        /**
         * Allows to easily iterate either the GenericObject animations or the parent object animations.
         */
        public IEnumerable<Animation> eachAnimation(bool wantGeneric)
        {
            foreach (Animation animation in this.animations)
            {
                string name = animation.name;
                bool isGeneric = (name == "KGTR" || name == "KGRT" || name == "KGSC");

                if ((wantGeneric && isGeneric) || (!wantGeneric && !isGeneric))
                {
                    yield return animation;
                }
            }
        }

        protected void WriteNonGenericAnimationChunks(BinaryStream stream)
        {
            foreach (var animation in eachAnimation(false))
            {
                animation.WriteMdx(stream);
            }
        }


        /**
         * Gets the byte length of the GenericObject part of whatever this object this.
         * 
         * This is needed because only the KGTR, KGRT, and KGSC animations actually belong to it.
         */
        public int GetGenericByteLength()
        {
            int size = 96;

            foreach (Animation animation in this.eachAnimation(true))
            {
                size += animation.GetByteLength();
            }

            return size;
        }

        public override int GetByteLength(int version = 0)
        {
            return 96 + base.GetByteLength(version);
        }

    }


}
