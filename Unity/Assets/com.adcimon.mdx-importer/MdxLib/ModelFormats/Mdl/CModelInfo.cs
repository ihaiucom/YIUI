using System;
using MdxLib.Model;
using MdxLib.ModelFormats.Mdl.Token;
using MdxLib.Primitives;

namespace MdxLib.ModelFormats.Mdl
{
	internal sealed class CModelInfo : CObject
	{
		private static class CSingleton
		{
			public static CModelInfo Instance;

			static CSingleton()
			{
				Instance = new CModelInfo();
			}
		}

		public static CModelInfo Instance => CSingleton.Instance;

		private CModelInfo()
		{
		}

		public void Load(CLoader Loader, MdxLib.Model.CModel Model)
		{
			float radius = 0f;
			CVector3 min = CConstants.DefaultVector3;
			CVector3 max = CConstants.DefaultVector3;
			Model.Name = Loader.ReadString();
			Loader.ExpectToken(EType.CurlyBracketLeft);
			while (Loader.PeekToken() != EType.CurlyBracketRight)
			{
				string text = Loader.ReadWord();
				switch (text)
				{
				case "formatversion":
					Model.Version = LoadInteger(Loader);
					break;
				case "blendtime":
					Model.BlendTime = LoadInteger(Loader);
					break;
				case "minimumextent":
					min = LoadVector3(Loader);
					break;
				case "maximumextent":
					max = LoadVector3(Loader);
					break;
				case "boundsradius":
					radius = LoadFloat(Loader);
					break;
				case "animationfile":
					Model.AnimationFile = LoadString(Loader);
					break;
				case "numgeosets":
					LoadInteger(Loader);
					break;
				case "numgeosetanims":
					LoadInteger(Loader);
					break;
				case "numhelpers":
					LoadInteger(Loader);
					break;
				case "numlights":
					LoadInteger(Loader);
					break;
				case "numbones":
					LoadInteger(Loader);
					break;
				case "numattachments":
					LoadInteger(Loader);
					break;
				case "numparticleemitters":
					LoadInteger(Loader);
					break;
				case "numparticleemitters2":
					LoadInteger(Loader);
					break;
				case "numribbonemitters":
					LoadInteger(Loader);
					break;
				case "numevents":
					LoadInteger(Loader);
					break;
				default:
					throw new Exception("Syntax error at line " + Loader.Line + ", unknown tag \"" + text + "\"!");
				}
				Model.Extent = new CExtent(min, max, radius);
			}
			Loader.ReadToken();
		}

		public void Save(CSaver Saver, MdxLib.Model.CModel Model)
		{
			Saver.BeginGroup("Model", Model.Name);
			SaveInteger(Saver, "NumGeosets", Model.Geosets.Count, ECondition.NotZero);
			SaveInteger(Saver, "NumGeosetAnims", Model.GeosetAnimations.Count, ECondition.NotZero);
			SaveInteger(Saver, "NumHelpers", Model.Helpers.Count, ECondition.NotZero);
			SaveInteger(Saver, "NumLights", Model.Lights.Count, ECondition.NotZero);
			SaveInteger(Saver, "NumBones", Model.Bones.Count, ECondition.NotZero);
			SaveInteger(Saver, "NumAttachments", Model.Attachments.Count, ECondition.NotZero);
			SaveInteger(Saver, "NumParticleEmitters", Model.ParticleEmitters.Count, ECondition.NotZero);
			SaveInteger(Saver, "NumParticleEmitters2", Model.ParticleEmitters2.Count, ECondition.NotZero);
			SaveInteger(Saver, "NumRibbonEmitters", Model.RibbonEmitters.Count, ECondition.NotZero);
			SaveInteger(Saver, "NumEvents", Model.Events.Count, ECondition.NotZero);
			SaveInteger(Saver, "BlendTime", Model.BlendTime);
			SaveVector3(Saver, "MinimumExtent", Model.Extent.Min, ECondition.NotZero);
			SaveVector3(Saver, "MaximumExtent", Model.Extent.Max, ECondition.NotZero);
			SaveFloat(Saver, "BoundsRadius", Model.Extent.Radius, ECondition.NotZero);
			SaveString(Saver, "AnimationFile", Model.AnimationFile, ECondition.NotEmpty);
			Saver.EndGroup();
		}
	}
}
