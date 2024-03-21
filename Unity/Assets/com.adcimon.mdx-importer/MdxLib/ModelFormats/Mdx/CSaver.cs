using System.Collections.Generic;
using System.IO;
using MdxLib.Primitives;

namespace MdxLib.ModelFormats.Mdx
{
	internal sealed class CSaver
	{
		private string _Name = "";

		private BinaryWriter Writer;

		private LinkedList<int> LocationStack;

		public string Name => _Name;

		public CSaver(string Name, Stream Stream)
		{
			_Name = Name;
			Writer = new BinaryWriter(Stream, CConstants.SimpleTextEncoding);
			LocationStack = new LinkedList<int>();
		}

		public void Write(byte[] Data)
		{
			Writer.Write(Data);
		}

		public void WriteByte(byte Value)
		{
			Writer.Write(Value);
		}

		public void WriteInt8(int Value)
		{
			Writer.Write((byte)Value);
		}

		public void WriteInt16(int Value)
		{
			Writer.Write((short)Value);
		}

		public void WriteInt32(int Value)
		{
			Writer.Write(Value);
		}

		public void WriteFloat(float Value)
		{
			Writer.Write(Value);
		}

		public void WriteDouble(double Value)
		{
			Writer.Write(Value);
		}

		public void WriteString(string Value, int Length)
		{
			int num = Length - Value.Length;
			string text = ((Value.Length > Length) ? Value.Substring(0, Length) : Value);
			Writer.Write(text.ToCharArray());
			for (int i = 0; i < num; i++)
			{
				Writer.Write('\0');
			}
		}

		public void WriteTag(string Value)
		{
			WriteString(Value, 4);
		}

		public void WriteVector2(CVector2 Value)
		{
			WriteFloat(Value.X);
			WriteFloat(Value.Y);
		}

		public void WriteVector3(CVector3 Value)
		{
			WriteFloat(Value.X);
			WriteFloat(Value.Y);
			WriteFloat(Value.Z);
		}

		public void WriteVector4(CVector4 Value)
		{
			WriteFloat(Value.X);
			WriteFloat(Value.Y);
			WriteFloat(Value.Z);
			WriteFloat(Value.W);
		}

		public void WriteExtent(CExtent Value)
		{
			WriteFloat(Value.Radius);
			WriteVector3(Value.Min);
			WriteVector3(Value.Max);
		}

		public void PushLocation()
		{
			LocationStack.AddLast((int)Writer.BaseStream.Position);
			WriteInt32(0);
		}

		public void PopLocation(int AdditionalSize)
		{
			int num = (int)Writer.BaseStream.Position;
			int value = LocationStack.Last.Value;
			LocationStack.RemoveLast();
			Writer.BaseStream.Position = value;
			WriteInt32(num - value + AdditionalSize);
			Writer.BaseStream.Position = num;
		}

		public void PopInclusiveLocation()
		{
			PopLocation(0);
		}

		public void PopExclusiveLocation()
		{
			PopLocation(-4);
		}
	}
}
