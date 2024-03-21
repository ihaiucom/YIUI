using System;
using System.Collections;
using System.Collections.Generic;

namespace MdxLib.Model
{
	/// <summary>
	/// Stores nodes which can be added, removed and enumerated. This is a wrapper
	/// around multiple object containers (for the node types).
	/// </summary>
	public sealed class CNodeContainer : IList<INode>, ICollection<INode>, IEnumerable<INode>, IEnumerable
	{
		private CModel _Model;

		/// <summary>
		/// Retrieves the associated model.
		/// </summary>
		public CModel Model => _Model;

		/// <summary>
		/// Retrieves the number of nodes in the container.
		/// </summary>
		public int Count
		{
			get
			{
				int num = 0;
				if (_Model.HasBones)
				{
					num += _Model.Bones.Count;
				}
				if (_Model.HasLights)
				{
					num += _Model.Lights.Count;
				}
				if (_Model.HasHelpers)
				{
					num += _Model.Helpers.Count;
				}
				if (_Model.HasAttachments)
				{
					num += _Model.Attachments.Count;
				}
				if (_Model.HasParticleEmitters)
				{
					num += _Model.ParticleEmitters.Count;
				}
				if (_Model.HasParticleEmitters2)
				{
					num += _Model.ParticleEmitters2.Count;
				}
				if (_Model.HasRibbonEmitters)
				{
					num += _Model.RibbonEmitters.Count;
				}
				if (_Model.HasEvents)
				{
					num += _Model.Events.Count;
				}
				if (_Model.HasCollisionShapes)
				{
					num += _Model.CollisionShapes.Count;
				}
				return num;
			}
		}

		/// <summary>
		/// Checks if the container is read-only (which it isn't).
		/// </summary>
		public bool IsReadOnly => false;

		/// <summary>
		/// Gets or sets a node in the container.
		/// </summary>
		/// <param name="Index">The index to get or set at</param>
		/// <returns>The accessed node</returns>
		public INode this[int Index]
		{
			get
			{
				return Get(Index);
			}
			set
			{
				if (value != null)
				{
					Set(Index, value);
				}
				else
				{
					RemoveAt(Index);
				}
			}
		}

		internal CNodeContainer(CModel Model)
		{
			_Model = Model;
		}

		/// <summary>
		/// Clears all nodes.
		/// </summary>
		public void Clear()
		{
			if (_Model.HasBones)
			{
				_Model.Bones.Clear();
			}
			if (_Model.HasLights)
			{
				_Model.Lights.Clear();
			}
			if (_Model.HasHelpers)
			{
				_Model.Helpers.Clear();
			}
			if (_Model.HasAttachments)
			{
				_Model.Attachments.Clear();
			}
			if (_Model.HasParticleEmitters)
			{
				_Model.ParticleEmitters.Clear();
			}
			if (_Model.HasParticleEmitters2)
			{
				_Model.ParticleEmitters2.Clear();
			}
			if (_Model.HasRibbonEmitters)
			{
				_Model.RibbonEmitters.Clear();
			}
			if (_Model.HasEvents)
			{
				_Model.Events.Clear();
			}
			if (_Model.HasCollisionShapes)
			{
				_Model.CollisionShapes.Clear();
			}
		}

		/// <summary>
		/// Adds a new node.
		/// </summary>
		/// <param name="Node">The node to add</param>
		public void Add(INode Node)
		{
			if (Node != null)
			{
				if (Node.Model != _Model)
				{
					throw new InvalidOperationException("The node belongs to another model!");
				}
				if (Node is CBone)
				{
					_Model.Bones.Add(Node as CBone);
				}
				if (Node is CLight)
				{
					_Model.Lights.Add(Node as CLight);
				}
				if (Node is CHelper)
				{
					_Model.Helpers.Add(Node as CHelper);
				}
				if (Node is CAttachment)
				{
					_Model.Attachments.Add(Node as CAttachment);
				}
				if (Node is CParticleEmitter)
				{
					_Model.ParticleEmitters.Add(Node as CParticleEmitter);
				}
				if (Node is CParticleEmitter2)
				{
					_Model.ParticleEmitters2.Add(Node as CParticleEmitter2);
				}
				if (Node is CRibbonEmitter)
				{
					_Model.RibbonEmitters.Add(Node as CRibbonEmitter);
				}
				if (Node is CEvent)
				{
					_Model.Events.Add(Node as CEvent);
				}
				if (Node is CCollisionShape)
				{
					_Model.CollisionShapes.Add(Node as CCollisionShape);
				}
			}
		}

