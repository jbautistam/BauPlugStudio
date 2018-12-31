using System;

namespace Bau.Libraries.LibBlogReader.Model
{
	/// <summary>
	///		Datos de una entrada del blog
	/// </summary>
	public class EntryModel : LibDataStructures.Base.BaseExtendedModel
	{ 
		// Enumerados públicos
		/// <summary>Estados de una entrada</summary>
		public enum StatusEntry
		{
			/// <summary>Desconocido</summary>
			Unknow,
			/// <summary>No leído</summary>
			NotRead,
			/// <summary>Leído</summary>
			Read,
			/// <summary>Marcado como interesante</summary>
			Interesting,
			/// <summary>Borrado</summary>
			Deleted
		}

		/// <summary>
		///		URL donde ese encuentra la entrada
		/// </summary>
		public string URL
		{
			get { return GlobalId; }
			set { GlobalId = value; }
		}

		/// <summary>
		///		Url del archivo adjunto
		/// </summary>
		public string UrlEnclosure { get; set; }

		/// <summary>
		///		Nombre del archivo descargado
		/// </summary>
		public string DownloadFileName { get; set; }

		/// <summary>
		///		Contenido de la entrada
		/// </summary>
		public string Content { get; set; }

		/// <summary>
		///		Autor de la entrada
		/// </summary>
		public string Author { get; set; }

		/// <summary>
		///		Fecha de publicación
		/// </summary>
		public DateTime DatePublish { get; set; } = DateTime.Now;

		/// <summary>
		///		Estado de la entrada
		/// </summary>
		public StatusEntry Status { get; set; } = StatusEntry.NotRead;

		/// <summary>
		///		Blog
		/// </summary>
		public BlogModel Blog { get; set; }
	}
}
