using System;
using System.Collections.Generic;
using MdxLib.Model;
using MdxLib.Primitives;

namespace MdxLib.ModelFormats.Mdx
{
	internal sealed class CPivotPoint : CObject
	{
		private static class CSingleton
		{
			public static CPivotPoint Instance;

			static CSingleton()
			{
				Instance = new CPivotPoint();
			}
		}

		public static CPivotPoint Instance => CSingleton.Instance;

		private CPivotPoint()
		{
		}

		public void Load(CLoader Loader, MdxLib.Model.CModel Model, ICollection<CVector3> PivotPointList)
		{
			int num = Loader.ReadInt32();
			while (num > 0)
			{
				Loader.PushLocation();
				PivotPointList.Add(Loader.ReadVector3());
				num -= Loader.PopLocation();
				if (num < 0)
				{
					throw new Exception("Error at location " + Loader.Location + ", too many PivotPoint bytes were read!");
				}
			}
		}

		public void Save(CSaver Saver, MdxLib.Model.CModel Model, ICollection<CVector3> PivotPointList)
		{
			if (PivotPointList.Count <= 0)
			{
				return;
			}
			Saver.WriteTag("PIVT");
			Saver.PushLocation();
			foreach (CVector3 PivotPoint in PivotPointList)
			{
				Saver.WriteVector3(PivotPoint);
			}
			Saver.PopExclusiveLocation();
		}
	}
}
