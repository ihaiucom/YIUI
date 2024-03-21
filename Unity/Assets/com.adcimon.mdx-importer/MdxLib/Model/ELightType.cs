namespace MdxLib.Model
{
	/// <summary>
	/// Enumerates the available light types.
	/// </summary>
	public enum ELightType
	{
		/// <summary>
		/// Represents omnidirectional lighting (equal distribution from source).
		/// </summary>
		Omnidirectional,
		/// <summary>
		/// Represents directional lighting (parallell waves).
		/// </summary>
		Directional,
		/// <summary>
		/// Represents ambient lighting (same lighting everywhere).
		/// </summary>
		Ambient
	}
}
