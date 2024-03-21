using MdxLib.Animator;
using MdxLib.Animator.Animatable;
using MdxLib.Primitives;

namespace MdxLib.Model
{
	/// <summary>
	/// A texture animation class. Animates the vertex coordinates of the
	/// texture to create effects like flowing water.
	/// </summary>
	public sealed class CTextureAnimation : CObject<CTextureAnimation>
	{
		private CAnimator<MdxLib.Primitives.CVector3> _Translation;

		private CAnimator<MdxLib.Primitives.CVector4> _Rotation;

		private CAnimator<MdxLib.Primitives.CVector3> _Scaling;

		/// <summary>
		/// Retrieves the translation animator.
		/// </summary>
		public CAnimator<MdxLib.Primitives.CVector3> Translation => _Translation ?? (_Translation = new CAnimator<MdxLib.Primitives.CVector3>(base.Model, new MdxLib.Animator.Animatable.CVector3(CConstants.DefaultTranslation)));

		/// <summary>
		/// Retrieves the rotation animator.
		/// </summary>
		public CAnimator<MdxLib.Primitives.CVector4> Rotation => _Rotation ?? (_Rotation = new CAnimator<MdxLib.Primitives.CVector4>(base.Model, new CQuaternion(CConstants.DefaultRotation)));

		/// <summary>
		/// Retrieves the scaling animator.
		/// </summary>
		public CAnimator<MdxLib.Primitives.CVector3> Scaling => _Scaling ?? (_Scaling = new CAnimator<MdxLib.Primitives.CVector3>(base.Model, new MdxLib.Animator.Animatable.CVector3(CConstants.DefaultScaling)));

		/// <summary>
		/// Parameterized constructor.
		/// </summary>
		/// <param name="Model">The model to be associated with this texture animation</param>
		public CTextureAnimation(CModel Model)
			: base(Model)
		{
		}

		/// <summary>
		/// Generates a string version of the texture animation.
		/// </summary>
		/// <returns>The generated string</returns>
		public override string ToString()
		{
			return "Texture Animation #" + base.ObjectId;
		}
	}
}
