using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Zeng.mdx.commons;

namespace Zeng.mdx.parsers.mdx
{
    public class Model
    {
        /**
            * 800 用于魔兽争霸3：RoC和TFT。
            * >800 用于魔兽争霸3：重制版。
            */
        public int version = 800;
        public string name = "";
        /**
            * 据我所知，这个应该始终保持为空。
            */
        public string animationFile = "";
        public Extent extent = new Extent();
        /**
            * 这仅用于随附Art Tools的已废弃的预览器。
            */
        public int blendTime = 0;
        public List<Sequence> sequences = new List<Sequence>();
        public List<int> globalSequences = new List<int>();
        public List<Material> materials = new List<Material>();
        public List<Texture> textures = new List<Texture>();
        public List<TextureAnimation> textureAnimations = new List<TextureAnimation>();
        public List<Geoset> geosets = new List<Geoset>();
        public List<GeosetAnimation> geosetAnimations = new List<GeosetAnimation>();
        public List<Bone> bones = new List<Bone>();
        public List<Light> lights = new List<Light>();
        public List<Helper> helpers = new List<Helper>();
        public List<Attachment> attachments = new List<Attachment>();
        public List<float[]> pivotPoints = new List<float[]>();
        public List<ParticleEmitter> particleEmitters = new List<ParticleEmitter>();
        public List<ParticleEmitter2> particleEmitters2 = new List<ParticleEmitter2>();
        /**
            * @since 900
            */
        public List<ParticleEmitterPopcorn> particleEmittersPopcorn = new List<ParticleEmitterPopcorn>();
        public List<RibbonEmitter> ribbonEmitters = new List<RibbonEmitter>();
        public List<Camera> cameras = new List<Camera>();
        public List<EventObject> eventObjects = new List<EventObject>();
        public List<CollisionShape> collisionShapes = new List<CollisionShape>();
        /**
            * @since 900
            */
        public List<FaceEffect> faceEffects = new List<FaceEffect>();
        /**
            * @since 900
            */
        public List<float[]> bindPose = new List<float[]>();
        /**
            * MDX格式是基于块的，魔兽争霸3不介意其中存在未知块。
            * 一些第三方工具使用它来附加模型的元数据。
            * 当遇到未知块时，它将被添加到这里。
            * 这些块将在保存为MDX时保存。
            */
        public List<UnknownChunk> unknownChunks = new List<UnknownChunk>();


        public void Load(string path)
        {
            FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            stream.Position = 0;
            MemoryStream ms = new MemoryStream();
            stream.CopyTo(ms);
            byte[] bytes = ms.ToArray();
            ms.Dispose();
            stream.Close();

            LoadMdx(bytes);
        }

        /**
         * Load the model from MDX.
         */
        public void LoadMdx(byte[] buffer)
        {
            BinaryStream stream = new BinaryStream(buffer);
            string tag;
            int size;

            stream.Skip(4); // MDLX
            while (stream.Remaining > 0)
            {
                tag = stream.ReadBinary(4);
                size = (int)stream.ReadUint32();

                if (tag == "VERS")
                {
                    LoadVersionChunk(stream);
                }
                else if (tag == "MODL")
                {
                    LoadModelChunk(stream);
                }
                else if (tag == "SEQS")
                {
                    LoadStaticObjects(sequences, typeof(Sequence), stream, size / 132);
                }
                else if (tag == "GLBS")
                {
                    LoadGlobalSequenceChunk(stream, size);
                }
                else if (tag == "MTLS")
                {
                    LoadDynamicObjects(materials, typeof(Material), stream, size);
                }
                else if (tag == "TEXS")
                {
                    LoadStaticObjects(textures, typeof(Texture), stream, size / 268);
                }
                else if (tag == "TXAN")
                {
                    LoadDynamicObjects(textureAnimations, typeof(TextureAnimation), stream, size);
                }
                else if (tag == "GEOS")
                {
                    LoadDynamicObjects(geosets, typeof(Geoset), stream, size);
                }
                else if (tag == "GEOA")
                {
                    LoadDynamicObjects(geosetAnimations, typeof(GeosetAnimation), stream, size);
                }
                else if (tag == "BONE")
                {
                    LoadDynamicObjects(bones, typeof(Bone), stream, size);
                }
                else if (tag == "LITE")
                {
                    LoadDynamicObjects(lights, typeof(Light), stream, size);
                }
                else if (tag == "HELP")
                {
                    LoadDynamicObjects(helpers, typeof(Helper), stream, size);
                }
                else if (tag == "ATCH")
                {
                    LoadDynamicObjects(attachments, typeof(Attachment), stream, size);
                }
                else if (tag == "PIVT")
                {
                    LoadPivotPointChunk(stream, size);
                }
                else if (tag == "PREM")
                {
                    LoadDynamicObjects(particleEmitters, typeof(ParticleEmitter), stream, size);
                }
                else if (tag == "PRE2")
                {
                    LoadDynamicObjects(particleEmitters2, typeof(ParticleEmitter2), stream, size);
                }
                else if (tag == "CORN")
                {
                    LoadDynamicObjects(particleEmittersPopcorn, typeof(ParticleEmitterPopcorn), stream, size);
                }
                else if (tag == "RIBB")
                {
                    LoadDynamicObjects(ribbonEmitters, typeof(RibbonEmitter), stream, size);
                }
                else if (tag == "CAMS")
                {
                    LoadDynamicObjects(cameras, typeof(Camera), stream, size);
                }
                else if (tag == "EVTS")
                {
                    LoadDynamicObjects(eventObjects, typeof(EventObject), stream, size);
                }
                else if (tag == "CLID")
                {
                    LoadDynamicObjects(collisionShapes, typeof(CollisionShape), stream, size);
                }
                else if (tag == "FAFX")
                {
                    LoadStaticObjects(faceEffects, typeof(FaceEffect), stream, size / 340);
                }
                else if (tag == "BPOS")
                {
                    LoadBindPoseChunk(stream, size);
                }
                else
                {
                    unknownChunks.Add(new UnknownChunk(stream, size, tag));
                }
            }
        }

