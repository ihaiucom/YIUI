using System;

namespace MdxLib.Primitives
{
	/// <summary>
	/// An immutable 2-dimensional vector, usually used for coordinates.
	/// </summary>
	public sealed class CVector2 : ICloneable
	{
		private float _X;

		private float _Y;

		/// <summary>
		/// Retrieves the X-coordinate.
		/// </summary>
		public float X => _X;

		/// <summary>
		/// Retrieves the Y-coordinate.
		/// </summary>
		public float Y => _Y;

		/// <summary>
		/// Default constructor.
		/// </summary>
		public CVector2()
		{
		}

		/// <summary>
		/// Copy constructor.
		/// </summary>
		/// <param name="Vector">The vector to copy from</param>
		public CVector2(CVector2 Vector)
		{
			_X = Vector._X;
			_Y = Vector._Y;
		}

		/// <summary>
		/// Parameterized constructor.
		/// </summary>
		/// <param name="X">The X-coordinate to use</param>
		/// <param name="Y">The Y-coordinate to use</param>
		public CVector2(float X, float Y)
		{
			_X = X;
			_Y = Y;
		}

		/// <summary>
		/// Clones the vector.
		/// </summary>
		/// <returns>The cloned vector</returns>
		public object Clone()
		{
			return new CVector2(this);
		}

		/// <summary>
		/// Generates a string version of the vector.
		/// </summary>
		/// <returns>The generated string</returns>
		public override string ToString()
		{
			return "{ " + _X + ", " + _Y + " }";
		}
	}
}
