
using System;
using System.Collections.Generic;

namespace Zeng.mdx.parsers.mdx
{
    public class AnimationMap
    {
        public class AnimationMapItem
        {
            public string key;
            public Type cls;

            public AnimationMapItem(string key, Type cls)
            {
                this.key = key;
                this.cls = cls;
            }

        }
        public static Dictionary<string, AnimationMapItem> map = new Dictionary<string, AnimationMapItem>() {
            // Layer
            {"KMTF", new AnimationMapItem("TextureID", typeof( UintAnimation))},
            {"KMTA", new AnimationMapItem("Alpha", typeof( FloatAnimation))},
            {"KMTE", new AnimationMapItem("EmissiveGain", typeof( FloatAnimation))},
            {"KFC3", new AnimationMapItem("FresnelColor", typeof( Vector3Animation))},
            {"KFCA", new AnimationMapItem("FresnelOpacity", typeof( FloatAnimation))},
            {"KFTC", new AnimationMapItem("FresnelTeamColor", typeof( UintAnimation))},
            // TextureAnimation
            {"KTAT", new AnimationMapItem("Translation", typeof( Vector3Animation))},
            {"KTAR", new AnimationMapItem("Rotation", typeof( Vector4Animation))},
            {"KTAS", new AnimationMapItem("Scaling", typeof( Vector3Animation))},
            // GeosetAnimation
            {"KGAO", new AnimationMapItem("Alpha", typeof( FloatAnimation))},
            {"KGAC", new AnimationMapItem("Color", typeof( Vector3Animation))},
            // GenericObject
            {"KGTR", new AnimationMapItem("Translation", typeof( Vector3Animation))},
            {"KGRT", new AnimationMapItem("Rotation", typeof( Vector4Animation))},
            {"KGSC", new AnimationMapItem("Scaling", typeof( Vector3Animation))},
            // Light
            {"KLAS", new AnimationMapItem("AttenuationStart", typeof( FloatAnimation))},
            {"KLAE", new AnimationMapItem("AttenuationEnd", typeof( FloatAnimation))},
            {"KLAC", new AnimationMapItem("Color", typeof( Vector3Animation))},
            {"KLAI", new AnimationMapItem("Intensity", typeof( FloatAnimation))},
            {"KLBI", new AnimationMapItem("AmbIntensity", typeof( FloatAnimation))},
            {"KLBC", new AnimationMapItem("AmbColor", typeof( Vector3Animation))},
            {"KLAV", new AnimationMapItem("Visibility", typeof( FloatAnimation))},
            // Attachment
            {"KATV", new AnimationMapItem("Visibility", typeof( FloatAnimation))},
            // ParticleEmitter
            {"KPEE", new AnimationMapItem("EmissionRate", typeof( FloatAnimation))},
            {"KPEG", new AnimationMapItem("Gravity", typeof( FloatAnimation))},
            {"KPLN", new AnimationMapItem("Longitude", typeof( FloatAnimation))},
            {"KPLT", new AnimationMapItem("Latitude", typeof( FloatAnimation))},
            {"KPEL", new AnimationMapItem("LifeSpan", typeof( FloatAnimation))},
            {"KPES", new AnimationMapItem("InitVelocity", typeof( FloatAnimation))},
            {"KPEV", new AnimationMapItem("Visibility", typeof( FloatAnimation))},
            // ParticleEmitter2
            {"KP2S", new AnimationMapItem("Speed", typeof( FloatAnimation))},
            {"KP2R", new AnimationMapItem("Variation", typeof( FloatAnimation))},
            {"KP2L", new AnimationMapItem("Latitude", typeof( FloatAnimation))},
            {"KP2G", new AnimationMapItem("Gravity", typeof( FloatAnimation))},
            {"KP2E", new AnimationMapItem("EmissionRate", typeof( FloatAnimation))},
            {"KP2N", new AnimationMapItem("Width", typeof( FloatAnimation))},
            {"KP2W", new AnimationMapItem("Length", typeof( FloatAnimation))},
            {"KP2V", new AnimationMapItem("Visibility", typeof( FloatAnimation))},
            // ParticleEmitterCorn
            {"KPPA", new AnimationMapItem("Alpha", typeof( FloatAnimation))},
            {"KPPC", new AnimationMapItem("Color", typeof( Vector3Animation))},
            {"KPPE", new AnimationMapItem("EmissionRate", typeof( FloatAnimation))},
            {"KPPL", new AnimationMapItem("LifeSpan", typeof( FloatAnimation))},
            {"KPPS", new AnimationMapItem("Speed", typeof( FloatAnimation))},
            {"KPPV", new AnimationMapItem("Visibility", typeof( FloatAnimation))},
            // RibbonEmitter
            {"KRHA", new AnimationMapItem("HeightAbove", typeof( FloatAnimation))},
            {"KRHB", new AnimationMapItem("HeightBelow", typeof( FloatAnimation))},
            {"KRAL", new AnimationMapItem("Alpha", typeof( FloatAnimation))},
            {"KRCO", new AnimationMapItem("Color", typeof( Vector3Animation))},
            {"KRTX", new AnimationMapItem("TextureSlot", typeof( UintAnimation))},
            {"KRVS", new AnimationMapItem("Visibility", typeof( FloatAnimation))},
            // Camera
            {"KCTR", new AnimationMapItem("Translation", typeof( Vector3Animation))},
            {"KTTR", new AnimationMapItem("Translation", typeof( Vector3Animation))},
            {"KCRL", new AnimationMapItem("Rotation", typeof( FloatAnimation))},
        };
    }
}
