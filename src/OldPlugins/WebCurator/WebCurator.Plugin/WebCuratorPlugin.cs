using System;
using System.Composition;

using Bau.Libraries.Plugins.Views.Plugins;

namespace Bau.Libraries.WebCurator.Plugin
{
	/// <summary>
	///		Plugin para el mantenimiento de proyectos de WebCurator
	/// </summary>
	[Export(typeof(IPluginController))]
	[Shared]
	[ExportMetadata("Name", "WebCurator")]
	[ExportMetadata("Description", "Curator de Webs")]
	public class WebCuratorPlugin : BasePluginController
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
			Name = "WebCurator";
			ViewModelManager = new ViewModel.WebCuratorViewModel("WebCurator", HostPluginsController.HostViewModelController, 
																 HostPluginsController.ControllerWindow, hostPluginsController.DialogsController,
																 ViewsController);
			ViewModelManager.InitModule();
		}

		/// <summary>
		///		Muestra los paneles del plugin
		/// </summary>
		public override void ShowPanes()
		{
		}

		/// <summary>
		///		Obtiene el control de configuración
		/// </summary>
		public override System.Windows.Controls.UserControl GetConfigurationControl()
		{
			return new Views.Configuration.ctlConfigurationWebCurator();
		}

		/// <summary>
		///		Manager para <see cref="WebCuratorViewModel"/>
		/// </summary>
		internal ViewModel.WebCuratorViewModel ViewModelManager { get; private set; }

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
		internal static WebCuratorPlugin MainInstance { get; private set; }
	}
}
