using MdxLib.Model;

namespace MdxLib.Animator
{
	/// <summary>
	/// An immutable time. Defines a point in time during a sequence
	/// (or global sequence).
	/// </summary>
	public sealed class CTime
	{
		private int _Time;

		private int _IntervalStart = int.MinValue;

		private int _IntervalEnd = int.MaxValue;

		/// <summary>
		/// Retrieves the time.
		/// </summary>
		public int Time => _Time;

		/// <summary>
		/// Retrieves the time when the animation starts.
		/// </summary>
		public int IntervalStart => _IntervalStart;

		/// <summary>
		/// Retrieves the time when the animation ends.
		/// </summary>
		public int IntervalEnd => _IntervalEnd;

		/// <summary>
		/// Default constructor.
		/// </summary>
		public CTime()
		{
		}

		/// <summary>
		/// Copy constructor.
		/// </summary>
		/// <param name="Time">The time to copy from</param>
		public CTime(CTime Time)
		{
			_Time = Time._Time;
			_IntervalStart = Time._IntervalStart;
			_IntervalEnd = Time._IntervalEnd;
		}

		/// <summary>
		/// Parameterized constructor.
		/// </summary>
		/// <param name="Time">The time to use</param>
		public CTime(int Time)
		{
			_Time = Time;
		}

		/// <summary>
		/// Parameterized constructor.
		/// </summary>
		/// <param name="Time">The time to use</param>
		/// <param name="IntervalStart">The time at which the animation starts</param>
		/// <param name="IntervalEnd">The time at which the animation ends</param>
		public CTime(int Time, int IntervalStart, int IntervalEnd)
		{
			_Time = Time;
			_IntervalStart = IntervalStart;
			_IntervalEnd = IntervalEnd;
		}

		/// <summary>
		/// Parameterized constructor.
		/// </summary>
		/// <param name="Time">The time to use</param>
		/// <param name="Sequence">The sequence defining when the animation starts and ends</param>
		public CTime(int Time, CSequence Sequence)
		{
			_Time = Time;
			_IntervalStart = Sequence.IntervalStart;
			_IntervalEnd = Sequence.IntervalEnd;
		}

		/// <summary>
		/// Parameterized constructor.
		/// </summary>
		/// <param name="Time">The time to use</param>
		/// <param name="GlobalSequence">The global sequence defining when the animation starts and ends</param>
		public CTime(int Time, CGlobalSequence GlobalSequence)
		{
			_Time = Time;
			_IntervalStart = 0;
			_IntervalEnd = GlobalSequence.Duration;
		}
	}
}