		/// <summary>
		/// Inserts a new node at a specific index.
		/// </summary>
		/// <param name="Index">The index to insert at</param>
		/// <param name="Node">The node to insert</param>
		public void Insert(int Index, INode Node)
		{
			if (Node == null)
			{
				return;
			}
			if (Node.Model != _Model)
			{
				throw new InvalidOperationException("The node belongs to another model!");
			}
			if (_Model.HasBones)
			{
				if (Index < 0)
				{
					return;
				}
				if (Index < _Model.Bones.Count)
				{
					_Model.Bones.Insert(Index, Node as CBone);
				}
				Index -= _Model.Bones.Count;
			}
			if (_Model.HasLights)
			{
				if (Index < 0)
				{
					return;
				}
				if (Index < _Model.Lights.Count)
				{
					_Model.Lights.Insert(Index, Node as CLight);
				}
				Index -= _Model.Lights.Count;
			}
			if (_Model.HasHelpers)
			{
				if (Index < 0)
				{
					return;
				}
				if (Index < _Model.Helpers.Count)
				{
					_Model.Helpers.Insert(Index, Node as CHelper);
				}
				Index -= _Model.Helpers.Count;
			}
			if (_Model.HasAttachments)
			{
				if (Index < 0)
				{
					return;
				}
				if (Index < _Model.Attachments.Count)
				{
					_Model.Attachments.Insert(Index, Node as CAttachment);
				}
				Index -= _Model.Attachments.Count;
			}
			if (_Model.HasParticleEmitters)
			{
				if (Index < 0)
				{
					return;
				}
				if (Index < _Model.ParticleEmitters.Count)
				{
					_Model.ParticleEmitters.Insert(Index, Node as CParticleEmitter);
				}
				Index -= _Model.ParticleEmitters.Count;
			}
			if (_Model.HasParticleEmitters2)
			{
				if (Index < 0)
				{
					return;
				}
				if (Index < _Model.ParticleEmitters2.Count)
				{
					_Model.ParticleEmitters2.Insert(Index, Node as CParticleEmitter2);
				}
				Index -= _Model.ParticleEmitters2.Count;
			}
			if (_Model.HasRibbonEmitters)
			{
				if (Index < 0)
				{
					return;
				}
				if (Index < _Model.RibbonEmitters.Count)
				{
					_Model.RibbonEmitters.Insert(Index, Node as CRibbonEmitter);
				}
				Index -= _Model.RibbonEmitters.Count;
			}
			if (_Model.HasEvents)
			{
				if (Index < 0)
				{
					return;
				}
				if (Index < _Model.Events.Count)
				{
					_Model.Events.Insert(Index, Node as CEvent);
				}
				Index -= _Model.Events.Count;
			}
			if (_Model.HasCollisionShapes && Index >= 0 && Index < _Model.CollisionShapes.Count)
			{
				_Model.CollisionShapes.Insert(Index, Node as CCollisionShape);
			}
		}

		/// <summary>
		/// Sets a new node at a specific index (removing whatever is there).
		/// </summary>
		/// <param name="Index">The index to set at</param>
		/// <param name="Node">The node to set</param>
		public void Set(int Index, INode Node)
		{
			if (Node == null)
			{
				return;
			}
			if (Node.Model != _Model)
			{
				throw new InvalidOperationException("The node belongs to another model!");
			}
			if (_Model.HasBones)
			{
				if (Index < 0)
				{
					return;
				}
				if (Index < _Model.Bones.Count)
				{
					_Model.Bones.Set(Index, Node as CBone);
				}
				Index -= _Model.Bones.Count;
			}
			if (_Model.HasLights)
			{
				if (Index < 0)
				{
					return;
				}
				if (Index < _Model.Lights.Count)
				{
					_Model.Lights.Set(Index, Node as CLight);
				}
				Index -= _Model.Lights.Count;
			}
			if (_Model.HasHelpers)
			{
				if (Index < 0)
				{
					return;
				}
				if (Index < _Model.Helpers.Count)
				{
					_Model.Helpers.Set(Index, Node as CHelper);
				}
				Index -= _Model.Helpers.Count;
			}
			if (_Model.HasAttachments)
			{
				if (Index < 0)
				{
					return;
				}
				if (Index < _Model.Attachments.Count)
				{
					_Model.Attachments.Set(Index, Node as CAttachment);
				}
				Index -= _Model.Attachments.Count;
			}
			if (_Model.HasParticleEmitters)
			{
				if (Index < 0)
				{
					return;
				}
				if (Index < _Model.ParticleEmitters.Count)
				{
					_Model.ParticleEmitters.Set(Index, Node as CParticleEmitter);
				}
				Index -= _Model.ParticleEmitters.Count;
			}
			if (_Model.HasParticleEmitters2)
			{
				if (Index < 0)
				{
					return;
				}
				if (Index < _Model.ParticleEmitters2.Count)
				{
					_Model.ParticleEmitters2.Set(Index, Node as CParticleEmitter2);
				}
				Index -= _Model.ParticleEmitters2.Count;
			}
			if (_Model.HasRibbonEmitters)
			{
				if (Index < 0)
				{
					return;
				}
				if (Index < _Model.RibbonEmitters.Count)
				{
					_Model.RibbonEmitters.Set(Index, Node as CRibbonEmitter);
				}
				Index -= _Model.RibbonEmitters.Count;
			}
			if (_Model.HasEvents)
			{
				if (Index < 0)
				{
					return;
				}
				if (Index < _Model.Events.Count)
				{
					_Model.Events.Set(Index, Node as CEvent);
				}
				Index -= _Model.Events.Count;
			}
			if (_Model.HasCollisionShapes && Index >= 0 && Index < _Model.CollisionShapes.Count)
			{
				_Model.CollisionShapes.Set(Index, Node as CCollisionShape);
			}
		}

