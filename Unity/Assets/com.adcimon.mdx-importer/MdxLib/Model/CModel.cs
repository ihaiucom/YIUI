using System.Xml;
using MdxLib.Command;
using MdxLib.Primitives;

namespace MdxLib.Model
{
	/// <summary>
	/// The central class and the container of most other components.
	/// This is the main object you're working with.
	/// </summary>
	public sealed class CModel
	{
		private object _Tag;

		private int _Version = 800;

		private int _BlendTime = 150;

		private string _Name = "";

		private string _AnimationFile = "";

		private CExtent _Extent = CConstants.DefaultExtent;

		private CObjectContainer<CAttachment> _Attachments;

		private CObjectContainer<CBone> _Bones;

		private CObjectContainer<CCollisionShape> _CollisionShapes;

		private CObjectContainer<CEvent> _Events;

		private CObjectContainer<CHelper> _Helpers;

		private CObjectContainer<CLight> _Lights;

		private CObjectContainer<CParticleEmitter> _ParticleEmitters;

		private CObjectContainer<CParticleEmitter2> _ParticleEmitters2;

		private CObjectContainer<CRibbonEmitter> _RibbonEmitters;

		private CObjectContainer<CCamera> _Cameras;

		private CObjectContainer<CGeoset> _Geosets;

		private CObjectContainer<CGeosetAnimation> _GeosetAnimations;

		private CObjectContainer<CGlobalSequence> _GlobalSequences;

		private CObjectContainer<CMaterial> _Materials;

		private CObjectContainer<CSequence> _Sequences;

		private CObjectContainer<CTexture> _Textures;

		private CObjectContainer<CTextureAnimation> _TextureAnimations;

		private CNodeContainer _Nodes;

		private XmlDocument _MetaData;

		internal CCommandGroup CommandGroup;

		/// <summary>
		/// Gets or sets the tag data of the model. Tag data is not saved when the model is.
		/// </summary>
		public object Tag
		{
			get
			{
				return _Tag;
			}
			set
			{
				_Tag = value;
			}
		}

		/// <summary>
		/// Gets or sets the version. Should be DefaultModelVersion.
		/// </summary>
		public int Version
		{
			get
			{
				return _Version;
			}
			set
			{
				AddSetModelFieldCommand("_Version", value);
				_Version = value;
			}
		}

		/// <summary>
		/// Gets or sets the blend time.
		/// </summary>
		public int BlendTime
		{
			get
			{
				return _BlendTime;
			}
			set
			{
				AddSetModelFieldCommand("_BlendTime", value);
				_BlendTime = value;
			}
		}

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		public string Name
		{
			get
			{
				return _Name;
			}
			set
			{
				AddSetModelFieldCommand("_Name", value);
				_Name = value;
			}
		}

		/// <summary>
		/// Gets or sets the animation file.
		/// </summary>
		public string AnimationFile
		{
			get
			{
				return _AnimationFile;
			}
			set
			{
				AddSetModelFieldCommand("_AnimationFile", value);
				_AnimationFile = value;
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
				AddSetModelFieldCommand("_Extent", value);
				_Extent = value;
			}
		}

		/// <summary>
		/// Checks if there exists some attachments.
		/// </summary>
		public bool HasAttachments
		{
			get
			{
				if (_Attachments == null)
				{
					return false;
				}
				return _Attachments.Count > 0;
			}
		}

		/// <summary>
		/// Checks if there exists some bones.
		/// </summary>
		public bool HasBones
		{
			get
			{
				if (_Bones == null)
				{
					return false;
				}
				return _Bones.Count > 0;
			}
		}

		/// <summary>
		/// Checks if there exists some collision shapes.
		/// </summary>
		public bool HasCollisionShapes
		{
			get
			{
				if (_CollisionShapes == null)
				{
					return false;
				}
				return _CollisionShapes.Count > 0;
			}
		}

