using System.Collections.Generic;
using MdxLib.Model;

namespace MdxLib.Command.ObjectContainer
{
	internal sealed class CClear<T> : ICommand where T : CObject<T>
	{
		private CObjectContainer<T> CurrentContainer;

		private List<T> OldObjectList;

		private List<T> NewObjectList;

		private IEnumerable<CDetacher> CurrentDetacherList;

		public CClear(CObjectContainer<T> Container, IEnumerable<CDetacher> DetacherList)
		{
			CurrentContainer = Container;
			OldObjectList = CurrentContainer.InternalObjectList;
			NewObjectList = new List<T>();
			CurrentDetacherList = DetacherList;
		}

		public void Do()
		{
			CDetacher.DetachAllDetachers(CurrentDetacherList);
			foreach (T oldObject in OldObjectList)
			{
				T current = oldObject;
				current.ObjectContainer = null;
			}
			CurrentContainer.InternalObjectList = NewObjectList;
		}

		public void Undo()
		{
			CurrentContainer.InternalObjectList = OldObjectList;
			foreach (T oldObject in OldObjectList)
			{
				T current = oldObject;
				current.ObjectContainer = CurrentContainer;
			}
			CDetacher.AttachAllDetachers(CurrentDetacherList);
		}
	}
}
