using MdxLib.Animator;
using MdxLib.Animator.Animatable;

namespace MdxLib.Model
{
	/// <summary>
	/// A particle emitter class. Emits particles or other models.
	/// For more advanced emitter options see particle emitter 2.
	/// </summary>
	public sealed class CParticleEmitter : CNode<CParticleEmitter>
	{
		private string _FileName = "";

		private bool _EmitterUsesMdl;

		private bool _EmitterUsesTga;

		private CAnimator<float> _EmissionRate;

		private CAnimator<float> _Gravity;

		private CAnimator<float> _Longitude;

		private CAnimator<float> _Latitude;

		private CAnimator<float> _LifeSpan;

		private CAnimator<float> _InitialVelocity;

		private CAnimator<float> _Visibility;

		/// <summary>
		/// Retrieves the node ID (if added to a container).
		/// </summary>
		public override int NodeId => base.Model.GetParticleEmitterNodeId(this);

		/// <summary>
		/// Gets or sets the filename.
		/// </summary>
		public string FileName
		{
			get
			{
				return _FileName;
			}
			set
			{
				AddSetObjectFieldCommand("_FileName", value);
				_FileName = value;
			}
		}

		/// <summary>
		/// Gets or sets the emitter uses mdl flag.
		/// </summary>
		public bool EmitterUsesMdl
		{
			get
			{
				return _EmitterUsesMdl;
			}
			set
			{
				AddSetObjectFieldCommand("_EmitterUsesMdl", value);
				_EmitterUsesMdl = value;
			}
		}

		/// <summary>
		/// Gets or sets the emitter uses tga flag.
		/// </summary>
		public bool EmitterUsesTga
		{
			get
			{
				return _EmitterUsesTga;
			}
			set
			{
				AddSetObjectFieldCommand("_EmitterUsesTga", value);
				_EmitterUsesTga = value;
			}
		}

		/// <summary>
		/// Retrieves the emission rate animator.
		/// </summary>
		public CAnimator<float> EmissionRate => _EmissionRate ?? (_EmissionRate = new CAnimator<float>(base.Model, new CFloat(0f)));

		/// <summary>
		/// Retrieves the gravity animator.
		/// </summary>
		public CAnimator<float> Gravity => _Gravity ?? (_Gravity = new CAnimator<float>(base.Model, new CFloat(0f)));

		/// <summary>
		/// Retrives the longitude animator.
		/// </summary>
		public CAnimator<float> Longitude => _Longitude ?? (_Longitude = new CAnimator<float>(base.Model, new CFloat(0f)));

		/// <summary>
		/// Retrieves the latitude animator.
		/// </summary>
		public CAnimator<float> Latitude => _Latitude ?? (_Latitude = new CAnimator<float>(base.Model, new CFloat(0f)));

		/// <summary>
		/// Retrives the life span animator.
		/// </summary>
		public CAnimator<float> LifeSpan => _LifeSpan ?? (_LifeSpan = new CAnimator<float>(base.Model, new CFloat(0f)));

		/// <summary>
		/// Retrives the initial velocity animator.
		/// </summary>
		public CAnimator<float> InitialVelocity => _InitialVelocity ?? (_InitialVelocity = new CAnimator<float>(base.Model, new CFloat(0f)));

		/// <summary>
		/// Retrieves the visibility animator.
		/// </summary>
		public CAnimator<float> Visibility => _Visibility ?? (_Visibility = new CAnimator<float>(base.Model, new CFloat(1f)));

		/// <summary>
		/// Parameterized constructor.
		/// </summary>
		/// <param name="Model">The model to be associated with this particle emitter</param>
		public CParticleEmitter(CModel Model)
			: base(Model)
		{
		}

		/// <summary>
		/// Generates a string version of the particle emitter.
		/// </summary>
		/// <returns>The generated string</returns>
		public override string ToString()
		{
			return "Particle Emitter #" + base.ObjectId;
		}
	}
}
