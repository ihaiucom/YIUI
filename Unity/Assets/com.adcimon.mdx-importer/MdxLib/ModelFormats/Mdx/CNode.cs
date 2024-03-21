using System;
using MdxLib.Model;
using MdxLib.ModelFormats.Mdx.Value;

namespace MdxLib.ModelFormats.Mdx
{
	internal abstract class CNode : CObject
	{
		public CNode()
		{
		}

		public static int LoadNode<T>(CLoader Loader, MdxLib.Model.CModel Model, CNode<T> Node) where T : CNode<T>
		{
			Loader.PushLocation();
			int num = Loader.ReadInt32();
			Node.Name = Loader.ReadString(80);
			Loader.ReadInt32();
			Loader.Attacher.AddNode(Model, Node.Parent, Loader.ReadInt32());
			int num2 = Loader.ReadInt32();
			Node.DontInheritTranslation = (num2 & 1) != 0;
			Node.DontInheritRotation = (num2 & 2) != 0;
			Node.DontInheritScaling = (num2 & 4) != 0;
			Node.Billboarded = (num2 & 8) != 0;
			Node.BillboardedLockX = (num2 & 0x10) != 0;
			Node.BillboardedLockY = (num2 & 0x20) != 0;
			Node.BillboardedLockZ = (num2 & 0x40) != 0;
			Node.CameraAnchored = (num2 & 0x80) != 0;
			num -= Loader.PopLocation();
			if (num < 0)
			{
				throw new Exception("Error at location " + Loader.Location + ", too many Node bytes were read!");
			}
			while (num > 0)
			{
				Loader.PushLocation();
				string text = Loader.ReadTag();
				switch (text)
				{
				case "KGTR":
					CObject.LoadAnimator(Loader, Model, Node.Translation, CVector3.Instance);
					break;
				case "KGRT":
					CObject.LoadAnimator(Loader, Model, Node.Rotation, CVector4.Instance);
					break;
				case "KGSC":
					CObject.LoadAnimator(Loader, Model, Node.Scaling, CVector3.Instance);
					break;
				default:
					throw new Exception("Error at location " + Loader.Location + ", unknown tag \"" + text + "\"!");
				}
				num -= Loader.PopLocation();
				if (num < 0)
				{
					throw new Exception("Error at location " + Loader.Location + ", too many Node bytes were read!");
				}
			}
			return num2;
		}

		public static void SaveNode<T>(CSaver Saver, MdxLib.Model.CModel Model, CNode<T> Node, int Flags) where T : CNode<T>
		{
			if (Node.DontInheritTranslation)
			{
				Flags |= 1;
			}
			if (Node.DontInheritRotation)
			{
				Flags |= 2;
			}
			if (Node.DontInheritScaling)
			{
				Flags |= 4;
			}
			if (Node.Billboarded)
			{
				Flags |= 8;
			}
			if (Node.BillboardedLockX)
			{
				Flags |= 0x10;
			}
			if (Node.BillboardedLockY)
			{
				Flags |= 0x20;
			}
			if (Node.BillboardedLockZ)
			{
				Flags |= 0x40;
			}
			if (Node.CameraAnchored)
			{
				Flags |= 0x80;
			}
			Saver.PushLocation();
			Saver.WriteString(Node.Name, 80);
			Saver.WriteInt32(Node.NodeId);
			Saver.WriteInt32(Node.Parent.NodeId);
			Saver.WriteInt32(Flags);
			CObject.SaveAnimator(Saver, Model, Node.Translation, CVector3.Instance, "KGTR");
			CObject.SaveAnimator(Saver, Model, Node.Rotation, CVector4.Instance, "KGRT");
			CObject.SaveAnimator(Saver, Model, Node.Scaling, CVector3.Instance, "KGSC");
			Saver.PopInclusiveLocation();
		}
	}
}