		/// <summary>
		/// Checks if there exists some events.
		/// </summary>
		public bool HasEvents
		{
			get
			{
				if (_Events == null)
				{
					return false;
				}
				return _Events.Count > 0;
			}
		}

		/// <summary>
		/// Checks if there exists some helpers.
		/// </summary>
		public bool HasHelpers
		{
			get
			{
				if (_Helpers == null)
				{
					return false;
				}
				return _Helpers.Count > 0;
			}
		}

		/// <summary>
		/// Checks if there exists some lights.
		/// </summary>
		public bool HasLights
		{
			get
			{
				if (_Lights == null)
				{
					return false;
				}
				return _Lights.Count > 0;
			}
		}

		/// <summary>
		/// Checks if there exists some particle emitters.
		/// </summary>
		public bool HasParticleEmitters
		{
			get
			{
				if (_ParticleEmitters == null)
				{
					return false;
				}
				return _ParticleEmitters.Count > 0;
			}
		}

		/// <summary>
		/// Checks if there exists some particle emitters 2.
		/// </summary>
		public bool HasParticleEmitters2
		{
			get
			{
				if (_ParticleEmitters2 == null)
				{
					return false;
				}
				return _ParticleEmitters2.Count > 0;
			}
		}

		/// <summary>
		/// Checks if there exists some ribbon emitters.
		/// </summary>
		public bool HasRibbonEmitters
		{
			get
			{
				if (_RibbonEmitters == null)
				{
					return false;
				}
				return _RibbonEmitters.Count > 0;
			}
		}

		/// <summary>
		/// Checks if there exists some cameras.
		/// </summary>
		public bool HasCameras
		{
			get
			{
				if (_Cameras == null)
				{
					return false;
				}
				return _Cameras.Count > 0;
			}
		}

		/// <summary>
		/// Checks if there exists some geosets.
		/// </summary>
		public bool HasGeosets
		{
			get
			{
				if (_Geosets == null)
				{
					return false;
				}
				return _Geosets.Count > 0;
			}
		}

		/// <summary>
		/// Checks if there exists some geoset animations.
		/// </summary>
		public bool HasGeosetAnimations
		{
			get
			{
				if (_GeosetAnimations == null)
				{
					return false;
				}
				return _GeosetAnimations.Count > 0;
			}
		}

		/// <summary>
		/// Checks if there exists some global sequences.
		/// </summary>
		public bool HasGlobalSequences
		{
			get
			{
				if (_GlobalSequences == null)
				{
					return false;
				}
				return _GlobalSequences.Count > 0;
			}
		}

		/// <summary>
		/// Checks if there exists some materials.
		/// </summary>
		public bool HasMaterials
		{
			get
			{
				if (_Materials == null)
				{
					return false;
				}
				return _Materials.Count > 0;
			}
		}

		/// <summary>
		/// Checks if there exists some sequences.
		/// </summary>
		public bool HasSequences
		{
			get
			{
				if (_Sequences == null)
				{
					return false;
				}
				return _Sequences.Count > 0;
			}
		}

		/// <summary>
		/// Checks if there exists some textures.
		/// </summary>
		public bool HasTextures
		{
			get
			{
				if (_Textures == null)
				{
					return false;
				}
				return _Textures.Count > 0;
			}
		}

		/// <summary>
		/// Checks if there exists some texture animations.
		/// </summary>
		public bool HasTextureAnimations
		{
			get
			{
				if (_TextureAnimations == null)
				{
					return false;
				}
				return _TextureAnimations.Count > 0;
			}
		}

		/// <summary>
		/// Checks if there exists some nodes.
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
		/// Checks if there exists some metadata.
		/// </summary>
		public bool HasMetaData
		{
			get
			{
				if (_MetaData == null)
				{
					return false;
				}
				if (_MetaData.DocumentElement == null)
				{
					return false;
				}
				return _MetaData.DocumentElement.ChildNodes.Count > 0;
			}
		}

