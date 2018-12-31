using System;
using System.Composition;

using Bau.Libraries.Plugins.Views.Plugins;
using Bau.Libraries.BauMessenger.ViewModel;

namespace Bau.Plugins.BauMessenger
{
	/// <summary>
	///		Plugin para el márketing de Twitter y redes sociales
	/// </summary>
	[Export(typeof(IPluginController))]
	[ExportMetadata("Name", "BauMessenger")]
	[ExportMetadata("Description", "Messenger sobre XMPP/Jabber")]
	public class BauMessengerPlugin : BasePluginController
	{ 
		// Variables privadas
		private static Controllers.ViewsController viewsController = null;

		/// <summary>
		///		Inicializa las librerías
		/// </summary>
		public override void InitLibraries(Libraries.Plugins.Views.Host.IHostPluginsController hostPluginsController)
		{
			MainInstance = this;
			HostPluginsController = hostPluginsController;
			Name = "BauMessenger";
			ViewModelManager = new BauMessengerViewModel("BauMessenger", HostPluginsController.HostViewModelController, 
														 HostPluginsController.ControllerWindow, hostPluginsController.DialogsController, 
														 new Controllers.ViewsController());
			ViewModelManager.InitModule();
		}

		/// <summary>
		///		Muestra los paneles del plugin
		/// </summary>
		public override void ShowPanes()
		{
			ViewModelManager.ViewsController.OpenBauMessengerView();
		}

		/// <summary>
		///		Obtiene el control de configuración
		/// </summary>
		public override System.Windows.Controls.UserControl GetConfigurationControl()
		{
			return null;
		}

		/// <summary>
		///		Manager para <see cref="BauMessengerViewModel"/>
		/// </summary>
		public BauMessengerViewModel ViewModelManager { get; private set; }

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
		internal static BauMessengerPlugin MainInstance { get; private set; }
	}
}
