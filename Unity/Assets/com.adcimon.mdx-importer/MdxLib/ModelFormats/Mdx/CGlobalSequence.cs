using System;
using MdxLib.Model;

namespace MdxLib.ModelFormats.Mdx
{
	internal sealed class CGlobalSequence : CObject
	{
		private static class CSingleton
		{
			public static CGlobalSequence Instance;

			static CSingleton()
			{
				Instance = new CGlobalSequence();
			}
		}

		public static CGlobalSequence Instance => CSingleton.Instance;

		private CGlobalSequence()
		{
		}

		public void LoadAll(CLoader Loader, MdxLib.Model.CModel Model)
		{
			int num = Loader.ReadInt32();
			while (num > 0)
			{
				Loader.PushLocation();
				MdxLib.Model.CGlobalSequence cGlobalSequence = new MdxLib.Model.CGlobalSequence(Model);
				Load(Loader, Model, cGlobalSequence);
				Model.GlobalSequences.Add(cGlobalSequence);
				num -= Loader.PopLocation();
				if (num < 0)
				{
					throw new Exception("Error at location " + Loader.Location + ", too many GlobalSequence bytes were read!");
				}
			}
		}

		public void Load(CLoader Loader, MdxLib.Model.CModel Model, MdxLib.Model.CGlobalSequence GlobalSequence)
		{
			GlobalSequence.Duration = Loader.ReadInt32();
		}

		public void SaveAll(CSaver Saver, MdxLib.Model.CModel Model)
		{
			if (!Model.HasGlobalSequences)
			{
				return;
			}
			Saver.WriteTag("GLBS");
			Saver.PushLocation();
			foreach (MdxLib.Model.CGlobalSequence globalSequence in Model.GlobalSequences)
			{
				Save(Saver, Model, globalSequence);
			}
			Saver.PopExclusiveLocation();
		}

		public void Save(CSaver Saver, MdxLib.Model.CModel Model, MdxLib.Model.CGlobalSequence GlobalSequence)
		{
			Saver.WriteInt32(GlobalSequence.Duration);
		}
	}
}
