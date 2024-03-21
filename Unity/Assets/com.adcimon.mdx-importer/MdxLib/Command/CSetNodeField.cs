using System.Reflection;
using MdxLib.Model;

namespace MdxLib.Command
{
	internal sealed class CSetNodeField<T1, T2> : ICommand where T1 : CNode<T1>
	{
		private T1 CurrentObject = null;

		private T2 OldValue = default(T2);

		private T2 NewValue = default(T2);

		private FieldInfo FieldInfo;

		public CSetNodeField(T1 Object, string FieldName, T2 Value)
		{
			FieldInfo = typeof(CNode<T1>).GetField(FieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			CurrentObject = Object;
			OldValue = (T2)FieldInfo.GetValue(CurrentObject);
			NewValue = Value;
		}

		public void Do()
		{
			FieldInfo.SetValue(CurrentObject, NewValue);
		}

		public void Undo()
		{
			FieldInfo.SetValue(CurrentObject, OldValue);
		}
	}
}
