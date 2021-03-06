﻿using System;

using Bau.Libraries.BauMvvm.ViewModels.Controllers;
using Bau.Libraries.LibMediaPlayer.ViewModel.Blogs;

namespace Bau.Libraries.LibMediaPlayer.ViewModel.Controllers
{
	/// <summary>
	///		Interface con los controladores de las vistas
	/// </summary>
	public interface IViewsController
	{
		/// <summary>
		///		Abre la ventana de propiedades de un álbum
		/// </summary>
		SystemControllerEnums.ResultType OpenUpdateAlbumView(AlbumViewModel viewModel);

		/// <summary>
		///		Abre la ventana de propiedades de una carpeta
		/// </summary>
		SystemControllerEnums.ResultType OpenUpdateFolderView(FolderViewModel viewModel);
	}
}
