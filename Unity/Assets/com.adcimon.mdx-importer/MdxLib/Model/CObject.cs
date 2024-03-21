using System.Collections.Generic;
using MdxLib.Command;

namespace MdxLib.Model
{
	/// <summary>
	/// The base class for all object components. This class is templated so
	/// use IObject if you want non-specified access.
	/// </summary>
	/// <typeparam name="T">The object type (class that inherits this class)</typeparam>
	public abstract class CObject<T> : CUnknown, IObject, IUnknown where T : CObject<T>
	{
		private CModel _Model;

		private CObjectContainer<T> _ObjectContainer;

		/// <summary>
		/// Retrieves the object ID (if added to a container).
		/// </summary>
		public int ObjectId
		{
			get
			{
				if (_ObjectContainer == null)
				{
					return -1;
				}
				return _ObjectContainer.IndexOf((T)this);
			}
		}

		/// <summary>
		/// Retrieves the associated model.
		/// </summary>
		public CModel Model => _Model;

		/// <summary>
		/// Checks if the object has references pointing to it.
		/// </summary>
		public virtual bool HasReferences => base.ObjectReferenceSet.Count > 0;

		internal CObjectContainer<T> ObjectContainer
		{
			get
			{
				return _ObjectContainer;
			}
			set
			{
				_ObjectContainer = value;
			}
		}

		/// <summary>
		/// Parameterized constructor.
		/// </summary>
		/// <param name="Model">The model to be associated with this object</param>
		public CObject(CModel Model)
		{
			_Model = Model;
		}

		internal override void BuildObjectDetacherList(ICollection<CDetacher> DetacherList)
		{
			foreach (object item in base.ObjectReferenceSet)
			{
				CObjectReference<T> cObjectReference = item as CObjectReference<T>;
				if (cObjectReference != null)
				{
					DetacherList.Add(new CObjectDetacher<T>(cObjectReference));
				}
			}
		}

		internal void AddSetObjectFieldCommand<T2>(string FieldName, T2 Value)
		{
			if (_Model.CommandGroup != null)
			{
				_Model.CommandGroup.Add(new CSetObjectField<T, T2>((T)this, FieldName, Value));
			}
		}
	}
}
