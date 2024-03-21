using MdxLib.Primitives;

namespace MdxLib.ModelFormats.Mdl.Value
{
	internal sealed class CVector4 : CUnknown, IValue<MdxLib.Primitives.CVector4>
	{
		private static class CSingleton
		{
			public static CVector4 Instance;

			static CSingleton()
			{
				Instance = new CVector4();
			}
		}

		public static CVector4 Instance => CSingleton.Instance;

		private CVector4()
		{
		}

		public MdxLib.Primitives.CVector4 Read(CLoader Loader)
		{
			return Loader.ReadVector4();
		}

		public void Write(CSaver Saver, MdxLib.Primitives.CVector4 Value)
		{
			Saver.WriteVector4(Value);
		}

		public bool ValidCondition(MdxLib.Primitives.CVector4 Value, ECondition Condition)
		{
			switch (Condition)
			{
			case ECondition.NotZero:
				if (Value.X == 0f && Value.Y == 0f && Value.Z == 0f && Value.W == 0f)
				{
					return false;
				}
				break;
			case ECondition.NotOne:
				if (Value.X == 1f && Value.Y == 1f && Value.Z == 1f && Value.W == 1f)
				{
					return false;
				}
				break;
			case ECondition.NotDefaultQuaternion:
				if (Value.X == 0f && Value.Y == 0f && Value.Z == 0f && Value.W == 1f)
				{
					return false;
				}
				break;
			}
			return true;
		}
	}
}
