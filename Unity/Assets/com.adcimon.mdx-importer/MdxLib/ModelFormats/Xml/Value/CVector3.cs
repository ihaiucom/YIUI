using System.Xml;
using MdxLib.Primitives;

namespace MdxLib.ModelFormats.Xml.Value
{
	internal sealed class CVector3 : CUnknown, IValue<MdxLib.Primitives.CVector3>
	{
		private static class CSingleton
		{
			public static CVector3 Instance;

			static CSingleton()
			{
				Instance = new CVector3();
			}
		}

		public static CVector3 Instance => CSingleton.Instance;

		private CVector3()
		{
		}

		public MdxLib.Primitives.CVector3 Read(XmlNode Node, string Name, MdxLib.Primitives.CVector3 DefaultValue)
		{
			return ReadVector3(Node, Name, DefaultValue);
		}

		public void Write(XmlNode Node, string Name, MdxLib.Primitives.CVector3 Value)
		{
			WriteVector3(Node, Name, Value);
		}
	}
}
