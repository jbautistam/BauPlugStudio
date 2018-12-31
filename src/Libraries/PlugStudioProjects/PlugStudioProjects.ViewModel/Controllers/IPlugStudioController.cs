using System;
using Bau.Libraries.BauMvvm.ViewModels.Controllers;

namespace Bau.Libraries.PlugStudioProjects.Controllers
{
	/// <summary>
	///		Controlador para los ViewModel de proyectos
	/// </summary>
	public interface IPlugStudioController
	{
		/// <summary>
		///		Abre una ventana de edición
		/// </summary>
		void OpenEditor(string fileName, string template, string fileNameHelp = null);

		/// <summary>
		///		Abre una ventana de imagen
		/// </summary>
		void OpenImage(string fileName);

		/// <summary>
		///		Abre el navegador web
		/// </summary>
		void OpenWebBrowser(string url);

		/// <summary>
		///		Abre la ventana de definición de un nuevo proyecto
		/// </summary>
		SystemControllerEnums.ResultType SelectNewFile(ViewModels.Definitions.SelectNewFileViewModel viewModel);

		/// <summary>
		///		Controlador de ventanas
		/// </summary>
		IHostSystemController ControllerWindow { get; }
	}
}
