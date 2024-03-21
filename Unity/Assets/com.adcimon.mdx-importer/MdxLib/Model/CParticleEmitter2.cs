using MdxLib.Animator;
using MdxLib.Animator.Animatable;
using MdxLib.Primitives;

namespace MdxLib.Model
{
	/// <summary>
	/// A particle emitter 2 class. Emits particles that can be animated in
	/// many ways. Is used to create effects such as fire, explosions and blood.
	/// </summary>
	public sealed class CParticleEmitter2 : CNode<CParticleEmitter2>
	{
		private EParticleEmitter2FilterMode _FilterMode;

		private int _Rows = 1;

		private int _Columns = 1;

		private int _PriorityPlane;

		private int _ReplaceableId = -1;

		private float _Time;

		private float _LifeSpan;

		private float _TailLength;

		private bool _SortPrimitivesFarZ;

		private bool _LineEmitter;

		private bool _ModelSpace;

		private bool _Unshaded;

		private bool _Unfogged;

		private bool _XYQuad;

		private bool _Squirt;

		private bool _Head;

		private bool _Tail;

		private CSegment _Segment1 = CConstants.DefaultSegment;

		private CSegment _Segment2 = CConstants.DefaultSegment;

		private CSegment _Segment3 = CConstants.DefaultSegment;

		private CInterval _HeadLife = CConstants.DefaultInterval;

		private CInterval _HeadDecay = CConstants.DefaultInterval;

		private CInterval _TailLife = CConstants.DefaultInterval;

		private CInterval _TailDecay = CConstants.DefaultInterval;

		private CAnimator<float> _Speed;

		private CAnimator<float> _Variation;

		private CAnimator<float> _Latitude;

		private CAnimator<float> _Gravity;

		private CAnimator<float> _EmissionRate;

		private CAnimator<float> _Width;

		private CAnimator<float> _Length;

		private CAnimator<float> _Visibility;

		private CObjectReference<CTexture> _Texture;

		/// <summary>
		/// Retrieves the node ID (if added to a container).
		/// </summary>
		public override int NodeId => base.Model.GetParticleEmitter2NodeId(this);

		/// <summary>
		/// Gets or sets the filter mode.
		/// </summary>
		public EParticleEmitter2FilterMode FilterMode
		{
			get
			{
				return _FilterMode;
			}
			set
			{
				AddSetObjectFieldCommand("_FilterMode", value);
				_FilterMode = value;
			}
		}

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
		/// Gets or sets the priority plane.
		/// </summary>
		public int PriorityPlane
		{
			get
			{
				return _PriorityPlane;
			}
			set
			{
				AddSetObjectFieldCommand("_PriorityPlane", value);
				_PriorityPlane = value;
			}
		}

		/// <summary>
		/// Gets or sets the replaceable ID.
		/// </summary>
		public int ReplaceableId
		{
			get
			{
				return _ReplaceableId;
			}
			set
			{
				AddSetObjectFieldCommand("_ReplaceableId", value);
				_ReplaceableId = value;
			}
		}

