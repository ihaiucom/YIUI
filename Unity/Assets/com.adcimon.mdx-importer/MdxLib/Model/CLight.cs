using MdxLib.Animator;
using MdxLib.Animator.Animatable;
using MdxLib.Primitives;

namespace MdxLib.Model
{
	/// <summary>
	/// A light class. Illuminates the model and its surrounding environment.
	/// </summary>
	public sealed class CLight : CNode<CLight>
	{
		private ELightType _Type;

		private CAnimator<float> _AttenuationStart;

		private CAnimator<float> _AttenuationEnd;

		private CAnimator<MdxLib.Primitives.CVector3> _Color;

		private CAnimator<float> _Intensity;

		private CAnimator<MdxLib.Primitives.CVector3> _AmbientColor;

		private CAnimator<float> _AmbientIntensity;

		private CAnimator<float> _Visibility;

		/// <summary>
		/// Retrieves the node ID (if added to a container).
		/// </summary>
		public override int NodeId => base.Model.GetLightNodeId(this);

		/// <summary>
		/// Gets or sets the type.
		/// </summary>
		public ELightType Type
		{
			get
			{
				return _Type;
			}
			set
			{
				AddSetObjectFieldCommand("_Type", value);
				_Type = value;
			}
		}

		/// <summary>
		/// Retrieves the attenuation start animator.
		/// </summary>
		public CAnimator<float> AttenuationStart => _AttenuationStart ?? (_AttenuationStart = new CAnimator<float>(base.Model, new CFloat(0f)));

		/// <summary>
		/// Retrieves the attenuation end animator.
		/// </summary>
		public CAnimator<float> AttenuationEnd => _AttenuationEnd ?? (_AttenuationEnd = new CAnimator<float>(base.Model, new CFloat(0f)));

		/// <summary>
		/// Retrieves the color animator.
		/// </summary>
		public CAnimator<MdxLib.Primitives.CVector3> Color => _Color ?? (_Color = new CAnimator<MdxLib.Primitives.CVector3>(base.Model, new MdxLib.Animator.Animatable.CVector3(CConstants.DefaultColor)));

		/// <summary>
		/// Retrieves the intensity animator.
		/// </summary>
		public CAnimator<float> Intensity => _Intensity ?? (_Intensity = new CAnimator<float>(base.Model, new CFloat(0f)));

		/// <summary>
		/// Retrieves the ambient color animator.
		/// </summary>
		public CAnimator<MdxLib.Primitives.CVector3> AmbientColor => _AmbientColor ?? (_AmbientColor = new CAnimator<MdxLib.Primitives.CVector3>(base.Model, new MdxLib.Animator.Animatable.CVector3(CConstants.DefaultColor)));

		/// <summary>
		/// Retrieves the ambient intensity animator.
		/// </summary>
		public CAnimator<float> AmbientIntensity => _AmbientIntensity ?? (_AmbientIntensity = new CAnimator<float>(base.Model, new CFloat(0f)));

		/// <summary>
		/// Retrieves the visibility animator.
		/// </summary>
		public CAnimator<float> Visibility => _Visibility ?? (_Visibility = new CAnimator<float>(base.Model, new CFloat(1f)));

		/// <summary>
		/// Parameterized constructor.
		/// </summary>
		/// <param name="Model">The model to be associated with this light</param>
		public CLight(CModel Model)
			: base(Model)
		{
		}

		/// <summary>
		/// Generates a string version of the light.
		/// </summary>
		/// <returns>The generated string</returns>
		public override string ToString()
		{
			return "Light #" + base.ObjectId;
		}
	}
}
