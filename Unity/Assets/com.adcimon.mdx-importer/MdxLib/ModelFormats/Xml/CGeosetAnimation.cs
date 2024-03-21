using System.Xml;
using MdxLib.Model;
using MdxLib.ModelFormats.Xml.Value;

namespace MdxLib.ModelFormats.Xml
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

		public void Load(CLoader Loader, XmlNode Node, MdxLib.Model.CModel Model, MdxLib.Model.CGeosetAnimation GeosetAnimation)
		{
			GeosetAnimation.UseColor = ReadBoolean(Node, "use_color", GeosetAnimation.UseColor);
			GeosetAnimation.DropShadow = ReadBoolean(Node, "drop_shadow", GeosetAnimation.DropShadow);
			LoadAnimator(Loader, Node, Model, GeosetAnimation.Color, CVector3.Instance, "color");
			LoadAnimator(Loader, Node, Model, GeosetAnimation.Alpha, CFloat.Instance, "alpha");
			Loader.Attacher.AddObject(Model.Geosets, GeosetAnimation.Geoset, ReadInteger(Node, "geoset", -1));
		}

		public void Save(CSaver Saver, XmlNode Node, MdxLib.Model.CModel Model, MdxLib.Model.CGeosetAnimation GeosetAnimation)
		{
			WriteBoolean(Node, "use_color", GeosetAnimation.UseColor);
			WriteBoolean(Node, "drop_shadow", GeosetAnimation.DropShadow);
			SaveAnimator(Saver, Node, Model, GeosetAnimation.Color, CVector3.Instance, "color");
			SaveAnimator(Saver, Node, Model, GeosetAnimation.Alpha, CFloat.Instance, "alpha");
			WriteInteger(Node, "geoset", GeosetAnimation.Geoset.ObjectId);
		}
	}
}
