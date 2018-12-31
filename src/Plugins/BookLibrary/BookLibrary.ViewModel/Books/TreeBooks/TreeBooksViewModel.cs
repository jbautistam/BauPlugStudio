using System;

using Bau.Libraries.BookLibrary.Application.Bussiness;
using Bau.Libraries.BookLibrary.Model.Books;

namespace Bau.Libraries.BookLibrary.ViewModel.Books.TreeBooks
{
	/// <summary>
	///		Arbol de bibliotecas / libros
	/// </summary>
	public class TreeBooksViewModel : BauMvvm.ViewModels.Forms.ControlItems.Trees.TreeViewModel<BaseNodeViewModel>
	{
		/// <summary>
		///		Carga los nodos
		/// </summary>
		protected override void LoadNodesData()
		{
			LibraryModelCollection libraries = new LibraryBussiness().Load(BookLibraryViewModel.Instance.BooksManager.Configuration.PathLibrary);
			BookModelCollection books = new BookBussiness().Load(BookLibraryViewModel.Instance.BooksManager.Configuration.PathLibrary);

				// Ordena las carpetas y las añade al árbol
				foreach (LibraryModel library in libraries)
					Children.Add(new LibraryNodeViewModel(library, null));
				// Ordena los libros y las añade al árbol
				books.SortByName();
				foreach (BookModel book in books)
					Children.Add(new BookNodeViewModel(book, null));
		}

		/// <summary>
		///		Obtiene la librería padre de la seleccionada
		/// </summary>
		public LibraryModel GetSelectedLibraryParent()
		{
			if (SelectedNode?.Parent != null && SelectedNode.Parent is LibraryNodeViewModel node)
				return node.Library;
			else
				return null;
		}

		/// <summary>
		///		Obtiene la librería seleccionada
		/// </summary>
		public LibraryModel GetSelectedLibrary()
		{
			return (SelectedNode as LibraryNodeViewModel)?.Library;
		}

		/// <summary>
		///		Obtiene el libro seleccionado
		/// </summary>
		public BookModel GetSelectedBook()
		{
			return (SelectedNode as BookNodeViewModel)?.Book;
		}
		/// <summary>
		///		Indica si está seleccionada una biblioteca
		/// </summary>
		public bool IsSelectedLibrary
		{
			get { return SelectedNode != null && SelectedNode is LibraryNodeViewModel; }
		}

		/// <summary>
		///		Indica si está seleccionado un libro
		/// </summary>
		public bool IsSelectedBook
		{
			get { return SelectedNode != null && SelectedNode is BookNodeViewModel; }
		}
	}
}
