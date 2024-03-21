using System.Reflection;
using MdxLib.Model;

namespace MdxLib.Command
{
	internal sealed class CSetModelField<T> : ICommand
	{
		private CModel CurrentModel;

		private T OldValue = default(T);

		private T NewValue = default(T);

		private FieldInfo FieldInfo;

		public CSetModelField(CModel Model, string FieldName, T Value)
		{
			FieldInfo = typeof(CModel).GetField(FieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			CurrentModel = Model;
			OldValue = (T)FieldInfo.GetValue(CurrentModel);
			NewValue = Value;
		}

		public void Do()
		{
			FieldInfo.SetValue(CurrentModel, NewValue);
		}

		public void Undo()
		{
			FieldInfo.SetValue(CurrentModel, OldValue);
		}
	}
}