		/// <summary>
		/// Retrieves the attachments container.
		/// </summary>
		public CObjectContainer<CAttachment> Attachments => _Attachments ?? (_Attachments = new CObjectContainer<CAttachment>(this));

		/// <summary>
		/// Retrieves the bones container.
		/// </summary>
		public CObjectContainer<CBone> Bones => _Bones ?? (_Bones = new CObjectContainer<CBone>(this));

		/// <summary>
		/// Retrieves the collision shapes container.
		/// </summary>
		public CObjectContainer<CCollisionShape> CollisionShapes => _CollisionShapes ?? (_CollisionShapes = new CObjectContainer<CCollisionShape>(this));

		/// <summary>
		/// Retrieves the events container.
		/// </summary>
		public CObjectContainer<CEvent> Events => _Events ?? (_Events = new CObjectContainer<CEvent>(this));

		/// <summary>
		/// Retrieves the helpers container.
		/// </summary>
		public CObjectContainer<CHelper> Helpers => _Helpers ?? (_Helpers = new CObjectContainer<CHelper>(this));

		/// <summary>
		/// Retrieves the lights container.
		/// </summary>
		public CObjectContainer<CLight> Lights => _Lights ?? (_Lights = new CObjectContainer<CLight>(this));

		/// <summary>
		/// Retrieves the particle emitters container.
		/// </summary>
		public CObjectContainer<CParticleEmitter> ParticleEmitters => _ParticleEmitters ?? (_ParticleEmitters = new CObjectContainer<CParticleEmitter>(this));

		/// <summary>
		/// Retrieves the particle emitters 2 container.
		/// </summary>
		public CObjectContainer<CParticleEmitter2> ParticleEmitters2 => _ParticleEmitters2 ?? (_ParticleEmitters2 = new CObjectContainer<CParticleEmitter2>(this));

		/// <summary>
		/// Retrieves the ribbon emitters container.
		/// </summary>
		public CObjectContainer<CRibbonEmitter> RibbonEmitters => _RibbonEmitters ?? (_RibbonEmitters = new CObjectContainer<CRibbonEmitter>(this));

		/// <summary>
		/// Retrieves the cameras container.
		/// </summary>
		public CObjectContainer<CCamera> Cameras => _Cameras ?? (_Cameras = new CObjectContainer<CCamera>(this));

		/// <summary>
		/// Retrieves the geosets container.
		/// </summary>
		public CObjectContainer<CGeoset> Geosets => _Geosets ?? (_Geosets = new CObjectContainer<CGeoset>(this));

		/// <summary>
		/// Retrieves the geoset animations container.
		/// </summary>
		public CObjectContainer<CGeosetAnimation> GeosetAnimations => _GeosetAnimations ?? (_GeosetAnimations = new CObjectContainer<CGeosetAnimation>(this));

		/// <summary>
		/// Retrieves the global sequences container.
		/// </summary>
		public CObjectContainer<CGlobalSequence> GlobalSequences => _GlobalSequences ?? (_GlobalSequences = new CObjectContainer<CGlobalSequence>(this));

		/// <summary>
		/// Retrieves the materials container.
		/// </summary>
		public CObjectContainer<CMaterial> Materials => _Materials ?? (_Materials = new CObjectContainer<CMaterial>(this));

		/// <summary>
		/// Retrieves the sequences container.
		/// </summary>
		public CObjectContainer<CSequence> Sequences => _Sequences ?? (_Sequences = new CObjectContainer<CSequence>(this));

		/// <summary>
		/// Retrieves the textures container.
		/// </summary>
		public CObjectContainer<CTexture> Textures => _Textures ?? (_Textures = new CObjectContainer<CTexture>(this));

		/// <summary>
		/// Retrieves the texture animations container.
		/// </summary>
		public CObjectContainer<CTextureAnimation> TextureAnimations => _TextureAnimations ?? (_TextureAnimations = new CObjectContainer<CTextureAnimation>(this));

