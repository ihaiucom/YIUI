using System.IO;
using MdxLib.Model;

namespace MdxLib.ModelFormats
{
	/// <summary>
	/// The interface for all model formats. All model formats should
	/// implement this interface to ensure common functionality.
	/// </summary>
	public interface IModelFormat
	{
		/// <summary>
		/// Loads a model from a stream.
		/// </summary>
		/// <param name="Name">The name of the model (only used in some error messages)</param>
		/// <param name="Stream">The stream to load from</param>
		/// <param name="Model">The model to load to (must be an empty model)</param>
		void Load(string Name, Stream Stream, CModel Model);

		/// <summary>
		/// Saves a model to a stream.
		/// </summary>
		/// <param name="Name">The name of the model (only used in some error messages)</param>
		/// <param name="Stream">The stream to save to</param>
		/// <param name="Model">The model to save from</param>
		void Save(string Name, Stream Stream, CModel Model);
	}
}
