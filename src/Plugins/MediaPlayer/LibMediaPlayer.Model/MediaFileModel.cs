using System;

namespace Bau.Libraries.LibMediaPlayer.Model
{
	/// <summary>
	///		Datos de un archivo
	/// </summary>
	public class MediaFileModel : LibDataStructures.Base.BaseExtendedModel
	{ 
		// Enumerados públicos
		/// <summary>Estados de una archivo</summary>
		public enum StatusFile
		{
			/// <summary>Desconocido</summary>
			Unknow,
			/// <summary>No leído</summary>
			NotPlayed,
			/// <summary>Leído</summary>
			Played,
			/// <summary>Marcado como interesante</summary>
			Interesting,
			/// <summary>Borrado</summary>
			Deleted
		}

		/// <summary>
		///		Url del archivo
		/// </summary>
		public string Url { get; set; }

		/// <summary>
		///		Nombre del archivo
		/// </summary>
		public string FileName { get; set; }

		/// <summary>
		///		Fecha de alta
		/// </summary>
		public DateTime CreatedAt { get; set; } = DateTime.Now;

		/// <summary>
		///		Estado del archivo
		/// </summary>
		public StatusFile Status { get; set; } = StatusFile.NotPlayed;

		/// <summary>
		///		Album al que pertenece el archivo
		/// </summary>
		public MediaAlbumModel Album { get; set; }
	}
}
