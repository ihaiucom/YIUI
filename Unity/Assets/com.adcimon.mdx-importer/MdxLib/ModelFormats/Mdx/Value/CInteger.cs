namespace MdxLib.ModelFormats.Mdx.Value
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
			return Loader.ReadInt32();
		}

		public void Write(CSaver Saver, int Value)
		{
			Saver.WriteInt32(Value);
		}
	}
}
