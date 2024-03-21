using MdxLib.Animator;
using MdxLib.Animator.Animatable;

namespace MdxLib.Model
{
	/// <summary>
	/// An attachment class. Represents a point to which stuff can
	/// be attached, like buffs and other special effects.
	/// </summary>
	public sealed class CAttachment : CNode<CAttachment>
	{
		private string _Path = "";

		private int _AttachmentId = -1;

		private CAnimator<float> _Visibility;

		/// <summary>
		/// Retrieves the node ID (if added to a container).
		/// </summary>
		public override int NodeId => base.Model.GetAttachmentNodeId(this);

		/// <summary>
		/// Gets or sets the path.
		/// </summary>
		public string Path
		{
			get
			{
				return _Path;
			}
			set
			{
				AddSetObjectFieldCommand("_Path", value);
				_Path = value;
			}
		}

		/// <summary>
		/// Gets or sets the attachment ID.
		/// </summary>
		public int AttachmentId
		{
			get
			{
				return _AttachmentId;
			}
			set
			{
				AddSetObjectFieldCommand("_AttachmentId", value);
				_AttachmentId = value;
			}
		}

		/// <summary>
		/// Retrieves the visibility animator.
		/// </summary>
		public CAnimator<float> Visibility => _Visibility ?? (_Visibility = new CAnimator<float>(base.Model, new CFloat(1f)));

		/// <summary>
		/// Parameterized constructor.
		/// </summary>
		/// <param name="Model">The model to be associated with this attachment</param>
		public CAttachment(CModel Model)
			: base(Model)
		{
		}

		/// <summary>
		/// Generates a string version of the attachment.
		/// </summary>
		/// <returns>The generated string</returns>
		public override string ToString()
		{
			return "Attachment #" + base.ObjectId;
		}
	}
}
