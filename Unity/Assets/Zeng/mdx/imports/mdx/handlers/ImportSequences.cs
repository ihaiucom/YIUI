
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using Zeng.mdx.parsers.mdx;
using CAnimation = Zeng.mdx.parsers.mdx.Animation;
using MdxParticleSystem2 = Zeng.Mdx.Runtimes.MdxParticleSystem2;
using ParticleEmitter2 = Zeng.mdx.parsers.mdx.ParticleEmitter2;

namespace Zeng.mdx.imports.mdx
{
    public class ImportSequences
    {
        public ImportModel importModel;
        public Model cmodel;
        private SortedDictionary<int, GameObject> bones;
        private ImportSetting importSetting;

        public List<AnimationClip> clips = new List<AnimationClip>();

        public ImportSequences(ImportModel importModel, Model cmodel)
        {
            this.importModel = importModel;
            this.cmodel = cmodel;

            bones = importModel.importSkeletons.bones;
            importSetting = importModel.importSetting;

        }


        public void ToUnity()
        {
            float frameRate = importSetting.frameRate;
            for (int i = 0; i < cmodel.sequences.Count; i ++)
            {
                Sequence csequence = cmodel.sequences[i];
                float ctimeBegin = csequence.interval[0];
                float ctimeEnd = csequence.interval[1];

                AnimationClip clip = new AnimationClip();
                clip.name = csequence.name;

                // Set the loop mode.
                if (csequence.nonLooping != 0)
                {
                    clip.wrapMode = WrapMode.Once;
                }
                clip.wrapMode = WrapMode.Loop;


                // For each bone.
                for (int j = 0; j < cmodel.bones.Count; j++)
                {

                    Bone cbone = cmodel.bones[j];
                    GameObject bone = bones[cbone.objectId];

                    SetBone(clip, bone, cbone, frameRate, ctimeBegin, ctimeEnd, cbone.geosetAnimationId);

                }

                // For each helper.
                for (int j = 0; j < cmodel.helpers.Count; j++)
                {
                    GenericObject cbone = cmodel.helpers[j];
                    GameObject bone = bones[cbone.objectId];

                    SetBone(clip, bone, cbone, frameRate, ctimeBegin, ctimeEnd);
                }

                // For each Attachments
                for (int j = 0; j < cmodel.attachments.Count; j++)
                {
                    Attachment cbone = cmodel.attachments[j];
                    GameObject bone = importModel.importAttachments.nodes[cbone.objectId];

                    SetBone(clip, bone, cbone, frameRate, ctimeBegin, ctimeEnd, -1, cbone.Visibility);
                }

                // For each Events
                for (int j = 0; j < cmodel.eventObjects.Count; j++)
                {
                    GenericObject cbone = cmodel.eventObjects[j];
                    GameObject bone = importModel.importEvents.nodes[cbone.objectId];

                    SetBone(clip, bone, cbone, frameRate, ctimeBegin, ctimeEnd);
                }

                // For each Geoset

                for (int j = 0; j < importModel.meshRenders.Count; j++)
                {
                    ImportMeshRender importMeshRender = importModel.meshRenders[j];
                    string path = importMeshRender.gameObject.name;

                    // GeosetAnimation
                    GeosetAnimation cgeosetAnimation = importMeshRender.importGeoset.cgeosetAnimation;
                    if (cgeosetAnimation != null) {


                        AnimationCurve curveColorR = new AnimationCurve();
                        AnimationCurve curveColorG = new AnimationCurve();
                        AnimationCurve curveColorB = new AnimationCurve();
                        AnimationCurve curveColorA = new AnimationCurve();

                        // Alpha
                        {
                            CAnimation canimation = cgeosetAnimation.Alpha;
                            if (canimation != null) {
                                for (int k = 0; k < canimation.frames.Count; k++)
                                {
                                    float ctime = canimation.frames[k];
                                    float[] frameValue = canimation.values[k];

                                    float InTangent = 0;
                                    float OutTangent = 0;
                                    if (canimation.interpolationType > InterpolationType.Linear)
                                    {
                                        InTangent = canimation.inTans[k][0];
                                        OutTangent = canimation.inTans[k][0];
                                    }

                                    if (ctimeBegin <= ctime && ctime <= ctimeEnd)
                                    {
                                        float time = ctime - ctimeBegin;
                                        float val = frameValue[0];

                                        Keyframe key = new Keyframe(time / frameRate, val);
                                        key.inTangent = InTangent;
                                        key.outTangent = OutTangent;
                                        curveColorA.AddKey(key);
                                    }
                                }

                                if (curveColorA.length == 0)
                                {
                                    Keyframe key = new Keyframe(0, cgeosetAnimation.alpha);
                                    curveColorA.AddKey(key);
                                }




                                if (curveColorA.length > 0)
                                {
                                    clip.SetCurve(path, typeof(SkinnedMeshRenderer), "material._geosetColor.a", curveColorA);
                                }
                            }
                           
                        }

                        // Color
                        {
                            CAnimation canimation = cgeosetAnimation.Color;
                            if (canimation != null) {

                                for (int k = 0; k < canimation.frames.Count; k++)
                                {
                                    float ctime = canimation.frames[k];
                                    float[] frameValue = canimation.values[k];

                                    Vector3 InTangent = new Vector3();
                                    Vector3 OutTangent = new Vector3();
                                    if (canimation.interpolationType > InterpolationType.Linear)
                                    {
                                        float[] inTans = canimation.inTans[k];
                                        float[] outTans = canimation.inTans[k];

                                        InTangent = new Vector3(inTans[0], inTans[1], inTans[2]);
                                        OutTangent = new Vector3(outTans[0], outTans[1], outTans[2]);
                                        InTangent = InTangent.VecToU3d();
                                        OutTangent = OutTangent.VecToU3d();
                                        InTangent.x = Mathf.Abs(InTangent.x);
                                        InTangent.y = Mathf.Abs(InTangent.y);
                                        InTangent.z = Mathf.Abs(InTangent.z);

                                        OutTangent.x = Mathf.Abs(OutTangent.x);
                                        OutTangent.y = Mathf.Abs(OutTangent.y);
                                        OutTangent.z = Mathf.Abs(OutTangent.z);
                                    }

                                    if (ctimeBegin <= ctime && ctime <= ctimeEnd)
                                    {
                                        float time = ctime - ctimeBegin;
                                        Vector3 val = new Vector3(frameValue[0], frameValue[1], frameValue[2]);
                                        val = val.VecToU3d();
                                        val.x = Mathf.Abs(val.x);
                                        val.y = Mathf.Abs(val.y);
                                        val.z = Mathf.Abs(val.z);

                                        Keyframe keyX = new Keyframe(time / frameRate, val.x);
                                        keyX.inTangent = InTangent.x;
                                        keyX.outTangent = OutTangent.x;
                                        curveColorR.AddKey(keyX);

                                        Keyframe keyY = new Keyframe(time / frameRate, val.y);
                                        keyY.inTangent = InTangent.y;
                                        keyY.outTangent = OutTangent.y;
                                        curveColorG.AddKey(keyY);

                                        Keyframe keyZ = new Keyframe(time / frameRate, val.z);
                                        keyZ.inTangent = InTangent.z;
                                        keyZ.outTangent = OutTangent.z;
                                        curveColorB.AddKey(keyZ);
                                    }

                                }



                                if (curveColorR.length == 0)
                                {
                                    Keyframe key = new Keyframe(0, cgeosetAnimation.color[0]);
                                    curveColorR.AddKey(key);

                                    key = new Keyframe(0, cgeosetAnimation.color[1]);
                                    curveColorG.AddKey(key);

                                    key = new Keyframe(0, cgeosetAnimation.color[2]);
                                    curveColorB.AddKey(key);
                                }



                                if (curveColorR.length > 0)
                                {
                                    clip.SetCurve(path, typeof(SkinnedMeshRenderer), "material._geosetColor.r", curveColorR);
                                }

                                if (curveColorG.length > 0)
                                {
                                    clip.SetCurve(path, typeof(SkinnedMeshRenderer), "material._geosetColor.g", curveColorG);
                                }

                                if (curveColorB.length > 0)
                                {
                                    clip.SetCurve(path, typeof(SkinnedMeshRenderer), "material._geosetColor.b", curveColorB);
                                }

                            }

                            if (cgeosetAnimation.Alpha != null && cgeosetAnimation.Color == null) {

                                Keyframe key = new Keyframe(0, cgeosetAnimation.color[0]);
                                curveColorR.AddKey(key);

                                key = new Keyframe(0, cgeosetAnimation.color[1]);
                                curveColorG.AddKey(key);

                                key = new Keyframe(0, cgeosetAnimation.color[2]);
                                curveColorB.AddKey(key);



                                clip.SetCurve(path, typeof(SkinnedMeshRenderer), "material._geosetColor.r", curveColorR);
                                clip.SetCurve(path, typeof(SkinnedMeshRenderer), "material._geosetColor.g", curveColorG);
                                clip.SetCurve(path, typeof(SkinnedMeshRenderer), "material._geosetColor.b", curveColorB);
                            }

                        }


                    }


                    // Layer 
                    {
                        Layer clayer = importMeshRender.importMaterail.clayer;
                        CAnimation canimation = clayer.AnimAlpha;
                        if (canimation != null) {

                            AnimationCurve curveLayerAlpha = new AnimationCurve();

                            for (int k = 0; k < canimation.frames.Count; k++)
                            {
                                float ctime = canimation.frames[k];
                                float[] frameValue = canimation.values[k];

                                float InTangent = 0;
                                float OutTangent = 0;
                                if (canimation.interpolationType > InterpolationType.Linear)
                                {
                                    InTangent = canimation.inTans[k][0];
                                    OutTangent = canimation.inTans[k][0];
                                }

                                if (ctimeBegin <= ctime && ctime <= ctimeEnd)
                                {
                                    float time = ctime - ctimeBegin;
                                    float val = frameValue[0];

                                    Keyframe key = new Keyframe(time / frameRate, val);
                                    key.inTangent = InTangent;
                                    key.outTangent = OutTangent;
                                    curveLayerAlpha.AddKey(key);
                                }
                            }

                            if (curveLayerAlpha.length == 0)
                            {
                                Keyframe key = new Keyframe(0, clayer.alpha);
                                curveLayerAlpha.AddKey(key);
                            }



                            if (curveLayerAlpha.length > 0)
                            {
                                clip.SetCurve(path, typeof(SkinnedMeshRenderer), "material._layerAlpha", curveLayerAlpha);
                            }

                        }


                    }
                }


                // For each ParticleEmitters2
                for(int j = 0; j < cmodel.particleEmitters2.Count; j ++)
                {
                    ParticleEmitter2 cparticleEmitter2 = cmodel.particleEmitters2[j];
                    GameObject bone = importModel.importParticleEmitter2s.nodes[cparticleEmitter2.objectId];

                    SetBone(clip, bone, cparticleEmitter2, frameRate, ctimeBegin, ctimeEnd, -1, cparticleEmitter2.AnimVisibility);
                    SetParticleEmitters2(clip, bone, cparticleEmitter2, frameRate, ctimeBegin, ctimeEnd);
                }


                // Realigns quaternion keys to ensure shortest interpolation paths and avoid rotation glitches.
                clip.EnsureQuaternionContinuity();

                if (clip.empty) {
                    Debug.Log($"没有动画曲线 clip.name={clip.name}, {this.importModel.name}" );
                }

                clips.Add(clip);

            }
        }

