using System.Collections.Generic;

namespace MdxLib.Command
{
	internal sealed class CCommandGroup : ICommand
	{
		private LinkedList<ICommand> CommandList;

		public CCommandGroup()
		{
			CommandList = new LinkedList<ICommand>();
		}

		public void Add(ICommand Command)
		{
			CommandList.AddLast(Command);
		}

		public void Do()
		{
			for (LinkedListNode<ICommand> linkedListNode = CommandList.First; linkedListNode != null; linkedListNode = linkedListNode.Next)
			{
				linkedListNode.Value.Do();
			}
		}

		public void Undo()
		{
			for (LinkedListNode<ICommand> linkedListNode = CommandList.Last; linkedListNode != null; linkedListNode = linkedListNode.Previous)
			{
				linkedListNode.Value.Undo();
			}
		}
	}
}
