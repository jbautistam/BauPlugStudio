using System;

using Bau.Libraries.BauMvvm.ViewModels.Forms.ControlItems;
using Bau.Libraries.BookLibrary.Model.Books;

namespace Bau.Libraries.BookLibrary.ViewModel.Books.Content.TreePages
{
	/// <summary>
	///		Nodos del árbol <see cref="TreePagesViewModel"/>
	/// </summary>
	public class PageNodeViewModel : ControlHierarchicalViewModel
	{
		public PageNodeViewModel(IHierarchicalViewModel nodePageParent, BookPageModel page)
								: base(nodePageParent, page.Name, page, true)
		{
			Page = page;
		}

		/// <summary>
		///		Carga los hijos del nodo
		/// </summary>
		public override void LoadChildrenData()
		{ 
			foreach (BookPageModel child in Page.Pages)
				Children.Add(new PageNodeViewModel(this, child));
		}

		/// <summary>
		///		Página
		/// </summary>
		public BookPageModel Page { get; }
	}
}
