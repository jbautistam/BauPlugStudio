using System;

using Bau.Libraries.BauMvvm.ViewModels.Forms.ControlItems.Trees;
using Bau.Libraries.BookLibrary.Model.Books;

namespace Bau.Libraries.BookLibrary.ViewModel.Books.Content.TreePages
{
	/// <summary>
	///		ViewModel para el árbol de páginas de un libro
	/// </summary>
	public class TreePagesViewModel : TreeViewModel<PageNodeViewModel>
	{   
		// Variables privadas
		private BookPageModelCollection _pages;

		public TreePagesViewModel(BookPageModelCollection pages)
		{
			_pages = pages;
		}

		/// <summary>
		///		Carga los nodos
		/// </summary>
		protected override void LoadNodesData()
		{
			foreach (BookPageModel page in _pages)
				Children.Add(new PageNodeViewModel(null, page));
		}
	}
}