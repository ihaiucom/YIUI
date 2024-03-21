using System;
using System.Xml;
using MdxLib.Primitives;

namespace MdxLib.ModelFormats.Xml
{
	internal abstract class CUnknown
	{
		public CUnknown()
		{
		}

		public bool ReadBoolean(XmlNode Node, string Name, bool DefaultValue)
		{
			XmlElement xmlElement = Node.SelectSingleNode(Name) as XmlElement;
			if (xmlElement == null)
			{
				return DefaultValue;
			}
			return Bool(xmlElement.GetAttribute("bool"), DefaultValue);
		}

		public int ReadInteger(XmlNode Node, string Name, int DefaultValue)
		{
			XmlElement xmlElement = Node.SelectSingleNode(Name) as XmlElement;
			if (xmlElement == null)
			{
				return DefaultValue;
			}
			return Int(xmlElement.GetAttribute("int"), DefaultValue);
		}

		public float ReadFloat(XmlNode Node, string Name, float DefaultValue)
		{
			XmlElement xmlElement = Node.SelectSingleNode(Name) as XmlElement;
			if (xmlElement == null)
			{
				return DefaultValue;
			}
			return Float(xmlElement.GetAttribute("float"), DefaultValue);
		}

		public string ReadString(XmlNode Node, string Name, string DefaultValue)
		{
			XmlElement xmlElement = Node.SelectSingleNode(Name) as XmlElement;
			if (xmlElement == null)
			{
				return DefaultValue;
			}
			string attribute = xmlElement.GetAttribute("string");
			if (attribute == null)
			{
				return DefaultValue;
			}
			return attribute;
		}

		public CVector2 ReadVector2(XmlNode Node, string Name, CVector2 DefaultValue)
		{
			XmlElement xmlElement = Node.SelectSingleNode(Name) as XmlElement;
			if (xmlElement == null)
			{
				return DefaultValue;
			}
			float x = Float(xmlElement.GetAttribute("x"), DefaultValue.X);
			float y = Float(xmlElement.GetAttribute("y"), DefaultValue.Y);
			return new CVector2(x, y);
		}

		public CVector3 ReadVector3(XmlNode Node, string Name, CVector3 DefaultValue)
		{
			XmlElement xmlElement = Node.SelectSingleNode(Name) as XmlElement;
			if (xmlElement == null)
			{
				return DefaultValue;
			}
			float x = Float(xmlElement.GetAttribute("x"), DefaultValue.X);
			float y = Float(xmlElement.GetAttribute("y"), DefaultValue.Y);
			float z = Float(xmlElement.GetAttribute("z"), DefaultValue.Z);
			return new CVector3(x, y, z);
		}

		public CVector4 ReadVector4(XmlNode Node, string Name, CVector4 DefaultValue)
		{
			XmlElement xmlElement = Node.SelectSingleNode(Name) as XmlElement;
			if (xmlElement == null)
			{
				return DefaultValue;
			}
			float x = Float(xmlElement.GetAttribute("x"), DefaultValue.X);
			float y = Float(xmlElement.GetAttribute("y"), DefaultValue.Y);
			float z = Float(xmlElement.GetAttribute("z"), DefaultValue.Z);
			float w = Float(xmlElement.GetAttribute("w"), DefaultValue.W);
			return new CVector4(x, y, z, w);
		}

		public CExtent ReadExtent(XmlNode Node, string Name, CExtent DefaultValue)
		{
			XmlNode xmlNode = Node.SelectSingleNode(Name);
			if (xmlNode == null)
			{
				return DefaultValue;
			}
			CVector3 min = ReadVector3(xmlNode, "min", DefaultValue.Min);
			CVector3 max = ReadVector3(xmlNode, "max", DefaultValue.Max);
			float radius = ReadFloat(xmlNode, "radius", DefaultValue.Radius);
			return new CExtent(min, max, radius);
		}

		public CSegment ReadSegment(XmlNode Node, string Name, CSegment DefaultValue)
		{
			XmlNode xmlNode = Node.SelectSingleNode(Name);
			if (xmlNode == null)
			{
				return DefaultValue;
			}
			CVector3 color = ReadVector3(xmlNode, "color", DefaultValue.Color);
			float alpha = ReadFloat(xmlNode, "alpha", DefaultValue.Alpha);
			float scaling = ReadFloat(xmlNode, "scaling", DefaultValue.Scaling);
			return new CSegment(color, alpha, scaling);
		}

