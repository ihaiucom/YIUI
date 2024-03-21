using System;
using MdxLib.Model;
using MdxLib.ModelFormats.Mdx.Value;
using MdxLib.Primitives;

namespace MdxLib.ModelFormats.Mdx
{
	internal sealed class CParticleEmitter2 : CNode
	{
		private struct SInterval
		{
			public int Start;

			public int End;

			public int Repeat;
		}

		private struct SSegment
		{
			public MdxLib.Primitives.CVector3 Color;

			public float Alpha;

			public float Scaling;
		}

		private static class CSingleton
		{
			public static CParticleEmitter2 Instance;

			static CSingleton()
			{
				Instance = new CParticleEmitter2();
			}
		}

		public static CParticleEmitter2 Instance => CSingleton.Instance;

		private CParticleEmitter2()
		{
		}

		public void LoadAll(CLoader Loader, MdxLib.Model.CModel Model)
		{
			int num = Loader.ReadInt32();
			while (num > 0)
			{
				Loader.PushLocation();
				MdxLib.Model.CParticleEmitter2 cParticleEmitter = new MdxLib.Model.CParticleEmitter2(Model);
				Load(Loader, Model, cParticleEmitter);
				Model.ParticleEmitters2.Add(cParticleEmitter);
				num -= Loader.PopLocation();
				if (num < 0)
				{
					throw new Exception("Error at location " + Loader.Location + ", too many ParticleEmitter2 bytes were read!");
				}
			}
		}

