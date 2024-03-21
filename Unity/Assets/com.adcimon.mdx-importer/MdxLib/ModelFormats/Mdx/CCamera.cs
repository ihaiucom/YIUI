using System;
using MdxLib.Model;
using MdxLib.ModelFormats.Mdx.Value;

namespace MdxLib.ModelFormats.Mdx
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

		public void LoadAll(CLoader Loader, MdxLib.Model.CModel Model)
		{
			int num = Loader.ReadInt32();
			while (num > 0)
			{
				Loader.PushLocation();
				MdxLib.Model.CCamera cCamera = new MdxLib.Model.CCamera(Model);
				Load(Loader, Model, cCamera);
				Model.Cameras.Add(cCamera);
				num -= Loader.PopLocation();
				if (num < 0)
				{
					throw new Exception("Error at location " + Loader.Location + ", too many Camera bytes were read!");
				}
			}
		}

		public void Load(CLoader Loader, MdxLib.Model.CModel Model, MdxLib.Model.CCamera Camera)
		{
			Loader.PushLocation();
			int num = Loader.ReadInt32();
			Camera.Name = Loader.ReadString(80);
			Camera.Position = Loader.ReadVector3();
			Camera.FieldOfView = Loader.ReadFloat();
			Camera.FarDistance = Loader.ReadFloat();
			Camera.NearDistance = Loader.ReadFloat();
			Camera.TargetPosition = Loader.ReadVector3();
			num -= Loader.PopLocation();
			if (num < 0)
			{
				throw new Exception("Error at location " + Loader.Location + ", too many Camera bytes were read!");
			}
			while (num > 0)
			{
				Loader.PushLocation();
				string text = Loader.ReadTag();
				switch (text)
				{
				case "KCTR":
					CObject.LoadAnimator(Loader, Model, Camera.Translation, CVector3.Instance);
					break;
				case "KTTR":
					CObject.LoadAnimator(Loader, Model, Camera.TargetTranslation, CVector3.Instance);
					break;
				case "KCRL":
					CObject.LoadAnimator(Loader, Model, Camera.Rotation, CFloat.Instance);
					break;
				default:
					throw new Exception("Error at location " + Loader.Location + ", unknown Camera tag \"" + text + "\"!");
				}
				num -= Loader.PopLocation();
				if (num < 0)
				{
					throw new Exception("Error at location " + Loader.Location + ", too many Camera bytes were read!");
				}
			}
		}

		public void SaveAll(CSaver Saver, MdxLib.Model.CModel Model)
		{
			if (!Model.HasCameras)
			{
				return;
			}
			Saver.WriteTag("CAMS");
			Saver.PushLocation();
			foreach (MdxLib.Model.CCamera camera in Model.Cameras)
			{
				Save(Saver, Model, camera);
			}
			Saver.PopExclusiveLocation();
		}

		public void Save(CSaver Saver, MdxLib.Model.CModel Model, MdxLib.Model.CCamera Camera)
		{
			Saver.PushLocation();
			Saver.WriteString(Camera.Name, 80);
			Saver.WriteVector3(Camera.Position);
			Saver.WriteFloat(Camera.FieldOfView);
			Saver.WriteFloat(Camera.FarDistance);
			Saver.WriteFloat(Camera.NearDistance);
			Saver.WriteVector3(Camera.TargetPosition);
			CObject.SaveAnimator(Saver, Model, Camera.Translation, CVector3.Instance, "KCTR");
			CObject.SaveAnimator(Saver, Model, Camera.TargetTranslation, CVector3.Instance, "KTTR");
			CObject.SaveAnimator(Saver, Model, Camera.Rotation, CFloat.Instance, "KCRL");
			Saver.PopInclusiveLocation();
		}
	}
}
