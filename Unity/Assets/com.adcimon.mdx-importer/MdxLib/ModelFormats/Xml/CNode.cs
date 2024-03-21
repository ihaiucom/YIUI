using System.Xml;
using MdxLib.Model;
using MdxLib.ModelFormats.Xml.Value;

namespace MdxLib.ModelFormats.Xml
{
	internal abstract class CNode : CObject
	{
		public CNode()
		{
		}

		public void LoadNode<T>(CLoader Loader, XmlNode Node, MdxLib.Model.CModel Model, T ModelNode) where T : CNode<T>
		{
			ModelNode.Name = ReadString(Node, "name", ModelNode.Name);
			ModelNode.DontInheritTranslation = ReadBoolean(Node, "dont_inherit_translation", ModelNode.DontInheritTranslation);
			ModelNode.DontInheritRotation = ReadBoolean(Node, "dont_inherit_rotation", ModelNode.DontInheritRotation);
			ModelNode.DontInheritScaling = ReadBoolean(Node, "dont_inherit_scaling", ModelNode.DontInheritScaling);
			ModelNode.Billboarded = ReadBoolean(Node, "billboarded", ModelNode.Billboarded);
			ModelNode.BillboardedLockX = ReadBoolean(Node, "billboarded_lock_x", ModelNode.BillboardedLockX);
			ModelNode.BillboardedLockY = ReadBoolean(Node, "billboarded_lock_y", ModelNode.BillboardedLockY);
			ModelNode.BillboardedLockZ = ReadBoolean(Node, "billboarded_lock_z", ModelNode.BillboardedLockZ);
			ModelNode.CameraAnchored = ReadBoolean(Node, "camera_anchored", ModelNode.CameraAnchored);
			ModelNode.PivotPoint = ReadVector3(Node, "pivot_point", ModelNode.PivotPoint);
			Loader.Attacher.AddNode(Model, ModelNode.Parent, ReadInteger(Node, "parent", -1));
			LoadAnimator(Loader, Node, Model, ModelNode.Translation, CVector3.Instance, "translation");
			LoadAnimator(Loader, Node, Model, ModelNode.Rotation, CVector4.Instance, "rotation");
			LoadAnimator(Loader, Node, Model, ModelNode.Scaling, CVector3.Instance, "scaling");
		}

		public void SaveNode<T>(CSaver Saver, XmlNode Node, MdxLib.Model.CModel Model, T ModelNode) where T : CNode<T>
		{
			WriteString(Node, "name", ModelNode.Name);
			WriteBoolean(Node, "dont_inherit_translation", ModelNode.DontInheritTranslation);
			WriteBoolean(Node, "dont_inherit_rotation", ModelNode.DontInheritRotation);
			WriteBoolean(Node, "dont_inherit_scaling", ModelNode.DontInheritScaling);
			WriteBoolean(Node, "billboarded", ModelNode.Billboarded);
			WriteBoolean(Node, "billboarded_lock_x", ModelNode.BillboardedLockX);
			WriteBoolean(Node, "billboarded_lock_y", ModelNode.BillboardedLockY);
			WriteBoolean(Node, "billboarded_lock_z", ModelNode.BillboardedLockZ);
			WriteBoolean(Node, "camera_anchored", ModelNode.CameraAnchored);
			WriteVector3(Node, "pivot_point", ModelNode.PivotPoint);
			WriteInteger(Node, "parent", ModelNode.Parent.NodeId);
			SaveAnimator(Saver, Node, Model, ModelNode.Translation, CVector3.Instance, "translation");
			SaveAnimator(Saver, Node, Model, ModelNode.Rotation, CVector4.Instance, "rotation");
			SaveAnimator(Saver, Node, Model, ModelNode.Scaling, CVector3.Instance, "scaling");
		}
	}
}
