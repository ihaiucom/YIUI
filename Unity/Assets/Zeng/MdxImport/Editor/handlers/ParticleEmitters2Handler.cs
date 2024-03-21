using MdxLib.Model;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Zeng.Mdx.Runtimes;

namespace Zeng.MdxImport.mdx.handlers
{
    public class ParticleEmitters2Handler
    {
        public ModelHandler modelHandler;
        private CModel cmodel;
        public List<MdxParticleSystem2> mdxParticleSystem2s = new List<MdxParticleSystem2>();

        public ParticleEmitters2Handler(ModelHandler modelHandler)
        {
            this.modelHandler = modelHandler;
            cmodel = modelHandler.cmodel;
        }


        public void Imports()
        {
            CObjectContainer<CParticleEmitter2> cparticles2 = cmodel.ParticleEmitters2;
            for (int i = 0; i < cparticles2.Count; i++)
            {
                CParticleEmitter2 cparticle2 = cparticles2.Get(i);
                GameObject go = modelHandler.CreateGameObject("ParticleEmitter2_" + cparticle2.Name, cparticle2.PivotPoint, cparticle2.Parent.NodeId);
                MdxParticleSystem2 p = go.AddComponent<MdxParticleSystem2>();
                p.material = AssetDatabase.LoadAssetAtPath<Material>("Assets/Zeng/MdxImport/particles.mat");
                mdxParticleSystem2s.Add(p);

                MdxParticleSystem2Render render = go.AddComponent<MdxParticleSystem2Render>();
                render.systems = p;

                p.width = cparticle2.Width.GetValue();
                p.length = cparticle2.Length.GetValue();
                p.speed = cparticle2.Speed.GetValue();
                p.latitude = cparticle2.Latitude.GetValue();
                p.gravity = cparticle2.Gravity.GetValue();
                p.emissionRate = cparticle2.EmissionRate.GetValue();
                p.variation = cparticle2.Variation.GetValue();
                p.visibility = cparticle2.Visibility.GetValue();


                p.squirt = cparticle2.Squirt;
                p.lifeSpan = cparticle2.LifeSpan;
                p.timeMiddle = cparticle2.Time;

                Texture texture = MdxTextureManager.I.Get(cparticle2?.Texture?.Object);
                p.texture = texture;
                p.columns = cparticle2.Columns;
                p.rows = cparticle2.Rows;
                p.replaceableId = cparticle2.ReplaceableId;

                p.tailLength = cparticle2.TailLength;
                p.head = cparticle2.Head;
                p.tail = cparticle2.Tail;
                p.colors[0] = new Vector4(cparticle2.Segment1.Color.X, cparticle2.Segment1.Color.Y, cparticle2.Segment1.Color.Z, cparticle2.Segment1.Alpha);
                p.colors[1] = new Vector4(cparticle2.Segment2.Color.X, cparticle2.Segment2.Color.Y, cparticle2.Segment2.Color.Z, cparticle2.Segment2.Alpha);
                p.colors[2] = new Vector4(cparticle2.Segment3.Color.X, cparticle2.Segment3.Color.Y, cparticle2.Segment3.Color.Z, cparticle2.Segment3.Alpha);
                p.scaling[0] = cparticle2.Segment1.Scaling;
                p.scaling[1] = cparticle2.Segment2.Scaling;
                p.scaling[2] = cparticle2.Segment3.Scaling;
                p.intervals[0] = new Vector3(cparticle2.HeadLife.Start, cparticle2.HeadLife.End, cparticle2.HeadLife.Repeat);
                p.intervals[1] = new Vector3(cparticle2.HeadDecay.Start, cparticle2.HeadDecay.End, cparticle2.HeadDecay.Repeat);
                p.intervals[2] = new Vector3(cparticle2.TailLife.Start, cparticle2.TailLife.End, cparticle2.TailLife.Repeat);
                p.intervals[3] = new Vector3(cparticle2.TailDecay.Start, cparticle2.TailDecay.End, cparticle2.TailDecay.Repeat);

                p.lineEmitter = cparticle2.LineEmitter;
                p.modelSpace = cparticle2.ModelSpace;
                p.xZQuad = cparticle2.XYQuad;


                p.filterMode = cparticle2.FilterMode;
                p.priorityPlane = cparticle2.PriorityPlane;
            }
        }

    }
}
