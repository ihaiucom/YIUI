namespace MdxLib.ModelFormats.Mdx.Value
{
	internal interface IValue<T> where T : new()
	{
		T Read(CLoader Loader);

		void Write(CSaver Saver, T Value);
	}
}
