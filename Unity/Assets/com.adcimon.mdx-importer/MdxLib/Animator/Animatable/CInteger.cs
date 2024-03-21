namespace MdxLib.Animator.Animatable
{
	internal sealed class CInteger : CAnimatable<int>
	{
		public CInteger(int DefaultValue)
			: base(DefaultValue)
		{
		}

		public override int InterpolateNone(CTime Time, CAnimatorNode<int> Node1, CAnimatorNode<int> Node2)
		{
			return Node1.Value;
		}

		public override int InterpolateLinear(CTime Time, CAnimatorNode<int> Node1, CAnimatorNode<int> Node2)
		{
			return Node1.Value;
		}

		public override int InterpolateBezier(CTime Time, CAnimatorNode<int> Node1, CAnimatorNode<int> Node2)
		{
			return Node1.Value;
		}

		public override int InterpolateHermite(CTime Time, CAnimatorNode<int> Node1, CAnimatorNode<int> Node2)
		{
			return Node1.Value;
		}
	}
}
