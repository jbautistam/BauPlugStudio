using System;

namespace Bau.Libraries.FtpManager.ViewModel.FtpExplorer.EventArguments
{
	/// <summary>
	///		Argumento de los eventos de tratamiento de archivos
	/// </summary>
	public class FileEventArgs : EventArgs
	{
		public FileEventArgs(string fileName)
		{
			FileName = fileName;
		}

		/// <summary>
		///		Nombre de archivo
		/// </summary>
		public string FileName { get; }
	}
}
