using System;
using MdxLib.Model;
using MdxLib.ModelFormats.Mdx.Value;

namespace MdxLib.ModelFormats.Mdx
{
	internal sealed class CGeosetAnimation : CObject
	{
		private static class CSingleton
		{
			public static CGeosetAnimation Instance;

			static CSingleton()
			{
				Instance = new CGeosetAnimation();
			}
		}

		public static CGeosetAnimation Instance => CSingleton.Instance;

		private CGeosetAnimation()
		{
		}

		public void LoadAll(CLoader Loader, MdxLib.Model.CModel Model)
		{
			int num = Loader.ReadInt32();
			while (num > 0)
			{
				Loader.PushLocation();
				MdxLib.Model.CGeosetAnimation cGeosetAnimation = new MdxLib.Model.CGeosetAnimation(Model);
				Load(Loader, Model, cGeosetAnimation);
				Model.GeosetAnimations.Add(cGeosetAnimation);
				num -= Loader.PopLocation();
				if (num < 0)
				{
					throw new Exception("Error at location " + Loader.Location + ", too many GeosetAnimation bytes were read!");
				}
			}
		}

		public void Load(CLoader Loader, MdxLib.Model.CModel Model, MdxLib.Model.CGeosetAnimation GeosetAnimation)
		{
			Loader.PushLocation();
			int num = Loader.ReadInt32();
			GeosetAnimation.Alpha.MakeStatic(Loader.ReadFloat());
			int num2 = Loader.ReadInt32();
			GeosetAnimation.Color.MakeStatic(Loader.ReadVector3());
			Loader.Attacher.AddObject(Model.Geosets, GeosetAnimation.Geoset, Loader.ReadInt32());
			GeosetAnimation.DropShadow = (num2 & 1) != 0;
			GeosetAnimation.UseColor = (num2 & 2) != 0;
			num -= Loader.PopLocation();
			if (num < 0)
			{
				throw new Exception("Error at location " + Loader.Location + ", too many GeosetAnimation bytes were read!");
			}
			while (num > 0)
			{
				Loader.PushLocation();
				string text = Loader.ReadTag();
				switch (text)
				{
				case "KGAO":
					CObject.LoadAnimator(Loader, Model, GeosetAnimation.Alpha, CFloat.Instance);
					break;
				case "KGAC":
					CObject.LoadAnimator(Loader, Model, GeosetAnimation.Color, CVector3.Instance);
					break;
				default:
					throw new Exception("Error at location " + Loader.Location + ", unknown GeosetAnimation tag \"" + text + "\"!");
				}
				num -= Loader.PopLocation();
				if (num < 0)
				{
					throw new Exception("Error at location " + Loader.Location + ", too many GeosetAnimation bytes were read!");
				}
			}
		}

		public void SaveAll(CSaver Saver, MdxLib.Model.CModel Model)
		{
			if (!Model.HasGeosetAnimations)
			{
				return;
			}
			Saver.WriteTag("GEOA");
			Saver.PushLocation();
			foreach (MdxLib.Model.CGeosetAnimation geosetAnimation in Model.GeosetAnimations)
			{
				Save(Saver, Model, geosetAnimation);
			}
			Saver.PopExclusiveLocation();
		}

		public void Save(CSaver Saver, MdxLib.Model.CModel Model, MdxLib.Model.CGeosetAnimation GeosetAnimation)
		{
			int num = 0;
			if (GeosetAnimation.DropShadow)
			{
				num |= 1;
			}
			if (GeosetAnimation.UseColor)
			{
				num |= 2;
			}
			Saver.PushLocation();
			Saver.WriteFloat(GeosetAnimation.Alpha.GetValue());
			Saver.WriteInt32(num);
			Saver.WriteVector3(GeosetAnimation.Color.GetValue());
			Saver.WriteInt32(GeosetAnimation.Geoset.ObjectId);
			CObject.SaveAnimator(Saver, Model, GeosetAnimation.Alpha, CFloat.Instance, "KGAO");
			CObject.SaveAnimator(Saver, Model, GeosetAnimation.Color, CVector3.Instance, "KGAC");
			Saver.PopInclusiveLocation();
		}
	}
}
