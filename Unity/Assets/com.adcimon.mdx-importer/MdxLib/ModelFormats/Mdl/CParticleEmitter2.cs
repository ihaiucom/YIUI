using System;
using MdxLib.Model;
using MdxLib.ModelFormats.Mdl.Token;
using MdxLib.ModelFormats.Mdl.Value;
using MdxLib.Primitives;

namespace MdxLib.ModelFormats.Mdl
{
	internal sealed class CParticleEmitter2 : CNode
	{
		private struct SSegment
		{
			public MdxLib.Primitives.CVector3 Color;

			public float Alpha;

			public float Scaling;

			public SSegment(MdxLib.Primitives.CVector3 Color, float Alpha, float Scaling)
			{
				this.Color = Color;
				this.Alpha = Alpha;
				this.Scaling = Scaling;
			}
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
			MdxLib.Model.CParticleEmitter2 cParticleEmitter = new MdxLib.Model.CParticleEmitter2(Model);
			Load(Loader, Model, cParticleEmitter);
			Model.ParticleEmitters2.Add(cParticleEmitter);
		}

		public void Load(CLoader Loader, MdxLib.Model.CModel Model, MdxLib.Model.CParticleEmitter2 ParticleEmitter2)
		{
			SSegment sSegment = new SSegment(CConstants.DefaultColor, 1f, 1f);
			SSegment sSegment2 = new SSegment(CConstants.DefaultColor, 1f, 1f);
			SSegment sSegment3 = new SSegment(CConstants.DefaultColor, 1f, 1f);
			ParticleEmitter2.Name = Loader.ReadString();
			Loader.ExpectToken(EType.CurlyBracketLeft);
			while (Loader.PeekToken() != EType.CurlyBracketRight)
			{
				string text = Loader.ReadWord();
				if (LoadNode(Loader, Model, ParticleEmitter2, text))
				{
					continue;
				}
				switch (text)
				{
				case "static":
					text = Loader.ReadWord();
					if (!LoadStaticNode(Loader, Model, ParticleEmitter2, text))
					{
						switch (text)
						{
						case "speed":
							LoadStaticAnimator(Loader, Model, ParticleEmitter2.Speed, CFloat.Instance);
							break;
						case "variation":
							LoadStaticAnimator(Loader, Model, ParticleEmitter2.Variation, CFloat.Instance);
							break;
						case "latitude":
							LoadStaticAnimator(Loader, Model, ParticleEmitter2.Latitude, CFloat.Instance);
							break;
						case "gravity":
							LoadStaticAnimator(Loader, Model, ParticleEmitter2.Gravity, CFloat.Instance);
							break;
						case "visibility":
							LoadStaticAnimator(Loader, Model, ParticleEmitter2.Visibility, CFloat.Instance);
							break;
						case "emissionrate":
							LoadStaticAnimator(Loader, Model, ParticleEmitter2.EmissionRate, CFloat.Instance);
							break;
						case "width":
							LoadStaticAnimator(Loader, Model, ParticleEmitter2.Width, CFloat.Instance);
							break;
						case "length":
							LoadStaticAnimator(Loader, Model, ParticleEmitter2.Length, CFloat.Instance);
							break;
						default:
							throw new Exception("Syntax error at line " + Loader.Line + ", unknown tag \"" + text + "\"!");
						}
					}
					break;
				case "speed":
					LoadAnimator(Loader, Model, ParticleEmitter2.Speed, CFloat.Instance);
					break;
				case "variation":
					LoadAnimator(Loader, Model, ParticleEmitter2.Variation, CFloat.Instance);
					break;
				case "latitude":
					LoadAnimator(Loader, Model, ParticleEmitter2.Latitude, CFloat.Instance);
					break;
				case "gravity":
					LoadAnimator(Loader, Model, ParticleEmitter2.Gravity, CFloat.Instance);
					break;
				case "visibility":
					LoadAnimator(Loader, Model, ParticleEmitter2.Visibility, CFloat.Instance);
					break;
				case "emissionrate":
					LoadAnimator(Loader, Model, ParticleEmitter2.EmissionRate, CFloat.Instance);
					break;
				case "width":
					LoadAnimator(Loader, Model, ParticleEmitter2.Width, CFloat.Instance);
					break;
				case "length":
					LoadAnimator(Loader, Model, ParticleEmitter2.Length, CFloat.Instance);
					break;
				case "rows":
					ParticleEmitter2.Rows = LoadInteger(Loader);
					break;
				case "columns":
					ParticleEmitter2.Columns = LoadInteger(Loader);
					break;
				case "textureid":
					Loader.Attacher.AddObject(Model.Textures, ParticleEmitter2.Texture, LoadId(Loader));
					break;
				case "replaceableid":
					ParticleEmitter2.ReplaceableId = LoadInteger(Loader);
					break;
				case "priorityplane":
					ParticleEmitter2.PriorityPlane = LoadInteger(Loader);
					break;
				case "time":
					ParticleEmitter2.Time = LoadFloat(Loader);
					break;
				case "lifespan":
					ParticleEmitter2.LifeSpan = LoadFloat(Loader);
					break;
				case "taillength":
					ParticleEmitter2.TailLength = LoadFloat(Loader);
					break;
				case "segmentcolor":
					Loader.ExpectToken(EType.CurlyBracketLeft);
					Loader.ExpectWord("color");
					sSegment.Color = Loader.ReadColor();
					Loader.ExpectToken(EType.Separator);
					Loader.ExpectWord("color");
					sSegment2.Color = Loader.ReadColor();
					Loader.ExpectToken(EType.Separator);
					Loader.ExpectWord("color");
					sSegment3.Color = Loader.ReadColor();
					Loader.ExpectToken(EType.Separator);
					Loader.ExpectToken(EType.CurlyBracketRight);
					Loader.ExpectToken(EType.Separator);
					break;
				case "alpha":
				{
					MdxLib.Primitives.CVector3 cVector6 = LoadVector3(Loader);
					sSegment.Alpha = cVector6.X / 255f;
					sSegment2.Alpha = cVector6.Y / 255f;
					sSegment3.Alpha = cVector6.Z / 255f;
					break;
				}
				case "particlescaling":
				{
					MdxLib.Primitives.CVector3 cVector5 = LoadVector3(Loader);
					sSegment.Scaling = cVector5.X;
					sSegment2.Scaling = cVector5.Y;
					sSegment3.Scaling = cVector5.Z;
					break;
				}
				case "lifespanuvanim":
				{
					MdxLib.Primitives.CVector3 cVector4 = LoadVector3(Loader);
					ParticleEmitter2.HeadLife = new CInterval((int)cVector4.X, (int)cVector4.Y, (int)cVector4.Z);
					break;
				}
				case "decayuvanim":
				{
					MdxLib.Primitives.CVector3 cVector3 = LoadVector3(Loader);
					ParticleEmitter2.HeadDecay = new CInterval((int)cVector3.X, (int)cVector3.Y, (int)cVector3.Z);
					break;
				}
				case "tailuvanim":
				{
					MdxLib.Primitives.CVector3 cVector2 = LoadVector3(Loader);
					ParticleEmitter2.TailLife = new CInterval((int)cVector2.X, (int)cVector2.Y, (int)cVector2.Z);
					break;
				}
				case "taildecayuvanim":
				{
					MdxLib.Primitives.CVector3 cVector = LoadVector3(Loader);
					ParticleEmitter2.TailDecay = new CInterval((int)cVector.X, (int)cVector.Y, (int)cVector.Z);
					break;
				}
				case "sortprimsfarz":
					ParticleEmitter2.SortPrimitivesFarZ = LoadBoolean(Loader);
					break;
				case "lineemitter":
					ParticleEmitter2.LineEmitter = LoadBoolean(Loader);
					break;
				case "modelspace":
					ParticleEmitter2.ModelSpace = LoadBoolean(Loader);
					break;
				case "unshaded":
					ParticleEmitter2.Unshaded = LoadBoolean(Loader);
					break;
				case "unfogged":
					ParticleEmitter2.Unfogged = LoadBoolean(Loader);
					break;
				case "xyquad":
					ParticleEmitter2.XYQuad = LoadBoolean(Loader);
					break;
				case "squirt":
					ParticleEmitter2.Squirt = LoadBoolean(Loader);
					break;
				case "head":
					ParticleEmitter2.Head = LoadBoolean(Loader);
					break;
				case "tail":
					ParticleEmitter2.Tail = LoadBoolean(Loader);
					break;
				case "both":
				{
					bool head = (ParticleEmitter2.Tail = LoadBoolean(Loader));
					ParticleEmitter2.Head = head;
					break;
				}
				case "blend":
					ParticleEmitter2.FilterMode = EParticleEmitter2FilterMode.Blend;
					LoadBoolean(Loader);
					break;
				case "additive":
					ParticleEmitter2.FilterMode = EParticleEmitter2FilterMode.Additive;
					LoadBoolean(Loader);
					break;
				case "modulate":
					ParticleEmitter2.FilterMode = EParticleEmitter2FilterMode.Modulate;
					LoadBoolean(Loader);
					break;
				case "modulate2x":
					ParticleEmitter2.FilterMode = EParticleEmitter2FilterMode.Modulate2x;
					LoadBoolean(Loader);
					break;
				case "alphakey":
					ParticleEmitter2.FilterMode = EParticleEmitter2FilterMode.AlphaKey;
					LoadBoolean(Loader);
					break;
				default:
					throw new Exception("Syntax error at line " + Loader.Line + ", unknown tag \"" + text + "\"!");
				}
			}
			Loader.ReadToken();
			ParticleEmitter2.Segment1 = new CSegment(sSegment.Color, sSegment.Alpha, sSegment.Scaling);
			ParticleEmitter2.Segment2 = new CSegment(sSegment2.Color, sSegment2.Alpha, sSegment2.Scaling);
			ParticleEmitter2.Segment3 = new CSegment(sSegment3.Color, sSegment3.Alpha, sSegment3.Scaling);
		}

