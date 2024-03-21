using System;
using MdxLib.Model;
using MdxLib.ModelFormats.Mdl.Token;
using MdxLib.ModelFormats.Mdl.Value;

namespace MdxLib.ModelFormats.Mdl
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
			MdxLib.Model.CRibbonEmitter cRibbonEmitter = new MdxLib.Model.CRibbonEmitter(Model);
			Load(Loader, Model, cRibbonEmitter);
			Model.RibbonEmitters.Add(cRibbonEmitter);
		}

		public void Load(CLoader Loader, MdxLib.Model.CModel Model, MdxLib.Model.CRibbonEmitter RibbonEmitter)
		{
			RibbonEmitter.Name = Loader.ReadString();
			Loader.ExpectToken(EType.CurlyBracketLeft);
			while (Loader.PeekToken() != EType.CurlyBracketRight)
			{
				string text = Loader.ReadWord();
				if (LoadNode(Loader, Model, RibbonEmitter, text))
				{
					continue;
				}
				switch (text)
				{
				case "static":
					text = Loader.ReadWord();
					if (!LoadStaticNode(Loader, Model, RibbonEmitter, text))
					{
						switch (text)
						{
						case "heightabove":
							LoadStaticAnimator(Loader, Model, RibbonEmitter.HeightAbove, CFloat.Instance);
							break;
						case "heightbelow":
							LoadStaticAnimator(Loader, Model, RibbonEmitter.HeightBelow, CFloat.Instance);
							break;
						case "alpha":
							LoadStaticAnimator(Loader, Model, RibbonEmitter.Alpha, CFloat.Instance);
							break;
						case "color":
							LoadStaticAnimator(Loader, Model, RibbonEmitter.Color, CColor.Instance);
							break;
						case "textureslot":
							LoadStaticAnimator(Loader, Model, RibbonEmitter.TextureSlot, CInteger.Instance);
							break;
						case "visibility":
							LoadStaticAnimator(Loader, Model, RibbonEmitter.Visibility, CFloat.Instance);
							break;
						default:
							throw new Exception("Syntax error at line " + Loader.Line + ", unknown tag \"" + text + "\"!");
						}
					}
					break;
				case "heightabove":
					LoadAnimator(Loader, Model, RibbonEmitter.HeightAbove, CFloat.Instance);
					break;
				case "heightbelow":
					LoadAnimator(Loader, Model, RibbonEmitter.HeightBelow, CFloat.Instance);
					break;
				case "alpha":
					LoadAnimator(Loader, Model, RibbonEmitter.Alpha, CFloat.Instance);
					break;
				case "color":
					LoadAnimator(Loader, Model, RibbonEmitter.Color, CColor.Instance);
					break;
				case "textureslot":
					LoadAnimator(Loader, Model, RibbonEmitter.TextureSlot, CInteger.Instance);
					break;
				case "visibility":
					LoadAnimator(Loader, Model, RibbonEmitter.Visibility, CFloat.Instance);
					break;
				case "emissionrate":
					RibbonEmitter.EmissionRate = LoadInteger(Loader);
					break;
				case "lifespan":
					RibbonEmitter.LifeSpan = LoadFloat(Loader);
					break;
				case "gravity":
					RibbonEmitter.Gravity = LoadFloat(Loader);
					break;
				case "rows":
					RibbonEmitter.Rows = LoadInteger(Loader);
					break;
				case "columns":
					RibbonEmitter.Columns = LoadInteger(Loader);
					break;
				case "materialid":
					Loader.Attacher.AddObject(Model.Materials, RibbonEmitter.Material, LoadId(Loader));
					break;
				default:
					throw new Exception("Syntax error at line " + Loader.Line + ", unknown tag \"" + text + "\"!");
				}
			}
			Loader.ReadToken();
		}

		public void SaveAll(CSaver Saver, MdxLib.Model.CModel Model)
		{
			if (!Model.HasRibbonEmitters)
			{
				return;
			}
			foreach (MdxLib.Model.CRibbonEmitter ribbonEmitter in Model.RibbonEmitters)
			{
				Save(Saver, Model, ribbonEmitter);
			}
		}

		public void Save(CSaver Saver, MdxLib.Model.CModel Model, MdxLib.Model.CRibbonEmitter RibbonEmitter)
		{
			Saver.BeginGroup("RibbonEmitter", RibbonEmitter.Name);
			SaveNode(Saver, Model, RibbonEmitter);
			SaveInteger(Saver, "EmissionRate", RibbonEmitter.EmissionRate);
			SaveFloat(Saver, "LifeSpan", RibbonEmitter.LifeSpan);
			SaveFloat(Saver, "Gravity", RibbonEmitter.Gravity, ECondition.NotZero);
			SaveInteger(Saver, "Rows", RibbonEmitter.Rows);
			SaveInteger(Saver, "Columns", RibbonEmitter.Columns);
			SaveId(Saver, "MaterialID", RibbonEmitter.Material.ObjectId, ECondition.NotInvalidId);
			SaveAnimator(Saver, Model, RibbonEmitter.HeightAbove, CFloat.Instance, "HeightAbove");
			SaveAnimator(Saver, Model, RibbonEmitter.HeightBelow, CFloat.Instance, "HeightBelow");
			SaveAnimator(Saver, Model, RibbonEmitter.Alpha, CFloat.Instance, "Alpha");
			SaveAnimator(Saver, Model, RibbonEmitter.Color, CColor.Instance, "Color");
			SaveAnimator(Saver, Model, RibbonEmitter.TextureSlot, CInteger.Instance, "TextureSlot");
			SaveAnimator(Saver, Model, RibbonEmitter.Visibility, CFloat.Instance, "Visibility", ECondition.NotOne);
			Saver.EndGroup();
		}
	}
}
