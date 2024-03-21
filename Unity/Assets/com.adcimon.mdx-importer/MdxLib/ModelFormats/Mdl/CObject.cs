using System;
using MdxLib.Animator;
using MdxLib.Model;
using MdxLib.ModelFormats.Mdl.Token;
using MdxLib.ModelFormats.Mdl.Value;

namespace MdxLib.ModelFormats.Mdl
{
	internal abstract class CObject : CUnknown
	{
		public CObject()
		{
		}

		public void LoadAnimator<T>(CLoader Loader, MdxLib.Model.CModel Model, CAnimator<T> Animator, IValue<T> ValueHandler) where T : new()
		{
			Animator.MakeAnimated();
			Loader.ReadInteger();
			Loader.ExpectToken(EType.CurlyBracketLeft);
			while (Loader.PeekToken() == EType.Word)
			{
				string text = Loader.ReadWord();
				switch (text)
				{
				case "dontinterp":
					Animator.Type = EInterpolationType.None;
					Loader.ExpectToken(EType.Separator);
					continue;
				case "linear":
					Animator.Type = EInterpolationType.Linear;
					Loader.ExpectToken(EType.Separator);
					continue;
				case "bezier":
					Animator.Type = EInterpolationType.Bezier;
					Loader.ExpectToken(EType.Separator);
					continue;
				case "hermite":
					Animator.Type = EInterpolationType.Hermite;
					Loader.ExpectToken(EType.Separator);
					continue;
				case "globalseqid":
					Loader.Attacher.AddObject(Model.GlobalSequences, Animator.GlobalSequence, Loader.ReadInteger());
					Loader.ExpectToken(EType.Separator);
					continue;
				}
				throw new Exception("Syntax error at line " + Loader.Line + ", unknown tag \"" + text + "\"!");
			}
			while (Loader.PeekToken() != EType.CurlyBracketRight)
			{
				int time = Loader.ReadInteger();
				Loader.ExpectToken(EType.Colon);
				T value = ValueHandler.Read(Loader);
				Loader.ExpectToken(EType.Separator);
				switch (Animator.Type)
				{
				case EInterpolationType.None:
				case EInterpolationType.Linear:
					Animator.Add(new CAnimatorNode<T>(time, value));
					break;
				case EInterpolationType.Bezier:
				case EInterpolationType.Hermite:
				{
					Loader.ExpectWord("intan");
					T inTangent = ValueHandler.Read(Loader);
					Loader.ExpectToken(EType.Separator);
					Loader.ExpectWord("outtan");
					T outTangent = ValueHandler.Read(Loader);
					Loader.ExpectToken(EType.Separator);
					Animator.Add(new CAnimatorNode<T>(time, value, inTangent, outTangent));
					break;
				}
				}
			}
			Loader.ReadToken();
		}

		public void LoadStaticAnimator<T>(CLoader Loader, MdxLib.Model.CModel Model, CAnimator<T> Animator, IValue<T> ValueHandler) where T : new()
		{
			Animator.MakeStatic(ValueHandler.Read(Loader));
			Loader.ExpectToken(EType.Separator);
		}

		public void SaveAnimator<T>(CSaver Saver, MdxLib.Model.CModel Model, CAnimator<T> Animator, IValue<T> ValueHandler, string Name) where T : new()
		{
			SaveAnimator(Saver, Model, Animator, ValueHandler, Name, ECondition.Always);
		}

		public void SaveAnimator<T>(CSaver Saver, MdxLib.Model.CModel Model, CAnimator<T> Animator, IValue<T> ValueHandler, string Name, ECondition Condition) where T : new()
		{
			if (Animator.Static)
			{
				if (ValueHandler.ValidCondition(Animator.GetValue(), Condition))
				{
					Saver.WriteTabs();
					Saver.WriteWord("static " + Name + " ");
					ValueHandler.Write(Saver, Animator.GetValue());
					Saver.WriteLine(",");
				}
				return;
			}
			Saver.BeginGroup(Name, Animator.Count);
			SaveBoolean(Saver, TypeToString(Animator.Type), Value: true);
			SaveId(Saver, "GlobalSeqId", Animator.GlobalSequence.ObjectId, ECondition.NotInvalidId);
			foreach (CAnimatorNode<T> item in Animator)
			{
				Saver.WriteTabs();
				Saver.WriteInteger(item.Time);
				Saver.WriteWord(": ");
				ValueHandler.Write(Saver, item.Value);
				Saver.WriteLine(",");
				switch (Animator.Type)
				{
				case EInterpolationType.Bezier:
				case EInterpolationType.Hermite:
					Saver.WriteTabs();
					Saver.WriteTabs(1);
					Saver.WriteWord("InTan ");
					ValueHandler.Write(Saver, item.InTangent);
					Saver.WriteLine(",");
					Saver.WriteTabs();
					Saver.WriteTabs(1);
					Saver.WriteWord("OutTan ");
					ValueHandler.Write(Saver, item.OutTangent);
					Saver.WriteLine(",");
					break;
				}
			}
			Saver.EndGroup();
		}

		private string TypeToString(EInterpolationType Type)
		{
			return Type switch
			{
				EInterpolationType.None => "DontInterp", 
				EInterpolationType.Linear => "Linear", 
				EInterpolationType.Bezier => "Bezier", 
				EInterpolationType.Hermite => "Hermite", 
				_ => "", 
			};
		}
	}
}
