using System.Xml;
using MdxLib.Model;
using MdxLib.ModelFormats.Xml.Value;

namespace MdxLib.ModelFormats.Xml
{
	internal sealed class CTextureAnimation : CObject
	{
		private static class CSingleton
		{
			public static CTextureAnimation Instance;

			static CSingleton()
			{
				Instance = new CTextureAnimation();
			}
		}

		public static CTextureAnimation Instance => CSingleton.Instance;

		private CTextureAnimation()
		{
		}

		public void Load(CLoader Loader, XmlNode Node, MdxLib.Model.CModel Model, MdxLib.Model.CTextureAnimation TextureAnimation)
		{
			LoadAnimator(Loader, Node, Model, TextureAnimation.Translation, CVector3.Instance, "translation");
			LoadAnimator(Loader, Node, Model, TextureAnimation.Rotation, CVector4.Instance, "rotation");
			LoadAnimator(Loader, Node, Model, TextureAnimation.Scaling, CVector3.Instance, "scaling");
		}

		public void Save(CSaver Saver, XmlNode Node, MdxLib.Model.CModel Model, MdxLib.Model.CTextureAnimation TextureAnimation)
		{
			SaveAnimator(Saver, Node, Model, TextureAnimation.Translation, CVector3.Instance, "translation");
			SaveAnimator(Saver, Node, Model, TextureAnimation.Rotation, CVector4.Instance, "rotation");
			SaveAnimator(Saver, Node, Model, TextureAnimation.Scaling, CVector3.Instance, "scaling");
		}
	}
}
