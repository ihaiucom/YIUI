namespace MdxLib.ModelFormats.Mdx.Value
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
	}
}
