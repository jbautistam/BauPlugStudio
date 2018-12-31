using System;

namespace Bau.Libraries.SourceEditor.ViewModel.Solutions.EventArguments
{
	/// <summary>
	///		Argumentos de los eventos de copia de archivos
	/// </summary>
	public class EndFileCopyEventArgs : EventArgs
	{
		public EndFileCopyEventArgs(string[] filesSource, string[] filesTarget)
		{
			FilesSource = filesSource;
			FilesTarget = filesTarget;
		}

		/// <summary>
		///		Nombres de archivos origen
		/// </summary>
		public string[] FilesSource { get; }

		/// <summary>
		///		Nombres de archivos destino
		/// </summary>
		public string[] FilesTarget { get; }
	}
}
