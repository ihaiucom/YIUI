using System.Xml;
using MdxLib.Model;
using MdxLib.ModelFormats.Xml.Value;

namespace MdxLib.ModelFormats.Xml
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

		public void Load(CLoader Loader, XmlNode Node, MdxLib.Model.CModel Model, MdxLib.Model.CLight Light)
		{
			LoadNode(Loader, Node, Model, Light);
			Light.Type = StringToType(ReadString(Node, "type", TypeToString(Light.Type)));
			LoadAnimator(Loader, Node, Model, Light.AttenuationStart, CFloat.Instance, "attenuation_start");
			LoadAnimator(Loader, Node, Model, Light.AttenuationEnd, CFloat.Instance, "attenuation_end");
			LoadAnimator(Loader, Node, Model, Light.Color, CVector3.Instance, "color");
			LoadAnimator(Loader, Node, Model, Light.Intensity, CFloat.Instance, "intensity");
			LoadAnimator(Loader, Node, Model, Light.AmbientColor, CVector3.Instance, "ambient_color");
			LoadAnimator(Loader, Node, Model, Light.AmbientIntensity, CFloat.Instance, "ambient_intensity");
			LoadAnimator(Loader, Node, Model, Light.Visibility, CFloat.Instance, "visibility");
		}

		public void Save(CSaver Saver, XmlNode Node, MdxLib.Model.CModel Model, MdxLib.Model.CLight Light)
		{
			SaveNode(Saver, Node, Model, Light);
			WriteString(Node, "type", TypeToString(Light.Type));
			SaveAnimator(Saver, Node, Model, Light.AttenuationStart, CFloat.Instance, "attenuation_start");
			SaveAnimator(Saver, Node, Model, Light.AttenuationEnd, CFloat.Instance, "attenuation_end");
			SaveAnimator(Saver, Node, Model, Light.Color, CVector3.Instance, "color");
			SaveAnimator(Saver, Node, Model, Light.Intensity, CFloat.Instance, "intensity");
			SaveAnimator(Saver, Node, Model, Light.AmbientColor, CVector3.Instance, "ambient_color");
			SaveAnimator(Saver, Node, Model, Light.AmbientIntensity, CFloat.Instance, "ambient_intensity");
			SaveAnimator(Saver, Node, Model, Light.Visibility, CFloat.Instance, "visibility");
		}

		private string TypeToString(ELightType Type)
		{
			return Type switch
			{
				ELightType.Omnidirectional => "omnidirectional", 
				ELightType.Directional => "directional", 
				ELightType.Ambient => "ambient", 
				_ => "", 
			};
		}

		private ELightType StringToType(string String)
		{
			return String switch
			{
				"omnidirectional" => ELightType.Omnidirectional, 
				"directional" => ELightType.Directional, 
				"ambient" => ELightType.Ambient, 
				_ => ELightType.Omnidirectional, 
			};
		}
	}
}
