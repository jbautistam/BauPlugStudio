using System;
using System.IO;

using Bau.Libraries.BookLibrary.Model.Books;

namespace Bau.Libraries.BookLibrary.Application.Bussiness
{
	/// <summary>
	///		Clase de negocio de <see cref="BookModel"/>
	/// </summary>
	public class BookBussiness
	{
		/// <summary>
		///		Carga los libros de un directorio
		/// </summary>
		public BookModelCollection Load(string path)
		{
			BookModelCollection books = new BookModelCollection();
			BookFormatModelCollection formats = new BookFormatModelCollection();

				// Rellena los formatos
				formats.FillBookFormats();
				// Carga los libros
				if (Directory.Exists(path))
				{
					string[] files = Directory.GetFiles(path, "*.*");

						// Añade los libros
						foreach (string file in files)
						{
							BookModel.BookType type = formats.GetBookType(file);

								if (type != BookModel.BookType.Unknown)
								{
									BookModel book = new BookModel();

										// Asigna las propiedades
										book.FileName = file;
										book.Name = Path.GetFileNameWithoutExtension(file);
										book.IDType = type;
										// Añade el libro a la colección
										books.Add(book);
								}
						}
				}
				// Devuelve la colección de libros
				return books;
		}

		/// <summary>
		///		Elimina un libro
		/// </summary>
		public void Delete(BookModel book)
		{
			LibCommonHelper.Files.HelperFiles.KillFile(book.FileName);
		}

		/// <summary>
		///		Interpreta un libro
		/// </summary>
		public BookPageModelCollection Parse(BookModel book, string pathTarget)
		{
			BookPageModelCollection pages = new BookPageModelCollection();

				// Obtiene las páginas
				switch (book.IDType)
				{
					case BookModel.BookType.Book:
							pages = ConvertPagesEPub(book, pathTarget);
						break;
					case BookModel.BookType.Comic:
							pages = ConvertPagesComic(book, pathTarget);
						break;
				}
				// Normaliza las páginas
				pages.NormalizePages();
				// Devuelve las páginas
				return pages;
		}

		/// <summary>
		///		Convierte las páginas de un cómic
		/// </summary>
		private BookPageModelCollection ConvertPagesComic(BookModel book, string pathTarget)
		{
			LibComicsBooks.ComicBook comic = new LibComicsBooks.ComicBook();
			BookPageModelCollection pages = new BookPageModelCollection();

				// Descomprime el archivo
				comic.Load(book.FileName);
				// Asigna las páginas
				for (int index = 0; index < comic.Pages.Count; index++)
				{
					BookPageModel page = new BookPageModel();

						// Asigna los datos
						page.Name = $"Página {index + 1}";
						page.FileName = comic.Pages[index].FileName;
						// Añade la página
						pages.Add(page);
				}
				// Devuelve la colección de páginas
				return pages;
		}

		/// <summary>
		///		Convierte las páginas de un libro en formato ePub
		/// </summary>
		private BookPageModelCollection ConvertPagesEPub(BookModel book, string pathTarget)
		{
			LibEBook.Formats.eBook.Book ePub = new LibEBook.BookFactory().Load(LibEBook.BookFactory.BookType.ePub, book.FileName, pathTarget);
			BookPageModelCollection pages = new BookPageModelCollection();

				// Crea las páginas
				pages.AddRange(ConvertPagesEPub(ePub.Index));
				// Devuelve la colección de páginas
				return pages;
		}

		/// <summary>
		///		Convierte las páginas
		/// </summary>
		private BookPageModelCollection ConvertPagesEPub(LibEBook.Formats.eBook.IndexItemsCollection ePubPages)
		{
			BookPageModelCollection pages = new BookPageModelCollection();

				// Añade las páginas
				foreach (LibEBook.Formats.eBook.IndexItem ePubPage in ePubPages)
				{
					BookPageModel page = new BookPageModel();

						// Asigna los datos
						page.Name = ePubPage.Name;
						page.FileName = ePubPage.URL;
						page.Pages.AddRange(ConvertPagesEPub(ePubPage.Items));
						// Añade la página a la colección
						pages.Add(page);
				}
				// Devuelve la colección de páginas
				return pages;
		}
	}
}
