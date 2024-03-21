using MdxLib.Animator;
using MdxLib.Primitives;

namespace MdxLib.Model
{
	/// <summary>
	/// The base interface for all node components.
	/// </summary>
	public interface INode : IObject, IUnknown
	{
		/// <summary>
		/// Retrieves the node ID (if added to a container).
		/// </summary>
		int NodeId { get; }

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		string Name { get; set; }

		/// <summary>
		/// Gets or sets the don't inherit translation flag.
		/// </summary>
		bool DontInheritTranslation { get; set; }

		/// <summary>
		/// Gets or sets the don't inherit rotation flag.
		/// </summary>
		bool DontInheritRotation { get; set; }

		/// <summary>
		/// Gets or sets the don't inherit scaling flag.
		/// </summary>
		bool DontInheritScaling { get; set; }

		/// <summary>
		/// Gets or sets the billboarded flag.
		/// </summary>
		bool Billboarded { get; set; }

		/// <summary>
		/// Gets or sets the billboarded lock X flag.
		/// </summary>
		bool BillboardedLockX { get; set; }

		/// <summary>
		/// Gets or sets the billboarded lock Y flag.
		/// </summary>
		bool BillboardedLockY { get; set; }

		/// <summary>
		/// Gets or sets the billboarded lock Z flag.
		/// </summary>
		bool BillboardedLockZ { get; set; }

		/// <summary>
		/// Gets or sets the camera anchored flag.
		/// </summary>
		bool CameraAnchored { get; set; }

		/// <summary>
		/// Gets or sets the pivot point.
		/// </summary>
		CVector3 PivotPoint { get; set; }

		/// <summary>
		/// Retrieves the translation animator.
		/// </summary>
		CAnimator<CVector3> Translation { get; }

		/// <summary>
		/// Retrieves the rotation animator.
		/// </summary>
		CAnimator<CVector4> Rotation { get; }

		/// <summary>
		/// Retrieves the scaling animator.
		/// </summary>
		CAnimator<CVector3> Scaling { get; }

		/// <summary>
		/// Retrieves the parent reference. Is used to construct the node skeleton hiearchy.
		/// </summary>
		CNodeReference Parent { get; }
	}
}
