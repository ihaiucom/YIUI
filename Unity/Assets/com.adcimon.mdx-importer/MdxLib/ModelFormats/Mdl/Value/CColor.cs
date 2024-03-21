using MdxLib.Primitives;

namespace MdxLib.ModelFormats.Mdl.Value
{
	internal sealed class CColor : CUnknown, IValue<MdxLib.Primitives.CVector3>
	{
		private static class CSingleton
		{
			public static CColor Instance;

			static CSingleton()
			{
				Instance = new CColor();
			}
		}

		public static CColor Instance => CSingleton.Instance;

		private CColor()
		{
		}

		public MdxLib.Primitives.CVector3 Read(CLoader Loader)
		{
			return Loader.ReadColor();
		}

		public void Write(CSaver Saver, MdxLib.Primitives.CVector3 Value)
		{
			Saver.WriteColor(Value);
		}

		public bool ValidCondition(MdxLib.Primitives.CVector3 Value, ECondition Condition)
		{
			switch (Condition)
			{
			case ECondition.NotZero:
				if (Value.X == 0f && Value.Y == 0f && Value.Z == 0f)
				{
					return false;
				}
				break;
			case ECondition.NotOne:
				if (Value.X == 1f && Value.Y == 1f && Value.Z == 1f)
				{
					return false;
				}
				break;
			}
			return true;
		}
	}
}
