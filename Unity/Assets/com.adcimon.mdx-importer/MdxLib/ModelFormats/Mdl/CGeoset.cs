using System;
using System.Collections.Generic;
using MdxLib.Model;
using MdxLib.ModelFormats.Mdl.Token;
using MdxLib.Primitives;

namespace MdxLib.ModelFormats.Mdl
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
			MdxLib.Model.CGeoset cGeoset = new MdxLib.Model.CGeoset(Model);
			Load(Loader, Model, cGeoset);
			Model.Geosets.Add(cGeoset);
		}

		public void Load(CLoader Loader, MdxLib.Model.CModel Model, MdxLib.Model.CGeoset Geoset)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			float radius = 0f;
			CVector3 min = CConstants.DefaultVector3;
			CVector3 max = CConstants.DefaultVector3;
			List<CVector3> list = new List<CVector3>();
			List<CVector3> list2 = new List<CVector3>();
			List<int> list3 = new List<int>();
			List<CVector2> list4 = new List<CVector2>();
			List<int> list5 = new List<int>();
			Loader.ExpectToken(EType.CurlyBracketLeft);
			while (Loader.PeekToken() != EType.CurlyBracketRight)
			{
				string text = Loader.ReadWord();
				switch (text)
				{
				case "minimumextent":
					min = LoadVector3(Loader);
					break;
				case "maximumextent":
					max = LoadVector3(Loader);
					break;
				case "boundsradius":
					radius = LoadFloat(Loader);
					break;
				case "materialid":
					Loader.Attacher.AddObject(Model.Materials, Geoset.Material, LoadId(Loader));
					break;
				case "selectiongroup":
					Geoset.SelectionGroup = LoadInteger(Loader);
					break;
				case "unselectable":
					Geoset.Unselectable = LoadBoolean(Loader);
					break;
				case "vertices":
				{
					num = Loader.ReadInteger();
					Loader.ExpectToken(EType.CurlyBracketLeft);
					for (int i = 0; i < num; i++)
					{
						list.Add(LoadVector3(Loader));
					}
					Loader.ExpectToken(EType.CurlyBracketRight);
					break;
				}
				case "normals":
				{
					num2 = Loader.ReadInteger();
					if (num2 != num)
					{
						throw new Exception("Vertex normal miscount at line " + Loader.Line + " (" + num2 + " normals, " + num + " positions)!");
					}
					Loader.ExpectToken(EType.CurlyBracketLeft);
					for (int l = 0; l < num2; l++)
					{
						list2.Add(LoadVector3(Loader));
					}
					Loader.ExpectToken(EType.CurlyBracketRight);
					break;
				}
				case "tvertices":
				{
					num4 = Loader.ReadInteger();
					if (num4 != num)
					{
						throw new Exception("Vertex texture position miscount at line " + Loader.Line + " (" + num4 + " texture positions, " + num + " positions)!");
					}
					Loader.ExpectToken(EType.CurlyBracketLeft);
					for (int n = 0; n < num4; n++)
					{
						list4.Add(LoadVector2(Loader));
					}
					Loader.ExpectToken(EType.CurlyBracketRight);
					break;
				}
				case "vertexgroup":
				{
					num3 = num;
					Loader.ExpectToken(EType.CurlyBracketLeft);
					for (int m = 0; m < num3; m++)
					{
						list3.Add(LoadInteger(Loader));
					}
					Loader.ExpectToken(EType.CurlyBracketRight);
					break;
				}
				case "faces":
				{
					Loader.ReadInteger();
					int num6 = Loader.ReadInteger();
					Loader.ExpectToken(EType.CurlyBracketLeft);
					Loader.ExpectWord("triangles");
					Loader.ExpectToken(EType.CurlyBracketLeft);
					Loader.ExpectToken(EType.CurlyBracketLeft);
					for (int k = 0; k < num6; k++)
					{
						list5.Add(Loader.ReadInteger());
						if (Loader.PeekToken() == EType.Separator)
						{
							Loader.ExpectToken(EType.Separator);
							continue;
						}
						Loader.ExpectToken(EType.CurlyBracketRight);
						Loader.ExpectToken(EType.Separator);
						if (k < num6 - 1)
						{
							Loader.ExpectToken(EType.CurlyBracketLeft);
						}
					}
					Loader.ExpectToken(EType.CurlyBracketRight);
					Loader.ExpectToken(EType.CurlyBracketRight);
					break;
				}
				case "groups":
				{
					int num5 = Loader.ReadInteger();
					Loader.ReadInteger();
					Loader.ExpectToken(EType.CurlyBracketLeft);
					for (int j = 0; j < num5; j++)
					{
						CGeosetGroup cGeosetGroup = new CGeosetGroup(Model);
						Geoset.Groups.Add(cGeosetGroup);
						Loader.ExpectWord("matrices");
						Loader.ExpectToken(EType.CurlyBracketLeft);
						while (true)
						{
							CGeosetGroupNode cGeosetGroupNode = new CGeosetGroupNode(Model);
							Loader.Attacher.AddNode(Model, cGeosetGroupNode.Node, Loader.ReadInteger());
							cGeosetGroup.Nodes.Add(cGeosetGroupNode);
							if (Loader.PeekToken() != EType.Separator)
							{
								break;
							}
							Loader.ExpectToken(EType.Separator);
						}
						Loader.ExpectToken(EType.CurlyBracketRight);
						Loader.ExpectToken(EType.Separator);
					}
					Loader.ExpectToken(EType.CurlyBracketRight);
					break;
				}
				case "anim":
				{
					float radius2 = 0f;
					CVector3 min2 = CConstants.DefaultVector3;
					CVector3 max2 = CConstants.DefaultVector3;
					Loader.ExpectToken(EType.CurlyBracketLeft);
					while (Loader.PeekToken() != EType.CurlyBracketRight)
					{
						text = Loader.ReadWord();
						switch (text)
						{
						case "minimumextent":
							min2 = LoadVector3(Loader);
							continue;
						case "maximumextent":
							max2 = LoadVector3(Loader);
							continue;
						case "boundsradius":
							radius2 = LoadFloat(Loader);
							continue;
						}
						throw new Exception("Syntax error at line " + Loader.Line + ", unknown tag \"" + text + "\"!");
					}
					Loader.ReadToken();
					CGeosetExtent cGeosetExtent = new CGeosetExtent(Model);
					cGeosetExtent.Extent = new CExtent(min2, max2, radius2);
					Geoset.Extents.Add(cGeosetExtent);
					break;
				}
				default:
					throw new Exception("Syntax error at line " + Loader.Line + ", unknown tag \"" + text + "\"!");
				}
			}
			Loader.ReadToken();
			Geoset.Extent = new CExtent(min, max, radius);
			for (int num7 = 0; num7 < num; num7++)
			{
				CGeosetVertex cGeosetVertex = new CGeosetVertex(Model);
				cGeosetVertex.Position = list[num7];
				cGeosetVertex.Normal = list2[num7];
				cGeosetVertex.TexturePosition = list4[num7];
				Loader.Attacher.AddObject(Geoset.Groups, cGeosetVertex.Group, list3[num7]);
				Geoset.Vertices.Add(cGeosetVertex);
			}
			if (list5.Count % 3 != 0)
			{
				throw new Exception("Bad Geoset at line " + Loader.Line + ", nr of indexes not divisible by 3!");
			}
			int num8 = list5.Count / 3;
			for (int num9 = 0; num9 < num8; num9++)
			{
				CGeosetFace cGeosetFace = new CGeosetFace(Model);
				Loader.Attacher.AddObject(Geoset.Vertices, cGeosetFace.Vertex1, list5[num9 * 3]);
				Loader.Attacher.AddObject(Geoset.Vertices, cGeosetFace.Vertex2, list5[num9 * 3 + 1]);
				Loader.Attacher.AddObject(Geoset.Vertices, cGeosetFace.Vertex3, list5[num9 * 3 + 2]);
				Geoset.Faces.Add(cGeosetFace);
			}
		}

		public void SaveAll(CSaver Saver, MdxLib.Model.CModel Model)
		{
			if (!Model.HasGeosets)
			{
				return;
			}
			foreach (MdxLib.Model.CGeoset geoset in Model.Geosets)
			{
				Save(Saver, Model, geoset);
			}
		}

		public void Save(CSaver Saver, MdxLib.Model.CModel Model, MdxLib.Model.CGeoset Geoset)
		{
			Saver.BeginGroup("Geoset");
			if (Geoset.Vertices.Count > 0)
			{
				Saver.BeginGroup("Vertices", Geoset.Vertices.Count);
				foreach (CGeosetVertex vertex in Geoset.Vertices)
				{
					Saver.WriteTabs();
					Saver.WriteVector3(vertex.Position);
					Saver.WriteLine(",");
				}
				Saver.EndGroup();
				Saver.BeginGroup("Normals", Geoset.Vertices.Count);
				foreach (CGeosetVertex vertex2 in Geoset.Vertices)
				{
					Saver.WriteTabs();
					Saver.WriteVector3(vertex2.Normal);
					Saver.WriteLine(",");
				}
				Saver.EndGroup();
				Saver.BeginGroup("TVertices", Geoset.Vertices.Count);
				foreach (CGeosetVertex vertex3 in Geoset.Vertices)
				{
					Saver.WriteTabs();
					Saver.WriteVector2(vertex3.TexturePosition);
					Saver.WriteLine(",");
				}
				Saver.EndGroup();
				Saver.BeginGroup("VertexGroup");
				foreach (CGeosetVertex vertex4 in Geoset.Vertices)
				{
					Saver.WriteTabs();
					Saver.WriteInteger(vertex4.Group.ObjectId);
					Saver.WriteLine(",");
				}
				Saver.EndGroup();
			}
			if (Geoset.Faces.Count > 0)
			{
				bool flag = true;
				Saver.BeginGroup("Faces", 1, Geoset.Faces.Count * 3);
				Saver.BeginGroup("Triangles");
				Saver.WriteTabs();
				Saver.WriteWord("{ ");
				foreach (CGeosetFace face in Geoset.Faces)
				{
					if (!flag)
					{
						Saver.WriteWord(", ");
					}
					Saver.WriteInteger(face.Vertex1.ObjectId);
					Saver.WriteWord(", ");
					Saver.WriteInteger(face.Vertex2.ObjectId);
					Saver.WriteWord(", ");
					Saver.WriteInteger(face.Vertex3.ObjectId);
					flag = false;
				}
				Saver.WriteLine(" },");
				Saver.EndGroup();
				Saver.EndGroup();
			}
			if (Geoset.Groups.Count > 0)
			{
				int num = 0;
				foreach (CGeosetGroup group in Geoset.Groups)
				{
					num += group.Nodes.Count;
				}
				Saver.BeginGroup("Groups", Geoset.Groups.Count, num);
				foreach (CGeosetGroup group2 in Geoset.Groups)
				{
					bool flag2 = true;
					Saver.WriteTabs();
					Saver.WriteWord("Matrices { ");
					foreach (CGeosetGroupNode node in group2.Nodes)
					{
						if (!flag2)
						{
							Saver.WriteWord(", ");
						}
						Saver.WriteInteger(node.Node.NodeId);
						flag2 = false;
					}
					Saver.WriteLine(" },");
				}
				Saver.EndGroup();
			}
			SaveVector3(Saver, "MinimumExtent", Geoset.Extent.Min, ECondition.NotZero);
			SaveVector3(Saver, "MaximumExtent", Geoset.Extent.Max, ECondition.NotZero);
			SaveFloat(Saver, "BoundsRadius", Geoset.Extent.Radius, ECondition.NotZero);
			foreach (CGeosetExtent extent in Geoset.Extents)
			{
				Saver.BeginGroup("Anim");
				SaveVector3(Saver, "MinimumExtent", extent.Extent.Min, ECondition.NotZero);
				SaveVector3(Saver, "MaximumExtent", extent.Extent.Max, ECondition.NotZero);
				SaveFloat(Saver, "BoundsRadius", extent.Extent.Radius, ECondition.NotZero);
				Saver.EndGroup();
			}
			SaveId(Saver, "MaterialID", Geoset.Material.ObjectId, ECondition.NotInvalidId);
			SaveInteger(Saver, "SelectionGroup", Geoset.SelectionGroup);
			SaveBoolean(Saver, "Unselectable", Geoset.Unselectable);
			Saver.EndGroup();
		}
	}
}
