using System.Collections.Generic;
using MdxLib.Model;

namespace MdxLib.Command.ObjectContainer
{
	internal sealed class CSet<T> : ICommand where T : CObject<T>
	{
		private CObjectContainer<T> CurrentContainer;

		private int CurrentIndex = -1;

		private T OldObject = null;

		private T NewObject = null;

		private IEnumerable<CDetacher> CurrentDetacherList;

		public CSet(CObjectContainer<T> Container, int Index, T Object, IEnumerable<CDetacher> DetacherList)
		{
			CurrentContainer = Container;
			CurrentIndex = Index;
			OldObject = CurrentContainer.InternalObjectList[CurrentIndex];
			NewObject = Object;
			CurrentDetacherList = DetacherList;
		}

		public void Do()
		{
			CDetacher.DetachAllDetachers(CurrentDetacherList);
			OldObject.ObjectContainer = null;
			CurrentContainer.InternalObjectList[CurrentIndex] = NewObject;
			NewObject.ObjectContainer = CurrentContainer;
		}

		public void Undo()
		{
			NewObject.ObjectContainer = null;
			CurrentContainer.InternalObjectList[CurrentIndex] = OldObject;
			OldObject.ObjectContainer = CurrentContainer;
			CDetacher.AttachAllDetachers(CurrentDetacherList);
		}
	}
}
