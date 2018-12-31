using System;

namespace Bau.Libraries.LibNSharpDoc.Projects.Models
{
	/// <summary>
	///		Parámetros de documentación
	/// </summary>
	public class DocumentationParameters
	{
		/// <summary>
		///		Indica si se documentan las estructuras públicas
		/// </summary>
		public bool ShowPublic { get; set; } = true;

		/// <summary>
		///		Indica si se documentan las estructuras internas
		/// </summary>
		public bool ShowInternal { get; set; } = true;

		/// <summary>
		///		Indica si se documentan las estructuras protegidas
		/// </summary>
		public bool ShowProtected { get; set; } = true;

		/// <summary>
		///		Indica si se documentan las estructuras privadas
		/// </summary>
		public bool ShowPrivate { get; set; } = true;
	}
}