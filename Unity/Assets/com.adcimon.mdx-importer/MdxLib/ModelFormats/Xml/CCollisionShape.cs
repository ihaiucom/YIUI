using System.Xml;
using MdxLib.Model;

namespace MdxLib.ModelFormats.Xml
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

		public void Load(CLoader Loader, XmlNode Node, MdxLib.Model.CModel Model, MdxLib.Model.CCollisionShape CollisionShape)
		{
			LoadNode(Loader, Node, Model, CollisionShape);
			CollisionShape.Type = StringToType(ReadString(Node, "type", TypeToString(CollisionShape.Type)));
			CollisionShape.Radius = ReadFloat(Node, "radius", CollisionShape.Radius);
			CollisionShape.Vertex1 = ReadVector3(Node, "vertex_1", CollisionShape.Vertex1);
			CollisionShape.Vertex2 = ReadVector3(Node, "vertex_2", CollisionShape.Vertex2);
		}

		public void Save(CSaver Saver, XmlNode Node, MdxLib.Model.CModel Model, MdxLib.Model.CCollisionShape CollisionShape)
		{
			SaveNode(Saver, Node, Model, CollisionShape);
			WriteString(Node, "type", TypeToString(CollisionShape.Type));
			WriteFloat(Node, "radius", CollisionShape.Radius);
			WriteVector3(Node, "vertex_1", CollisionShape.Vertex1);
			WriteVector3(Node, "vertex_2", CollisionShape.Vertex2);
		}

		private string TypeToString(ECollisionShapeType Type)
		{
			return Type switch
			{
				ECollisionShapeType.Box => "box", 
				ECollisionShapeType.Sphere => "sphere", 
				_ => "", 
			};
		}

		private ECollisionShapeType StringToType(string String)
		{
			return String switch
			{
				"box" => ECollisionShapeType.Box, 
				"sphere" => ECollisionShapeType.Sphere, 
				_ => ECollisionShapeType.Box, 
			};
		}
	}
}
