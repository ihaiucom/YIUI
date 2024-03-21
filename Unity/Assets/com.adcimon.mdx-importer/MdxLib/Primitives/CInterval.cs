using System;

namespace MdxLib.Primitives
{
	/// <summary>
	/// An immutable interval. Used by particle emitters to define how
	/// the particle's sprites are animated.
	/// </summary>
	public sealed class CInterval : ICloneable
	{
		private int _Start;

		private int _End;

		private int _Repeat;

		/// <summary>
		/// Retrieves the start index.
		/// </summary>
		public int Start => _Start;

		/// <summary>
		/// Retrieves the end index.
		/// </summary>
		public int End => _End;

		/// <summary>
		/// Retrieves the repeat count.
		/// </summary>
		public int Repeat => _Repeat;

		/// <summary>
		/// Default constructor.
		/// </summary>
		public CInterval()
		{
		}

		/// <summary>
		/// Copy constructor.
		/// </summary>
		/// <param name="Interval">The interval to copy from</param>
		public CInterval(CInterval Interval)
		{
			_Start = Interval._Start;
			_End = Interval._End;
			_Repeat = Interval._Repeat;
		}

		/// <summary>
		/// Parameterized constructor.
		/// </summary>
		/// <param name="Start">The start index to use</param>
		/// <param name="End">The end index to use</param>
		/// <param name="Repeat">The repeat count to use</param>
		public CInterval(int Start, int End, int Repeat)
		{
			_Start = Start;
			_End = End;
			_Repeat = Repeat;
		}

		/// <summary>
		/// Clones the interval.
		/// </summary>
		/// <returns>The cloned interval</returns>
		public object Clone()
		{
			return new CInterval(this);
		}

		/// <summary>
		/// Generates a string version of the interval.
		/// </summary>
		/// <returns>The generated string</returns>
		public override string ToString()
		{
			return "{ " + _Start + ", " + _End + ", " + _Repeat + " }";
		}
	}
}
