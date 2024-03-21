namespace MdxLib.Model
{
	/// <summary>
	/// Enumerates the available collision shape types.
	/// </summary>
	public enum ECollisionShapeType
	{
		/// <summary>
		/// Represents a box (both vertices used, min and max corner).
		/// </summary>
		Box,
		/// <summary>
		/// Represents a sphere (vertex 1 and radius used, center and radius).
		/// </summary>
		Sphere
	}
}
