using System.Xml;
using MdxLib.Model;

namespace MdxLib.ModelFormats.Xml
{
	internal sealed class CEventTrack : CObject
	{
		private static class CSingleton
		{
			public static CEventTrack Instance;

			static CSingleton()
			{
				Instance = new CEventTrack();
			}
		}

		public static CEventTrack Instance => CSingleton.Instance;

		private CEventTrack()
		{
		}

		public void Load(CLoader Loader, XmlNode Node, MdxLib.Model.CModel Model, MdxLib.Model.CEvent Event, MdxLib.Model.CEventTrack EventTrack)
		{
			EventTrack.Time = ReadInteger(Node, "time", EventTrack.Time);
		}

		public void Save(CSaver Saver, XmlNode Node, MdxLib.Model.CModel Model, MdxLib.Model.CEvent Event, MdxLib.Model.CEventTrack EventTrack)
		{
			WriteInteger(Node, "time", EventTrack.Time);
		}
	}
}
