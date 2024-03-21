using System.Xml;
using MdxLib.Model;

namespace MdxLib.ModelFormats.Xml
{
	internal sealed class CSequence : CObject
	{
		private static class CSingleton
		{
			public static CSequence Instance;

			static CSingleton()
			{
				Instance = new CSequence();
			}
		}

		public static CSequence Instance => CSingleton.Instance;

		private CSequence()
		{
		}

		public void Load(CLoader Loader, XmlNode Node, MdxLib.Model.CModel Model, MdxLib.Model.CSequence Sequence)
		{
			Sequence.Name = ReadString(Node, "name", Sequence.Name);
			Sequence.Rarity = ReadFloat(Node, "rarity", Sequence.Rarity);
			Sequence.MoveSpeed = ReadFloat(Node, "move_speed", Sequence.MoveSpeed);
			Sequence.IntervalStart = ReadInteger(Node, "interval_start", Sequence.IntervalStart);
			Sequence.IntervalEnd = ReadInteger(Node, "interval_end", Sequence.IntervalEnd);
			Sequence.SyncPoint = ReadInteger(Node, "sync_point", Sequence.SyncPoint);
			Sequence.NonLooping = ReadBoolean(Node, "non_looping", Sequence.NonLooping);
			Sequence.Extent = ReadExtent(Node, "extent", Sequence.Extent);
		}

		public void Save(CSaver Saver, XmlNode Node, MdxLib.Model.CModel Model, MdxLib.Model.CSequence Sequence)
		{
			WriteString(Node, "name", Sequence.Name);
			WriteFloat(Node, "rarity", Sequence.Rarity);
			WriteFloat(Node, "move_speed", Sequence.MoveSpeed);
			WriteInteger(Node, "interval_start", Sequence.IntervalStart);
			WriteInteger(Node, "interval_end", Sequence.IntervalEnd);
			WriteInteger(Node, "sync_point", Sequence.SyncPoint);
			WriteBoolean(Node, "non_looping", Sequence.NonLooping);
			WriteExtent(Node, "extent", Sequence.Extent);
		}
	}
}
