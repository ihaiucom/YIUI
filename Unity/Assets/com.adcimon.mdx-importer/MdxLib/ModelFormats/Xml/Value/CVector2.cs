using System.Xml;
using MdxLib.Primitives;

namespace MdxLib.ModelFormats.Xml.Value
{
	internal sealed class CVector2 : CUnknown, IValue<MdxLib.Primitives.CVector2>
	{
		private static class CSingleton
		{
			public static CVector2 Instance;

			static CSingleton()
			{
				Instance = new CVector2();
			}
		}

		public static CVector2 Instance => CSingleton.Instance;

		private CVector2()
		{
		}

		public MdxLib.Primitives.CVector2 Read(XmlNode Node, string Name, MdxLib.Primitives.CVector2 DefaultValue)
		{
			return ReadVector2(Node, Name, DefaultValue);
		}

		public void Write(XmlNode Node, string Name, MdxLib.Primitives.CVector2 Value)
		{
			WriteVector2(Node, Name, Value);
		}
	}
}
