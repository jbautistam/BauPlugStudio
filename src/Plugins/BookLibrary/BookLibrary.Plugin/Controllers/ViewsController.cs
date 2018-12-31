using System;

using Bau.Libraries.BauMvvm.ViewModels.Controllers;
using Bau.Libraries.BookLibrary.ViewModel.Books.Content;
using Bau.Libraries.BookLibrary.ViewModel.Books.Content.Comic;
using Bau.Libraries.Plugins.Views.Host;

namespace Bau.Libraries.BookLibrary.Plugin.Controllers
{
	/// <summary>
	///		Controlador de ventanas del lector
	/// </summary>
	public class ViewsController : ViewModel.Controllers.IViewsController
	{
		/// <summary>
		///		Abre la ventana de mantenimiento de una librería
		/// </summary>
		public SystemControllerEnums.ResultType OpenUpdateLibrary(ViewModel.Books.LibraryViewModel viewModel)
		{
			return BookLibraryPlugin.MainInstance.HostPluginsController.HostViewsController.ShowDialog(new Views.Books.LibraryView(viewModel));
		}

		/// <summary>
		///		Abre el árbol de librerías
		/// </summary>
		public void OpenTreeLibrariesView()
		{
			BookLibraryPlugin.MainInstance.HostPluginsController.LayoutController.ShowDockPane("BOOKS_TREE", LayoutEnums.DockPosition.Left,
																							   "Bibliotecas", new Views.Books.BookTreeExplorerView());
		}

		/// <summary>
		///		Abre un libro
		/// </summary>
		public void OpenBook(BaseContentViewModel viewModel)
		{
			if (viewModel is BookContentViewModel bookViewModel)
				OpenEBook(bookViewModel);
			else if (viewModel is ComicContentViewModel comicViewModel)
				OpenComic(comicViewModel);
		}

		/// <summary>
		///		Abre el formulario de un cómic
		/// </summary>
		private void OpenComic(ComicContentViewModel viewModel)
		{
			BookLibraryPlugin.MainInstance.HostPluginsController.LayoutController.ShowDocument($"COMIC_{viewModel.FileName}", viewModel.Name,
																							   new Views.Comics.ComicContentView(viewModel));
		}

		/// <summary>
		///		Abre el formulario de un libro
		/// </summary>
		private void OpenEBook(BookContentViewModel viewModel)
		{
			BookLibraryPlugin.MainInstance.HostPluginsController.LayoutController.ShowDocument($"BOOK_{viewModel.FileName}", viewModel.Name,
																							   new Views.Books.BookContentView(viewModel));
		}

		/// <summary>
		///		Abre el formulario de compilación de un libro
		/// </summary>
		public SystemControllerEnums.ResultType OpenFormCompile(ViewModel.Books.Content.eBook.BookCompileViewModel viewModel)
		{
			return BookLibraryPlugin.MainInstance.HostPluginsController.HostViewsController.ShowDialog(new Views.Books.BookCompileView(viewModel));
		}
	}
}
