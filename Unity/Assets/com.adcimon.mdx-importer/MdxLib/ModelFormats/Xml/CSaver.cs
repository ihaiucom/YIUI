namespace MdxLib.ModelFormats.Xml
{
	internal sealed class CSaver
	{
		private string _Name = "";

		public string Name => _Name;

		public CSaver(string Name)
		{
			_Name = Name;
		}
	}
}
