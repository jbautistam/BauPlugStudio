using System;
using System.Composition;

using Bau.Libraries.Plugins.Views.Plugins;

namespace Bau.Plugins.FtpManager
{
	/// <summary>
	///		Plugin para el IDE de manejo de conexiones Ftp
	/// </summary>
	[Export(typeof(IPluginController))]
	[Shared]
	[ExportMetadata("Name", "FtpManager")]
	[ExportMetadata("Description", "Entorno para tratamiento de conexiones a servidores FTP")]
	public class FtpManagerPlugin : BasePluginController
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
			Name = "FtpManager";
			ViewModelManager = new Libraries.FtpManager.ViewModel.FtpManagerViewModel(Name, HostPluginsController.HostViewModelController, 
																					  HostPluginsController.ControllerWindow,
																					  hostPluginsController.DialogsController, ViewsController);
			ViewModelManager.InitModule();
		}

		/// <summary>
		///		Muestra los paneles del plugin
		/// </summary>
		public override void ShowPanes()
		{ 
			// ... no tiene paneles a mostrar. Utiliza los de SourceEditor
		}

		/// <summary>
		///		Obtiene el control de configuración
		/// </summary>
		public override System.Windows.Controls.UserControl GetConfigurationControl()
		{
			return null;
		}

		/// <summary>
		///		Manager para <see cref="FtpManagerViewModel"/>
		/// </summary>
		internal Libraries.FtpManager.ViewModel.FtpManagerViewModel ViewModelManager { get; private set; }

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
		internal static FtpManagerPlugin MainInstance { get; private set; }
	}
}
