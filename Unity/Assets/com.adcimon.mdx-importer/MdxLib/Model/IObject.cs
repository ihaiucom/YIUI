namespace MdxLib.Model
{
	/// <summary>
	/// The base interface for all object components.
	/// </summary>
	public interface IObject : IUnknown
	{
		/// <summary>
		/// Retrieves the object ID (if added to a container).
		/// </summary>
		int ObjectId { get; }

		/// <summary>
		/// Retrieves the associated model.
		/// </summary>
		CModel Model { get; }

		/// <summary>
		/// Checks if the object has references pointing to it.
		/// </summary>
		bool HasReferences { get; }
	}
}
