using System.Collections.Generic;

namespace MdxLib.Model
{
	/// <summary>
	/// A material class. Defines how a geoset's surface looks like.
	/// Can consist of multiple layers for more advanced effects,
	/// like teamcolor.
	/// </summary>
	public sealed class CMaterial : CObject<CMaterial>
	{
		private int _PriorityPlane;

		private bool _ConstantColor;

		private bool _FullResolution;

		private bool _SortPrimitivesFarZ;

		private bool _SortPrimitivesNearZ;

		private CObjectContainer<CMaterialLayer> _Layers;

		/// <summary>
		/// Checks if the material has references pointing to it.
		/// </summary>
		public override bool HasReferences
		{
			get
			{
				if (_Layers != null && _Layers.HasReferences)
				{
					return true;
				}
				return base.HasReferences;
			}
		}

		/// <summary>
		/// Gets or sets the priority plane.
		/// </summary>
		public int PriorityPlane
		{
			get
			{
				return _PriorityPlane;
			}
			set
			{
				AddSetObjectFieldCommand("_PriorityPlane", value);
				_PriorityPlane = value;
			}
		}

		/// <summary>
		/// Gets or sets the constant color flag.
		/// </summary>
		public bool ConstantColor
		{
			get
			{
				return _ConstantColor;
			}
			set
			{
				AddSetObjectFieldCommand("_ConstantColor", value);
				_ConstantColor = value;
			}
		}

		/// <summary>
		/// Gets or sets the full resolution flag.
		/// </summary>
		public bool FullResolution
		{
			get
			{
				return _FullResolution;
			}
			set
			{
				AddSetObjectFieldCommand("_FullResolution", value);
				_FullResolution = value;
			}
		}

		/// <summary>
		/// Gets or sets the sort primitives far Z flag.
		/// </summary>
		public bool SortPrimitivesFarZ
		{
			get
			{
				return _SortPrimitivesFarZ;
			}
			set
			{
				AddSetObjectFieldCommand("_SortPrimitivesFarZ", value);
				_SortPrimitivesFarZ = value;
			}
		}

		/// <summary>
		/// Gets or sets the sort primitives near Z flag.
		/// </summary>
		public bool SortPrimitivesNearZ
		{
			get
			{
				return _SortPrimitivesNearZ;
			}
			set
			{
				AddSetObjectFieldCommand("_SortPrimitivesNearZ", value);
				_SortPrimitivesNearZ = value;
			}
		}

		/// <summary>
		/// Checks if there exists some material layers.
		/// </summary>
		public bool HasLayers
		{
			get
			{
				if (_Layers == null)
				{
					return false;
				}
				return _Layers.Count > 0;
			}
		}

		/// <summary>
		/// Retrieves the material layers container.
		/// </summary>
		public CObjectContainer<CMaterialLayer> Layers => _Layers ?? (_Layers = new CObjectContainer<CMaterialLayer>(base.Model));

		/// <summary>
		/// Parameterized constructor.
		/// </summary>
		/// <param name="Model">The model to be associated with this material</param>
		public CMaterial(CModel Model)
			: base(Model)
		{
		}

		internal override void BuildDetacherList(ICollection<CDetacher> DetacherList)
		{
			base.BuildDetacherList(DetacherList);
			if (_Layers != null)
			{
				_Layers.BuildDetacherList(DetacherList);
			}
		}

		/// <summary>
		/// Generates a string version of the material.
		/// </summary>
		/// <returns>The generated string</returns>
		public override string ToString()
		{
			return "Material #" + base.ObjectId;
		}
	}
}
