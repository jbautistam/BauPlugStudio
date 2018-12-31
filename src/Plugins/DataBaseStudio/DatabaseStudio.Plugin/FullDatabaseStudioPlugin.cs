using System;
using System.Composition;

using Bau.Libraries.Plugins.Views.Plugins;
using Bau.Libraries.DatabaseStudio.ViewModels;

namespace Bau.Libraries.FullDatabaseStudio.Plugin
{
	/// <summary>
	///		Plugin para el IDE de manejo de bases de datos
	/// </summary>
	[Export(typeof(IPluginController))]
	[Shared]
	[ExportMetadata("Name", "FullDatabaseStudio")]
	[ExportMetadata("Description", "Entorno para tratamiento de scripts e informes de bases de datos")]
	public class FullDatabaseStudioPlugin : BasePluginController
	{
		public FullDatabaseStudioPlugin()
		{
			MainInstance = this;
			Name = "FullDataBaseStudio";
		}

		/// <summary>
		///		Inicializa las librerías
		/// </summary>
		public override void InitLibraries(Plugins.Views.Host.IHostPluginsController hostPluginsController)
		{
			HostPluginsController = hostPluginsController;
			ViewsController = new Controllers.ViewsController(hostPluginsController);
			ViewModelManager = new MainViewModel("DataBaseStudio", HostPluginsController.HostViewModelController, 
												 HostPluginsController.ControllerWindow, hostPluginsController.DialogsController, 
												 ViewsController);
			ViewModelManager.InitModule();
		}

		/// <summary>
		///		Muestra los paneles del plugin
		/// </summary>
		public override void ShowPanes()
		{
			HostPluginsController.LayoutController.ShowDockPane("DATABASE_PLUGIN_TREEEXPLORER", Plugins.Views.Host.LayoutEnums.DockPosition.Left,
																"Base de datos", 
																new PlugStudioProjects.Views.TreeExplorerView(GetProjectExplorerViewModel()));
		}

		/// <summary>
		///		Obtiene el ViewModel para el generador de proyectos
		/// </summary>
		private DatabaseStudio.ViewModels.Projects.ProjectExplorerViewModel GetProjectExplorerViewModel()
		{
			return new DatabaseStudio.ViewModels.Projects.ProjectExplorerViewModel(new PlugStudioProjects.Views.Controllers.PlugStudioController(HostPluginsController));
		}

		/// <summary>
		///		Obtiene el control de configuración
		/// </summary>
		public override System.Windows.Controls.UserControl GetConfigurationControl()
		{
			return new Views.Configuration.ctlConfiguration();
		}

		/// <summary>
		///		Manager para los viewModels
		/// </summary>
		internal MainViewModel ViewModelManager { get; private set; }

		/// <summary>
		///		Controlador de vistas
		/// </summary>
		internal Controllers.ViewsController ViewsController { get; private set; }

		/// <summary>
		///		Instancia principal de la aplicación
		/// </summary>
		internal static FullDatabaseStudioPlugin MainInstance { get; private set; }
	}
}
