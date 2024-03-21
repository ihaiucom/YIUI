using System;
using MdxLib.Model;

namespace MdxLib.ModelFormats.Mdx
{
	internal sealed class CTexture : CObject
	{
		private static class CSingleton
		{
			public static CTexture Instance;

			static CSingleton()
			{
				Instance = new CTexture();
			}
		}

		public static CTexture Instance => CSingleton.Instance;

		private CTexture()
		{
		}

		public void LoadAll(CLoader Loader, MdxLib.Model.CModel Model)
		{
			int num = Loader.ReadInt32();
			while (num > 0)
			{
				Loader.PushLocation();
				MdxLib.Model.CTexture cTexture = new MdxLib.Model.CTexture(Model);
				Load(Loader, Model, cTexture);
				Model.Textures.Add(cTexture);
				num -= Loader.PopLocation();
				if (num < 0)
				{
					throw new Exception("Error at location " + Loader.Location + ", too many Texture bytes were read!");
				}
			}
		}

		public void Load(CLoader Loader, MdxLib.Model.CModel Model, MdxLib.Model.CTexture Texture)
		{
			Texture.ReplaceableId = Loader.ReadInt32();
			Texture.FileName = Loader.ReadString(260);
			int num = Loader.ReadInt32();
			Texture.WrapWidth = (num & 1) != 0;
			Texture.WrapHeight = (num & 2) != 0;
		}

		public void SaveAll(CSaver Saver, MdxLib.Model.CModel Model)
		{
			if (!Model.HasTextures)
			{
				return;
			}
			Saver.WriteTag("TEXS");
			Saver.PushLocation();
			foreach (MdxLib.Model.CTexture texture in Model.Textures)
			{
				Save(Saver, Model, texture);
			}
			Saver.PopExclusiveLocation();
		}

		public void Save(CSaver Saver, MdxLib.Model.CModel Model, MdxLib.Model.CTexture Texture)
		{
			int num = 0;
			if (Texture.WrapWidth)
			{
				num |= 1;
			}
			if (Texture.WrapHeight)
			{
				num |= 2;
			}
			Saver.WriteInt32(Texture.ReplaceableId);
			Saver.WriteString(Texture.FileName, 260);
			Saver.WriteInt32(num);
		}
	}
}
