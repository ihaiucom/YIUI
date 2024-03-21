using System;
using MdxLib.Model;
using MdxLib.ModelFormats.Mdl.Token;
using MdxLib.ModelFormats.Mdl.Value;

namespace MdxLib.ModelFormats.Mdl
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
			MdxLib.Model.CAttachment cAttachment = new MdxLib.Model.CAttachment(Model);
			Load(Loader, Model, cAttachment);
			Model.Attachments.Add(cAttachment);
		}

		public void Load(CLoader Loader, MdxLib.Model.CModel Model, MdxLib.Model.CAttachment Attachment)
		{
			Attachment.Name = Loader.ReadString();
			Loader.ExpectToken(EType.CurlyBracketLeft);
			while (Loader.PeekToken() != EType.CurlyBracketRight)
			{
				string text = Loader.ReadWord();
				if (LoadNode(Loader, Model, Attachment, text))
				{
					continue;
				}
				switch (text)
				{
				case "static":
					text = Loader.ReadWord();
					if (!LoadStaticNode(Loader, Model, Attachment, text))
					{
						string text2;
						if ((text2 = text) == null || !(text2 == "visibility"))
						{
							throw new Exception("Syntax error at line " + Loader.Line + ", unknown tag \"" + text + "\"!");
						}
						LoadStaticAnimator(Loader, Model, Attachment.Visibility, CFloat.Instance);
					}
					break;
				case "visibility":
					LoadAnimator(Loader, Model, Attachment.Visibility, CFloat.Instance);
					break;
				case "attachmentid":
					Attachment.AttachmentId = LoadInteger(Loader);
					break;
				case "path":
					Attachment.Path = LoadString(Loader);
					break;
				default:
					throw new Exception("Syntax error at line " + Loader.Line + ", unknown tag \"" + text + "\"!");
				}
			}
			Loader.ReadToken();
		}

		public void SaveAll(CSaver Saver, MdxLib.Model.CModel Model)
		{
			if (!Model.HasAttachments)
			{
				return;
			}
			foreach (MdxLib.Model.CAttachment attachment in Model.Attachments)
			{
				Save(Saver, Model, attachment);
			}
		}

		public void Save(CSaver Saver, MdxLib.Model.CModel Model, MdxLib.Model.CAttachment Attachment)
		{
			Saver.BeginGroup("Attachment", Attachment.Name);
			SaveNode(Saver, Model, Attachment);
			SaveId(Saver, "AttachmentID", Attachment.AttachmentId, ECondition.NotInvalidId);
			SaveString(Saver, "Path", Attachment.Path, ECondition.NotEmpty);
			SaveAnimator(Saver, Model, Attachment.Visibility, CFloat.Instance, "Visibility", ECondition.NotOne);
			Saver.EndGroup();
		}
	}
}
