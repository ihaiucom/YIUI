namespace MdxLib.Animator.Animatable
{
	internal sealed class CFloat : CAnimatable<float>
	{
		public CFloat(float DefaultValue)
			: base(DefaultValue)
		{
		}

		public override float InterpolateNone(CTime Time, CAnimatorNode<float> Node1, CAnimatorNode<float> Node2)
		{
			return Node1.Value;
		}

		public override float InterpolateLinear(CTime Time, CAnimatorNode<float> Node1, CAnimatorNode<float> Node2)
		{
			float num = (float)(Time.Time - Node1.Time) / (float)(Node2.Time - Node1.Time);
			float num2 = 1f - num;
			return Node1.Value * num2 + Node2.Value * num;
		}

		public override float InterpolateBezier(CTime Time, CAnimatorNode<float> Node1, CAnimatorNode<float> Node2)
		{
			float num = (float)(Time.Time - Node1.Time) / (float)(Node2.Time - Node1.Time);
			float num2 = num * num;
			float num3 = 1f - num;
			float num4 = num3 * num3;
			float num5 = num4 * num3;
			float num6 = 3f * num * num4;
			float num7 = 3f * num2 * num3;
			float num8 = num2 * num;
			return Node1.Value * num5 + Node1.OutTangent * num6 + Node2.InTangent * num7 + Node2.Value * num8;
		}

		public override float InterpolateHermite(CTime Time, CAnimatorNode<float> Node1, CAnimatorNode<float> Node2)
		{
			float num = (float)(Time.Time - Node1.Time) / (float)(Node2.Time - Node1.Time);
			float num2 = num * num;
			float num3 = num2 * (2f * num - 3f) + 1f;
			float num4 = num2 * (num - 2f) + num;
			float num5 = num2 * (num - 1f);
			float num6 = num2 * (3f - 2f * num);
			return Node1.Value * num3 + Node1.OutTangent * num4 + Node2.InTangent * num5 + Node2.Value * num6;
		}
	}
}
