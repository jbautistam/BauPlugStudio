using System;

using Bau.Libraries.BauMvvm.ViewModels.Controllers;
using Bau.Libraries.LibBlogReader.ViewModel.Blogs;

namespace Bau.Libraries.LibBlogReader.ViewModel.Controllers
{
	/// <summary>
	///		Interface con los controladores de las vistas
	/// </summary>
	public interface IViewsController
	{
		/// <summary>
		///		Abre la ventana de propiedades de un blog
		/// </summary>
		SystemControllerEnums.ResultType OpenUpdateBlogView(BlogViewModel viewModel);

		/// <summary>
		///		Abre la ventana de propiedades de una carpeta
		/// </summary>
		SystemControllerEnums.ResultType OpenUpdateFolderView(FolderViewModel viewModel);

		/// <summary>
		///		Abre la ventana que muestra las noticias asociadas a un blog
		/// </summary>
		void OpenSeeNewsBlog(Blogs.TreeBlogs.BaseNodeViewModel node);

		/// <summary>
		///		Abre la ventana que muestra el árbol de blogs
		/// </summary>
		void OpenTreeBlogsView();
	}
}
