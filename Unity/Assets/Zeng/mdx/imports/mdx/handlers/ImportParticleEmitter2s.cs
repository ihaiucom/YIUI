
using MdxLib.Model;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Zeng.mdx.commons;
using Zeng.mdx.parsers.mdx;
using Zeng.Mdx.Runtimes;
using ParticleEmitter2 = Zeng.mdx.parsers.mdx.ParticleEmitter2;

namespace Zeng.mdx.imports.mdx
{
    public class ImportParticleEmitter2s
    {
        public ImportModel importModel;
        public Model cmodel;

        public SortedDictionary<int, GameObject> nodes = new SortedDictionary<int, GameObject>();
        public List<MdxParticleSystem2> mdxParticleSystem2s = new List<MdxParticleSystem2>();

        public ImportParticleEmitter2s(ImportModel importModel, Model cmodel)
        {
            this.importModel = importModel;
            this.cmodel = cmodel;
        
        }


        public void ToUnity()
        {
            for (int i = 0; i < cmodel.particleEmitters2.Count; i++)
            {
                ParticleEmitter2 cparticle2 = cmodel.particleEmitters2[i];

                Vector3 pivot = new Vector3();
                if (cparticle2.objectId < cmodel.pivotPoints.Count)
                {
                    var cpivot = cmodel.pivotPoints[cparticle2.objectId];
                    pivot.x = cpivot[0];
                    pivot.y = cpivot[1];
                    pivot.z = cpivot[2];
                    pivot = pivot.VecToU3d();
                }

                GameObject go = importModel.CreateGameObject($"ParticleEmitter2_{cparticle2.name}" , pivot, cparticle2.parentId);
                nodes.Add(cparticle2.objectId, go);

                MdxParticleSystem2 p = go.AddComponent<MdxParticleSystem2>();
                p.material = AssetDatabase.LoadAssetAtPath<UnityEngine.Material>("Assets/Zeng/MdxImport/particles.mat");
                mdxParticleSystem2s.Add(p);


                MdxParticleSystem2Render render = go.AddComponent<MdxParticleSystem2Render>();
                render.systems = p;

                p.width = cparticle2.width;
                p.length = cparticle2.length;
                p.speed = cparticle2.speed;
                p.latitude = cparticle2.latitude;
                p.gravity = cparticle2.gravity;
                p.emissionRate = cparticle2.emissionRate;
                p.variation = cparticle2.variation;
                p.visibility = 1.0f;


                p.squirt = cparticle2.squirt == 1;
                p.lifeSpan = cparticle2.lifeSpan;
                p.timeMiddle = cparticle2.timeMiddle;

                Debug.Log("cparticle2.textureId " + cparticle2.textureId + ", cparticle2.replaceableId=" + cparticle2.replaceableId);
                if (cparticle2.textureId > -1 && cparticle2.textureId < cmodel.textures.Count)
                {
                    string texturePath = cmodel.textures[cparticle2.textureId].TexturePath;
                    p.texturePath = texturePath;
                    if (!string.IsNullOrEmpty(texturePath))
                    {
                        texturePath = MdxUnityResPathDefine.GetPath(texturePath);
                        UnityEngine.Texture texture = AssetDatabase.LoadAssetAtPath<UnityEngine.Texture>(texturePath);
                        p.texture = texture;
                    }
                }
                else
                {
                    string texturePath = cparticle2.ReplaceablePath;
                    p.texturePath = texturePath;
                    if (!string.IsNullOrEmpty(texturePath))
                    {
                        texturePath = MdxUnityResPathDefine.GetPath(texturePath);
                        UnityEngine.Texture texture = AssetDatabase.LoadAssetAtPath<UnityEngine.Texture>(texturePath);
                        p.texture = texture;
                    }

                }


                p.columns = (int)cparticle2.columns;
                p.rows = (int)cparticle2.rows;
                p.replaceableId = (int)cparticle2.replaceableId;
                p.textureId = cparticle2.textureId;

                p.tailLength = cparticle2.tailLength;
                p.head = cparticle2.Head;
                p.tail = cparticle2.Tail;
                p.colors[0] = new Vector4(cparticle2.segmentColors[0][0], cparticle2.segmentColors[0][1], cparticle2.segmentColors[0][2], cparticle2.segmentAlphas[0] / 255f);
                p.colors[1] = new Vector4(cparticle2.segmentColors[1][0], cparticle2.segmentColors[1][1], cparticle2.segmentColors[1][2], cparticle2.segmentAlphas[1] / 255f);
                p.colors[2] = new Vector4(cparticle2.segmentColors[2][0], cparticle2.segmentColors[2][1], cparticle2.segmentColors[2][2], cparticle2.segmentAlphas[2] / 255f);

                p.scaling[0] = cparticle2.segmentScaling[0];
                p.scaling[1] = cparticle2.segmentScaling[1];
                p.scaling[2] = cparticle2.segmentScaling[2];

                p.intervals[0] = new Vector3(cparticle2.headIntervals[0][0], cparticle2.headIntervals[0][1], cparticle2.headIntervals[0][2]);
                p.intervals[1] = new Vector3(cparticle2.headIntervals[1][0], cparticle2.headIntervals[1][1], cparticle2.headIntervals[1][2]);


                p.intervals[2] = new Vector3(cparticle2.tailIntervals[0][0], cparticle2.tailIntervals[0][1], cparticle2.tailIntervals[0][2]);
                p.intervals[3] = new Vector3(cparticle2.tailIntervals[1][0], cparticle2.tailIntervals[1][1], cparticle2.tailIntervals[1][2]);

                p.lineEmitter = (cparticle2.flags & (int)ParticleEmitter2.Flags.LineEmitter) != 0;
                p.modelSpace = (cparticle2.flags & (int)ParticleEmitter2.Flags.ModelSpace) != 0; 
                p.xZQuad = (cparticle2.flags & (int)ParticleEmitter2.Flags.XYQuad) != 0; 


                p.filterMode = (EParticleEmitter2FilterMode)(int)cparticle2.filterMode;
                p.priorityPlane = cparticle2.priorityPlane;
            }
        }

    }
}
