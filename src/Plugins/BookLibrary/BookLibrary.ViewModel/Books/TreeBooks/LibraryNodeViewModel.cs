using System;

using Bau.Libraries.BookLibrary.Model.Books;

namespace Bau.Libraries.BookLibrary.ViewModel.Books.TreeBooks
{
	/// <summary>
	///		Nodo del árbol <see cref="LibraryModel"/>
	/// </summary>
	public class LibraryNodeViewModel : BaseNodeViewModel
	{
		public LibraryNodeViewModel(LibraryModel library, LibraryNodeViewModel nodeLibrary)
								: base(nodeLibrary, library.PathName, library, true)
		{
			Library = library;
			IsBold = true;
		}

		/// <summary>
		///		Carga los hijos del nodo
		/// </summary>
		public override void LoadChildrenData()
		{
			LibraryModelCollection libraries = new Application.Bussiness.LibraryBussiness().Load(Library.Path);
			BookModelCollection books = new Application.Bussiness.BookBussiness().Load(Library.Path);

				// Carga las carpetas hija
				foreach (LibraryModel library in libraries)
					Children.Add(new LibraryNodeViewModel(library, this));
				// Carga los libros hijo
				books.SortByName();
				foreach (BookModel book in books)
					Children.Add(new BookNodeViewModel(book, this));
		}

		/// <summary>
		///		Biblioteca
		/// </summary>
		public LibraryModel Library { get; }
	}
}
