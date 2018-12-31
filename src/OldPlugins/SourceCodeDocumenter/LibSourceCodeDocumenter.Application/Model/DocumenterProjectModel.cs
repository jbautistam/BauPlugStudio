using System;

namespace Bau.Libraries.LibSourceCodeDocumenter.Application.Model
{
	/// <summary>
	///		Proyecto de documentación
	/// </summary>
	public class DocumenterProjectModel : LibDataStructures.Base.BaseExtendedModel
	{
		/// <summary>
		///		Directorio de generación de la documentación
		/// </summary>
		public string PathGenerate { get; set; }

		/// <summary>
		///		Directorio de páginas adicionales
		/// </summary>
		public string PathPages { get; set; }

		/// <summary>
		///		Directorio donde se encuentran las plantillas
		/// </summary>
		public string PathTemplates { get; set; }

		/// <summary>
		///		Indica si se deben documentar las estructuras internas
		/// </summary>
		public bool ShowInternal { get; set; } = true;

		/// <summary>
		///		Indica si se deben documentar las estructuras privadas
		/// </summary>
		public bool ShowPrivate { get; set; } = true;

		/// <summary>
		///		Indica si se deben documentar las estructuras protegidas
		/// </summary>
		public bool ShowProtected { get; set; } = true;

		/// <summary>
		///		Indica si se deben documentar las estructuras públicas
		/// </summary>
		public bool ShowPublic { get; set; } = true;
	}
}
