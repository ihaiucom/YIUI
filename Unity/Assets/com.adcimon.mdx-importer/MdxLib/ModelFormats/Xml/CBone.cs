using System.Xml;
using MdxLib.Model;

namespace MdxLib.ModelFormats.Xml
{
	internal sealed class CBone : CNode
	{
		private static class CSingleton
		{
			public static CBone Instance;

			static CSingleton()
			{
				Instance = new CBone();
			}
		}

		public static CBone Instance => CSingleton.Instance;

		private CBone()
		{
		}

		public void Load(CLoader Loader, XmlNode Node, MdxLib.Model.CModel Model, MdxLib.Model.CBone Bone)
		{
			LoadNode(Loader, Node, Model, Bone);
			Loader.Attacher.AddObject(Model.Geosets, Bone.Geoset, ReadInteger(Node, "geoset", -1));
			Loader.Attacher.AddObject(Model.GeosetAnimations, Bone.GeosetAnimation, ReadInteger(Node, "geoset_animation", -1));
		}

		public void Save(CSaver Saver, XmlNode Node, MdxLib.Model.CModel Model, MdxLib.Model.CBone Bone)
		{
			SaveNode(Saver, Node, Model, Bone);
			WriteInteger(Node, "geoset", Bone.Geoset.ObjectId);
			WriteInteger(Node, "geoset_animation", Bone.GeosetAnimation.ObjectId);
		}
	}
}
