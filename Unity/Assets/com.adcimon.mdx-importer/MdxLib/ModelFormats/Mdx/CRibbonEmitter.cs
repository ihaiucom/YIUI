using System;
using MdxLib.Model;
using MdxLib.ModelFormats.Mdx.Value;

namespace MdxLib.ModelFormats.Mdx
{
	internal sealed class CRibbonEmitter : CNode
	{
		private static class CSingleton
		{
			public static CRibbonEmitter Instance;

			static CSingleton()
			{
				Instance = new CRibbonEmitter();
			}
		}

		public static CRibbonEmitter Instance => CSingleton.Instance;

		private CRibbonEmitter()
		{
		}

		public void LoadAll(CLoader Loader, MdxLib.Model.CModel Model)
		{
			int num = Loader.ReadInt32();
			while (num > 0)
			{
				Loader.PushLocation();
				MdxLib.Model.CRibbonEmitter cRibbonEmitter = new MdxLib.Model.CRibbonEmitter(Model);
				Load(Loader, Model, cRibbonEmitter);
				Model.RibbonEmitters.Add(cRibbonEmitter);
				num -= Loader.PopLocation();
				if (num < 0)
				{
					throw new Exception("Error at location " + Loader.Location + ", too many RibbonEmitter bytes were read!");
				}
			}
		}

		public void Load(CLoader Loader, MdxLib.Model.CModel Model, MdxLib.Model.CRibbonEmitter RibbonEmitter)
		{
			Loader.PushLocation();
			int num = Loader.ReadInt32();
			CNode.LoadNode(Loader, Model, RibbonEmitter);
			RibbonEmitter.HeightAbove.MakeStatic(Loader.ReadFloat());
			RibbonEmitter.HeightBelow.MakeStatic(Loader.ReadFloat());
			RibbonEmitter.Alpha.MakeStatic(Loader.ReadFloat());
			RibbonEmitter.Color.MakeStatic(Loader.ReadVector3());
			RibbonEmitter.LifeSpan = Loader.ReadFloat();
			RibbonEmitter.TextureSlot.MakeStatic(Loader.ReadInt32());
			RibbonEmitter.EmissionRate = Loader.ReadInt32();
			RibbonEmitter.Rows = Loader.ReadInt32();
			RibbonEmitter.Columns = Loader.ReadInt32();
			Loader.Attacher.AddObject(Model.Materials, RibbonEmitter.Material, Loader.ReadInt32());
			RibbonEmitter.Gravity = Loader.ReadFloat();
			num -= Loader.PopLocation();
			if (num < 0)
			{
				throw new Exception("Error at location " + Loader.Location + ", too many RibbonEmitter bytes were read!");
			}
			while (num > 0)
			{
				Loader.PushLocation();
				string text = Loader.ReadTag();
				switch (text)
				{
				case "KRHA":
					CObject.LoadAnimator(Loader, Model, RibbonEmitter.HeightAbove, CFloat.Instance);
					break;
				case "KRHB":
					CObject.LoadAnimator(Loader, Model, RibbonEmitter.HeightBelow, CFloat.Instance);
					break;
				case "KRAL":
					CObject.LoadAnimator(Loader, Model, RibbonEmitter.Alpha, CFloat.Instance);
					break;
				case "KRCO":
					CObject.LoadAnimator(Loader, Model, RibbonEmitter.Color, CVector3.Instance);
					break;
				case "KRTX":
					CObject.LoadAnimator(Loader, Model, RibbonEmitter.TextureSlot, CInteger.Instance);
					break;
				case "KRVS":
					CObject.LoadAnimator(Loader, Model, RibbonEmitter.Visibility, CFloat.Instance);
					break;
				default:
					throw new Exception("Error at location " + Loader.Location + ", unknown RibbonEmitter tag \"" + text + "\"!");
				}
				num -= Loader.PopLocation();
				if (num < 0)
				{
					throw new Exception("Error at location " + Loader.Location + ", too many RibbonEmitter bytes were read!");
				}
			}
		}

		public void SaveAll(CSaver Saver, MdxLib.Model.CModel Model)
		{
			if (!Model.HasRibbonEmitters)
			{
				return;
			}
			Saver.WriteTag("RIBB");
			Saver.PushLocation();
			foreach (MdxLib.Model.CRibbonEmitter ribbonEmitter in Model.RibbonEmitters)
			{
				Save(Saver, Model, ribbonEmitter);
			}
			Saver.PopExclusiveLocation();
		}

		public void Save(CSaver Saver, MdxLib.Model.CModel Model, MdxLib.Model.CRibbonEmitter RibbonEmitter)
		{
			Saver.PushLocation();
			CNode.SaveNode(Saver, Model, RibbonEmitter, 16384);
			Saver.WriteFloat(RibbonEmitter.HeightAbove.GetValue());
			Saver.WriteFloat(RibbonEmitter.HeightBelow.GetValue());
			Saver.WriteFloat(RibbonEmitter.Alpha.GetValue());
			Saver.WriteVector3(RibbonEmitter.Color.GetValue());
			Saver.WriteFloat(RibbonEmitter.LifeSpan);
			Saver.WriteInt32(RibbonEmitter.TextureSlot.GetValue());
			Saver.WriteInt32(RibbonEmitter.EmissionRate);
			Saver.WriteInt32(RibbonEmitter.Rows);
			Saver.WriteInt32(RibbonEmitter.Columns);
			Saver.WriteInt32(RibbonEmitter.Material.ObjectId);
			Saver.WriteFloat(RibbonEmitter.Gravity);
			CObject.SaveAnimator(Saver, Model, RibbonEmitter.HeightAbove, CFloat.Instance, "KRHA");
			CObject.SaveAnimator(Saver, Model, RibbonEmitter.HeightBelow, CFloat.Instance, "KRHB");
			CObject.SaveAnimator(Saver, Model, RibbonEmitter.Alpha, CFloat.Instance, "KRAL");
			CObject.SaveAnimator(Saver, Model, RibbonEmitter.Color, CVector3.Instance, "KRCO");
			CObject.SaveAnimator(Saver, Model, RibbonEmitter.TextureSlot, CInteger.Instance, "KRTX");
			CObject.SaveAnimator(Saver, Model, RibbonEmitter.Visibility, CFloat.Instance, "KRVS");
			Saver.PopInclusiveLocation();
		}
	}
}
