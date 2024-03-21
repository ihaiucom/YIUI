using System.Collections.Generic;
using MdxLib.Primitives;

namespace MdxLib.Model
{
	/// <summary>
	/// A geoset class. Contains vertices and faces which constructs
	/// the geometry of the model (the shapes you see).
	/// </summary>
	public sealed class CGeoset : CObject<CGeoset>
	{
		private int _SelectionGroup;

		private bool _Unselectable;

		private CExtent _Extent = CConstants.DefaultExtent;

		private CObjectReference<CMaterial> _Material;

		private CObjectContainer<CGeosetVertex> _Vertices;

		private CObjectContainer<CGeosetFace> _Faces;

		private CObjectContainer<CGeosetGroup> _Groups;

		private CObjectContainer<CGeosetExtent> _Extents;

		/// <summary>
		/// Checks if the geoset has references pointing to it.
		/// </summary>
		public override bool HasReferences
		{
			get
			{
				if (_Vertices != null && _Vertices.HasReferences)
				{
					return true;
				}
				if (_Faces != null && _Faces.HasReferences)
				{
					return true;
				}
				if (_Groups != null && _Groups.HasReferences)
				{
					return true;
				}
				if (_Extents != null && _Extents.HasReferences)
				{
					return true;
				}
				return base.HasReferences;
			}
		}

		/// <summary>
		/// Gets or sets the selection group.
		/// </summary>
		public int SelectionGroup
		{
			get
			{
				return _SelectionGroup;
			}
			set
			{
				AddSetObjectFieldCommand("_SelectionGroup", value);
				_SelectionGroup = value;
			}
		}

		/// <summary>
		/// Gets or sets the unselectable flag.
		/// </summary>
		public bool Unselectable
		{
			get
			{
				return _Unselectable;
			}
			set
			{
				AddSetObjectFieldCommand("_Unselectable", value);
				_Unselectable = value;
			}
		}

		/// <summary>
		/// Gets or sets the extent.
		/// </summary>
		public CExtent Extent
		{
			get
			{
				return _Extent;
			}
			set
			{
				AddSetObjectFieldCommand("_Extent", value);
				_Extent = value;
			}
		}

		/// <summary>
		/// Retrieves the material reference.
		/// </summary>
		public CObjectReference<CMaterial> Material => _Material ?? (_Material = new CObjectReference<CMaterial>(base.Model));

		/// <summary>
		/// Checks if there exists som geoset vertices.
		/// </summary>
		public bool HasVertices
		{
			get
			{
				if (_Vertices == null)
				{
					return false;
				}
				return _Vertices.Count > 0;
			}
		}

		/// <summary>
		/// Checks if there exists some geoset faces.
		/// </summary>
		public bool HasFaces
		{
			get
			{
				if (_Faces == null)
				{
					return false;
				}
				return _Faces.Count > 0;
			}
		}

		/// <summary>
		/// Checks if there exists some geoset groups.
		/// </summary>
		public bool HasGroups
		{
			get
			{
				if (_Groups == null)
				{
					return false;
				}
				return _Groups.Count > 0;
			}
		}

		/// <summary>
		/// Checks if there exists some geoset extents.
		/// </summary>
		public bool HasExtents
		{
			get
			{
				if (_Extents == null)
				{
					return false;
				}
				return _Extents.Count > 0;
			}
		}

		/// <summary>
		/// Retrieves the geoset vertices container.
		/// </summary>
		public CObjectContainer<CGeosetVertex> Vertices => _Vertices ?? (_Vertices = new CObjectContainer<CGeosetVertex>(base.Model));

		/// <summary>
		/// Retrieves the geoset faces container.
		/// </summary>
		public CObjectContainer<CGeosetFace> Faces => _Faces ?? (_Faces = new CObjectContainer<CGeosetFace>(base.Model));

		/// <summary>
		/// Retrieves the geoset groups container.
		/// </summary>
		public CObjectContainer<CGeosetGroup> Groups => _Groups ?? (_Groups = new CObjectContainer<CGeosetGroup>(base.Model));

		/// <summary>
		/// Retrieves the geoset extents container.
		/// </summary>
		public CObjectContainer<CGeosetExtent> Extents => _Extents ?? (_Extents = new CObjectContainer<CGeosetExtent>(base.Model));

		/// <summary>
		/// Parameterized constructor.
		/// </summary>
		/// <param name="Model">The model to be associated with this geoset</param>
		public CGeoset(CModel Model)
			: base(Model)
		{
		}

		internal override void BuildDetacherList(ICollection<CDetacher> DetacherList)
		{
			base.BuildDetacherList(DetacherList);
			if (_Vertices != null)
			{
				_Vertices.BuildDetacherList(DetacherList);
			}
			if (_Faces != null)
			{
				_Faces.BuildDetacherList(DetacherList);
			}
			if (_Groups != null)
			{
				_Groups.BuildDetacherList(DetacherList);
			}
			if (_Extents != null)
			{
				_Extents.BuildDetacherList(DetacherList);
			}
		}

		/// <summary>
		/// Generates a string version of the geoset.
		/// </summary>
		/// <returns>The generated string</returns>
		public override string ToString()
		{
			return "Geoset #" + base.ObjectId;
		}
	}
}
