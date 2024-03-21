using System;
using MdxLib.Animator;
using MdxLib.Animator.Animatable;
using MdxLib.Primitives;

namespace MdxLib.Model
{
	/// <summary>
	/// A camera class. Represents a camera that are used for
	/// unit portraits.
	/// </summary>
	public sealed class CCamera : CObject<CCamera>
	{
		private string _Name = "";

		private float _FieldOfView = (float)Math.PI / 4f;

		private float _NearDistance = 10f;

		private float _FarDistance = 1000f;

		private MdxLib.Primitives.CVector3 _Position = CConstants.DefaultVector3;

		private MdxLib.Primitives.CVector3 _TargetPosition = CConstants.DefaultVector3;

		private CAnimator<float> _Rotation;

		private CAnimator<MdxLib.Primitives.CVector3> _Translation;

		private CAnimator<MdxLib.Primitives.CVector3> _TargetTranslation;

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		public string Name
		{
			get
			{
				return _Name;
			}
			set
			{
				AddSetObjectFieldCommand("_Name", value);
				_Name = value;
			}
		}

		/// <summary>
		/// Gets or sets the field of view.
		/// </summary>
		public float FieldOfView
		{
			get
			{
				return _FieldOfView;
			}
			set
			{
				AddSetObjectFieldCommand("_FieldOfView", value);
				_FieldOfView = value;
			}
		}

		/// <summary>
		/// Gets or sets the near clipping distance.
		/// </summary>
		public float NearDistance
		{
			get
			{
				return _NearDistance;
			}
			set
			{
				AddSetObjectFieldCommand("_NearDistance", value);
				_NearDistance = value;
			}
		}

		/// <summary>
		/// Gets or sets the far clipping distance.
		/// </summary>
		public float FarDistance
		{
			get
			{
				return _FarDistance;
			}
			set
			{
				AddSetObjectFieldCommand("_FarDistance", value);
				_FarDistance = value;
			}
		}

		/// <summary>
		/// Gets or sets the position (where the camera is).
		/// </summary>
		public MdxLib.Primitives.CVector3 Position
		{
			get
			{
				return _Position;
			}
			set
			{
				AddSetObjectFieldCommand("_Position", value);
				_Position = value;
			}
		}

		/// <summary>
		/// Gets or sets the target position (where the camera looks at).
		/// </summary>
		public MdxLib.Primitives.CVector3 TargetPosition
		{
			get
			{
				return _TargetPosition;
			}
			set
			{
				AddSetObjectFieldCommand("_TargetPosition", value);
				_TargetPosition = value;
			}
		}

		/// <summary>
		/// Retrieves the rotation animator.
		/// </summary>
		public CAnimator<float> Rotation => _Rotation ?? (_Rotation = new CAnimator<float>(base.Model, new CFloat(0f)));

		/// <summary>
		/// Retrieves the translation animator (where the camera is).
		/// </summary>
		public CAnimator<MdxLib.Primitives.CVector3> Translation => _Translation ?? (_Translation = new CAnimator<MdxLib.Primitives.CVector3>(base.Model, new MdxLib.Animator.Animatable.CVector3(CConstants.DefaultTranslation)));

		/// <summary>
		/// Retrieves the target translation animator (where the camera looks at).
		/// </summary>
		public CAnimator<MdxLib.Primitives.CVector3> TargetTranslation => _TargetTranslation ?? (_TargetTranslation = new CAnimator<MdxLib.Primitives.CVector3>(base.Model, new MdxLib.Animator.Animatable.CVector3(CConstants.DefaultTranslation)));

		/// <summary>
		/// Parameterized constructor.
		/// </summary>
		/// <param name="Model">The model to be associated with this camera</param>
		public CCamera(CModel Model)
			: base(Model)
		{
		}

		/// <summary>
		/// Generates a string version of the camera.
		/// </summary>
		/// <returns>The generated string</returns>
		public override string ToString()
		{
			return "Camera #" + base.ObjectId;
		}
	}
}
