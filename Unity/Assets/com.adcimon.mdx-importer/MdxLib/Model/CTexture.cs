namespace MdxLib.Model
{
	/// <summary>
	/// A texture class. Represents a texture which can be a real image
	/// or a replaceable texture (like teamcolor).
	/// </summary>
	public sealed class CTexture : CObject<CTexture>
	{
		private string _FileName = "";

		private int _ReplaceableId;

		private bool _WrapWidth;

		private bool _WrapHeight;

		/// <summary>
		/// Gets or sets the filename.
		/// </summary>
		public string FileName
		{
			get
			{
				return _FileName;
			}
			set
			{
				AddSetObjectFieldCommand("_FileName", value);
				_FileName = value;
			}
		}

		/// <summary>
		/// Gets or sets the replaceable ID.
		/// </summary>
		public int ReplaceableId
		{
			get
			{
				return _ReplaceableId;
			}
			set
			{
				AddSetObjectFieldCommand("_ReplaceableId", value);
				_ReplaceableId = value;
			}
		}

		/// <summary>
		/// Gets or sets the wrap width flag.
		/// </summary>
		public bool WrapWidth
		{
			get
			{
				return _WrapWidth;
			}
			set
			{
				AddSetObjectFieldCommand("_WrapWidth", value);
				_WrapWidth = value;
			}
		}

		/// <summary>
		/// Gets or sets the wrap height flag.
		/// </summary>
		public bool WrapHeight
		{
			get
			{
				return _WrapHeight;
			}
			set
			{
				AddSetObjectFieldCommand("_WrapHeight", value);
				_WrapHeight = value;
			}
		}

		/// <summary>
		/// Parameterized constructor.
		/// </summary>
		/// <param name="Model">The model to be associated with this texture</param>
		public CTexture(CModel Model)
			: base(Model)
		{
		}

		/// <summary>
		/// Generates a string version of the texture.
		/// </summary>
		/// <returns>The generated string</returns>
		public override string ToString()
		{
			return "Texture #" + base.ObjectId;
		}
	}
}
