using System.Collections.Generic;
using System.IO;
using MdxLib.Model;
using MdxLib.Primitives;

namespace MdxLib.ModelFormats.Mdx
{
	internal sealed class CModel
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
			List<CVector3> pivotPointList = new List<CVector3>();
			Loader.ExpectTag("MDLX");
			while (true)
			{
				string text = "";
				try
				{
					if (Loader.Eof())
					{
						break;
					}
					text = Loader.ReadTag();
					goto IL_0033;
				}
				catch (EndOfStreamException)
				{
				}
				break;
				IL_0033:
				switch (text)
				{
				case "VERS":
					CModelVersion.Instance.Load(Loader, Model);
					break;
				case "MODL":
					CModelInfo.Instance.Load(Loader, Model);
					break;
				case "SEQS":
					CSequence.Instance.LoadAll(Loader, Model);
					break;
				case "GLBS":
					CGlobalSequence.Instance.LoadAll(Loader, Model);
					break;
				case "TEXS":
					CTexture.Instance.LoadAll(Loader, Model);
					break;
				case "MTLS":
					CMaterial.Instance.LoadAll(Loader, Model);
					break;
				case "TXAN":
					CTextureAnimation.Instance.LoadAll(Loader, Model);
					break;
				case "GEOS":
					CGeoset.Instance.LoadAll(Loader, Model);
					break;
				case "GEOA":
					CGeosetAnimation.Instance.LoadAll(Loader, Model);
					break;
				case "BONE":
					CBone.Instance.LoadAll(Loader, Model);
					break;
				case "LITE":
					CLight.Instance.LoadAll(Loader, Model);
					break;
				case "HELP":
					CHelper.Instance.LoadAll(Loader, Model);
					break;
				case "ATCH":
					CAttachment.Instance.LoadAll(Loader, Model);
					break;
				case "PIVT":
					CPivotPoint.Instance.Load(Loader, Model, pivotPointList);
					break;
				case "PREM":
					CParticleEmitter.Instance.LoadAll(Loader, Model);
					break;
				case "PRE2":
					CParticleEmitter2.Instance.LoadAll(Loader, Model);
					break;
				case "RIBB":
					CRibbonEmitter.Instance.LoadAll(Loader, Model);
					break;
				case "EVTS":
					CEvent.Instance.LoadAll(Loader, Model);
					break;
				case "CAMS":
					CCamera.Instance.LoadAll(Loader, Model);
					break;
				case "CLID":
					CCollisionShape.Instance.LoadAll(Loader, Model);
					break;
				case "META":
					CMetaData.Instance.Load(Loader, Model);
					break;
				default:
					Loader.Skip(Loader.ReadInt32());
					break;
				}
			}
			SetPivotPoints(Model, pivotPointList);
		}

		public void Save(CSaver Saver, MdxLib.Model.CModel Model, string Info)
		{
			List<CVector3> pivotPointList = new List<CVector3>();
			GetPivotPoints(Model, pivotPointList);
			Saver.WriteTag("MDLX");
			Saver.WriteTag("INFO");
			Saver.PushLocation();
			Saver.WriteString(Info, Info.Length);
			Saver.PopExclusiveLocation();
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
