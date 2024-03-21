using System;
using MdxLib.Model;
using MdxLib.ModelFormats.Mdl.Token;
using MdxLib.ModelFormats.Mdl.Value;

namespace MdxLib.ModelFormats.Mdl
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
			MdxLib.Model.CCamera cCamera = new MdxLib.Model.CCamera(Model);
			Load(Loader, Model, cCamera);
			Model.Cameras.Add(cCamera);
		}

		public void Load(CLoader Loader, MdxLib.Model.CModel Model, MdxLib.Model.CCamera Camera)
		{
			Camera.Name = Loader.ReadString();
			Loader.ExpectToken(EType.CurlyBracketLeft);
			while (Loader.PeekToken() != EType.CurlyBracketRight)
			{
				string text = Loader.ReadWord();
				switch (text)
				{
				case "static":
					text = Loader.ReadWord();
					switch (text)
					{
					case "translation":
						LoadStaticAnimator(Loader, Model, Camera.Translation, CVector3.Instance);
						break;
					case "rotation":
						LoadStaticAnimator(Loader, Model, Camera.Rotation, CFloat.Instance);
						break;
					default:
						throw new Exception("Syntax error at line " + Loader.Line + ", unknown tag \"" + text + "\"!");
					}
					break;
				case "translation":
					LoadAnimator(Loader, Model, Camera.Translation, CVector3.Instance);
					break;
				case "rotation":
					LoadAnimator(Loader, Model, Camera.Rotation, CFloat.Instance);
					break;
				case "position":
					Camera.Position = LoadVector3(Loader);
					break;
				case "fieldofview":
					Camera.FieldOfView = LoadFloat(Loader);
					break;
				case "nearclip":
					Camera.NearDistance = LoadFloat(Loader);
					break;
				case "farclip":
					Camera.FarDistance = LoadFloat(Loader);
					break;
				case "target":
					Loader.ExpectToken(EType.CurlyBracketLeft);
					if (Loader.PeekToken() == EType.CurlyBracketRight)
					{
						Loader.ReadToken();
						break;
					}
					text = Loader.ReadWord();
					switch (text)
					{
					case "static":
					{
						text = Loader.ReadWord();
						string text2;
						if ((text2 = text) != null && text2 == "translation")
						{
							LoadStaticAnimator(Loader, Model, Camera.TargetTranslation, CVector3.Instance);
							break;
						}
						throw new Exception("Syntax error at line " + Loader.Line + ", unknown tag \"" + text + "\"!");
					}
					case "translation":
						LoadAnimator(Loader, Model, Camera.TargetTranslation, CVector3.Instance);
						break;
					case "position":
						Camera.TargetPosition = LoadVector3(Loader);
						break;
					default:
						throw new Exception("Syntax error at line " + Loader.Line + ", unknown tag \"" + text + "\"!");
					}
					break;
				default:
					throw new Exception("Syntax error at line " + Loader.Line + ", unknown tag \"" + text + "\"!");
				}
			}
			Loader.ReadToken();
		}

		public void SaveAll(CSaver Saver, MdxLib.Model.CModel Model)
		{
			if (!Model.HasCameras)
			{
				return;
			}
			foreach (MdxLib.Model.CCamera camera in Model.Cameras)
			{
				Save(Saver, Model, camera);
			}
		}

		public void Save(CSaver Saver, MdxLib.Model.CModel Model, MdxLib.Model.CCamera Camera)
		{
			Saver.BeginGroup("Camera", Camera.Name);
			SaveFloat(Saver, "FieldOfView", Camera.FieldOfView);
			SaveFloat(Saver, "FarClip", Camera.FarDistance);
			SaveFloat(Saver, "NearClip", Camera.NearDistance);
			SaveVector3(Saver, "Position", Camera.Position);
			SaveAnimator(Saver, Model, Camera.Translation, CVector3.Instance, "Translation", ECondition.NotZero);
			Saver.BeginGroup("Target");
			SaveVector3(Saver, "Position", Camera.TargetPosition);
			SaveAnimator(Saver, Model, Camera.TargetTranslation, CVector3.Instance, "Translation", ECondition.NotZero);
			SaveAnimator(Saver, Model, Camera.Rotation, CFloat.Instance, "Rotation", ECondition.NotZero);
			Saver.EndGroup();
			Saver.EndGroup();
		}
	}
}
