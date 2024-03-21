using MdxLib.ModelFormats.Attacher;

namespace MdxLib.ModelFormats.Xml
{
	internal sealed class CLoader
	{
		private string _Name = "";

		private CAttacherContainer _Attacher;

		public string Name => _Name;

		public CAttacherContainer Attacher => _Attacher;

		public CLoader(string Name)
		{
			_Name = Name;
			_Attacher = new CAttacherContainer();
		}
	}
}
