using System.IO;
using System.Text;
using MdxLib.Primitives;

namespace MdxLib.ModelFormats.Mdl
{
	internal sealed class CSaver
	{
		private string _Name = "";

		private int TabDepth;

		private Stream OutputStream;

		private StringBuilder OutputBuilder;

		public string Name => _Name;

		public CSaver(string Name, Stream Stream)
		{
			_Name = Name;
			OutputStream = Stream;
			OutputBuilder = new StringBuilder();
		}

		public void WriteToStream()
		{
			using StreamWriter streamWriter = new StreamWriter(OutputStream, CConstants.TextEncoding);
			streamWriter.Write(OutputBuilder.ToString());
		}

		public void WriteTabs()
		{
			for (int i = 0; i < TabDepth; i++)
			{
				OutputBuilder.Append("\t");
			}
		}

		public void WriteTabs(int NrOfTabs)
		{
			for (int i = 0; i < NrOfTabs; i++)
			{
				OutputBuilder.Append("\t");
			}
		}

		public void WriteInteger(int Value)
		{
			OutputBuilder.Append(Value);
		}

		public void WriteFloat(float Value)
		{
			OutputBuilder.Append(Value.ToString(CConstants.NumberFormat));
		}

		public void WriteCharacter(char Value)
		{
			OutputBuilder.Append(Value);
		}

		public void WriteWord(string Value)
		{
			OutputBuilder.Append(Value);
		}

		public void WriteLine()
		{
			OutputBuilder.AppendLine();
		}

		public void WriteLine(string Value)
		{
			OutputBuilder.AppendLine(Value);
		}

		public void WriteString(string Value)
		{
			OutputBuilder.Append("\"" + Value.Replace("\"", "\\\"") + "\"");
		}

		public void WriteVector2(CVector2 Value)
		{
			WriteWord("{ ");
			WriteFloat(Value.X);
			WriteWord(", ");
			WriteFloat(Value.Y);
			WriteWord(" }");
		}

		public void WriteVector3(CVector3 Value)
		{
			WriteWord("{ ");
			WriteFloat(Value.X);
			WriteWord(", ");
			WriteFloat(Value.Y);
			WriteWord(", ");
			WriteFloat(Value.Z);
			WriteWord(" }");
		}

		public void WriteVector4(CVector4 Value)
		{
			WriteWord("{ ");
			WriteFloat(Value.X);
			WriteWord(", ");
			WriteFloat(Value.Y);
			WriteWord(", ");
			WriteFloat(Value.Z);
			WriteWord(", ");
			WriteFloat(Value.W);
			WriteWord(" }");
		}

		public void WriteColor(CVector3 Value)
		{
			WriteWord("{ ");
			WriteFloat(Value.Z);
			WriteWord(", ");
			WriteFloat(Value.Y);
			WriteWord(", ");
			WriteFloat(Value.X);
			WriteWord(" }");
		}

		public void BeginGroup(string Group)
		{
			WriteTabs();
			OutputBuilder.AppendLine(Group + " {");
			TabDepth++;
		}

		public void BeginGroup(string Group, string Name)
		{
			WriteTabs();
			OutputBuilder.AppendLine(Group + " \"" + Name + "\" {");
			TabDepth++;
		}

		public void BeginGroup(string Group, int Size)
		{
			WriteTabs();
			OutputBuilder.AppendLine(Group + " " + Size + " {");
			TabDepth++;
		}

		public void BeginGroup(string Group, int Size1, int Size2)
		{
			WriteTabs();
			OutputBuilder.AppendLine(Group + " " + Size1 + " " + Size2 + " {");
			TabDepth++;
		}

		public void EndGroup()
		{
			TabDepth--;
			WriteTabs();
			OutputBuilder.AppendLine("}");
		}

		public void EndGroup(string ExtraString)
		{
			TabDepth--;
			WriteTabs();
			OutputBuilder.AppendLine("}" + ExtraString);
		}
	}
}
