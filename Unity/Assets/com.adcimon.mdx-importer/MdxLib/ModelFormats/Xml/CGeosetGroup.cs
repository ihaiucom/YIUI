using System.Xml;
using MdxLib.Model;

namespace MdxLib.ModelFormats.Xml
{
	internal sealed class CGeosetGroup : CObject
	{
		private static class CSingleton
		{
			public static CGeosetGroup Instance;

			static CSingleton()
			{
				Instance = new CGeosetGroup();
			}
		}

		public static CGeosetGroup Instance => CSingleton.Instance;

		private CGeosetGroup()
		{
		}

		public void Load(CLoader Loader, XmlNode Node, MdxLib.Model.CModel Model, MdxLib.Model.CGeoset Geoset, MdxLib.Model.CGeosetGroup GeosetGroup)
		{
			foreach (XmlNode item in Node.SelectNodes("geoset_group_node"))
			{
				MdxLib.Model.CGeosetGroupNode cGeosetGroupNode = new MdxLib.Model.CGeosetGroupNode(Model);
				CGeosetGroupNode.Instance.Load(Loader, item, Model, Geoset, GeosetGroup, cGeosetGroupNode);
				GeosetGroup.Nodes.Add(cGeosetGroupNode);
			}
		}

		public void Save(CSaver Saver, XmlNode Node, MdxLib.Model.CModel Model, MdxLib.Model.CGeoset Geoset, MdxLib.Model.CGeosetGroup GeosetGroup)
		{
			if (!GeosetGroup.HasNodes)
			{
				return;
			}
			foreach (MdxLib.Model.CGeosetGroupNode node2 in GeosetGroup.Nodes)
			{
				XmlElement node = AppendElement(Node, "geoset_group_node");
				CGeosetGroupNode.Instance.Save(Saver, node, Model, Geoset, GeosetGroup, node2);
			}
		}
	}
}
