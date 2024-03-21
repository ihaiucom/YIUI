using MdxLib.Primitives;

namespace MdxLib.ModelFormats.Mdx.Value
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
	}
}
