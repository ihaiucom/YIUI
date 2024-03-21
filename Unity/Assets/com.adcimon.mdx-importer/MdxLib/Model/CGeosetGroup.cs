using System.Collections.Generic;

namespace MdxLib.Model
{
	/// <summary>
	/// A geoset group class. Defines which matrices (nodes) its transformation
	/// should be constructed from. Each vertex connects to one group and gets
	/// transformed through it.
	/// </summary>
	public sealed class CGeosetGroup : CObject<CGeosetGroup>
	{
		private CObjectContainer<CGeosetGroupNode> _Nodes;

		/// <summary>
		/// Checks if the geoset group has references pointing to it.
		/// </summary>
		public override bool HasReferences
		{
			get
			{
				if (_Nodes != null && _Nodes.HasReferences)
				{
					return true;
				}
				return base.HasReferences;
			}
		}

		/// <summary>
		/// Checks if there exists some geoset group nodes.
		/// </summary>
		public bool HasNodes
		{
			get
			{
				if (_Nodes == null)
				{
					return false;
				}
				return _Nodes.Count > 0;
			}
		}

		/// <summary>
		/// Retrieves the geoset group nodes container.
		/// </summary>
		public CObjectContainer<CGeosetGroupNode> Nodes => _Nodes ?? (_Nodes = new CObjectContainer<CGeosetGroupNode>(base.Model));

		/// <summary>
		/// Parameterized constructor.
		/// </summary>
		/// <param name="Model">The model to be associated with this geoset group</param>
		public CGeosetGroup(CModel Model)
			: base(Model)
		{
		}

		internal override void BuildDetacherList(ICollection<CDetacher> DetacherList)
		{
			base.BuildDetacherList(DetacherList);
			if (_Nodes != null)
			{
				_Nodes.BuildDetacherList(DetacherList);
			}
		}

		/// <summary>
		/// Generates a string version of the geoset group.
		/// </summary>
		/// <returns>The generated string</returns>
		public override string ToString()
		{
			return "Geoset Group #" + base.ObjectId;
		}
	}
}
