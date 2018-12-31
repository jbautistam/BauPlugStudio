using System;

namespace Bau.Libraries.LibNSharpDoc.Projects.Models
{
	/// <summary>
	///		Proyecto
	/// </summary>
	public class ProjectDocumentationModel
	{   
		// Enumerados públicos
		/// <summary>
		///		Tipo de documentación a generar
		/// </summary>
		public enum DocumentationType
		{
			/// <summary>Archivos Html</summary>
			Html,
			/// <summary>Archivos Nhtml</summary>
			Nhtml,
			/// <summary>Archivos Xml</summary>
			Xml
		}

		/// <summary>
		///		Tipo de documentación
		/// </summary>
		public DocumentationType IDType { get; set; } = DocumentationType.Html;

		/// <summary>
		///		Nombre del archivo
		/// </summary>
		public string FileName { get; set; }

		/// <summary>
		///		Directorio de salida
		/// </summary>
		public string OutputPath { get; set; }

		/// <summary>
		///		Directorio de plantillas
		/// </summary>
		public string TemplatePath { get; set; }

		/// <summary>
		///		Parámetros de configuración
		/// </summary>
		public DocumentationParameters GenerationParameters { get; set; } = new DocumentationParameters();

		/// <summary>
		///		Proveedores
		/// </summary>
		public ProviderModelCollection Providers { get; set; } = new ProviderModelCollection();
	}
}
