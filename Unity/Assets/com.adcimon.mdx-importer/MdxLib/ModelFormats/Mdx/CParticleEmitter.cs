using System;
using MdxLib.Model;
using MdxLib.ModelFormats.Mdx.Value;

namespace MdxLib.ModelFormats.Mdx
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
			int num = Loader.ReadInt32();
			while (num > 0)
			{
				Loader.PushLocation();
				MdxLib.Model.CParticleEmitter cParticleEmitter = new MdxLib.Model.CParticleEmitter(Model);
				Load(Loader, Model, cParticleEmitter);
				Model.ParticleEmitters.Add(cParticleEmitter);
				num -= Loader.PopLocation();
				if (num < 0)
				{
					throw new Exception("Error at location " + Loader.Location + ", too many ParticleEmitter bytes were read!");
				}
			}
		}

		public void Load(CLoader Loader, MdxLib.Model.CModel Model, MdxLib.Model.CParticleEmitter ParticleEmitter)
		{
			Loader.PushLocation();
			int num = Loader.ReadInt32();
			int num2 = CNode.LoadNode(Loader, Model, ParticleEmitter);
			ParticleEmitter.EmissionRate.MakeStatic(Loader.ReadFloat());
			ParticleEmitter.Gravity.MakeStatic(Loader.ReadFloat());
			ParticleEmitter.Longitude.MakeStatic(Loader.ReadFloat());
			ParticleEmitter.Latitude.MakeStatic(Loader.ReadFloat());
			ParticleEmitter.FileName = Loader.ReadString(260);
			ParticleEmitter.LifeSpan.MakeStatic(Loader.ReadFloat());
			ParticleEmitter.InitialVelocity.MakeStatic(Loader.ReadFloat());
			ParticleEmitter.EmitterUsesMdl = (num2 & 0x8000) != 0;
			ParticleEmitter.EmitterUsesTga = (num2 & 0x10000) != 0;
			num -= Loader.PopLocation();
			if (num < 0)
			{
				throw new Exception("Error at location " + Loader.Location + ", too many ParticleEmitter bytes were read!");
			}
			while (num > 0)
			{
				Loader.PushLocation();
				string text = Loader.ReadTag();
				switch (text)
				{
				case "KPEE":
					CObject.LoadAnimator(Loader, Model, ParticleEmitter.EmissionRate, CFloat.Instance);
					break;
				case "KPEG":
					CObject.LoadAnimator(Loader, Model, ParticleEmitter.Gravity, CFloat.Instance);
					break;
				case "KPLN":
					CObject.LoadAnimator(Loader, Model, ParticleEmitter.Longitude, CFloat.Instance);
					break;
				case "KPLT":
					CObject.LoadAnimator(Loader, Model, ParticleEmitter.Latitude, CFloat.Instance);
					break;
				case "KPEL":
					CObject.LoadAnimator(Loader, Model, ParticleEmitter.LifeSpan, CFloat.Instance);
					break;
				case "KPES":
					CObject.LoadAnimator(Loader, Model, ParticleEmitter.InitialVelocity, CFloat.Instance);
					break;
				case "KPEV":
					CObject.LoadAnimator(Loader, Model, ParticleEmitter.Visibility, CFloat.Instance);
					break;
				default:
					throw new Exception("Error at location " + Loader.Location + ", unknown ParticleEmitter tag \"" + text + "\"!");
				}
				num -= Loader.PopLocation();
				if (num < 0)
				{
					throw new Exception("Error at location " + Loader.Location + ", too many ParticleEmitter bytes were read!");
				}
			}
		}

		public void SaveAll(CSaver Saver, MdxLib.Model.CModel Model)
		{
			if (!Model.HasParticleEmitters)
			{
				return;
			}
			Saver.WriteTag("PREM");
			Saver.PushLocation();
			foreach (MdxLib.Model.CParticleEmitter particleEmitter in Model.ParticleEmitters)
			{
				Save(Saver, Model, particleEmitter);
			}
			Saver.PopExclusiveLocation();
		}

		public void Save(CSaver Saver, MdxLib.Model.CModel Model, MdxLib.Model.CParticleEmitter ParticleEmitter)
		{
			int num = 4096;
			if (ParticleEmitter.EmitterUsesMdl)
			{
				num |= 0x8000;
			}
			if (ParticleEmitter.EmitterUsesTga)
			{
				num |= 0x10000;
			}
			Saver.PushLocation();
			CNode.SaveNode(Saver, Model, ParticleEmitter, num);
			Saver.WriteFloat(ParticleEmitter.EmissionRate.GetValue());
			Saver.WriteFloat(ParticleEmitter.Gravity.GetValue());
			Saver.WriteFloat(ParticleEmitter.Longitude.GetValue());
			Saver.WriteFloat(ParticleEmitter.Latitude.GetValue());
			Saver.WriteString(ParticleEmitter.FileName, 260);
			Saver.WriteFloat(ParticleEmitter.LifeSpan.GetValue());
			Saver.WriteFloat(ParticleEmitter.InitialVelocity.GetValue());
			CObject.SaveAnimator(Saver, Model, ParticleEmitter.EmissionRate, CFloat.Instance, "KPEE");
			CObject.SaveAnimator(Saver, Model, ParticleEmitter.Gravity, CFloat.Instance, "KPEG");
			CObject.SaveAnimator(Saver, Model, ParticleEmitter.Longitude, CFloat.Instance, "KPLN");
			CObject.SaveAnimator(Saver, Model, ParticleEmitter.Latitude, CFloat.Instance, "KPLT");
			CObject.SaveAnimator(Saver, Model, ParticleEmitter.LifeSpan, CFloat.Instance, "KPEL");
			CObject.SaveAnimator(Saver, Model, ParticleEmitter.InitialVelocity, CFloat.Instance, "KPES");
			CObject.SaveAnimator(Saver, Model, ParticleEmitter.Visibility, CFloat.Instance, "KPEV");
			Saver.PopInclusiveLocation();
		}
	}
}
