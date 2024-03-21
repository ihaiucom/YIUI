using MdxLib.Animator;
using MdxLib.Model;
using MdxLib.ModelFormats.Mdx.Value;

namespace MdxLib.ModelFormats.Mdx
{
	internal abstract class CObject : CUnknown
	{
		public CObject()
		{
		}

		public static void LoadAnimator<T>(CLoader Loader, MdxLib.Model.CModel Model, CAnimator<T> Animator, IValue<T> ValueHandler) where T : new()
		{
			Animator.MakeAnimated();
			int num = Loader.ReadInt32();
			int num2 = Loader.ReadInt32();
			Loader.Attacher.AddObject(Model.GlobalSequences, Animator.GlobalSequence, Loader.ReadInt32());
			switch (num2)
			{
			case 0:
				Animator.Type = EInterpolationType.None;
				break;
			case 1:
				Animator.Type = EInterpolationType.Linear;
				break;
			case 2:
				Animator.Type = EInterpolationType.Hermite;
				break;
			case 3:
				Animator.Type = EInterpolationType.Bezier;
				break;
			}
			for (int i = 0; i < num; i++)
			{
				int time = Loader.ReadInt32();
				T value = ValueHandler.Read(Loader);
				if (num2 > 1)
				{
					T inTangent = ValueHandler.Read(Loader);
					T outTangent = ValueHandler.Read(Loader);
					Animator.Add(new CAnimatorNode<T>(time, value, inTangent, outTangent));
				}
				else
				{
					Animator.Add(new CAnimatorNode<T>(time, value));
				}
			}
		}

		public static void SaveAnimator<T>(CSaver Saver, MdxLib.Model.CModel Model, CAnimator<T> Animator, IValue<T> ValueHandler, string Tag) where T : new()
		{
			int num = 0;
			if (Animator.Static)
			{
				return;
			}
			switch (Animator.Type)
			{
			case EInterpolationType.None:
				num = 0;
				break;
			case EInterpolationType.Linear:
				num = 1;
				break;
			case EInterpolationType.Hermite:
				num = 2;
				break;
			case EInterpolationType.Bezier:
				num = 3;
				break;
			}
			Saver.WriteTag(Tag);
			Saver.WriteInt32(Animator.Count);
			Saver.WriteInt32(num);
			Saver.WriteInt32(Animator.GlobalSequence.ObjectId);
			foreach (CAnimatorNode<T> item in Animator)
			{
				Saver.WriteInt32(item.Time);
				ValueHandler.Write(Saver, item.Value);
				if (num > 1)
				{
					ValueHandler.Write(Saver, item.InTangent);
					ValueHandler.Write(Saver, item.OutTangent);
				}
			}
		}
	}
}
