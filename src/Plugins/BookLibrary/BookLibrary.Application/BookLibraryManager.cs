using System;

namespace Bau.Libraries.BookLibrary.Application
{
	/// <summary>
	///		Manager de la biblioteca de libros
	/// </summary>
	public class BookLibraryManager
	{ 
		// Constantes privadas
		private const string FileID = "BookLibrary";
		// Variables privadas
		private Controllers.Configuration _configuration;

		/// <summary>
		///		Configuración del procesador
		/// </summary>
		public Controllers.Configuration Configuration
		{
			get
			{ 
				// Crea el objeto de configuración si no estaba en memoria
				if (_configuration == null)
					_configuration = new Controllers.Configuration();
				// Devuelve el objeto de configuración
				return _configuration;
			}
		}
	}
}
