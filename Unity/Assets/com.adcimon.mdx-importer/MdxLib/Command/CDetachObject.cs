using MdxLib.Model;

namespace MdxLib.Command
{
	internal sealed class CDetachObject<T> : ICommand where T : CObject<T>
	{
		private CObjectReference<T> CurrentObjectReference;

		private CUnknown Unknown;

		private T OldObject = null;

		public CDetachObject(CObjectReference<T> ObjectReference, T Object)
		{
			CurrentObjectReference = ObjectReference;
			Unknown = Object;
			OldObject = Object;
		}

		public void Do()
		{
			Unknown.ObjectReferenceSet.Remove(CurrentObjectReference);
			CurrentObjectReference.InternalObject = null;
		}

		public void Undo()
		{
			Unknown.ObjectReferenceSet.Add(CurrentObjectReference);
			CurrentObjectReference.InternalObject = OldObject;
		}
	}
}
