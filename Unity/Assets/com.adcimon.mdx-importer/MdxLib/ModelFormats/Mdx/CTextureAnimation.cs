using System;
using MdxLib.Model;
using MdxLib.ModelFormats.Mdx.Value;

namespace MdxLib.ModelFormats.Mdx
{
	internal sealed class CTextureAnimation : CObject
	{
		private static class CSingleton
		{
			public static CTextureAnimation Instance;

			static CSingleton()
			{
				Instance = new CTextureAnimation();
			}
		}

		public static CTextureAnimation Instance => CSingleton.Instance;

		private CTextureAnimation()
		{
		}

		public void LoadAll(CLoader Loader, MdxLib.Model.CModel Model)
		{
			int num = Loader.ReadInt32();
			while (num > 0)
			{
				Loader.PushLocation();
				MdxLib.Model.CTextureAnimation cTextureAnimation = new MdxLib.Model.CTextureAnimation(Model);
				Load(Loader, Model, cTextureAnimation);
				Model.TextureAnimations.Add(cTextureAnimation);
				num -= Loader.PopLocation();
				if (num < 0)
				{
					throw new Exception("Error at location " + Loader.Location + ", too many TextureAnimation bytes were read!");
				}
			}
		}

		public void Load(CLoader Loader, MdxLib.Model.CModel Model, MdxLib.Model.CTextureAnimation TextureAnimation)
		{
			Loader.PushLocation();
			int num = Loader.ReadInt32();
			num -= Loader.PopLocation();
			if (num < 0)
			{
				throw new Exception("Error at location " + Loader.Location + ", too many TextureAnimation bytes were read!");
			}
			while (num > 0)
			{
				Loader.PushLocation();
				string text = Loader.ReadTag();
				switch (text)
				{
				case "KTAT":
					CObject.LoadAnimator(Loader, Model, TextureAnimation.Translation, CVector3.Instance);
					break;
				case "KTAR":
					CObject.LoadAnimator(Loader, Model, TextureAnimation.Rotation, CVector4.Instance);
					break;
				case "KTAS":
					CObject.LoadAnimator(Loader, Model, TextureAnimation.Scaling, CVector3.Instance);
					break;
				default:
					throw new Exception("Error at location " + Loader.Location + ", unknown TextureAnimation tag \"" + text + "\"!");
				}
				num -= Loader.PopLocation();
				if (num < 0)
				{
					throw new Exception("Error at location " + Loader.Location + ", too many TextureAnimation bytes were read!");
				}
			}
		}

		public void SaveAll(CSaver Saver, MdxLib.Model.CModel Model)
		{
			if (!Model.HasTextureAnimations)
			{
				return;
			}
			Saver.WriteTag("TXAN");
			Saver.PushLocation();
			foreach (MdxLib.Model.CTextureAnimation textureAnimation in Model.TextureAnimations)
			{
				Save(Saver, Model, textureAnimation);
			}
			Saver.PopExclusiveLocation();
		}

		public void Save(CSaver Saver, MdxLib.Model.CModel Model, MdxLib.Model.CTextureAnimation TextureAnimation)
		{
			Saver.PushLocation();
			CObject.SaveAnimator(Saver, Model, TextureAnimation.Translation, CVector3.Instance, "KTAT");
			CObject.SaveAnimator(Saver, Model, TextureAnimation.Rotation, CVector4.Instance, "KTAR");
			CObject.SaveAnimator(Saver, Model, TextureAnimation.Scaling, CVector3.Instance, "KTAS");
			Saver.PopInclusiveLocation();
		}
	}
}
