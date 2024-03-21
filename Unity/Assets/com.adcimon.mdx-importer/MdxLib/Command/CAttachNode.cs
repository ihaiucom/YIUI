using MdxLib.Model;

namespace MdxLib.Command
{
	internal sealed class CAttachNode : ICommand
	{
		private CNodeReference CurrentNodeReference;

		private CUnknown Unknown;

		private INode NewNode;

		public CAttachNode(CNodeReference NodeReference, INode Node)
		{
			CurrentNodeReference = NodeReference;
			Unknown = Node as CUnknown;
			NewNode = Node;
		}

		public void Do()
		{
			Unknown.NodeReferenceSet.Add(CurrentNodeReference);
			CurrentNodeReference.InternalNode = NewNode;
		}

		public void Undo()
		{
			Unknown.NodeReferenceSet.Remove(CurrentNodeReference);
			CurrentNodeReference.InternalNode = null;
		}
	}
}