		/// <summary>
		/// Removes an existing node.
		/// </summary>
		/// <param name="Node">The node to remove</param>
		/// <returns>True on success, False on failure</returns>
		public bool Remove(INode Node)
		{
			if (Node == null)
			{
				return false;
			}
			if (Node is CBone)
			{
				return _Model.Bones.Remove(Node as CBone);
			}
			if (Node is CLight)
			{
				return _Model.Lights.Remove(Node as CLight);
			}
			if (Node is CHelper)
			{
				return _Model.Helpers.Remove(Node as CHelper);
			}
			if (Node is CAttachment)
			{
				return _Model.Attachments.Remove(Node as CAttachment);
			}
			if (Node is CParticleEmitter)
			{
				return _Model.ParticleEmitters.Remove(Node as CParticleEmitter);
			}
			if (Node is CParticleEmitter2)
			{
				return _Model.ParticleEmitters2.Remove(Node as CParticleEmitter2);
			}
			if (Node is CRibbonEmitter)
			{
				return _Model.RibbonEmitters.Remove(Node as CRibbonEmitter);
			}
			if (Node is CEvent)
			{
				return _Model.Events.Remove(Node as CEvent);
			}
			if (Node is CCollisionShape)
			{
				return _Model.CollisionShapes.Remove(Node as CCollisionShape);
			}
			return false;
		}

		/// <summary>
		/// Removes an existing node at a specific index.
		/// </summary>
		/// <param name="Index">The index to remove at</param>
		public void RemoveAt(int Index)
		{
			if (_Model.HasBones)
			{
				if (Index < 0)
				{
					return;
				}
				if (Index < _Model.Bones.Count)
				{
					_Model.Bones.RemoveAt(Index);
				}
				Index -= _Model.Bones.Count;
			}
			if (_Model.HasLights)
			{
				if (Index < 0)
				{
					return;
				}
				if (Index < _Model.Lights.Count)
				{
					_Model.Lights.RemoveAt(Index);
				}
				Index -= _Model.Lights.Count;
			}
			if (_Model.HasHelpers)
			{
				if (Index < 0)
				{
					return;
				}
				if (Index < _Model.Helpers.Count)
				{
					_Model.Helpers.RemoveAt(Index);
				}
				Index -= _Model.Helpers.Count;
			}
			if (_Model.HasAttachments)
			{
				if (Index < 0)
				{
					return;
				}
				if (Index < _Model.Attachments.Count)
				{
					_Model.Attachments.RemoveAt(Index);
				}
				Index -= _Model.Attachments.Count;
			}
			if (_Model.HasParticleEmitters)
			{
				if (Index < 0)
				{
					return;
				}
				if (Index < _Model.ParticleEmitters.Count)
				{
					_Model.ParticleEmitters.RemoveAt(Index);
				}
				Index -= _Model.ParticleEmitters.Count;
			}
			if (_Model.HasParticleEmitters2)
			{
				if (Index < 0)
				{
					return;
				}
				if (Index < _Model.ParticleEmitters2.Count)
				{
					_Model.ParticleEmitters2.RemoveAt(Index);
				}
				Index -= _Model.ParticleEmitters2.Count;
			}
			if (_Model.HasRibbonEmitters)
			{
				if (Index < 0)
				{
					return;
				}
				if (Index < _Model.RibbonEmitters.Count)
				{
					_Model.RibbonEmitters.RemoveAt(Index);
				}
				Index -= _Model.RibbonEmitters.Count;
			}
			if (_Model.HasEvents)
			{
				if (Index < 0)
				{
					return;
				}
				if (Index < _Model.Events.Count)
				{
					_Model.Events.RemoveAt(Index);
				}
				Index -= _Model.Events.Count;
			}
			if (_Model.HasCollisionShapes && Index >= 0 && Index < _Model.CollisionShapes.Count)
			{
				_Model.CollisionShapes.RemoveAt(Index);
			}
		}

