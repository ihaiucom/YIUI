using System;
using MdxLib.Model;

namespace MdxLib.ModelFormats.Mdx
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
			int num = Loader.ReadInt32();
			while (num > 0)
			{
				Loader.PushLocation();
				MdxLib.Model.CCollisionShape cCollisionShape = new MdxLib.Model.CCollisionShape(Model);
				Load(Loader, Model, cCollisionShape);
				Model.CollisionShapes.Add(cCollisionShape);
				num -= Loader.PopLocation();
				if (num < 0)
				{
					throw new Exception("Error at location " + Loader.Location + ", too many CollisionShape bytes were read!");
				}
			}
		}

		public void Load(CLoader Loader, MdxLib.Model.CModel Model, MdxLib.Model.CCollisionShape CollisionShape)
		{
			CNode.LoadNode(Loader, Model, CollisionShape);
			switch (Loader.ReadInt32())
			{
			case 0:
				CollisionShape.Type = ECollisionShapeType.Box;
				break;
			case 2:
				CollisionShape.Type = ECollisionShapeType.Sphere;
				break;
			}
			switch (CollisionShape.Type)
			{
			case ECollisionShapeType.Box:
				CollisionShape.Vertex1 = Loader.ReadVector3();
				CollisionShape.Vertex2 = Loader.ReadVector3();
				break;
			case ECollisionShapeType.Sphere:
				CollisionShape.Vertex1 = Loader.ReadVector3();
				CollisionShape.Radius = Loader.ReadFloat();
				break;
			}
		}

		public void SaveAll(CSaver Saver, MdxLib.Model.CModel Model)
		{
			if (!Model.HasCollisionShapes)
			{
				return;
			}
			Saver.WriteTag("CLID");
			Saver.PushLocation();
			foreach (MdxLib.Model.CCollisionShape collisionShape in Model.CollisionShapes)
			{
				Save(Saver, Model, collisionShape);
			}
			Saver.PopExclusiveLocation();
		}

		public void Save(CSaver Saver, MdxLib.Model.CModel Model, MdxLib.Model.CCollisionShape CollisionShape)
		{
			CNode.SaveNode(Saver, Model, CollisionShape, 8192);
			switch (CollisionShape.Type)
			{
			case ECollisionShapeType.Box:
				Saver.WriteInt32(0);
				Saver.WriteVector3(CollisionShape.Vertex1);
				Saver.WriteVector3(CollisionShape.Vertex2);
				break;
			case ECollisionShapeType.Sphere:
				Saver.WriteInt32(2);
				Saver.WriteVector3(CollisionShape.Vertex1);
				Saver.WriteFloat(CollisionShape.Radius);
				break;
			}
		}
	}
}
