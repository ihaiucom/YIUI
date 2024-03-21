using System;
using MdxLib.Model;
using MdxLib.ModelFormats.Mdl.Token;

namespace MdxLib.ModelFormats.Mdl
{
	internal sealed class CModelVersion : CObject
	{
		private static class CSingleton
		{
			public static CModelVersion Instance;

			static CSingleton()
			{
				Instance = new CModelVersion();
			}
		}

		public static CModelVersion Instance => CSingleton.Instance;

		private CModelVersion()
		{
		}

		public void Load(CLoader Loader, MdxLib.Model.CModel Model)
		{
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
				if ((text2 = text) == null || !(text2 == "formatversion"))
				{
					break;
				}
				Model.Version = LoadInteger(Loader);
			}
			throw new Exception("Syntax error at line " + Loader.Line + ", unknown tag \"" + text + "\"!");
		}

		public void Save(CSaver Saver, MdxLib.Model.CModel Model)
		{
			Saver.BeginGroup("Version");
			SaveInteger(Saver, "FormatVersion", Model.Version);
			Saver.EndGroup();
		}
	}
}
