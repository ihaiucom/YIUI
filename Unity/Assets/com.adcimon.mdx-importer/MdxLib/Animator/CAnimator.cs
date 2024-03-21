using System.Collections;
using System.Collections.Generic;
using MdxLib.Command;
using MdxLib.Command.Animator;
using MdxLib.Model;

namespace MdxLib.Animator
{
	/// <summary>
	/// The main class for animated values.
	/// </summary>
	/// <typeparam name="T">The value type</typeparam>
	public sealed class CAnimator<T> : IList<CAnimatorNode<T>>, ICollection<CAnimatorNode<T>>, IEnumerable<CAnimatorNode<T>>, IEnumerable where T : new()
	{
		private CModel _Model;

		private CAnimatable<T> _Animatable;

		private CObjectReference<CGlobalSequence> _GlobalSequence;

		private bool _Animated;

		private EInterpolationType _Type;

		private T _StaticValue = default(T);

		private List<CAnimatorNode<T>> NodeList;

		/// <summary>
		/// Checks if the animator is static (the opposite of animated).
		/// </summary>
		public bool Static => !_Animated;

		/// <summary>
		/// Checks if the animator is animated (the opposite of static).
		/// </summary>
		public bool Animated => _Animated;

		/// <summary>
		/// Gets or sets the type.
		/// </summary>
		public EInterpolationType Type
		{
			get
			{
				return _Type;
			}
			set
			{
				AddSetAnimatorFieldCommand("_Type", value);
				_Type = value;
			}
		}

		/// <summary>
		/// Retrieves the number of nodes in the animator.
		/// </summary>
		public int Count => NodeList.Count;

		/// <summary>
		/// Checks if the animator is read-only (which it isn't).
		/// </summary>
		public bool IsReadOnly => false;

		/// <summary>
		/// Gets or sets a node in the animator.
		/// </summary>
		/// <param name="Index">The index to get or set at</param>
		/// <returns>The accessed node</returns>
		public CAnimatorNode<T> this[int Index]
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

		/// <summary>
		/// Retrieves the associated model.
		/// </summary>
		public CModel Model => _Model;

		/// <summary>
		/// Retrieves the global sequence reference.
		/// </summary>
		public CObjectReference<CGlobalSequence> GlobalSequence => _GlobalSequence ?? (_GlobalSequence = new CObjectReference<CGlobalSequence>(Model));

		internal bool CanAddCommand => _Model.CommandGroup != null;

		internal List<CAnimatorNode<T>> InternalNodeList
		{
			get
			{
				return NodeList;
			}
			set
			{
				NodeList = value;
			}
		}

		internal CAnimator(CModel Model, CAnimatable<T> Animatable)
		{
			_Model = Model;
			_Animatable = Animatable;
			_StaticValue = Animatable.DefaultValue;
			NodeList = new List<CAnimatorNode<T>>();
		}

		/// <summary>
		/// Retrieves the static value.
		/// </summary>
		/// <returns>The value</returns>
		public T GetValue()
		{
			return _StaticValue;
		}

		/// <summary>
		/// Retrieves the value of the animator. If it's animated it will be interpolated,
		/// otherwise it will be the static value.
		/// </summary>
		/// <param name="Time">The time to interpolate at</param>
		/// <returns>The value</returns>
		public T GetValue(CTime Time)
		{
			if (!_Animated)
			{
				return _StaticValue;
			}
			return _Animatable.Interpolate(_Type, Time, GetLowerNodeAtTime(Time), GetUpperNodeAtTime(Time));
		}

		/// <summary>
		/// Retrieves the node to the left of a specific point in time.
		/// </summary>
		/// <param name="Time">The time</param>
		/// <returns>The left node, or null if none exists</returns>
		public CAnimatorNode<T> GetLowerNodeAtTime(CTime Time)
		{
			for (int num = NodeList.Count - 1; num >= 0; num--)
			{
				CAnimatorNode<T> cAnimatorNode = NodeList[num];
				if (cAnimatorNode.Time < Time.IntervalStart)
				{
					return null;
				}
				if (cAnimatorNode.Time <= Time.Time)
				{
					return cAnimatorNode;
				}
			}
			return null;
		}

		/// <summary>
		/// Retrieves the node to the right of a specific point in time.
		/// </summary>
		/// <param name="Time">The time</param>
		/// <returns>The right node, or null if none exists</returns>
		public CAnimatorNode<T> GetUpperNodeAtTime(CTime Time)
		{
			for (int i = 0; i < NodeList.Count; i++)
			{
				CAnimatorNode<T> cAnimatorNode = NodeList[i];
				if (cAnimatorNode.Time > Time.IntervalEnd)
				{
					return null;
				}
				if (cAnimatorNode.Time >= Time.Time)
				{
					return cAnimatorNode;
				}
			}
			return null;
		}

		/// <summary>
		/// Makes the animator static.
		/// </summary>
		/// <param name="StaticValue">The new static value to use</param>
		public void MakeStatic(T StaticValue)
		{
			AddSetAnimatorFieldCommand("_StaticValue", StaticValue);
			_StaticValue = StaticValue;
			AddSetAnimatorFieldCommand("_Animated", Value: false);
			_Animated = false;
		}

		/// <summary>
		/// Makes the animator animated.
		/// </summary>
		public void MakeAnimated()
		{
			AddSetAnimatorFieldCommand("_Animated", Value: true);
			_Animated = true;
		}

		/// <summary>
		/// Clears all nodes.
		/// </summary>
		public void Clear()
		{
			if (NodeList.Count > 0)
			{
				if (CanAddCommand)
				{
					ICommand command = new CClear<T>(this);
					command.Do();
					AddCommand(command);
				}
				else
				{
					NodeList.Clear();
				}
			}
		}

