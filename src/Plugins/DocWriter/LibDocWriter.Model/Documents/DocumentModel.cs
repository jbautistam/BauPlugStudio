using System;

using Bau.Libraries.LibCommonHelper.Extensors;

namespace Bau.Libraries.LibDocWriter.Model.Documents
{
	/// <summary>
	///		Modelo con los datos de un documento
	/// </summary>
	public class DocumentModel : Base.BaseDocWriterModel
	{ 
		// Enumerados públicos
		/// <summary>
		///		Modo en que se muestran los documentos hijo
		/// </summary>
		public enum ShowChildsMode
		{
			/// <summary>No se muestran</summary>
			None,
			/// <summary>Ordenados por fecha</summary>
			SortByDate,
			/// <summary>Ordenados por página</summary>
			SortByPages
		}
		/// <summary>
		///		Subtipo de documento
		/// </summary>
		public enum ScopeType
		{
			/// <summary>Desconocido</summary>
			Unknown,
			/// <summary>Toda la Web</summary>
			Web,
			/// <summary>Página</summary>
			Page,
			/// <summary>Noticias</summary>
			News,
			/// <summary>Mapa del sitio</summary>
			Sitemap
		}

		public DocumentModel(Solutions.FileModel file)
		{
			File = file;
			Title = file.Title;
			Tags = new Solutions.FilesModelCollection(file.Project);
			ChildPages = new Solutions.FilesModelCollection(file.Project);
		}

		/// <summary>
		///		Archivo del documento
		/// </summary>
		public Solutions.FileModel File { get; }

		/// <summary>
		///		Título
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		///		Palabras clave
		/// </summary>
		public string KeyWords { get; set; }

		/// <summary>
		///		Ambito de documento
		/// </summary>
		public ScopeType IDScope { get; set; }

		/// <summary>
		///		Contenido
		/// </summary>
		public string Content { get; set; }

		/// <summary>
		///		URL de la imagen resumen
		/// </summary>
		public string URLImageSummary { get; set; }

		/// <summary>
		///		Obtiene la Url del thumbnail principal
		/// </summary>
		public string URLThumbImageSummary
		{
			get
			{
				if (URLImageSummary.IsEmpty())
					return "";
				else
					return System.IO.Path.Combine(System.IO.Path.GetDirectoryName(URLImageSummary),
												  "th_" + System.IO.Path.GetFileName(URLImageSummary));
			}
		}

		/// <summary>
		///		Indica si se debe mostrar en las fuentes RSS
		/// </summary>
		public bool ShowAtRSS { get; set; }

		/// <summary>
		///		Modo de visualización de los documentos hijo
		/// </summary>
		public ShowChildsMode ModeShow { get; set; }

		/// <summary>
		///		Indica si debe tratarse la categoría como recursiva
		/// </summary>
		public bool IsRecursive { get; set; }

		/// <summary>
		///		Fecha de alta
		/// </summary>
		public DateTime DateNew { get; set; }

		/// <summary>
		///		Etiquetas
		/// </summary>
		public Solutions.FilesModelCollection Tags { get; set; }

		/// <summary>
		///		Páginas hija
		/// </summary>
		public Solutions.FilesModelCollection ChildPages { get; set; }

		/// <summary>
		///		Plantillas
		/// </summary>
		public TemplatesArrayModel Templates { get; set; } = new TemplatesArrayModel();
	}
}
