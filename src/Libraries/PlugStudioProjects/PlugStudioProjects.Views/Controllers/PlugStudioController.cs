using System;

using Bau.Libraries.BauMvvm.ViewModels.Controllers;
using Bau.Libraries.Plugins.Views.Host;
using Bau.Libraries.PlugStudioProjects.Controllers;

namespace Bau.Libraries.PlugStudioProjects.Views.Controllers
{
	/// <summary>
	///		Controlador para el plugin de proyecto
	/// </summary>
	public class PlugStudioController : IPlugStudioController
	{
		public PlugStudioController(IHostPluginsController hostPluginsController)
		{
			HostPluginsController = hostPluginsController;
		}

		/// <summary>
		///		Abre una ventana de edición
		/// </summary>
		public void OpenEditor(string fileName, string template, string fileNameHelp = null)
		{
			HostPluginsController.ShowCodeEditor(fileName, template, LayoutEnums.Editor.Xml, fileNameHelp);
		}

		/// <summary>
		///		Abre una ventana de imagen
		/// </summary>
		public void OpenImage(string fileName)
		{
			HostPluginsController.ShowImage(fileName);
		}

		/// <summary>
		///		Abre el navegador web
		/// </summary>
		public void OpenWebBrowser(string url)
		{
			HostPluginsController.ShowWebBrowser(url);
		}

		/// <summary>
		///		Abre la ventana de creación de un nuevo archivo
		/// </summary>
		public SystemControllerEnums.ResultType SelectNewFile(ViewModels.Definitions.SelectNewFileViewModel viewModel)
		{
			return HostPluginsController.HostViewsController.ShowDialog(new FileNewView(viewModel));
		}

		/// <summary>
		///		Controlador de plugins
		/// </summary>
		private IHostPluginsController HostPluginsController { get; }

		/// <summary>
		///		Controlador de ventanas
		/// </summary>
		public IHostSystemController ControllerWindow 
		{ 
			get { return HostPluginsController.ControllerWindow; }
		}
	}
}
