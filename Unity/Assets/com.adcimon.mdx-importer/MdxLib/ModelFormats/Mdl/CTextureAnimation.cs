using System;
using MdxLib.Model;
using MdxLib.ModelFormats.Mdl.Token;
using MdxLib.ModelFormats.Mdl.Value;

namespace MdxLib.ModelFormats.Mdl
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
				if ((text2 = text) == null || !(text2 == "tvertexanim"))
				{
					break;
				}
				MdxLib.Model.CTextureAnimation cTextureAnimation = new MdxLib.Model.CTextureAnimation(Model);
				Load(Loader, Model, cTextureAnimation);
				Model.TextureAnimations.Add(cTextureAnimation);
			}
			throw new Exception("Syntax error at line " + Loader.Line + ", unknown tag \"" + text + "\"!");
		}

		public void Load(CLoader Loader, MdxLib.Model.CModel Model, MdxLib.Model.CTextureAnimation TextureAnimation)
		{
			Loader.ExpectToken(EType.CurlyBracketLeft);
			while (Loader.PeekToken() != EType.CurlyBracketRight)
			{
				string text = Loader.ReadWord();
				switch (text)
				{
				case "static":
					text = Loader.ReadWord();
					switch (text)
					{
					case "translation":
						LoadStaticAnimator(Loader, Model, TextureAnimation.Translation, CVector3.Instance);
						break;
					case "rotation":
						LoadStaticAnimator(Loader, Model, TextureAnimation.Rotation, CVector4.Instance);
						break;
					case "scaling":
						LoadStaticAnimator(Loader, Model, TextureAnimation.Scaling, CVector3.Instance);
						break;
					default:
						throw new Exception("Syntax error at line " + Loader.Line + ", unknown tag \"" + text + "\"!");
					}
					break;
				case "translation":
					LoadAnimator(Loader, Model, TextureAnimation.Translation, CVector3.Instance);
					break;
				case "rotation":
					LoadAnimator(Loader, Model, TextureAnimation.Rotation, CVector4.Instance);
					break;
				case "scaling":
					LoadAnimator(Loader, Model, TextureAnimation.Scaling, CVector3.Instance);
					break;
				default:
					throw new Exception("Syntax error at line " + Loader.Line + ", unknown tag \"" + text + "\"!");
				}
			}
			Loader.ReadToken();
		}

		public void SaveAll(CSaver Saver, MdxLib.Model.CModel Model)
		{
			if (!Model.HasTextureAnimations)
			{
				return;
			}
			Saver.BeginGroup("TextureAnims", Model.TextureAnimations.Count);
			foreach (MdxLib.Model.CTextureAnimation textureAnimation in Model.TextureAnimations)
			{
				Save(Saver, Model, textureAnimation);
			}
			Saver.EndGroup();
		}

		public void Save(CSaver Saver, MdxLib.Model.CModel Model, MdxLib.Model.CTextureAnimation TextureAnimation)
		{
			Saver.BeginGroup("TVertexAnim");
			SaveAnimator(Saver, Model, TextureAnimation.Translation, CVector3.Instance, "Translation", ECondition.NotZero);
			SaveAnimator(Saver, Model, TextureAnimation.Rotation, CVector4.Instance, "Rotation", ECondition.NotDefaultQuaternion);
			SaveAnimator(Saver, Model, TextureAnimation.Scaling, CVector3.Instance, "Scaling", ECondition.NotOne);
			Saver.EndGroup();
		}
	}
}
