using System.Xml;
using MdxLib.Model;
using MdxLib.ModelFormats.Xml.Value;

namespace MdxLib.ModelFormats.Xml
{
	internal sealed class CAttachment : CNode
	{
		private static class CSingleton
		{
			public static CAttachment Instance;

			static CSingleton()
			{
				Instance = new CAttachment();
			}
		}

		public static CAttachment Instance => CSingleton.Instance;

		private CAttachment()
		{
		}

		public void Load(CLoader Loader, XmlNode Node, MdxLib.Model.CModel Model, MdxLib.Model.CAttachment Attachment)
		{
			LoadNode(Loader, Node, Model, Attachment);
			Attachment.Path = ReadString(Node, "path", Attachment.Path);
			Attachment.AttachmentId = ReadInteger(Node, "attachment_id", Attachment.AttachmentId);
			LoadAnimator(Loader, Node, Model, Attachment.Visibility, CFloat.Instance, "visibility");
		}

		public void Save(CSaver Saver, XmlNode Node, MdxLib.Model.CModel Model, MdxLib.Model.CAttachment Attachment)
		{
			SaveNode(Saver, Node, Model, Attachment);
			WriteString(Node, "path", Attachment.Path);
			WriteInteger(Node, "attachment_id", Attachment.AttachmentId);
			SaveAnimator(Saver, Node, Model, Attachment.Visibility, CFloat.Instance, "visibility");
		}
	}
}
