using System.Collections.Generic;

namespace MdxLib.Model
{
	/// <summary>
	/// An event class. Performs certain actions during an animation.
	/// </summary>
	public sealed class CEvent : CNode<CEvent>
	{
		private CObjectReference<CGlobalSequence> _GlobalSequence;

		private CObjectContainer<CEventTrack> _Tracks;

		/// <summary>
		/// Retrieves the node ID (if added to a container).
		/// </summary>
		public override int NodeId => base.Model.GetEventNodeId(this);

		/// <summary>
		/// Checks if the event has references pointing to it.
		/// </summary>
		public override bool HasReferences
		{
			get
			{
				if (_Tracks != null && _Tracks.HasReferences)
				{
					return true;
				}
				return base.HasReferences;
			}
		}

		/// <summary>
		/// Retrieves the global sequence reference.
		/// </summary>
		public CObjectReference<CGlobalSequence> GlobalSequence => _GlobalSequence ?? (_GlobalSequence = new CObjectReference<CGlobalSequence>(base.Model));

		/// <summary>
		/// Checks if there exists some event tracks.
		/// </summary>
		public bool HasTracks
		{
			get
			{
				if (_Tracks == null)
				{
					return false;
				}
				return _Tracks.Count > 0;
			}
		}

		/// <summary>
		/// Retrieves the event tracks container.
		/// </summary>
		public CObjectContainer<CEventTrack> Tracks => _Tracks ?? (_Tracks = new CObjectContainer<CEventTrack>(base.Model));

		/// <summary>
		/// Parameterized constructor.
		/// </summary>
		/// <param name="Model">The model to be associated with this event</param>
		public CEvent(CModel Model)
			: base(Model)
		{
		}

		internal override void BuildDetacherList(ICollection<CDetacher> DetacherList)
		{
			base.BuildDetacherList(DetacherList);
			if (_Tracks != null)
			{
				_Tracks.BuildDetacherList(DetacherList);
			}
		}

		/// <summary>
		/// Generates a string version of the event.
		/// </summary>
		/// <returns>The generated string</returns>
		public override string ToString()
		{
			return "Event #" + base.ObjectId;
		}
	}
}
