namespace MdxLib.Model
{
	/// <summary>
	/// A geoset face class. Defines a single face (trinagle).
	/// </summary>
	public sealed class CGeosetFace : CObject<CGeosetFace>
	{
		private CObjectReference<CGeosetVertex> _Vertex1;

		private CObjectReference<CGeosetVertex> _Vertex2;

		private CObjectReference<CGeosetVertex> _Vertex3;

		/// <summary>
		/// Retrieves the first vertex reference.
		/// </summary>
		public CObjectReference<CGeosetVertex> Vertex1 => _Vertex1 ?? (_Vertex1 = new CObjectReference<CGeosetVertex>(base.Model));

		/// <summary>
		/// Retrieves the second vertex reference.
		/// </summary>
		public CObjectReference<CGeosetVertex> Vertex2 => _Vertex2 ?? (_Vertex2 = new CObjectReference<CGeosetVertex>(base.Model));

		/// <summary>
		/// Retrieves the third vertex reference.
		/// </summary>
		public CObjectReference<CGeosetVertex> Vertex3 => _Vertex3 ?? (_Vertex3 = new CObjectReference<CGeosetVertex>(base.Model));

		/// <summary>
		/// Parameterized constructor.
		/// </summary>
		/// <param name="Model">The model to be associated with this geoset face</param>
		public CGeosetFace(CModel Model)
			: base(Model)
		{
		}

		/// <summary>
		/// Generates a string version of the geoset face.
		/// </summary>
		/// <returns>The generated string</returns>
		public override string ToString()
		{
			return "Geoset Face #" + base.ObjectId;
		}
	}
}
