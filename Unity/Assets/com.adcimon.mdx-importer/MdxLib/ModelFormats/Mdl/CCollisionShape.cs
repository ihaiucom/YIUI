using System;
using MdxLib.Model;
using MdxLib.ModelFormats.Mdl.Token;

namespace MdxLib.ModelFormats.Mdl
{
	internal sealed class CCollisionShape : CNode
	{
		private static class CSingleton
		{
			public static CCollisionShape Instance;

			static CSingleton()
			{
				Instance = new CCollisionShape();
			}
		}

		public static CCollisionShape Instance => CSingleton.Instance;

		private CCollisionShape()
		{
		}

		public void LoadAll(CLoader Loader, MdxLib.Model.CModel Model)
		{
			MdxLib.Model.CCollisionShape cCollisionShape = new MdxLib.Model.CCollisionShape(Model);
			Load(Loader, Model, cCollisionShape);
			Model.CollisionShapes.Add(cCollisionShape);
		}

		public void Load(CLoader Loader, MdxLib.Model.CModel Model, MdxLib.Model.CCollisionShape CollisionShape)
		{
			CollisionShape.Name = Loader.ReadString();
			Loader.ExpectToken(EType.CurlyBracketLeft);
			while (Loader.PeekToken() != EType.CurlyBracketRight)
			{
				string text = Loader.ReadWord();
				if (LoadNode(Loader, Model, CollisionShape, text))
				{
					continue;
				}
				switch (text)
				{
				case "static":
					text = Loader.ReadWord();
					if (!LoadStaticNode(Loader, Model, CollisionShape, text))
					{
						throw new Exception("Syntax error at line " + Loader.Line + ", unknown tag \"" + text + "\"!");
					}
					break;
				case "box":
					CollisionShape.Type = ECollisionShapeType.Box;
					LoadBoolean(Loader);
					break;
				case "sphere":
					CollisionShape.Type = ECollisionShapeType.Sphere;
					LoadBoolean(Loader);
					break;
				case "boundsradius":
					CollisionShape.Radius = LoadFloat(Loader);
					break;
				case "vertices":
				{
					int num = Loader.ReadInteger();
					Loader.ExpectToken(EType.CurlyBracketLeft);
					switch (num)
					{
					case 1:
						CollisionShape.Vertex1 = LoadVector3(Loader);
						break;
					case 2:
						CollisionShape.Vertex1 = LoadVector3(Loader);
						CollisionShape.Vertex2 = LoadVector3(Loader);
						break;
					default:
						throw new Exception("Bad vertex count at line " + Loader.Line + ", got " + num + " vertices!");
					}
					Loader.ExpectToken(EType.CurlyBracketRight);
					break;
				}
				default:
					throw new Exception("Syntax error at line " + Loader.Line + ", unknown tag \"" + text + "\"!");
				}
			}
			Loader.ReadToken();
		}

		public void SaveAll(CSaver Saver, MdxLib.Model.CModel Model)
		{
			if (!Model.HasCollisionShapes)
			{
				return;
			}
			foreach (MdxLib.Model.CCollisionShape collisionShape in Model.CollisionShapes)
			{
				Save(Saver, Model, collisionShape);
			}
		}

		public void Save(CSaver Saver, MdxLib.Model.CModel Model, MdxLib.Model.CCollisionShape CollisionShape)
		{
			Saver.BeginGroup("CollisionShape", CollisionShape.Name);
			SaveNode(Saver, Model, CollisionShape);
			SaveBoolean(Saver, TypeToString(CollisionShape.Type), Value: true);
			switch (CollisionShape.Type)
			{
			case ECollisionShapeType.Box:
				Saver.BeginGroup("Vertices", 2);
				Saver.WriteTabs();
				Saver.WriteVector3(CollisionShape.Vertex1);
				Saver.WriteLine(",");
				Saver.WriteTabs();
				Saver.WriteVector3(CollisionShape.Vertex2);
				Saver.WriteLine(",");
				Saver.EndGroup();
				break;
			case ECollisionShapeType.Sphere:
				Saver.BeginGroup("Vertices", 1);
				Saver.WriteTabs();
				Saver.WriteVector3(CollisionShape.Vertex1);
				Saver.WriteLine(",");
				Saver.EndGroup();
				SaveFloat(Saver, "BoundsRadius", CollisionShape.Radius, ECondition.NotZero);
				break;
			}
			Saver.EndGroup();
		}

		private string TypeToString(ECollisionShapeType Type)
		{
			return Type switch
			{
				ECollisionShapeType.Box => "Box", 
				ECollisionShapeType.Sphere => "Sphere", 
				_ => "", 
			};
		}
	}
}