        void LoadVersionChunk(BinaryStream stream)
        {
            this.version = (int)stream.ReadUint32();
        }

        void LoadModelChunk(BinaryStream stream)
        {
            this.name = stream.Read(80);
            this.animationFile = stream.Read(260);
            this.extent.ReadMdx(stream);
            this.blendTime = (int)stream.ReadUint32();
        }

        void LoadStaticObjects<T>(List<T> outList, Type clsType, BinaryStream stream, int count) where T: IMdxStaticObject // Sequence | Texture | FaceEffect
        {
            for (int i = 0; i < count; i++)
            {
                T obj = (T)Activator.CreateInstance(clsType);

                obj.ReadMdx(stream);

                outList.Add(obj);
            }
        }

        void LoadGlobalSequenceChunk(BinaryStream stream, int size)
        {
            for (int i = 0, l = size / 4; i < l; i++)
            {
                this.globalSequences.Add((int)stream.ReadUint32());
            }
        }

        void LoadDynamicObjects<T>(List<T> outList, Type clsType, BinaryStream stream, int size) where T : IMdxDynamicObject //  Material | TextureAnimation | Geoset | GeosetAnimation | Bone | Light | Helper | Attachment | ParticleEmitter | ParticleEmitter2 | RibbonEmitter | Camera | EventObject | CollisionShape | ParticleEmitterPopcorn;
        {
            int end = stream.Index + size;

            while (stream.Index < end)
            {
                T obj = (T)Activator.CreateInstance(clsType);

                obj.ReadMdx(stream, this.version);

                outList.Add(obj);
            }
        }

        void LoadPivotPointChunk(BinaryStream stream, int size)
        {
            for (int i = 0, l = size / 12; i < l; i++)
            {
                this.pivotPoints.Add(stream.ReadFloat32Array(3));
            }
        }

        void LoadBindPoseChunk(BinaryStream stream, int size)
        {
            int l = (int)stream.ReadUint32();

            for (int i = 0; i < l; i++)
            {
                this.bindPose[i] = stream.ReadFloat32Array(12);
            }
        }



        /**
         * Save the model as MDX.
         */
        public byte[] SaveMdx()
        {
            BinaryStream stream = new BinaryStream(new byte[this.GetByteLength()]);

            stream.WriteBinary("MDLX");
            SaveVersionChunk(stream);
            SaveModelChunk(stream);
            SaveStaticObjectChunk(stream, "SEQS", sequences, 132);
            SaveGlobalSequenceChunk(stream);
            SaveDynamicObjectChunk(stream, "MTLS", materials);
            SaveStaticObjectChunk(stream, "TEXS", textures, 268);
            SaveDynamicObjectChunk(stream, "TXAN", textureAnimations);
            SaveDynamicObjectChunk(stream, "GEOS", geosets);
            SaveDynamicObjectChunk(stream, "GEOA", geosetAnimations);
            SaveDynamicObjectChunk(stream, "BONE", bones);
            SaveDynamicObjectChunk(stream, "LITE", lights);
            SaveDynamicObjectChunk(stream, "HELP", helpers);
            SaveDynamicObjectChunk(stream, "ATCH", attachments);
            SavePivotPointChunk(stream);
            SaveDynamicObjectChunk(stream, "PREM", particleEmitters);
            SaveDynamicObjectChunk(stream, "PRE2", particleEmitters2);

            if (version > 800)
            {
                SaveDynamicObjectChunk(stream, "CORN", particleEmittersPopcorn);
            }

            SaveDynamicObjectChunk(stream, "RIBB", ribbonEmitters);
            SaveDynamicObjectChunk(stream, "CAMS", cameras);
            SaveDynamicObjectChunk(stream, "EVTS", eventObjects);
            SaveDynamicObjectChunk(stream, "CLID", collisionShapes);

            if (version > 800)
            {
                SaveStaticObjectChunk(stream, "FAFX", faceEffects, 340);
                SaveBindPoseChunk(stream);
            }

            foreach (var chunk in unknownChunks)
            {
                chunk.WriteMdx(stream, this.version);
            }

            return stream.Uint8array;
        }

