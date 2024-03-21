using MdxLib.Model;

namespace MdxLib.Command.ObjectContainer
{
	internal sealed class CInsert<T> : ICommand where T : CObject<T>
	{
		private CObjectContainer<T> CurrentContainer;

		private int CurrentIndex = -1;

		private T NewObject = null;

		public CInsert(CObjectContainer<T> Container, int Index, T Object)
		{
			CurrentContainer = Container;
			CurrentIndex = Index;
			NewObject = Object;
		}

		public void Do()
		{
			CurrentContainer.InternalObjectList.Insert(CurrentIndex, NewObject);
			NewObject.ObjectContainer = CurrentContainer;
		}

		public void Undo()
		{
			NewObject.ObjectContainer = null;
			CurrentContainer.InternalObjectList.RemoveAt(CurrentIndex);
		}
	}
}
