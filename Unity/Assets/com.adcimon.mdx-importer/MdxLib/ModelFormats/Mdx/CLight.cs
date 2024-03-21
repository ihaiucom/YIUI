using System;
using MdxLib.Model;
using MdxLib.ModelFormats.Mdx.Value;

namespace MdxLib.ModelFormats.Mdx
{
	internal sealed class CLight : CNode
	{
		private static class CSingleton
		{
			public static CLight Instance;

			static CSingleton()
			{
				Instance = new CLight();
			}
		}

		public static CLight Instance => CSingleton.Instance;

		private CLight()
		{
		}

		public void LoadAll(CLoader Loader, MdxLib.Model.CModel Model)
		{
			int num = Loader.ReadInt32();
			while (num > 0)
			{
				Loader.PushLocation();
				MdxLib.Model.CLight cLight = new MdxLib.Model.CLight(Model);
				Load(Loader, Model, cLight);
				Model.Lights.Add(cLight);
				num -= Loader.PopLocation();
				if (num < 0)
				{
					throw new Exception("Error at location " + Loader.Location + ", too many Light bytes were read!");
				}
			}
		}

		public void Load(CLoader Loader, MdxLib.Model.CModel Model, MdxLib.Model.CLight Light)
		{
			Loader.PushLocation();
			int num = Loader.ReadInt32();
			CNode.LoadNode(Loader, Model, Light);
			int num2 = Loader.ReadInt32();
			Light.AttenuationStart.MakeStatic(Loader.ReadFloat());
			Light.AttenuationEnd.MakeStatic(Loader.ReadFloat());
			Light.Color.MakeStatic(Loader.ReadVector3());
			Light.Intensity.MakeStatic(Loader.ReadFloat());
			Light.AmbientColor.MakeStatic(Loader.ReadVector3());
			Light.AmbientIntensity.MakeStatic(Loader.ReadFloat());
			switch (num2)
			{
			case 0:
				Light.Type = ELightType.Omnidirectional;
				break;
			case 1:
				Light.Type = ELightType.Directional;
				break;
			case 2:
				Light.Type = ELightType.Ambient;
				break;
			}
			num -= Loader.PopLocation();
			if (num < 0)
			{
				throw new Exception("Error at location " + Loader.Location + ", too many Light bytes were read!");
			}
			while (num > 0)
			{
				Loader.PushLocation();
				string text = Loader.ReadTag();
				switch (text)
				{
				case "KLAS":
					CObject.LoadAnimator(Loader, Model, Light.AttenuationStart, CFloat.Instance);
					break;
				case "KLAE":
					CObject.LoadAnimator(Loader, Model, Light.AttenuationEnd, CFloat.Instance);
					break;
				case "KLAC":
					CObject.LoadAnimator(Loader, Model, Light.Color, CVector3.Instance);
					break;
				case "KLAI":
					CObject.LoadAnimator(Loader, Model, Light.Intensity, CFloat.Instance);
					break;
				case "KLBC":
					CObject.LoadAnimator(Loader, Model, Light.AmbientColor, CVector3.Instance);
					break;
				case "KLBI":
					CObject.LoadAnimator(Loader, Model, Light.AmbientIntensity, CFloat.Instance);
					break;
				case "KLAV":
					CObject.LoadAnimator(Loader, Model, Light.Visibility, CFloat.Instance);
					break;
				default:
					throw new Exception("Error at location " + Loader.Location + ", unknown Light tag \"" + text + "\"!");
				}
				num -= Loader.PopLocation();
				if (num < 0)
				{
					throw new Exception("Error at location " + Loader.Location + ", too many Light bytes were read!");
				}
			}
		}

		public void SaveAll(CSaver Saver, MdxLib.Model.CModel Model)
		{
			if (!Model.HasLights)
			{
				return;
			}
			Saver.WriteTag("LITE");
			Saver.PushLocation();
			foreach (MdxLib.Model.CLight light in Model.Lights)
			{
				Save(Saver, Model, light);
			}
			Saver.PopExclusiveLocation();
		}

		public void Save(CSaver Saver, MdxLib.Model.CModel Model, MdxLib.Model.CLight Light)
		{
			int value = 0;
			switch (Light.Type)
			{
			case ELightType.Omnidirectional:
				value = 0;
				break;
			case ELightType.Directional:
				value = 1;
				break;
			case ELightType.Ambient:
				value = 2;
				break;
			}
			Saver.PushLocation();
			CNode.SaveNode(Saver, Model, Light, 512);
			Saver.WriteInt32(value);
			Saver.WriteFloat(Light.AttenuationStart.GetValue());
			Saver.WriteFloat(Light.AttenuationEnd.GetValue());
			Saver.WriteVector3(Light.Color.GetValue());
			Saver.WriteFloat(Light.Intensity.GetValue());
			Saver.WriteVector3(Light.AmbientColor.GetValue());
			Saver.WriteFloat(Light.AmbientIntensity.GetValue());
			CObject.SaveAnimator(Saver, Model, Light.AttenuationStart, CFloat.Instance, "KLAS");
			CObject.SaveAnimator(Saver, Model, Light.AttenuationEnd, CFloat.Instance, "KLAE");
			CObject.SaveAnimator(Saver, Model, Light.Color, CVector3.Instance, "KLAC");
			CObject.SaveAnimator(Saver, Model, Light.Intensity, CFloat.Instance, "KLAI");
			CObject.SaveAnimator(Saver, Model, Light.AmbientColor, CVector3.Instance, "KLBC");
			CObject.SaveAnimator(Saver, Model, Light.AmbientIntensity, CFloat.Instance, "KLBI");
			CObject.SaveAnimator(Saver, Model, Light.Visibility, CFloat.Instance, "KLAV");
			Saver.PopInclusiveLocation();
		}
	}
}
