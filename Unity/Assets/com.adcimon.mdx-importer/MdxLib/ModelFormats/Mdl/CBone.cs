using System;
using MdxLib.Model;
using MdxLib.ModelFormats.Mdl.Token;

namespace MdxLib.ModelFormats.Mdl
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
			MdxLib.Model.CBone cBone = new MdxLib.Model.CBone(Model);
			Load(Loader, Model, cBone);
			Model.Bones.Add(cBone);
		}

		public void Load(CLoader Loader, MdxLib.Model.CModel Model, MdxLib.Model.CBone Bone)
		{
			Bone.Name = Loader.ReadString();
			Loader.ExpectToken(EType.CurlyBracketLeft);
			while (Loader.PeekToken() != EType.CurlyBracketRight)
			{
				string text = Loader.ReadWord();
				if (LoadNode(Loader, Model, Bone, text))
				{
					continue;
				}
				switch (text)
				{
				case "static":
					text = Loader.ReadWord();
					if (!LoadStaticNode(Loader, Model, Bone, text))
					{
						throw new Exception("Syntax error at line " + Loader.Line + ", unknown tag \"" + text + "\"!");
					}
					break;
				case "geosetid":
					Loader.Attacher.AddObject(Model.Geosets, Bone.Geoset, LoadId(Loader));
					break;
				case "geosetanimid":
					Loader.Attacher.AddObject(Model.GeosetAnimations, Bone.GeosetAnimation, LoadId(Loader));
					break;
				default:
					throw new Exception("Syntax error at line " + Loader.Line + ", unknown tag \"" + text + "\"!");
				}
			}
			Loader.ReadToken();
		}

		public void SaveAll(CSaver Saver, MdxLib.Model.CModel Model)
		{
			if (!Model.HasBones)
			{
				return;
			}
			foreach (MdxLib.Model.CBone bone in Model.Bones)
			{
				Save(Saver, Model, bone);
			}
		}

		public void Save(CSaver Saver, MdxLib.Model.CModel Model, MdxLib.Model.CBone Bone)
		{
			Saver.BeginGroup("Bone", Bone.Name);
			SaveNode(Saver, Model, Bone);
			SaveId(Saver, "GeosetId", Bone.Geoset.ObjectId, UseMultipleAsNone: true);
			SaveId(Saver, "GeosetAnimId", Bone.GeosetAnimation.ObjectId, UseMultipleAsNone: false);
			Saver.EndGroup();
		}
	}
}
