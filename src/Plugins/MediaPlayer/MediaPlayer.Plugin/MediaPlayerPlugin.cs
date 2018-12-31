using System;
using System.Composition;

using Bau.Libraries.Plugins.Views.Plugins;
using Bau.Libraries.LibMediaPlayer.ViewModel;
using Bau.Libraries.BauMvvm.ViewModels.Controllers;
using Bau.Libraries.Plugins.Views.Host;

namespace Bau.Plugins.MediaPlayer
{
	/// <summary>
	///		Plugin para el lector de blogs
	/// </summary>
	[Export(typeof(IPluginController))]
	[Shared]
	[ExportMetadata("Name", "MediaPlayer")]
	[ExportMetadata("Description", "Reproductor de archivos multimedia")]
	public class MediaPlayerPlugin : BasePluginController
	{ 
		// Variables privadas
		private static Controllers.ViewsController viewsController = null;

		/// <summary>
		///		Inicializa las librerías
		/// </summary>
		public override void InitLibraries(IHostPluginsController hostPluginsController)
		{
			MainInstance = this;
			HostPluginsController = hostPluginsController;
			Name = "MediaPlayer";
			ViewModelManager = new MediaPlayerViewModel("MediaPlayer", HostPluginsController.HostViewModelController, 
														HostPluginsController.ControllerWindow, hostPluginsController.DialogsController,
														ViewsController, GetRouteIcons());
			ViewModelManager.InitModule();
		}

		/// <summary>
		///		Obtiene las rutas de los iconos
		/// </summary>
		private System.Collections.Generic.Dictionary<MediaPlayerViewModel.IconIndex, string> GetRouteIcons()
		{
			System.Collections.Generic.Dictionary<MediaPlayerViewModel.IconIndex, string> routeIcons = new System.Collections.Generic.Dictionary<MediaPlayerViewModel.IconIndex, string>();

				// Añade las rutas
				routeIcons.Add(MediaPlayerViewModel.IconIndex.NewFolder, Controls.Themes.ThemesConstants.IconFolderRoute);
				routeIcons.Add(MediaPlayerViewModel.IconIndex.NewAlbum, Controls.Themes.ThemesConstants.IconConnectionRoute);
				// Devuelve el diccionario con las rutas
				return routeIcons;
		}

		/// <summary>
		///		Muestra los paneles del plugin
		/// </summary>
		public override void ShowPanes()
		{
			MainInstance.HostPluginsController.LayoutController.ShowDockPane("MEDIA_PLAYER_REPRODUCTION_VIEW", LayoutEnums.DockPosition.Right,
																			 "Listas reproducción", new Views.Media.MediaPlayerView());
		}

		/// <summary>
		///		Obtiene el control de configuración
		/// </summary>
		public override System.Windows.Controls.UserControl GetConfigurationControl()
		{
			return new Views.Configuration.ctlConfigurationMediaPlayer();
		}

		/// <summary>
		///		Manager para <see cref="MediaPlayerViewModel"/>
		/// </summary>
		internal MediaPlayerViewModel ViewModelManager { get; private set; }

		/// <summary>
		///		Controlador de vistas
		/// </summary>
		internal Controllers.ViewsController ViewsController
		{
			get
			{ 
				// Crea el controlador
				if (viewsController == null)
					viewsController = new Controllers.ViewsController();
				// Devuelve el controlador
				return viewsController;
			}
		}

		/// <summary>
		///		Instancia principal del plugin
		/// </summary>
		internal static MediaPlayerPlugin MainInstance { get; private set; }
	}
}
