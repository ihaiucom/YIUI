using MdxLib.Model;

namespace MdxLib.Command
{
	internal sealed class CDetachNode : ICommand
	{
		private CNodeReference CurrentNodeReference;

		private CUnknown Unknown;

		private INode OldNode;

		public CDetachNode(CNodeReference NodeReference, INode Node)
		{
			CurrentNodeReference = NodeReference;
			Unknown = Node as CUnknown;
			OldNode = Node;
		}

		public void Do()
		{
			Unknown.NodeReferenceSet.Remove(CurrentNodeReference);
			CurrentNodeReference.InternalNode = null;
		}

		public void Undo()
		{
			Unknown.NodeReferenceSet.Add(CurrentNodeReference);
			CurrentNodeReference.InternalNode = OldNode;
		}
	}
}
