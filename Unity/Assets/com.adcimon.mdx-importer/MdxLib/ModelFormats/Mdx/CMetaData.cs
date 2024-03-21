using System.IO;
using System.Xml;
using MdxLib.Model;

namespace MdxLib.ModelFormats.Mdx
{
	internal sealed class CMetaData : CObject
	{
		private static class CSingleton
		{
			public static CMetaData Instance;

			static CSingleton()
			{
				Instance = new CMetaData();
			}
		}

		public static CMetaData Instance => CSingleton.Instance;

		private CMetaData()
		{
		}

		public void Load(CLoader Loader, MdxLib.Model.CModel Model)
		{
			int length = Loader.ReadInt32();
			string s = Loader.ReadString(length);
			using StringReader input = new StringReader(s);
			using XmlTextReader reader = new XmlTextReader(input);
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(reader);
			XmlNode xmlNode = xmlDocument.SelectSingleNode("meta");
			if (xmlNode != null && xmlNode.ChildNodes.Count > 0)
			{
				XmlNode newChild = Model.MetaData.ImportNode(xmlNode, deep: true);
				Model.MetaData.ReplaceChild(newChild, Model.MetaData.DocumentElement);
			}
		}

		public void Save(CSaver Saver, MdxLib.Model.CModel Model)
		{
			if (!Model.HasMetaData)
			{
				return;
			}
			Saver.WriteTag("META");
			Saver.PushLocation();
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, CConstants.SimpleTextEncoding);
				xmlTextWriter.Formatting = Formatting.None;
				xmlTextWriter.WriteStartDocument();
				Model.MetaData.Save(xmlTextWriter);
				Saver.Write(memoryStream.GetBuffer());
			}
			Saver.PopExclusiveLocation();
		}
	}
}
