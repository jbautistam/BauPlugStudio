using System;
using System.ComponentModel.Composition;

using Bau.Libraries.Plugins.Views.Plugins;
using Bau.Libraries.SourceEditor.ViewModel;

namespace Bau.Libraries.SourceEditor.Plugin
{
	/// <summary>
	///		Plugin para el gestión de editores de código fuentes
	/// </summary>
	[Export(typeof(IPluginController))]
	[ExportMetadata("Name", "SourceEditor")]
	[ExportMetadata("Description", "Editor de código fuente")]
	public class SourceEditorPlugin : BasePluginController
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
			Name = "SourceEditor";
			ViewModelManager = new SourceEditorViewModel("SourceEditor", HostPluginsController.HostViewModelController, 
														 HostPluginsController.ControllerWindow, hostPluginsController.DialogsController,
														 ViewsController, GetRouteImages());
			ViewModelManager.InitModule();
		}

		/// <summary>
		///		Obtiene las rutas de las imágenes
		/// </summary>
		private System.Collections.Generic.Dictionary<SourceEditorViewModel.IconIndex, string> GetRouteImages()
		{
			System.Collections.Generic.Dictionary<SourceEditorViewModel.IconIndex, string> dctRoutes = new System.Collections.Generic.Dictionary<SourceEditorViewModel.IconIndex, string>();

				// Añade las rutas
				dctRoutes.Add(SourceEditorViewModel.IconIndex.OpenSolution, Controls.Themes.ThemesConstants.IconOpenRoute);
				dctRoutes.Add(SourceEditorViewModel.IconIndex.NewFolder, Controls.Themes.ThemesConstants.IconFolderRoute);
				dctRoutes.Add(SourceEditorViewModel.IconIndex.NewProject, Controls.Themes.ThemesConstants.IconProjectRoute);
				dctRoutes.Add(SourceEditorViewModel.IconIndex.NewDocument, Controls.Themes.ThemesConstants.IconDocumentRoute);
				// Devuelve el diccionario de imágenes
				return dctRoutes;
		}

		/// <summary>
		///		Inicializa la solicitud de información a otros plugins
		/// </summary>
		public override void InitComunicationBetweenPlugins()
		{
			ViewModelManager.InitComunicationBetweenPlugins();
		}

		/// <summary>
		///		Muestra los paneles del plugin
		/// </summary>
		public override void ShowPanes()
		{
			ViewModelManager.ViewsController.OpenTreeProjectsView();
		}

		/// <summary>
		///		Obiene el control de configuración
		/// </summary>
		public override System.Windows.Controls.UserControl GetConfigurationControl()
		{
			return new Views.Configuration.ctlConfigurationSourceEditor();
		}

		/// <summary>
		///		Manager para <see cref="SourceEditorViewModel"/>
		/// </summary>
		internal SourceEditorViewModel ViewModelManager { get; private set; }

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
		internal static SourceEditorPlugin MainInstance { get; private set; }
	}
}
