using MdxLib.Primitives;

namespace MdxLib.ModelFormats.Mdx.Value
{
	internal sealed class CVector2 : CUnknown, IValue<MdxLib.Primitives.CVector2>
	{
		private static class CSingleton
		{
			public static CVector2 Instance;

			static CSingleton()
			{
				Instance = new CVector2();
			}
		}

		public static CVector2 Instance => CSingleton.Instance;

		private CVector2()
		{
		}

		public MdxLib.Primitives.CVector2 Read(CLoader Loader)
		{
			return Loader.ReadVector2();
		}

		public void Write(CSaver Saver, MdxLib.Primitives.CVector2 Value)
		{
			Saver.WriteVector2(Value);
		}
	}
}
