using System.Collections.Generic;

namespace MdxLib.Model
{
	internal abstract class CDetacher
	{
		public CDetacher()
		{
		}

		public abstract void Detach();

		public abstract void Attach();

		public static void DetachAllDetachers(IEnumerable<CDetacher> DetacherList)
		{
			foreach (CDetacher Detacher in DetacherList)
			{
				Detacher.Detach();
			}
		}

		public static void AttachAllDetachers(IEnumerable<CDetacher> DetacherList)
		{
			foreach (CDetacher Detacher in DetacherList)
			{
				Detacher.Attach();
			}
		}
	}
}
