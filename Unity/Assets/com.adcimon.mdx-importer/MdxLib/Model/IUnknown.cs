namespace MdxLib.Model
{
	/// <summary>
	/// The base interface for all objects.
	/// </summary>
	public interface IUnknown
	{
		/// <summary>
		/// Gets or sets the tag data of the object. Tag data is not saved when the model is.
		/// </summary>
		object Tag { get; set; }
	}
}
