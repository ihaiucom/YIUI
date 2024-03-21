using System;
using System.Collections;
using System.Collections.Generic;
using MdxLib.Command;
using MdxLib.Command.ObjectContainer;

namespace MdxLib.Model
{
	/// <summary>
	/// Stores objects which can be added, removed and enumerated.
	/// </summary>
	/// <typeparam name="T">The object type</typeparam>
	public sealed class CObjectContainer<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable where T : CObject<T>
	{
		private CModel _Model;

		private List<T> ObjectList;

		/// <summary>
		/// Retrieves the associated model.
		/// </summary>
		public CModel Model => _Model;

		/// <summary>
		/// Checks if the container has any objects with references pointing to them.
		/// </summary>
		public bool HasReferences
		{
			get
			{
				foreach (T @object in ObjectList)
				{
					T current = @object;
					if (current.HasReferences)
					{
						return true;
					}
				}
				return false;
			}
		}

		/// <summary>
		/// Retrieves the number of objects in the container.
		/// </summary>
		public int Count => ObjectList.Count;

		/// <summary>
		/// Checks if the container is read-only (which it isn't).
		/// </summary>
		public bool IsReadOnly => false;

		/// <summary>
		/// Gets or set an object in the container.
		/// </summary>
		/// <param name="Index">The index to get or set at</param>
		/// <returns>The accessed object</returns>
		public T this[int Index]
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

		internal bool CanAddCommand => _Model.CommandGroup != null;

		internal List<T> InternalObjectList
		{
			get
			{
				return ObjectList;
			}
			set
			{
				ObjectList = value;
			}
		}

		internal CObjectContainer(CModel Model)
		{
			_Model = Model;
			ObjectList = new List<T>();
		}

		/// <summary>
		/// Clears all objects.
		/// </summary>
		public void Clear()
		{
			if (ObjectList.Count <= 0)
			{
				return;
			}
			LinkedList<CDetacher> detacherList = new LinkedList<CDetacher>();
			BuildDetacherList(detacherList);
			if (CanAddCommand)
			{
				ICommand command = new CClear<T>(this, detacherList);
				command.Do();
				AddCommand(command);
				return;
			}
			CDetacher.DetachAllDetachers(detacherList);
			foreach (T @object in ObjectList)
			{
				T current = @object;
				current.ObjectContainer = null;
			}
			ObjectList.Clear();
		}

		/// <summary>
		/// Adds a new object.
		/// </summary>
		/// <param name="Object">The object to add</param>
		public void Add(T Object)
		{
			if (Object != null)
			{
				if (Object.Model != _Model)
				{
					throw new InvalidOperationException("The object belongs to another model!");
				}
				if (Object.ObjectContainer != null)
				{
					throw new InvalidOperationException("The object is already in a container!");
				}
				if (CanAddCommand)
				{
					ICommand command = new CAdd<T>(this, Object);
					command.Do();
					AddCommand(command);
				}
				else
				{
					ObjectList.Add(Object);
					Object.ObjectContainer = this;
				}
			}
		}

		/// <summary>
		/// Inserts a new object at a specific index.
		/// </summary>
		/// <param name="Index">The index to insert at</param>
		/// <param name="Object">The object to insert</param>
		public void Insert(int Index, T Object)
		{
			if (Object == null)
			{
				return;
			}
			if (Object.Model != _Model)
			{
				throw new InvalidOperationException("The object belongs to another model!");
			}
			if (Object.ObjectContainer != null)
			{
				throw new InvalidOperationException("The object is already in a container!");
			}
			if (Index >= 0 && Index <= ObjectList.Count)
			{
				if (CanAddCommand)
				{
					ICommand command = new CInsert<T>(this, Index, Object);
					command.Do();
					AddCommand(command);
				}
				else
				{
					ObjectList.Insert(Index, Object);
					Object.ObjectContainer = this;
				}
			}
		}

