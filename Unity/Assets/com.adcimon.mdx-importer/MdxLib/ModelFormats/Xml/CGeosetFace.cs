using System.Xml;
using MdxLib.Model;

namespace MdxLib.ModelFormats.Xml
{
	internal sealed class CGeosetFace : CObject
	{
		private static class CSingleton
		{
			public static CGeosetFace Instance;

			static CSingleton()
			{
				Instance = new CGeosetFace();
			}
		}

		public static CGeosetFace Instance => CSingleton.Instance;

		private CGeosetFace()
		{
		}

		public void Load(CLoader Loader, XmlNode Node, MdxLib.Model.CModel Model, MdxLib.Model.CGeoset Geoset, MdxLib.Model.CGeosetFace GeosetFace)
		{
			Loader.Attacher.AddObject(Geoset.Vertices, GeosetFace.Vertex1, ReadInteger(Node, "vertex_1", -1));
			Loader.Attacher.AddObject(Geoset.Vertices, GeosetFace.Vertex2, ReadInteger(Node, "vertex_2", -1));
			Loader.Attacher.AddObject(Geoset.Vertices, GeosetFace.Vertex3, ReadInteger(Node, "vertex_3", -1));
		}

		public void Save(CSaver Saver, XmlNode Node, MdxLib.Model.CModel Model, MdxLib.Model.CGeoset Geoset, MdxLib.Model.CGeosetFace GeosetFace)
		{
			WriteInteger(Node, "vertex_1", GeosetFace.Vertex1.ObjectId);
			WriteInteger(Node, "vertex_2", GeosetFace.Vertex2.ObjectId);
			WriteInteger(Node, "vertex_3", GeosetFace.Vertex3.ObjectId);
		}
	}
}
