using MdxLib.Model;

namespace MdxLib.Command
{
	internal sealed class CAttachObject<T> : ICommand where T : CObject<T>
	{
		private CObjectReference<T> CurrentObjectReference;

		private CUnknown Unknown;

		private T NewObject = null;

		public CAttachObject(CObjectReference<T> ObjectReference, T Object)
		{
			CurrentObjectReference = ObjectReference;
			Unknown = Object;
			NewObject = Object;
		}

		public void Do()
		{
			Unknown.ObjectReferenceSet.Add(CurrentObjectReference);
			CurrentObjectReference.InternalObject = NewObject;
		}

		public void Undo()
		{
			Unknown.ObjectReferenceSet.Remove(CurrentObjectReference);
			CurrentObjectReference.InternalObject = null;
		}
	}
}
