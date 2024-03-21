using System;
using MdxLib.Model;
using MdxLib.ModelFormats.Mdl.Token;
using MdxLib.ModelFormats.Mdl.Value;

namespace MdxLib.ModelFormats.Mdl
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
			MdxLib.Model.CLight cLight = new MdxLib.Model.CLight(Model);
			Load(Loader, Model, cLight);
			Model.Lights.Add(cLight);
		}

		public void Load(CLoader Loader, MdxLib.Model.CModel Model, MdxLib.Model.CLight Light)
		{
			Light.Name = Loader.ReadString();
			Loader.ExpectToken(EType.CurlyBracketLeft);
			while (Loader.PeekToken() != EType.CurlyBracketRight)
			{
				string text = Loader.ReadWord();
				if (LoadNode(Loader, Model, Light, text))
				{
					continue;
				}
				switch (text)
				{
				case "static":
					text = Loader.ReadWord();
					if (!LoadStaticNode(Loader, Model, Light, text))
					{
						switch (text)
						{
						case "attenuationstart":
							LoadStaticAnimator(Loader, Model, Light.AttenuationStart, CFloat.Instance);
							break;
						case "attenuationend":
							LoadStaticAnimator(Loader, Model, Light.AttenuationEnd, CFloat.Instance);
							break;
						case "color":
							LoadStaticAnimator(Loader, Model, Light.Color, CColor.Instance);
							break;
						case "intensity":
							LoadStaticAnimator(Loader, Model, Light.Intensity, CFloat.Instance);
							break;
						case "ambcolor":
							LoadStaticAnimator(Loader, Model, Light.AmbientColor, CColor.Instance);
							break;
						case "ambintensity":
							LoadStaticAnimator(Loader, Model, Light.AmbientIntensity, CFloat.Instance);
							break;
						case "visibility":
							LoadStaticAnimator(Loader, Model, Light.Visibility, CFloat.Instance);
							break;
						default:
							throw new Exception("Syntax error at line " + Loader.Line + ", unknown tag \"" + text + "\"!");
						}
					}
					break;
				case "attenuationstart":
					LoadAnimator(Loader, Model, Light.AttenuationStart, CFloat.Instance);
					break;
				case "attenuationend":
					LoadAnimator(Loader, Model, Light.AttenuationEnd, CFloat.Instance);
					break;
				case "color":
					LoadAnimator(Loader, Model, Light.Color, CColor.Instance);
					break;
				case "intensity":
					LoadAnimator(Loader, Model, Light.Intensity, CFloat.Instance);
					break;
				case "ambcolor":
					LoadAnimator(Loader, Model, Light.AmbientColor, CColor.Instance);
					break;
				case "ambintensity":
					LoadAnimator(Loader, Model, Light.AmbientIntensity, CFloat.Instance);
					break;
				case "visibility":
					LoadAnimator(Loader, Model, Light.Visibility, CFloat.Instance);
					break;
				case "omnidirectional":
					Light.Type = ELightType.Omnidirectional;
					LoadBoolean(Loader);
					break;
				case "directional":
					Light.Type = ELightType.Directional;
					LoadBoolean(Loader);
					break;
				case "ambient":
					Light.Type = ELightType.Ambient;
					LoadBoolean(Loader);
					break;
				default:
					throw new Exception("Syntax error at line " + Loader.Line + ", unknown tag \"" + text + "\"!");
				}
			}
			Loader.ReadToken();
		}

		public void SaveAll(CSaver Saver, MdxLib.Model.CModel Model)
		{
			if (!Model.HasLights)
			{
				return;
			}
			foreach (MdxLib.Model.CLight light in Model.Lights)
			{
				Save(Saver, Model, light);
			}
		}

		public void Save(CSaver Saver, MdxLib.Model.CModel Model, MdxLib.Model.CLight Light)
		{
			Saver.BeginGroup("Light", Light.Name);
			SaveNode(Saver, Model, Light);
			SaveBoolean(Saver, TypeToString(Light.Type), Value: true);
			SaveAnimator(Saver, Model, Light.AttenuationStart, CFloat.Instance, "AttenuationStart");
			SaveAnimator(Saver, Model, Light.AttenuationEnd, CFloat.Instance, "AttenuationEnd");
			SaveAnimator(Saver, Model, Light.Color, CColor.Instance, "Color");
			SaveAnimator(Saver, Model, Light.Intensity, CFloat.Instance, "Intensity");
			SaveAnimator(Saver, Model, Light.AmbientColor, CColor.Instance, "AmbColor");
			SaveAnimator(Saver, Model, Light.AmbientIntensity, CFloat.Instance, "AmbIntensity");
			SaveAnimator(Saver, Model, Light.Visibility, CFloat.Instance, "Visibility", ECondition.NotOne);
			Saver.EndGroup();
		}

		private string TypeToString(ELightType Type)
		{
			return Type switch
			{
				ELightType.Omnidirectional => "Omnidirectional", 
				ELightType.Directional => "Directional", 
				ELightType.Ambient => "Ambient", 
				_ => "", 
			};
		}
	}
}
