namespace MdxLib.Model
{
	/// <summary>
	/// An event track class. Defines each point in time at which
	/// an event should perform its actions.
	/// </summary>
	public sealed class CEventTrack : CObject<CEventTrack>
	{
		private int _Time;

		/// <summary>
		/// Gets or sets the time. This is the point in time when the event fires.
		/// </summary>
		public int Time
		{
			get
			{
				return _Time;
			}
			set
			{
				AddSetObjectFieldCommand("_Time", value);
				_Time = value;
			}
		}

		/// <summary>
		/// Parameterized constructor.
		/// </summary>
		/// <param name="Model">The model to be associated with this event track</param>
		public CEventTrack(CModel Model)
			: base(Model)
		{
		}

		/// <summary>
		/// Generates a string version of the event track.
		/// </summary>
		/// <returns>The generated string</returns>
		public override string ToString()
		{
			return "Event Track #" + base.ObjectId;
		}
	}
}
