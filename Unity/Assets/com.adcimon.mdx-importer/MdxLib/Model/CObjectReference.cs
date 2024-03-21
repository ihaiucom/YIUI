using System;
using MdxLib.Command;

namespace MdxLib.Model
{
	/// <summary>
	/// Handles a reference to an object component. References are powerful links to other
	/// parts of the model which will not be invalid even if you add/remove stuff
	/// (like a common ID would).
	/// </summary>
	/// <typeparam name="T">The object type</typeparam>
	public sealed class CObjectReference<T> where T : CObject<T>
	{
		private CModel _Model;

		private T _Object = null;

		/// <summary>
		/// Retrieves the associated model.
		/// </summary>
		public CModel Model => _Model;

		/// <summary>
		/// Retrieves the attached object, or null if not attached.
		/// </summary>
		public T Object => _Object;

		/// <summary>
		/// Retrieves the object ID of the attached object, or InvalidId if not attached.
		/// </summary>
		public int ObjectId
		{
			get
			{
				if (_Object == null)
				{
					return -1;
				}
				return _Object.ObjectId;
			}
		}

		internal bool CanAddCommand => _Model.CommandGroup != null;

		internal T InternalObject
		{
			get
			{
				return _Object;
			}
			set
			{
				_Object = value;
			}
		}

		internal CObjectReference(CModel Model)
		{
			_Model = Model;
		}

		/// <summary>
		/// Attaches the reference to an object.
		/// </summary>
		/// <param name="Object">The object to attach to</param>
		public void Attach(T Object)
		{
			Detach();
			if (Object == null)
			{
				return;
			}
			if (Object.Model != _Model)
			{
				throw new InvalidOperationException("The object belongs to another model!");
			}
			CUnknown cUnknown = Object;
			if (cUnknown != null)
			{
				if (CanAddCommand)
				{
					ICommand command = new CAttachObject<T>(this, Object);
					command.Do();
					AddCommand(command);
				}
				else
				{
					cUnknown.ObjectReferenceSet.Add(this);
					_Object = Object;
				}
			}
		}

		/// <summary>
		/// Detachers the reference from the object (if attached).
		/// </summary>
		public void Detach()
		{
			if (_Object == null)
			{
				return;
			}
			CUnknown @object = _Object;
			if (@object != null)
			{
				if (CanAddCommand)
				{
					ICommand command = new CDetachObject<T>(this, _Object);
					command.Do();
					AddCommand(command);
				}
				else
				{
					@object.ObjectReferenceSet.Remove(this);
					_Object = null;
				}
			}
		}

		internal void ForceAttach(T Object)
		{
			ForceDetach();
			if (Object != null)
			{
				Object.ObjectReferenceSet.Add(this);
				_Object = Object;
			}
		}

		internal void ForceDetach()
		{
			CUnknown @object = _Object;
			if (@object != null)
			{
				@object.ObjectReferenceSet.Remove(this);
				_Object = null;
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
