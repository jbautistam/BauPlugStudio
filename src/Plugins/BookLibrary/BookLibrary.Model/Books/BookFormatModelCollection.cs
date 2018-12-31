using System;

namespace Bau.Libraries.BookLibrary.Model.Books
{
	/// <summary>
	///		Colección de <see cref="BookFormatModel"/>
	/// </summary>
	public class BookFormatModelCollection : LibDataStructures.Base.BaseExtendedModelCollection<BookFormatModel>
	{
		/// <summary>
		///		Añade un tipo de libro a la colección
		/// </summary>
		public void Add(string name, string description, string extension, BookModel.BookType type)
		{
			Add(new BookFormatModel
			{
				Name = name,
				Description = description,
				Extension = extension,
				BookType = type
			});
		}

		/// <summary>
		///		Asigna los formatos de libros que puede utilizar la aplicación
		/// </summary>
		public void FillBookFormats()
		{ 
			// eBooks
			Add("eBook - ePub", "eBook en formato ePub", "epub", BookModel.BookType.Book);
			// Cómics
			Add("Cómic - cbz", "Cómic en formato cbz", "cbz", BookModel.BookType.Comic);
			Add("Cómic - cbr", "Cómic en formato cbr", "cbr", BookModel.BookType.Comic);
			Add("Cómic - cbt", "Cómic en formato cbt", "cbt", BookModel.BookType.Comic);
			Add("Cómic - zip", "Cómic en formato zip", "zip", BookModel.BookType.Comic);
			Add("Cómic - rar", "Cómic en formato rar", "rar", BookModel.BookType.Comic);
			Add("Cómic - tar", "Cómic en formato tar", "tar", BookModel.BookType.Comic);
			Add("PDF", "Archivo PDF", "pdf", BookModel.BookType.Comic);
		}

		/// <summary>
		///		Obtiene los formatos de determinado tipo de la colección
		/// </summary>
		public BookFormatModelCollection SearchByType(BookModel.BookType type)
		{
			BookFormatModelCollection formats = new BookFormatModelCollection();

				// Obtiene los formatos
				foreach (BookFormatModel format in this)
					if (format.BookType == type)
						formats.Add(format);
				// Devuelve la colección
				return formats;
		}

		/// <summary>
		///		Obtiene un tipo de libro a partir de un nombre de archivo
		/// </summary>
		public BookModel.BookType GetBookType(string file)
		{ 
			// Busca el formato del libro a partir de la extensión
			foreach (BookFormatModel format in this)
				if (file.EndsWith(format.Extension, StringComparison.CurrentCultureIgnoreCase))
					return format.BookType;
			// Si ha llegado hasta aquí es porque no ha encontrado nada
			return BookModel.BookType.Unknown;
		}
	}
}
