namespace MdxLib.Model
{
	/// <summary>
	/// A geoset group node class. Specifies a single matrix (node) for
	/// its geoset group.
	/// </summary>
	public sealed class CGeosetGroupNode : CObject<CGeosetGroupNode>
	{
		private CNodeReference _Node;

		/// <summary>
		/// Retrieves the node reference.
		/// </summary>
		public CNodeReference Node => _Node ?? (_Node = new CNodeReference(base.Model));

		/// <summary>
		/// Parameterized constructor.
		/// </summary>
		/// <param name="Model">The model to be associated with this geoset group node</param>
		public CGeosetGroupNode(CModel Model)
			: base(Model)
		{
		}

		/// <summary>
		/// Generates a string version of the geoset group node.
		/// </summary>
		/// <returns>The generated string</returns>
		public override string ToString()
		{
			return "Geoset Group Node #" + base.ObjectId;
		}
	}
}
