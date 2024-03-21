using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using MdxLib.ModelFormats.Attacher;
using MdxLib.ModelFormats.Mdl.Token;
using MdxLib.Primitives;

namespace MdxLib.ModelFormats.Mdl
{
	internal sealed class CLoader
	{
		private const int FirstGroupIndex = 4;

		private int _Line = 1;

		private string _Name = "";

		private bool _Eof;

		private CAttacherContainer _Attacher;

		private string InputString = "";

		private string ExpressionString = "";

		private Regex Expression;

		private bool AlreadyParsed;

		private EType CurrentType;

		private Match CurrentMatch;

		private Group CurrentGroup;

		public int Line => _Line;

		public string Name => _Name;

		public bool Eof => _Eof;

		public CAttacherContainer Attacher => _Attacher;

		public CLoader(string Name, Stream Stream)
		{
			_Name = Name;
			_Attacher = new CAttacherContainer();
			AppendPattern("newline", "\\n");
			AppendPattern("whitespace", "[\\t\\r ]+");
			AppendPattern("metacomment", "//\\<\\?xml(.|\\n)*?//\\</meta\\>");
			AppendPattern("comment", "//.*?\\n|/\\*(.|\\n)*?\\*/");
			AppendPattern("colon", ":");
			AppendPattern("separator", ",");
			AppendPattern("word", "[a-zA-Z_][a-zA-Z0-9_]*");
			AppendPattern("float", "[+-]?\\d*\\.\\d+[Ee][+-]?\\d+|[+-]?\\d+[Ee][+-]?\\d+|[+-]?\\d*\\.\\d+");
			AppendPattern("integer", "[+-]?\\d+");
			AppendPattern("string", "\"([^\"\n\\\\]|\\\\.)*?\"");
			AppendPattern("bracket_rl", "\\(");
			AppendPattern("bracket_rr", "\\)");
			AppendPattern("bracket_sl", "\\[");
			AppendPattern("bracket_sr", "\\]");
			AppendPattern("bracket_al", "\\<");
			AppendPattern("bracket_ar", "\\>");
			AppendPattern("bracket_cl", "{");
			AppendPattern("bracket_cr", "}");
			AppendPattern("unknown", ".+?");
			Expression = new Regex(ExpressionString);
			using StreamReader streamReader = new StreamReader(Stream, CConstants.TextEncoding, detectEncodingFromByteOrderMarks: true);
			InputString = streamReader.ReadToEnd();
		}

		public EType ReadToken()
		{
			ReadNextToken();
			return CurrentType;
		}

		public string ReadMetaData()
		{
			ReadNextToken();
			if (CurrentType != EType.MetaComment)
			{
				throw new Exception("Syntax error at line " + Line + ", expected a meta comment, got \"" + CurrentGroup.Value + "\"!");
			}
			StringBuilder stringBuilder = new StringBuilder();
			string[] array = CurrentGroup.Value.Split(new string[1] { "\n" }, StringSplitOptions.None);
			string[] array2 = array;
			foreach (string text in array2)
			{
				string text2 = text.Trim();
				if (text2.StartsWith("//"))
				{
					text2 = text2.Remove(0, 2);
				}
				stringBuilder.Append(text2.Trim());
			}
			return stringBuilder.ToString();
		}

		public string ReadWord()
		{
			ReadNextToken();
			if (CurrentType != EType.Word)
			{
				throw new Exception("Syntax error at line " + Line + ", expected a word, got \"" + CurrentGroup.Value + "\"!");
			}
			return CurrentGroup.Value.ToLower();
		}

		public int ReadId()
		{
			ReadNextToken();
			switch (CurrentType)
			{
			case EType.Word:
			{
				string text = CurrentGroup.Value.ToLower();
				switch (text)
				{
				case "none":
				case "multiple":
					return -1;
				default:
					throw new Exception("Syntax error at line " + Line + ", unknown ID \"" + text + "\"!");
				}
			}
			case EType.Integer:
				return int.Parse(CurrentGroup.Value);
			default:
				throw new Exception("Syntax error at line " + Line + ", expected an ID, got \"" + CurrentGroup.Value + "\"!");
			}
		}