		/// <summary>
		/// Sets a new object at a specific index (removing whatever is there).
		/// </summary>
		/// <param name="Index">The index to set at</param>
		/// <param name="Object">The object to set</param>
		public void Set(int Index, T Object)
		{
			if (Object == null)
			{
				return;
			}
			if (Object.Model != _Model)
			{
				throw new InvalidOperationException("The object belongs to another model!");
			}
			if (Object.ObjectContainer != null)
			{
				throw new InvalidOperationException("The object is already in a container!");
			}
			if (ContainsIndex(Index))
			{
				T val = ObjectList[Index];
				LinkedList<CDetacher> detacherList = new LinkedList<CDetacher>();
				val.BuildDetacherList(detacherList);
				if (CanAddCommand)
				{
					ICommand command = new CSet<T>(this, Index, Object, detacherList);
					command.Do();
					AddCommand(command);
				}
				else
				{
					CDetacher.DetachAllDetachers(detacherList);
					val.ObjectContainer = null;
					ObjectList[Index] = Object;
					Object.ObjectContainer = this;
				}
			}
		}

		/// <summary>
		/// Removes an existing object.
		/// </summary>
		/// <param name="Object">The object to remove</param>
		/// <returns>True on success, False on failure</returns>
		public bool Remove(T Object)
		{
			if (Object == null)
			{
				return false;
			}
			int num = IndexOf(Object);
			if (num == -1)
			{
				return false;
			}
			LinkedList<CDetacher> detacherList = new LinkedList<CDetacher>();
			Object.BuildDetacherList(detacherList);
			if (CanAddCommand)
			{
				ICommand command = new CRemoveAt<T>(this, num, detacherList);
				command.Do();
				AddCommand(command);
			}
			else
			{
				CDetacher.DetachAllDetachers(detacherList);
				Object.ObjectContainer = null;
				ObjectList.RemoveAt(num);
			}
			return true;
		}

		/// <summary>
		/// Removes an existing object at a specific index.
		/// </summary>
		/// <param name="Index">The index to remove at</param>
		public void RemoveAt(int Index)
		{
			T val = Get(Index);
			if (val != null)
			{
				LinkedList<CDetacher> detacherList = new LinkedList<CDetacher>();
				val.BuildDetacherList(detacherList);
				if (CanAddCommand)
				{
					ICommand command = new CRemoveAt<T>(this, Index, detacherList);
					command.Do();
					AddCommand(command);
				}
				else
				{
					CDetacher.DetachAllDetachers(detacherList);
					val.ObjectContainer = null;
					ObjectList.RemoveAt(Index);
				}
			}
		}

		/// <summary>
		/// Retrieves the object at a specific index.
		/// </summary>
		/// <param name="Index">The index to retrieve at</param>
		/// <returns>The retrieved object, null on failure</returns>
		public T Get(int Index)
		{
			if (!ContainsIndex(Index))
			{
				return null;
			}
			return ObjectList[Index];
		}

		/// <summary>
		/// Retrieves the index of an existing object.
		/// </summary>
		/// <param name="Object">The object whose index to retrieve</param>
		/// <returns>The index of the object, InvalidIndex on failure</returns>
		public int IndexOf(T Object)
		{
			return ObjectList.IndexOf(Object);
		}

		/// <summary>
		/// Checks if an object exists in the container.
		/// </summary>
		/// <param name="Object">The object to check for</param>
		/// <returns>True if it exists, False otherwise</returns>
		public bool Contains(T Object)
		{
			if (Object == null)
			{
				return false;
			}
			return ObjectList.Contains(Object);
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
				return Index < ObjectList.Count;
			}
			return false;
		}

		/// <summary>
		/// Copies the contents of the container to an array.
		/// </summary>
		/// <param name="Array">The array to copy to</param>
		/// <param name="Index">The index in the array to start copying to</param>
		public void CopyTo(T[] Array, int Index)
		{
			ObjectList.CopyTo(Array, Index);
		}

		/// <summary>
		/// Retrieves an enumerator for the objects in the container.
		/// </summary>
		/// <returns>The retrieved enumerator</returns>
		public IEnumerator<T> GetEnumerator()
		{
			return ObjectList.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ObjectList.GetEnumerator();
		}

		internal void BuildDetacherList(ICollection<CDetacher> DetacherList)
		{
			foreach (T @object in ObjectList)
			{
				T current = @object;
				current.BuildDetacherList(DetacherList);
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
