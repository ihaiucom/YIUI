using MdxLib.Model;

namespace MdxLib.ModelFormats.Attacher
{
	public class CNodeAttacher : IAttacher
	{
		private CModel _Model;

		private CNodeReference _Reference;

		private int _Id = -1;

		public CNodeAttacher(CModel Model, CNodeReference Reference, int Id)
		{
			_Model = Model;
			_Reference = Reference;
			_Id = Id;
		}

		public void Attach()
		{
			if (_Id != -1)
			{
				_Reference.Attach(_Model.Nodes[_Id]);
			}
		}
	}
}