		public int ReadInteger()
		{
			ReadNextToken();
			if (CurrentType != EType.Integer)
			{
				throw new Exception("Syntax error at line " + Line + ", expected an integer, got \"" + CurrentGroup.Value + "\"!");
			}
			return int.Parse(CurrentGroup.Value);
		}

		public float ReadFloat()
		{
			ReadNextToken();
			if (CurrentType != EType.Integer && CurrentType != EType.Float)
			{
				throw new Exception("Syntax error at line " + Line + ", expected a float, got \"" + CurrentGroup.Value + "\"!");
			}
			return float.Parse(CurrentGroup.Value, CConstants.NumberFormat);
		}

		public string ReadString()
		{
			ReadNextToken();
			if (CurrentType != EType.String)
			{
				throw new Exception("Syntax error at line " + Line + ", expected a string, got \"" + CurrentGroup.Value + "\"!");
			}
			string text = CurrentGroup.Value;
			if (text.StartsWith("\""))
			{
				text = text.Remove(0, 1);
			}
			if (text.EndsWith("\""))
			{
				text = text.Remove(text.Length - 1, 1);
			}
			return text.Replace("\\\"", "\"");
		}

		public CVector2 ReadVector2()
		{
			ExpectToken(EType.CurlyBracketLeft);
			float x = ReadFloat();
			ExpectToken(EType.Separator);
			float y = ReadFloat();
			ExpectToken(EType.CurlyBracketRight);
			return new CVector2(x, y);
		}

		public CVector3 ReadVector3()
		{
			ExpectToken(EType.CurlyBracketLeft);
			float x = ReadFloat();
			ExpectToken(EType.Separator);
			float y = ReadFloat();
			ExpectToken(EType.Separator);
			float z = ReadFloat();
			ExpectToken(EType.CurlyBracketRight);
			return new CVector3(x, y, z);
		}

		public CVector4 ReadVector4()
		{
			ExpectToken(EType.CurlyBracketLeft);
			float x = ReadFloat();
			ExpectToken(EType.Separator);
			float y = ReadFloat();
			ExpectToken(EType.Separator);
			float z = ReadFloat();
			ExpectToken(EType.Separator);
			float w = ReadFloat();
			ExpectToken(EType.CurlyBracketRight);
			return new CVector4(x, y, z, w);
		}

		public CVector3 ReadColor()
		{
			ExpectToken(EType.CurlyBracketLeft);
			float z = ReadFloat();
			ExpectToken(EType.Separator);
			float y = ReadFloat();
			ExpectToken(EType.Separator);
			float x = ReadFloat();
			ExpectToken(EType.CurlyBracketRight);
			return new CVector3(x, y, z);
		}

		public void ExpectToken(EType ExpectedType)
		{
			EType eType = ReadToken();
			if (eType != ExpectedType)
			{
				throw new Exception("Syntax error at line " + Line + ", expected " + TokenToText(eType) + ", got \"" + CurrentGroup.Value + "\"!");
			}
		}

		public void ExpectWord(string ExpectedWord)
		{
			string text = ReadWord();
			if (text != ExpectedWord.ToLower())
			{
				throw new Exception("Syntax error at line " + Line + ", expected \"" + ExpectedWord + "\", got \"" + text + "\"!");
			}
		}

		public EType PeekToken()
		{
			PeekNextToken();
			return CurrentType;
		}

		private void ReadNextToken()
		{
			if (!ParseNextToken())
			{
				throw new Exception("Unexpected EOF reached at line " + Line + "!");
			}
		}

		private void PeekNextToken()
		{
			if (!ParseNextToken())
			{
				throw new Exception("Unexpected EOF reached at line " + Line + "!");
			}
			AlreadyParsed = true;
		}

