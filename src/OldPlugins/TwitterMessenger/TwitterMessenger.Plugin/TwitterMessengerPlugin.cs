using System;
using System.ComponentModel.Composition;

using Bau.Libraries.Plugins.Views.Plugins;
using Bau.Libraries.TwitterMessenger.ViewModel;

namespace Bau.Plugins.TwitterMessenger
{
	/// <summary>
	///		Plugin para el márketing de Twitter y redes sociales
	/// </summary>
	[Export(typeof(IPluginController))]
	[ExportMetadata("Name", "TwitterMessenger")]
	[ExportMetadata("Description", "Márketing de Twitter")]
	public class TwitterMessengerPlugin : BasePluginController
	{ 
		// Variables privadas
		private static Controllers.ViewsController _viewsController = null;

		/// <summary>
		///		Inicializa las librerías
		/// </summary>
		public override void InitLibraries(Libraries.Plugins.Views.Host.IHostPluginsController hostPluginsController)
		{
			MainInstance = this;
			HostPluginsController = hostPluginsController;
			Name = "TwitterMessenger";
			ViewModelManager = new TwitterMessengerViewModel(Name, HostPluginsController.HostViewModelController, 
															 HostPluginsController.ControllerWindow, hostPluginsController.DialogsController, 
															 new Controllers.ViewsController());
			ViewModelManager.InitModule();
		}

		/// <summary>
		///		Muestra los paneles del plugin
		/// </summary>
		public override void ShowPanes()
		{
			ViewModelManager.ViewsController.OpenTwitterMessagesView();
		}

		/// <summary>
		///		Obtiene el control de configuración
		/// </summary>
		public override System.Windows.Controls.UserControl GetConfigurationControl()
		{
			return new Views.Configuration.ctlConfigurationTwitterMessenger();
		}

		/// <summary>
		///		Manager para <see cref="DocWriterViewModel"/>
		/// </summary>
		public TwitterMessengerViewModel ViewModelManager { get; private set; }

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
		///		Instancia principal de la aplicación
		/// </summary>
		internal static TwitterMessengerPlugin MainInstance { get; private set; }
	}
}
