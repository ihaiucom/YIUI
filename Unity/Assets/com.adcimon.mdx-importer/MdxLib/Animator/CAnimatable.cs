namespace MdxLib.Animator
{
	internal abstract class CAnimatable<T> where T : new()
	{
		private T _DefaultValue = default(T);

		public T DefaultValue => _DefaultValue;

		public CAnimatable(T DefaultValue)
		{
			_DefaultValue = DefaultValue;
		}

		public T Interpolate(EInterpolationType Type, CTime Time, CAnimatorNode<T> Node1, CAnimatorNode<T> Node2)
		{
			if (Node1 == null)
			{
				return _DefaultValue;
			}
			if (Node2 == null)
			{
				return Node1.Value;
			}
			if (Node1.Time >= Node2.Time)
			{
				return Node1.Value;
			}
			return Type switch
			{
				EInterpolationType.None => InterpolateNone(Time, Node1, Node2), 
				EInterpolationType.Linear => InterpolateLinear(Time, Node1, Node2), 
				EInterpolationType.Bezier => InterpolateBezier(Time, Node1, Node2), 
				EInterpolationType.Hermite => InterpolateHermite(Time, Node1, Node2), 
				_ => _DefaultValue, 
			};
		}

		public abstract T InterpolateNone(CTime Time, CAnimatorNode<T> Node1, CAnimatorNode<T> Node2);

		public abstract T InterpolateLinear(CTime Time, CAnimatorNode<T> Node1, CAnimatorNode<T> Node2);

		public abstract T InterpolateBezier(CTime Time, CAnimatorNode<T> Node1, CAnimatorNode<T> Node2);

		public abstract T InterpolateHermite(CTime Time, CAnimatorNode<T> Node1, CAnimatorNode<T> Node2);
	}
}
