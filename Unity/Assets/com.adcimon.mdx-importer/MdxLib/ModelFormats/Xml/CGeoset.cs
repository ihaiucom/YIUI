using System.Xml;
using MdxLib.Model;

namespace MdxLib.ModelFormats.Xml
{
	internal sealed class CGeoset : CObject
	{
		private static class CSingleton
		{
			public static CGeoset Instance;

			static CSingleton()
			{
				Instance = new CGeoset();
			}
		}

		public static CGeoset Instance => CSingleton.Instance;

		private CGeoset()
		{
		}

		public void Load(CLoader Loader, XmlNode Node, MdxLib.Model.CModel Model, MdxLib.Model.CGeoset Geoset)
		{
			Geoset.SelectionGroup = ReadInteger(Node, "selection_group", Geoset.SelectionGroup);
			Geoset.Unselectable = ReadBoolean(Node, "unselectable", Geoset.Unselectable);
			Geoset.Extent = ReadExtent(Node, "extent", Geoset.Extent);
			Loader.Attacher.AddObject(Model.Materials, Geoset.Material, ReadInteger(Node, "material", -1));
			foreach (XmlNode item in Node.SelectNodes("geoset_vertex"))
			{
				MdxLib.Model.CGeosetVertex cGeosetVertex = new MdxLib.Model.CGeosetVertex(Model);
				CGeosetVertex.Instance.Load(Loader, item, Model, Geoset, cGeosetVertex);
				Geoset.Vertices.Add(cGeosetVertex);
			}
			foreach (XmlNode item2 in Node.SelectNodes("geoset_face"))
			{
				MdxLib.Model.CGeosetFace cGeosetFace = new MdxLib.Model.CGeosetFace(Model);
				CGeosetFace.Instance.Load(Loader, item2, Model, Geoset, cGeosetFace);
				Geoset.Faces.Add(cGeosetFace);
			}
			foreach (XmlNode item3 in Node.SelectNodes("geoset_group"))
			{
				MdxLib.Model.CGeosetGroup cGeosetGroup = new MdxLib.Model.CGeosetGroup(Model);
				CGeosetGroup.Instance.Load(Loader, item3, Model, Geoset, cGeosetGroup);
				Geoset.Groups.Add(cGeosetGroup);
			}
			foreach (XmlNode item4 in Node.SelectNodes("geoset_extent"))
			{
				MdxLib.Model.CGeosetExtent cGeosetExtent = new MdxLib.Model.CGeosetExtent(Model);
				CGeosetExtent.Instance.Load(Loader, item4, Model, Geoset, cGeosetExtent);
				Geoset.Extents.Add(cGeosetExtent);
			}
		}

		public void Save(CSaver Saver, XmlNode Node, MdxLib.Model.CModel Model, MdxLib.Model.CGeoset Geoset)
		{
			WriteInteger(Node, "selection_group", Geoset.SelectionGroup);
			WriteBoolean(Node, "unselectable", Geoset.Unselectable);
			WriteExtent(Node, "extent", Geoset.Extent);
			WriteInteger(Node, "material", Geoset.Material.ObjectId);
			if (Geoset.HasVertices)
			{
				foreach (MdxLib.Model.CGeosetVertex vertex in Geoset.Vertices)
				{
					XmlElement node = AppendElement(Node, "geoset_vertex");
					CGeosetVertex.Instance.Save(Saver, node, Model, Geoset, vertex);
				}
			}
			if (Geoset.HasFaces)
			{
				foreach (MdxLib.Model.CGeosetFace face in Geoset.Faces)
				{
					XmlElement node2 = AppendElement(Node, "geoset_face");
					CGeosetFace.Instance.Save(Saver, node2, Model, Geoset, face);
				}
			}
			if (Geoset.HasGroups)
			{
				foreach (MdxLib.Model.CGeosetGroup group in Geoset.Groups)
				{
					XmlElement node3 = AppendElement(Node, "geoset_group");
					CGeosetGroup.Instance.Save(Saver, node3, Model, Geoset, group);
				}
			}
			if (!Geoset.HasExtents)
			{
				return;
			}
			foreach (MdxLib.Model.CGeosetExtent extent in Geoset.Extents)
			{
				XmlElement node4 = AppendElement(Node, "geoset_extent");
				CGeosetExtent.Instance.Save(Saver, node4, Model, Geoset, extent);
			}
		}
	}
}
