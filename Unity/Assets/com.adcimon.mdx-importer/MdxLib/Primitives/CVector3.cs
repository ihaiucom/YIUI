using System;

namespace MdxLib.Primitives
{
	/// <summary>
	/// An immutable 3-dimensional vector, usually used for coordinates.
	/// If used for colors then X = Red, Y = Green and Z = Blue.
	/// </summary>
	public sealed class CVector3 : ICloneable
	{
		private float _X;

		private float _Y;

		private float _Z;

		/// <summary>
		/// Retrieves the X-coordinate (Red if it's a color).
		/// </summary>
		public float X => _X;

		/// <summary>
		/// Retrieves the Y-coordinate (Green if it's a color).
		/// </summary>
		public float Y => _Y;

		/// <summary>
		/// Retrieves the Z-coordinate (Blue if it's a color).
		/// </summary>
		public float Z => _Z;

		/// <summary>
		/// Default constructor.
		/// </summary>
		public CVector3()
		{
		}

		/// <summary>
		/// Copy constructor.
		/// </summary>
		/// <param name="Vector">The vector to copy from</param>
		public CVector3(CVector3 Vector)
		{
			_X = Vector._X;
			_Y = Vector._Y;
			_Z = Vector._Z;
		}

		/// <summary>
		/// Parameterized constructor.
		/// </summary>
		/// <param name="X">The X-coordinate to use (Red if it's a color)</param>
		/// <param name="Y">The Y-coordinate to use (Green if it's a color)</param>
		/// <param name="Z">The Z-coordinate to use (Blue if it's a color)</param>
		public CVector3(float X, float Y, float Z)
		{
			_X = X;
			_Y = Y;
			_Z = Z;
		}

		/// <summary>
		/// Clones the vector.
		/// </summary>
		/// <returns>The cloned vector</returns>
		public object Clone()
		{
			return new CVector3(this);
		}

		/// <summary>
		/// Generates a string version of the vector.
		/// </summary>
		/// <returns>The generated string</returns>
		public override string ToString()
		{
			return "{ " + _X + ", " + _Y + ", " + _Z + " }";
		}
	}
}
