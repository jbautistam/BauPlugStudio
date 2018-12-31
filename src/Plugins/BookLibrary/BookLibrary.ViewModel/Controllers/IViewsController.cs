using System;

using Bau.Libraries.BauMvvm.ViewModels.Controllers;

namespace Bau.Libraries.BookLibrary.ViewModel.Controllers
{
	/// <summary>
	///		Interface con los controladores de las vistas
	/// </summary>
	public interface IViewsController
	{
		/// <summary>
		///		Abre la ventana de mantenimiento de una biblioteca
		/// </summary>
		SystemControllerEnums.ResultType OpenUpdateLibrary(Books.LibraryViewModel viewModel);

		/// <summary>
		///		Abre la ventana que muestra el árbol de libros
		/// </summary>
		void OpenTreeLibrariesView();

		/// <summary>
		///		Abre un libro
		/// </summary>
		void OpenBook(Books.Content.BaseContentViewModel viewModel);

		/// <summary>
		///		Abre el formulario de compilación de un libro
		/// </summary>
		SystemControllerEnums.ResultType OpenFormCompile(Books.Content.eBook.BookCompileViewModel viewModel);
	}
}
