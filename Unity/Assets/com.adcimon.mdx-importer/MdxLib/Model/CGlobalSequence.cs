namespace MdxLib.Model
{
	/// <summary>
	/// A global sequence class. Defines a sequence which is not tied to
	/// the common Stand/Walk sequences and totally independent from them.
	/// One example is the Gyrocopter's propeller.
	/// </summary>
	public sealed class CGlobalSequence : CObject<CGlobalSequence>
	{
		private int _Duration;

		/// <summary>
		/// Gets or sets the duration. This is the length of the sequence.
		/// </summary>
		public int Duration
		{
			get
			{
				return _Duration;
			}
			set
			{
				AddSetObjectFieldCommand("_Duration", value);
				_Duration = value;
			}
		}

		/// <summary>
		/// Parameterized constructor.
		/// </summary>
		/// <param name="Model">The model to be associated with this global sequence</param>
		public CGlobalSequence(CModel Model)
			: base(Model)
		{
		}

		/// <summary>
		/// Generates a string version of the global sequence.
		/// </summary>
		/// <returns>The generated string</returns>
		public override string ToString()
		{
			return "Global Sequence #" + base.ObjectId;
		}
	}
}