		public CInterval ReadInterval(XmlNode Node, string Name, CInterval DefaultValue)
		{
			XmlNode xmlNode = Node.SelectSingleNode(Name);
			if (xmlNode == null)
			{
				return DefaultValue;
			}
			int start = ReadInteger(xmlNode, "start", DefaultValue.Start);
			int end = ReadInteger(xmlNode, "end", DefaultValue.End);
			int repeat = ReadInteger(xmlNode, "repeat", DefaultValue.Repeat);
			return new CInterval(start, end, repeat);
		}

		public void WriteBoolean(XmlNode Node, string Name, bool Value)
		{
			XmlElement node = AppendElement(Node, Name);
			AppendAttribute(node, "bool", Value ? "1" : "0");
		}

		public void WriteInteger(XmlNode Node, string Name, int Value)
		{
			XmlElement node = AppendElement(Node, Name);
			AppendAttribute(node, "int", Value.ToString());
		}

		public void WriteFloat(XmlNode Node, string Name, float Value)
		{
			XmlElement node = AppendElement(Node, Name);
			AppendAttribute(node, "float", Value.ToString(CConstants.NumberFormat));
		}

		public void WriteString(XmlNode Node, string Name, string Value)
		{
			XmlElement node = AppendElement(Node, Name);
			AppendAttribute(node, "string", Value);
		}

		public void WriteVector2(XmlNode Node, string Name, CVector2 Value)
		{
			XmlElement node = AppendElement(Node, Name);
			AppendAttribute(node, "x", Value.X.ToString(CConstants.NumberFormat));
			AppendAttribute(node, "y", Value.Y.ToString(CConstants.NumberFormat));
		}

		public void WriteVector3(XmlNode Node, string Name, CVector3 Value)
		{
			XmlElement node = AppendElement(Node, Name);
			AppendAttribute(node, "x", Value.X.ToString(CConstants.NumberFormat));
			AppendAttribute(node, "y", Value.Y.ToString(CConstants.NumberFormat));
			AppendAttribute(node, "z", Value.Z.ToString(CConstants.NumberFormat));
		}

		public void WriteVector4(XmlNode Node, string Name, CVector4 Value)
		{
			XmlElement node = AppendElement(Node, Name);
			AppendAttribute(node, "x", Value.X.ToString(CConstants.NumberFormat));
			AppendAttribute(node, "y", Value.Y.ToString(CConstants.NumberFormat));
			AppendAttribute(node, "z", Value.Z.ToString(CConstants.NumberFormat));
			AppendAttribute(node, "w", Value.W.ToString(CConstants.NumberFormat));
		}

		public void WriteExtent(XmlNode Node, string Name, CExtent Value)
		{
			XmlElement node = AppendElement(Node, Name);
			WriteVector3(node, "min", Value.Min);
			WriteVector3(node, "max", Value.Max);
			WriteFloat(node, "radius", Value.Radius);
		}

		public void WriteSegment(XmlNode Node, string Name, CSegment Value)
		{
			XmlElement node = AppendElement(Node, Name);
			WriteVector3(node, "color", Value.Color);
			WriteFloat(node, "alpha", Value.Alpha);
			WriteFloat(node, "scaling", Value.Scaling);
		}

		public void WriteInterval(XmlNode Node, string Name, CInterval Value)
		{
			XmlElement node = AppendElement(Node, Name);
			WriteInteger(node, "start", Value.Start);
			WriteInteger(node, "end", Value.End);
			WriteInteger(node, "repeat", Value.Repeat);
		}

		public XmlElement AppendElement(XmlNode Node, string Name)
		{
			XmlElement xmlElement = Node.OwnerDocument.CreateElement(Name);
			Node.AppendChild(xmlElement);
			return xmlElement;
		}

		public XmlAttribute AppendAttribute(XmlNode Node, string Name, string Value)
		{
			XmlAttribute xmlAttribute = Node.OwnerDocument.CreateAttribute(Name);
			xmlAttribute.Value = Value;
			Node.Attributes.Append(xmlAttribute);
			return xmlAttribute;
		}

		public bool Bool(string String, bool DefaultValue)
		{
			try
			{
				if (String == null)
				{
					return DefaultValue;
				}
				return int.Parse(String) != 0;
			}
			catch (Exception)
			{
				return DefaultValue;
			}
		}

		public int Int(string String, int DefaultValue)
		{
			try
			{
				if (String == null)
				{
					return DefaultValue;
				}
				return int.Parse(String);
			}
			catch (Exception)
			{
				return DefaultValue;
			}
		}

		public float Float(string String, float DefaultValue)
		{
			try
			{
				if (String == null)
				{
					return DefaultValue;
				}
				return float.Parse(String, CConstants.NumberFormat);
			}
			catch (Exception)
			{
				return DefaultValue;
			}
		}
	}
}
