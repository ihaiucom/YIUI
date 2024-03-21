using System.Collections;
using System.Collections.Generic;
using MdxLib.Model;

namespace MdxLib.ModelFormats.Attacher
{
	public class CAttacherContainer : IEnumerable<IAttacher>, IEnumerable
	{
		private LinkedList<IAttacher> AttacherList;

		public CAttacherContainer()
		{
			AttacherList = new LinkedList<IAttacher>();
		}

		public void Clear()
		{
			AttacherList.Clear();
		}

		public void Add(IAttacher Attacher)
		{
			AttacherList.AddLast(Attacher);
		}

		public void AddObject<T>(CObjectContainer<T> Container, CObjectReference<T> Reference, int Id) where T : CObject<T>
		{
			if (Id != -1)
			{
				Add(new CObjectAttacher<T>(Container, Reference, Id));
			}
		}

		public void AddNode(CModel Model, CNodeReference Reference, int Id)
		{
			if (Id != -1)
			{
				Add(new CNodeAttacher(Model, Reference, Id));
			}
		}

		public void Attach()
		{
			foreach (IAttacher attacher in AttacherList)
			{
				attacher.Attach();
			}
		}

		public IEnumerator<IAttacher> GetEnumerator()
		{
			return AttacherList.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return AttacherList.GetEnumerator();
		}
	}
}
