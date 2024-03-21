using MdxLib.Primitives;

namespace MdxLib.Model
{
	/// <summary>
	/// A geoset extent class. Defines an extent for a geoset for each
	/// sequence that exists (an animated geoset might occupy more space
	/// than a static one).
	/// </summary>
	public sealed class CGeosetExtent : CObject<CGeosetExtent>
	{
		private CExtent _Extent = CConstants.DefaultExtent;

		/// <summary>
		/// Gets or sets the extent.
		/// </summary>
		public CExtent Extent
		{
			get
			{
				return _Extent;
			}
			set
			{
				AddSetObjectFieldCommand("_Extent", value);
				_Extent = value;
			}
		}

		/// <summary>
		/// Parameterized constructor.
		/// </summary>
		/// <param name="Model">The model to be associated with this geoset extent</param>
		public CGeosetExtent(CModel Model)
			: base(Model)
		{
		}

		/// <summary>
		/// Generates a string version of the geoset extent.
		/// </summary>
		/// <returns>The generated string</returns>
		public override string ToString()
		{
			return "Geoset Extent #" + base.ObjectId;
		}
	}
}
