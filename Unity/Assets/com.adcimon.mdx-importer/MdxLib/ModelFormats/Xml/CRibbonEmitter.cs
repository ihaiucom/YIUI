using System.Xml;
using MdxLib.Model;
using MdxLib.ModelFormats.Xml.Value;

namespace MdxLib.ModelFormats.Xml
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

		public void Load(CLoader Loader, XmlNode Node, MdxLib.Model.CModel Model, MdxLib.Model.CRibbonEmitter RibbonEmitter)
		{
			LoadNode(Loader, Node, Model, RibbonEmitter);
			RibbonEmitter.EmissionRate = ReadInteger(Node, "emission_rate", RibbonEmitter.EmissionRate);
			RibbonEmitter.LifeSpan = ReadFloat(Node, "life_span", RibbonEmitter.LifeSpan);
			RibbonEmitter.Gravity = ReadFloat(Node, "gravity", RibbonEmitter.Gravity);
			RibbonEmitter.Rows = ReadInteger(Node, "rows", RibbonEmitter.Rows);
			RibbonEmitter.Columns = ReadInteger(Node, "columns", RibbonEmitter.Columns);
			LoadAnimator(Loader, Node, Model, RibbonEmitter.HeightAbove, CFloat.Instance, "height_above");
			LoadAnimator(Loader, Node, Model, RibbonEmitter.HeightBelow, CFloat.Instance, "height_below");
			LoadAnimator(Loader, Node, Model, RibbonEmitter.Alpha, CFloat.Instance, "alpha");
			LoadAnimator(Loader, Node, Model, RibbonEmitter.Color, CVector3.Instance, "color");
			LoadAnimator(Loader, Node, Model, RibbonEmitter.TextureSlot, CInteger.Instance, "texture_slot");
			LoadAnimator(Loader, Node, Model, RibbonEmitter.Visibility, CFloat.Instance, "visibility");
			Loader.Attacher.AddObject(Model.Materials, RibbonEmitter.Material, ReadInteger(Node, "material", -1));
		}

		public void Save(CSaver Saver, XmlNode Node, MdxLib.Model.CModel Model, MdxLib.Model.CRibbonEmitter RibbonEmitter)
		{
			SaveNode(Saver, Node, Model, RibbonEmitter);
			WriteInteger(Node, "emission_rate", RibbonEmitter.EmissionRate);
			WriteFloat(Node, "life_span", RibbonEmitter.LifeSpan);
			WriteFloat(Node, "gravity", RibbonEmitter.Gravity);
			WriteInteger(Node, "rows", RibbonEmitter.Rows);
			WriteInteger(Node, "columns", RibbonEmitter.Columns);
			SaveAnimator(Saver, Node, Model, RibbonEmitter.HeightAbove, CFloat.Instance, "height_above");
			SaveAnimator(Saver, Node, Model, RibbonEmitter.HeightBelow, CFloat.Instance, "height_below");
			SaveAnimator(Saver, Node, Model, RibbonEmitter.Alpha, CFloat.Instance, "alpha");
			SaveAnimator(Saver, Node, Model, RibbonEmitter.Color, CVector3.Instance, "color");
			SaveAnimator(Saver, Node, Model, RibbonEmitter.TextureSlot, CInteger.Instance, "texture_slot");
			SaveAnimator(Saver, Node, Model, RibbonEmitter.Visibility, CFloat.Instance, "visibility");
			WriteInteger(Node, "material", RibbonEmitter.Material.ObjectId);
		}
	}
}
