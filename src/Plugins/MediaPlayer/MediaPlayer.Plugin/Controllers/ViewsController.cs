using System;

using Bau.Libraries.LibMediaPlayer.ViewModel.Blogs;
using Bau.Libraries.BauMvvm.ViewModels.Controllers;

namespace Bau.Plugins.MediaPlayer.Controllers
{
	/// <summary>
	///		Controlador de ventanas multimedia
	/// </summary>
	public class ViewsController : Libraries.LibMediaPlayer.ViewModel.Controllers.IViewsController
	{
		/// <summary>
		///		Abre la ventana de mantenimiento de un álbum
		/// </summary>
		public SystemControllerEnums.ResultType OpenUpdateAlbumView(AlbumViewModel viewModel)
		{
			return MediaPlayerPlugin.MainInstance.HostPluginsController.HostViewsController.ShowDialog(new Views.BlogView(viewModel));
		}

		/// <summary>
		///		Abre la ventana de mantenimiento de una carpeta
		/// </summary>
		public SystemControllerEnums.ResultType OpenUpdateFolderView(FolderViewModel viewModel)
		{
			return MediaPlayerPlugin.MainInstance.HostPluginsController.HostViewsController.ShowDialog(new Views.FolderView(viewModel));
		}
	}
}
