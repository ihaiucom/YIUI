using System.Reflection;
using MdxLib.Animator;

namespace MdxLib.Command
{
	internal sealed class CSetAnimatorField<T, T2> : ICommand where T : new()
	{
		private CAnimator<T> CurrentAnimator;

		private T2 OldValue = default(T2);

		private T2 NewValue = default(T2);

		private FieldInfo FieldInfo;

		public CSetAnimatorField(CAnimator<T> Animator, string FieldName, T2 Value)
		{
			FieldInfo = typeof(CAnimator<T>).GetField(FieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			CurrentAnimator = Animator;
			OldValue = (T2)FieldInfo.GetValue(CurrentAnimator);
			NewValue = Value;
		}

		public void Do()
		{
			FieldInfo.SetValue(CurrentAnimator, NewValue);
		}

		public void Undo()
		{
			FieldInfo.SetValue(CurrentAnimator, OldValue);
		}
	}
}
