using System;
using MdxLib.Model;
using MdxLib.ModelFormats.Mdl.Token;

namespace MdxLib.ModelFormats.Mdl
{
	internal sealed class CGlobalSequence : CObject
	{
		private static class CSingleton
		{
			public static CGlobalSequence Instance;

			static CSingleton()
			{
				Instance = new CGlobalSequence();
			}
		}

		public static CGlobalSequence Instance => CSingleton.Instance;

		private CGlobalSequence()
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
				if ((text2 = text) == null || !(text2 == "duration"))
				{
					break;
				}
				MdxLib.Model.CGlobalSequence cGlobalSequence = new MdxLib.Model.CGlobalSequence(Model);
				Load(Loader, Model, cGlobalSequence);
				Model.GlobalSequences.Add(cGlobalSequence);
			}
			throw new Exception("Syntax error at line " + Loader.Line + ", unknown tag \"" + text + "\"!");
		}

		public void Load(CLoader Loader, MdxLib.Model.CModel Model, MdxLib.Model.CGlobalSequence GlobalSequence)
		{
			GlobalSequence.Duration = LoadInteger(Loader);
		}

		public void SaveAll(CSaver Saver, MdxLib.Model.CModel Model)
		{
			if (!Model.HasGlobalSequences)
			{
				return;
			}
			Saver.BeginGroup("GlobalSequences", Model.GlobalSequences.Count);
			foreach (MdxLib.Model.CGlobalSequence globalSequence in Model.GlobalSequences)
			{
				Save(Saver, Model, globalSequence);
			}
			Saver.EndGroup();
		}

		public void Save(CSaver Saver, MdxLib.Model.CModel Model, MdxLib.Model.CGlobalSequence GlobalSequence)
		{
			Saver.WriteTabs();
			Saver.WriteWord("Duration ");
			Saver.WriteInteger(GlobalSequence.Duration);
			Saver.WriteLine(",");
		}
	}
}
