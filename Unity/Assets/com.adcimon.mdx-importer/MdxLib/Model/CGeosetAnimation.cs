using MdxLib.Animator;
using MdxLib.Animator.Animatable;
using MdxLib.Primitives;

namespace MdxLib.Model
{
	/// <summary>
	/// A geoset animation class. Can animate certain aspects of a geoset,
	/// like its color.
	/// </summary>
	public sealed class CGeosetAnimation : CObject<CGeosetAnimation>
	{
		private bool _UseColor;

		private bool _DropShadow;

		private CAnimator<MdxLib.Primitives.CVector3> _Color;

		private CAnimator<float> _Alpha;

		private CObjectReference<CGeoset> _Geoset;

		/// <summary>
		/// Gets or sets the use color flag.
		/// </summary>
		public bool UseColor
		{
			get
			{
				return _UseColor;
			}
			set
			{
				AddSetObjectFieldCommand("_UseColor", value);
				_UseColor = value;
			}
		}

		/// <summary>
		/// Gets or sets the drop shadow flag.
		/// </summary>
		public bool DropShadow
		{
			get
			{
				return _DropShadow;
			}
			set
			{
				AddSetObjectFieldCommand("_DropShadow", value);
				_DropShadow = value;
			}
		}

		/// <summary>
		/// Retrieves the color animator.
		/// </summary>
		public CAnimator<MdxLib.Primitives.CVector3> Color => _Color ?? (_Color = new CAnimator<MdxLib.Primitives.CVector3>(base.Model, new MdxLib.Animator.Animatable.CVector3(CConstants.DefaultColor)));

		/// <summary>
		/// Retrieves the alpha animator.
		/// </summary>
		public CAnimator<float> Alpha => _Alpha ?? (_Alpha = new CAnimator<float>(base.Model, new CFloat(1f)));

		/// <summary>
		/// Retrieves the geoset reference.
		/// </summary>
		public CObjectReference<CGeoset> Geoset => _Geoset ?? (_Geoset = new CObjectReference<CGeoset>(base.Model));

		/// <summary>
		/// Parameterized constructor.
		/// </summary>
		/// <param name="Model">The model to be associated with this geoset animation</param>
		public CGeosetAnimation(CModel Model)
			: base(Model)
		{
		}

		/// <summary>
		/// Generates a string version of the geoset animation.
		/// </summary>
		/// <returns>The generated string</returns>
		public override string ToString()
		{
			return "Geoset Animation #" + base.ObjectId;
		}
	}
}
