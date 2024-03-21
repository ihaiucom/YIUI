using MdxLib.Animator;
using MdxLib.Animator.Animatable;
using MdxLib.Primitives;

namespace MdxLib.Model
{
	/// <summary>
	/// A ribbon emitter class. Is used to create trailing ribbon effects,
	/// like a floating band that slowly dissipates.
	/// </summary>
	public sealed class CRibbonEmitter : CNode<CRibbonEmitter>
	{
		private int _Rows = 1;

		private int _Columns = 1;

		private int _EmissionRate;

		private float _LifeSpan;

		private float _Gravity = 1f;

		private CAnimator<float> _HeightAbove;

		private CAnimator<float> _HeightBelow;

		private CAnimator<float> _Alpha;

		private CAnimator<MdxLib.Primitives.CVector3> _Color;

		private CAnimator<int> _TextureSlot;

		private CAnimator<float> _Visibility;

		private CObjectReference<CMaterial> _Material;

		/// <summary>
		/// Retrieves the node ID (if added to a container).
		/// </summary>
		public override int NodeId => base.Model.GetRibbonEmitterNodeId(this);

		/// <summary>
		/// Gets or sets the rows.
		/// </summary>
		public int Rows
		{
			get
			{
				return _Rows;
			}
			set
			{
				AddSetObjectFieldCommand("_Rows", value);
				_Rows = value;
			}
		}

		/// <summary>
		/// Gets or sets the columns.
		/// </summary>
		public int Columns
		{
			get
			{
				return _Columns;
			}
			set
			{
				AddSetObjectFieldCommand("_Columns", value);
				_Columns = value;
			}
		}

		/// <summary>
		/// Gets or sets the emission rate.
		/// </summary>
		public int EmissionRate
		{
			get
			{
				return _EmissionRate;
			}
			set
			{
				AddSetObjectFieldCommand("_EmissionRate", value);
				_EmissionRate = value;
			}
		}

		/// <summary>
		/// Gets or sets the life span.
		/// </summary>
		public float LifeSpan
		{
			get
			{
				return _LifeSpan;
			}
			set
			{
				AddSetObjectFieldCommand("_LifeSpan", value);
				_LifeSpan = value;
			}
		}

		/// <summary>
		/// Gets or sets the gravity.
		/// </summary>
		public float Gravity
		{
			get
			{
				return _Gravity;
			}
			set
			{
				AddSetObjectFieldCommand("_Gravity", value);
				_Gravity = value;
			}
		}

		/// <summary>
		/// Retrieves the height above animator.
		/// </summary>
		public CAnimator<float> HeightAbove => _HeightAbove ?? (_HeightAbove = new CAnimator<float>(base.Model, new CFloat(0f)));

		/// <summary>
		/// Retrieves the height below animator.
		/// </summary>
		public CAnimator<float> HeightBelow => _HeightBelow ?? (_HeightBelow = new CAnimator<float>(base.Model, new CFloat(0f)));

		/// <summary>
		/// Retrieves the alpha animator.
		/// </summary>
		public CAnimator<float> Alpha => _Alpha ?? (_Alpha = new CAnimator<float>(base.Model, new CFloat(1f)));

		/// <summary>
		/// Retrieves the color animator.
		/// </summary>
		public CAnimator<MdxLib.Primitives.CVector3> Color => _Color ?? (_Color = new CAnimator<MdxLib.Primitives.CVector3>(base.Model, new MdxLib.Animator.Animatable.CVector3(CConstants.DefaultColor)));

		/// <summary>
		/// Retrieves the texture slot animator.
		/// </summary>
		public CAnimator<int> TextureSlot => _TextureSlot ?? (_TextureSlot = new CAnimator<int>(base.Model, new CInteger(0)));

		/// <summary>
		/// Retrieves the visibility animator.
		/// </summary>
		public CAnimator<float> Visibility => _Visibility ?? (_Visibility = new CAnimator<float>(base.Model, new CFloat(1f)));

		/// <summary>
		/// Retrieves the material reference.
		/// </summary>
		public CObjectReference<CMaterial> Material => _Material ?? (_Material = new CObjectReference<CMaterial>(base.Model));

		/// <summary>
		/// Parameterized constructor.
		/// </summary>
		/// <param name="Model">The model to be associated with this ribbon emitter</param>
		public CRibbonEmitter(CModel Model)
			: base(Model)
		{
		}

		/// <summary>
		/// Generates a string version of the ribbon emitter.
		/// </summary>
		/// <returns>The generated string</returns>
		public override string ToString()
		{
			return "Ribbon Emitter #" + base.ObjectId;
		}
	}
}
