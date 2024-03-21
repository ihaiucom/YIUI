using MdxLib.Model;
using UnityEngine.Rendering;

namespace Zeng.Mdx.Runtimes
{
    public class FilterModeBlendUtils
    {
        public static BlendMode[] emitterFilterMode(EParticleEmitter2FilterMode filterMode)
        {
            switch (filterMode)
            {
                case EParticleEmitter2FilterMode.Blend:
                    return new BlendMode[] { BlendMode.SrcAlpha, BlendMode.OneMinusSrcAlpha };
                case EParticleEmitter2FilterMode.Additive:
                    return new BlendMode[] { BlendMode.SrcAlpha, BlendMode.One };
                case EParticleEmitter2FilterMode.AlphaKey:
                    return new BlendMode[] { BlendMode.SrcAlpha, BlendMode.OneMinusSrcAlpha };
                case EParticleEmitter2FilterMode.Modulate:
                    return new BlendMode[] { BlendMode.Zero, BlendMode.SrcColor };
                case EParticleEmitter2FilterMode.Modulate2x:
                    return new BlendMode[] { BlendMode.DstColor, BlendMode.SrcColor };


            }
            return new BlendMode[] { BlendMode.Zero, BlendMode.Zero };
        }
    }
}
