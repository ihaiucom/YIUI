using System.Xml;

namespace MdxLib.ModelFormats.Xml.Value
{
	internal interface IValue<T> where T : new()
	{
		T Read(XmlNode Node, string Name, T DefaultValue);

		void Write(XmlNode Node, string Name, T Value);
	}
}
