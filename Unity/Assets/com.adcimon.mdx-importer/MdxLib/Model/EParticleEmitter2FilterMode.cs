namespace MdxLib.Model
{
	/// <summary>
	/// Enumerates the available particle emitter 2 filter modes.
	/// </summary>
	public enum EParticleEmitter2FilterMode
	{
		/// <summary>
		/// Represents blending filtering, makes parts transparent allowing
		/// geometry behind the particle to shine through.
		/// </summary>
		Blend,
		/// <summary>
		/// Represents additive filtering (like addition), makes material brighter.
		/// </summary>
		Additive,
		/// <summary>
		/// Represents modulation (like multiplication), makes material darker.
		/// </summary>
		Modulate,
		/// <summary>
		/// Represents even more modulation (like multiplication), makes material darker.
		/// </summary>
		Modulate2x,
		/// <summary>
		/// Represents alpha-keyed filtering. Unknown effect.
		/// </summary>
		AlphaKey
	}
}
