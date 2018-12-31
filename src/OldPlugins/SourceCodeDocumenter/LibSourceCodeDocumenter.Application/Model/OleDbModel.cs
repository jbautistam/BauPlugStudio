using System;

namespace Bau.Libraries.LibSourceCodeDocumenter.Application.Model
{
	/// <summary>
	///		Clase de modelo para un proceso de documentación de código fuente
	/// </summary>
	public class OleDbModel : LibDataStructures.Base.BaseExtendedModel
	{
		/// <summary>
		///		Cadena de conexión
		/// </summary>
		public string ConnectionString { get; set; }
	}
}
