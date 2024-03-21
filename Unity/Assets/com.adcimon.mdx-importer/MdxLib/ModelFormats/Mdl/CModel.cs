using System;
using System.Collections.Generic;
using MdxLib.Model;
using MdxLib.ModelFormats.Mdl.Token;
using MdxLib.Primitives;

namespace MdxLib.ModelFormats.Mdl
{
	internal sealed class CModel : CObject
	{
		private static class CSingleton
		{
			public static CModel Instance;

			static CSingleton()
			{
				Instance = new CModel();
			}
		}

		public static CModel Instance => CSingleton.Instance;

		private CModel()
		{
		}

		public void Load(CLoader Loader, MdxLib.Model.CModel Model)
		{
			string text = "";
			List<CVector3> pivotPointList = new List<CVector3>();
			while (true)
			{
				try
				{
					text = ((Loader.PeekToken() != EType.MetaComment) ? Loader.ReadWord() : "metacomment");
				}
				catch (Exception ex)
				{
					if (!Loader.Eof)
					{
						throw ex;
					}
					break;
				}
				switch (text)
				{
				case "version":
					CModelVersion.Instance.Load(Loader, Model);
					break;
				case "model":
					CModelInfo.Instance.Load(Loader, Model);
					break;
				case "sequences":
					CSequence.Instance.LoadAll(Loader, Model);
					break;
				case "globalsequences":
					CGlobalSequence.Instance.LoadAll(Loader, Model);
					break;
				case "textures":
					CTexture.Instance.LoadAll(Loader, Model);
					break;
				case "materials":
					CMaterial.Instance.LoadAll(Loader, Model);
					break;
				case "textureanims":
					CTextureAnimation.Instance.LoadAll(Loader, Model);
					break;
				case "geoset":
					CGeoset.Instance.LoadAll(Loader, Model);
					break;
				case "geosetanim":
					CGeosetAnimation.Instance.LoadAll(Loader, Model);
					break;
				case "bone":
					CBone.Instance.LoadAll(Loader, Model);
					break;
				case "light":
					CLight.Instance.LoadAll(Loader, Model);
					break;
				case "helper":
					CHelper.Instance.LoadAll(Loader, Model);
					break;
				case "attachment":
					CAttachment.Instance.LoadAll(Loader, Model);
					break;
				case "pivotpoints":
					CPivotPoint.Instance.Load(Loader, Model, pivotPointList);
					break;
				case "particleemitter":
					CParticleEmitter.Instance.LoadAll(Loader, Model);
					break;
				case "particleemitter2":
					CParticleEmitter2.Instance.LoadAll(Loader, Model);
					break;
				case "ribbonemitter":
					CRibbonEmitter.Instance.LoadAll(Loader, Model);
					break;
				case "eventobject":
					CEvent.Instance.LoadAll(Loader, Model);
					break;
				case "camera":
					CCamera.Instance.LoadAll(Loader, Model);
					break;
				case "collisionshape":
					CCollisionShape.Instance.LoadAll(Loader, Model);
					break;
				case "metacomment":
					CMetaData.Instance.Load(Loader, Model);
					break;
				}
			}
			SetPivotPoints(Model, pivotPointList);
		}

		public void Save(CSaver Saver, MdxLib.Model.CModel Model)
		{
			List<CVector3> pivotPointList = new List<CVector3>();
			GetPivotPoints(Model, pivotPointList);
			CModelVersion.Instance.Save(Saver, Model);
			CModelInfo.Instance.Save(Saver, Model);
			CSequence.Instance.SaveAll(Saver, Model);
			CGlobalSequence.Instance.SaveAll(Saver, Model);
			CTexture.Instance.SaveAll(Saver, Model);
			CMaterial.Instance.SaveAll(Saver, Model);
			CTextureAnimation.Instance.SaveAll(Saver, Model);
			CGeoset.Instance.SaveAll(Saver, Model);
			CGeosetAnimation.Instance.SaveAll(Saver, Model);
			CBone.Instance.SaveAll(Saver, Model);
			CLight.Instance.SaveAll(Saver, Model);
			CHelper.Instance.SaveAll(Saver, Model);
			CAttachment.Instance.SaveAll(Saver, Model);
			CPivotPoint.Instance.Save(Saver, Model, pivotPointList);
			CParticleEmitter.Instance.SaveAll(Saver, Model);
			CParticleEmitter2.Instance.SaveAll(Saver, Model);
			CRibbonEmitter.Instance.SaveAll(Saver, Model);
			CEvent.Instance.SaveAll(Saver, Model);
			CCamera.Instance.SaveAll(Saver, Model);
			CCollisionShape.Instance.SaveAll(Saver, Model);
			CMetaData.Instance.Save(Saver, Model);
		}

		private void SetPivotPoints(MdxLib.Model.CModel Model, ICollection<CVector3> PivotPointList)
		{
			int num = 0;
			int count = Model.Nodes.Count;
			foreach (CVector3 PivotPoint in PivotPointList)
			{
				if (num < count)
				{
					Model.Nodes[num].PivotPoint = PivotPoint;
					num++;
					continue;
				}
				break;
			}
		}

		private void GetPivotPoints(MdxLib.Model.CModel Model, ICollection<CVector3> PivotPointList)
		{
			foreach (INode node in Model.Nodes)
			{
				PivotPointList.Add(node.PivotPoint);
			}
		}
	}
}
