using System;
using MdxLib.Model;
using MdxLib.ModelFormats.Mdl.Token;
using MdxLib.ModelFormats.Mdl.Value;

namespace MdxLib.ModelFormats.Mdl
{
	internal abstract class CNode : CObject
	{
		public CNode()
		{
		}

		public bool LoadNode<T>(CLoader Loader, MdxLib.Model.CModel Model, CNode<T> Node, string Tag) where T : CNode<T>
		{
			switch (Tag)
			{
			case "translation":
				LoadAnimator(Loader, Model, Node.Translation, CVector3.Instance);
				return true;
			case "rotation":
				LoadAnimator(Loader, Model, Node.Rotation, CVector4.Instance);
				return true;
			case "scaling":
				LoadAnimator(Loader, Model, Node.Scaling, CVector3.Instance);
				return true;
			case "objectid":
				LoadInteger(Loader);
				return true;
			case "parent":
				Loader.Attacher.AddNode(Model, Node.Parent, LoadId(Loader));
				return true;
			case "billboarded":
				Node.Billboarded = LoadBoolean(Loader);
				return true;
			case "billboardedlockx":
				Node.BillboardedLockX = LoadBoolean(Loader);
				return true;
			case "billboardedlocky":
				Node.BillboardedLockY = LoadBoolean(Loader);
				return true;
			case "billboardedlockz":
				Node.BillboardedLockZ = LoadBoolean(Loader);
				return true;
			case "cameraanchored":
				Node.CameraAnchored = LoadBoolean(Loader);
				return true;
			case "dontinherit":
				Loader.ExpectToken(EType.CurlyBracketLeft);
				while (Loader.PeekToken() != EType.CurlyBracketRight)
				{
					if (Loader.PeekToken() == EType.Separator)
					{
						Loader.ReadToken();
						continue;
					}
					Tag = Loader.ReadWord();
					switch (Tag)
					{
					case "translation":
						Node.DontInheritTranslation = true;
						continue;
					case "rotation":
						Node.DontInheritRotation = true;
						continue;
					case "scaling":
						Node.DontInheritScaling = true;
						continue;
					}
					throw new Exception("Syntax error at line " + Loader.Line + ", unknown tag \"" + Tag + "\"!");
				}
				Loader.ReadToken();
				Loader.ExpectToken(EType.Separator);
				return true;
			default:
				return false;
			}
		}

		public bool LoadStaticNode<T>(CLoader Loader, MdxLib.Model.CModel Model, CNode<T> Node, string Tag) where T : CNode<T>
		{
			switch (Tag)
			{
			case "translation":
				LoadStaticAnimator(Loader, Model, Node.Translation, CVector3.Instance);
				return true;
			case "rotation":
				LoadStaticAnimator(Loader, Model, Node.Rotation, CVector4.Instance);
				return true;
			case "scaling":
				LoadStaticAnimator(Loader, Model, Node.Scaling, CVector3.Instance);
				return true;
			default:
				return false;
			}
		}

		public void SaveNode<T>(CSaver Saver, MdxLib.Model.CModel Model, CNode<T> Node) where T : CNode<T>
		{
			int num = 0;
			SaveId(Saver, "ObjectId", Node.NodeId, ECondition.NotInvalidId);
			SaveId(Saver, "Parent", Node.Parent.NodeId, ECondition.NotInvalidId);
			if (Node.DontInheritTranslation)
			{
				num++;
			}
			if (Node.DontInheritRotation)
			{
				num++;
			}
			if (Node.DontInheritScaling)
			{
				num++;
			}
			if (num > 0)
			{
				Saver.WriteTabs();
				Saver.WriteWord("DontInherit { ");
				if (Node.DontInheritTranslation)
				{
					num--;
					Saver.WriteWord("Translation");
					Saver.WriteWord((num > 0) ? ", " : "");
				}
				if (Node.DontInheritRotation)
				{
					num--;
					Saver.WriteWord("Rotation");
					Saver.WriteWord((num > 0) ? ", " : "");
				}
				if (Node.DontInheritScaling)
				{
					num--;
					Saver.WriteWord("Scaling");
					Saver.WriteWord((num > 0) ? ", " : "");
				}
				Saver.WriteLine(" },");
			}
			SaveBoolean(Saver, "Billboarded", Node.Billboarded);
			SaveBoolean(Saver, "BillboardedLockX", Node.BillboardedLockX);
			SaveBoolean(Saver, "BillboardedLockY", Node.BillboardedLockY);
			SaveBoolean(Saver, "BillboardedLockZ", Node.BillboardedLockZ);
			SaveBoolean(Saver, "CameraAnchored", Node.CameraAnchored);
			SaveAnimator(Saver, Model, Node.Translation, CVector3.Instance, "Translation", ECondition.NotZero);
			SaveAnimator(Saver, Model, Node.Rotation, CVector4.Instance, "Rotation", ECondition.NotDefaultQuaternion);
			SaveAnimator(Saver, Model, Node.Scaling, CVector3.Instance, "Scaling", ECondition.NotOne);
		}
	}
}
