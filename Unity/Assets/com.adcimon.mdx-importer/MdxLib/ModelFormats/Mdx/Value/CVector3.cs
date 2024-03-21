using MdxLib.Primitives;

namespace MdxLib.ModelFormats.Mdx.Value
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
	}
}
