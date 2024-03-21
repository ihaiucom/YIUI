using System.Xml;
using MdxLib.Model;
using MdxLib.ModelFormats.Xml.Value;

namespace MdxLib.ModelFormats.Xml
{
	internal sealed class CMaterialLayer : CObject
	{
		private static class CSingleton
		{
			public static CMaterialLayer Instance;

			static CSingleton()
			{
				Instance = new CMaterialLayer();
			}
		}

		public static CMaterialLayer Instance => CSingleton.Instance;

		private CMaterialLayer()
		{
		}

		public void Load(CLoader Loader, XmlNode Node, MdxLib.Model.CModel Model, MdxLib.Model.CMaterial Material, MdxLib.Model.CMaterialLayer MaterialLayer)
		{
			MaterialLayer.FilterMode = StringToFilterMode(ReadString(Node, "filter_mode", FilterModeToString(MaterialLayer.FilterMode)));
			MaterialLayer.CoordId = ReadInteger(Node, "coord_id", MaterialLayer.CoordId);
			MaterialLayer.Unshaded = ReadBoolean(Node, "unshaded", MaterialLayer.Unshaded);
			MaterialLayer.Unfogged = ReadBoolean(Node, "unfogged", MaterialLayer.Unfogged);
			MaterialLayer.TwoSided = ReadBoolean(Node, "two_sided", MaterialLayer.TwoSided);
			MaterialLayer.SphereEnvironmentMap = ReadBoolean(Node, "sphere_environment_map", MaterialLayer.SphereEnvironmentMap);
			MaterialLayer.NoDepthTest = ReadBoolean(Node, "no_depth_test", MaterialLayer.NoDepthTest);
			MaterialLayer.NoDepthSet = ReadBoolean(Node, "no_depth_set", MaterialLayer.NoDepthSet);
			LoadAnimator(Loader, Node, Model, MaterialLayer.TextureId, CInteger.Instance, "texture_id");
			LoadAnimator(Loader, Node, Model, MaterialLayer.Alpha, CFloat.Instance, "alpha");
			Loader.Attacher.AddObject(Model.Textures, MaterialLayer.Texture, ReadInteger(Node, "texture", -1));
			Loader.Attacher.AddObject(Model.TextureAnimations, MaterialLayer.TextureAnimation, ReadInteger(Node, "texture_animation", -1));
		}

		public void Save(CSaver Saver, XmlNode Node, MdxLib.Model.CModel Model, MdxLib.Model.CMaterial Material, MdxLib.Model.CMaterialLayer MaterialLayer)
		{
			WriteString(Node, "filter_mode", FilterModeToString(MaterialLayer.FilterMode));
			WriteInteger(Node, "coord_id", MaterialLayer.CoordId);
			WriteBoolean(Node, "unshaded", MaterialLayer.Unshaded);
			WriteBoolean(Node, "unfogged", MaterialLayer.Unfogged);
			WriteBoolean(Node, "two_sided", MaterialLayer.TwoSided);
			WriteBoolean(Node, "sphere_environment_map", MaterialLayer.SphereEnvironmentMap);
			WriteBoolean(Node, "no_depth_test", MaterialLayer.NoDepthTest);
			WriteBoolean(Node, "no_depth_set", MaterialLayer.NoDepthSet);
			SaveAnimator(Saver, Node, Model, MaterialLayer.TextureId, CInteger.Instance, "texture_id");
			SaveAnimator(Saver, Node, Model, MaterialLayer.Alpha, CFloat.Instance, "alpha");
			WriteInteger(Node, "texture", MaterialLayer.Texture.ObjectId);
			WriteInteger(Node, "texture_animation", MaterialLayer.TextureAnimation.ObjectId);
		}

		private string FilterModeToString(EMaterialLayerFilterMode FilterMode)
		{
			return FilterMode switch
			{
				EMaterialLayerFilterMode.None => "none", 
				EMaterialLayerFilterMode.Transparent => "transparent", 
				EMaterialLayerFilterMode.Blend => "blend", 
				EMaterialLayerFilterMode.Additive => "additive", 
				EMaterialLayerFilterMode.AdditiveAlpha => "additive_alpha", 
				EMaterialLayerFilterMode.Modulate => "modulate", 
				EMaterialLayerFilterMode.Modulate2x => "modulate_2x", 
				_ => "", 
			};
		}

		private EMaterialLayerFilterMode StringToFilterMode(string String)
		{
			return String switch
			{
				"none" => EMaterialLayerFilterMode.None, 
				"transparent" => EMaterialLayerFilterMode.Transparent, 
				"blend" => EMaterialLayerFilterMode.Blend, 
				"additive" => EMaterialLayerFilterMode.Additive, 
				"additive_alpha" => EMaterialLayerFilterMode.AdditiveAlpha, 
				"modulate" => EMaterialLayerFilterMode.Modulate, 
				"modulate_2x" => EMaterialLayerFilterMode.Modulate2x, 
				_ => EMaterialLayerFilterMode.None, 
			};
		}
	}
}