        void SaveVersionChunk(BinaryStream stream)
        {
            stream.WriteBinary("VERS");
            stream.WriteUint32(4);
            stream.WriteUint32((uint)this.version);
        }

        void SaveModelChunk(BinaryStream stream)
        {
            stream.WriteBinary("MODL");
            stream.WriteUint32(372);
            stream.Skip(80 - stream.Write(this.name));
            stream.Skip(260 - stream.Write(this.animationFile));
            this.extent.WriteMdx(stream);
            stream.WriteUint32((uint)this.blendTime);
        }

        void SaveStaticObjectChunk<T>(BinaryStream stream, string name, List<T> objects, int size) where T : IMdxStaticObject
        {
            if (objects.Count > 0)
            {
                stream.WriteBinary(name);
                stream.WriteUint32((uint)(objects.Count * size));

                foreach (var obj in objects)
                {
                    obj.WriteMdx(stream);
                }
            }
        }



        void SaveGlobalSequenceChunk(BinaryStream stream)
        {
            if (this.globalSequences.Count > 0)
            {
                stream.WriteBinary("GLBS");
                stream.WriteUint32((uint)(this.globalSequences.Count * 4));

                foreach (var globalSequence in this.globalSequences)
                {
                    stream.WriteUint32((uint)globalSequence);
                }
            }
        }



        void SaveDynamicObjectChunk<T>(BinaryStream stream, string name, List<T> objects) where T:IMdxDynamicObject
        {
            if (objects.Count > 0)
            {
                stream.WriteBinary(name);
                stream.WriteUint32((uint)GetObjectsByteLength(objects));

                foreach (var obj in objects)
                {
                    obj.WriteMdx(stream, this.version);
                }
            }
        }

        void SavePivotPointChunk(BinaryStream stream)
        {
            if (pivotPoints.Count > 0)
            {
                stream.WriteBinary("PIVT");
                stream.WriteUint32((uint)(pivotPoints.Count * 12));

                foreach (var pivotPoint in pivotPoints)
                {
                    stream.WriteFloat32Array(pivotPoint);
                }
            }
        }

        void SaveBindPoseChunk(BinaryStream stream)
        {
            if (bindPose.Count > 0)
            {
                stream.WriteBinary("BPOS");
                stream.WriteUint32((uint)(4 + bindPose.Count * 48));
                stream.WriteUint32((uint)bindPose.Count);

                foreach (var matrix in bindPose)
                {
                    stream.WriteFloat32Array(matrix);
                }
            }
        }




        /**
         * Calculate the size of the model as MDX.
         */
        public int GetByteLength()
        {
            int size = 396;

            size += GetStaticObjectsChunkByteLength(sequences.Count, 132);
            size += GetStaticObjectsChunkByteLength(globalSequences.Count, 4);
            size += GetDynamicObjectsChunkByteLength(materials);
            size += GetStaticObjectsChunkByteLength(textures.Count, 268);
            size += GetDynamicObjectsChunkByteLength(textureAnimations);
            size += GetDynamicObjectsChunkByteLength(geosets);
            size += GetDynamicObjectsChunkByteLength(geosetAnimations);
            size += GetDynamicObjectsChunkByteLength(bones);
            size += GetDynamicObjectsChunkByteLength(lights);
            size += GetDynamicObjectsChunkByteLength(helpers);
            size += GetDynamicObjectsChunkByteLength(attachments);
            size += GetStaticObjectsChunkByteLength(pivotPoints.Count, 12);
            size += GetDynamicObjectsChunkByteLength(particleEmitters);
            size += GetDynamicObjectsChunkByteLength(particleEmitters2);

            if (version > 800)
            {
                size += GetDynamicObjectsChunkByteLength(particleEmittersPopcorn);
            }

            size += GetDynamicObjectsChunkByteLength(ribbonEmitters);
            size += GetDynamicObjectsChunkByteLength(cameras);
            size += GetDynamicObjectsChunkByteLength(eventObjects);
            size += GetDynamicObjectsChunkByteLength(collisionShapes);

            if (version > 800)
            {
                size += GetStaticObjectsChunkByteLength(faceEffects.Count, 340);
                size += GetBindPoseChunkByteLength();
            }

            size += GetObjectsByteLength(unknownChunks);

            return size;
        }

        public int GetObjectsByteLength<T>(List<T> objects) where T : IMdxDynamicObject 
        {
            int size = 0;

            foreach (var obj in objects)
            {
                size += obj.GetByteLength(this.version);
            }

            return size;
        }
        public int GetDynamicObjectsChunkByteLength<T>(List<T> objects) where T: IMdxDynamicObject
        {
            if (objects.Count > 0)
            {
                return 8 + GetObjectsByteLength(objects);
            }

            return 0;
        }

        public int GetStaticObjectsChunkByteLength(int listCount, int size)
        {
            if (listCount > 0)
            {
                return 8 + listCount * size;
            }

            return 0;
        }

        public int GetBindPoseChunkByteLength()
        {
            if (this.bindPose.Count > 0)
            {
                return 12 + this.bindPose.Count * 48;
            }

            return 0;
        }






    }
}
