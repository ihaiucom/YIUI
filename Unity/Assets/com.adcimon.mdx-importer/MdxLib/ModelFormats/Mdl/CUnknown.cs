using MdxLib.ModelFormats.Mdl.Token;
using MdxLib.Primitives;

namespace MdxLib.ModelFormats.Mdl
{
	internal abstract class CUnknown
	{
		public CUnknown()
		{
		}

		public int LoadId(CLoader Loader)
		{
			int result = Loader.ReadId();
			Loader.ExpectToken(EType.Separator);
			return result;
		}

		public bool LoadBoolean(CLoader Loader)
		{
			Loader.ExpectToken(EType.Separator);
			return true;
		}

		public int LoadInteger(CLoader Loader)
		{
			int result = Loader.ReadInteger();
			Loader.ExpectToken(EType.Separator);
			return result;
		}

		public float LoadFloat(CLoader Loader)
		{
			float result = Loader.ReadFloat();
			Loader.ExpectToken(EType.Separator);
			return result;
		}

		public string LoadString(CLoader Loader)
		{
			string result = Loader.ReadString();
			Loader.ExpectToken(EType.Separator);
			return result;
		}

		public string LoadWord(CLoader Loader)
		{
			string result = Loader.ReadWord();
			Loader.ExpectToken(EType.Separator);
			return result;
		}

		public CVector2 LoadVector2(CLoader Loader)
		{
			CVector2 result = Loader.ReadVector2();
			Loader.ExpectToken(EType.Separator);
			return result;
		}

		public CVector3 LoadVector3(CLoader Loader)
		{
			CVector3 result = Loader.ReadVector3();
			Loader.ExpectToken(EType.Separator);
			return result;
		}

		public CVector4 LoadVector4(CLoader Loader)
		{
			CVector4 result = Loader.ReadVector4();
			Loader.ExpectToken(EType.Separator);
			return result;
		}

		public CVector3 LoadColor(CLoader Loader)
		{
			CVector3 result = Loader.ReadColor();
			Loader.ExpectToken(EType.Separator);
			return result;
		}

		public void SaveId(CSaver Saver, string Name, int Value)
		{
			SaveId(Saver, Name, Value, ECondition.Always, UseMultipleAsNone: false);
		}

		public void SaveId(CSaver Saver, string Name, int Value, ECondition Condition)
		{
			SaveId(Saver, Name, Value, Condition, UseMultipleAsNone: false);
		}

		public void SaveId(CSaver Saver, string Name, int Value, bool UseMultipleAsNone)
		{
			SaveId(Saver, Name, Value, ECondition.Always, UseMultipleAsNone);
		}

		public void SaveId(CSaver Saver, string Name, int Value, ECondition Condition, bool UseMultipleAsNone)
		{
			if (Condition != ECondition.NotInvalidId || Value != -1)
			{
				Saver.WriteTabs();
				Saver.WriteWord(Name + " ");
				Saver.WriteWord((Value != -1) ? Value.ToString() : (UseMultipleAsNone ? "Multiple" : "None"));
				Saver.WriteLine(",");
			}
		}

		public void SaveBoolean(CSaver Saver, string Name, bool Value)
		{
			if (Value)
			{
				Saver.WriteTabs();
				Saver.WriteLine(Name + ",");
			}
		}

		public void SaveInteger(CSaver Saver, string Name, int Value)
		{
			SaveInteger(Saver, Name, Value, ECondition.Always);
		}

		public void SaveInteger(CSaver Saver, string Name, int Value, ECondition Condition)
		{
			switch (Condition)
			{
			case ECondition.NotZero:
				if (Value == 0)
				{
					return;
				}
				break;
			case ECondition.NotOne:
				if (Value == 1)
				{
					return;
				}
				break;
			}
			Saver.WriteTabs();
			Saver.WriteWord(Name + " ");
			Saver.WriteInteger(Value);
			Saver.WriteLine(",");
		}

		public void SaveFloat(CSaver Saver, string Name, float Value)
		{
			SaveFloat(Saver, Name, Value, ECondition.Always);
		}

