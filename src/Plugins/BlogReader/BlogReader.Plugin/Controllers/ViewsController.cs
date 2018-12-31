using System;

using Bau.Libraries.LibBlogReader.ViewModel.Blogs;
using Bau.Libraries.BauMvvm.ViewModels.Controllers;
using Bau.Libraries.Plugins.Views.Host;

namespace Bau.Plugins.BlogReader.Controllers
{
	/// <summary>
	///		Controlador de ventanas del lector de blogs
	/// </summary>
	public class ViewsController : Libraries.LibBlogReader.ViewModel.Controllers.IViewsController
	{
		/// <summary>
		///		Abre la ventana de mantenimiento de un blog
		/// </summary>
		public SystemControllerEnums.ResultType OpenUpdateBlogView(BlogViewModel viewModel)
		{
			return BlogReaderPlugin.MainInstance.HostPluginsController.HostViewsController.ShowDialog(new Views.BlogView(viewModel));
		}

		/// <summary>
		///		Abre la ventana de mantenimiento de una carpeta
		/// </summary>
		public SystemControllerEnums.ResultType OpenUpdateFolderView(FolderViewModel viewModel)
		{
			return BlogReaderPlugin.MainInstance.HostPluginsController.HostViewsController.ShowDialog(new Views.FolderView(viewModel));
		}

		/// <summary>
		///		Abre la ventana para ver las noticias de un blog o una carpeta
		/// </summary>
		public void OpenSeeNewsBlog(Libraries.LibBlogReader.ViewModel.Blogs.TreeBlogs.BaseNodeViewModel node)
		{
			if (node != null)
				BlogReaderPlugin.MainInstance.HostPluginsController.LayoutController.ShowDocument("NEWS_" + node.Text, "Noticias", new Views.BlogSeeNewsControlView(node));
		}

		/// <summary>
		///		Abre la ventana que muestra el árbol de blogs
		/// </summary>
		public void OpenTreeBlogsView()
		{
			BlogReaderPlugin.MainInstance.HostPluginsController.LayoutController.ShowDockPane("BLOGS_TREE", LayoutEnums.DockPosition.Left,
																							  "Blogs", new Views.BlogTreeControlView());
		}
	}
}
