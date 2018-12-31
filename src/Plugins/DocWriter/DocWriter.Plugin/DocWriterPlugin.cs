using System;
using System.Collections.Generic;
using System.Composition;

using Bau.Libraries.Plugins.Views.Plugins;
using Bau.Libraries.LibDocWriter.ViewModel;

namespace Bau.Plugins.DocWriter
{
	/// <summary>
	///		Plugin para el generador de documentación
	/// </summary>
	[Export(typeof(IPluginController))]
	[Shared]
	[ExportMetadata("Name", "DocWriter")]
	[ExportMetadata("Description", "Generador de sitios web estáticos")]
	public class DocWriterPlugin : BasePluginController
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
			Name = "DocWriter";
			ViewModelManager = new DocWriterViewModel("DocWriter", HostPluginsController.HostViewModelController, 
													  HostPluginsController.ControllerWindow, hostPluginsController.DialogsController, 
													  ViewsController, GetRouteImages());
			ViewModelManager.InitModule();
		}

		/// <summary>
		///		Obtiene las rutas de las imágenes
		/// </summary>
		private Dictionary<DocWriterViewModel.IconIndex, string> GetRouteImages()
		{
			Dictionary<DocWriterViewModel.IconIndex, string> dctRoutes = new Dictionary<DocWriterViewModel.IconIndex, string>();

				// Añade las rutas
				dctRoutes.Add(DocWriterViewModel.IconIndex.OpenSolution, Controls.Themes.ThemesConstants.IconOpenRoute);
				dctRoutes.Add(DocWriterViewModel.IconIndex.NewFolder, Controls.Themes.ThemesConstants.IconFolderRoute);
				dctRoutes.Add(DocWriterViewModel.IconIndex.NewProject, Controls.Themes.ThemesConstants.IconProjectRoute);
				dctRoutes.Add(DocWriterViewModel.IconIndex.NewDocument, Controls.Themes.ThemesConstants.IconDocumentRoute);
				dctRoutes.Add(DocWriterViewModel.IconIndex.AddReference, Controls.Themes.ThemesConstants.IconImageRoute);
				dctRoutes.Add(DocWriterViewModel.IconIndex.Compile, Controls.Themes.ThemesConstants.IconProcessRoute);
				// Devuelve el diccionario de imágenes
				return dctRoutes;
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
			return new Views.Configuration.ctlConfigurationDocWriter();
		}

		/// <summary>
		///		Manager para <see cref="DocWriterViewModel"/>
		/// </summary>
		internal DocWriterViewModel ViewModelManager { get; private set; }

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
		internal static DocWriterPlugin MainInstance { get; private set; }
	}
}
