using MdxLib.Animator;
using MdxLib.Model;
using MdxLib.Primitives;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEditor.Animations;
using Zeng.Mdx.Runtimes;

namespace Zeng.MdxImport.mdx.handlers
{
    public class SequenceHandler
    {
        public ModelHandler modelHandler;
        private CModel cmodel;
        private MdxImportSettings settings;
        private SortedDictionary<int, GameObject> bones = new SortedDictionary<int, GameObject>();

        public List<AnimationClip> clips { get; private set; }

        public SequenceHandler(ModelHandler modelHandler)
        {
            this.modelHandler = modelHandler;
            cmodel = modelHandler.cmodel;
            settings = modelHandler.settings;
            bones = modelHandler.skeletonHandler.bones;

            clips = new List<AnimationClip>();
        }


        public void SaveUnityFile(string saveDirPath, string fileName, GameObject gameObject)
        {
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

            string animationControllerPath = saveDirPath + fileName + ".controller";
            AnimatorController animatorController = AnimatorControllerUtils.Create(animationControllerPath, directory);

            Animator animator = gameObject.AddComponent<Animator>();
            animator.runtimeAnimatorController = animatorController;
        }


        private string GetPath(GameObject bone)
        {
            return modelHandler.GetPath(bone);
        }

        public void ImportAnimations()
        {
            // For each sequence.
            for (int i = 0; i < cmodel.Sequences.Count; i++)
            {
                CSequence csequence = cmodel.Sequences.Get(i);
                if (settings.excludeAnimations.Contains(csequence.Name))
                {
                    continue;
                }

                AnimationClip clip = new AnimationClip();
                clip.name = csequence.Name;

                // Set the loop mode.
                if (!csequence.NonLooping)
                {
                    clip.wrapMode = WrapMode.Loop;
                }
                clip.wrapMode = WrapMode.Loop;

                // For each bone.
                for (int j = 0; j < cmodel.Bones.Count; j++)
                {
                    CBone cbone = cmodel.Bones.Get(j);
                    GameObject bone = bones[cbone.NodeId];
                    string path = GetPath(bone);

                    // Translation.
                    {
                        AnimationCurve curveX = new AnimationCurve();
                        AnimationCurve curveY = new AnimationCurve();
                        AnimationCurve curveZ = new AnimationCurve();

                        CAnimator<CVector3> ctranslations = cbone.Translation;
                        for (int k = 0; k < ctranslations.Count; k++)
                        {
                            CAnimatorNode<CVector3> node = ctranslations.Get(k);
                            if (csequence.IntervalStart <= node.Time && node.Time <= csequence.IntervalEnd)
                            {
                                float time = node.Time - csequence.IntervalStart;
                                Vector3 position = bone.transform.localPosition + node.Value.ToVector3().SwapYZX();

                                Keyframe keyX = new Keyframe(time / settings.frameRate, position.x);
                                if (settings.importTangents)
                                {
                                    keyX.inTangent = node.InTangent.Y;
                                    keyX.outTangent = node.OutTangent.Y;
                                }
                                curveX.AddKey(keyX);

                                Keyframe keyY = new Keyframe(time / settings.frameRate, position.y);
                                if (settings.importTangents)
                                {
                                    keyY.inTangent = node.InTangent.Z;
                                    keyY.outTangent = node.OutTangent.Z;
                                }
                                curveY.AddKey(keyY);

                                Keyframe keyZ = new Keyframe(time / settings.frameRate, position.z);
                                if (settings.importTangents)
                                {
                                    keyZ.inTangent = node.InTangent.X;
                                    keyZ.outTangent = node.OutTangent.X;
                                }
                                curveZ.AddKey(keyZ);
                            }
                        }

                        if (!bone.name.Contains("Plane") && !bone.name.Contains("Glow"))
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

                    // Rotation.
                    {
                        AnimationCurve curveX = new AnimationCurve();
                        AnimationCurve curveY = new AnimationCurve();
                        AnimationCurve curveZ = new AnimationCurve();
                        AnimationCurve curveW = new AnimationCurve();

                        CAnimator<CVector4> crotations = cbone.Rotation;
                        for (int k = 0; k < crotations.Count; k++)
                        {
                            CAnimatorNode<CVector4> node = crotations.Get(k);
                            if (csequence.IntervalStart <= node.Time && node.Time <= csequence.IntervalEnd)
                            {
                                float time = node.Time - csequence.IntervalStart;
                                Quaternion rotation = node.Value.ToQuaternion();

                                Keyframe keyX = new Keyframe(time / settings.frameRate, -rotation.y);
                                if (settings.importTangents)
                                {
                                    keyX.inTangent = node.InTangent.Y;
                                    keyX.outTangent = node.OutTangent.Y;
                                }
                                curveX.AddKey(keyX);

                                Keyframe keyY = new Keyframe(time / settings.frameRate, rotation.z);
                                if (settings.importTangents)
                                {
                                    keyY.inTangent = node.InTangent.Z;
                                    keyY.outTangent = node.OutTangent.Z;
                                }
                                curveY.AddKey(keyY);

                                Keyframe keyZ = new Keyframe(time / settings.frameRate, rotation.x);
                                if (settings.importTangents)
                                {
                                    keyZ.inTangent = node.InTangent.X;
                                    keyZ.outTangent = node.OutTangent.X;
                                }
                                curveZ.AddKey(keyZ);

                                Keyframe keyW = new Keyframe(time / settings.frameRate, -rotation.w);
                                if (settings.importTangents)
                                {
                                    keyW.inTangent = node.InTangent.W;
                                    keyW.outTangent = node.OutTangent.W;
                                }
                                curveW.AddKey(keyW);
                            }
                        }


                        if (MdxUtils.IsForceKeyFrame(bone.name))
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

                    // Scaling.
                    {
                        AnimationCurve curveX = new AnimationCurve();
                        AnimationCurve curveY = new AnimationCurve();
                        AnimationCurve curveZ = new AnimationCurve();


                        CAnimator<CVector3> cscalings = cbone.Scaling;

                        if (cbone.Name == "Plane0101")
                        {
                            Debug.Log("Plane0101 cscalings.Count= " + cscalings.Count);
                        }
                        for (int k = 0; k < cscalings.Count; k++)
                        {
                            CAnimatorNode<CVector3> node = cscalings.Get(k);
                            if (csequence.IntervalStart <= node.Time && node.Time <= csequence.IntervalEnd)
                            {
                                float time = node.Time - csequence.IntervalStart;

                                Keyframe keyX = new Keyframe(time / settings.frameRate, -node.Value.Y);
                                curveX.AddKey(keyX);

                                Keyframe keyY = new Keyframe(time / settings.frameRate, node.Value.Z);
                                curveY.AddKey(keyY);

                                Keyframe keyZ = new Keyframe(time / settings.frameRate, node.Value.X);
                                curveZ.AddKey(keyZ);
                            }
                        }


                        if (MdxUtils.IsForceKeyFrame(bone.name))
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

                // For each helper.
                for (int j = 0; j < cmodel.Helpers.Count; j++)
                {
                    CHelper chelper = cmodel.Helpers.Get(j);
                    GameObject bone = bones[chelper.NodeId];
                    string path = GetPath(bone);

                    // Translation.
                    {
                        AnimationCurve curveX = new AnimationCurve();
                        AnimationCurve curveY = new AnimationCurve();
                        AnimationCurve curveZ = new AnimationCurve();

                        CAnimator<CVector3> ctranslations = chelper.Translation;
                        for (int k = 0; k < ctranslations.Count; k++)
                        {
                            CAnimatorNode<CVector3> node = ctranslations.Get(k);
                            if (csequence.IntervalStart <= node.Time && node.Time <= csequence.IntervalEnd)
                            {
                                float time = node.Time - csequence.IntervalStart;
                                Vector3 position = bone.transform.localPosition + node.Value.ToVector3().SwapYZX();

                                Keyframe keyX = new Keyframe(time / settings.frameRate, position.x);
                                if (settings.importTangents)
                                {
                                    keyX.inTangent = node.InTangent.Y;
                                    keyX.outTangent = node.OutTangent.Y;
                                }
                                curveX.AddKey(keyX);

                                Keyframe keyY = new Keyframe(time / settings.frameRate, position.y);
                                if (settings.importTangents)
                                {
                                    keyY.inTangent = node.InTangent.Z;
                                    keyY.outTangent = node.OutTangent.Z;
                                }
                                curveY.AddKey(keyY);

                                Keyframe keyZ = new Keyframe(time / settings.frameRate, position.z);
                                if (settings.importTangents)
                                {
                                    keyZ.inTangent = node.InTangent.X;
                                    keyZ.outTangent = node.OutTangent.X;
                                }
                                curveZ.AddKey(keyZ);
                            }
                        }

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

                    // Rotation.
                    {
                        AnimationCurve curveX = new AnimationCurve();
                        AnimationCurve curveY = new AnimationCurve();
                        AnimationCurve curveZ = new AnimationCurve();
                        AnimationCurve curveW = new AnimationCurve();

                        CAnimator<CVector4> crotations = chelper.Rotation;
                        for (int k = 0; k < crotations.Count; k++)
                        {
                            CAnimatorNode<CVector4> node = crotations.Get(k);
                            if (csequence.IntervalStart <= node.Time && node.Time <= csequence.IntervalEnd)
                            {
                                float time = node.Time - csequence.IntervalStart;
                                Quaternion rotation = node.Value.ToQuaternion();

                                Keyframe keyX = new Keyframe(time / settings.frameRate, -rotation.y);
                                if (settings.importTangents)
                                {
                                    keyX.inTangent = node.InTangent.Y;
                                    keyX.outTangent = node.OutTangent.Y;
                                }
                                curveX.AddKey(keyX);

                                Keyframe keyY = new Keyframe(time / settings.frameRate, rotation.z);
                                if (settings.importTangents)
                                {
                                    keyY.inTangent = node.InTangent.Z;
                                    keyY.outTangent = node.OutTangent.Z;
                                }
                                curveY.AddKey(keyY);

                                Keyframe keyZ = new Keyframe(time / settings.frameRate, rotation.x);
                                if (settings.importTangents)
                                {
                                    keyZ.inTangent = node.InTangent.X;
                                    keyZ.outTangent = node.OutTangent.X;
                                }
                                curveZ.AddKey(keyZ);

                                Keyframe keyW = new Keyframe(time / settings.frameRate, -rotation.w);
                                if (settings.importTangents)
                                {
                                    keyW.inTangent = node.InTangent.W;
                                    keyW.outTangent = node.OutTangent.W;
                                }
                                curveW.AddKey(keyW);
                            }
                        }


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

                    // Scaling.
                    {
                        AnimationCurve curveX = new AnimationCurve();
                        AnimationCurve curveY = new AnimationCurve();
                        AnimationCurve curveZ = new AnimationCurve();

                        CAnimator<CVector3> cscalings = chelper.Scaling;
                        for (int k = 0; k < cscalings.Count; k++)
                        {
                            CAnimatorNode<CVector3> node = cscalings.Get(k);
                            if (csequence.IntervalStart <= node.Time && node.Time <= csequence.IntervalEnd)
                            {
                                float time = node.Time - csequence.IntervalStart;

                                Keyframe keyX = new Keyframe(time / settings.frameRate, -node.Value.Y);
                                curveX.AddKey(keyX);

                                Keyframe keyY = new Keyframe(time / settings.frameRate, node.Value.Z);
                                curveY.AddKey(keyY);

                                Keyframe keyZ = new Keyframe(time / settings.frameRate, node.Value.X);
                                curveZ.AddKey(keyZ);
                            }
                        }


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


                // For each Geoset
                for (int j = 0; j < cmodel.GeosetAnimations.Count; j++)
                {


                    CGeosetAnimation cgeosetAnimation = cmodel.GeosetAnimations.Get(j);
                    GeosetHandler geosetHandler = modelHandler.FindGoeset(cgeosetAnimation.Geoset.Object);
                    if (geosetHandler == null)
                    {
                        continue;
                    }

                    GameObject go = geosetHandler.gameObject;
                    //Material material = geosetHandler.renderer.material;
                    string path = go.name;


                    AnimationCurve curveLayerAlpha = new AnimationCurve();
                    AnimationCurve curveA = new AnimationCurve();
                    AnimationCurve curveColorR = new AnimationCurve();
                    AnimationCurve curveColorG = new AnimationCurve();
                    AnimationCurve curveColorB = new AnimationCurve();
                    AnimationCurve curveColorA = new AnimationCurve();

                    CMaterialLayer cMaterialLayer = geosetHandler.cgeoset.Material.Object.Layers[0];
                    CAnimator<float> layeralpha = cMaterialLayer.Alpha;
                    for (int k = 0; k < layeralpha.Count; k++)
                    {
                        CAnimatorNode<float> node = layeralpha.Get(k);
                        if (csequence.IntervalStart <= node.Time && node.Time <= csequence.IntervalEnd)
                        {
                            float time = node.Time - csequence.IntervalStart;
                            float value = node.Value;
                            Debug.Log($"layeralpha {path}, {time}, {value} ");

                            Keyframe keyA = new Keyframe(time / settings.frameRate, node.Value);
                            if (settings.importTangents)
                            {
                                keyA.inTangent = node.InTangent;
                                keyA.outTangent = node.OutTangent;
                            }
                            curveLayerAlpha.AddKey(keyA);
                        }
                    }



                    CAnimator<float> alpha = cgeosetAnimation.Alpha;
                    for (int k = 0; k < alpha.Count; k++)
                    {
                        CAnimatorNode<float> node = alpha.Get(k);
                        if (csequence.IntervalStart <= node.Time && node.Time <= csequence.IntervalEnd)
                        {
                            float time = node.Time - csequence.IntervalStart;
                            int value = node.Value != 0 ? 1 : 0;
                            Debug.Log($"curveA {path}, {time}, {value} ");
                            Keyframe keyX = new Keyframe(time / settings.frameRate, value);
                            if (settings.importTangents)
                            {
                                keyX.inTangent = node.InTangent;
                                keyX.outTangent = node.OutTangent;
                            }
                            curveA.AddKey(keyX);


                            Keyframe keyA = new Keyframe(time / settings.frameRate, node.Value);
                            if (settings.importTangents)
                            {
                                keyA.inTangent = node.InTangent;
                                keyA.outTangent = node.OutTangent;
                            }
                            curveColorA.AddKey(keyA);
                        }
                    }


                    CAnimator<CVector3> color = cgeosetAnimation.Color;
                    for (int k = 0; k < color.Count; k++)
                    {
                        CAnimatorNode<CVector3> node = color.Get(k);
                        if (csequence.IntervalStart <= node.Time && node.Time <= csequence.IntervalEnd)
                        {
                            float time = node.Time - csequence.IntervalStart;
                            Debug.Log($"curveColorR {path}, {time}, {node.Value.X} ");
                            Keyframe keyX = new Keyframe(time / settings.frameRate, node.Value.X);
                            if (settings.importTangents)
                            {
                                keyX.inTangent = node.InTangent.X;
                                keyX.outTangent = node.OutTangent.X;
                            }
                            curveColorR.AddKey(keyX);


                            Debug.Log($"curveColorG {path}, {time}, {node.Value.Y} ");
                            Keyframe keyY = new Keyframe(time / settings.frameRate, node.Value.Y);
                            if (settings.importTangents)
                            {
                                keyY.inTangent = node.InTangent.Y;
                                keyY.outTangent = node.OutTangent.Y;
                            }
                            curveColorG.AddKey(keyY);


                            Debug.Log($"curveColorB {path}, {time}, {node.Value.Z} ");
                            Keyframe keyZ = new Keyframe(time / settings.frameRate, node.Value.Z);
                            if (settings.importTangents)
                            {
                                keyZ.inTangent = node.InTangent.Z;
                                keyZ.outTangent = node.OutTangent.Z;
                            }
                            curveColorB.AddKey(keyZ);
                        }
                    }


                    if (curveA.length == 0)
                    {
                        Keyframe key = new Keyframe(0, 1);
                        curveA.AddKey(key);
                    }

                    if (curveColorR.length == 0)
                    {
                        Keyframe key = new Keyframe(0, 1);
                        curveColorR.AddKey(key);
                    }

                    if (curveColorG.length == 0)
                    {
                        Keyframe key = new Keyframe(0, 1);
                        curveColorG.AddKey(key);
                    }

                    if (curveColorB.length == 0)
                    {
                        Keyframe key = new Keyframe(0, 1);
                        curveColorB.AddKey(key);
                    }

                    if (curveColorA.length == 0)
                    {
                        Keyframe key = new Keyframe(0, 1);
                        curveColorA.AddKey(key);
                    }



                    if (curveA.length > 0)
                    {
                        clip.SetCurve(path, typeof(GameObject), "m_IsActive", curveA);
                    }



                    if (curveColorR.length > 0)
                    {
                        clip.SetCurve(path, typeof(SkinnedMeshRenderer), "material._Color.r", curveColorR);
                    }

                    if (curveColorG.length > 0)
                    {
                        clip.SetCurve(path, typeof(SkinnedMeshRenderer), "material._Color.g", curveColorG);
                    }

                    if (curveColorB.length > 0)
                    {
                        clip.SetCurve(path, typeof(SkinnedMeshRenderer), "material._Color.b", curveColorB);
                    }

                    if (curveColorA.length > 0)
                    {
                        clip.SetCurve(path, typeof(SkinnedMeshRenderer), "material._Color.a", curveColorA);
                    }

                    if (curveLayerAlpha.length > 0)
                    {
                        clip.SetCurve(path, typeof(SkinnedMeshRenderer), "material._layerAlpha", curveLayerAlpha);
                    }
                }


                // For each ParticleEmitters2
                for (int j = 0; j < cmodel.ParticleEmitters2.Count; j++)
                {

                    CParticleEmitter2 cparticleEmitter2 = cmodel.ParticleEmitters2.Get(j);
                    MdxParticleSystem2 p = modelHandler.particleEmitters2Handler.mdxParticleSystem2s[j];
                    string path = GetPath(p.gameObject);


                    AnimationCurve curve;
                    CAnimator<float> floatList;

                    curve = new AnimationCurve();
                    floatList = cparticleEmitter2.Width;
                    for (int k = 0; k < floatList.Count; k++)
                    {
                        CAnimatorNode<float> node = floatList.Get(k);
                        if (csequence.IntervalStart <= node.Time && node.Time <= csequence.IntervalEnd)
                        {
                            float time = node.Time - csequence.IntervalStart;
                            float value = node.Value;
                            Keyframe keyX = new Keyframe(time / settings.frameRate, value);
                            if (settings.importTangents)
                            {
                                keyX.inTangent = node.InTangent;
                                keyX.outTangent = node.OutTangent;
                            }
                            curve.AddKey(keyX);
                        }
                    }
                    if (curve.length > 0)
                    {
                        clip.SetCurve(path, typeof(MdxParticleSystem2), "width", curve);
                    }


                    curve = new AnimationCurve();
                    floatList = cparticleEmitter2.Length;
                    for (int k = 0; k < floatList.Count; k++)
                    {
                        CAnimatorNode<float> node = floatList.Get(k);
                        if (csequence.IntervalStart <= node.Time && node.Time <= csequence.IntervalEnd)
                        {
                            float time = node.Time - csequence.IntervalStart;
                            float value = node.Value;
                            Keyframe keyX = new Keyframe(time / settings.frameRate, value);
                            if (settings.importTangents)
                            {
                                keyX.inTangent = node.InTangent;
                                keyX.outTangent = node.OutTangent;
                            }
                            curve.AddKey(keyX);
                        }
                    }
                    if (curve.length > 0)
                    {
                        clip.SetCurve(path, typeof(MdxParticleSystem2), "length", curve);
                    }



                    curve = new AnimationCurve();
                    floatList = cparticleEmitter2.Speed;
                    for (int k = 0; k < floatList.Count; k++)
                    {
                        CAnimatorNode<float> node = floatList.Get(k);
                        if (csequence.IntervalStart <= node.Time && node.Time <= csequence.IntervalEnd)
                        {
                            float time = node.Time - csequence.IntervalStart;
                            float value = node.Value;
                            Keyframe keyX = new Keyframe(time / settings.frameRate, value);
                            if (settings.importTangents)
                            {
                                keyX.inTangent = node.InTangent;
                                keyX.outTangent = node.OutTangent;
                            }
                            curve.AddKey(keyX);
                        }
                    }
                    if (curve.length > 0)
                    {
                        clip.SetCurve(path, typeof(MdxParticleSystem2), "speed", curve);
                    }


                    curve = new AnimationCurve();
                    floatList = cparticleEmitter2.Latitude;
                    for (int k = 0; k < floatList.Count; k++)
                    {
                        CAnimatorNode<float> node = floatList.Get(k);
                        if (csequence.IntervalStart <= node.Time && node.Time <= csequence.IntervalEnd)
                        {
                            float time = node.Time - csequence.IntervalStart;
                            float value = node.Value;
                            Keyframe keyX = new Keyframe(time / settings.frameRate, value);
                            if (settings.importTangents)
                            {
                                keyX.inTangent = node.InTangent;
                                keyX.outTangent = node.OutTangent;
                            }
                            curve.AddKey(keyX);
                        }
                    }
                    if (curve.length > 0)
                    {
                        clip.SetCurve(path, typeof(MdxParticleSystem2), "latitude", curve);
                    }


                    curve = new AnimationCurve();
                    floatList = cparticleEmitter2.Gravity;
                    for (int k = 0; k < floatList.Count; k++)
                    {
                        CAnimatorNode<float> node = floatList.Get(k);
                        if (csequence.IntervalStart <= node.Time && node.Time <= csequence.IntervalEnd)
                        {
                            float time = node.Time - csequence.IntervalStart;
                            float value = node.Value;
                            Keyframe keyX = new Keyframe(time / settings.frameRate, value);
                            if (settings.importTangents)
                            {
                                keyX.inTangent = node.InTangent;
                                keyX.outTangent = node.OutTangent;
                            }
                            curve.AddKey(keyX);
                        }
                    }
                    if (curve.length > 0)
                    {
                        clip.SetCurve(path, typeof(MdxParticleSystem2), "gravity", curve);
                    }

                    curve = new AnimationCurve();
                    AnimationCurve curveKey = new AnimationCurve();
                    floatList = cparticleEmitter2.EmissionRate;
                    for (int k = 0; k < floatList.Count; k++)
                    {
                        CAnimatorNode<float> node = floatList.Get(k);
                        if (csequence.IntervalStart <= node.Time && node.Time <= csequence.IntervalEnd)
                        {
                            float time = node.Time - csequence.IntervalStart;
                            float value = node.Value;
                            Keyframe keyX = new Keyframe(time / settings.frameRate, value);
                            if (settings.importTangents)
                            {
                                keyX.inTangent = node.InTangent;
                                keyX.outTangent = node.OutTangent;
                            }
                            curve.AddKey(keyX);

                            curveKey.AddKey(new Keyframe(time / settings.frameRate, value > 0 ? 1 : -1));
                        }
                    }
                    if (curve.length > 0)
                    {
                        clip.SetCurve(path, typeof(MdxParticleSystem2), "emissionRate", curve);
                    }

                    if (curveKey.length > 0)
                    {
                        clip.SetCurve(path, typeof(MdxParticleSystem2), "emissionRateKey", curveKey);
                    }


                    curve = new AnimationCurve();
                    floatList = cparticleEmitter2.Variation;
                    for (int k = 0; k < floatList.Count; k++)
                    {
                        CAnimatorNode<float> node = floatList.Get(k);
                        if (csequence.IntervalStart <= node.Time && node.Time <= csequence.IntervalEnd)
                        {
                            float time = node.Time - csequence.IntervalStart;
                            float value = node.Value;
                            Keyframe keyX = new Keyframe(time / settings.frameRate, value);
                            if (settings.importTangents)
                            {
                                keyX.inTangent = node.InTangent;
                                keyX.outTangent = node.OutTangent;
                            }
                            curve.AddKey(keyX);
                        }
                    }
                    if (curve.length > 0)
                    {
                        clip.SetCurve(path, typeof(MdxParticleSystem2), "variation", curve);
                    }




                    curve = new AnimationCurve();
                    floatList = cparticleEmitter2.Visibility;
                    for (int k = 0; k < floatList.Count; k++)
                    {
                        CAnimatorNode<float> node = floatList.Get(k);
                        if (csequence.IntervalStart <= node.Time && node.Time <= csequence.IntervalEnd)
                        {
                            float time = node.Time - csequence.IntervalStart;
                            float value = node.Value;
                            Keyframe keyX = new Keyframe(time / settings.frameRate, value);
                            if (settings.importTangents)
                            {
                                keyX.inTangent = node.InTangent;
                                keyX.outTangent = node.OutTangent;
                            }
                            curve.AddKey(keyX);
                        }
                    }
                    if (curve.length > 0)
                    {
                        clip.SetCurve(path, typeof(MdxParticleSystem2), "visibility", curve);
                    }

                }

                // Realigns quaternion keys to ensure shortest interpolation paths and avoid rotation glitches.
                clip.EnsureQuaternionContinuity();

                clips.Add(clip);
            }
        }
    }
}