		public void SaveFloat(CSaver Saver, string Name, float Value, ECondition Condition)
		{
			switch (Condition)
			{
			case ECondition.NotZero:
				if (Value == 0f)
				{
					return;
				}
				break;
			case ECondition.NotOne:
				if (Value == 1f)
				{
					return;
				}
				break;
			}
			Saver.WriteTabs();
			Saver.WriteWord(Name + " ");
			Saver.WriteFloat(Value);
			Saver.WriteLine(",");
		}

		public void SaveString(CSaver Saver, string Name, string Value)
		{
			SaveString(Saver, Name, Value, ECondition.Always);
		}

		public void SaveString(CSaver Saver, string Name, string Value, ECondition Condition)
		{
			if (Condition != ECondition.NotEmpty || !(Value == ""))
			{
				Saver.WriteTabs();
				Saver.WriteWord(Name + " ");
				Saver.WriteString(Value);
				Saver.WriteLine(",");
			}
		}

		public void SaveVector2(CSaver Saver, string Name, CVector2 Value)
		{
			SaveVector2(Saver, Name, Value, ECondition.Always);
		}

		public void SaveVector2(CSaver Saver, string Name, CVector2 Value, ECondition Condition)
		{
			switch (Condition)
			{
			case ECondition.NotZero:
				if (Value.X == 0f && Value.Y == 0f)
				{
					return;
				}
				break;
			case ECondition.NotOne:
				if (Value.X == 1f && Value.Y == 1f)
				{
					return;
				}
				break;
			}
			Saver.WriteTabs();
			Saver.WriteWord(Name + " ");
			Saver.WriteVector2(Value);
			Saver.WriteLine(",");
		}

		public void SaveVector3(CSaver Saver, string Name, CVector3 Value)
		{
			SaveVector3(Saver, Name, Value, ECondition.Always);
		}

		public void SaveVector3(CSaver Saver, string Name, CVector3 Value, ECondition Condition)
		{
			switch (Condition)
			{
			case ECondition.NotZero:
				if (Value.X == 0f && Value.Y == 0f && Value.Z == 0f)
				{
					return;
				}
				break;
			case ECondition.NotOne:
				if (Value.X == 1f && Value.Y == 1f && Value.Z == 1f)
				{
					return;
				}
				break;
			}
			Saver.WriteTabs();
			Saver.WriteWord(Name + " ");
			Saver.WriteVector3(Value);
			Saver.WriteLine(",");
		}

		public void SaveVector4(CSaver Saver, string Name, CVector4 Value)
		{
			SaveVector4(Saver, Name, Value, ECondition.Always);
		}

		public void SaveVector4(CSaver Saver, string Name, CVector4 Value, ECondition Condition)
		{
			switch (Condition)
			{
			case ECondition.NotZero:
				if (Value.X == 0f && Value.Y == 0f && Value.Z == 0f && Value.W == 0f)
				{
					return;
				}
				break;
			case ECondition.NotOne:
				if (Value.X == 1f && Value.Y == 1f && Value.Z == 1f && Value.W == 1f)
				{
					return;
				}
				break;
			case ECondition.NotDefaultQuaternion:
				if (Value.X == 0f && Value.Y == 0f && Value.Z == 0f && Value.W == 1f)
				{
					return;
				}
				break;
			}
			Saver.WriteTabs();
			Saver.WriteWord(Name + " ");
			Saver.WriteVector4(Value);
			Saver.WriteLine(",");
		}

		public void SaveColor(CSaver Saver, string Name, CVector3 Value)
		{
			SaveColor(Saver, Name, Value, ECondition.Always);
		}

		public void SaveColor(CSaver Saver, string Name, CVector3 Value, ECondition Condition)
		{
			switch (Condition)
			{
			case ECondition.NotZero:
				if (Value.X == 0f && Value.Y == 0f && Value.Z == 0f)
				{
					return;
				}
				break;
			case ECondition.NotOne:
				if (Value.X == 1f && Value.Y == 1f && Value.Z == 1f)
				{
					return;
				}
				break;
			}
			Saver.WriteTabs();
			Saver.WriteWord(Name + " ");
			Saver.WriteColor(Value);
			Saver.WriteLine(",");
		}
	}
}