		public void SaveAll(CSaver Saver, MdxLib.Model.CModel Model)
		{
			if (!Model.HasParticleEmitters2)
			{
				return;
			}
			foreach (MdxLib.Model.CParticleEmitter2 item in Model.ParticleEmitters2)
			{
				Save(Saver, Model, item);
			}
		}

		public void Save(CSaver Saver, MdxLib.Model.CModel Model, MdxLib.Model.CParticleEmitter2 ParticleEmitter2)
		{
			Saver.BeginGroup("ParticleEmitter2", ParticleEmitter2.Name);
			SaveNode(Saver, Model, ParticleEmitter2);
			SaveBoolean(Saver, FilterModeToString(ParticleEmitter2.FilterMode), Value: true);
			SaveBoolean(Saver, "SortPrimsFarZ", ParticleEmitter2.SortPrimitivesFarZ);
			SaveBoolean(Saver, "LineEmitter", ParticleEmitter2.LineEmitter);
			SaveBoolean(Saver, "ModelSpace", ParticleEmitter2.ModelSpace);
			SaveBoolean(Saver, "Unshaded", ParticleEmitter2.Unshaded);
			SaveBoolean(Saver, "Unfogged", ParticleEmitter2.Unfogged);
			SaveBoolean(Saver, "XYQuad", ParticleEmitter2.XYQuad);
			SaveBoolean(Saver, "Squirt", ParticleEmitter2.Squirt);
			SaveBoolean(Saver, "Head", ParticleEmitter2.Head && !ParticleEmitter2.Tail);
			SaveBoolean(Saver, "Tail", ParticleEmitter2.Tail && !ParticleEmitter2.Head);
			SaveBoolean(Saver, "Both", ParticleEmitter2.Head && ParticleEmitter2.Tail);
			SaveInteger(Saver, "Rows", ParticleEmitter2.Rows, ECondition.NotZero);
			SaveInteger(Saver, "Columns", ParticleEmitter2.Columns, ECondition.NotZero);
			SaveId(Saver, "TextureID", ParticleEmitter2.Texture.ObjectId, ECondition.NotInvalidId);
			SaveInteger(Saver, "ReplaceableId", ParticleEmitter2.ReplaceableId, ECondition.NotZero);
			SaveInteger(Saver, "PriorityPlane", ParticleEmitter2.PriorityPlane, ECondition.NotZero);
			SaveFloat(Saver, "Time", ParticleEmitter2.Time, ECondition.NotZero);
			SaveFloat(Saver, "LifeSpan", ParticleEmitter2.LifeSpan, ECondition.NotZero);
			SaveFloat(Saver, "TailLength", ParticleEmitter2.TailLength, ECondition.NotZero);
			Saver.BeginGroup("SegmentColor");
			SaveColor(Saver, "Color", ParticleEmitter2.Segment1.Color);
			SaveColor(Saver, "Color", ParticleEmitter2.Segment2.Color);
			SaveColor(Saver, "Color", ParticleEmitter2.Segment3.Color);
			Saver.EndGroup(",");
			Saver.WriteTabs();
			Saver.WriteWord("Alpha { ");
			Saver.WriteInteger((int)(ParticleEmitter2.Segment1.Alpha * 255f));
			Saver.WriteWord(", ");
			Saver.WriteInteger((int)(ParticleEmitter2.Segment2.Alpha * 255f));
			Saver.WriteWord(", ");
			Saver.WriteInteger((int)(ParticleEmitter2.Segment3.Alpha * 255f));
			Saver.WriteLine(" },");
			SaveVector3(Saver, "ParticleScaling", new MdxLib.Primitives.CVector3(ParticleEmitter2.Segment1.Scaling, ParticleEmitter2.Segment2.Scaling, ParticleEmitter2.Segment3.Scaling));
			SaveVector3(Saver, "LifeSpanUVAnim", new MdxLib.Primitives.CVector3(ParticleEmitter2.HeadLife.Start, ParticleEmitter2.HeadLife.End, ParticleEmitter2.HeadLife.Repeat));
			SaveVector3(Saver, "DecayUVAnim", new MdxLib.Primitives.CVector3(ParticleEmitter2.HeadDecay.Start, ParticleEmitter2.HeadDecay.End, ParticleEmitter2.HeadDecay.Repeat));
			SaveVector3(Saver, "TailUVAnim", new MdxLib.Primitives.CVector3(ParticleEmitter2.TailLife.Start, ParticleEmitter2.TailLife.End, ParticleEmitter2.TailLife.Repeat));
			SaveVector3(Saver, "TailDecayUVAnim", new MdxLib.Primitives.CVector3(ParticleEmitter2.TailDecay.Start, ParticleEmitter2.TailDecay.End, ParticleEmitter2.TailDecay.Repeat));
			SaveAnimator(Saver, Model, ParticleEmitter2.Speed, CFloat.Instance, "Speed");
			SaveAnimator(Saver, Model, ParticleEmitter2.Variation, CFloat.Instance, "Variation");
			SaveAnimator(Saver, Model, ParticleEmitter2.Latitude, CFloat.Instance, "Latitude");
			SaveAnimator(Saver, Model, ParticleEmitter2.Gravity, CFloat.Instance, "Gravity");
			SaveAnimator(Saver, Model, ParticleEmitter2.EmissionRate, CFloat.Instance, "EmissionRate");
			SaveAnimator(Saver, Model, ParticleEmitter2.Width, CFloat.Instance, "Width");
			SaveAnimator(Saver, Model, ParticleEmitter2.Length, CFloat.Instance, "Length");
			SaveAnimator(Saver, Model, ParticleEmitter2.Visibility, CFloat.Instance, "Visibility", ECondition.NotOne);
			Saver.EndGroup();
		}

		private string FilterModeToString(EParticleEmitter2FilterMode FilterMode)
		{
			return FilterMode switch
			{
				EParticleEmitter2FilterMode.Blend => "Blend", 
				EParticleEmitter2FilterMode.Additive => "Additive", 
				EParticleEmitter2FilterMode.Modulate => "Modulate", 
				EParticleEmitter2FilterMode.Modulate2x => "Modulate2x", 
				EParticleEmitter2FilterMode.AlphaKey => "AlphaKey", 
				_ => "", 
			};
		}
	}
}