		/// <summary>
		/// Gets or sets the time.
		/// </summary>
		public float Time
		{
			get
			{
				return _Time;
			}
			set
			{
				AddSetObjectFieldCommand("_Time", value);
				_Time = value;
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
		/// Gets or sets the tail length.
		/// </summary>
		public float TailLength
		{
			get
			{
				return _TailLength;
			}
			set
			{
				AddSetObjectFieldCommand("_TailLength", value);
				_TailLength = value;
			}
		}

		/// <summary>
		/// Gets or sets the sort primitives far Z flag.
		/// </summary>
		public bool SortPrimitivesFarZ
		{
			get
			{
				return _SortPrimitivesFarZ;
			}
			set
			{
				AddSetObjectFieldCommand("_SortPrimitivesFarZ", value);
				_SortPrimitivesFarZ = value;
			}
		}

		/// <summary>
		/// Gets or sets the line emitter flag.
		/// </summary>
		public bool LineEmitter
		{
			get
			{
				return _LineEmitter;
			}
			set
			{
				AddSetObjectFieldCommand("_LineEmitter", value);
				_LineEmitter = value;
			}
		}

		/// <summary>
		/// Gets or sets the model space flag.
		/// </summary>
		public bool ModelSpace
		{
			get
			{
				return _ModelSpace;
			}
			set
			{
				AddSetObjectFieldCommand("_ModelSpace", value);
				_ModelSpace = value;
			}
		}

		/// <summary>
		/// Gets or sets the unshaded flag.
		/// </summary>
		public bool Unshaded
		{
			get
			{
				return _Unshaded;
			}
			set
			{
				AddSetObjectFieldCommand("_Unshaded", value);
				_Unshaded = value;
			}
		}

		/// <summary>
		/// Gets or sets the unfogged flag.
		/// </summary>
		public bool Unfogged
		{
			get
			{
				return _Unfogged;
			}
			set
			{
				AddSetObjectFieldCommand("_Unfogged", value);
				_Unfogged = value;
			}
		}

		/// <summary>
		/// Gets or sets the XY quad flag. This decides if the particles will be
		/// aligned with the XY-plane (the ground).
		/// </summary>
		public bool XYQuad
		{
			get
			{
				return _XYQuad;
			}
			set
			{
				AddSetObjectFieldCommand("_XYQuad", value);
				_XYQuad = value;
			}
		}

		/// <summary>
		/// Gets or sets the squirt flag.
		/// </summary>
		public bool Squirt
		{
			get
			{
				return _Squirt;
			}
			set
			{
				AddSetObjectFieldCommand("_Squirt", value);
				_Squirt = value;
			}
		}

		/// <summary>
		/// Gets or sets the head flag.
		/// </summary>
		public bool Head
		{
			get
			{
				return _Head;
			}
			set
			{
				AddSetObjectFieldCommand("_Head", value);
				_Head = value;
			}
		}

		/// <summary>
		/// Gets or sets the tail flag.
		/// </summary>
		public bool Tail
		{
			get
			{
				return _Tail;
			}
			set
			{
				AddSetObjectFieldCommand("_Tail", value);
				_Tail = value;
			}
		}

		/// <summary>
		/// Gets or sets the first segment.
		/// </summary>
		public CSegment Segment1
		{
			get
			{
				return _Segment1;
			}
			set
			{
				AddSetObjectFieldCommand("_Segment1", value);
				_Segment1 = value;
			}
		}

		/// <summary>
		/// Gets or sets the second segment.
		/// </summary>
		public CSegment Segment2
		{
			get
			{
				return _Segment2;
			}
			set
			{
				AddSetObjectFieldCommand("_Segment2", value);
				_Segment2 = value;
			}
		}

		/// <summary>
		/// Gets or sets the third segment.
		/// </summary>
		public CSegment Segment3
		{
			get
			{
				return _Segment3;
			}
			set
			{
				AddSetObjectFieldCommand("_Segment3", value);
				_Segment3 = value;
			}
		}

		/// <summary>
		/// Gets or sets the head life interval.
		/// </summary>
		public CInterval HeadLife
		{
			get
			{
				return _HeadLife;
			}
			set
			{
				AddSetObjectFieldCommand("_HeadLife", value);
				_HeadLife = value;
			}
		}

		/// <summary>
		/// Gets or sets the head decay interval.
		/// </summary>
		public CInterval HeadDecay
		{
			get
			{
				return _HeadDecay;
			}
			set
			{
				AddSetObjectFieldCommand("_HeadDecay", value);
				_HeadDecay = value;
			}
		}

		/// <summary>
		/// Gets or sets the tail life interval.
		/// </summary>
		public CInterval TailLife
		{
			get
			{
				return _TailLife;
			}
			set
			{
				AddSetObjectFieldCommand("_TailLife", value);
				_TailLife = value;
			}
		}

		/// <summary>
		/// Gets or sets the tail decay interval.
		/// </summary>
		public CInterval TailDecay
		{
			get
			{
				return _TailDecay;
			}
			set
			{
				AddSetObjectFieldCommand("_TailDecay", value);
				_TailDecay = value;
			}
		}

		/// <summary>
		/// Retrieves the speed animator.
		/// </summary>
		public CAnimator<float> Speed => _Speed ?? (_Speed = new CAnimator<float>(base.Model, new CFloat(0f)));

		/// <summary>
		/// Retrieves the variation animator.
		/// </summary>
		public CAnimator<float> Variation => _Variation ?? (_Variation = new CAnimator<float>(base.Model, new CFloat(0f)));

		/// <summary>
		/// Retrieves the latitude animator.
		/// </summary>
		public CAnimator<float> Latitude => _Latitude ?? (_Latitude = new CAnimator<float>(base.Model, new CFloat(0f)));

		/// <summary>
		/// Retrieves the gravity animator.
		/// </summary>
		public CAnimator<float> Gravity => _Gravity ?? (_Gravity = new CAnimator<float>(base.Model, new CFloat(0f)));

		/// <summary>
		/// Retrieves the emission rate animator.
		/// </summary>
		public CAnimator<float> EmissionRate => _EmissionRate ?? (_EmissionRate = new CAnimator<float>(base.Model, new CFloat(0f)));

		/// <summary>
		/// Retrieves the width animator.
		/// </summary>
		public CAnimator<float> Width => _Width ?? (_Width = new CAnimator<float>(base.Model, new CFloat(0f)));

		/// <summary>
		/// Retrieves the length animator.
		/// </summary>
		public CAnimator<float> Length => _Length ?? (_Length = new CAnimator<float>(base.Model, new CFloat(0f)));

		/// <summary>
		/// Retrieves the visibility animator.
		/// </summary>
		public CAnimator<float> Visibility => _Visibility ?? (_Visibility = new CAnimator<float>(base.Model, new CFloat(1f)));

		/// <summary>
		/// Retrieves the texture reference.
		/// </summary>
		public CObjectReference<CTexture> Texture => _Texture ?? (_Texture = new CObjectReference<CTexture>(base.Model));

		/// <summary>
		/// Parameterized constructor.
		/// </summary>
		/// <param name="Model">The model to be associated with this particle emitter 2</param>
		public CParticleEmitter2(CModel Model)
			: base(Model)
		{
		}

		/// <summary>
		/// Generates a string version of the particle emitter 2.
		/// </summary>
		/// <returns>The generated string</returns>
		public override string ToString()
		{
			return "Particle Emitter 2 #" + base.ObjectId;
		}
	}
}
