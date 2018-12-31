using System;

namespace Bau.Libraries.LibDocWriter.ViewModel.Solutions.EventArguments
{
	/// <summary>
	///		Argumentos de los eventos de copia de archivos
	/// </summary>
	public class EndFileCopyEventArgs : EventArgs
	{   
		// Enumerados públicos
		/// <summary>
		///		Modo de copia de las imágenes
		/// </summary>
		public enum CopyImageType
		{
			/// <summary>Normal. Lista de párrafos</summary>
			Normal,
			/// <summary>Galería</summary>
			Gallery
		}

		public EndFileCopyEventArgs(string [] filesSource, string [] filesTarget, CopyImageType idCopyImageMode)
		{
			FilesSource = filesSource;
			FilesTarget = filesTarget;
			CopyImageMode = idCopyImageMode;
		}

		/// <summary>
		///		Nombres de archivos origen
		/// </summary>
		public string [] FilesSource { get; }

		/// <summary>
		///		Nombres de archivos destino
		/// </summary>
		public string [] FilesTarget { get; }

		/// <summary>
		///		Modo en que se deben copiar las imágenes
		/// </summary>
		public CopyImageType CopyImageMode { get; }
	}
}
