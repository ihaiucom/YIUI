using System;
using MdxLib.Model;
using MdxLib.ModelFormats.Mdl.Token;
using MdxLib.ModelFormats.Mdl.Value;

namespace MdxLib.ModelFormats.Mdl
{
	internal sealed class CGeosetAnimation : CObject
	{
		private static class CSingleton
		{
			public static CGeosetAnimation Instance;

			static CSingleton()
			{
				Instance = new CGeosetAnimation();
			}
		}

		public static CGeosetAnimation Instance => CSingleton.Instance;

		private CGeosetAnimation()
		{
		}

		public void LoadAll(CLoader Loader, MdxLib.Model.CModel Model)
		{
			MdxLib.Model.CGeosetAnimation cGeosetAnimation = new MdxLib.Model.CGeosetAnimation(Model);
			Load(Loader, Model, cGeosetAnimation);
			Model.GeosetAnimations.Add(cGeosetAnimation);
		}

		public void Load(CLoader Loader, MdxLib.Model.CModel Model, MdxLib.Model.CGeosetAnimation GeosetAnimation)
		{
			Loader.ExpectToken(EType.CurlyBracketLeft);
			while (Loader.PeekToken() != EType.CurlyBracketRight)
			{
				string text = Loader.ReadWord();
				switch (text)
				{
				case "static":
					text = Loader.ReadWord();
					switch (text)
					{
					case "alpha":
						LoadStaticAnimator(Loader, Model, GeosetAnimation.Alpha, CFloat.Instance);
						break;
					case "color":
						LoadStaticAnimator(Loader, Model, GeosetAnimation.Color, CColor.Instance);
						GeosetAnimation.UseColor = true;
						break;
					default:
						throw new Exception("Syntax error at line " + Loader.Line + ", unknown tag \"" + text + "\"!");
					}
					break;
				case "alpha":
					LoadAnimator(Loader, Model, GeosetAnimation.Alpha, CFloat.Instance);
					break;
				case "color":
					LoadAnimator(Loader, Model, GeosetAnimation.Color, CColor.Instance);
					break;
				case "geosetid":
					Loader.Attacher.AddObject(Model.Geosets, GeosetAnimation.Geoset, LoadId(Loader));
					break;
				case "dropshadow":
					GeosetAnimation.DropShadow = LoadBoolean(Loader);
					break;
				default:
					throw new Exception("Syntax error at line " + Loader.Line + ", unknown tag \"" + text + "\"!");
				}
			}
			Loader.ReadToken();
		}

		public void SaveAll(CSaver Saver, MdxLib.Model.CModel Model)
		{
			if (!Model.HasGeosetAnimations)
			{
				return;
			}
			foreach (MdxLib.Model.CGeosetAnimation geosetAnimation in Model.GeosetAnimations)
			{
				Save(Saver, Model, geosetAnimation);
			}
		}

		public void Save(CSaver Saver, MdxLib.Model.CModel Model, MdxLib.Model.CGeosetAnimation GeosetAnimation)
		{
			Saver.BeginGroup("GeosetAnim");
			SaveAnimator(Saver, Model, GeosetAnimation.Alpha, CFloat.Instance, "Alpha", ECondition.NotOne);
			if (GeosetAnimation.UseColor)
			{
				SaveAnimator(Saver, Model, GeosetAnimation.Color, CColor.Instance, "Color", ECondition.NotOne);
			}
			SaveId(Saver, "GeosetId", GeosetAnimation.Geoset.ObjectId, ECondition.NotInvalidId);
			SaveBoolean(Saver, "DropShadow", GeosetAnimation.DropShadow);
			Saver.EndGroup();
		}
	}
}
