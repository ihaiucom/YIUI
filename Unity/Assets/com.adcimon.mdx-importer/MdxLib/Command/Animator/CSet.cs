using MdxLib.Animator;

namespace MdxLib.Command.Animator
{
	internal sealed class CSet<T> : ICommand where T : new()
	{
		private CAnimator<T> CurrentAnimator;

		private int CurrentIndex = -1;

		private CAnimatorNode<T> OldNode;

		private CAnimatorNode<T> NewNode;

		public CSet(CAnimator<T> Animator, int Index, CAnimatorNode<T> Node)
		{
			CurrentAnimator = Animator;
			CurrentIndex = Index;
			OldNode = CurrentAnimator.InternalNodeList[CurrentIndex];
			NewNode = Node;
		}

		public void Do()
		{
			CurrentAnimator.InternalNodeList[CurrentIndex] = NewNode;
		}

		public void Undo()
		{
			CurrentAnimator.InternalNodeList[CurrentIndex] = OldNode;
		}
	}
}
