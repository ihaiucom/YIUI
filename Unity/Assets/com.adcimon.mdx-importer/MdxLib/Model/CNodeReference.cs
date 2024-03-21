using System;
using MdxLib.Command;

namespace MdxLib.Model
{
	/// <summary>
	/// Handles a reference to a node component. References are powerful links to other
	/// parts of the model which will not be invalid even if you add/remove stuff
	/// (like a common ID would).
	/// </summary>
	public sealed class CNodeReference
	{
		private CModel _Model;

		private INode _Node;

		/// <summary>
		/// Retrieves the associated model.
		/// </summary>
		public CModel Model => _Model;

		/// <summary>
		/// Retrieves the attached node, or null if not attached.
		/// </summary>
		public INode Node => _Node;

		/// <summary>
		/// Retrieves the node ID of the attached node, or InvalidId if not attached.
		/// </summary>
		public int NodeId
		{
			get
			{
				if (_Node == null)
				{
					return -1;
				}
				return _Node.NodeId;
			}
		}

		/// <summary>
		/// Retrieves the object ID of the attached node, or InvalidId if not attached.
		/// </summary>
		public int ObjectId
		{
			get
			{
				if (_Node == null)
				{
					return -1;
				}
				return _Node.ObjectId;
			}
		}

		internal bool CanAddCommand => _Model.CommandGroup != null;

		internal INode InternalNode
		{
			get
			{
				return _Node;
			}
			set
			{
				_Node = value;
			}
		}

		internal CNodeReference(CModel Model)
		{
			_Model = Model;
		}

		/// <summary>
		/// Attaches the reference to a node.
		/// </summary>
		/// <param name="Node">The node to attach to</param>
		public void Attach(INode Node)
		{
			Detach();
			if (Node == null)
			{
				return;
			}
			if (Node.Model != _Model)
			{
				throw new InvalidOperationException("The node belongs to another model!");
			}
			CUnknown cUnknown = Node as CUnknown;
			if (cUnknown != null)
			{
				if (CanAddCommand)
				{
					ICommand command = new CAttachNode(this, Node);
					command.Do();
					AddCommand(command);
				}
				else
				{
					cUnknown.NodeReferenceSet.Add(this);
					_Node = Node;
				}
			}
		}

		/// <summary>
		/// Detachers the reference from the node (if attached).
		/// </summary>
		public void Detach()
		{
			if (_Node == null)
			{
				return;
			}
			CUnknown cUnknown = _Node as CUnknown;
			if (cUnknown != null)
			{
				if (CanAddCommand)
				{
					ICommand command = new CDetachNode(this, _Node);
					command.Do();
					AddCommand(command);
				}
				else
				{
					cUnknown.NodeReferenceSet.Remove(this);
					_Node = null;
				}
			}
		}

		internal void ForceAttach(INode Node)
		{
			ForceDetach();
			CUnknown cUnknown = Node as CUnknown;
			if (cUnknown != null)
			{
				cUnknown.NodeReferenceSet.Add(this);
				_Node = Node;
			}
		}

		internal void ForceDetach()
		{
			CUnknown cUnknown = _Node as CUnknown;
			if (cUnknown != null)
			{
				cUnknown.NodeReferenceSet.Remove(this);
				_Node = null;
			}
		}

		internal void AddCommand(ICommand Command)
		{
			if (_Model.CommandGroup != null)
			{
				_Model.CommandGroup.Add(Command);
			}
		}
	}
}