		private bool ParseNextToken()
		{
			if (AlreadyParsed)
			{
				AlreadyParsed = false;
				return true;
			}
			while (true)
			{
				CurrentMatch = ((CurrentMatch != null) ? CurrentMatch.NextMatch() : Expression.Match(InputString));
				if (!CurrentMatch.Success)
				{
					break;
				}
				for (int i = 4; i < CurrentMatch.Groups.Count; i++)
				{
					CurrentGroup = CurrentMatch.Groups[i];
					if (!CurrentGroup.Success)
					{
						continue;
					}
					CurrentType = PatternNameToType(Expression.GroupNameFromNumber(i));
					switch (CurrentType)
					{
					case EType.WhiteSpace:
						break;
					case EType.NewLine:
						_Line++;
						break;
					case EType.MetaComment:
					{
						string value2 = CurrentGroup.Value;
						foreach (char c2 in value2)
						{
							if (c2 == '\n')
							{
								_Line++;
							}
						}
						return true;
					}
					case EType.Comment:
					{
						string value = CurrentGroup.Value;
						foreach (char c in value)
						{
							if (c == '\n')
							{
								_Line++;
							}
						}
						break;
					}
					case EType.Unknown:
						throw new Exception("Syntax error at line " + Line + ", unknown token \"" + CurrentGroup.Value + "\"!");
					default:
						return true;
					}
					break;
				}
			}
			_Eof = true;
			CurrentType = EType.Unknown;
			CurrentMatch = null;
			CurrentGroup = null;
			return false;
		}

		private void AppendPattern(string Name, string Pattern)
		{
			if (ExpressionString != "")
			{
				ExpressionString += "|";
			}
			string expressionString = ExpressionString;
			ExpressionString = expressionString + "(?<" + Name + ">" + Pattern + ")";
		}

		private EType PatternNameToType(string Name)
		{
			return Name switch
			{
				"newline" => EType.NewLine, 
				"whitespace" => EType.WhiteSpace, 
				"metacomment" => EType.MetaComment, 
				"comment" => EType.Comment, 
				"colon" => EType.Colon, 
				"separator" => EType.Separator, 
				"word" => EType.Word, 
				"float" => EType.Float, 
				"integer" => EType.Integer, 
				"string" => EType.String, 
				"bracket_rl" => EType.RoundBracketLeft, 
				"bracket_rr" => EType.RoundBracketRight, 
				"bracket_sl" => EType.SquareBracketLeft, 
				"bracket_sr" => EType.SquareBracketRight, 
				"bracket_al" => EType.AngleBracketLeft, 
				"bracket_ar" => EType.AngleBracketRight, 
				"bracket_cl" => EType.CurlyBracketLeft, 
				"bracket_cr" => EType.CurlyBracketRight, 
				"unknown" => EType.Unknown, 
				_ => EType.Unknown, 
			};
		}

		private string TokenToText(EType Type)
		{
			return Type switch
			{
				EType.NewLine => "a newline", 
				EType.WhiteSpace => "whitespace", 
				EType.MetaComment => "a meta comment", 
				EType.Comment => "a comment", 
				EType.Colon => "\":\"", 
				EType.Separator => "\",\"", 
				EType.Word => "a word", 
				EType.Float => "a float", 
				EType.Integer => "an integer", 
				EType.String => "a string", 
				EType.RoundBracketLeft => "\"(\"", 
				EType.RoundBracketRight => "\")\"", 
				EType.SquareBracketLeft => "\"[\"", 
				EType.SquareBracketRight => "\"]\"", 
				EType.AngleBracketLeft => "\"<\"", 
				EType.AngleBracketRight => "\">\"", 
				EType.CurlyBracketLeft => "\"{\"", 
				EType.CurlyBracketRight => "\"}\"", 
				EType.Unknown => "<unknown>", 
				_ => "<unknown>", 
			};
		}
	}
}
