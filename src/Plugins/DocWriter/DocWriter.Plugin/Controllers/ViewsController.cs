using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibDocWriter.Application.Bussiness.Documents;
using Bau.Libraries.LibDocWriter.Model.Documents;
using Bau.Libraries.LibDocWriter.Application.Bussiness.Solutions;
using Bau.Libraries.LibDocWriter.Model.Solutions;
using Bau.Plugins.DocWriter.Views;
using Bau.Libraries.BauMvvm.ViewModels.Controllers;
using Bau.Libraries.Plugins.Views.Host;

namespace Bau.Plugins.DocWriter.Controllers
{
	/// <summary>
	///		Controlador de ventanas de DocWriter
	/// </summary>
	public class ViewsController : Libraries.LibDocWriter.ViewModel.Controllers.IViewsController
	{
		/// <summary>
		///		Abre la ventana que muestra el árbol de proyectos
		/// </summary>
		public void OpenTreeProjectsView()
		{
			DocWriterPlugin.MainInstance.HostPluginsController.LayoutController.ShowDockPane("DOCWRITER_TREE", LayoutEnums.DockPosition.Left,
																							 "DocWriter", new ProjectsTreeControlView());
		}

		/// <summary>
		///		Abre la ventana para crear un nuevo proyecto
		/// </summary>
		public SystemControllerEnums.ResultType OpenFormNewProject(SolutionModel solution, SolutionFolderModel folder)
		{
			return DocWriterPlugin.MainInstance.HostPluginsController.HostViewsController.ShowDialog(null, new ProjectNewView(solution, folder));
		}

		/// <summary>
		///		Abre la ventana de modificación de propiedades de un proyecto
		/// </summary>
		public SystemControllerEnums.ResultType OpenFormUpdateProject(ProjectModel project)
		{
			ProjectView frmProject = new ProjectView(project);

				// Abre el formulario
				DocWriterPlugin.MainInstance.HostPluginsController.LayoutController.ShowDocument("DOCWRITER_PROJECT_" + project.Name, project.Name, frmProject);
				// Devuelve el resultado
				return SystemControllerEnums.ResultType.Yes;
		}

		/// <summary>
		///		Abre la ventana para crear / modificar una carpeta
		/// </summary>
		public SystemControllerEnums.ResultType OpenUpdateFolderView(ProjectModel project, FileModel folderParent, FileModel folder)
		{
			return DocWriterPlugin.MainInstance.HostPluginsController.HostViewsController.ShowDialog(new FolderView(project, folderParent, folder));
		}

		/// <summary>
		///		Abre la ventana para crear / modificar una carpeta de solución
		/// </summary>
		public SystemControllerEnums.ResultType OpenUpdateFolderSolutionView(SolutionModel solution,
																		SolutionFolderModel solutionFolderParent, SolutionFolderModel solutionFolder)
		{
			return DocWriterPlugin.MainInstance.HostPluginsController.HostViewsController.ShowDialog(new SolutionFolderView(solution, solutionFolderParent, solutionFolder));
		}

		/// <summary>
		///		Abre la ventana para crear / modificar un archivo
		/// </summary>
		public SystemControllerEnums.ResultType OpenFormUpdateFile(SolutionModel solution, ProjectModel project, FileModel folderParent, FileModel file)
		{
			SystemControllerEnums.ResultType result = SystemControllerEnums.ResultType.Yes;

				// Abre la ventana de nuevo archivo
				if (file == null)
				{
					FileNewView frmNewView = new FileNewView(project, folderParent);

						// Muestra el diálogo de creación de archivo
						result = DocWriterPlugin.MainInstance.HostPluginsController.HostViewsController.ShowDialog(frmNewView);
						// Obtiene el archivo
						if (result == SystemControllerEnums.ResultType.Yes)
							file = frmNewView.ViewModel.File;
				}
				// Si se ha creado un nuevo archivo (o es una modificación, abre el formulario de modificación)
				if (result == SystemControllerEnums.ResultType.Yes)
					OpenFormDocument(solution, file);
				// Devuelve el resultado
				return result;
		}

		/// <summary>
		///		Abre el formulario de mantenimiento de referencias
		/// </summary>
		public SystemControllerEnums.ResultType OpenFormNewReference(ProjectModel project, FileModel folderParent)
		{
			return DocWriterPlugin.MainInstance.HostPluginsController.HostViewsController.ShowDialog(new ReferenceNewView(project, folderParent));
		}

