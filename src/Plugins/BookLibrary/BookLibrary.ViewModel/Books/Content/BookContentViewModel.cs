using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.BookLibrary.Model.Books;

namespace Bau.Libraries.BookLibrary.ViewModel.Books.Content
{
	/// <summary>
	///		ViewModel para ver el contenido de un <see cref="BookModel"/>
	/// </summary>
	public class BookContentViewModel : BaseContentViewModel
	{   
		// Eventos públicos
		public event EventHandler<Controllers.PageShowEventArgs> PageShow;
		// Variables privadas
		private TreePages.TreePagesViewModel _treePages;

		public BookContentViewModel(BookModel book) : base(book) { }

		/// <summary>
		///		Inicializa la vista del libro
		/// </summary>
		protected override void InitBookView()
		{
			TreePages = new TreePages.TreePagesViewModel(EBookContent);
			TreePages.PropertyChanged += (sender, evntArgs) =>
															{
																if (evntArgs.PropertyName.EqualsIgnoreCase(nameof(TreePages.SelectedNode)))
																	ShowPage(GetPageFromTree(sender as TreePages.TreePagesViewModel));
															};
			TreePages.LoadNodes();
		}

		/// <summary>
		///		Obtiene la página seleccionada del árbol
		/// </summary>
		private TreePages.PageNodeViewModel GetPageFromTree(TreePages.TreePagesViewModel treePagesViewModel)
		{
			if (treePagesViewModel != null && treePagesViewModel.SelectedNode != null &&
					  treePagesViewModel.SelectedNode is TreePages.PageNodeViewModel node)
				return node;
			else
				return null;
		}

		/// <summary>
		///		Muestra una página a partir de un nodo
		/// </summary>
		private void ShowPage(TreePages.PageNodeViewModel pageNode)
		{
			if (pageNode != null && pageNode.Page != null)
				ActualPage = pageNode.Page.PageNumber;
		}

		/// <summary>
		///		Muestra el contenido de la página
		/// </summary>
		protected override void ShowPageReal(int pageIndex)
		{
			BookPageModel page = EBookContent.SearchPage(pageIndex + 1);

				if (PageShow != null && page != null)
					PageShow(this, new Controllers.PageShowEventArgs(System.IO.Path.Combine(PathTarget, page.FileName).Replace("/", "\\")));
		}

		/// <summary>
		///		ViewModel del índice de páginas
		/// </summary>
		public TreePages.TreePagesViewModel TreePages 
		{ 
			get { return _treePages; }
			set { CheckObject(ref _treePages, value); }
		}
	}
}