		/// <summary>
		/// Retrieves the node at a specific index.
		/// </summary>
		/// <param name="Index">The index to retrieve at</param>
		/// <returns>The retrieved node, null on failure</returns>
		public INode Get(int Index)
		{
			if (_Model.HasBones)
			{
				if (Index < 0)
				{
					return null;
				}
				if (Index < _Model.Bones.Count)
				{
					return _Model.Bones.Get(Index);
				}
				Index -= _Model.Bones.Count;
			}
			if (_Model.HasLights)
			{
				if (Index < 0)
				{
					return null;
				}
				if (Index < _Model.Lights.Count)
				{
					return _Model.Lights.Get(Index);
				}
				Index -= _Model.Lights.Count;
			}
			if (_Model.HasHelpers)
			{
				if (Index < 0)
				{
					return null;
				}
				if (Index < _Model.Helpers.Count)
				{
					return _Model.Helpers.Get(Index);
				}
				Index -= _Model.Helpers.Count;
			}
			if (_Model.HasAttachments)
			{
				if (Index < 0)
				{
					return null;
				}
				if (Index < _Model.Attachments.Count)
				{
					return _Model.Attachments.Get(Index);
				}
				Index -= _Model.Attachments.Count;
			}
			if (_Model.HasParticleEmitters)
			{
				if (Index < 0)
				{
					return null;
				}
				if (Index < _Model.ParticleEmitters.Count)
				{
					return _Model.ParticleEmitters.Get(Index);
				}
				Index -= _Model.ParticleEmitters.Count;
			}
			if (_Model.HasParticleEmitters2)
			{
				if (Index < 0)
				{
					return null;
				}
				if (Index < _Model.ParticleEmitters2.Count)
				{
					return _Model.ParticleEmitters2.Get(Index);
				}
				Index -= _Model.ParticleEmitters2.Count;
			}
			if (_Model.HasRibbonEmitters)
			{
				if (Index < 0)
				{
					return null;
				}
				if (Index < _Model.RibbonEmitters.Count)
				{
					return _Model.RibbonEmitters.Get(Index);
				}
				Index -= _Model.RibbonEmitters.Count;
			}
			if (_Model.HasEvents)
			{
				if (Index < 0)
				{
					return null;
				}
				if (Index < _Model.Events.Count)
				{
					return _Model.Events.Get(Index);
				}
				Index -= _Model.Events.Count;
			}
			if (_Model.HasCollisionShapes)
			{
				if (Index < 0)
				{
					return null;
				}
				if (Index < _Model.CollisionShapes.Count)
				{
					return _Model.CollisionShapes.Get(Index);
				}
			}
			return null;
		}

		/// <summary>
		/// Retrieves the index of an existing node.
		/// </summary>
		/// <param name="Node">The node whose index to retrieve</param>
		/// <returns>The index of the node, InvalidIndex on failure</returns>
		public int IndexOf(INode Node)
		{
			if (Node == null || Node.Model != _Model)
			{
				return -1;
			}
			return Node.NodeId;
		}

		/// <summary>
		/// Checks if a node exists in the container.
		/// </summary>
		/// <param name="Node">The node to check for</param>
		/// <returns>True if it exists, False otherwise</returns>
		public bool Contains(INode Node)
		{
			if (Node == null)
			{
				return false;
			}
			if (Node is CBone)
			{
				return _Model.Bones.Contains(Node as CBone);
			}
			if (Node is CLight)
			{
				return _Model.Lights.Contains(Node as CLight);
			}
			if (Node is CHelper)
			{
				return _Model.Helpers.Contains(Node as CHelper);
			}
			if (Node is CAttachment)
			{
				return _Model.Attachments.Contains(Node as CAttachment);
			}
			if (Node is CParticleEmitter)
			{
				return _Model.ParticleEmitters.Contains(Node as CParticleEmitter);
			}
			if (Node is CParticleEmitter2)
			{
				return _Model.ParticleEmitters2.Contains(Node as CParticleEmitter2);
			}
			if (Node is CRibbonEmitter)
			{
				return _Model.RibbonEmitters.Contains(Node as CRibbonEmitter);
			}
			if (Node is CEvent)
			{
				return _Model.Events.Contains(Node as CEvent);
			}
			if (Node is CCollisionShape)
			{
				return _Model.CollisionShapes.Contains(Node as CCollisionShape);
			}
			return false;
		}

