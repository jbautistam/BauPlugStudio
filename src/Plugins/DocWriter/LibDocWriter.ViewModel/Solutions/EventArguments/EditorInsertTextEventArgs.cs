using System;

namespace Bau.Libraries.LibDocWriter.ViewModel.Solutions.EventArguments
{
	/// <summary>
	///		Argumentos de inserción de texto en el editor
	/// </summary>
	public class EditorInsertTextEventArgs : EventArgs
	{
		public EditorInsertTextEventArgs(string text, int offset = 0)
		{
			Text = text;
			Offset = offset;
		}

		/// <summary>
		///		Texto
		/// </summary>
		public string Text { get; }

		/// <summary>
		///		Desplazamiento del cursor
		/// </summary>
		public int Offset { get; }
	}
}
