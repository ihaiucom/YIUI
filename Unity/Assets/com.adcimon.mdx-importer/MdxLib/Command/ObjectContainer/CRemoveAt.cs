using System.Collections.Generic;
using MdxLib.Model;

namespace MdxLib.Command.ObjectContainer
{
	internal sealed class CRemoveAt<T> : ICommand where T : CObject<T>
	{
		private CObjectContainer<T> CurrentContainer;

		private int CurrentIndex = -1;

		private T OldObject = null;

		private IEnumerable<CDetacher> CurrentDetacherList;

		public CRemoveAt(CObjectContainer<T> Container, int Index, IEnumerable<CDetacher> DetacherList)
		{
			CurrentContainer = Container;
			CurrentIndex = Index;
			OldObject = CurrentContainer.InternalObjectList[CurrentIndex];
			CurrentDetacherList = DetacherList;
		}

		public void Do()
		{
			CDetacher.DetachAllDetachers(CurrentDetacherList);
			OldObject.ObjectContainer = null;
			CurrentContainer.InternalObjectList.RemoveAt(CurrentIndex);
		}

		public void Undo()
		{
			CurrentContainer.InternalObjectList.Insert(CurrentIndex, OldObject);
			OldObject.ObjectContainer = CurrentContainer;
			CDetacher.AttachAllDetachers(CurrentDetacherList);
		}
	}
}
