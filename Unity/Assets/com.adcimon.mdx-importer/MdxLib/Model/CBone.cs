namespace MdxLib.Model
{
	/// <summary>
	/// A bone class. The leaf object in the node skeleton hiearchy
	/// and the object which geosets can attach themselves to.
	/// </summary>
	public sealed class CBone : CNode<CBone>
	{
		private CObjectReference<CGeoset> _Geoset;

		private CObjectReference<CGeosetAnimation> _GeosetAnimation;

		/// <summary>
		/// Retrieves the node ID (if added to a container).
		/// </summary>
		public override int NodeId => base.Model.GetBoneNodeId(this);

		/// <summary>
		/// Retrieves the geoset reference.
		/// </summary>
		public CObjectReference<CGeoset> Geoset => _Geoset ?? (_Geoset = new CObjectReference<CGeoset>(base.Model));

		/// <summary>
		/// Retrieves the geoset animation reference.
		/// </summary>
		public CObjectReference<CGeosetAnimation> GeosetAnimation => _GeosetAnimation ?? (_GeosetAnimation = new CObjectReference<CGeosetAnimation>(base.Model));

		/// <summary>
		/// Parameterized constructor.
		/// </summary>
		/// <param name="Model">The model to be associated with this bone</param>
		public CBone(CModel Model)
			: base(Model)
		{
		}

		/// <summary>
		/// Generates a string version of the bone.
		/// </summary>
		/// <returns>The generated string</returns>
		public override string ToString()
		{
			return "Bone #" + base.ObjectId;
		}
	}
}
