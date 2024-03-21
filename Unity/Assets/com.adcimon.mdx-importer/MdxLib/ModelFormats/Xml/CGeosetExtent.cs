using System.Xml;
using MdxLib.Model;

namespace MdxLib.ModelFormats.Xml
{
	internal sealed class CGeosetExtent : CObject
	{
		private static class CSingleton
		{
			public static CGeosetExtent Instance;

			static CSingleton()
			{
				Instance = new CGeosetExtent();
			}
		}

		public static CGeosetExtent Instance => CSingleton.Instance;

		private CGeosetExtent()
		{
		}

		public void Load(CLoader Loader, XmlNode Node, MdxLib.Model.CModel Model, MdxLib.Model.CGeoset Geoset, MdxLib.Model.CGeosetExtent GeosetExtent)
		{
			GeosetExtent.Extent = ReadExtent(Node, "extent", GeosetExtent.Extent);
		}

		public void Save(CSaver Saver, XmlNode Node, MdxLib.Model.CModel Model, MdxLib.Model.CGeoset Geoset, MdxLib.Model.CGeosetExtent GeosetExtent)
		{
			WriteExtent(Node, "extent", GeosetExtent.Extent);
		}
	}
}
