using System.Collections.Generic;

namespace MdxLib.Model
{
	/// <summary>
	/// The base class for all objects. Exists for internal purposes only.
	/// </summary>
	public abstract class CUnknown : IUnknown
	{
		private object _Tag;

		private HashSet<object> _ObjectReferenceSet;

		private HashSet<object> _NodeReferenceSet;

		/// <summary>
		/// Gets or sets the tag data of the object. Tag data is not saved when the model is.
		/// </summary>
		public object Tag
		{
			get
			{
				return _Tag;
			}
			set
			{
				_Tag = value;
			}
		}

		internal HashSet<object> ObjectReferenceSet => _ObjectReferenceSet ?? (_ObjectReferenceSet = new HashSet<object>());

		internal HashSet<object> NodeReferenceSet => _NodeReferenceSet ?? (_NodeReferenceSet = new HashSet<object>());

		/// <summary>
		/// Default constructor.
		/// </summary>
		public CUnknown()
		{
		}

		internal virtual void BuildDetacherList(ICollection<CDetacher> DetacherList)
		{
			if (_ObjectReferenceSet != null)
			{
				BuildObjectDetacherList(DetacherList);
			}
			if (_NodeReferenceSet != null)
			{
				BuildNodeDetacherList(DetacherList);
			}
		}

		internal virtual void BuildObjectDetacherList(ICollection<CDetacher> DetacherList)
		{
		}

		internal virtual void BuildNodeDetacherList(ICollection<CDetacher> DetacherList)
		{
		}
	}
}
