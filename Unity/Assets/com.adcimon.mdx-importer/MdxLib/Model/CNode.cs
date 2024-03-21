using System.Collections.Generic;
using MdxLib.Animator;
using MdxLib.Animator.Animatable;
using MdxLib.Command;
using MdxLib.Primitives;

namespace MdxLib.Model
{
	/// <summary>
	/// The base class for all node components. This class is templated so
	/// use INode if you want non-specified access.
	/// </summary>
	/// <typeparam name="T">The object type (class that inherits this class)</typeparam>
	public abstract class CNode<T> : CObject<T>, INode, IObject, IUnknown where T : CNode<T>
	{
		private string _Name = "";

		private bool _DontInheritTranslation;

		private bool _DontInheritRotation;

		private bool _DontInheritScaling;

		private bool _Billboarded;

		private bool _BillboardedLockX;

		private bool _BillboardedLockY;

		private bool _BillboardedLockZ;

		private bool _CameraAnchored;

		private MdxLib.Primitives.CVector3 _PivotPoint = CConstants.DefaultVector3;

		private CAnimator<MdxLib.Primitives.CVector3> _Translation;

		private CAnimator<MdxLib.Primitives.CVector4> _Rotation;

		private CAnimator<MdxLib.Primitives.CVector3> _Scaling;

		private CNodeReference _Parent;

		/// <summary>
		/// Retrieves the node ID (if added to a container).
		/// </summary>
		public abstract int NodeId { get; }

		/// <summary>
		/// Checks if the node has references pointing to it.
		/// </summary>
		public override bool HasReferences
		{
			get
			{
				if (base.NodeReferenceSet.Count > 0)
				{
					return true;
				}
				return base.HasReferences;
			}
		}

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
				AddSetNodeFieldCommand("_Name", value);
				_Name = value;
			}
		}

		/// <summary>
		/// Gets or sets the don't inherit translation flag.
		/// </summary>
		public bool DontInheritTranslation
		{
			get
			{
				return _DontInheritTranslation;
			}
			set
			{
				AddSetNodeFieldCommand("_DontInheritTranslation", value);
				_DontInheritTranslation = value;
			}
		}

		/// <summary>
		/// Gets or sets the don't inherit rotation flag.
		/// </summary>
		public bool DontInheritRotation
		{
			get
			{
				return _DontInheritRotation;
			}
			set
			{
				AddSetNodeFieldCommand("_DontInheritRotation", value);
				_DontInheritRotation = value;
			}
		}

		/// <summary>
		/// Gets or sets the don't inherit scaling flag.
		/// </summary>
		public bool DontInheritScaling
		{
			get
			{
				return _DontInheritScaling;
			}
			set
			{
				AddSetNodeFieldCommand("_DontInheritScaling", value);
				_DontInheritScaling = value;
			}
		}

		/// <summary>
		/// Gets or sets the billboarded flag.
		/// </summary>
		public bool Billboarded
		{
			get
			{
				return _Billboarded;
			}
			set
			{
				AddSetNodeFieldCommand("_Billboarded", value);
				_Billboarded = value;
			}
		}

		/// <summary>
		/// Gets or sets the billboarded lock X flag.
		/// </summary>
		public bool BillboardedLockX
		{
			get
			{
				return _BillboardedLockX;
			}
			set
			{
				AddSetNodeFieldCommand("_BillboardedLockX", value);
				_BillboardedLockX = value;
			}
		}

		/// <summary>
		/// Gets or sets the billboarded lock Y flag.
		/// </summary>
		public bool BillboardedLockY
		{
			get
			{
				return _BillboardedLockY;
			}
			set
			{
				AddSetNodeFieldCommand("_BillboardedLockY", value);
				_BillboardedLockY = value;
			}
		}

		/// <summary>
		/// Gets or sets the billboarded lock Z flag.
		/// </summary>
		public bool BillboardedLockZ
		{
			get
			{
				return _BillboardedLockZ;
			}
			set
			{
				AddSetNodeFieldCommand("_BillboardedLockZ", value);
				_BillboardedLockZ = value;
			}
		}

		/// <summary>
		/// Gets or sets the camera anchored flag.
		/// </summary>
		public bool CameraAnchored
		{
			get
			{
				return _CameraAnchored;
			}
			set
			{
				AddSetNodeFieldCommand("_CameraAnchored", value);
				_CameraAnchored = value;
			}
		}

		/// <summary>
		/// Gets or sets the pivot point.
		/// </summary>
		public MdxLib.Primitives.CVector3 PivotPoint
		{
			get
			{
				return _PivotPoint;
			}
			set
			{
				AddSetNodeFieldCommand("_PivotPoint", value);
				_PivotPoint = value;
			}
		}

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
		/// Retrieves the parent reference. Is used to construct the node skeleton hiearchy.
		/// </summary>
		public CNodeReference Parent => _Parent ?? (_Parent = new CNodeReference(base.Model));

		/// <summary>
		/// Parameterized constructor.
		/// </summary>
		/// <param name="Model">The model to be associated with this node</param>
		public CNode(CModel Model)
			: base(Model)
		{
		}

		internal override void BuildNodeDetacherList(ICollection<CDetacher> DetacherList)
		{
			foreach (object item in base.NodeReferenceSet)
			{
				CNodeReference cNodeReference = item as CNodeReference;
				if (cNodeReference != null)
				{
					DetacherList.Add(new CNodeDetacher(cNodeReference));
				}
			}
		}

		internal void AddSetNodeFieldCommand<T2>(string FieldName, T2 Value)
		{
			if (base.Model.CommandGroup != null)
			{
				base.Model.CommandGroup.Add(new CSetNodeField<T, T2>((T)this, FieldName, Value));
			}
		}
	}
}
