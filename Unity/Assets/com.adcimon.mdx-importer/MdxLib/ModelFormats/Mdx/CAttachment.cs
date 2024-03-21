using System;
using MdxLib.Model;
using MdxLib.ModelFormats.Mdx.Value;

namespace MdxLib.ModelFormats.Mdx
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

		public void LoadAll(CLoader Loader, MdxLib.Model.CModel Model)
		{
			int num = Loader.ReadInt32();
			while (num > 0)
			{
				Loader.PushLocation();
				MdxLib.Model.CAttachment cAttachment = new MdxLib.Model.CAttachment(Model);
				Load(Loader, Model, cAttachment);
				Model.Attachments.Add(cAttachment);
				num -= Loader.PopLocation();
				if (num < 0)
				{
					throw new Exception("Error at location " + Loader.Location + ", too many Attachment bytes were read!");
				}
			}
		}

		public void Load(CLoader Loader, MdxLib.Model.CModel Model, MdxLib.Model.CAttachment Attachment)
		{
			Loader.PushLocation();
			int num = Loader.ReadInt32();
			CNode.LoadNode(Loader, Model, Attachment);
			Attachment.Path = Loader.ReadString(260);
			Attachment.AttachmentId = Loader.ReadInt32();
			num -= Loader.PopLocation();
			if (num < 0)
			{
				throw new Exception("Error at location " + Loader.Location + ", too many Attachment bytes were read!");
			}
			while (num > 0)
			{
				Loader.PushLocation();
				string text = Loader.ReadTag();
				string text2;
				if ((text2 = text) != null && text2 == "KATV")
				{
					CObject.LoadAnimator(Loader, Model, Attachment.Visibility, CFloat.Instance);
					num -= Loader.PopLocation();
					if (num < 0)
					{
						throw new Exception("Error at location " + Loader.Location + ", too many Attachment bytes were read!");
					}
					continue;
				}
				throw new Exception("Error at location " + Loader.Location + ", unknown Attachment tag \"" + text + "\"!");
			}
		}

		public void SaveAll(CSaver Saver, MdxLib.Model.CModel Model)
		{
			if (!Model.HasAttachments)
			{
				return;
			}
			Saver.WriteTag("ATCH");
			Saver.PushLocation();
			foreach (MdxLib.Model.CAttachment attachment in Model.Attachments)
			{
				Save(Saver, Model, attachment);
			}
			Saver.PopExclusiveLocation();
		}

		public void Save(CSaver Saver, MdxLib.Model.CModel Model, MdxLib.Model.CAttachment Attachment)
		{
			Saver.PushLocation();
			CNode.SaveNode(Saver, Model, Attachment, 2048);
			Saver.WriteString(Attachment.Path, 260);
			Saver.WriteInt32(Attachment.AttachmentId);
			CObject.SaveAnimator(Saver, Model, Attachment.Visibility, CFloat.Instance, "KATV");
			Saver.PopInclusiveLocation();
		}
	}
}