		/// <summary>
		/// Checks if an index exists in the container.
		/// </summary>
		/// <param name="Index">The index to check for</param>
		/// <returns>True if it exists, False otherwise</returns>
		public bool ContainsIndex(int Index)
		{
			if (Index >= 0)
			{
				return Index < Count;
			}
			return false;
		}

		/// <summary>
		/// Copies the contents of the container to an array.
		/// </summary>
		/// <param name="Array">The array to copy to</param>
		/// <param name="Index">The index in the array to start copying to</param>
		public void CopyTo(INode[] Array, int Index)
		{
			int num = 0;
			if (_Model.HasBones)
			{
				foreach (CBone bone in _Model.Bones)
				{
					CBone cBone = (CBone)(Array[Index + num] = bone);
					num++;
				}
			}
			if (_Model.HasLights)
			{
				foreach (CLight light in _Model.Lights)
				{
					CLight cLight = (CLight)(Array[Index + num] = light);
					num++;
				}
			}
			if (_Model.HasHelpers)
			{
				foreach (CHelper helper in _Model.Helpers)
				{
					CHelper cHelper = (CHelper)(Array[Index + num] = helper);
					num++;
				}
			}
			if (_Model.HasAttachments)
			{
				foreach (CAttachment attachment in _Model.Attachments)
				{
					CAttachment cAttachment = (CAttachment)(Array[Index + num] = attachment);
					num++;
				}
			}
			if (_Model.HasParticleEmitters)
			{
				foreach (CParticleEmitter particleEmitter in _Model.ParticleEmitters)
				{
					CParticleEmitter cParticleEmitter = (CParticleEmitter)(Array[Index + num] = particleEmitter);
					num++;
				}
			}
			if (_Model.HasParticleEmitters2)
			{
				foreach (CParticleEmitter2 item in _Model.ParticleEmitters2)
				{
					CParticleEmitter2 cParticleEmitter2 = (CParticleEmitter2)(Array[Index + num] = item);
					num++;
				}
			}
			if (_Model.HasRibbonEmitters)
			{
				foreach (CRibbonEmitter ribbonEmitter in _Model.RibbonEmitters)
				{
					CRibbonEmitter cRibbonEmitter = (CRibbonEmitter)(Array[Index + num] = ribbonEmitter);
					num++;
				}
			}
			if (_Model.HasEvents)
			{
				foreach (CEvent @event in _Model.Events)
				{
					CEvent cEvent = (CEvent)(Array[Index + num] = @event);
					num++;
				}
			}
			if (!_Model.HasCollisionShapes)
			{
				return;
			}
			foreach (CCollisionShape collisionShape in _Model.CollisionShapes)
			{
				CCollisionShape cCollisionShape = (CCollisionShape)(Array[Index + num] = collisionShape);
				num++;
			}
		}

		/// <summary>
		/// Retrieves an enumerator for the nodes in the container.
		/// </summary>
		/// <returns>The retrieved enumerator</returns>
		public IEnumerator<INode> GetEnumerator()
		{
			if (_Model.HasBones)
			{
				foreach (CBone bone in _Model.Bones)
				{
					yield return bone;
				}
			}
			if (_Model.HasLights)
			{
				foreach (CLight light in _Model.Lights)
				{
					yield return light;
				}
			}
			if (_Model.HasHelpers)
			{
				foreach (CHelper helper in _Model.Helpers)
				{
					yield return helper;
				}
			}
			if (_Model.HasAttachments)
			{
				foreach (CAttachment attachment in _Model.Attachments)
				{
					yield return attachment;
				}
			}
			if (_Model.HasParticleEmitters)
			{
				foreach (CParticleEmitter particleEmitter in _Model.ParticleEmitters)
				{
					yield return particleEmitter;
				}
			}
			if (_Model.HasParticleEmitters2)
			{
				foreach (CParticleEmitter2 item in _Model.ParticleEmitters2)
				{
					yield return item;
				}
			}
			if (_Model.HasRibbonEmitters)
			{
				foreach (CRibbonEmitter ribbonEmitter in _Model.RibbonEmitters)
				{
					yield return ribbonEmitter;
				}
			}
			if (_Model.HasEvents)
			{
				foreach (CEvent @event in _Model.Events)
				{
					yield return @event;
				}
			}
			if (!_Model.HasCollisionShapes)
			{
				yield break;
			}
			foreach (CCollisionShape collisionShape in _Model.CollisionShapes)
			{
				yield return collisionShape;
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
