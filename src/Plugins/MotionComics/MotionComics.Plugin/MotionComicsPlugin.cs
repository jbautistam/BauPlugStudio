using System;
using System.Composition;

using Bau.Libraries.Plugins.Views.Plugins;

namespace Bau.Plugins.MotionComics
{
	/// <summary>
	///		Plugin para edición de cómics en movimiento
	/// </summary>
	[Export(typeof(IPluginController))]
	[Shared]
	[ExportMetadata("Name", "MotionComics")]
	[ExportMetadata("Description", "Edición de cómics en movimiento")]
	public class MotionComicsPlugin : BasePluginController
	{ 
		// Variables privadas
		private static Controllers.ViewsController _viewsController = null;
		private static string _pathPlugin = null;

		/// <summary>
		///		Inicializa las librerías
		/// </summary>
		public override void InitLibraries(Libraries.Plugins.Views.Host.IHostPluginsController hostPluginsController)
		{
			MainInstance = this;
			HostPluginsController = hostPluginsController;
			Name = "MotionComics";
			ViewModelManager = new Libraries.MotionComics.ViewModel.MotionComicsViewModel(Name, HostPluginsController.HostViewModelController, 
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
			return new Views.Configuration.ctlConfiguration();
		}

		/// <summary>
		///		Manager para <see cref="MotionComicsViewModel"/>
		/// </summary>
		internal Libraries.MotionComics.ViewModel.MotionComicsViewModel ViewModelManager { get; private set; }

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
		internal static MotionComicsPlugin MainInstance { get; private set; }

		/// <summary>
		///		Directorio base del plugin
		/// </summary>
		internal string PathPlugin
		{
			get
			{ 
				// Obtiene el path del plugin si no estaba en memoria (se trata como una URL porque es file://...)
				if (string.IsNullOrEmpty(_pathPlugin))
				{
					// Obtiene el path del ensamblado
					_pathPlugin = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
					// Cambia el formato
					_pathPlugin = System.IO.Path.GetDirectoryName(Uri.UnescapeDataString(new UriBuilder(_pathPlugin).Path));
				}
				// Devuelve el directorio
				return _pathPlugin;
			}
		}

		/// <summary>
		///		Directorio predeterminado de plantillas
		/// </summary>
		public string PathTemplates
		{
			get { return System.IO.Path.Combine(PathPlugin, "Data\\Templates"); }
		}
	}
}
