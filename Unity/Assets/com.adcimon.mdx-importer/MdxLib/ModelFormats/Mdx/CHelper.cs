using System;
using MdxLib.Model;

namespace MdxLib.ModelFormats.Mdx
{
	internal sealed class CHelper : CNode
	{
		private static class CSingleton
		{
			public static CHelper Instance;

			static CSingleton()
			{
				Instance = new CHelper();
			}
		}

		public static CHelper Instance => CSingleton.Instance;

		private CHelper()
		{
		}

		public void LoadAll(CLoader Loader, MdxLib.Model.CModel Model)
		{
			int num = Loader.ReadInt32();
			while (num > 0)
			{
				Loader.PushLocation();
				MdxLib.Model.CHelper cHelper = new MdxLib.Model.CHelper(Model);
				Load(Loader, Model, cHelper);
				Model.Helpers.Add(cHelper);
				num -= Loader.PopLocation();
				if (num < 0)
				{
					throw new Exception("Error at location " + Loader.Location + ", too many Helper bytes were read!");
				}
			}
		}

		public void Load(CLoader Loader, MdxLib.Model.CModel Model, MdxLib.Model.CHelper Helper)
		{
			CNode.LoadNode(Loader, Model, Helper);
		}

		public void SaveAll(CSaver Saver, MdxLib.Model.CModel Model)
		{
			if (!Model.HasHelpers)
			{
				return;
			}
			Saver.WriteTag("HELP");
			Saver.PushLocation();
			foreach (MdxLib.Model.CHelper helper in Model.Helpers)
			{
				Save(Saver, Model, helper);
			}
			Saver.PopExclusiveLocation();
		}

		public void Save(CSaver Saver, MdxLib.Model.CModel Model, MdxLib.Model.CHelper Helper)
		{
			CNode.SaveNode(Saver, Model, Helper, 0);
		}
	}
}
