using System;
using MdxLib.Model;
using MdxLib.ModelFormats.Mdl.Token;
using MdxLib.Primitives;

namespace MdxLib.ModelFormats.Mdl
{
	internal sealed class CSequence : CObject
	{
		private static class CSingleton
		{
			public static CSequence Instance;

			static CSingleton()
			{
				Instance = new CSequence();
			}
		}

		public static CSequence Instance => CSingleton.Instance;

		private CSequence()
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
				if ((text2 = text) == null || !(text2 == "anim"))
				{
					break;
				}
				MdxLib.Model.CSequence cSequence = new MdxLib.Model.CSequence(Model);
				Load(Loader, Model, cSequence);
				Model.Sequences.Add(cSequence);
			}
			throw new Exception("Syntax error at line " + Loader.Line + ", unknown tag \"" + text + "\"!");
		}

		public void Load(CLoader Loader, MdxLib.Model.CModel Model, MdxLib.Model.CSequence Sequence)
		{
			float radius = 0f;
			CVector3 min = CConstants.DefaultVector3;
			CVector3 max = CConstants.DefaultVector3;
			Sequence.Name = Loader.ReadString();
			Loader.ExpectToken(EType.CurlyBracketLeft);
			while (Loader.PeekToken() != EType.CurlyBracketRight)
			{
				string text = Loader.ReadWord();
				switch (text)
				{
				case "syncpoint":
					Sequence.SyncPoint = LoadInteger(Loader);
					break;
				case "rarity":
					Sequence.Rarity = LoadFloat(Loader);
					break;
				case "movespeed":
					Sequence.MoveSpeed = LoadFloat(Loader);
					break;
				case "minimumextent":
					min = LoadVector3(Loader);
					break;
				case "maximumextent":
					max = LoadVector3(Loader);
					break;
				case "boundsradius":
					radius = LoadFloat(Loader);
					break;
				case "nonlooping":
					Sequence.NonLooping = LoadBoolean(Loader);
					break;
				case "interval":
					Loader.ExpectToken(EType.CurlyBracketLeft);
					Sequence.IntervalStart = Loader.ReadInteger();
					Loader.ExpectToken(EType.Separator);
					Sequence.IntervalEnd = Loader.ReadInteger();
					Loader.ExpectToken(EType.CurlyBracketRight);
					Loader.ExpectToken(EType.Separator);
					break;
				default:
					throw new Exception("Syntax error at line " + Loader.Line + ", unknown tag \"" + text + "\"!");
				}
			}
			Loader.ReadToken();
			Sequence.Extent = new CExtent(min, max, radius);
		}

		public void SaveAll(CSaver Saver, MdxLib.Model.CModel Model)
		{
			if (!Model.HasSequences)
			{
				return;
			}
			Saver.BeginGroup("Sequences", Model.Sequences.Count);
			foreach (MdxLib.Model.CSequence sequence in Model.Sequences)
			{
				Save(Saver, Model, sequence);
			}
			Saver.EndGroup();
		}

		public void Save(CSaver Saver, MdxLib.Model.CModel Model, MdxLib.Model.CSequence Sequence)
		{
			Saver.BeginGroup("Anim", Sequence.Name);
			Saver.WriteTabs();
			Saver.WriteWord("Interval { ");
			Saver.WriteInteger(Sequence.IntervalStart);
			Saver.WriteWord(", ");
			Saver.WriteInteger(Sequence.IntervalEnd);
			Saver.WriteLine(" },");
			SaveInteger(Saver, "SyncPoint", Sequence.SyncPoint, ECondition.NotZero);
			SaveFloat(Saver, "Rarity", Sequence.Rarity, ECondition.NotZero);
			SaveFloat(Saver, "MoveSpeed", Sequence.MoveSpeed, ECondition.NotZero);
			SaveBoolean(Saver, "NonLooping", Sequence.NonLooping);
			SaveVector3(Saver, "MinimumExtent", Sequence.Extent.Min, ECondition.NotZero);
			SaveVector3(Saver, "MaximumExtent", Sequence.Extent.Max, ECondition.NotZero);
			SaveFloat(Saver, "BoundsRadius", Sequence.Extent.Radius, ECondition.NotZero);
			Saver.EndGroup();
		}
	}
}
