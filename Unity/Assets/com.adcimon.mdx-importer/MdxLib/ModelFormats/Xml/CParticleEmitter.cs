using System.Xml;
using MdxLib.Model;
using MdxLib.ModelFormats.Xml.Value;

namespace MdxLib.ModelFormats.Xml
{
	internal sealed class CParticleEmitter : CNode
	{
		private static class CSingleton
		{
			public static CParticleEmitter Instance;

			static CSingleton()
			{
				Instance = new CParticleEmitter();
			}
		}

		public static CParticleEmitter Instance => CSingleton.Instance;

		private CParticleEmitter()
		{
		}

		public void Load(CLoader Loader, XmlNode Node, MdxLib.Model.CModel Model, MdxLib.Model.CParticleEmitter ParticleEmitter)
		{
			LoadNode(Loader, Node, Model, ParticleEmitter);
			ParticleEmitter.FileName = ReadString(Node, "filename", ParticleEmitter.FileName);
			ParticleEmitter.EmitterUsesMdl = ReadBoolean(Node, "emitter_uses_mdl", ParticleEmitter.EmitterUsesMdl);
			ParticleEmitter.EmitterUsesTga = ReadBoolean(Node, "emitter_uses_tga", ParticleEmitter.EmitterUsesTga);
			LoadAnimator(Loader, Node, Model, ParticleEmitter.EmissionRate, CFloat.Instance, "emission_rate");
			LoadAnimator(Loader, Node, Model, ParticleEmitter.Gravity, CFloat.Instance, "gravity");
			LoadAnimator(Loader, Node, Model, ParticleEmitter.Longitude, CFloat.Instance, "longitude");
			LoadAnimator(Loader, Node, Model, ParticleEmitter.Latitude, CFloat.Instance, "latitude");
			LoadAnimator(Loader, Node, Model, ParticleEmitter.Visibility, CFloat.Instance, "visibility");
			LoadAnimator(Loader, Node, Model, ParticleEmitter.LifeSpan, CFloat.Instance, "life_span");
			LoadAnimator(Loader, Node, Model, ParticleEmitter.InitialVelocity, CFloat.Instance, "initial_velocity");
		}

		public void Save(CSaver Saver, XmlNode Node, MdxLib.Model.CModel Model, MdxLib.Model.CParticleEmitter ParticleEmitter)
		{
			SaveNode(Saver, Node, Model, ParticleEmitter);
			WriteString(Node, "filename", ParticleEmitter.FileName);
			WriteBoolean(Node, "emitter_uses_mdl", ParticleEmitter.EmitterUsesMdl);
			WriteBoolean(Node, "emitter_uses_tga", ParticleEmitter.EmitterUsesTga);
			SaveAnimator(Saver, Node, Model, ParticleEmitter.EmissionRate, CFloat.Instance, "emission_rate");
			SaveAnimator(Saver, Node, Model, ParticleEmitter.Gravity, CFloat.Instance, "gravity");
			SaveAnimator(Saver, Node, Model, ParticleEmitter.Longitude, CFloat.Instance, "longitude");
			SaveAnimator(Saver, Node, Model, ParticleEmitter.Latitude, CFloat.Instance, "latitude");
			SaveAnimator(Saver, Node, Model, ParticleEmitter.Visibility, CFloat.Instance, "visibility");
			SaveAnimator(Saver, Node, Model, ParticleEmitter.LifeSpan, CFloat.Instance, "life_span");
			SaveAnimator(Saver, Node, Model, ParticleEmitter.InitialVelocity, CFloat.Instance, "initial_velocity");
		}
	}
}
