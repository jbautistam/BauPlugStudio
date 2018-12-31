using System;

using Bau.Libraries.BauMvvm.ViewModels.Forms.ControlItems;

namespace Bau.Libraries.BookLibrary.ViewModel.Books.TreeBooks
{
	/// <summary>
	///		Clase base para los nodos del árbol <see cref="PaneTreeBooksViewModel"/>
	/// </summary>
	public abstract class BaseNodeViewModel : ControlHierarchicalViewModel
	{
		public BaseNodeViewModel(ControlHierarchicalViewModel parent, string text, object tag, bool lazyLoadChildren = true)
							: base(parent, text, tag, lazyLoadChildren)
		{
		}
	}
}
