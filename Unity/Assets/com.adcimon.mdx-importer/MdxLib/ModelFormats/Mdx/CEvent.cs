using System;
using MdxLib.Model;

namespace MdxLib.ModelFormats.Mdx
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

		public void LoadAll(CLoader Loader, MdxLib.Model.CModel Model)
		{
			int num = Loader.ReadInt32();
			while (num > 0)
			{
				Loader.PushLocation();
				MdxLib.Model.CEvent cEvent = new MdxLib.Model.CEvent(Model);
				Load(Loader, Model, cEvent);
				Model.Events.Add(cEvent);
				num -= Loader.PopLocation();
				if (num < 0)
				{
					throw new Exception("Error at location " + Loader.Location + ", too many Event bytes were read!");
				}
			}
		}

		public void Load(CLoader Loader, MdxLib.Model.CModel Model, MdxLib.Model.CEvent Event)
		{
			CNode.LoadNode(Loader, Model, Event);
			Loader.ExpectTag("KEVT");
			int num = Loader.ReadInt32();
			Loader.Attacher.AddObject(Model.GlobalSequences, Event.GlobalSequence, Loader.ReadInt32());
			for (int i = 0; i < num; i++)
			{
				CEventTrack cEventTrack = new CEventTrack(Model);
				LoadTrack(Loader, Model, Event, cEventTrack);
				Event.Tracks.Add(cEventTrack);
			}
		}

		public void LoadTrack(CLoader Loader, MdxLib.Model.CModel Model, MdxLib.Model.CEvent Event, CEventTrack Track)
		{
			Track.Time = Loader.ReadInt32();
		}

		public void SaveAll(CSaver Saver, MdxLib.Model.CModel Model)
		{
			if (!Model.HasEvents)
			{
				return;
			}
			Saver.WriteTag("EVTS");
			Saver.PushLocation();
			foreach (MdxLib.Model.CEvent @event in Model.Events)
			{
				Save(Saver, Model, @event);
			}
			Saver.PopExclusiveLocation();
		}

		public void Save(CSaver Saver, MdxLib.Model.CModel Model, MdxLib.Model.CEvent Event)
		{
			CNode.SaveNode(Saver, Model, Event, 1024);
			Saver.WriteTag("KEVT");
			Saver.WriteInt32(Event.Tracks.Count);
			Saver.WriteInt32(Event.GlobalSequence.ObjectId);
			foreach (CEventTrack track in Event.Tracks)
			{
				SaveTrack(Saver, Model, Event, track);
			}
		}

		public void SaveTrack(CSaver Saver, MdxLib.Model.CModel Model, MdxLib.Model.CEvent Event, CEventTrack Track)
		{
			Saver.WriteInt32(Track.Time);
		}
	}
}
