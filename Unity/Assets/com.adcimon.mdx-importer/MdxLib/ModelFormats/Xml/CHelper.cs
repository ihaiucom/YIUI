using System.Xml;
using MdxLib.Model;

namespace MdxLib.ModelFormats.Xml
{
	internal sealed class CHelper : CNode
	{
		private static class CSingleton
		{
			public static CHelper Instance;

			static CSingleton()
			{
				Instance = new CHelper();
			}
		}

		public static CHelper Instance => CSingleton.Instance;

		private CHelper()
		{
		}

		public void Load(CLoader Loader, XmlNode Node, MdxLib.Model.CModel Model, MdxLib.Model.CHelper Helper)
		{
			LoadNode(Loader, Node, Model, Helper);
		}

		public void Save(CSaver Saver, XmlNode Node, MdxLib.Model.CModel Model, MdxLib.Model.CHelper Helper)
		{
			SaveNode(Saver, Node, Model, Helper);
		}
	}
}
