namespace MdxLib.ModelFormats.Mdl.Value
{
	internal sealed class CInteger : CUnknown, IValue<int>
	{
		private static class CSingleton
		{
			public static CInteger Instance;

			static CSingleton()
			{
				Instance = new CInteger();
			}
		}

		public static CInteger Instance => CSingleton.Instance;

		private CInteger()
		{
		}

		public int Read(CLoader Loader)
		{
			return Loader.ReadInteger();
		}

		public void Write(CSaver Saver, int Value)
		{
			Saver.WriteInteger(Value);
		}

		public bool ValidCondition(int Value, ECondition Condition)
		{
			switch (Condition)
			{
			case ECondition.NotZero:
				if (Value == 0)
				{
					return false;
				}
				break;
			case ECondition.NotOne:
				if (Value == 1)
				{
					return false;
				}
				break;
			case ECondition.NotInvalidId:
				if (Value == -1)
				{
					return false;
				}
				break;
			}
			return true;
		}
	}
}
