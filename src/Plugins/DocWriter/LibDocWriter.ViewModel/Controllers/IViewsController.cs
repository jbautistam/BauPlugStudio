using System;

using Bau.Libraries.BauMvvm.ViewModels.Controllers;
using Bau.Libraries.LibDocWriter.Model.Solutions;

namespace Bau.Libraries.LibDocWriter.ViewModel.Controllers
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
		SystemControllerEnums.ResultType OpenFormNewProject(SolutionModel solution, SolutionFolderModel folder);

		/// <summary>
		///		Abre la ventana de propiedades de un proyecto
		/// </summary>
		SystemControllerEnums.ResultType OpenFormUpdateProject(ProjectModel project);

		/// <summary>
		///		Abre la ventana para crear / modificar una carpeta
		/// </summary>
		SystemControllerEnums.ResultType OpenUpdateFolderView(ProjectModel project, FileModel folderParent, FileModel folder);

		/// <summary>
		///		Abre el formulario para abrir una carpeta de solución
		/// </summary>
		SystemControllerEnums.ResultType OpenUpdateFolderSolutionView(SolutionModel solution,
																 SolutionFolderModel solutionFolderParent,
																 SolutionFolderModel solutionFolder);

		/// <summary>
		///		Abre la ventana para crear / modificar un archivo / documento
		/// </summary>
		SystemControllerEnums.ResultType OpenFormUpdateFile(SolutionModel solution,
													   ProjectModel project, FileModel folderParent,
													   FileModel file);

		/// <summary>
		///		Abre la ventana para crear una referencia
		/// </summary>
		SystemControllerEnums.ResultType OpenFormNewReference(ProjectModel project, FileModel folderParent);

		/// <summary>
		///		Busca un archivo
		/// </summary>
		string SearchFile(string fileName, string filter);

		/// <summary>
		///		Abre el formulario para cambiar el nombre de un archivo
		/// </summary>
		SystemControllerEnums.ResultType OpenFormChangeFileName(FileModel file);

		/// <summary>
		///		Abre el formulario de edición de una instrucción de código en el editor
		/// </summary>
		SystemControllerEnums.ResultType OpenFormEditorInstruction(Application.Bussiness.EditorInstruction.Model.EditorInstructionModel instruction);

		/// <summary>
		///		Abre el formulario que muestra la lista de instrucciones
		/// </summary>
		SystemControllerEnums.ResultType OpenFormInstructionsLists();

		/// <summary>
		///		Abre el formulario de selección de archivos de un proyecto
		/// </summary>
		SystemControllerEnums.ResultType SelectFilesProject(ProjectModel project,
															FileModel.DocumentType idDocumentType, FilesModelCollection filesSelected,
															out FilesModelCollection filesOutput);

		/// <summary>
		///		Muestra un documento en el navegador
		/// </summary>
		void ShowWebBrowser(string pageMain);

		/// <summary>
		///		Graba todos los documentos abiertos
		/// </summary>
		void SaveAllDocuments();
	}
}