using System;
using MdxLib.Primitives;

namespace MdxLib.Animator.Animatable
{
	internal sealed class CQuaternion : CAnimatable<MdxLib.Primitives.CVector4>
	{
		private const float Threshold = 0.95f;

		public CQuaternion(MdxLib.Primitives.CVector4 DefaultValue)
			: base(DefaultValue)
		{
		}

		public override MdxLib.Primitives.CVector4 InterpolateNone(CTime Time, CAnimatorNode<MdxLib.Primitives.CVector4> Node1, CAnimatorNode<MdxLib.Primitives.CVector4> Node2)
		{
			return Node1.Value;
		}

		public override MdxLib.Primitives.CVector4 InterpolateLinear(CTime Time, CAnimatorNode<MdxLib.Primitives.CVector4> Node1, CAnimatorNode<MdxLib.Primitives.CVector4> Node2)
		{
			float factor = (float)(Time.Time - Node1.Time) / (float)(Node2.Time - Node1.Time);
			return GetSlerp(Node1.Value, Node2.Value, factor, InvertIfNeccessary: true);
		}

		public override MdxLib.Primitives.CVector4 InterpolateBezier(CTime Time, CAnimatorNode<MdxLib.Primitives.CVector4> Node1, CAnimatorNode<MdxLib.Primitives.CVector4> Node2)
		{
			float factor = (float)(Time.Time - Node1.Time) / (float)(Node2.Time - Node1.Time);
			MdxLib.Primitives.CVector4 slerp = GetSlerp(Node1.Value, Node1.OutTangent, factor, InvertIfNeccessary: false);
			MdxLib.Primitives.CVector4 slerp2 = GetSlerp(Node1.OutTangent, Node2.InTangent, factor, InvertIfNeccessary: false);
			MdxLib.Primitives.CVector4 slerp3 = GetSlerp(Node2.InTangent, Node2.Value, factor, InvertIfNeccessary: false);
			MdxLib.Primitives.CVector4 slerp4 = GetSlerp(slerp, slerp2, factor, InvertIfNeccessary: false);
			MdxLib.Primitives.CVector4 slerp5 = GetSlerp(slerp2, slerp3, factor, InvertIfNeccessary: false);
			return GetSlerp(slerp4, slerp5, factor, InvertIfNeccessary: false);
		}

		public override MdxLib.Primitives.CVector4 InterpolateHermite(CTime Time, CAnimatorNode<MdxLib.Primitives.CVector4> Node1, CAnimatorNode<MdxLib.Primitives.CVector4> Node2)
		{
			float num = (float)(Time.Time - Node1.Time) / (float)(Node2.Time - Node1.Time);
			MdxLib.Primitives.CVector4 slerp = GetSlerp(Node1.Value, Node2.Value, num, InvertIfNeccessary: false);
			MdxLib.Primitives.CVector4 slerp2 = GetSlerp(Node1.OutTangent, Node2.InTangent, num, InvertIfNeccessary: false);
			return GetSlerp(slerp, slerp2, 2f * num * (1f - num), InvertIfNeccessary: false);
		}

		private float GetDotProduct(MdxLib.Primitives.CVector4 Quaternion1, MdxLib.Primitives.CVector4 Quaternion2)
		{
			return Quaternion1.X * Quaternion2.X + Quaternion1.Y * Quaternion2.Y + Quaternion1.Z * Quaternion2.Z + Quaternion1.W * Quaternion2.W;
		}

		private MdxLib.Primitives.CVector4 GetSlerp(MdxLib.Primitives.CVector4 Quaternion1, MdxLib.Primitives.CVector4 Quaternion2, float Factor, bool InvertIfNeccessary)
		{
			float num = 1f - Factor;
			float num2 = GetDotProduct(Quaternion1, Quaternion2);
			if (InvertIfNeccessary && num2 < 0f)
			{
				num2 = 0f - num2;
				Quaternion2 = new MdxLib.Primitives.CVector4(0f - Quaternion2.X, 0f - Quaternion2.Y, 0f - Quaternion2.Z, 0f - Quaternion2.W);
			}
			if (num2 > -0.95f && num2 < 0.95f)
			{
				float num3 = (float)Math.Acos(num2);
				float num4 = 1f / (float)Math.Sin(num3);
				float num5 = num4 * (float)Math.Sin(num3 * num);
				float num6 = num4 * (float)Math.Sin(num3 * Factor);
				return new MdxLib.Primitives.CVector4(Quaternion1.X * num5 + Quaternion2.X * num6, Quaternion1.Y * num5 + Quaternion2.Y * num6, Quaternion1.Z * num5 + Quaternion2.Z * num6, Quaternion1.W * num5 + Quaternion2.W * num6);
			}
			float num7 = Quaternion1.X * num + Quaternion2.X * Factor;
			float num8 = Quaternion1.Y * num + Quaternion2.Y * Factor;
			float num9 = Quaternion1.Z * num + Quaternion2.Z * Factor;
			float num10 = Quaternion1.W * num + Quaternion2.W * Factor;
			float num11 = (float)Math.Sqrt(num7 * num7 + num8 * num8 + num9 * num9 + num10 * num10);
			float num12 = ((num11 != 0f) ? (1f / num11) : 0f);
			return new MdxLib.Primitives.CVector4(num7 * num12, num8 * num11, num9 * num11, num10 * num11);
		}
	}
}
