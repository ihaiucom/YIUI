using System;
using System.Collections.Generic;
using Zeng.mdx.commons;

namespace Zeng.mdx.parsers.mdx
{
    /**
     * The parent class for all objects that have animated data in them.
     */
    public class AnimatedObject
    {
        public List<Animation> animations = new List<Animation>();
        public Dictionary<string, Animation> animationMap = new Dictionary<string, Animation>();




        public void ReadAnimations(BinaryStream stream, int size)
        {
            int end = stream.Index + size;

            while (stream.Index < end)
            {
                string name = stream.ReadBinary(4);
                Type type = AnimationMap.map[name].cls;
                Animation animation = (Animation)Activator.CreateInstance(type);

                animation.ReadMdx(stream, name);

                this.animations.Add(animation);
                this.animationMap.Add(name, animation);
            }
        }

        public void WriteAnimations(BinaryStream stream)
        {
            foreach (Animation animation in this.animations)
            {
                animation.WriteMdx(stream);
            }
        }

        /**
         * AnimatedObject itself doesn't care about versions, however objects that inherit it do.
         */
        public virtual int GetByteLength(int version = 0)
        {
            int size = 0;

            foreach (Animation animation in this.animations)
            {
                size += animation.GetByteLength();
            }

            return size;
        }


    }

}
