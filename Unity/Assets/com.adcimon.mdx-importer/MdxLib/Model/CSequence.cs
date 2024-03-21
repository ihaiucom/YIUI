using MdxLib.Primitives;

namespace MdxLib.Model
{
	/// <summary>
	/// A sequence class. Represents an animation like Stand/Walk.
	/// </summary>
	public sealed class CSequence : CObject<CSequence>
	{
		private string _Name = "";

		private int _IntervalStart;

		private int _IntervalEnd;

		private int _SyncPoint;

		private float _Rarity;

		private float _MoveSpeed;

		private bool _NonLooping;

		private CExtent _Extent = CConstants.DefaultExtent;

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		public string Name
		{
			get
			{
				return _Name;
			}
			set
			{
				AddSetObjectFieldCommand("_Name", value);
				_Name = value;
			}
		}

		/// <summary>
		/// Gets or sets the interval start time.
		/// </summary>
		public int IntervalStart
		{
			get
			{
				return _IntervalStart;
			}
			set
			{
				AddSetObjectFieldCommand("_IntervalStart", value);
				_IntervalStart = value;
			}
		}

		/// <summary>
		/// Gets or sets the interval end time.
		/// </summary>
		public int IntervalEnd
		{
			get
			{
				return _IntervalEnd;
			}
			set
			{
				AddSetObjectFieldCommand("_IntervalEnd", value);
				_IntervalEnd = value;
			}
		}

		/// <summary>
		/// Gets or sets the sync point.
		/// </summary>
		public int SyncPoint
		{
			get
			{
				return _SyncPoint;
			}
			set
			{
				AddSetObjectFieldCommand("_SyncPoint", value);
				_SyncPoint = value;
			}
		}

		/// <summary>
		/// Gets or sets the rarity.
		/// </summary>
		public float Rarity
		{
			get
			{
				return _Rarity;
			}
			set
			{
				AddSetObjectFieldCommand("_Rarity", value);
				_Rarity = value;
			}
		}

		/// <summary>
		/// Gets or sets the move speed.
		/// </summary>
		public float MoveSpeed
		{
			get
			{
				return _MoveSpeed;
			}
			set
			{
				AddSetObjectFieldCommand("_MoveSpeed", value);
				_MoveSpeed = value;
			}
		}

		/// <summary>
		/// Gets or sets the non looping flag.
		/// </summary>
		public bool NonLooping
		{
			get
			{
				return _NonLooping;
			}
			set
			{
				AddSetObjectFieldCommand("_NonLooping", value);
				_NonLooping = value;
			}
		}

		/// <summary>
		/// Gets or sets the extent.
		/// </summary>
		public CExtent Extent
		{
			get
			{
				return _Extent;
			}
			set
			{
				AddSetObjectFieldCommand("_Extent", value);
				_Extent = value;
			}
		}

		/// <summary>
		/// Parameterized constructor.
		/// </summary>
		/// <param name="Model">The model to be associated with this sequence</param>
		public CSequence(CModel Model)
			: base(Model)
		{
		}

		/// <summary>
		/// Generates a string version of the sequence.
		/// </summary>
		/// <returns>The generated string</returns>
		public override string ToString()
		{
			return "Sequence #" + base.ObjectId;
		}
	}
}
