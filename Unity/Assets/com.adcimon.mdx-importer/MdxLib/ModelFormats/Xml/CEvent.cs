using System.Xml;
using MdxLib.Model;

namespace MdxLib.ModelFormats.Xml
{
	internal sealed class CEvent : CNode
	{
		private static class CSingleton
		{
			public static CEvent Instance;

			static CSingleton()
			{
				Instance = new CEvent();
			}
		}

		public static CEvent Instance => CSingleton.Instance;

		private CEvent()
		{
		}

		public void Load(CLoader Loader, XmlNode Node, MdxLib.Model.CModel Model, MdxLib.Model.CEvent Event)
		{
			LoadNode(Loader, Node, Model, Event);
			Loader.Attacher.AddObject(Model.GlobalSequences, Event.GlobalSequence, ReadInteger(Node, "global_sequence", -1));
			foreach (XmlNode item in Node.SelectNodes("event_track"))
			{
				MdxLib.Model.CEventTrack cEventTrack = new MdxLib.Model.CEventTrack(Model);
				CEventTrack.Instance.Load(Loader, item, Model, Event, cEventTrack);
				Event.Tracks.Add(cEventTrack);
			}
		}

		public void Save(CSaver Saver, XmlNode Node, MdxLib.Model.CModel Model, MdxLib.Model.CEvent Event)
		{
			SaveNode(Saver, Node, Model, Event);
			WriteInteger(Node, "global_sequence", Event.GlobalSequence.ObjectId);
			if (!Event.HasTracks)
			{
				return;
			}
			foreach (MdxLib.Model.CEventTrack track in Event.Tracks)
			{
				XmlElement node = AppendElement(Node, "event_track");
				CEventTrack.Instance.Save(Saver, node, Model, Event, track);
			}
		}
	}
}
