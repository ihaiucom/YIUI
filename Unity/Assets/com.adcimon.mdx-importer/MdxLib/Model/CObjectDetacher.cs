namespace MdxLib.Model
{
	internal sealed class CObjectDetacher<T> : CDetacher where T : CObject<T>
	{
		private T Object = null;

		private CObjectReference<T> Reference;

		public CObjectDetacher(CObjectReference<T> ObjectReference)
		{
			Reference = ObjectReference;
			Object = Reference.Object;
		}

		public override void Detach()
		{
			if (Object != null)
			{
				Reference.ForceDetach();
			}
		}

		public override void Attach()
		{
			if (Object != null)
			{
				Reference.ForceAttach(Object);
			}
		}
	}
}
