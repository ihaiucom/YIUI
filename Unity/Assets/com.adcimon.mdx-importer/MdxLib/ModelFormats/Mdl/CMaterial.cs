using System;
using MdxLib.Model;
using MdxLib.ModelFormats.Mdl.Token;
using MdxLib.ModelFormats.Mdl.Value;

namespace MdxLib.ModelFormats.Mdl
{
	internal sealed class CMaterial : CObject
	{
		private static class CSingleton
		{
			public static CMaterial Instance;

			static CSingleton()
			{
				Instance = new CMaterial();
			}
		}

		public static CMaterial Instance => CSingleton.Instance;

		private CMaterial()
		{
		}

		public void LoadAll(CLoader Loader, MdxLib.Model.CModel Model)
		{
			Loader.ReadInteger();
			Loader.ExpectToken(EType.CurlyBracketLeft);
			string text;
			while (true)
			{
				if (Loader.PeekToken() == EType.CurlyBracketRight)
				{
					Loader.ReadToken();
					return;
				}
				text = Loader.ReadWord();
				string text2;
				if ((text2 = text) == null || !(text2 == "material"))
				{
					break;
				}
				MdxLib.Model.CMaterial cMaterial = new MdxLib.Model.CMaterial(Model);
				Load(Loader, Model, cMaterial);
				Model.Materials.Add(cMaterial);
			}
			throw new Exception("Syntax error at line " + Loader.Line + ", unknown tag \"" + text + "\"!");
		}

		public void Load(CLoader Loader, MdxLib.Model.CModel Model, MdxLib.Model.CMaterial Material)
		{
			Loader.ExpectToken(EType.CurlyBracketLeft);
			while (Loader.PeekToken() != EType.CurlyBracketRight)
			{
				string text = Loader.ReadWord();
				switch (text)
				{
				case "priorityplane":
					Material.PriorityPlane = LoadInteger(Loader);
					break;
				case "constantcolor":
					Material.ConstantColor = LoadBoolean(Loader);
					break;
				case "sortprimsnearz":
					Material.SortPrimitivesNearZ = LoadBoolean(Loader);
					break;
				case "sortprimsfarz":
					Material.SortPrimitivesFarZ = LoadBoolean(Loader);
					break;
				case "fullresolution":
					Material.FullResolution = LoadBoolean(Loader);
					break;
				case "layer":
				{
					CMaterialLayer cMaterialLayer = new CMaterialLayer(Model);
					Loader.ExpectToken(EType.CurlyBracketLeft);
					while (Loader.PeekToken() != EType.CurlyBracketRight)
					{
						text = Loader.ReadWord();
						switch (text)
						{
						case "static":
							text = Loader.ReadWord();
							switch (text)
							{
							case "textureid":
								LoadStaticAnimator(Loader, Model, cMaterialLayer.TextureId, CInteger.Instance);
								Loader.Attacher.AddObject(Model.Textures, cMaterialLayer.Texture, cMaterialLayer.TextureId.GetValue());
								break;
							case "alpha":
								LoadStaticAnimator(Loader, Model, cMaterialLayer.Alpha, CFloat.Instance);
								break;
							default:
								throw new Exception("Syntax error at line " + Loader.Line + ", unknown tag \"" + text + "\"!");
							}
							break;
						case "textureid":
							LoadAnimator(Loader, Model, cMaterialLayer.TextureId, CInteger.Instance);
							break;
						case "alpha":
							LoadAnimator(Loader, Model, cMaterialLayer.Alpha, CFloat.Instance);
							break;
						case "tvertexanimid":
							Loader.Attacher.AddObject(Model.TextureAnimations, cMaterialLayer.TextureAnimation, LoadInteger(Loader));
							break;
						case "coordid":
							cMaterialLayer.CoordId = LoadInteger(Loader);
							break;
						case "twosided":
							cMaterialLayer.TwoSided = LoadBoolean(Loader);
							break;
						case "unshaded":
							cMaterialLayer.Unshaded = LoadBoolean(Loader);
							break;
						case "unfogged":
							cMaterialLayer.Unfogged = LoadBoolean(Loader);
							break;
						case "sphereenvmap":
							cMaterialLayer.SphereEnvironmentMap = LoadBoolean(Loader);
							break;
						case "nodepthtest":
							cMaterialLayer.NoDepthTest = LoadBoolean(Loader);
							break;
						case "nodepthset":
							cMaterialLayer.NoDepthSet = LoadBoolean(Loader);
							break;
						case "filtermode":
							cMaterialLayer.FilterMode = StringToFilterMode(LoadWord(Loader));
							break;
						default:
							throw new Exception("Syntax error at line " + Loader.Line + ", unknown tag \"" + text + "\"!");
						}
					}
					Loader.ReadToken();
					Material.Layers.Add(cMaterialLayer);
					break;
				}
				default:
					throw new Exception("Syntax error at line " + Loader.Line + ", unknown tag \"" + text + "\"!");
				}
			}
			Loader.ReadToken();
		}