        private void SetParticleEmitters2(AnimationClip clip, GameObject bone, ParticleEmitter2 cparticleEmitter2, float frameRate, float ctimeBegin, float ctimeEnd, int geosetAnimationId = -1)
        {
            string path = GetPath(bone);

            MdxParticleSystem2 p = bone.GetComponent<MdxParticleSystem2>();
            CAnimation canimation;

            // Speed
            canimation = cparticleEmitter2.AnimSpeed;
            SetCurveFloat(clip, frameRate, ctimeBegin, ctimeEnd, canimation, cparticleEmitter2.speed, path, typeof(MdxParticleSystem2), "speed");


            // Variation
            canimation = cparticleEmitter2.AnimVariation;
            SetCurveFloat(clip, frameRate, ctimeBegin, ctimeEnd, canimation, cparticleEmitter2.variation, path, typeof(MdxParticleSystem2), "variation");


            // Latitude
            canimation = cparticleEmitter2.AnimLatitude;
            SetCurveFloat(clip, frameRate, ctimeBegin, ctimeEnd, canimation, cparticleEmitter2.latitude, path, typeof(MdxParticleSystem2), "latitude");

            // Gravity
            canimation = cparticleEmitter2.AnimGravity;
            SetCurveFloat(clip, frameRate, ctimeBegin, ctimeEnd, canimation, cparticleEmitter2.gravity, path, typeof(MdxParticleSystem2), "gravity");

            // EmissionRate
            canimation = cparticleEmitter2.AnimEmissionRate;
            SetCurveFloat(clip, frameRate, ctimeBegin, ctimeEnd, canimation, cparticleEmitter2.emissionRate, path, typeof(MdxParticleSystem2), "emissionRate");
            if (cparticleEmitter2.squirt != 0 || true) {
                SetCurveSwitch(clip, frameRate, ctimeBegin, ctimeEnd, canimation, cparticleEmitter2.emissionRate > 0 ? 1 : -1, path, typeof(MdxParticleSystem2), "emissionRateKey", 0, 1, -1);
            }

            // Width
            canimation = cparticleEmitter2.AnimWidth;
            SetCurveFloat(clip, frameRate, ctimeBegin, ctimeEnd, canimation, cparticleEmitter2.width, path, typeof(MdxParticleSystem2), "width");

            // Length
            canimation = cparticleEmitter2.AnimLength;
            SetCurveFloat(clip, frameRate, ctimeBegin, ctimeEnd, canimation, cparticleEmitter2.length, path, typeof(MdxParticleSystem2), "length");

            // Visibility
            //canimation = cparticleEmitter2.AnimVisibility;
            //SetCurveFloat(clip, frameRate, ctimeBegin, ctimeEnd, canimation, 1.0f, path, typeof(MdxParticleSystem2), "visibility");


        }