		/// <summary>
		/// Retrieves the nodes container.
		/// </summary>
		public CNodeContainer Nodes => _Nodes ?? (_Nodes = new CNodeContainer(this));

		/// <summary>
		/// Retrieves the metadata document. Metadata is used to store custom (hidden)
		/// data which is normally not in the model. Metadata is not handled by the
		/// undo/redo command chain.
		/// </summary>
		public XmlDocument MetaData
		{
			get
			{
				if (_MetaData == null)
				{
					_MetaData = new XmlDocument();
					_MetaData.AppendChild(_MetaData.CreateElement("meta"));
				}
				return _MetaData;
			}
		}

		/// <summary>
		/// Retrieves the root element of the metadata document. This should always
		/// be "meta".
		/// </summary>
		public XmlElement MetaDataRoot
		{
			get
			{
				if (_MetaData == null)
				{
					_MetaData = new XmlDocument();
					_MetaData.AppendChild(_MetaData.CreateElement("meta"));
				}
				return _MetaData.DocumentElement;
			}
		}

		/// <summary>
		/// Default constructor.
		/// </summary>
		public CModel()
		{
		}

		/// <summary>
		/// Begins a new undo/redo session. All changes on the model within
		/// this session will be stored in a command object.
		/// </summary>
		public void BeginUndoRedoSession()
		{
			if (CommandGroup == null)
			{
				CommandGroup = new CCommandGroup();
			}
		}

		/// <summary>
		/// Ends the current undo/redo session. All changes on the model within
		/// this session is returned as a command object.
		/// </summary>
		/// <returns>The generated undo/redo command object</returns>
		public ICommand EndUndoRedoSession()
		{
			if (CommandGroup != null)
			{
				CCommandGroup commandGroup = CommandGroup;
				CommandGroup = null;
				return commandGroup;
			}
			return null;
		}

		/// <summary>
		/// Generates a string version of the model.
		/// </summary>
		/// <returns>The generated string</returns>
		public override string ToString()
		{
			return "Model \"" + Name + "\"";
		}

		internal void AddSetModelFieldCommand<T>(string FieldName, T Value)
		{
			if (CommandGroup != null)
			{
				CommandGroup.Add(new CSetModelField<T>(this, FieldName, Value));
			}
		}

		internal int GetAttachmentNodeId(CAttachment Attachment)
		{
			if (_Attachments == null)
			{
				return -1;
			}
			int num = _Attachments.IndexOf(Attachment);
			if (num == -1)
			{
				return -1;
			}
			if (_Bones != null)
			{
				num += _Bones.Count;
			}
			if (_Lights != null)
			{
				num += _Lights.Count;
			}
			if (_Helpers != null)
			{
				num += _Helpers.Count;
			}
			return num;
		}

		internal int GetBoneNodeId(CBone Bone)
		{
			if (_Bones == null)
			{
				return -1;
			}
			int num = _Bones.IndexOf(Bone);
			if (num == -1)
			{
				return -1;
			}
			return num;
		}

		internal int GetCollisionShapeNodeId(CCollisionShape CollisionShape)
		{
			if (_CollisionShapes == null)
			{
				return -1;
			}
			int num = _CollisionShapes.IndexOf(CollisionShape);
			if (num == -1)
			{
				return -1;
			}
			if (_Bones != null)
			{
				num += _Bones.Count;
			}
			if (_Lights != null)
			{
				num += _Lights.Count;
			}
			if (_Helpers != null)
			{
				num += _Helpers.Count;
			}
			if (_Attachments != null)
			{
				num += _Attachments.Count;
			}
			if (_ParticleEmitters != null)
			{
				num += _ParticleEmitters.Count;
			}
			if (_ParticleEmitters2 != null)
			{
				num += _ParticleEmitters2.Count;
			}
			if (_RibbonEmitters != null)
			{
				num += _RibbonEmitters.Count;
			}
			if (_Events != null)
			{
				num += _Events.Count;
			}
			return num;
		}

