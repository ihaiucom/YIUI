using MdxLib.Primitives;

namespace MdxLib.Animator.Animatable
{
	internal sealed class CVector4 : CAnimatable<MdxLib.Primitives.CVector4>
	{
		public CVector4(MdxLib.Primitives.CVector4 DefaultValue)
			: base(DefaultValue)
		{
		}

		public override MdxLib.Primitives.CVector4 InterpolateNone(CTime Time, CAnimatorNode<MdxLib.Primitives.CVector4> Node1, CAnimatorNode<MdxLib.Primitives.CVector4> Node2)
		{
			return Node1.Value;
		}

		public override MdxLib.Primitives.CVector4 InterpolateLinear(CTime Time, CAnimatorNode<MdxLib.Primitives.CVector4> Node1, CAnimatorNode<MdxLib.Primitives.CVector4> Node2)
		{
			float num = (float)(Time.Time - Node1.Time) / (float)(Node2.Time - Node1.Time);
			float num2 = 1f - num;
			float x = Node1.Value.X * num2 + Node2.Value.X * num;
			float y = Node1.Value.Y * num2 + Node2.Value.Y * num;
			float z = Node1.Value.Z * num2 + Node2.Value.Z * num;
			float w = Node1.Value.W * num2 + Node2.Value.W * num;
			return new MdxLib.Primitives.CVector4(x, y, z, w);
		}

		public override MdxLib.Primitives.CVector4 InterpolateBezier(CTime Time, CAnimatorNode<MdxLib.Primitives.CVector4> Node1, CAnimatorNode<MdxLib.Primitives.CVector4> Node2)
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
			float z = Node1.Value.Z * num5 + Node1.OutTangent.Z * num6 + Node2.InTangent.Z * num7 + Node2.Value.Z * num8;
			float w = Node1.Value.W * num5 + Node1.OutTangent.W * num6 + Node2.InTangent.W * num7 + Node2.Value.W * num8;
			return new MdxLib.Primitives.CVector4(x, y, z, w);
		}

		public override MdxLib.Primitives.CVector4 InterpolateHermite(CTime Time, CAnimatorNode<MdxLib.Primitives.CVector4> Node1, CAnimatorNode<MdxLib.Primitives.CVector4> Node2)
		{
			float num = (float)(Time.Time - Node1.Time) / (float)(Node2.Time - Node1.Time);
			float num2 = num * num;
			float num3 = num2 * (2f * num - 3f) + 1f;
			float num4 = num2 * (num - 2f) + num;
			float num5 = num2 * (num - 1f);
			float num6 = num2 * (3f - 2f * num);
			float x = Node1.Value.X * num3 + Node1.OutTangent.X * num4 + Node2.InTangent.X * num5 + Node2.Value.X * num6;
			float y = Node1.Value.Y * num3 + Node1.OutTangent.Y * num4 + Node2.InTangent.Y * num5 + Node2.Value.Y * num6;
			float z = Node1.Value.Z * num3 + Node1.OutTangent.Z * num4 + Node2.InTangent.Z * num5 + Node2.Value.Z * num6;
			float w = Node1.Value.W * num3 + Node1.OutTangent.W * num4 + Node2.InTangent.W * num5 + Node2.Value.W * num6;
			return new MdxLib.Primitives.CVector4(x, y, z, w);
		}
	}
}
