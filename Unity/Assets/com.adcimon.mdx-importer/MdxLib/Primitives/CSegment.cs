using System;

namespace MdxLib.Primitives
{
	/// <summary>
	/// An immutable segment. Used by particle emitters to define how
	/// the particles are animated.
	/// </summary>
	public sealed class CSegment : ICloneable
	{
		private CVector3 _Color;

		private float _Alpha;

		private float _Scaling;

		/// <summary>
		/// Retrieves the color.
		/// </summary>
		public CVector3 Color => _Color;

		/// <summary>
		/// Retrieves the alpha (solidity).
		/// </summary>
		public float Alpha => _Alpha;

		/// <summary>
		/// Retrieves the scaling.
		/// </summary>
		public float Scaling => _Scaling;

		/// <summary>
		/// Default constructor.
		/// </summary>
		public CSegment()
		{
			_Color = new CVector3(1f, 1f, 1f);
		}

		/// <summary>
		/// Copy constructor.
		/// </summary>
		/// <param name="Segment">The segment to copy from</param>
		public CSegment(CSegment Segment)
		{
			_Color = Segment._Color;
			_Alpha = Segment._Alpha;
			_Scaling = Segment._Scaling;
		}

		/// <summary>
		/// Parameterized constructor.
		/// </summary>
		/// <param name="Color">The color to use</param>
		/// <param name="Alpha">The alpha (solidity) to use</param>
		/// <param name="Scaling">The scaling to use</param>
		public CSegment(CVector3 Color, float Alpha, float Scaling)
		{
			_Color = Color;
			_Alpha = Alpha;
			_Scaling = Scaling;
		}

		/// <summary>
		/// Clones the segment.
		/// </summary>
		/// <returns>The cloned segment</returns>
		public object Clone()
		{
			return new CSegment(this);
		}

		/// <summary>
		/// Generates a string version of the segment.
		/// </summary>
		/// <returns>The generated string</returns>
		public override string ToString()
		{
			return "";
		}
	}
}
