using System;

using Bau.Libraries.BauMvvm.ViewModels.Controllers;
using Bau.Libraries.SourceEditor.Model.Solutions;
using Bau.Libraries.SourceEditor.ViewModel.Solutions;
using Bau.Libraries.SourceEditor.ViewModel.Solutions.NewItems;

namespace Bau.Libraries.SourceEditor.ViewModel.Controllers
{
	/// <summary>
	///		Interface con los controladores de las vistas
	/// </summary>
	public interface IViewsController
	{
		/// <summary>
		///		Abre la ventana que muestra el árbol de proyectos
		/// </summary>
		void OpenTreeProjectsView();

		/// <summary>
		///		Abre la ventana para crear un nuevo proyecto
		/// </summary>
		SystemControllerEnums.ResultType OpenFormNewProject(ProjectNewViewModel projectNewViewModel);

		/// <summary>
		///		Abre el formulario para abrir una carpeta de solución
		/// </summary>
		SystemControllerEnums.ResultType OpenUpdateFolderSolutionView(SolutionFolderViewModel viewModel);

		/// <summary>
		///		Abre la ventana para crear un archivo
		/// </summary>
		SystemControllerEnums.ResultType OpenFormNewFile(FileNewViewModel viewModel);

		/// <summary>
		///		Abre la ventana para modificar un archivo / documento
		/// </summary>
		void OpenFormUpdateFile(Model.Definitions.ProjectDefinitionModel definition, FileModel file, bool isNew);

		/// <summary>
		///		Busca un archivo
		/// </summary>
		string SearchFile(string fileName, string filter);

		/// <summary>
		///		Abre el formulario para cambiar el nombre de un archivo
		/// </summary>
		SystemControllerEnums.ResultType OpenFormChangeFileName(FileModel file);
	}
}