		/// <summary>
		/// Adds a new node.
		/// </summary>
		/// <param name="Node">The node to add</param>
		public void Add(CAnimatorNode<T> Node)
		{
			int insertIndex = GetInsertIndex(Node);
			if (CanAddCommand)
			{
				ICommand command = new CInsert<T>(this, insertIndex, Node);
				command.Do();
				AddCommand(command);
			}
			else
			{
				NodeList.Insert(insertIndex, Node);
			}
		}

		/// <summary>
		/// Inserts a new node at a specific index. Since the nodes are sorted
		/// it doesn't actually insert at the specified index.
		/// </summary>
		/// <param name="Index">The index to insert at</param>
		/// <param name="Node">The node to insert</param>
		public void Insert(int Index, CAnimatorNode<T> Node)
		{
			int insertIndex = GetInsertIndex(Node);
			if (CanAddCommand)
			{
				ICommand command = new CInsert<T>(this, insertIndex, Node);
				command.Do();
				AddCommand(command);
			}
			else
			{
				NodeList.Insert(insertIndex, Node);
			}
		}

		/// <summary>
		/// Sets a new node at a specific index (removing whatever is there).
		/// </summary>
		/// <param name="Index">The index to set at</param>
		/// <param name="Node">The node to set</param>
		public void Set(int Index, CAnimatorNode<T> Node)
		{
			if (ContainsIndex(Index))
			{
				int time = NodeList[Index].Time;
				CAnimatorNode<T> cAnimatorNode = new CAnimatorNode<T>(time, Node.Value, Node.InTangent, Node.OutTangent);
				if (CanAddCommand)
				{
					ICommand command = new CSet<T>(this, Index, cAnimatorNode);
					command.Do();
					AddCommand(command);
				}
				else
				{
					NodeList[Index] = cAnimatorNode;
				}
			}
		}

		/// <summary>
		/// Removes an existing node.
		/// </summary>
		/// <param name="Node">The node to remove</param>
		/// <returns>True on success, False on failure</returns>
		public bool Remove(CAnimatorNode<T> Node)
		{
			int index = NodeList.IndexOf(Node);
			if (!ContainsIndex(index))
			{
				return false;
			}
			if (CanAddCommand)
			{
				ICommand command = new CRemoveAt<T>(this, index);
				command.Do();
				AddCommand(command);
			}
			else
			{
				NodeList.RemoveAt(index);
			}
			return true;
		}

		/// <summary>
		/// Removes an existing node at a specific index.
		/// </summary>
		/// <param name="Index">The index to remove at</param>
		public void RemoveAt(int Index)
		{
			if (ContainsIndex(Index))
			{
				if (CanAddCommand)
				{
					ICommand command = new CRemoveAt<T>(this, Index);
					command.Do();
					AddCommand(command);
				}
				else
				{
					NodeList.RemoveAt(Index);
				}
			}
		}

		/// <summary>
		/// Retrieves the node at a specific index.
		/// </summary>
		/// <param name="Index">The index to retrieve at</param>
		/// <returns>The retrieved node, null on failure</returns>
		public CAnimatorNode<T> Get(int Index)
		{
			if (!ContainsIndex(Index))
			{
				return null;
			}
			return NodeList[Index];
		}

		/// <summary>
		/// Retrieves the index of an existing node.
		/// </summary>
		/// <param name="Node">The node whose index to retrieve</param>
		/// <returns>The index of the node, InvalidIndex on failure</returns>
		public int IndexOf(CAnimatorNode<T> Node)
		{
			return NodeList.IndexOf(Node);
		}

		/// <summary>
		/// Checks if a node exists in the animator.
		/// </summary>
		/// <param name="Node">The node to check for</param>
		/// <returns>True if it exists, False otherwise</returns>
		public bool Contains(CAnimatorNode<T> Node)
		{
			return NodeList.Contains(Node);
		}

		/// <summary>
		/// Checks if an index exists in the animator.
		/// </summary>
		/// <param name="Index">The index to check for</param>
		/// <returns>True if it exists, False otherwise</returns>
		public bool ContainsIndex(int Index)
		{
			if (Index >= 0)
			{
				return Index < NodeList.Count;
			}
			return false;
		}

		/// <summary>
		/// Copies the contents of the animator to an array.
		/// </summary>
		/// <param name="Array">The array to copy to</param>
		/// <param name="Index">The index in the array to start copying to</param>
		public void CopyTo(CAnimatorNode<T>[] Array, int Index)
		{
			NodeList.CopyTo(Array, Index);
		}

		/// <summary>
		/// Retrieves an enumerator for the nodes in the animator.
		/// </summary>
		/// <returns>The retrieved enumerator</returns>
		public IEnumerator<CAnimatorNode<T>> GetEnumerator()
		{
			return NodeList.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return NodeList.GetEnumerator();
		}

		private int GetInsertIndex(CAnimatorNode<T> Node)
		{
			if (NodeList.Count > 0 && Node.Time > NodeList[NodeList.Count - 1].Time)
			{
				return NodeList.Count;
			}
			for (int i = 0; i < NodeList.Count; i++)
			{
				if (Node.Time < NodeList[i].Time)
				{
					return i;
				}
			}
			return NodeList.Count;
		}

		internal void AddCommand(ICommand Command)
		{
			if (_Model.CommandGroup != null)
			{
				_Model.CommandGroup.Add(Command);
			}
		}

		internal void AddSetAnimatorFieldCommand<T2>(string FieldName, T2 Value)
		{
			if (_Model.CommandGroup != null)
			{
				_Model.CommandGroup.Add(new CSetAnimatorField<T, T2>(this, FieldName, Value));
			}
		}
	}
}
