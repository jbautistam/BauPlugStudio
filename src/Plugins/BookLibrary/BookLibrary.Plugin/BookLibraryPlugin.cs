using System;
using System.Composition;

using Bau.Libraries.Plugins.Views.Plugins;

namespace Bau.Libraries.BookLibrary.Plugin
{
	/// <summary>
	///		Plugin para el lector de blogs
	/// </summary>
	[Export(typeof(IPluginController))]
	[Shared]
	[ExportMetadata("Name", "BookLibraryPlugin")]
	[ExportMetadata("Description", "Gestor de bibliotecas digitales")]
	public class BookLibraryPlugin : BasePluginController
	{ 
		// Variables privadas
		private static Controllers.ViewsController _viewsController = null;

		/// <summary>
		///		Inicializa las librerías
		/// </summary>
		public override void InitLibraries(Plugins.Views.Host.IHostPluginsController hostPluginsController)
		{
			MainInstance = this;
			HostPluginsController = hostPluginsController;
			Name = "BookLibrary";
			ViewModelManager = new ViewModel.BookLibraryViewModel("BookLibrary", HostPluginsController.HostViewModelController, 
																  HostPluginsController.ControllerWindow, hostPluginsController.DialogsController, 
																  ViewsController);
			ViewModelManager.InitModule();
		}

		/// <summary>
		///		Muestra los paneles del plugin
		/// </summary>
		public override void ShowPanes()
		{
			ViewsController.OpenTreeLibrariesView();
		}

		/// <summary>
		///		Obtiene el control de configuración
		/// </summary>
		public override System.Windows.Controls.UserControl GetConfigurationControl()
		{
			return new Views.Configuration.ctlConfigurationBookLibrary();
		}

		/// <summary>
		///		Manager para <see cref="BookLibraryViewModel"/>
		/// </summary>
		internal ViewModel.BookLibraryViewModel ViewModelManager { get; private set; }

		/// <summary>
		///		Controlador de vistas
		/// </summary>
		internal Controllers.ViewsController ViewsController
		{
			get
			{ 
				// Crea el controlador
				if (_viewsController == null)
					_viewsController = new Controllers.ViewsController();
				// Devuelve el controlador
				return _viewsController;
			}
		}

		/// <summary>
		///		Instancia principal del plugin
		/// </summary>
		internal static BookLibraryPlugin MainInstance { get; private set; }
	}
}
