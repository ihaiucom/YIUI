using System.Collections.Generic;
using MdxLib.Animator;

namespace MdxLib.Command.Animator
{
	internal sealed class CClear<T> : ICommand where T : new()
	{
		private CAnimator<T> CurrentAnimator;

		private List<CAnimatorNode<T>> OldNodeList;

		private List<CAnimatorNode<T>> NewNodeList;

		public CClear(CAnimator<T> Animator)
		{
			CurrentAnimator = Animator;
			OldNodeList = CurrentAnimator.InternalNodeList;
			NewNodeList = new List<CAnimatorNode<T>>();
		}

		public void Do()
		{
			CurrentAnimator.InternalNodeList = NewNodeList;
		}

		public void Undo()
		{
			CurrentAnimator.InternalNodeList = OldNodeList;
		}
	}
}
