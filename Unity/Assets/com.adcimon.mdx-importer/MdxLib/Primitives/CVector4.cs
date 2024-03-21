using System;

namespace MdxLib.Primitives
{
	/// <summary>
	/// An immutable 4-dimensional vector, usually used for quaternions.
	/// </summary>
	public sealed class CVector4 : ICloneable
	{
		private float _X;

		private float _Y;

		private float _Z;

		private float _W;

		/// <summary>
		/// Retrieves the X-coordinate.
		/// </summary>
		public float X => _X;

		/// <summary>
		/// Retrieves the Y-coordinate.
		/// </summary>
		public float Y => _Y;

		/// <summary>
		/// Retrieves the Z-coordinate.
		/// </summary>
		public float Z => _Z;

		/// <summary>
		/// Retrieves the W-coordinate.
		/// </summary>
		public float W => _W;

		/// <summary>
		/// Default constructor.
		/// </summary>
		public CVector4()
		{
		}

		/// <summary>
		/// Copy constructor.
		/// </summary>
		/// <param name="Vector">The vector to copy from</param>
		public CVector4(CVector4 Vector)
		{
			_X = Vector._X;
			_Y = Vector._Y;
			_Z = Vector._Z;
			_W = Vector._W;
		}

		/// <summary>
		/// Parameterized constructor.
		/// </summary>
		/// <param name="X">The X-coordinate to use</param>
		/// <param name="Y">The Y-coordinate to use</param>
		/// <param name="Z">The Z-coordinate to use</param>
		/// <param name="W">The W-coordinate to use</param>
		public CVector4(float X, float Y, float Z, float W)
		{
			_X = X;
			_Y = Y;
			_Z = Z;
			_W = W;
		}

		/// <summary>
		/// Clones the vector.
		/// </summary>
		/// <returns>The cloned vector</returns>
		public object Clone()
		{
			return new CVector4(this);
		}

		/// <summary>
		/// Generates a string version of the vector.
		/// </summary>
		/// <returns>The generated string</returns>
		public override string ToString()
		{
			return "{ " + _X + ", " + _Y + ", " + _Z + ", " + _W + " }";
		}
	}
}
