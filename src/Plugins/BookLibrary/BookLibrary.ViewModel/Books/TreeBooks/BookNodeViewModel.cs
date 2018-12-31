using System;

using Bau.Libraries.BookLibrary.Model.Books;

namespace Bau.Libraries.BookLibrary.ViewModel.Books.TreeBooks
{
	/// <summary>
	///		Nodo del árbol para un <see cref="BookModel"/>
	/// </summary>
	public class BookNodeViewModel : BaseNodeViewModel
	{
		public BookNodeViewModel(BookModel book, LibraryNodeViewModel nodeLibrary)
								: base(nodeLibrary, book.Name, book, false)
		{
			Book = book;
		}

		/// <summary>
		///		Libro
		/// </summary>
		public BookModel Book { get; }
	}
}
