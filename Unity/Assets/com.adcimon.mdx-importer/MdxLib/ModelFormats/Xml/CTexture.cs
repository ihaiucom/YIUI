using System.Xml;
using MdxLib.Model;

namespace MdxLib.ModelFormats.Xml
{
	internal sealed class CTexture : CObject
	{
		private static class CSingleton
		{
			public static CTexture Instance;

			static CSingleton()
			{
				Instance = new CTexture();
			}
		}

		public static CTexture Instance => CSingleton.Instance;

		private CTexture()
		{
		}

		public void Load(CLoader Loader, XmlNode Node, MdxLib.Model.CModel Model, MdxLib.Model.CTexture Texture)
		{
			Texture.FileName = ReadString(Node, "filename", Texture.FileName);
			Texture.ReplaceableId = ReadInteger(Node, "replaceable_id", Texture.ReplaceableId);
			Texture.WrapWidth = ReadBoolean(Node, "wrap_width", Texture.WrapWidth);
			Texture.WrapHeight = ReadBoolean(Node, "wrap_height", Texture.WrapHeight);
		}

		public void Save(CSaver Saver, XmlNode Node, MdxLib.Model.CModel Model, MdxLib.Model.CTexture Texture)
		{
			WriteString(Node, "filename", Texture.FileName);
			WriteInteger(Node, "replaceable_id", Texture.ReplaceableId);
			WriteBoolean(Node, "wrap_width", Texture.WrapWidth);
			WriteBoolean(Node, "wrap_height", Texture.WrapHeight);
		}
	}
}
