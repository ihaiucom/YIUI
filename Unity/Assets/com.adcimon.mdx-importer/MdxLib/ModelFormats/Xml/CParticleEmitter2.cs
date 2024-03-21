using System.Xml;
using MdxLib.Model;
using MdxLib.ModelFormats.Xml.Value;

namespace MdxLib.ModelFormats.Xml
{
	internal sealed class CParticleEmitter2 : CNode
	{
		private static class CSingleton
		{
			public static CParticleEmitter2 Instance;

			static CSingleton()
			{
				Instance = new CParticleEmitter2();
			}
		}

		public static CParticleEmitter2 Instance => CSingleton.Instance;

		private CParticleEmitter2()
		{
		}

		public void Load(CLoader Loader, XmlNode Node, MdxLib.Model.CModel Model, MdxLib.Model.CParticleEmitter2 ParticleEmitter2)
		{
			LoadNode(Loader, Node, Model, ParticleEmitter2);
			ParticleEmitter2.FilterMode = StringToFilterMode(ReadString(Node, "filter_mode", FilterModeToString(ParticleEmitter2.FilterMode)));
			ParticleEmitter2.Rows = ReadInteger(Node, "rows", ParticleEmitter2.Rows);
			ParticleEmitter2.Columns = ReadInteger(Node, "columns", ParticleEmitter2.Columns);
			ParticleEmitter2.PriorityPlane = ReadInteger(Node, "priority_plane", ParticleEmitter2.PriorityPlane);
			ParticleEmitter2.ReplaceableId = ReadInteger(Node, "replaceable_id", ParticleEmitter2.ReplaceableId);
			ParticleEmitter2.Time = ReadFloat(Node, "time", ParticleEmitter2.Time);
			ParticleEmitter2.LifeSpan = ReadFloat(Node, "life_span", ParticleEmitter2.LifeSpan);
			ParticleEmitter2.TailLength = ReadFloat(Node, "tail_length", ParticleEmitter2.TailLength);
			ParticleEmitter2.SortPrimitivesFarZ = ReadBoolean(Node, "sort_primitives_far_z", ParticleEmitter2.SortPrimitivesFarZ);
			ParticleEmitter2.LineEmitter = ReadBoolean(Node, "line_emitter", ParticleEmitter2.LineEmitter);
			ParticleEmitter2.ModelSpace = ReadBoolean(Node, "model_space", ParticleEmitter2.ModelSpace);
			ParticleEmitter2.Unshaded = ReadBoolean(Node, "unshaded", ParticleEmitter2.Unshaded);
			ParticleEmitter2.Unfogged = ReadBoolean(Node, "unfogged", ParticleEmitter2.Unfogged);
			ParticleEmitter2.XYQuad = ReadBoolean(Node, "xy_quad", ParticleEmitter2.XYQuad);
			ParticleEmitter2.Squirt = ReadBoolean(Node, "squirt", ParticleEmitter2.Squirt);
			ParticleEmitter2.Head = ReadBoolean(Node, "head", ParticleEmitter2.Head);
			ParticleEmitter2.Tail = ReadBoolean(Node, "tail", ParticleEmitter2.Tail);
			ParticleEmitter2.Segment1 = ReadSegment(Node, "segment_1", ParticleEmitter2.Segment1);
			ParticleEmitter2.Segment2 = ReadSegment(Node, "segment_2", ParticleEmitter2.Segment2);
			ParticleEmitter2.Segment3 = ReadSegment(Node, "segment_3", ParticleEmitter2.Segment3);
			ParticleEmitter2.HeadLife = ReadInterval(Node, "head_life", ParticleEmitter2.HeadLife);
			ParticleEmitter2.HeadDecay = ReadInterval(Node, "head_decay", ParticleEmitter2.HeadDecay);
			ParticleEmitter2.TailLife = ReadInterval(Node, "tail_life", ParticleEmitter2.TailLife);
			ParticleEmitter2.TailDecay = ReadInterval(Node, "tail_decay", ParticleEmitter2.TailDecay);
			LoadAnimator(Loader, Node, Model, ParticleEmitter2.Speed, CFloat.Instance, "speed");
			LoadAnimator(Loader, Node, Model, ParticleEmitter2.Variation, CFloat.Instance, "variation");
			LoadAnimator(Loader, Node, Model, ParticleEmitter2.Latitude, CFloat.Instance, "latitude");
			LoadAnimator(Loader, Node, Model, ParticleEmitter2.Gravity, CFloat.Instance, "gravity");
			LoadAnimator(Loader, Node, Model, ParticleEmitter2.Visibility, CFloat.Instance, "visibility");
			LoadAnimator(Loader, Node, Model, ParticleEmitter2.EmissionRate, CFloat.Instance, "emission_rate");
			LoadAnimator(Loader, Node, Model, ParticleEmitter2.Width, CFloat.Instance, "width");
			LoadAnimator(Loader, Node, Model, ParticleEmitter2.Length, CFloat.Instance, "length");
			Loader.Attacher.AddObject(Model.Textures, ParticleEmitter2.Texture, ReadInteger(Node, "texture", -1));
		}

