namespace MdxLib.Command
{
	/// <summary>
	/// The base interface for all commands that are performed
	/// during an Undo-Redo session.
	/// </summary>
	public interface ICommand
	{
		/// <summary>
		/// Performs the command (do/redo).
		/// </summary>
		void Do();

		/// <summary>
		/// Reverts the command (undo).
		/// </summary>
		void Undo();
	}
}
