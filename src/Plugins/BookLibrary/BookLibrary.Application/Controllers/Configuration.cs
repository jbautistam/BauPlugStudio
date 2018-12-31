using System;

namespace Bau.Libraries.BookLibrary.Application.Controllers
{
	/// <summary>
	///		Clase de configuración
	/// </summary>
	public class Configuration
	{
		public Configuration()
		{
			BookFormats.FillBookFormats();
		}

		/// <summary>
		///		Directorio donde se encuentra la biblioteca
		/// </summary>
		public string PathLibrary { get; set; }

		/// <summary>
		///		Formatos de libros que puede utilizar la aplicación
		/// </summary>
		public Model.Books.BookFormatModelCollection BookFormats { get; } = new Model.Books.BookFormatModelCollection();
	}
}
