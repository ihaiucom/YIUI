namespace MdxLib.ModelFormats.Mdl.Value
{
	internal interface IValue<T> where T : new()
	{
		T Read(CLoader Loader);

		void Write(CSaver Saver, T Value);

		bool ValidCondition(T Value, ECondition Condition);
	}
}
