using System;
using MdxLib.Model;
using MdxLib.ModelFormats.Mdl.Token;
using MdxLib.ModelFormats.Mdl.Value;

namespace MdxLib.ModelFormats.Mdl
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

		public void LoadAll(CLoader Loader, MdxLib.Model.CModel Model)
		{
			MdxLib.Model.CParticleEmitter cParticleEmitter = new MdxLib.Model.CParticleEmitter(Model);
			Load(Loader, Model, cParticleEmitter);
			Model.ParticleEmitters.Add(cParticleEmitter);
		}

		public void Load(CLoader Loader, MdxLib.Model.CModel Model, MdxLib.Model.CParticleEmitter ParticleEmitter)
		{
			ParticleEmitter.Name = Loader.ReadString();
			Loader.ExpectToken(EType.CurlyBracketLeft);
			while (Loader.PeekToken() != EType.CurlyBracketRight)
			{
				string text = Loader.ReadWord();
				if (LoadNode(Loader, Model, ParticleEmitter, text))
				{
					continue;
				}
				switch (text)
				{
				case "static":
					text = Loader.ReadWord();
					if (!LoadStaticNode(Loader, Model, ParticleEmitter, text))
					{
						switch (text)
						{
						case "emissionrate":
							LoadStaticAnimator(Loader, Model, ParticleEmitter.EmissionRate, CFloat.Instance);
							break;
						case "gravity":
							LoadStaticAnimator(Loader, Model, ParticleEmitter.Gravity, CFloat.Instance);
							break;
						case "longitude":
							LoadStaticAnimator(Loader, Model, ParticleEmitter.Longitude, CFloat.Instance);
							break;
						case "latitude":
							LoadStaticAnimator(Loader, Model, ParticleEmitter.Latitude, CFloat.Instance);
							break;
						case "visibility":
							LoadStaticAnimator(Loader, Model, ParticleEmitter.Visibility, CFloat.Instance);
							break;
						default:
							throw new Exception("Syntax error at line " + Loader.Line + ", unknown tag \"" + text + "\"!");
						}
					}
					break;
				case "emissionrate":
					LoadAnimator(Loader, Model, ParticleEmitter.EmissionRate, CFloat.Instance);
					break;
				case "gravity":
					LoadAnimator(Loader, Model, ParticleEmitter.Gravity, CFloat.Instance);
					break;
				case "longitude":
					LoadAnimator(Loader, Model, ParticleEmitter.Longitude, CFloat.Instance);
					break;
				case "latitude":
					LoadAnimator(Loader, Model, ParticleEmitter.Latitude, CFloat.Instance);
					break;
				case "visibility":
					LoadAnimator(Loader, Model, ParticleEmitter.Visibility, CFloat.Instance);
					break;
				case "emitterusesmdl":
					ParticleEmitter.EmitterUsesMdl = LoadBoolean(Loader);
					break;
				case "emitterusestga":
					ParticleEmitter.EmitterUsesTga = LoadBoolean(Loader);
					break;
				case "particle":
					Loader.ExpectToken(EType.CurlyBracketLeft);
					while (Loader.PeekToken() != EType.CurlyBracketRight)
					{
						text = Loader.ReadWord();
						switch (text)
						{
						case "static":
							text = Loader.ReadWord();
							switch (text)
							{
							case "lifespan":
								LoadStaticAnimator(Loader, Model, ParticleEmitter.LifeSpan, CFloat.Instance);
								break;
							case "initvelocity":
								LoadStaticAnimator(Loader, Model, ParticleEmitter.InitialVelocity, CFloat.Instance);
								break;
							default:
								throw new Exception("Syntax error at line " + Loader.Line + ", unknown tag \"" + text + "\"!");
							}
							break;
						case "lifespan":
							LoadAnimator(Loader, Model, ParticleEmitter.LifeSpan, CFloat.Instance);
							break;
						case "initvelocity":
							LoadAnimator(Loader, Model, ParticleEmitter.InitialVelocity, CFloat.Instance);
							break;
						case "path":
							ParticleEmitter.FileName = LoadString(Loader);
							break;
						default:
							throw new Exception("Syntax error at line " + Loader.Line + ", unknown tag \"" + text + "\"!");
						}
					}
					Loader.ReadToken();
					break;
				default:
					throw new Exception("Syntax error at line " + Loader.Line + ", unknown tag \"" + text + "\"!");
				}
			}
			Loader.ReadToken();
		}

		public void SaveAll(CSaver Saver, MdxLib.Model.CModel Model)
		{
			if (!Model.HasParticleEmitters)
			{
				return;
			}
			foreach (MdxLib.Model.CParticleEmitter particleEmitter in Model.ParticleEmitters)
			{
				Save(Saver, Model, particleEmitter);
			}
		}

		public void Save(CSaver Saver, MdxLib.Model.CModel Model, MdxLib.Model.CParticleEmitter ParticleEmitter)
		{
			Saver.BeginGroup("ParticleEmitter", ParticleEmitter.Name);
			SaveNode(Saver, Model, ParticleEmitter);
			SaveBoolean(Saver, "EmitterUsesMDL", ParticleEmitter.EmitterUsesMdl);
			SaveBoolean(Saver, "EmitterUsesTGA", ParticleEmitter.EmitterUsesTga);
			SaveAnimator(Saver, Model, ParticleEmitter.EmissionRate, CFloat.Instance, "EmissionRate");
			SaveAnimator(Saver, Model, ParticleEmitter.Gravity, CFloat.Instance, "Gravity");
			SaveAnimator(Saver, Model, ParticleEmitter.Longitude, CFloat.Instance, "Longitude");
			SaveAnimator(Saver, Model, ParticleEmitter.Latitude, CFloat.Instance, "Latitude");
			SaveAnimator(Saver, Model, ParticleEmitter.Visibility, CFloat.Instance, "Visibility", ECondition.NotOne);
			Saver.BeginGroup("Particle");
			SaveString(Saver, "Path", ParticleEmitter.FileName);
			SaveAnimator(Saver, Model, ParticleEmitter.LifeSpan, CFloat.Instance, "LifeSpan");
			SaveAnimator(Saver, Model, ParticleEmitter.InitialVelocity, CFloat.Instance, "InitVelocity");
			Saver.EndGroup();
			Saver.EndGroup();
		}
	}
}
