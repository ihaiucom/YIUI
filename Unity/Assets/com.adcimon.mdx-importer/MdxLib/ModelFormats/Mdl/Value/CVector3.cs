using MdxLib.Primitives;

namespace MdxLib.ModelFormats.Mdl.Value
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

		public MdxLib.Primitives.CVector3 Read(CLoader Loader)
		{
			return Loader.ReadVector3();
		}

		public void Write(CSaver Saver, MdxLib.Primitives.CVector3 Value)
		{
			Saver.WriteVector3(Value);
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
