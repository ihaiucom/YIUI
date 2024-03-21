using MdxLib.Model;

namespace MdxLib.ModelFormats.Attacher
{
	public class CObjectAttacher<T> : IAttacher where T : CObject<T>
	{
		private CObjectContainer<T> _Container;

		private CObjectReference<T> _Reference;

		private int _Id = -1;

		public CObjectAttacher(CObjectContainer<T> Container, CObjectReference<T> Reference, int Id)
		{
			_Container = Container;
			_Reference = Reference;
			_Id = Id;
		}

		public void Attach()
		{
			if (_Id != -1)
			{
				_Reference.Attach(_Container[_Id]);
			}
		}
	}
}
