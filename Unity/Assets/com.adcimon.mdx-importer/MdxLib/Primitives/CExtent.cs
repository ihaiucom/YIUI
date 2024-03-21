using System;

namespace MdxLib.Primitives
{
	/// <summary>
	/// An immutable extent. Defines a shell in which no geoset
	/// (static or animated) should exceed.
	/// </summary>
	public sealed class CExtent : ICloneable
	{
		private CVector3 _Min;

		private CVector3 _Max;

		private float _Radius;

		/// <summary>
		/// Retrieves the minimum point.
		/// </summary>
		public CVector3 Min => _Min;

		/// <summary>
		/// Retrieves the maximum point.
		/// </summary>
		public CVector3 Max => _Max;

		/// <summary>
		/// Retrieves the radius.
		/// </summary>
		public float Radius => _Radius;

		/// <summary>
		/// Default constructor.
		/// </summary>
		public CExtent()
		{
			_Min = new CVector3();
			_Max = new CVector3();
		}

		/// <summary>
		/// Copy constructor.
		/// </summary>
		/// <param name="Extent">The extent to copy from</param>
		public CExtent(CExtent Extent)
		{
			_Min = Extent._Min;
			_Max = Extent._Max;
			_Radius = Extent._Radius;
		}

		/// <summary>
		/// Parameterized constructor.
		/// </summary>
		/// <param name="Min">The minimum point to use</param>
		/// <param name="Max">The maximum point to use</param>
		/// <param name="Radius">The radius to use</param>
		public CExtent(CVector3 Min, CVector3 Max, float Radius)
		{
			_Min = Min;
			_Max = Max;
			_Radius = Radius;
		}

		/// <summary>
		/// Clones the extent.
		/// </summary>
		/// <returns>The cloned extent</returns>
		public object Clone()
		{
			return new CExtent(this);
		}

		/// <summary>
		/// Generates a string version of the extent.
		/// </summary>
		/// <returns>The generated string</returns>
		public override string ToString()
		{
			return string.Concat("{ ", _Min, ", ", _Max, ", ", _Radius, " }");
		}
	}
}
