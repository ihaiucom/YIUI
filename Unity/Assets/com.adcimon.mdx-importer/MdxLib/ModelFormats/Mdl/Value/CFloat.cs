namespace MdxLib.ModelFormats.Mdl.Value
{
	internal sealed class CFloat : CUnknown, IValue<float>
	{
		private static class CSingleton
		{
			public static CFloat Instance;

			static CSingleton()
			{
				Instance = new CFloat();
			}
		}

		public static CFloat Instance => CSingleton.Instance;

		private CFloat()
		{
		}

		public float Read(CLoader Loader)
		{
			return Loader.ReadFloat();
		}

		public void Write(CSaver Saver, float Value)
		{
			Saver.WriteFloat(Value);
		}

		public bool ValidCondition(float Value, ECondition Condition)
		{
			switch (Condition)
			{
			case ECondition.NotZero:
				if (Value == 0f)
				{
					return false;
				}
				break;
			case ECondition.NotOne:
				if (Value == 1f)
				{
					return false;
				}
				break;
			}
			return true;
		}
	}
}
