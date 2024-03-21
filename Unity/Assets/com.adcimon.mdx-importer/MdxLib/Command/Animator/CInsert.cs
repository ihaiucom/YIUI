using MdxLib.Animator;

namespace MdxLib.Command.Animator
{
	internal sealed class CInsert<T> : ICommand where T : new()
	{
		private CAnimator<T> CurrentAnimator;

		private int CurrentIndex = -1;

		private CAnimatorNode<T> NewNode;

		public CInsert(CAnimator<T> Animator, int Index, CAnimatorNode<T> Node)
		{
			CurrentAnimator = Animator;
			CurrentIndex = Index;
			NewNode = Node;
		}

		public void Do()
		{
			CurrentAnimator.InternalNodeList.Insert(CurrentIndex, NewNode);
		}

		public void Undo()
		{
			CurrentAnimator.InternalNodeList.RemoveAt(CurrentIndex);
		}
	}
}