		public void SaveAll(CSaver Saver, MdxLib.Model.CModel Model)
		{
			if (!Model.HasMaterials)
			{
				return;
			}
			Saver.BeginGroup("Materials", Model.Materials.Count);
			foreach (MdxLib.Model.CMaterial material in Model.Materials)
			{
				Save(Saver, Model, material);
			}
			Saver.EndGroup();
		}

		public void Save(CSaver Saver, MdxLib.Model.CModel Model, MdxLib.Model.CMaterial Material)
		{
			Saver.BeginGroup("Material");
			SaveBoolean(Saver, "ConstantColor", Material.ConstantColor);
			SaveBoolean(Saver, "SortPrimsNearZ", Material.SortPrimitivesNearZ);
			SaveBoolean(Saver, "SortPrimsFarZ", Material.SortPrimitivesFarZ);
			SaveBoolean(Saver, "FullResolution", Material.FullResolution);
			SaveInteger(Saver, "PriorityPlane", Material.PriorityPlane, ECondition.NotZero);
			foreach (CMaterialLayer layer in Material.Layers)
			{
				Saver.BeginGroup("Layer");
				Saver.WriteTabs();
				Saver.WriteWord("FilterMode ");
				Saver.WriteWord(FilterModeToString(layer.FilterMode));
				Saver.WriteLine(",");
				if (layer.TextureId.Animated)
				{
					SaveAnimator(Saver, Model, layer.TextureId, CInteger.Instance, "TextureID");
				}
				else
				{
					SaveId(Saver, "static TextureID", layer.Texture.ObjectId, ECondition.NotInvalidId);
				}
				SaveAnimator(Saver, Model, layer.Alpha, CFloat.Instance, "Alpha", ECondition.NotOne);
				SaveBoolean(Saver, "TwoSided", layer.TwoSided);
				SaveBoolean(Saver, "Unshaded", layer.Unshaded);
				SaveBoolean(Saver, "Unfogged", layer.Unfogged);
				SaveBoolean(Saver, "SphereEnvMap", layer.SphereEnvironmentMap);
				SaveBoolean(Saver, "NoDepthTest", layer.NoDepthTest);
				SaveBoolean(Saver, "NoDepthSet", layer.NoDepthSet);
				SaveId(Saver, "TVertexAnimId", layer.TextureAnimation.ObjectId, ECondition.NotInvalidId);
				SaveInteger(Saver, "CoordId", layer.CoordId, ECondition.NotZero);
				Saver.EndGroup();
			}
			Saver.EndGroup();
		}

		private string FilterModeToString(EMaterialLayerFilterMode FilterMode)
		{
			return FilterMode switch
			{
				EMaterialLayerFilterMode.None => "None", 
				EMaterialLayerFilterMode.Transparent => "Transparent", 
				EMaterialLayerFilterMode.Blend => "Blend", 
				EMaterialLayerFilterMode.Additive => "Additive", 
				EMaterialLayerFilterMode.AdditiveAlpha => "AddAlpha", 
				EMaterialLayerFilterMode.Modulate => "Modulate", 
				EMaterialLayerFilterMode.Modulate2x => "Modulate2x", 
				_ => "", 
			};
		}

		private EMaterialLayerFilterMode StringToFilterMode(string String)
		{
			return String switch
			{
				"none" => EMaterialLayerFilterMode.None, 
				"transparent" => EMaterialLayerFilterMode.Transparent, 
				"blend" => EMaterialLayerFilterMode.Blend, 
				"additive" => EMaterialLayerFilterMode.Additive, 
				"addalpha" => EMaterialLayerFilterMode.AdditiveAlpha, 
				"modulate" => EMaterialLayerFilterMode.Modulate, 
				"modulate2x" => EMaterialLayerFilterMode.Modulate2x, 
				_ => EMaterialLayerFilterMode.None, 
			};
		}
	}
}
