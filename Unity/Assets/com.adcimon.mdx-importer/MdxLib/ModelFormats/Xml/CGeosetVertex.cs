using System.Xml;
using MdxLib.Model;

namespace MdxLib.ModelFormats.Xml
{
	internal sealed class CGeosetVertex : CObject
	{
		private static class CSingleton
		{
			public static CGeosetVertex Instance;

			static CSingleton()
			{
				Instance = new CGeosetVertex();
			}
		}

		public static CGeosetVertex Instance => CSingleton.Instance;

		private CGeosetVertex()
		{
		}

		public void Load(CLoader Loader, XmlNode Node, MdxLib.Model.CModel Model, MdxLib.Model.CGeoset Geoset, MdxLib.Model.CGeosetVertex GeosetVertex)
		{
			GeosetVertex.Position = ReadVector3(Node, "position", GeosetVertex.Position);
			GeosetVertex.Normal = ReadVector3(Node, "normal", GeosetVertex.Normal);
			GeosetVertex.TexturePosition = ReadVector2(Node, "texture_position", GeosetVertex.TexturePosition);
			Loader.Attacher.AddObject(Geoset.Groups, GeosetVertex.Group, ReadInteger(Node, "geoset_group", -1));
		}

		public void Save(CSaver Saver, XmlNode Node, MdxLib.Model.CModel Model, MdxLib.Model.CGeoset Geoset, MdxLib.Model.CGeosetVertex GeosetVertex)
		{
			WriteVector3(Node, "position", GeosetVertex.Position);
			WriteVector3(Node, "normal", GeosetVertex.Normal);
			WriteVector2(Node, "texture_position", GeosetVertex.TexturePosition);
			WriteInteger(Node, "geoset_group", GeosetVertex.Group.ObjectId);
		}
	}
}
