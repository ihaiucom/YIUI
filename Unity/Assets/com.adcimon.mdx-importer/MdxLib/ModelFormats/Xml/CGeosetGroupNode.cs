using System.Xml;
using MdxLib.Model;

namespace MdxLib.ModelFormats.Xml
{
	internal sealed class CGeosetGroupNode : CObject
	{
		private static class CSingleton
		{
			public static CGeosetGroupNode Instance;

			static CSingleton()
			{
				Instance = new CGeosetGroupNode();
			}
		}

		public static CGeosetGroupNode Instance => CSingleton.Instance;

		private CGeosetGroupNode()
		{
		}

		public void Load(CLoader Loader, XmlNode Node, MdxLib.Model.CModel Model, MdxLib.Model.CGeoset Geoset, MdxLib.Model.CGeosetGroup GeosetGroup, MdxLib.Model.CGeosetGroupNode GeosetGroupNode)
		{
			Loader.Attacher.AddNode(Model, GeosetGroupNode.Node, ReadInteger(Node, "node", -1));
		}

		public void Save(CSaver Saver, XmlNode Node, MdxLib.Model.CModel Model, MdxLib.Model.CGeoset Geoset, MdxLib.Model.CGeosetGroup GeosetGroup, MdxLib.Model.CGeosetGroupNode GeosetGroupNode)
		{
			WriteInteger(Node, "node", GeosetGroupNode.Node.NodeId);
		}
	}
}
