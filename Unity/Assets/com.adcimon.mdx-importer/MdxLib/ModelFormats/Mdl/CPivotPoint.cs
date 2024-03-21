using System.Collections.Generic;
using MdxLib.Model;
using MdxLib.ModelFormats.Mdl.Token;
using MdxLib.Primitives;

namespace MdxLib.ModelFormats.Mdl
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
			Loader.ReadInteger();
			Loader.ExpectToken(EType.CurlyBracketLeft);
			while (Loader.PeekToken() != EType.CurlyBracketRight)
			{
				PivotPointList.Add(LoadVector3(Loader));
			}
			Loader.ReadToken();
		}

		public void Save(CSaver Saver, MdxLib.Model.CModel Model, ICollection<CVector3> PivotPointList)
		{
			if (PivotPointList.Count <= 0)
			{
				return;
			}
			Saver.BeginGroup("PivotPoints", PivotPointList.Count);
			foreach (CVector3 PivotPoint in PivotPointList)
			{
				Saver.WriteTabs();
				Saver.WriteVector3(PivotPoint);
				Saver.WriteLine(",");
			}
			Saver.EndGroup();
		}
	}
}
