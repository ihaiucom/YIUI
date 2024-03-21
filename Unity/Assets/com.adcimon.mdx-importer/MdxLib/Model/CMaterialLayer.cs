using MdxLib.Animator;
using MdxLib.Animator.Animatable;

namespace MdxLib.Model
{
	/// <summary>
	/// A material layer class. Represents a single layer for a material.
	/// </summary>
	public sealed class CMaterialLayer : CObject<CMaterialLayer>
	{
		private EMaterialLayerFilterMode _FilterMode;

		private int _CoordId;

		private bool _Unshaded;

		private bool _Unfogged;

		private bool _TwoSided;

		private bool _SphereEnvironmentMap;

		private bool _NoDepthTest;

		private bool _NoDepthSet;

		private CAnimator<int> _TextureId;

		private CAnimator<float> _Alpha;

		private CObjectReference<CTexture> _Texture;

		private CObjectReference<CTextureAnimation> _TextureAnimation;

		/// <summary>
		/// Gets or sets the filter mode.
		/// </summary>
		public EMaterialLayerFilterMode FilterMode
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
		/// Gets or sets the coord ID.
		/// </summary>
		public int CoordId
		{
			get
			{
				return _CoordId;
			}
			set
			{
				AddSetObjectFieldCommand("_CoordId", value);
				_CoordId = value;
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
		/// Gets or sets the two sided flag.
		/// </summary>
		public bool TwoSided
		{
			get
			{
				return _TwoSided;
			}
			set
			{
				AddSetObjectFieldCommand("_TwoSided", value);
				_TwoSided = value;
			}
		}

		/// <summary>
		/// Gets or sets the sphere environment map flag.
		/// </summary>
		public bool SphereEnvironmentMap
		{
			get
			{
				return _SphereEnvironmentMap;
			}
			set
			{
				AddSetObjectFieldCommand("_SphereEnvironmentMap", value);
				_SphereEnvironmentMap = value;
			}
		}

		/// <summary>
		/// Gets or sets the no depth test flag.
		/// </summary>
		public bool NoDepthTest
		{
			get
			{
				return _NoDepthTest;
			}
			set
			{
				AddSetObjectFieldCommand("_NoDepthTest", value);
				_NoDepthTest = value;
			}
		}

		/// <summary>
		/// Gets or sets the no depth set flag.
		/// </summary>
		public bool NoDepthSet
		{
			get
			{
				return _NoDepthSet;
			}
			set
			{
				AddSetObjectFieldCommand("_NoDepthSet", value);
				_NoDepthSet = value;
			}
		}

		/// <summary>
		/// Retrieves the texture ID animator. Use this if the texture is animated,
		/// otherwise use the Texture reference.
		/// </summary>
		public CAnimator<int> TextureId => _TextureId ?? (_TextureId = new CAnimator<int>(base.Model, new CInteger(-1)));

		/// <summary>
		/// Retrieves the alpha animator.
		/// </summary>
		public CAnimator<float> Alpha => _Alpha ?? (_Alpha = new CAnimator<float>(base.Model, new CFloat(1f)));

		/// <summary>
		/// Retrieves the texture reference. Use this if the texture is static,
		/// otherwise use the TextureId animator.
		/// </summary>
		public CObjectReference<CTexture> Texture => _Texture ?? (_Texture = new CObjectReference<CTexture>(base.Model));

		/// <summary>
		/// Retrieves the texture animation reference.
		/// </summary>
		public CObjectReference<CTextureAnimation> TextureAnimation => _TextureAnimation ?? (_TextureAnimation = new CObjectReference<CTextureAnimation>(base.Model));

		/// <summary>
		/// Parameterized constructor.
		/// </summary>
		/// <param name="Model">The model to be associated with this material layer</param>
		public CMaterialLayer(CModel Model)
			: base(Model)
		{
		}

		/// <summary>
		/// Generates a string version of the material layer.
		/// </summary>
		/// <returns>The generated string</returns>
		public override string ToString()
		{
			return "Material Layer #" + base.ObjectId;
		}
	}
}
