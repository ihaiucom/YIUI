using System;
using MdxLib.Model;
using MdxLib.ModelFormats.Mdl.Token;

namespace MdxLib.ModelFormats.Mdl
{
	internal sealed class CHelper : CNode
	{
		private static class CSingleton
		{
			public static CHelper Instance;

			static CSingleton()
			{
				Instance = new CHelper();
			}
		}

		public static CHelper Instance => CSingleton.Instance;

		private CHelper()
		{
		}

		public void LoadAll(CLoader Loader, MdxLib.Model.CModel Model)
		{
			MdxLib.Model.CHelper cHelper = new MdxLib.Model.CHelper(Model);
			Load(Loader, Model, cHelper);
			Model.Helpers.Add(cHelper);
		}

		public void Load(CLoader Loader, MdxLib.Model.CModel Model, MdxLib.Model.CHelper Helper)
		{
			Helper.Name = Loader.ReadString();
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
				if (!LoadNode(Loader, Model, Helper, text))
				{
					string text2;
					if ((text2 = text) == null || !(text2 == "static"))
					{
						break;
					}
					text = Loader.ReadWord();
					if (!LoadStaticNode(Loader, Model, Helper, text))
					{
						throw new Exception("Syntax error at line " + Loader.Line + ", unknown tag \"" + text + "\"!");
					}
				}
			}
			throw new Exception("Syntax error at line " + Loader.Line + ", unknown tag \"" + text + "\"!");
		}

		public void SaveAll(CSaver Saver, MdxLib.Model.CModel Model)
		{
			if (!Model.HasHelpers)
			{
				return;
			}
			foreach (MdxLib.Model.CHelper helper in Model.Helpers)
			{
				Save(Saver, Model, helper);
			}
		}

		public void Save(CSaver Saver, MdxLib.Model.CModel Model, MdxLib.Model.CHelper Helper)
		{
			Saver.BeginGroup("Helper", Helper.Name);
			SaveNode(Saver, Model, Helper);
			Saver.EndGroup();
		}
	}
}
