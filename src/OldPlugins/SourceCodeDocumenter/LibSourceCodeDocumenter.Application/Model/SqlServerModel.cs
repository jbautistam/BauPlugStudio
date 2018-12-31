using System;

namespace Bau.Libraries.LibSourceCodeDocumenter.Application.Model
{
	/// <summary>
	///		Clase de modelo para un proceso de documentación de código fuente
	/// </summary>
	public class SqlServerModel : LibDataStructures.Base.BaseExtendedModel
	{
		/// <summary>
		///		Servidor
		/// </summary>
		public string Server { get; set; }

		/// <summary>
		///		Base de datos
		/// </summary>
		public string DataBase { get; set; }

		/// <summary>
		///		Indica si se debe utilizar un archivo de base de datos
		/// </summary>
		public bool UseDataBaseFile { get; set; }

		/// <summary>
		///		Archivo de base de datos
		/// </summary>
		public string DataBaseFile { get; set; }

		/// <summary>
		///		Indica si se debe utilizar la autentificación de Windows
		/// </summary>
		public bool UseWindowsAuthentification { get; set; }

		/// <summary>
		///		Usuario
		/// </summary>
		public string User { get; set; }

		/// <summary>
		///		Contraseña
		/// </summary>
		public string Password { get; set; }
	}
}
