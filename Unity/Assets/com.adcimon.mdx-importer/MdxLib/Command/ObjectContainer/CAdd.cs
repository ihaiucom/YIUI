using MdxLib.Model;

namespace MdxLib.Command.ObjectContainer
{
	internal sealed class CAdd<T> : ICommand where T : CObject<T>
	{
		private CObjectContainer<T> CurrentContainer;

		private T NewObject = null;

		public CAdd(CObjectContainer<T> Container, T Object)
		{
			CurrentContainer = Container;
			NewObject = Object;
		}

		public void Do()
		{
			CurrentContainer.InternalObjectList.Add(NewObject);
			NewObject.ObjectContainer = CurrentContainer;
		}

		public void Undo()
		{
			NewObject.ObjectContainer = null;
			CurrentContainer.InternalObjectList.Remove(NewObject);
		}
	}
}
