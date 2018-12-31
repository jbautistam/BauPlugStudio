using System;

using Bau.Libraries.BauMvvm.ViewModels.Controllers;
using Bau.Libraries.WebCurator.ViewModel.Controllers;
using Bau.Libraries.WebCurator.ViewModel.WebSites;

namespace Bau.Libraries.WebCurator.Plugin.Controllers
{
	/// <summary>
	///		Controlador de ventanas del lector de blogs
	/// </summary>
	public class ViewsController : IViewsController
	{
		/// <summary>
		///		Abre la ventana de mantenimiento de un proyecto destino
		/// </summary>
		public SystemControllerEnums.ResultType OpenUpdateProjectTarget(ProjectTargetViewModel viewModel)
		{
			return WebCuratorPlugin.MainInstance.HostPluginsController.HostViewsController.ShowDialog(new Views.WebSites.ProjectTargetView(viewModel));
		}

		/// <summary>
		///		Abre el formulario de modificación de un archivo
		/// </summary>
		public void OpenFormUpdateFile(ProjectViewModel viewModel)
		{
			WebCuratorPlugin.MainInstance.HostPluginsController.LayoutController.ShowDocument("WEBCURATOR_PROJECT_" + viewModel.FileName,
																							  viewModel.Name, new Views.WebSites.ProjectView(viewModel));
		}

		/// <summary>
		///		Icono del archivo de proyecto
		/// </summary>
		public string IconFileProject
		{
			get { return "pack://application:,,,/WebCurator.Plugin;component/Themes/Images/Project.png"; }
		}

		/// <summary>
		///		Icono del archivo del sitio Web
		/// </summary>
		public string IconFileWebSite
		{
			get { return "pack://application:,,,/WebCurator.Plugin;component/Themes/Images/WebSite.png"; }
		}

		/// <summary>
		///		Icono del archivo de frases
		/// </summary>
		public string IconFileSentence
		{
			get { return "pack://application:,,,/WebCurator.Plugin;component/Themes/Images/Sentence.png"; }
		}

		/// <summary>
		///		Icono del menú de compilación
		/// </summary>
		public string IconMenuGenerate
		{
			get { return "pack://application:,,,/WebCurator.Plugin;component/Themes/Images/Process.png"; }
		}
	}
}
