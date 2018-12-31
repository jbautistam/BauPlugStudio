using System;

namespace Bau.Libraries.LibMediaPlayer.Application.Bussiness.EventArguments
{
	/// <summary>
	///		Argumentos del evento de descarga de archivos
	/// </summary>
	public class MediaFileDownloadEventArgs : EventArgs
	{
		public MediaFileDownloadEventArgs(Model.MediaFileModel file, int actual, int total)
		{
			File = file;
			Actual = actual;
			Total = total;
		}

		/// <summary>
		///		Archivo
		/// </summary>
		public Model.MediaFileModel File { get; }

		/// <summary>
		///		Número actual de archivos
		/// </summary>
		public int Actual { get; }

		/// <summary>
		///		Número total de archivos
		/// </summary>
		public int Total { get; }
	}
}
