using System;

namespace Bau.Libraries.LibMediaPlayer.ViewModel.Controllers.EventArguments
{
	/// <summary>
	///		Argumentos de un tratamiento de archivo
	/// </summary>
	public class FileEventArguments : EventArgs
	{
		public FileEventArguments(System.Collections.Generic.List<string> files)
		{
			Files.AddRange(files);
		}

		/// <summary>
		///		Nombre de archivo
		/// </summary>
		public System.Collections.Generic.List<string> Files { get; } = new System.Collections.Generic.List<string>();
	}
}
