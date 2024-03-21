using System.Xml;
using MdxLib.Model;
using MdxLib.ModelFormats.Xml.Value;

namespace MdxLib.ModelFormats.Xml
{
	internal sealed class CCamera : CObject
	{
		private static class CSingleton
		{
			public static CCamera Instance;

			static CSingleton()
			{
				Instance = new CCamera();
			}
		}

		public static CCamera Instance => CSingleton.Instance;

		private CCamera()
		{
		}

		public void Load(CLoader Loader, XmlNode Node, MdxLib.Model.CModel Model, MdxLib.Model.CCamera Camera)
		{
			Camera.Name = ReadString(Node, "name", Camera.Name);
			Camera.FieldOfView = ReadFloat(Node, "field_of_view", Camera.FieldOfView);
			Camera.NearDistance = ReadFloat(Node, "near_distance", Camera.NearDistance);
			Camera.FarDistance = ReadFloat(Node, "far_distance", Camera.FarDistance);
			Camera.Position = ReadVector3(Node, "position", Camera.Position);
			Camera.TargetPosition = ReadVector3(Node, "target_position", Camera.TargetPosition);
			LoadAnimator(Loader, Node, Model, Camera.Translation, CVector3.Instance, "source_translation");
			LoadAnimator(Loader, Node, Model, Camera.TargetTranslation, CVector3.Instance, "target_translation");
			LoadAnimator(Loader, Node, Model, Camera.Rotation, CFloat.Instance, "rotation");
		}

		public void Save(CSaver Saver, XmlNode Node, MdxLib.Model.CModel Model, MdxLib.Model.CCamera Camera)
		{
			WriteString(Node, "name", Camera.Name);
			WriteFloat(Node, "field_of_view", Camera.FieldOfView);
			WriteFloat(Node, "near_distance", Camera.NearDistance);
			WriteFloat(Node, "far_distance", Camera.FarDistance);
			WriteVector3(Node, "position", Camera.Position);
			WriteVector3(Node, "target_position", Camera.TargetPosition);
			SaveAnimator(Saver, Node, Model, Camera.Translation, CVector3.Instance, "source_translation");
			SaveAnimator(Saver, Node, Model, Camera.TargetTranslation, CVector3.Instance, "target_translation");
			SaveAnimator(Saver, Node, Model, Camera.Rotation, CFloat.Instance, "rotation");
		}
	}
}
