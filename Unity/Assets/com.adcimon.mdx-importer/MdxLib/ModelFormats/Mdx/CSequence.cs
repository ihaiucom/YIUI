using System;
using MdxLib.Model;

namespace MdxLib.ModelFormats.Mdx
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

		public void LoadAll(CLoader Loader, MdxLib.Model.CModel Model)
		{
			int num = Loader.ReadInt32();
			while (num > 0)
			{
				Loader.PushLocation();
				MdxLib.Model.CSequence cSequence = new MdxLib.Model.CSequence(Model);
				Load(Loader, Model, cSequence);
				Model.Sequences.Add(cSequence);
				num -= Loader.PopLocation();
				if (num < 0)
				{
					throw new Exception("Error at location " + Loader.Location + ", too many Sequence bytes were read!");
				}
			}
		}

		public void Load(CLoader Loader, MdxLib.Model.CModel Model, MdxLib.Model.CSequence Sequence)
		{
			Sequence.Name = Loader.ReadString(80);
			Sequence.IntervalStart = Loader.ReadInt32();
			Sequence.IntervalEnd = Loader.ReadInt32();
			Sequence.MoveSpeed = Loader.ReadFloat();
			int num = Loader.ReadInt32();
			Sequence.Rarity = Loader.ReadFloat();
			Sequence.SyncPoint = Loader.ReadInt32();
			Sequence.Extent = Loader.ReadExtent();
			Sequence.NonLooping = (num & 1) != 0;
		}

		public void SaveAll(CSaver Saver, MdxLib.Model.CModel Model)
		{
			if (!Model.HasSequences)
			{
				return;
			}
			Saver.WriteTag("SEQS");
			Saver.PushLocation();
			foreach (MdxLib.Model.CSequence sequence in Model.Sequences)
			{
				Save(Saver, Model, sequence);
			}
			Saver.PopExclusiveLocation();
		}

		public void Save(CSaver Saver, MdxLib.Model.CModel Model, MdxLib.Model.CSequence Sequence)
		{
			int num = 0;
			if (Sequence.NonLooping)
			{
				num |= 1;
			}
			Saver.WriteString(Sequence.Name, 80);
			Saver.WriteInt32(Sequence.IntervalStart);
			Saver.WriteInt32(Sequence.IntervalEnd);
			Saver.WriteFloat(Sequence.MoveSpeed);
			Saver.WriteInt32(num);
			Saver.WriteFloat(Sequence.Rarity);
			Saver.WriteInt32(Sequence.SyncPoint);
			Saver.WriteExtent(Sequence.Extent);
		}
	}
}
