using System;

using Bau.Libraries.BauMvvm.ViewModels.Controllers;
using Bau.Libraries.WebCurator.ViewModel.WebSites;

namespace Bau.Libraries.WebCurator.ViewModel.Controllers
{
	/// <summary>
	///		Interface con los controladores de las vistas
	/// </summary>
	public interface IViewsController
	{
		/// <summary>
		///		Abre la ventana de mantenimiento de un proyecto destino
		/// </summary>
		SystemControllerEnums.ResultType OpenUpdateProjectTarget(ProjectTargetViewModel viewModel);

		/// <summary>
		///		Abre el formulario de modificación de archivos
		/// </summary>
		void OpenFormUpdateFile(ProjectViewModel viewModel);

		/// <summary>
		///		Icono del archivo de proyectos
		/// </summary>
		string IconFileProject { get; }

		/// <summary>
		///		Icono del archivo de sitio web
		/// </summary>
		string IconFileWebSite { get; }

		/// <summary>
		///		Icono del archivo de sentencias
		/// </summary>
		string IconFileSentence { get; }

		/// <summary>
		///		Icono del menú de generación
		/// </summary>
		string IconMenuGenerate { get; }
	}
}