        private void SetCurveSwitch(AnimationClip clip, float frameRate, float ctimeBegin, float ctimeEnd, CAnimation canimation, float defaultVal, string path, Type curveType, string propertyName, float valT, float valA, float valB)
        {

            if (canimation == null)
            {
                return;
            }

            AnimationCurve curve = new AnimationCurve();
            for (int k = 0; k < canimation.frames.Count; k++)
            {
                float ctime = canimation.frames[k];
                float[] frameValue = canimation.values[k];

                float InTangent = 0;
                float OutTangent = 0;
                if (canimation.interpolationType > InterpolationType.Linear)
                {
                    InTangent = canimation.inTans[k][0];
                    OutTangent = canimation.inTans[k][0];
                }

                if (ctimeBegin <= ctime && ctime <= ctimeEnd)
                {
                    float time = ctime - ctimeBegin;
                    float val = frameValue[0];

                    Keyframe key = new Keyframe(time / frameRate, val > valT ? valA: valB);
                    curve.AddKey(key);
                }
            }

            if (curve.length == 0)
            {
                Keyframe key = new Keyframe(0, defaultVal);
                curve.AddKey(key);
            }

            if (curve.length > 0)
            {
                clip.SetCurve(path, curveType, propertyName, curve);
            }
        }


        private void SetCurveFloat(AnimationClip clip, float frameRate, float ctimeBegin, float ctimeEnd, CAnimation canimation, float defaultVal, string path, Type curveType, string propertyName)
        {

            if (canimation == null)
            {
                return;
            }

            AnimationCurve curve = new AnimationCurve();
            for (int k = 0; k < canimation.frames.Count; k++)
            {
                float ctime = canimation.frames[k];
                float[] frameValue = canimation.values[k];

                float InTangent = 0;
                float OutTangent = 0;
                if (canimation.interpolationType > InterpolationType.Linear)
                {
                    InTangent = canimation.inTans[k][0];
                    OutTangent = canimation.inTans[k][0];
                }

                if (ctimeBegin <= ctime && ctime <= ctimeEnd)
                {
                    float time = ctime - ctimeBegin;
                    float val = frameValue[0];

                    Keyframe key = new Keyframe(time / frameRate, val);
                    key.inTangent = InTangent;
                    key.outTangent = OutTangent;
                    curve.AddKey(key);
                }
            }

            if (curve.length == 0)
            {
                Keyframe key = new Keyframe(0, defaultVal);
                curve.AddKey(key);
            }

            if (curve.length > 0)
            {
                clip.SetCurve(path, curveType, propertyName, curve);
            }
        }