		public void Save(CSaver Saver, XmlNode Node, MdxLib.Model.CModel Model, MdxLib.Model.CParticleEmitter2 ParticleEmitter2)
		{
			SaveNode(Saver, Node, Model, ParticleEmitter2);
			WriteString(Node, "filter_mode", FilterModeToString(ParticleEmitter2.FilterMode));
			WriteInteger(Node, "rows", ParticleEmitter2.Rows);
			WriteInteger(Node, "columns", ParticleEmitter2.Columns);
			WriteInteger(Node, "priority_plane", ParticleEmitter2.PriorityPlane);
			WriteInteger(Node, "replaceable_id", ParticleEmitter2.ReplaceableId);
			WriteFloat(Node, "time", ParticleEmitter2.Time);
			WriteFloat(Node, "life_span", ParticleEmitter2.LifeSpan);
			WriteFloat(Node, "tail_length", ParticleEmitter2.TailLength);
			WriteBoolean(Node, "sort_primitives_far_z", ParticleEmitter2.SortPrimitivesFarZ);
			WriteBoolean(Node, "line_emitter", ParticleEmitter2.LineEmitter);
			WriteBoolean(Node, "model_space", ParticleEmitter2.ModelSpace);
			WriteBoolean(Node, "unshaded", ParticleEmitter2.Unshaded);
			WriteBoolean(Node, "unfogged", ParticleEmitter2.Unfogged);
			WriteBoolean(Node, "xy_quad", ParticleEmitter2.XYQuad);
			WriteBoolean(Node, "squirt", ParticleEmitter2.Squirt);
			WriteBoolean(Node, "head", ParticleEmitter2.Head);
			WriteBoolean(Node, "tail", ParticleEmitter2.Tail);
			WriteSegment(Node, "segment_1", ParticleEmitter2.Segment1);
			WriteSegment(Node, "segment_2", ParticleEmitter2.Segment2);
			WriteSegment(Node, "segment_3", ParticleEmitter2.Segment3);
			WriteInterval(Node, "head_life", ParticleEmitter2.HeadLife);
			WriteInterval(Node, "head_decay", ParticleEmitter2.HeadDecay);
			WriteInterval(Node, "tail_life", ParticleEmitter2.TailLife);
			WriteInterval(Node, "tail_decay", ParticleEmitter2.TailDecay);
			SaveAnimator(Saver, Node, Model, ParticleEmitter2.Speed, CFloat.Instance, "speed");
			SaveAnimator(Saver, Node, Model, ParticleEmitter2.Variation, CFloat.Instance, "variation");
			SaveAnimator(Saver, Node, Model, ParticleEmitter2.Latitude, CFloat.Instance, "latitude");
			SaveAnimator(Saver, Node, Model, ParticleEmitter2.Gravity, CFloat.Instance, "gravity");
			SaveAnimator(Saver, Node, Model, ParticleEmitter2.Visibility, CFloat.Instance, "visibility");
			SaveAnimator(Saver, Node, Model, ParticleEmitter2.EmissionRate, CFloat.Instance, "emission_rate");
			SaveAnimator(Saver, Node, Model, ParticleEmitter2.Width, CFloat.Instance, "width");
			SaveAnimator(Saver, Node, Model, ParticleEmitter2.Length, CFloat.Instance, "length");
			WriteInteger(Node, "texture", ParticleEmitter2.Texture.ObjectId);
		}

		private string FilterModeToString(EParticleEmitter2FilterMode FilterMode)
		{
			return FilterMode switch
			{
				EParticleEmitter2FilterMode.Blend => "blend", 
				EParticleEmitter2FilterMode.Additive => "additive", 
				EParticleEmitter2FilterMode.Modulate => "modulate", 
				EParticleEmitter2FilterMode.Modulate2x => "modulate_2x", 
				EParticleEmitter2FilterMode.AlphaKey => "alpha_key", 
				_ => "", 
			};
		}

		private EParticleEmitter2FilterMode StringToFilterMode(string String)
		{
			return String switch
			{
				"blend" => EParticleEmitter2FilterMode.Blend, 
				"additive" => EParticleEmitter2FilterMode.Additive, 
				"modulate" => EParticleEmitter2FilterMode.Modulate, 
				"modulate_2x" => EParticleEmitter2FilterMode.Modulate2x, 
				"alpha_key" => EParticleEmitter2FilterMode.AlphaKey, 
				_ => EParticleEmitter2FilterMode.Blend, 
			};
		}
	}
}