		internal int GetEventNodeId(CEvent Event)
		{
			if (_Events == null)
			{
				return -1;
			}
			int num = _Events.IndexOf(Event);
			if (num == -1)
			{
				return -1;
			}
			if (_Bones != null)
			{
				num += _Bones.Count;
			}
			if (_Lights != null)
			{
				num += _Lights.Count;
			}
			if (_Helpers != null)
			{
				num += _Helpers.Count;
			}
			if (_Attachments != null)
			{
				num += _Attachments.Count;
			}
			if (_ParticleEmitters != null)
			{
				num += _ParticleEmitters.Count;
			}
			if (_ParticleEmitters2 != null)
			{
				num += _ParticleEmitters2.Count;
			}
			if (_RibbonEmitters != null)
			{
				num += _RibbonEmitters.Count;
			}
			return num;
		}

		internal int GetHelperNodeId(CHelper Helper)
		{
			if (_Helpers == null)
			{
				return -1;
			}
			int num = _Helpers.IndexOf(Helper);
			if (num == -1)
			{
				return -1;
			}
			if (_Bones != null)
			{
				num += _Bones.Count;
			}
			if (_Lights != null)
			{
				num += _Lights.Count;
			}
			return num;
		}

		internal int GetLightNodeId(CLight Light)
		{
			if (_Lights == null)
			{
				return -1;
			}
			int num = _Lights.IndexOf(Light);
			if (num == -1)
			{
				return -1;
			}
			if (_Bones != null)
			{
				num += _Bones.Count;
			}
			return num;
		}

		internal int GetParticleEmitterNodeId(CParticleEmitter ParticleEmitter)
		{
			if (_ParticleEmitters == null)
			{
				return -1;
			}
			int num = _ParticleEmitters.IndexOf(ParticleEmitter);
			if (num == -1)
			{
				return -1;
			}
			if (_Bones != null)
			{
				num += _Bones.Count;
			}
			if (_Lights != null)
			{
				num += _Lights.Count;
			}
			if (_Helpers != null)
			{
				num += _Helpers.Count;
			}
			if (_Attachments != null)
			{
				num += _Attachments.Count;
			}
			return num;
		}

		internal int GetParticleEmitter2NodeId(CParticleEmitter2 ParticleEmitter2)
		{
			if (_ParticleEmitters2 == null)
			{
				return -1;
			}
			int num = _ParticleEmitters2.IndexOf(ParticleEmitter2);
			if (num == -1)
			{
				return -1;
			}
			if (_Bones != null)
			{
				num += _Bones.Count;
			}
			if (_Lights != null)
			{
				num += _Lights.Count;
			}
			if (_Helpers != null)
			{
				num += _Helpers.Count;
			}
			if (_Attachments != null)
			{
				num += _Attachments.Count;
			}
			if (_ParticleEmitters != null)
			{
				num += _ParticleEmitters.Count;
			}
			return num;
		}

		internal int GetRibbonEmitterNodeId(CRibbonEmitter RibbonEmitter)
		{
			if (_RibbonEmitters == null)
			{
				return -1;
			}
			int num = _RibbonEmitters.IndexOf(RibbonEmitter);
			if (num == -1)
			{
				return -1;
			}
			if (_Bones != null)
			{
				num += _Bones.Count;
			}
			if (_Lights != null)
			{
				num += _Lights.Count;
			}
			if (_Helpers != null)
			{
				num += _Helpers.Count;
			}
			if (_Attachments != null)
			{
				num += _Attachments.Count;
			}
			if (_ParticleEmitters != null)
			{
				num += _ParticleEmitters.Count;
			}
			if (_ParticleEmitters2 != null)
			{
				num += _ParticleEmitters2.Count;
			}
			return num;
		}
	}
}
