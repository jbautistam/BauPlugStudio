using System;
using System.Windows;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.SourceEditor.Model.Solutions;
using Bau.Libraries.SourceEditor.Plugin.Views;
using Bau.Libraries.SourceEditor.ViewModel.Solutions;
using Bau.Libraries.BauMvvm.ViewModels.Controllers;
using Bau.Libraries.Plugins.Views.Host;

namespace Bau.Libraries.SourceEditor.Plugin.Controllers
{
	/// <summary>
	///		Controlador de ventanas de SourceEditor
	/// </summary>
	public class ViewsController : ViewModel.Controllers.IViewsController
	{
		/// <summary>
		///		Abre la ventana que muestra el árbol de proyectos
		/// </summary>
		public void OpenTreeProjectsView()
		{
			SourceEditorPlugin.MainInstance.HostPluginsController.LayoutController.ShowDockPane("SOURCEEDITOR_TREE", LayoutEnums.DockPosition.Left,
																								"Soluciones", new ProjectsTreeControlView());
		}

		/// <summary>
		///		Abre la ventana para crear un nuevo proyecto
		/// </summary>
		public SystemControllerEnums.ResultType OpenFormNewProject(ViewModel.Solutions.NewItems.ProjectNewViewModel projectNewViewModel)
		{
			return SourceEditorPlugin.MainInstance.HostPluginsController.HostViewsController.ShowDialog(new Views.NewItems.ProjectNewView(projectNewViewModel));
		}

		/// <summary>
		///		Abre la ventana para crear / modificar una carpeta de solución
		/// </summary>
		public SystemControllerEnums.ResultType OpenUpdateFolderSolutionView(SolutionFolderViewModel viewModel)
		{
			return SourceEditorPlugin.MainInstance.HostPluginsController.HostViewsController.ShowDialog(new SolutionFolderView(viewModel));
		}

		/// <summary>
		///		Abre la ventana para crear un archivo
		/// </summary>
		public SystemControllerEnums.ResultType OpenFormNewFile(ViewModel.Solutions.NewItems.FileNewViewModel viewModel)
		{
			Views.NewItems.FileNewView frmNewView = new Views.NewItems.FileNewView(viewModel);

				// Muestra el diálogo de creación de archivo
				return SourceEditorPlugin.MainInstance.HostPluginsController.HostViewsController.ShowDialog(frmNewView);
		}

		/// <summary>
		///		Abre la ventana para crear / modificar un archivo
		/// </summary>
		public void OpenFormUpdateFile(Model.Definitions.ProjectDefinitionModel definition, FileModel file, bool isNew)
		{
			if (file != null)
			{
				Model.Definitions.AbstractDefinitionModel.OpenMode mode = SearchOpenMode(definition, file);

					if (LibCommonHelper.Files.HelperFiles.CheckIsImage(file.FullFileName) || mode == Model.Definitions.AbstractDefinitionModel.OpenMode.Image)
						SourceEditorPlugin.MainInstance.HostPluginsController.ShowImage(file.FullFileName);
					else if (mode == Model.Definitions.AbstractDefinitionModel.OpenMode.SourceCode)
						OpenFormFile(definition, file, isNew);
					else if (!SourceEditorPlugin.MainInstance.ViewModelManager.MessagesController.OpenFile(definition, file, false))
					{
						if (!file.IsFolder && System.IO.File.Exists(file.FullFileName))
							OpenFormFile(definition, file, isNew);
					}
			}
		}

		/// <summary>
		///		Obtiene el modo de apertura de un archivo
		/// </summary>
		private Model.Definitions.AbstractDefinitionModel.OpenMode SearchOpenMode(Model.Definitions.ProjectDefinitionModel definition, FileModel file)
		{
			Model.Definitions.AbstractDefinitionModel fileDefinition = definition.FilesDefinition.SearchByExtension(file.Extension);

				if (fileDefinition == null)
					return Model.Definitions.AbstractDefinitionModel.OpenMode.Owner;
				else
					return fileDefinition.IDOpenMode;
		}

		/// <summary>
		///		Abre el formulario de edición de un archivo (sin comprobar su tipo)
		/// </summary>
		private void OpenFormFile(Model.Definitions.ProjectDefinitionModel definition, FileModel file, bool isNew)
		{
			Model.Definitions.FileDefinitionModel fileDefinition = definition.FilesDefinition.SearchByExtension(file.Extension) as Model.Definitions.FileDefinitionModel;

				// Si es un archivo nuevo con una plantilla, se copia antes de abrir
				if (isNew && fileDefinition != null)
				{
					string template = fileDefinition.Template;

						if (!template.IsEmpty() && System.IO.File.Exists(template))
							LibCommonHelper.Files.HelperFiles.CopyFile(template, file.FullFileName);
				}
				// Abre la ventana de código
				SourceEditorPlugin.MainInstance.HostPluginsController.LayoutController.ShowDocument("SOURCEEDITOR_FILE" + file.FullFileName,
																									file.Name, new FileView(file, fileDefinition));
		}

		/// <summary>
		///		Solicita al usuario que busque un archivo
		/// </summary>
		public string SearchFile(string fileName, string filter)
		{
			return SourceEditorPlugin.MainInstance.HostPluginsController.DialogsController.OpenDialogLoad(fileName, filter);
		}

		/// <summary>
		///		Abre el formulario para cambiar un nombre de archivo
		/// </summary>
		public SystemControllerEnums.ResultType OpenFormChangeFileName(FileModel file)
		{
			RenameFileView frmNewRename = new RenameFileView(file);

				return SourceEditorPlugin.MainInstance.HostPluginsController.HostViewsController.ShowDialog(frmNewRename);
		}
	}
}
