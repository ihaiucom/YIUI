using MdxLib.Animator;

namespace MdxLib.Command.Animator
{
	internal sealed class CRemoveAt<T> : ICommand where T : new()
	{
		private CAnimator<T> CurrentAnimator;

		private int CurrentIndex = -1;

		private CAnimatorNode<T> OldNode;

		public CRemoveAt(CAnimator<T> Animator, int Index)
		{
			CurrentAnimator = Animator;
			CurrentIndex = Index;
			OldNode = CurrentAnimator.InternalNodeList[CurrentIndex];
		}

		public void Do()
		{
			CurrentAnimator.InternalNodeList.RemoveAt(CurrentIndex);
		}

		public void Undo()
		{
			CurrentAnimator.InternalNodeList.Insert(CurrentIndex, OldNode);
		}
	}
}
