using System;
using System.Composition;

using Bau.Libraries.Plugins.Views.Plugins;

namespace Bau.Plugins.DataBaseStudio
{
	/// <summary>
	///		Plugin para el IDE de manejo de bases de datos
	/// </summary>
	[Export(typeof(IPluginController))]
	[Shared]
	[ExportMetadata("Name", "DataBaseStudio")]
	[ExportMetadata("Description", "Entorno para tratamiento de bases de datos")]
	public class DataBaseStudioPlugin : BasePluginController
	{ 
		// Variables privadas
		private static Controllers.ViewsController _viewsController = null;
		private string _pathPlugin = null;

		/// <summary>
		///		Inicializa las librerías
		/// </summary>
		public override void InitLibraries(Libraries.Plugins.Views.Host.IHostPluginsController hostPluginsController)
		{
			MainInstance = this;
			HostPluginsController = hostPluginsController;
			Name = "DataBaseStudio";
			ViewModelManager = new Libraries.LibDataBaseStudio.ViewModel.DataBaseStudioViewModel("DataBaseStudio", HostPluginsController.HostViewModelController, 
																								 HostPluginsController.ControllerWindow, 
																								 hostPluginsController.DialogsController,
																								 ViewsController);
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
		///		Manager para <see cref="DataBaseStudioViewModel"/>
		/// </summary>
		internal Libraries.LibDataBaseStudio.ViewModel.DataBaseStudioViewModel ViewModelManager { get; private set; }

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
		///		Directorio base del plugin
		/// </summary>
		internal string PathPlugin
		{
			get
			{ 
				// Obtiene el path del plugin si no estaba en memoria
				if (string.IsNullOrEmpty(_pathPlugin))
				{
					UriBuilder uriBuilder;

						// Obtiene el path del ensamblado
						_pathPlugin = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
						// Lo trata como una URL porque viene como file://c:/xxxx
						uriBuilder = new UriBuilder(_pathPlugin);
						// Cambia el formato
						_pathPlugin = System.IO.Path.GetDirectoryName(Uri.UnescapeDataString(uriBuilder.Path));
				}
				// Devuelve el directorio
				return _pathPlugin;
			}
		}

		/// <summary>
		///		Instancia principal de la aplicación
		/// </summary>
		internal static DataBaseStudioPlugin MainInstance { get; private set; }
	}
}