		public void Load(CLoader Loader, MdxLib.Model.CModel Model, MdxLib.Model.CParticleEmitter2 ParticleEmitter2)
		{
			SSegment sSegment = default(SSegment);
			SSegment sSegment2 = default(SSegment);
			SSegment sSegment3 = default(SSegment);
			SInterval sInterval = default(SInterval);
			SInterval sInterval2 = default(SInterval);
			SInterval sInterval3 = default(SInterval);
			SInterval sInterval4 = default(SInterval);
			Loader.PushLocation();
			int num = Loader.ReadInt32();
			int num2 = CNode.LoadNode(Loader, Model, ParticleEmitter2);
			ParticleEmitter2.Speed.MakeStatic(Loader.ReadFloat());
			ParticleEmitter2.Variation.MakeStatic(Loader.ReadFloat());
			ParticleEmitter2.Latitude.MakeStatic(Loader.ReadFloat());
			ParticleEmitter2.Gravity.MakeStatic(Loader.ReadFloat());
			ParticleEmitter2.LifeSpan = Loader.ReadFloat();
			ParticleEmitter2.EmissionRate.MakeStatic(Loader.ReadFloat());
			ParticleEmitter2.Length.MakeStatic(Loader.ReadFloat());
			ParticleEmitter2.Width.MakeStatic(Loader.ReadFloat());
			int num3 = Loader.ReadInt32();
			ParticleEmitter2.Rows = Loader.ReadInt32();
			ParticleEmitter2.Columns = Loader.ReadInt32();
			int num4 = Loader.ReadInt32();
			ParticleEmitter2.TailLength = Loader.ReadFloat();
			ParticleEmitter2.Time = Loader.ReadFloat();
			sSegment.Color = Loader.ReadVector3();
			sSegment2.Color = Loader.ReadVector3();
			sSegment3.Color = Loader.ReadVector3();
			sSegment.Alpha = (float)Loader.ReadInt8() / 255f;
			sSegment2.Alpha = (float)Loader.ReadInt8() / 255f;
			sSegment3.Alpha = (float)Loader.ReadInt8() / 255f;
			sSegment.Scaling = Loader.ReadFloat();
			sSegment2.Scaling = Loader.ReadFloat();
			sSegment3.Scaling = Loader.ReadFloat();
			sInterval.Start = Loader.ReadInt32();
			sInterval.End = Loader.ReadInt32();
			sInterval.Repeat = Loader.ReadInt32();
			sInterval2.Start = Loader.ReadInt32();
			sInterval2.End = Loader.ReadInt32();
			sInterval2.Repeat = Loader.ReadInt32();
			sInterval3.Start = Loader.ReadInt32();
			sInterval3.End = Loader.ReadInt32();
			sInterval3.Repeat = Loader.ReadInt32();
			sInterval4.Start = Loader.ReadInt32();
			sInterval4.End = Loader.ReadInt32();
			sInterval4.Repeat = Loader.ReadInt32();
			ParticleEmitter2.Segment1 = new CSegment(sSegment.Color, sSegment.Alpha, sSegment.Scaling);
			ParticleEmitter2.Segment2 = new CSegment(sSegment2.Color, sSegment2.Alpha, sSegment2.Scaling);
			ParticleEmitter2.Segment3 = new CSegment(sSegment3.Color, sSegment3.Alpha, sSegment3.Scaling);
			ParticleEmitter2.HeadLife = new CInterval(sInterval.Start, sInterval.End, sInterval.Repeat);
			ParticleEmitter2.HeadDecay = new CInterval(sInterval2.Start, sInterval2.End, sInterval2.Repeat);
			ParticleEmitter2.TailLife = new CInterval(sInterval3.Start, sInterval3.End, sInterval3.Repeat);
			ParticleEmitter2.TailDecay = new CInterval(sInterval4.Start, sInterval4.End, sInterval4.Repeat);
			Loader.Attacher.AddObject(Model.Textures, ParticleEmitter2.Texture, Loader.ReadInt32());
			int num5 = Loader.ReadInt32();
			ParticleEmitter2.PriorityPlane = Loader.ReadInt32();
			ParticleEmitter2.ReplaceableId = Loader.ReadInt32();
			switch (num3)
			{
			case 0:
				ParticleEmitter2.FilterMode = EParticleEmitter2FilterMode.Blend;
				break;
			case 1:
				ParticleEmitter2.FilterMode = EParticleEmitter2FilterMode.Additive;
				break;
			case 2:
				ParticleEmitter2.FilterMode = EParticleEmitter2FilterMode.Modulate;
				break;
			case 3:
				ParticleEmitter2.FilterMode = EParticleEmitter2FilterMode.Modulate2x;
				break;
			case 4:
				ParticleEmitter2.FilterMode = EParticleEmitter2FilterMode.AlphaKey;
				break;
			}
			ParticleEmitter2.Unshaded = (num2 & 0x8000) != 0;
			ParticleEmitter2.SortPrimitivesFarZ = (num2 & 0x10000) != 0;
			ParticleEmitter2.LineEmitter = (num2 & 0x20000) != 0;
			ParticleEmitter2.Unfogged = (num2 & 0x40000) != 0;
			ParticleEmitter2.ModelSpace = (num2 & 0x80000) != 0;
			ParticleEmitter2.XYQuad = (num2 & 0x100000) != 0;
			ParticleEmitter2.Head = num4 == 0 || num4 == 2;
			ParticleEmitter2.Tail = num4 == 1 || num4 == 2;
			ParticleEmitter2.Squirt = num5 == 1;
			num -= Loader.PopLocation();
			if (num < 0)
			{
				throw new Exception("Error at location " + Loader.Location + ", too many ParticleEmitter2 bytes were read!");
			}
			while (num > 0)
			{
				Loader.PushLocation();
				string text = Loader.ReadTag();
				switch (text)
				{
				case "KP2S":
					CObject.LoadAnimator(Loader, Model, ParticleEmitter2.Speed, CFloat.Instance);
					break;
				case "KP2R":
					CObject.LoadAnimator(Loader, Model, ParticleEmitter2.Variation, CFloat.Instance);
					break;
				case "KP2L":
					CObject.LoadAnimator(Loader, Model, ParticleEmitter2.Latitude, CFloat.Instance);
					break;
				case "KP2G":
					CObject.LoadAnimator(Loader, Model, ParticleEmitter2.Gravity, CFloat.Instance);
					break;
				case "KP2E":
					CObject.LoadAnimator(Loader, Model, ParticleEmitter2.EmissionRate, CFloat.Instance);
					break;
				case "KP2W":
					CObject.LoadAnimator(Loader, Model, ParticleEmitter2.Width, CFloat.Instance);
					break;
				case "KP2N":
					CObject.LoadAnimator(Loader, Model, ParticleEmitter2.Length, CFloat.Instance);
					break;
				case "KP2V":
					CObject.LoadAnimator(Loader, Model, ParticleEmitter2.Visibility, CFloat.Instance);
					break;
				default:
					throw new Exception("Error at location " + Loader.Location + ", unknown ParticleEmitter2 tag \"" + text + "\"!");
				}
				num -= Loader.PopLocation();
				if (num < 0)
				{
					throw new Exception("Error at location " + Loader.Location + ", too many ParticleEmitter2 bytes were read!");
				}
			}
		}

		public void SaveAll(CSaver Saver, MdxLib.Model.CModel Model)
		{
			if (!Model.HasParticleEmitters2)
			{
				return;
			}
			Saver.WriteTag("PRE2");
			Saver.PushLocation();
			foreach (MdxLib.Model.CParticleEmitter2 item in Model.ParticleEmitters2)
			{
				Save(Saver, Model, item);
			}
			Saver.PopExclusiveLocation();
		}

