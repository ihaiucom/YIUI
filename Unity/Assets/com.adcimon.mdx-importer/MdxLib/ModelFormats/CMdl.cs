using System;
using System.IO;
using System.Text;
using MdxLib.Model;
using MdxLib.ModelFormats.Mdl;

namespace MdxLib.ModelFormats
{
	/// <summary>
	/// Handles the MDL model format. Can load and save MDL models.
	/// </summary>
	public sealed class CMdl : IModelFormat
	{
		/// <summary>
		/// Default constructor.
		/// </summary>
		public CMdl()
		{
		}

		/// <summary>
		/// Loads a model from a stream.
		/// </summary>
		/// <param name="Name">The name of the model (only used in some error messages)</param>
		/// <param name="Stream">The stream to load from (must support reading)</param>
		/// <param name="Model">The model to load to (must be an empty model)</param>
		public void Load(string Name, Stream Stream, MdxLib.Model.CModel Model)
		{
			if (!Stream.CanRead)
			{
				throw new NotSupportedException("Unable to load \"" + Name + "\", the stream does not support reading!");
			}
			CLoader cLoader = new CLoader(Name, Stream);
			MdxLib.ModelFormats.Mdl.CModel.Instance.Load(cLoader, Model);
			cLoader.Attacher.Attach();
		}

		/// <summary>
		/// Saves a model to a stream.
		/// </summary>
		/// <param name="Name">The name of the model (only used in some error messages)</param>
		/// <param name="Stream">The stream to save to (must support writing)</param>
		/// <param name="Model">The model to save from</param>
		public void Save(string Name, Stream Stream, MdxLib.Model.CModel Model)
		{
			if (!Stream.CanWrite)
			{
				throw new NotSupportedException("Unable to save \"" + Name + "\", the stream does not support writing!");
			}
			CSaver cSaver = new CSaver(Name, Stream);
			cSaver.WriteWord(BuildHeader(Name));
			MdxLib.ModelFormats.Mdl.CModel.Instance.Save(cSaver, Model);
			cSaver.WriteToStream();
		}

		private string BuildHeader(string Name)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("//+-----------------------------------------------------------------------------");
			stringBuilder.AppendLine("//|");
			stringBuilder.AppendLine("//| " + Name.Replace("\n", "").Replace("\r", ""));
			stringBuilder.AppendLine("//| Generated by MdxLib v1.04 (written by Magnus Ostberg, aka Magos)");
			stringBuilder.AppendLine("//| " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
			stringBuilder.AppendLine("//| http://www.magosx.com");
			stringBuilder.AppendLine("//|");
			stringBuilder.AppendLine("//+-----------------------------------------------------------------------------");
			return stringBuilder.ToString();
		}
	}
}
