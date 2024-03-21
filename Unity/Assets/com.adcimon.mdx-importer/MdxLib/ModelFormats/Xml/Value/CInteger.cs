using System.Xml;

namespace MdxLib.ModelFormats.Xml.Value
{
	internal sealed class CInteger : CUnknown, IValue<int>
	{
		private static class CSingleton
		{
			public static CInteger Instance;

			static CSingleton()
			{
				Instance = new CInteger();
			}
		}

		public static CInteger Instance => CSingleton.Instance;

		private CInteger()
		{
		}

		public int Read(XmlNode Node, string Name, int DefaultValue)
		{
			return ReadInteger(Node, Name, DefaultValue);
		}

		public void Write(XmlNode Node, string Name, int Value)
		{
			WriteInteger(Node, Name, Value);
		}
	}
}
