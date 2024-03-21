using System.Xml;
using MdxLib.Model;

namespace MdxLib.ModelFormats.Xml
{
	internal sealed class CGlobalSequence : CObject
	{
		private static class CSingleton
		{
			public static CGlobalSequence Instance;

			static CSingleton()
			{
				Instance = new CGlobalSequence();
			}
		}

		public static CGlobalSequence Instance => CSingleton.Instance;

		private CGlobalSequence()
		{
		}

		public void Load(CLoader Loader, XmlNode Node, MdxLib.Model.CModel Model, MdxLib.Model.CGlobalSequence GlobalSequence)
		{
			GlobalSequence.Duration = ReadInteger(Node, "duration", GlobalSequence.Duration);
		}

		public void Save(CSaver Saver, XmlNode Node, MdxLib.Model.CModel Model, MdxLib.Model.CGlobalSequence GlobalSequence)
		{
			WriteInteger(Node, "duration", GlobalSequence.Duration);
		}
	}
}
