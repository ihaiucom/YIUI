using System.Xml;
using MdxLib.Model;

namespace MdxLib.ModelFormats.Xml
{
	internal sealed class CModel : CObject
	{
		private static class CSingleton
		{
			public static CModel Instance;

			static CSingleton()
			{
				Instance = new CModel();
			}
		}

		public static CModel Instance => CSingleton.Instance;

		private CModel()
		{
		}

		public void Load(CLoader Loader, XmlNode Node, MdxLib.Model.CModel Model)
		{
			Model.Version = ReadInteger(Node, "version", Model.Version);
			Model.BlendTime = ReadInteger(Node, "blend_time", Model.BlendTime);
			Model.Name = ReadString(Node, "name", Model.Name);
			Model.AnimationFile = ReadString(Node, "animation_file", Model.AnimationFile);
			Model.Extent = ReadExtent(Node, "extent", Model.Extent);
			foreach (XmlNode item in Node.SelectNodes("sequence"))
			{
				MdxLib.Model.CSequence cSequence = new MdxLib.Model.CSequence(Model);
				CSequence.Instance.Load(Loader, item, Model, cSequence);
				Model.Sequences.Add(cSequence);
			}
			foreach (XmlNode item2 in Node.SelectNodes("global_sequence"))
			{
				MdxLib.Model.CGlobalSequence cGlobalSequence = new MdxLib.Model.CGlobalSequence(Model);
				CGlobalSequence.Instance.Load(Loader, item2, Model, cGlobalSequence);
				Model.GlobalSequences.Add(cGlobalSequence);
			}
			foreach (XmlNode item3 in Node.SelectNodes("material"))
			{
				MdxLib.Model.CMaterial cMaterial = new MdxLib.Model.CMaterial(Model);
				CMaterial.Instance.Load(Loader, item3, Model, cMaterial);
				Model.Materials.Add(cMaterial);
			}
			foreach (XmlNode item4 in Node.SelectNodes("texture"))
			{
				MdxLib.Model.CTexture cTexture = new MdxLib.Model.CTexture(Model);
				CTexture.Instance.Load(Loader, item4, Model, cTexture);
				Model.Textures.Add(cTexture);
			}
			foreach (XmlNode item5 in Node.SelectNodes("texture_animation"))
			{
				MdxLib.Model.CTextureAnimation cTextureAnimation = new MdxLib.Model.CTextureAnimation(Model);
				CTextureAnimation.Instance.Load(Loader, item5, Model, cTextureAnimation);
				Model.TextureAnimations.Add(cTextureAnimation);
			}
			foreach (XmlNode item6 in Node.SelectNodes("geoset"))
			{
				MdxLib.Model.CGeoset cGeoset = new MdxLib.Model.CGeoset(Model);
				CGeoset.Instance.Load(Loader, item6, Model, cGeoset);
				Model.Geosets.Add(cGeoset);
			}
			foreach (XmlNode item7 in Node.SelectNodes("geoset_animation"))
			{
				MdxLib.Model.CGeosetAnimation cGeosetAnimation = new MdxLib.Model.CGeosetAnimation(Model);
				CGeosetAnimation.Instance.Load(Loader, item7, Model, cGeosetAnimation);
				Model.GeosetAnimations.Add(cGeosetAnimation);
			}
			foreach (XmlNode item8 in Node.SelectNodes("bone"))
			{
				MdxLib.Model.CBone cBone = new MdxLib.Model.CBone(Model);
				CBone.Instance.Load(Loader, item8, Model, cBone);
				Model.Bones.Add(cBone);
			}
			foreach (XmlNode item9 in Node.SelectNodes("light"))
			{
				MdxLib.Model.CLight cLight = new MdxLib.Model.CLight(Model);
				CLight.Instance.Load(Loader, item9, Model, cLight);
				Model.Lights.Add(cLight);
			}
			foreach (XmlNode item10 in Node.SelectNodes("helper"))
			{
				MdxLib.Model.CHelper cHelper = new MdxLib.Model.CHelper(Model);
				CHelper.Instance.Load(Loader, item10, Model, cHelper);
				Model.Helpers.Add(cHelper);
			}
			foreach (XmlNode item11 in Node.SelectNodes("attachment"))
			{
				MdxLib.Model.CAttachment cAttachment = new MdxLib.Model.CAttachment(Model);
				CAttachment.Instance.Load(Loader, item11, Model, cAttachment);
				Model.Attachments.Add(cAttachment);
			}
			foreach (XmlNode item12 in Node.SelectNodes("particle_emitter"))
			{
				MdxLib.Model.CParticleEmitter cParticleEmitter = new MdxLib.Model.CParticleEmitter(Model);
				CParticleEmitter.Instance.Load(Loader, item12, Model, cParticleEmitter);
				Model.ParticleEmitters.Add(cParticleEmitter);
			}
			foreach (XmlNode item13 in Node.SelectNodes("particle_emitter_2"))
			{
				MdxLib.Model.CParticleEmitter2 cParticleEmitter2 = new MdxLib.Model.CParticleEmitter2(Model);
				CParticleEmitter2.Instance.Load(Loader, item13, Model, cParticleEmitter2);
				Model.ParticleEmitters2.Add(cParticleEmitter2);
			}
			foreach (XmlNode item14 in Node.SelectNodes("ribbon_emitter"))
			{
				MdxLib.Model.CRibbonEmitter cRibbonEmitter = new MdxLib.Model.CRibbonEmitter(Model);
				CRibbonEmitter.Instance.Load(Loader, item14, Model, cRibbonEmitter);
				Model.RibbonEmitters.Add(cRibbonEmitter);
			}
			foreach (XmlNode item15 in Node.SelectNodes("camera"))
			{
				MdxLib.Model.CCamera cCamera = new MdxLib.Model.CCamera(Model);
				CCamera.Instance.Load(Loader, item15, Model, cCamera);
				Model.Cameras.Add(cCamera);
			}
			foreach (XmlNode item16 in Node.SelectNodes("event"))
			{
				MdxLib.Model.CEvent cEvent = new MdxLib.Model.CEvent(Model);
				CEvent.Instance.Load(Loader, item16, Model, cEvent);
				Model.Events.Add(cEvent);
			}
			foreach (XmlNode item17 in Node.SelectNodes("collision_shape"))
			{
				MdxLib.Model.CCollisionShape cCollisionShape = new MdxLib.Model.CCollisionShape(Model);
				CCollisionShape.Instance.Load(Loader, item17, Model, cCollisionShape);
				Model.CollisionShapes.Add(cCollisionShape);
			}
			XmlNode xmlNode = Node.SelectSingleNode("meta");
			if (xmlNode != null && xmlNode.ChildNodes.Count > 0)
			{
				XmlNode newChild = Model.MetaData.ImportNode(xmlNode, deep: true);
				Model.MetaData.ReplaceChild(newChild, Model.MetaDataRoot);
			}
		}

