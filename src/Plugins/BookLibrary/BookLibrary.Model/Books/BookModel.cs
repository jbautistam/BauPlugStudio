using System;

namespace Bau.Libraries.BookLibrary.Model.Books
{
	/// <summary>
	///		Modelo con los datos de un libro
	/// </summary>
	public class BookModel : LibDataStructures.Base.BaseExtendedModel
	{
		/// <summary>
		///		Tipo de libro
		/// </summary>
		public enum BookType
		{
			/// <summary>Desconocido</summary>
			Unknown,
			/// <summary>Libro</summary>
			Book,
			/// <summary>Cómic</summary>
			Comic
		}

		/// <summary>
		///		Tipo de libro
		/// </summary>
		public BookType IDType { get; set; }

		/// <summary>
		///		Nombre del archivo
		/// </summary>
		public string FileName { get; set; }
	}
}
