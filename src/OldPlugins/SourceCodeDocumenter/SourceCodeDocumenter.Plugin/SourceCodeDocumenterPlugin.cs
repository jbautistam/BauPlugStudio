using System;
using System.Composition;

using Bau.Libraries.Plugins.Views.Plugins;

namespace Bau.Plugins.SourceCodeDocumenter
{
	/// <summary>
	///		Plugin para el documentador de esquemas
	/// </summary>
	[Export(typeof(IPluginController))]
	[Shared]
	[ExportMetadata("Name", "SourceCodeDocumenter")]
	[ExportMetadata("Description", "Documentación de código fuente")]
	public class SourceCodeDocumenterPlugin : BasePluginController
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
			Name = "SourceCodeDocumenter";
			ViewModelManager = new Libraries.LibSourceCodeDocumenter.ViewModel.SourceCodeDocumenterViewModel(Name, HostPluginsController.HostViewModelController, 
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
		///		Manager para <see cref="SourceCodeDocumenterViewModel"/>
		/// </summary>
		internal Libraries.LibSourceCodeDocumenter.ViewModel.SourceCodeDocumenterViewModel ViewModelManager { get; private set; }

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
		internal static SourceCodeDocumenterPlugin MainInstance { get; private set; }

		/// <summary>
		///		Directorio donde se encuentra el plugin
		/// </summary>
		public string PathPlugin
		{
			get
			{ 
				// Obtiene el path del plugin si no estaba en memoria
				if (string.IsNullOrEmpty(_pathPlugin))
				{
					UriBuilder builder;

						// Obtiene el path del ensamblado
						_pathPlugin = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
						// Lo trata como una URL porque viene como file://c:/xxxx
						builder = new UriBuilder(_pathPlugin);
						// Cambia el formato
						_pathPlugin = System.IO.Path.GetDirectoryName(Uri.UnescapeDataString(builder.Path));
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
			get
			{
				return System.IO.Path.Combine(PathPlugin, "Data\\Templates\\General");
			}
		}
	}
}
