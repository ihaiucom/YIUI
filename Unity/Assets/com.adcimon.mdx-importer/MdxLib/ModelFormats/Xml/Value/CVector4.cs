using System.Xml;
using MdxLib.Primitives;

namespace MdxLib.ModelFormats.Xml.Value
{
	internal sealed class CVector4 : CUnknown, IValue<MdxLib.Primitives.CVector4>
	{
		private static class CSingleton
		{
			public static CVector4 Instance;

			static CSingleton()
			{
				Instance = new CVector4();
			}
		}

		public static CVector4 Instance => CSingleton.Instance;

		private CVector4()
		{
		}

		public MdxLib.Primitives.CVector4 Read(XmlNode Node, string Name, MdxLib.Primitives.CVector4 DefaultValue)
		{
			return ReadVector4(Node, Name, DefaultValue);
		}

		public void Write(XmlNode Node, string Name, MdxLib.Primitives.CVector4 Value)
		{
			WriteVector4(Node, Name, Value);
		}
	}
}
