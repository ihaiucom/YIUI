using System;
using System.Collections.Generic;
using MdxLib.Model;
using MdxLib.Primitives;

namespace MdxLib.ModelFormats.Mdx
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

		public void LoadAll(CLoader Loader, MdxLib.Model.CModel Model)
		{
			int num = Loader.ReadInt32();
			while (num > 0)
			{
				Loader.PushLocation();
				MdxLib.Model.CGeoset cGeoset = new MdxLib.Model.CGeoset(Model);
				Load(Loader, Model, cGeoset);
				Model.Geosets.Add(cGeoset);
				num -= Loader.PopLocation();
				if (num < 0)
				{
					throw new Exception("Error at location " + Loader.Location + ", too many Geoset bytes were read!");
				}
			}
		}

		public void Load(CLoader Loader, MdxLib.Model.CModel Model, MdxLib.Model.CGeoset Geoset)
		{
			List<CVector3> list = new List<CVector3>();
			List<CVector3> list2 = new List<CVector3>();
			List<int> list3 = new List<int>();
			List<CVector2> list4 = new List<CVector2>();
			List<int> list5 = new List<int>();
			Loader.ReadInt32();
			Loader.ExpectTag("VRTX");
			int num = Loader.ReadInt32();
			for (int i = 0; i < num; i++)
			{
				list.Add(Loader.ReadVector3());
			}
			Loader.ExpectTag("NRMS");
			int num2 = Loader.ReadInt32();
			if (num2 != num)
			{
				throw new Exception("Error at location " + Loader.Location + ", vertex normal miscount (" + num2 + " normals, " + num + " positions)!");
			}
			for (int j = 0; j < num2; j++)
			{
				list2.Add(Loader.ReadVector3());
			}
			Loader.ExpectTag("PTYP");
			int num3 = Loader.ReadInt32();
			for (int k = 0; k < num3; k++)
			{
				int num4 = Loader.ReadInt32();
				if (num4 != 4)
				{
					throw new Exception("Error at location " + Loader.Location + ", unsupported Geoset face type (type " + num4 + ")!");
				}
			}
			Loader.ExpectTag("PCNT");
			int num5 = Loader.ReadInt32();
			for (int l = 0; l < num5; l++)
			{
				Loader.ReadInt32();
			}
			Loader.ExpectTag("PVTX");
			int num6 = Loader.ReadInt32();
			if (num6 % 3 != 0)
			{
				throw new Exception("Error at location " + Loader.Location + ", bad Geoset, nr of indexes not divisible by 3!");
			}
			int num7 = num6 / 3;
			for (int m = 0; m < num7; m++)
			{
				CGeosetFace cGeosetFace = new CGeosetFace(Model);
				Loader.Attacher.AddObject(Geoset.Vertices, cGeosetFace.Vertex1, Loader.ReadInt16());
				Loader.Attacher.AddObject(Geoset.Vertices, cGeosetFace.Vertex2, Loader.ReadInt16());
				Loader.Attacher.AddObject(Geoset.Vertices, cGeosetFace.Vertex3, Loader.ReadInt16());
				Geoset.Faces.Add(cGeosetFace);
			}
			Loader.ExpectTag("GNDX");
			int num8 = Loader.ReadInt32();
			if (num8 != num)
			{
				throw new Exception("Error at location " + Loader.Location + ", vertex group miscount (" + num8 + " groups, " + num + " positions)!");
			}
			for (int n = 0; n < num8; n++)
			{
				list3.Add(Loader.ReadInt8());
			}
			Loader.ExpectTag("MTGC");
			int num9 = Loader.ReadInt32();
			for (int num10 = 0; num10 < num9; num10++)
			{
				list5.Add(Loader.ReadInt32());
				Geoset.Groups.Add(new CGeosetGroup(Model));
			}
			Loader.ExpectTag("MATS");
			int num11 = Loader.ReadInt32();
			int num12 = -1;
			int num13 = 0;
			CGeosetGroup cGeosetGroup = null;
			for (int num14 = 0; num14 < num11; num14++)
			{
				if (num13 <= 0)
				{
					num12++;
					num13 = list5[num12];
					cGeosetGroup = Geoset.Groups[num12];
				}
				CGeosetGroupNode cGeosetGroupNode = new CGeosetGroupNode(Model);
				Loader.Attacher.AddNode(Model, cGeosetGroupNode.Node, Loader.ReadInt32());
				cGeosetGroup.Nodes.Add(cGeosetGroupNode);
				num13--;
			}
			Loader.Attacher.AddObject(Model.Materials, Geoset.Material, Loader.ReadInt32());
			int num15 = Loader.ReadInt32();
			Geoset.SelectionGroup = Loader.ReadInt32();
			Geoset.Extent = Loader.ReadExtent();
			int num16 = Loader.ReadInt32();
			for (int num17 = 0; num17 < num16; num17++)
			{
				CGeosetExtent cGeosetExtent = new CGeosetExtent(Model);
				cGeosetExtent.Extent = Loader.ReadExtent();
				Geoset.Extents.Add(cGeosetExtent);
			}
			Loader.ExpectTag("UVAS");
			Loader.ReadInt32();
			Loader.ExpectTag("UVBS");
			int num18 = Loader.ReadInt32();
			if (num18 != num)
			{
				throw new Exception("Error at location " + Loader.Location + ", vertex texture position miscount (" + num18 + " texture positions, " + num + " positions)!");
			}
			for (int num19 = 0; num19 < num18; num19++)
			{
				list4.Add(Loader.ReadVector2());
			}
			Geoset.Unselectable = (num15 & 4) != 0;
			for (int num20 = 0; num20 < num; num20++)
			{
				CGeosetVertex cGeosetVertex = new CGeosetVertex(Model);
				cGeosetVertex.Position = list[num20];
				cGeosetVertex.Normal = list2[num20];
				cGeosetVertex.TexturePosition = list4[num20];
				Loader.Attacher.AddObject(Geoset.Groups, cGeosetVertex.Group, list3[num20]);
				Geoset.Vertices.Add(cGeosetVertex);
			}
		}

		public void SaveAll(CSaver Saver, MdxLib.Model.CModel Model)
		{
			if (!Model.HasGeosets)
			{
				return;
			}
			Saver.WriteTag("GEOS");
			Saver.PushLocation();
			foreach (MdxLib.Model.CGeoset geoset in Model.Geosets)
			{
				Save(Saver, Model, geoset);
			}
			Saver.PopExclusiveLocation();
		}

		public void Save(CSaver Saver, MdxLib.Model.CModel Model, MdxLib.Model.CGeoset Geoset)
		{
			int num = 0;
			int num2 = 0;
			if (Geoset.Unselectable)
			{
				num |= 4;
			}
			Saver.PushLocation();
			Saver.WriteTag("VRTX");
			Saver.WriteInt32(Geoset.Vertices.Count);
			foreach (CGeosetVertex vertex in Geoset.Vertices)
			{
				Saver.WriteVector3(vertex.Position);
			}
			Saver.WriteTag("NRMS");
			Saver.WriteInt32(Geoset.Vertices.Count);
			foreach (CGeosetVertex vertex2 in Geoset.Vertices)
			{
				Saver.WriteVector3(vertex2.Normal);
			}
			Saver.WriteTag("PTYP");
			Saver.WriteInt32(1);
			Saver.WriteInt32(4);
			Saver.WriteTag("PCNT");
			Saver.WriteInt32(1);
			Saver.WriteInt32(Geoset.Faces.Count * 3);
			Saver.WriteTag("PVTX");
			Saver.WriteInt32(Geoset.Faces.Count * 3);
			foreach (CGeosetFace face in Geoset.Faces)
			{
				Saver.WriteInt16(face.Vertex1.ObjectId);
				Saver.WriteInt16(face.Vertex2.ObjectId);
				Saver.WriteInt16(face.Vertex3.ObjectId);
			}
			Saver.WriteTag("GNDX");
			Saver.WriteInt32(Geoset.Vertices.Count);
			foreach (CGeosetVertex vertex3 in Geoset.Vertices)
			{
				Saver.WriteInt8(vertex3.Group.ObjectId);
			}
			Saver.WriteTag("MTGC");
			Saver.WriteInt32(Geoset.Groups.Count);
			foreach (CGeosetGroup group in Geoset.Groups)
			{
				num2 += group.Nodes.Count;
				Saver.WriteInt32(group.Nodes.Count);
			}
			Saver.WriteTag("MATS");
			Saver.WriteInt32(num2);
			foreach (CGeosetGroup group2 in Geoset.Groups)
			{
				foreach (CGeosetGroupNode node in group2.Nodes)
				{
					Saver.WriteInt32(node.Node.ObjectId);
				}
			}
			Saver.WriteInt32(Geoset.Material.ObjectId);
			Saver.WriteInt32(num);
			Saver.WriteInt32(Geoset.SelectionGroup);
			Saver.WriteExtent(Geoset.Extent);
			Saver.WriteInt32(Geoset.Extents.Count);
			foreach (CGeosetExtent extent in Geoset.Extents)
			{
				Saver.WriteExtent(extent.Extent);
			}
			Saver.WriteTag("UVAS");
			Saver.WriteInt32(1);
			Saver.WriteTag("UVBS");
			Saver.WriteInt32(Geoset.Vertices.Count);
			foreach (CGeosetVertex vertex4 in Geoset.Vertices)
			{
				Saver.WriteVector2(vertex4.TexturePosition);
			}
			Saver.PopInclusiveLocation();
		}
	}
}
