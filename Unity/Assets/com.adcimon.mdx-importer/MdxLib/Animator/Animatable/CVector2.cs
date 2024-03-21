using MdxLib.Primitives;

namespace MdxLib.Animator.Animatable
{
	internal sealed class CVector2 : CAnimatable<MdxLib.Primitives.CVector2>
	{
		public CVector2(MdxLib.Primitives.CVector2 DefaultValue)
			: base(DefaultValue)
		{
		}

		public override MdxLib.Primitives.CVector2 InterpolateNone(CTime Time, CAnimatorNode<MdxLib.Primitives.CVector2> Node1, CAnimatorNode<MdxLib.Primitives.CVector2> Node2)
		{
			return Node1.Value;
		}

		public override MdxLib.Primitives.CVector2 InterpolateLinear(CTime Time, CAnimatorNode<MdxLib.Primitives.CVector2> Node1, CAnimatorNode<MdxLib.Primitives.CVector2> Node2)
		{
			float num = (float)(Time.Time - Node1.Time) / (float)(Node2.Time - Node1.Time);
			float num2 = 1f - num;
			float x = Node1.Value.X * num2 + Node2.Value.X * num;
			float y = Node1.Value.Y * num2 + Node2.Value.Y * num;
			return new MdxLib.Primitives.CVector2(x, y);
		}

		public override MdxLib.Primitives.CVector2 InterpolateBezier(CTime Time, CAnimatorNode<MdxLib.Primitives.CVector2> Node1, CAnimatorNode<MdxLib.Primitives.CVector2> Node2)
		{
			float num = (float)(Time.Time - Node1.Time) / (float)(Node2.Time - Node1.Time);
			float num2 = num * num;
			float num3 = 1f - num;
			float num4 = num3 * num3;
			float num5 = num4 * num3;
			float num6 = 3f * num * num4;
			float num7 = 3f * num2 * num3;
			float num8 = num2 * num;
			float x = Node1.Value.X * num5 + Node1.OutTangent.X * num6 + Node2.InTangent.X * num7 + Node2.Value.X * num8;
			float y = Node1.Value.Y * num5 + Node1.OutTangent.Y * num6 + Node2.InTangent.Y * num7 + Node2.Value.Y * num8;
			return new MdxLib.Primitives.CVector2(x, y);
		}

		public override MdxLib.Primitives.CVector2 InterpolateHermite(CTime Time, CAnimatorNode<MdxLib.Primitives.CVector2> Node1, CAnimatorNode<MdxLib.Primitives.CVector2> Node2)
		{
			float num = (float)(Time.Time - Node1.Time) / (float)(Node2.Time - Node1.Time);
			float num2 = num * num;
			float num3 = num2 * (2f * num - 3f) + 1f;
			float num4 = num2 * (num - 2f) + num;
			float num5 = num2 * (num - 1f);
			float num6 = num2 * (3f - 2f * num);
			float x = Node1.Value.X * num3 + Node1.OutTangent.X * num4 + Node2.InTangent.X * num5 + Node2.Value.X * num6;
			float y = Node1.Value.Y * num3 + Node1.OutTangent.Y * num4 + Node2.InTangent.Y * num5 + Node2.Value.Y * num6;
			return new MdxLib.Primitives.CVector2(x, y);
		}
	}
}
