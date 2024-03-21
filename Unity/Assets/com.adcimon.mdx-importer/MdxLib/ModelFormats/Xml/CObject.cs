using System.Xml;
using MdxLib.Animator;
using MdxLib.Model;
using MdxLib.ModelFormats.Xml.Value;

namespace MdxLib.ModelFormats.Xml
{
	internal abstract class CObject : CUnknown
	{
		public CObject()
		{
		}

		public void LoadAnimator<T>(CLoader Loader, XmlNode Node, MdxLib.Model.CModel Model, CAnimator<T> Animator, IValue<T> ValueHandler, string Name) where T : new()
		{
			XmlElement xmlElement = Node.SelectSingleNode(Name) as XmlElement;
			if (xmlElement == null)
			{
				return;
			}
			T defaultValue = new T();
			if (Bool(xmlElement.GetAttribute("animated"), DefaultValue: false))
			{
				Animator.MakeAnimated();
				Animator.Type = StringToType(ReadString(xmlElement, "type", TypeToString(Animator.Type)));
				Loader.Attacher.AddObject(Model.GlobalSequences, Animator.GlobalSequence, ReadInteger(xmlElement, "global_sequence", -1));
				foreach (XmlNode item in xmlElement.SelectNodes("node"))
				{
					int time = ReadInteger(item, "time", 0);
					T value = ValueHandler.Read(item, "value", Animator.GetValue());
					T inTangent = ValueHandler.Read(item, "in_tangent", defaultValue);
					T outTangent = ValueHandler.Read(item, "out_tangent", defaultValue);
					CAnimatorNode<T> node2 = new CAnimatorNode<T>(time, value, inTangent, outTangent);
					Animator.Add(node2);
				}
			}
			else
			{
				T staticValue = ValueHandler.Read(xmlElement, "static", Animator.GetValue());
				Animator.MakeStatic(staticValue);
			}
		}

		public void SaveAnimator<T>(CSaver Saver, XmlNode Node, MdxLib.Model.CModel Model, CAnimator<T> Animator, IValue<T> ValueHandler, string Name) where T : new()
		{
			XmlElement node = AppendElement(Node, Name);
			AppendAttribute(node, "animated", Animator.Animated ? "1" : "0");
			if (Animator.Animated)
			{
				WriteString(node, "type", TypeToString(Animator.Type));
				WriteInteger(Node, "global_sequence", Animator.GlobalSequence.ObjectId);
				foreach (CAnimatorNode<T> item in Animator)
				{
					XmlNode node2 = AppendElement(node, "node");
					WriteInteger(node2, "time", item.Time);
					ValueHandler.Write(node2, "value", item.Value);
					ValueHandler.Write(node2, "in_tangent", item.InTangent);
					ValueHandler.Write(node2, "out_tangent", item.OutTangent);
				}
			}
			else
			{
				ValueHandler.Write(node, "static", Animator.GetValue());
			}
		}

		private string TypeToString(EInterpolationType Type)
		{
			return Type switch
			{
				EInterpolationType.None => "none", 
				EInterpolationType.Linear => "linear", 
				EInterpolationType.Bezier => "bezier", 
				EInterpolationType.Hermite => "hermite", 
				_ => "", 
			};
		}

		private EInterpolationType StringToType(string String)
		{
			return String switch
			{
				"none" => EInterpolationType.None, 
				"linear" => EInterpolationType.Linear, 
				"bezier" => EInterpolationType.Bezier, 
				"hermite" => EInterpolationType.Hermite, 
				_ => EInterpolationType.None, 
			};
		}
	}
}
