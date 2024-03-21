using MdxLib.Primitives;

namespace MdxLib.Model
{
	/// <summary>
	/// A geoset vertex class. Defines a single vertex (point).
	/// </summary>
	public sealed class CGeosetVertex : CObject<CGeosetVertex>
	{
		private CVector3 _Position = CConstants.DefaultVector3;

		private CVector3 _Normal = CConstants.DefaultVector3;

		private CVector2 _TexturePosition = CConstants.DefaultVector2;

		private CObjectReference<CGeosetGroup> _Group;

		/// <summary>
		/// Gets or sets the position.
		/// </summary>
		public CVector3 Position
		{
			get
			{
				return _Position;
			}
			set
			{
				AddSetObjectFieldCommand("_Position", value);
				_Position = value;
			}
		}

		/// <summary>
		/// Gets or sets the normal.
		/// </summary>
		public CVector3 Normal
		{
			get
			{
				return _Normal;
			}
			set
			{
				AddSetObjectFieldCommand("_Normal", value);
				_Normal = value;
			}
		}

		/// <summary>
		/// Gets or sets the texture position.
		/// </summary>
		public CVector2 TexturePosition
		{
			get
			{
				return _TexturePosition;
			}
			set
			{
				AddSetObjectFieldCommand("_TexturePosition", value);
				_TexturePosition = value;
			}
		}

		/// <summary>
		/// Retrieves the geoset group reference.
		/// </summary>
		public CObjectReference<CGeosetGroup> Group => _Group ?? (_Group = new CObjectReference<CGeosetGroup>(base.Model));

		/// <summary>
		/// Parameterized constructor.
		/// </summary>
		/// <param name="Model">The model to be associated with this geoset vertex</param>
		public CGeosetVertex(CModel Model)
			: base(Model)
		{
		}

		/// <summary>
		/// Generates a string version of the geoset vertex.
		/// </summary>
		/// <returns>The generated string</returns>
		public override string ToString()
		{
			return "Geoset Vertex #" + base.ObjectId;
		}
	}
}