        private void SetBone(AnimationClip clip, GameObject bone, GenericObject cbone, float frameRate, float ctimeBegin, float ctimeEnd, int geosetAnimationId = -1, CAnimation visiableAnimation = null)
        {

            string path = GetPath(bone);

            // Translation.
            {
                AnimationCurve curveX = new AnimationCurve();
                AnimationCurve curveY = new AnimationCurve();
                AnimationCurve curveZ = new AnimationCurve();

                CAnimation canimation = cbone.Translation;
                if (canimation != null) { 
                    for (int k = 0; k < canimation.frames.Count; k++)
                    {
                        float ctime = canimation.frames[k];
                        float[] frameValue = canimation.values[k];

                        Vector3 InTangent = new Vector3();
                        Vector3 OutTangent = new Vector3();
                        if (canimation.interpolationType > InterpolationType.Linear)
                        {
                            float[] inTans = canimation.inTans[k];
                            float[] outTans = canimation.inTans[k];

                            InTangent = new Vector3(inTans[0], inTans[1], inTans[2]);
                            OutTangent = new Vector3(outTans[0], outTans[1], outTans[2]);
                            InTangent = InTangent.VecToU3d();
                            OutTangent = OutTangent.VecToU3d();
                        }

                        if (ctimeBegin <= ctime && ctime <= ctimeEnd)
                        {
                            float time = ctime - ctimeBegin;
                            Vector3 val = new Vector3(frameValue[0], frameValue[1], frameValue[2]);
                            val = val.VecToU3d();
                            val = bone.transform.localPosition + val;

                            Keyframe keyX = new Keyframe(time / frameRate, val.x);
                            keyX.inTangent = InTangent.x;
                            keyX.outTangent = OutTangent.x;
                            curveX.AddKey(keyX);

                            Keyframe keyY = new Keyframe(time / frameRate, val.y);
                            keyY.inTangent = InTangent.y;
                            keyY.outTangent = OutTangent.y;
                            curveY.AddKey(keyY);

                            Keyframe keyZ = new Keyframe(time / frameRate, val.z);
                            keyZ.inTangent = InTangent.z;
                            keyZ.outTangent = OutTangent.z;
                            curveZ.AddKey(keyZ);
                        }
                    }


                    if (MdxUnityUtils.IsForceKeyFrame(bone.name))
                    {

                        if (curveX.length == 0)
                        {
                            Keyframe key = new Keyframe(0, bone.transform.localPosition.x);
                            curveX.AddKey(key);
                        }

                        if (curveY.length == 0)
                        {
                            Keyframe key = new Keyframe(0, bone.transform.localPosition.y);
                            curveY.AddKey(key);
                        }

                        if (curveZ.length == 0)
                        {
                            Keyframe key = new Keyframe(0, bone.transform.localPosition.z);
                            curveZ.AddKey(key);
                        }
                    }


                    if (curveX.length > 0)
                    {
                        clip.SetCurve(path, typeof(Transform), "localPosition.x", curveX);
                    }
                    if (curveY.length > 0)
                    {
                        clip.SetCurve(path, typeof(Transform), "localPosition.y", curveY);
                    }
                    if (curveZ.length > 0)
                    {
                        clip.SetCurve(path, typeof(Transform), "localPosition.z", curveZ);
                    }


                }

            }



            // Rotation.
            {
                AnimationCurve curveX = new AnimationCurve();
                AnimationCurve curveY = new AnimationCurve();
                AnimationCurve curveZ = new AnimationCurve();
                AnimationCurve curveW = new AnimationCurve();

                CAnimation canimation = cbone.Rotation;
                if (canimation != null) { 
                    for (int k = 0; k < canimation.frames.Count; k++)
                    {
                        float ctime = canimation.frames[k];
                        float[] frameValue = canimation.values[k];

                        Quaternion InTangent = new Quaternion();
                        Quaternion OutTangent = new Quaternion();
                        if (canimation.interpolationType > InterpolationType.Linear)
                        {
                            float[] inTans = canimation.inTans[k];
                            float[] outTans = canimation.inTans[k];

                            InTangent = new Quaternion(inTans[0], inTans[1], inTans[2], inTans[3]);
                            OutTangent = new Quaternion(outTans[0], outTans[1], outTans[2], outTans[3]);
                            InTangent = InTangent.QuaternionToU3d();
                            OutTangent = OutTangent.QuaternionToU3d();
                        }

                        if (ctimeBegin <= ctime && ctime <= ctimeEnd)
                        {
                            float time = ctime - ctimeBegin;
                            Quaternion val = new Quaternion(frameValue[0], frameValue[1], frameValue[2], frameValue[3]);
                            val = val.QuaternionToU3d();

                            Keyframe keyX = new Keyframe(time / frameRate, val.x);
                            keyX.inTangent = InTangent.x;
                            keyX.outTangent = OutTangent.x;
                            curveX.AddKey(keyX);

                            Keyframe keyY = new Keyframe(time / frameRate, val.y);
                            keyY.inTangent = InTangent.y;
                            keyY.outTangent = OutTangent.y;
                            curveY.AddKey(keyY);

                            Keyframe keyZ = new Keyframe(time / frameRate, val.z);
                            keyZ.inTangent = InTangent.z;
                            keyZ.outTangent = OutTangent.z;
                            curveZ.AddKey(keyZ);

                            Keyframe keyW = new Keyframe(time / frameRate, val.w);
                            keyW.inTangent = InTangent.w;
                            keyW.outTangent = OutTangent.w;
                            curveW.AddKey(keyW);
                        }
                    }


                    if (MdxUnityUtils.IsForceKeyFrame(bone.name))
                    {
                        if (curveX.length == 0)
                        {
                            Keyframe key = new Keyframe(0, bone.transform.localRotation.x);
                            curveX.AddKey(key);
                        }

                        if (curveY.length == 0)
                        {
                            Keyframe key = new Keyframe(0, bone.transform.localRotation.y);
                            curveY.AddKey(key);
                        }

                        if (curveZ.length == 0)
                        {
                            Keyframe key = new Keyframe(0, bone.transform.localRotation.z);
                            curveZ.AddKey(key);
                        }

                        if (curveW.length == 0)
                        {
                            Keyframe key = new Keyframe(0, bone.transform.localRotation.w);
                            curveW.AddKey(key);
                        }
                    }


                    if (curveX.length > 0)
                    {
                        clip.SetCurve(path, typeof(Transform), "localRotation.x", curveX);
                    }
                    if (curveY.length > 0)
                    {
                        clip.SetCurve(path, typeof(Transform), "localRotation.y", curveY);
                    }
                    if (curveZ.length > 0)
                    {
                        clip.SetCurve(path, typeof(Transform), "localRotation.z", curveZ);
                    }
                    if (curveW.length > 0)
                    {
                        clip.SetCurve(path, typeof(Transform), "localRotation.w", curveW);
                    }
                }
            }



            // Scaling.
            {
                AnimationCurve curveX = new AnimationCurve();
                AnimationCurve curveY = new AnimationCurve();
                AnimationCurve curveZ = new AnimationCurve();

                CAnimation canimation = cbone.Scaling;
                if (canimation != null) {
                    for (int k = 0; k < canimation.frames.Count; k++)
                    {
                        float ctime = canimation.frames[k];
                        float[] frameValue = canimation.values[k];

                        Vector3 InTangent = new Vector3();
                        Vector3 OutTangent = new Vector3();
                        if (canimation.interpolationType > InterpolationType.Linear)
                        {
                            float[] inTans = canimation.inTans[k];
                            float[] outTans = canimation.inTans[k];

                            InTangent = new Vector3(inTans[0], inTans[1], inTans[2]);
                            OutTangent = new Vector3(outTans[0], outTans[1], outTans[2]);
                            InTangent = InTangent.VecToU3d();
                            OutTangent = OutTangent.VecToU3d();
                        }

                        if (ctimeBegin <= ctime && ctime <= ctimeEnd)
                        {
                            float time = ctime - ctimeBegin;
                            Vector3 val = new Vector3(frameValue[0], frameValue[1], frameValue[2]);
                            val = val.VecToU3d();

                            Keyframe keyX = new Keyframe(time / frameRate, val.x);
                            keyX.inTangent = InTangent.x;
                            keyX.outTangent = OutTangent.x;
                            curveX.AddKey(keyX);

                            Keyframe keyY = new Keyframe(time / frameRate, val.y);
                            keyY.inTangent = InTangent.y;
                            keyY.outTangent = OutTangent.y;
                            curveY.AddKey(keyY);

                            Keyframe keyZ = new Keyframe(time / frameRate, val.z);
                            keyZ.inTangent = InTangent.z;
                            keyZ.outTangent = OutTangent.z;
                            curveZ.AddKey(keyZ);
                        }
                    }


                    if (MdxUnityUtils.IsForceKeyFrame(bone.name))
                    {

                        if (curveX.length == 0)
                        {

                            Keyframe key = new Keyframe(0, bone.transform.lossyScale.x);
                            curveX.AddKey(key);
                        }

                        if (curveY.length == 0)
                        {
                            Keyframe key = new Keyframe(0, bone.transform.lossyScale.y);
                            curveY.AddKey(key);
                        }

                        if (curveZ.length == 0)
                        {
                            Keyframe key = new Keyframe(0, bone.transform.lossyScale.z);
                            curveZ.AddKey(key);
                        }
                    }


                    if (curveX.length > 0)
                    {
                        clip.SetCurve(path, typeof(Transform), "localScale.x", curveX);
                    }
                    if (curveY.length > 0)
                    {
                        clip.SetCurve(path, typeof(Transform), "localScale.y", curveY);
                    }
                    if (curveZ.length > 0)
                    {
                        clip.SetCurve(path, typeof(Transform), "localScale.z", curveZ);
                    }
                }
               
            }


            // Visibility
            if (geosetAnimationId < cmodel.geosetAnimations.Count && geosetAnimationId >= 0)
            {
                GeosetAnimation cgeosetAnimation = cmodel.geosetAnimations[geosetAnimationId];

                AnimationCurve curveVisibility = new AnimationCurve();


                CAnimation canimation = cgeosetAnimation.Alpha;
                if (canimation != null) {

                    for (int k = 0; k < canimation.frames.Count; k++)
                    {
                        float ctime = canimation.frames[k];
                        float[] frameValue = canimation.values[k];

                        float InTangent = 0;
                        float OutTangent = 0;
                        if (canimation.interpolationType > InterpolationType.Linear)
                        {
                            InTangent = canimation.inTans[k][0];
                            OutTangent = canimation.inTans[k][0];
                        }

                        if (ctimeBegin <= ctime && ctime <= ctimeEnd)
                        {
                            float time = ctime - ctimeBegin;
                            float val = frameValue[0];

                            Keyframe keyVisibility = new Keyframe(time / frameRate, val);
                            keyVisibility.inTangent = InTangent;
                            keyVisibility.outTangent = OutTangent;
                            curveVisibility.AddKey(keyVisibility);
                        }
                    }

                    if (curveVisibility.length == 0)
                    {
                        Keyframe key = new Keyframe(0, cgeosetAnimation.alpha);
                        curveVisibility.AddKey(key);
                    }

                    if (curveVisibility.length > 0)
                    {
                        clip.SetCurve(path, typeof(GameObject), "m_IsActive", curveVisibility);
                    }
                }

            }


            // visiableAnimation
            if (visiableAnimation != null) {

                AnimationCurve curveVisibility = new AnimationCurve();
                CAnimation canimation = visiableAnimation;
                if (canimation != null)
                {

                    for (int k = 0; k < canimation.frames.Count; k++)
                    {
                        float ctime = canimation.frames[k];
                        float[] frameValue = canimation.values[k];

                        float InTangent = 0;
                        float OutTangent = 0;
                        if (canimation.interpolationType > InterpolationType.Linear)
                        {
                            InTangent = canimation.inTans[k][0];
                            OutTangent = canimation.inTans[k][0];
                        }

                        if (ctimeBegin <= ctime && ctime <= ctimeEnd)
                        {
                            float time = ctime - ctimeBegin;
                            float val = frameValue[0];

                            Keyframe keyVisibility = new Keyframe(time / frameRate, val);
                            keyVisibility.inTangent = InTangent;
                            keyVisibility.outTangent = OutTangent;
                            curveVisibility.AddKey(keyVisibility);
                        }
                    }

                    if (curveVisibility.length == 0)
                    {
                        Keyframe key = new Keyframe(0, 1);
                        curveVisibility.AddKey(key);
                    }

                    if (curveVisibility.length > 0)
                    {
                        clip.SetCurve(path, typeof(GameObject), "m_IsActive", curveVisibility);
                    }
                }
            }
        }



        private string GetPath(GameObject bone)
        {
            return importModel.GetPath(bone);
        }


        public void Save()
        {
            string saveDirPath = importModel.saveDirPath;
            string directory = saveDirPath + "/Animations/";

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            foreach (AnimationClip clip in clips)
            {
                AssetDatabase.CreateAsset(clip, directory + clip.name + ".anim");
                AssetDatabase.SaveAssets();
            }

            string animationControllerPath = saveDirPath + "/" + importModel.name + ".controller";
            AnimatorController animatorController = AnimatorControllerUtils.Create(animationControllerPath, directory);

            Animator animator = importModel.gameObject.AddComponent<Animator>();
            animator.runtimeAnimatorController = animatorController;


        }
    }
}
