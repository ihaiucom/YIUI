using MdxLib.Primitives;

namespace MdxLib.Model
{
	/// <summary>
	/// A collision shape class. Defines bounds in which a user can
	/// click to select the model.
	/// </summary>
	public sealed class CCollisionShape : CNode<CCollisionShape>
	{
		private ECollisionShapeType _Type;

		private float _Radius;

		private CVector3 _Vertex1 = CConstants.DefaultVector3;

		private CVector3 _Vertex2 = CConstants.DefaultVector3;

		/// <summary>
		/// Retrieves the node ID (if added to a container).
		/// </summary>
		public override int NodeId => base.Model.GetCollisionShapeNodeId(this);

		/// <summary>
		/// Gets or sets the type.
		/// </summary>
		public ECollisionShapeType Type
		{
			get
			{
				return _Type;
			}
			set
			{
				AddSetObjectFieldCommand("_Type", value);
				_Type = value;
			}
		}

		/// <summary>
		/// Gets or sets the radius (for Sphere).
		/// </summary>
		public float Radius
		{
			get
			{
				return _Radius;
			}
			set
			{
				AddSetObjectFieldCommand("_Radius", value);
				_Radius = value;
			}
		}

		/// <summary>
		/// Gets or sets the first vertex (corner 1 for Box, center for Sphere).
		/// </summary>
		public CVector3 Vertex1
		{
			get
			{
				return _Vertex1;
			}
			set
			{
				AddSetObjectFieldCommand("_Vertex1", value);
				_Vertex1 = value;
			}
		}

		/// <summary>
		/// Gets or sets the second vertex (corner 2 for Box).
		/// </summary>
		public CVector3 Vertex2
		{
			get
			{
				return _Vertex2;
			}
			set
			{
				AddSetObjectFieldCommand("_Vertex2", value);
				_Vertex2 = value;
			}
		}

		/// <summary>
		/// Parameterized constructor.
		/// </summary>
		/// <param name="Model">The model to be associated with this collision shape</param>
		public CCollisionShape(CModel Model)
			: base(Model)
		{
		}

		/// <summary>
		/// Generates a string version of the collision shape.
		/// </summary>
		/// <returns>The generated string</returns>
		public override string ToString()
		{
			return "Collision Shape #" + base.ObjectId;
		}
	}
}
