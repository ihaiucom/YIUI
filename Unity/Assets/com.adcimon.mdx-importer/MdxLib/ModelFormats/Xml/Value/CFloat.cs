using System.Xml;

namespace MdxLib.ModelFormats.Xml.Value
{
	internal sealed class CFloat : CUnknown, IValue<float>
	{
		private static class CSingleton
		{
			public static CFloat Instance;

			static CSingleton()
			{
				Instance = new CFloat();
			}
		}

		public static CFloat Instance => CSingleton.Instance;

		private CFloat()
		{
		}

		public float Read(XmlNode Node, string Name, float DefaultValue)
		{
			return ReadFloat(Node, Name, DefaultValue);
		}

		public void Write(XmlNode Node, string Name, float Value)
		{
			WriteFloat(Node, Name, Value);
		}
	}
}
