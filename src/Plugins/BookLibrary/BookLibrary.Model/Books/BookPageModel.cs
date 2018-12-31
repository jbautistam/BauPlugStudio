using System;

namespace Bau.Libraries.BookLibrary.Model.Books
{
	/// <summary>
	///		Página de un libro
	/// </summary>
	public class BookPageModel : LibDataStructures.Base.BaseExtendedModel
	{
		/// <summary>
		///		Nombre de archivo
		/// </summary>
		public string FileName { get; set; }

		/// <summary>
		///		Número de página
		/// </summary>
		public int PageNumber { get; set; }

		/// <summary>
		///		Páginas hija
		/// </summary>
		public BookPageModelCollection Pages { get; } = new BookPageModelCollection();
	}
}
