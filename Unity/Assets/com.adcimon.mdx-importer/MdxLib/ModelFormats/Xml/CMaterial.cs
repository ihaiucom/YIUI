using System.Xml;
using MdxLib.Model;

namespace MdxLib.ModelFormats.Xml
{
	internal sealed class CMaterial : CObject
	{
		private static class CSingleton
		{
			public static CMaterial Instance;

			static CSingleton()
			{
				Instance = new CMaterial();
			}
		}

		public static CMaterial Instance => CSingleton.Instance;

		private CMaterial()
		{
		}

		public void Load(CLoader Loader, XmlNode Node, MdxLib.Model.CModel Model, MdxLib.Model.CMaterial Material)
		{
			Material.PriorityPlane = ReadInteger(Node, "priority_plane", Material.PriorityPlane);
			Material.ConstantColor = ReadBoolean(Node, "constant_color", Material.ConstantColor);
			Material.FullResolution = ReadBoolean(Node, "full_resolution", Material.FullResolution);
			Material.SortPrimitivesFarZ = ReadBoolean(Node, "sort_primitives_far_z", Material.SortPrimitivesFarZ);
			Material.SortPrimitivesNearZ = ReadBoolean(Node, "sort_primitives_near_z", Material.SortPrimitivesNearZ);
			foreach (XmlNode item in Node.SelectNodes("material_layer"))
			{
				MdxLib.Model.CMaterialLayer cMaterialLayer = new MdxLib.Model.CMaterialLayer(Model);
				CMaterialLayer.Instance.Load(Loader, item, Model, Material, cMaterialLayer);
				Material.Layers.Add(cMaterialLayer);
			}
		}

		public void Save(CSaver Saver, XmlNode Node, MdxLib.Model.CModel Model, MdxLib.Model.CMaterial Material)
		{
			WriteInteger(Node, "priority_plane", Material.PriorityPlane);
			WriteBoolean(Node, "constant_color", Material.ConstantColor);
			WriteBoolean(Node, "full_resolution", Material.FullResolution);
			WriteBoolean(Node, "sort_primitives_far_z", Material.SortPrimitivesFarZ);
			WriteBoolean(Node, "sort_primitives_near_z", Material.SortPrimitivesNearZ);
			if (!Material.HasLayers)
			{
				return;
			}
			foreach (MdxLib.Model.CMaterialLayer layer in Material.Layers)
			{
				XmlElement node = AppendElement(Node, "material_layer");
				CMaterialLayer.Instance.Save(Saver, node, Model, Material, layer);
			}
		}
	}
}