		public void Save(CSaver Saver, MdxLib.Model.CModel Model, MdxLib.Model.CParticleEmitter2 ParticleEmitter2)
		{
			int num = 4096;
			int num2 = 0;
			int value = 0;
			if (ParticleEmitter2.Unshaded)
			{
				num |= 0x8000;
			}
			if (ParticleEmitter2.SortPrimitivesFarZ)
			{
				num |= 0x10000;
			}
			if (ParticleEmitter2.LineEmitter)
			{
				num |= 0x20000;
			}
			if (ParticleEmitter2.Unfogged)
			{
				num |= 0x40000;
			}
			if (ParticleEmitter2.ModelSpace)
			{
				num |= 0x80000;
			}
			if (ParticleEmitter2.XYQuad)
			{
				num |= 0x100000;
			}
			num2 = (ParticleEmitter2.Head ? (ParticleEmitter2.Tail ? 2 : 0) : (ParticleEmitter2.Tail ? 1 : 0));
			switch (ParticleEmitter2.FilterMode)
			{
			case EParticleEmitter2FilterMode.Blend:
				value = 0;
				break;
			case EParticleEmitter2FilterMode.Additive:
				value = 1;
				break;
			case EParticleEmitter2FilterMode.Modulate:
				value = 2;
				break;
			case EParticleEmitter2FilterMode.Modulate2x:
				value = 3;
				break;
			case EParticleEmitter2FilterMode.AlphaKey:
				value = 4;
				break;
			}
			Saver.PushLocation();
			CNode.SaveNode(Saver, Model, ParticleEmitter2, num);
			Saver.WriteFloat(ParticleEmitter2.Speed.GetValue());
			Saver.WriteFloat(ParticleEmitter2.Variation.GetValue());
			Saver.WriteFloat(ParticleEmitter2.Latitude.GetValue());
			Saver.WriteFloat(ParticleEmitter2.Gravity.GetValue());
			Saver.WriteFloat(ParticleEmitter2.LifeSpan);
			Saver.WriteFloat(ParticleEmitter2.EmissionRate.GetValue());
			Saver.WriteFloat(ParticleEmitter2.Length.GetValue());
			Saver.WriteFloat(ParticleEmitter2.Width.GetValue());
			Saver.WriteInt32(value);
			Saver.WriteInt32(ParticleEmitter2.Rows);
			Saver.WriteInt32(ParticleEmitter2.Columns);
			Saver.WriteInt32(num2);
			Saver.WriteFloat(ParticleEmitter2.TailLength);
			Saver.WriteFloat(ParticleEmitter2.Time);
			Saver.WriteVector3(ParticleEmitter2.Segment1.Color);
			Saver.WriteVector3(ParticleEmitter2.Segment2.Color);
			Saver.WriteVector3(ParticleEmitter2.Segment3.Color);
			Saver.WriteInt8((int)(ParticleEmitter2.Segment1.Alpha * 255f));
			Saver.WriteInt8((int)(ParticleEmitter2.Segment2.Alpha * 255f));
			Saver.WriteInt8((int)(ParticleEmitter2.Segment3.Alpha * 255f));
			Saver.WriteFloat(ParticleEmitter2.Segment1.Scaling);
			Saver.WriteFloat(ParticleEmitter2.Segment2.Scaling);
			Saver.WriteFloat(ParticleEmitter2.Segment3.Scaling);
			Saver.WriteInt32(ParticleEmitter2.HeadLife.Start);
			Saver.WriteInt32(ParticleEmitter2.HeadLife.End);
			Saver.WriteInt32(ParticleEmitter2.HeadLife.Repeat);
			Saver.WriteInt32(ParticleEmitter2.HeadDecay.Start);
			Saver.WriteInt32(ParticleEmitter2.HeadDecay.End);
			Saver.WriteInt32(ParticleEmitter2.HeadDecay.Repeat);
			Saver.WriteInt32(ParticleEmitter2.TailLife.Start);
			Saver.WriteInt32(ParticleEmitter2.TailLife.End);
			Saver.WriteInt32(ParticleEmitter2.TailLife.Repeat);
			Saver.WriteInt32(ParticleEmitter2.TailDecay.Start);
			Saver.WriteInt32(ParticleEmitter2.TailDecay.End);
			Saver.WriteInt32(ParticleEmitter2.TailDecay.Repeat);
			Saver.WriteInt32(ParticleEmitter2.Texture.ObjectId);
			Saver.WriteInt32(ParticleEmitter2.Squirt ? 1 : 0);
			Saver.WriteInt32(ParticleEmitter2.PriorityPlane);
			Saver.WriteInt32(ParticleEmitter2.ReplaceableId);
			CObject.SaveAnimator(Saver, Model, ParticleEmitter2.Speed, CFloat.Instance, "KP2S");
			CObject.SaveAnimator(Saver, Model, ParticleEmitter2.Variation, CFloat.Instance, "KP2R");
			CObject.SaveAnimator(Saver, Model, ParticleEmitter2.Latitude, CFloat.Instance, "KP2L");
			CObject.SaveAnimator(Saver, Model, ParticleEmitter2.Gravity, CFloat.Instance, "KP2G");
			CObject.SaveAnimator(Saver, Model, ParticleEmitter2.EmissionRate, CFloat.Instance, "KP2E");
			CObject.SaveAnimator(Saver, Model, ParticleEmitter2.Width, CFloat.Instance, "KP2W");
			CObject.SaveAnimator(Saver, Model, ParticleEmitter2.Length, CFloat.Instance, "KP2N");
			CObject.SaveAnimator(Saver, Model, ParticleEmitter2.Visibility, CFloat.Instance, "KP2V");
			Saver.PopInclusiveLocation();
		}
	}
}
