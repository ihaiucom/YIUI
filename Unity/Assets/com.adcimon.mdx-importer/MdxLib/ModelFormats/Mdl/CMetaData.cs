using System;
using System.IO;
using System.Xml;
using MdxLib.Model;

namespace MdxLib.ModelFormats.Mdl
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
			string s = Loader.ReadMetaData();
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
			using StringWriter stringWriter = new StringWriter();
			using XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
			xmlTextWriter.Formatting = Formatting.Indented;
			xmlTextWriter.WriteStartDocument();
			Model.MetaData.Save(xmlTextWriter);
			string[] array = stringWriter.ToString().Replace("\r", "").Split(new string[1] { "\n" }, StringSplitOptions.None);
			string[] array2 = array;
			foreach (string text in array2)
			{
				Saver.WriteLine("//" + text);
			}
		}
	}
}
