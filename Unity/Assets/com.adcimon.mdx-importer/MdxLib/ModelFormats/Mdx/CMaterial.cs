using System;
using MdxLib.Model;
using MdxLib.ModelFormats.Mdx.Value;

namespace MdxLib.ModelFormats.Mdx
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
			int num = Loader.ReadInt32();
			while (num > 0)
			{
				Loader.PushLocation();
				MdxLib.Model.CMaterial cMaterial = new MdxLib.Model.CMaterial(Model);
				Load(Loader, Model, cMaterial);
				Model.Materials.Add(cMaterial);
				num -= Loader.PopLocation();
				if (num < 0)
				{
					throw new Exception("Error at location " + Loader.Location + ", too many Material bytes were read!");
				}
			}
		}

		public void Load(CLoader Loader, MdxLib.Model.CModel Model, MdxLib.Model.CMaterial Material)
		{
			Loader.PushLocation();
			int num = Loader.ReadInt32();
			Material.PriorityPlane = Loader.ReadInt32();
			int num2 = Loader.ReadInt32();
			Material.ConstantColor = (num2 & 1) != 0;
			Material.SortPrimitivesNearZ = (num2 & 8) != 0;
			Material.SortPrimitivesFarZ = (num2 & 0x10) != 0;
			Material.FullResolution = (num2 & 0x20) != 0;
			num -= Loader.PopLocation();
			if (num < 0)
			{
				throw new Exception("Error at location " + Loader.Location + ", too many Material bytes were read!");
			}
			Loader.ExpectTag("LAYS");
			int num3 = Loader.ReadInt32();
			for (int i = 0; i < num3; i++)
			{
				CMaterialLayer cMaterialLayer = new CMaterialLayer(Model);
				LoadLayer(Loader, Model, Material, cMaterialLayer);
				Material.Layers.Add(cMaterialLayer);
			}
		}

		public void LoadLayer(CLoader Loader, MdxLib.Model.CModel Model, MdxLib.Model.CMaterial Material, CMaterialLayer Layer)
		{
			Loader.PushLocation();
			int num = Loader.ReadInt32();
			int num2 = Loader.ReadInt32();
			int num3 = Loader.ReadInt32();
			int num4 = Loader.ReadInt32();
			Layer.TextureId.MakeStatic(num4);
			Loader.Attacher.AddObject(Model.Textures, Layer.Texture, num4);
			Loader.Attacher.AddObject(Model.TextureAnimations, Layer.TextureAnimation, Loader.ReadInt32());
			Layer.CoordId = Loader.ReadInt32();
			Layer.Alpha.MakeStatic(Loader.ReadFloat());
			switch (num2)
			{
			case 0:
				Layer.FilterMode = EMaterialLayerFilterMode.None;
				break;
			case 1:
				Layer.FilterMode = EMaterialLayerFilterMode.Transparent;
				break;
			case 2:
				Layer.FilterMode = EMaterialLayerFilterMode.Blend;
				break;
			case 3:
				Layer.FilterMode = EMaterialLayerFilterMode.Additive;
				break;
			case 4:
				Layer.FilterMode = EMaterialLayerFilterMode.AdditiveAlpha;
				break;
			case 5:
				Layer.FilterMode = EMaterialLayerFilterMode.Modulate;
				break;
			case 6:
				Layer.FilterMode = EMaterialLayerFilterMode.Modulate2x;
				break;
			}
			Layer.Unshaded = (num3 & 1) != 0;
			Layer.SphereEnvironmentMap = (num3 & 2) != 0;
			Layer.TwoSided = (num3 & 0x10) != 0;
			Layer.Unfogged = (num3 & 0x20) != 0;
			Layer.NoDepthTest = (num3 & 0x40) != 0;
			Layer.NoDepthSet = (num3 & 0x80) != 0;
			num -= Loader.PopLocation();
			if (num < 0)
			{
				throw new Exception("Error at location " + Loader.Location + ", too many MaterialLayer bytes were read!");
			}
			while (num > 0)
			{
				Loader.PushLocation();
				string text = Loader.ReadTag();
				switch (text)
				{
				case "KMTF":
					CObject.LoadAnimator(Loader, Model, Layer.TextureId, CInteger.Instance);
					break;
				case "KMTA":
					CObject.LoadAnimator(Loader, Model, Layer.Alpha, CFloat.Instance);
					break;
				default:
					throw new Exception("Error at location " + Loader.Location + ", unknown MaterialLayer tag \"" + text + "\"!");
				}
				num -= Loader.PopLocation();
				if (num < 0)
				{
					throw new Exception("Error at location " + Loader.Location + ", too many MaterialLayer bytes were read!");
				}
			}
		}

		public void SaveAll(CSaver Saver, MdxLib.Model.CModel Model)
		{
			if (!Model.HasMaterials)
			{
				return;
			}
			Saver.WriteTag("MTLS");
			Saver.PushLocation();
			foreach (MdxLib.Model.CMaterial material in Model.Materials)
			{
				Save(Saver, Model, material);
			}
			Saver.PopExclusiveLocation();
		}

		public void Save(CSaver Saver, MdxLib.Model.CModel Model, MdxLib.Model.CMaterial Material)
		{
			int num = 0;
			if (Material.ConstantColor)
			{
				num |= 1;
			}
			if (Material.SortPrimitivesNearZ)
			{
				num |= 8;
			}
			if (Material.SortPrimitivesFarZ)
			{
				num |= 0x10;
			}
			if (Material.FullResolution)
			{
				num |= 0x20;
			}
			Saver.PushLocation();
			Saver.WriteInt32(Material.PriorityPlane);
			Saver.WriteInt32(num);
			Saver.WriteTag("LAYS");
			Saver.WriteInt32(Material.Layers.Count);
			foreach (CMaterialLayer layer in Material.Layers)
			{
				SaveLayer(Saver, Model, Material, layer);
			}
			Saver.PopInclusiveLocation();
		}

		public void SaveLayer(CSaver Saver, MdxLib.Model.CModel Model, MdxLib.Model.CMaterial Material, CMaterialLayer Layer)
		{
			int num = 0;
			int value = 0;
			if (Layer.Unshaded)
			{
				num |= 1;
			}
			if (Layer.SphereEnvironmentMap)
			{
				num |= 2;
			}
			if (Layer.TwoSided)
			{
				num |= 0x10;
			}
			if (Layer.Unfogged)
			{
				num |= 0x20;
			}
			if (Layer.NoDepthTest)
			{
				num |= 0x40;
			}
			if (Layer.NoDepthSet)
			{
				num |= 0x80;
			}
			switch (Layer.FilterMode)
			{
			case EMaterialLayerFilterMode.None:
				value = 0;
				break;
			case EMaterialLayerFilterMode.Transparent:
				value = 1;
				break;
			case EMaterialLayerFilterMode.Blend:
				value = 2;
				break;
			case EMaterialLayerFilterMode.Additive:
				value = 3;
				break;
			case EMaterialLayerFilterMode.AdditiveAlpha:
				value = 4;
				break;
			case EMaterialLayerFilterMode.Modulate:
				value = 5;
				break;
			case EMaterialLayerFilterMode.Modulate2x:
				value = 6;
				break;
			}
			Saver.PushLocation();
			Saver.WriteInt32(value);
			Saver.WriteInt32(num);
			Saver.WriteInt32(Layer.Texture.ObjectId);
			Saver.WriteInt32(Layer.TextureAnimation.ObjectId);
			Saver.WriteInt32(Layer.CoordId);
			Saver.WriteFloat(Layer.Alpha.GetValue());
			CObject.SaveAnimator(Saver, Model, Layer.TextureId, CInteger.Instance, "KMTF");
			CObject.SaveAnimator(Saver, Model, Layer.Alpha, CFloat.Instance, "KMTA");
			Saver.PopInclusiveLocation();
		}
	}
}
