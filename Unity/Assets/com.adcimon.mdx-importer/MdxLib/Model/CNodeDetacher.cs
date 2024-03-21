namespace MdxLib.Model
{
	internal sealed class CNodeDetacher : CDetacher
	{
		private INode Node;

		private CNodeReference Reference;

		public CNodeDetacher(CNodeReference NodeReference)
		{
			Reference = NodeReference;
			Node = Reference.Node;
		}

		public override void Detach()
		{
			if (Node != null)
			{
				Reference.ForceDetach();
			}
		}

		public override void Attach()
		{
			if (Node != null)
			{
				Reference.ForceAttach(Node);
			}
		}
	}
}
