using MdxLib.Model;

namespace MdxLib.ModelFormats.Mdx
{
	internal sealed class CModelInfo : CObject
	{
		private static class CSingleton
		{
			public static CModelInfo Instance;

			static CSingleton()
			{
				Instance = new CModelInfo();
			}
		}

		public static CModelInfo Instance => CSingleton.Instance;

		private CModelInfo()
		{
		}

		public void Load(CLoader Loader, MdxLib.Model.CModel Model)
		{
			Loader.ReadInt32();
			Model.Name = Loader.ReadString(80);
			Model.AnimationFile = Loader.ReadString(260);
			Model.Extent = Loader.ReadExtent();
			Model.BlendTime = Loader.ReadInt32();
		}

		public void Save(CSaver Saver, MdxLib.Model.CModel Model)
		{
			Saver.WriteTag("MODL");
			Saver.PushLocation();
			Saver.WriteString(Model.Name, 80);
			Saver.WriteString(Model.AnimationFile, 260);
			Saver.WriteExtent(Model.Extent);
			Saver.WriteInt32(Model.BlendTime);
			Saver.PopExclusiveLocation();
		}
	}
}