		/// <summary>
		///		Abre el formulario de modificación del archivo al que hace referencia un documento
		/// </summary>
		private void OpenFormUpdateReference(SolutionModel solution, FileModel file)
		{
			ReferenceModel reference = new ReferenceBussiness().Load(file);
			bool found = false;

				// Busca el archivo al que se hace referencia
				if (!reference.FileNameReference.IsEmpty())
				{
					string fileName = new ReferenceBussiness().GetFileName(solution, reference);

						// Si existe el archivo, lo abre
						if (System.IO.File.Exists(fileName))
						{ 
							// Abre el formulario del documento al que se hace referencia
							OpenFormDocument(solution, new FileFactory().GetInstance(file.Project, fileName));
							// Indica que se ha encontrado el archivo
							found = true;
						}
				}
				// Si no se ha encontrado el archivo al que se hace referencia, se pregunta al usuaro si quiere abrir el XML
				if (!found &&
						DocWriterPlugin.MainInstance.HostPluginsController.ControllerWindow.ShowQuestion("No existe el archivo al que se hace referencia. ¿Desea abrir el código del archivo?"))
					OpenFormFile(file);
		}

		/// <summary>
		///		Abre el formulario de documentos
		/// </summary>
		private void OpenFormDocument(SolutionModel solution, FileModel file)
		{
			if (file.IsImage)
				DocWriterPlugin.MainInstance.HostPluginsController.ShowImage(file.FullFileName);
			else
				switch (file.FileType)
				{
					case FileModel.DocumentType.Unknown:
					case FileModel.DocumentType.File:
							OpenFormFile(file);
						break;
					case FileModel.DocumentType.Reference:
							OpenFormUpdateReference(solution, file);
						break;
					case FileModel.DocumentType.Folder:
							DocWriterPlugin.MainInstance.HostPluginsController.ControllerWindow.ShowMessage("No se pueden abrir carpetas");
						break;
					default:
							DocumentView view = new DocumentView(solution, file);

								DocWriterPlugin.MainInstance.HostPluginsController.LayoutController.ShowDocument("DOCWRITER_DOCUMENT_" + file.GlobalId, file.Title, view);
						break;
				}
		}

		/// <summary>
		///		Abre el formulario de edición de un archivo (sin comprobar su tipo)
		/// </summary>
		private void OpenFormFile(FileModel file)
		{
			DocWriterPlugin.MainInstance.HostPluginsController.LayoutController.ShowDocument("DOCWRITER_FILE" + file.GlobalId, file.Title, new FileView(file));
		}

		/// <summary>
		///		Solicita al usuario que busque un archivo
		/// </summary>
		public string SearchFile(string fileName, string filter)
		{
			return DocWriterPlugin.MainInstance.HostPluginsController.DialogsController.OpenDialogLoad(fileName, filter);
		}

		/// <summary>
		///		Abre el formulario para cambiar un nombre de archivo
		/// </summary>
		public SystemControllerEnums.ResultType OpenFormChangeFileName(FileModel file)
		{
			return DocWriterPlugin.MainInstance.HostPluginsController.HostViewsController.ShowDialog(new RenameFileView(file));
		}

		/// <summary>
		///		Abre el formulario de selección de archivos de un proyecto
		/// </summary>
		public SystemControllerEnums.ResultType SelectFilesProject(ProjectModel project, FileModel.DocumentType idDocumentType, FilesModelCollection filesSelected,
																  out FilesModelCollection filesOutput)
		{
			SelectFilesProjectView frmNewSelect = new SelectFilesProjectView(project, idDocumentType, filesSelected);
			SystemControllerEnums.ResultType result;

				// Inicializa los datos de salida
				filesOutput = new FilesModelCollection(project);
				// Muestra el formulario
				result = DocWriterPlugin.MainInstance.HostPluginsController.HostViewsController.ShowDialog(frmNewSelect);
				// Recoge los valores
				if (result == SystemControllerEnums.ResultType.Yes)
					filesOutput = frmNewSelect.ViewModel.FilesSelected;
				// Devuelve el resultado
				return result;
		}

		/// <summary>
		///		Abre el formulario de edición de una instrucción de código para el editor
		/// </summary>
		public SystemControllerEnums.ResultType OpenFormEditorInstruction(Libraries.LibDocWriter.Application.Bussiness.EditorInstruction.Model.EditorInstructionModel instruction)
		{
			return DocWriterPlugin.MainInstance.HostPluginsController.HostViewsController.ShowDialog(new Views.EditorInstructions.EditorInstructionView(instruction));
		}

		/// <summary>
		///		Abre el formulario con la lista de instrucciones
		/// </summary>
		public SystemControllerEnums.ResultType OpenFormInstructionsLists()
		{
			return DocWriterPlugin.MainInstance.HostPluginsController.HostViewsController.ShowDialog(new Views.EditorInstructions.ListInstructionsView());
		}


		/// <summary>
		///		Muestra un documento en el navegador
		/// </summary>
		public void ShowWebBrowser(string pageMain)
		{
			DocWriterPlugin.MainInstance.HostPluginsController.ShowWebBrowser(pageMain);
		}

		/// <summary>
		///		Graba todos los documentos abiertos
		/// </summary>
		public void SaveAllDocuments()
		{ 
			DocWriterPlugin.MainInstance.HostPluginsController.LayoutController.SaveAllDocuments();
		}
	}
}