		public void Save(CSaver Saver, XmlNode Node, MdxLib.Model.CModel Model)
		{
			WriteInteger(Node, "version", Model.Version);
			WriteInteger(Node, "blend_time", Model.BlendTime);
			WriteString(Node, "name", Model.Name);
			WriteString(Node, "animation_file", Model.AnimationFile);
			WriteExtent(Node, "extent", Model.Extent);
			if (Model.HasSequences)
			{
				foreach (MdxLib.Model.CSequence sequence in Model.Sequences)
				{
					XmlElement node = AppendElement(Node, "sequence");
					CSequence.Instance.Save(Saver, node, Model, sequence);
				}
			}
			if (Model.HasGlobalSequences)
			{
				foreach (MdxLib.Model.CGlobalSequence globalSequence in Model.GlobalSequences)
				{
					XmlElement node2 = AppendElement(Node, "global_sequence");
					CGlobalSequence.Instance.Save(Saver, node2, Model, globalSequence);
				}
			}
			if (Model.HasMaterials)
			{
				foreach (MdxLib.Model.CMaterial material in Model.Materials)
				{
					XmlElement node3 = AppendElement(Node, "material");
					CMaterial.Instance.Save(Saver, node3, Model, material);
				}
			}
			if (Model.HasTextures)
			{
				foreach (MdxLib.Model.CTexture texture in Model.Textures)
				{
					XmlElement node4 = AppendElement(Node, "texture");
					CTexture.Instance.Save(Saver, node4, Model, texture);
				}
			}
			if (Model.HasTextureAnimations)
			{
				foreach (MdxLib.Model.CTextureAnimation textureAnimation in Model.TextureAnimations)
				{
					XmlElement node5 = AppendElement(Node, "texture_animation");
					CTextureAnimation.Instance.Save(Saver, node5, Model, textureAnimation);
				}
			}
			if (Model.HasGeosets)
			{
				foreach (MdxLib.Model.CGeoset geoset in Model.Geosets)
				{
					XmlElement node6 = AppendElement(Node, "geoset");
					CGeoset.Instance.Save(Saver, node6, Model, geoset);
				}
			}
			if (Model.HasGeosetAnimations)
			{
				foreach (MdxLib.Model.CGeosetAnimation geosetAnimation in Model.GeosetAnimations)
				{
					XmlElement node7 = AppendElement(Node, "geoset_animation");
					CGeosetAnimation.Instance.Save(Saver, node7, Model, geosetAnimation);
				}
			}
			if (Model.HasBones)
			{
				foreach (MdxLib.Model.CBone bone in Model.Bones)
				{
					XmlElement node8 = AppendElement(Node, "bone");
					CBone.Instance.Save(Saver, node8, Model, bone);
				}
			}
			if (Model.HasLights)
			{
				foreach (MdxLib.Model.CLight light in Model.Lights)
				{
					XmlElement node9 = AppendElement(Node, "light");
					CLight.Instance.Save(Saver, node9, Model, light);
				}
			}
			if (Model.HasHelpers)
			{
				foreach (MdxLib.Model.CHelper helper in Model.Helpers)
				{
					XmlElement node10 = AppendElement(Node, "helper");
					CHelper.Instance.Save(Saver, node10, Model, helper);
				}
			}
			if (Model.HasAttachments)
			{
				foreach (MdxLib.Model.CAttachment attachment in Model.Attachments)
				{
					XmlElement node11 = AppendElement(Node, "attachment");
					CAttachment.Instance.Save(Saver, node11, Model, attachment);
				}
			}
			if (Model.HasParticleEmitters)
			{
				foreach (MdxLib.Model.CParticleEmitter particleEmitter in Model.ParticleEmitters)
				{
					XmlElement node12 = AppendElement(Node, "particle_emitter");
					CParticleEmitter.Instance.Save(Saver, node12, Model, particleEmitter);
				}
			}
			if (Model.HasParticleEmitters2)
			{
				foreach (MdxLib.Model.CParticleEmitter2 item in Model.ParticleEmitters2)
				{
					XmlElement node13 = AppendElement(Node, "particle_emitter_2");
					CParticleEmitter2.Instance.Save(Saver, node13, Model, item);
				}
			}
			if (Model.HasRibbonEmitters)
			{
				foreach (MdxLib.Model.CRibbonEmitter ribbonEmitter in Model.RibbonEmitters)
				{
					XmlElement node14 = AppendElement(Node, "ribbon_emitter");
					CRibbonEmitter.Instance.Save(Saver, node14, Model, ribbonEmitter);
				}
			}
			if (Model.HasCameras)
			{
				foreach (MdxLib.Model.CCamera camera in Model.Cameras)
				{
					XmlElement node15 = AppendElement(Node, "camera");
					CCamera.Instance.Save(Saver, node15, Model, camera);
				}
			}
			if (Model.HasEvents)
			{
				foreach (MdxLib.Model.CEvent @event in Model.Events)
				{
					XmlElement node16 = AppendElement(Node, "event");
					CEvent.Instance.Save(Saver, node16, Model, @event);
				}
			}
			if (Model.HasCollisionShapes)
			{
				foreach (MdxLib.Model.CCollisionShape collisionShape in Model.CollisionShapes)
				{
					XmlElement node17 = AppendElement(Node, "collision_shape");
					CCollisionShape.Instance.Save(Saver, node17, Model, collisionShape);
				}
			}
			if (Model.HasMetaData)
			{
				XmlNode newChild = Node.OwnerDocument.ImportNode(Model.MetaDataRoot, deep: true);
				Node.OwnerDocument.DocumentElement.AppendChild(newChild);
			}
		}
	}
}
