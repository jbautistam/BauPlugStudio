using System;
using System.Composition;

using Bau.Libraries.Plugins.Views.Plugins;
using Bau.Libraries.DevConferences.Admon.ViewModel;
using Bau.Libraries.Plugins.Views.Host;

namespace Bau.Plugins.DevConferences.Admon
{
	/// <summary>
	///		Plugin para la administración de canales de DevConferences
	/// </summary>
	[Export(typeof(IPluginController))]
	[Shared]
	[ExportMetadata("Name", "DevConferences")]
	[ExportMetadata("Description", "Administración de conferencias para DevConference")]
	public class DevConferencesPlugin : BasePluginController
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
			Name = "DevConferences";
			ViewModelManager = new DevConferencesViewModel(Name, HostPluginsController.HostViewModelController, 
														   HostPluginsController.ControllerWindow, hostPluginsController.DialogsController, 
														   new Controllers.ViewsController());
			ViewModelManager.InitModule();
		}

		/// <summary>
		///		Muestra los paneles del plugin
		/// </summary>
		public override void ShowPanes()
		{
			ViewModelManager.ViewsController.OpenDevConferenceView();
		}

		/// <summary>
		///		Obtiene el control de configuración
		/// </summary>
		public override System.Windows.Controls.UserControl GetConfigurationControl()
		{
			return new Views.Configuration.ctlConfigurationDevConferences();
		}

		/// <summary>
		///		Manager para <see cref="DevConferencesViewModel"/>
		/// </summary>
		public DevConferencesViewModel ViewModelManager { get; private set; }

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
		///		Instancia principal de la aplicación
		/// </summary>
		internal static DevConferencesPlugin MainInstance { get; private set; }
	}
}
