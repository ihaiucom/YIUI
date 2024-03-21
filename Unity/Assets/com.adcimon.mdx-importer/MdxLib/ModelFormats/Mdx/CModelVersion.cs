using MdxLib.Model;

namespace MdxLib.ModelFormats.Mdx
{
	internal sealed class CModelVersion : CObject
	{
		private static class CSingleton
		{
			public static CModelVersion Instance;

			static CSingleton()
			{
				Instance = new CModelVersion();
			}
		}

		public static CModelVersion Instance => CSingleton.Instance;

		private CModelVersion()
		{
		}

		public void Load(CLoader Loader, MdxLib.Model.CModel Model)
		{
			Loader.ReadInt32();
			Model.Version = Loader.ReadInt32();
		}

		public void Save(CSaver Saver, MdxLib.Model.CModel Model)
		{
			Saver.WriteTag("VERS");
			Saver.PushLocation();
			Saver.WriteInt32(Model.Version);
			Saver.PopExclusiveLocation();
		}
	}
}
