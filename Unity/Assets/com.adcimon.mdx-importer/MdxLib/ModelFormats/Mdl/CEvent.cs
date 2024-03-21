using System;
using MdxLib.Model;
using MdxLib.ModelFormats.Mdl.Token;

namespace MdxLib.ModelFormats.Mdl
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
			MdxLib.Model.CEvent cEvent = new MdxLib.Model.CEvent(Model);
			Load(Loader, Model, cEvent);
			Model.Events.Add(cEvent);
		}

		public void Load(CLoader Loader, MdxLib.Model.CModel Model, MdxLib.Model.CEvent Event)
		{
			Event.Name = Loader.ReadString();
			Loader.ExpectToken(EType.CurlyBracketLeft);
			while (Loader.PeekToken() != EType.CurlyBracketRight)
			{
				string text = Loader.ReadWord();
				if (LoadNode(Loader, Model, Event, text))
				{
					continue;
				}
				switch (text)
				{
				case "static":
					text = Loader.ReadWord();
					if (!LoadStaticNode(Loader, Model, Event, text))
					{
						throw new Exception("Syntax error at line " + Loader.Line + ", unknown tag \"" + text + "\"!");
					}
					break;
				case "eventtrack":
					Loader.ReadInteger();
					Loader.ExpectToken(EType.CurlyBracketLeft);
					while (Loader.PeekToken() != EType.CurlyBracketRight)
					{
						CEventTrack cEventTrack = new CEventTrack(Model);
						cEventTrack.Time = LoadInteger(Loader);
						Event.Tracks.Add(cEventTrack);
					}
					Loader.ReadToken();
					break;
				default:
					throw new Exception("Syntax error at line " + Loader.Line + ", unknown tag \"" + text + "\"!");
				}
			}
			Loader.ReadToken();
		}

		public void SaveAll(CSaver Saver, MdxLib.Model.CModel Model)
		{
			if (!Model.HasEvents)
			{
				return;
			}
			foreach (MdxLib.Model.CEvent @event in Model.Events)
			{
				Save(Saver, Model, @event);
			}
		}

		public void Save(CSaver Saver, MdxLib.Model.CModel Model, MdxLib.Model.CEvent Event)
		{
			Saver.BeginGroup("EventObject", Event.Name);
			SaveNode(Saver, Model, Event);
			Saver.BeginGroup("EventTrack", Event.Tracks.Count);
			foreach (CEventTrack track in Event.Tracks)
			{
				Saver.WriteTabs();
				Saver.WriteInteger(track.Time);
				Saver.WriteLine(",");
			}
			Saver.EndGroup();
			Saver.EndGroup();
		}
	}
}
