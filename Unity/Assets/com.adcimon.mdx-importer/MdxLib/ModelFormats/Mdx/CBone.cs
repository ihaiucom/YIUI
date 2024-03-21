using System;
using MdxLib.Model;

namespace MdxLib.ModelFormats.Mdx
{
	internal sealed class CBone : CNode
	{
		private static class CSingleton
		{
			public static CBone Instance;

			static CSingleton()
			{
				Instance = new CBone();
			}
		}

		public static CBone Instance => CSingleton.Instance;

		private CBone()
		{
		}

		public void LoadAll(CLoader Loader, MdxLib.Model.CModel Model)
		{
			int num = Loader.ReadInt32();
			while (num > 0)
			{
				Loader.PushLocation();
				MdxLib.Model.CBone cBone = new MdxLib.Model.CBone(Model);
				Load(Loader, Model, cBone);
				Model.Bones.Add(cBone);
				num -= Loader.PopLocation();
				if (num < 0)
				{
					throw new Exception("Error at location " + Loader.Location + ", too many Bone bytes were read!");
				}
			}
		}

		public void Load(CLoader Loader, MdxLib.Model.CModel Model, MdxLib.Model.CBone Bone)
		{
			CNode.LoadNode(Loader, Model, Bone);
			Loader.Attacher.AddObject(Model.Geosets, Bone.Geoset, Loader.ReadInt32());
			Loader.Attacher.AddObject(Model.GeosetAnimations, Bone.GeosetAnimation, Loader.ReadInt32());
		}

		public void SaveAll(CSaver Saver, MdxLib.Model.CModel Model)
		{
			if (!Model.HasBones)
			{
				return;
			}
			Saver.WriteTag("BONE");
			Saver.PushLocation();
			foreach (MdxLib.Model.CBone bone in Model.Bones)
			{
				Save(Saver, Model, bone);
			}
			Saver.PopExclusiveLocation();
		}

		public void Save(CSaver Saver, MdxLib.Model.CModel Model, MdxLib.Model.CBone Bone)
		{
			CNode.SaveNode(Saver, Model, Bone, 256);
			Saver.WriteInt32(Bone.Geoset.ObjectId);
			Saver.WriteInt32(Bone.GeosetAnimation.ObjectId);
		}
	}
}
