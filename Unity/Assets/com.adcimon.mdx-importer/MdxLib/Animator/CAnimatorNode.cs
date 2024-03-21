namespace MdxLib.Animator
{
	/// <summary>
	/// A node for the animator. Animator values are interpolated
	/// between these nodes.
	/// </summary>
	/// <typeparam name="T">The value type</typeparam>
	public sealed class CAnimatorNode<T> where T : new()
	{
		private int _Time;

		private T _Value = default(T);

		private T _InTangent = default(T);

		private T _OutTangent = default(T);

		/// <summary>
		/// Retrieves the time.
		/// </summary>
		public int Time => _Time;

		/// <summary>
		/// Retrieves the value.
		/// </summary>
		public T Value => _Value;

		/// <summary>
		/// Retrieves the incoming tangent.
		/// </summary>
		public T InTangent => _InTangent;

		/// <summary>
		/// Retrieves the outgoing tangent.
		/// </summary>
		public T OutTangent => _OutTangent;

		/// <summary>
		/// Default constructor.
		/// </summary>
		public CAnimatorNode()
		{
			_Value = new T();
			_InTangent = new T();
			_OutTangent = new T();
		}

		/// <summary>
		/// Copy constructor.
		/// </summary>
		/// <param name="Node">The node to copy from</param>
		public CAnimatorNode(CAnimatorNode<T> Node)
		{
			_Time = Node._Time;
			_Value = Node._Value;
			_InTangent = Node._InTangent;
			_OutTangent = Node._OutTangent;
		}

		/// <summary>
		/// Parameterized constructor.
		/// </summary>
		/// <param name="Time">The time to use</param>
		/// <param name="Value">The value to use</param>
		public CAnimatorNode(int Time, T Value)
		{
			_Time = Time;
			_Value = Value;
			_InTangent = new T();
			_OutTangent = new T();
		}

		/// <summary>
		/// Parameterized constructor.
		/// </summary>
		/// <param name="Time">The time to use</param>
		/// <param name="Value">The value to use</param>
		/// <param name="InTangent">The in tangent to use</param>
		/// <param name="OutTangent">The out tangent to use</param>
		public CAnimatorNode(int Time, T Value, T InTangent, T OutTangent)
		{
			_Time = Time;
			_Value = Value;
			_InTangent = InTangent;
			_OutTangent = OutTangent;
		}
	}
}
