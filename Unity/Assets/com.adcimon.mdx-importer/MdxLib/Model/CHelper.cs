namespace MdxLib.Model
{
	/// <summary>
	/// A helper class. The node object in the node skeleton hiearchy.
	/// </summary>
	public sealed class CHelper : CNode<CHelper>
	{
		/// <summary>
		/// Retrieves the node ID (if added to a container).
		/// </summary>
		public override int NodeId => base.Model.GetHelperNodeId(this);

		/// <summary>
		/// Parameterized constructor.
		/// </summary>
		/// <param name="Model">The model to be associated with this helper</param>
		public CHelper(CModel Model)
			: base(Model)
		{
		}

		/// <summary>
		/// Generates a string version of the helper.
		/// </summary>
		/// <returns>The generated string</returns>
		public override string ToString()
		{
			return "Helper #" + base.ObjectId;
		}
	}
}
