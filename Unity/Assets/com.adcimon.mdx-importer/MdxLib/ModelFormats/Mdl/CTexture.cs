using System;
using MdxLib.Model;
using MdxLib.ModelFormats.Mdl.Token;

namespace MdxLib.ModelFormats.Mdl
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
			Loader.ReadInteger();
			Loader.ExpectToken(EType.CurlyBracketLeft);
			string text;
			while (true)
			{
				if (Loader.PeekToken() == EType.CurlyBracketRight)
				{
					Loader.ReadToken();
					return;
				}
				text = Loader.ReadWord();
				string text2;
				if ((text2 = text) == null || !(text2 == "bitmap"))
				{
					break;
				}
				MdxLib.Model.CTexture cTexture = new MdxLib.Model.CTexture(Model);
				Load(Loader, Model, cTexture);
				Model.Textures.Add(cTexture);
			}
			throw new Exception("Syntax error at line " + Loader.Line + ", unknown tag \"" + text + "\"!");
		}

		public void Load(CLoader Loader, MdxLib.Model.CModel Model, MdxLib.Model.CTexture Texture)
		{
			Loader.ExpectToken(EType.CurlyBracketLeft);
			while (Loader.PeekToken() != EType.CurlyBracketRight)
			{
				string text = Loader.ReadWord();
				switch (text)
				{
				case "image":
					Texture.FileName = LoadString(Loader);
					continue;
				case "replaceableid":
					Texture.ReplaceableId = LoadInteger(Loader);
					continue;
				case "wrapwidth":
					Texture.WrapWidth = LoadBoolean(Loader);
					continue;
				case "wrapheight":
					Texture.WrapHeight = LoadBoolean(Loader);
					continue;
				}
				throw new Exception("Syntax error at line " + Loader.Line + ", unknown tag \"" + text + "\"!");
			}
			Loader.ReadToken();
		}

		public void SaveAll(CSaver Saver, MdxLib.Model.CModel Model)
		{
			if (!Model.HasTextures)
			{
				return;
			}
			Saver.BeginGroup("Textures", Model.Textures.Count);
			foreach (MdxLib.Model.CTexture texture in Model.Textures)
			{
				Save(Saver, Model, texture);
			}
			Saver.EndGroup();
		}

		public void Save(CSaver Saver, MdxLib.Model.CModel Model, MdxLib.Model.CTexture Texture)
		{
			Saver.BeginGroup("Bitmap");
			SaveString(Saver, "Image", Texture.FileName);
			SaveInteger(Saver, "ReplaceableId", Texture.ReplaceableId, ECondition.NotZero);
			SaveBoolean(Saver, "WrapWidth", Texture.WrapWidth);
			SaveBoolean(Saver, "WrapHeight", Texture.WrapHeight);
			Saver.EndGroup();
		}
	}
}
