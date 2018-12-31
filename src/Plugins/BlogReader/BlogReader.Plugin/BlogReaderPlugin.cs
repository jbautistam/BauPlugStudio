using System;
using System.Composition;

using Bau.Libraries.Plugins.Views.Plugins;
using Bau.Libraries.LibBlogReader.ViewModel;

namespace Bau.Plugins.BlogReader
{
	/// <summary>
	///		Plugin para el lector de blogs
	/// </summary>
	[Export(typeof(IPluginController))]
	[Shared]
	[ExportMetadata("Name", "BlogReader")]
	[ExportMetadata("Description", "Lector de blogs")]
	public class BlogReaderPlugin : BasePluginController
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
			Name = "BlogReader";
			ViewModelManager = new BlogReaderViewModel("BlogReader", HostPluginsController.HostViewModelController, 
													   HostPluginsController.ControllerWindow, 
													   hostPluginsController.DialogsController, 
													   ViewsController, GetRouteIcons());
			ViewModelManager.InitModule();
		}

		/// <summary>
		///		Obtiene las rutas de los iconos
		/// </summary>
		private System.Collections.Generic.Dictionary<BlogReaderViewModel.IconIndex, string> GetRouteIcons()
		{
			System.Collections.Generic.Dictionary<BlogReaderViewModel.IconIndex, string> routeIcons = new System.Collections.Generic.Dictionary<BlogReaderViewModel.IconIndex, string>();

				// Añade las rutas
				routeIcons.Add(BlogReaderViewModel.IconIndex.NewFolder, Controls.Themes.ThemesConstants.IconFolderRoute);
				routeIcons.Add(BlogReaderViewModel.IconIndex.NewBlog, Controls.Themes.ThemesConstants.IconConnectionRoute);
				// Devuelve el diccionario con las rutas
				return routeIcons;
		}

		/// <summary>
		///		Muestra los paneles del plugin
		/// </summary>
		public override void ShowPanes()
		{
			ViewsController.OpenTreeBlogsView();
		}

		/// <summary>
		///		Obtiene el control de configuración
		/// </summary>
		public override System.Windows.Controls.UserControl GetConfigurationControl()
		{
			return new Views.Configuration.ctlConfigurationBlogReader();
		}

		/// <summary>
		///		Manager para <see cref="BlogReaderViewModel"/>
		/// </summary>
		internal BlogReaderViewModel ViewModelManager { get; private set; }

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
		///		Instancia principal del plugin
		/// </summary>
		internal static BlogReaderPlugin MainInstance { get; private set; }
	}
}
