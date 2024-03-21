namespace MdxLib.Model
{
	/// <summary>
	/// Enumerates the available material layer filter modes.
	/// </summary>
	public enum EMaterialLayerFilterMode
	{
		/// <summary>
		/// Represents no filtering.
		/// </summary>
		None,
		/// <summary>
		/// Represents transparent filtering, makes parts transparent allowing
		/// geometry behind the geoset to shine through.
		/// </summary>
		Transparent,
		/// <summary>
		/// Represents blending filtering, makes parts transparent allowing
		/// sublayers to shine through (usually used for teamcolors).
		/// </summary>
		Blend,
		/// <summary>
		/// Represents additive filtering (like addition), makes material brighter.
		/// Does not add to alpha channel.
		/// </summary>
		Additive,
		/// <summary>
		/// Represents additive filtering (like addition), makes material brighter.
		/// Also adds to alpha channel.
		/// </summary>
		AdditiveAlpha,
		/// <summary>
		/// Represents modulation (like multiplication), makes material darker.
		/// </summary>
		Modulate,
		/// <summary>
		/// Represents even more modulation (like multiplication), makes material darker.
		/// </summary>
		Modulate2x
	}
}
