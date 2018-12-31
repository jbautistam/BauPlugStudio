using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibCommonHelper.Files;
using Bau.Libraries.MVVM.ViewModels.ListItems;
using Bau.Libraries.SourceEditor.Model.Solutions;

namespace Bau.Libraries.SourceEditor.ViewModel.Solutions.NewItems
{
	/// <summary>
	///		ViewModel de <see cref="ProjectModel"/> para creación de un proyecto
	/// </summary>
	public class ProjectNewViewModel : BauMvvm.ViewModels.Forms.Dialogs.BaseDialogViewModel
	{ 
		// Variables privadas
		private SolutionModel _solution;
		private SolutionFolderModel _folder;
		private string _name, _pathTarget;
		private ListViewModel<ListItems.ProjectDefinitionListItemViewModel> _definitions;

		public ProjectNewViewModel(SolutionModel solution, SolutionFolderModel folder)
		{
			_solution = solution;
			_folder = folder;
			InitProperties();
		}

		/// <summary>
		///		Inicializa las propiedades
		/// </summary>
		private void InitProperties()
		{ 
			// Inicializa la colección
			ProjectsDefinition = new ListViewModel<ListItems.ProjectDefinitionListItemViewModel>();
			// Carga la colección de proyectos
			foreach (Model.Definitions.ProjectDefinitionModel project in SourceEditorViewModel.Instance.PluginsController.ProjectDefinitions)
				ProjectsDefinition.Add(new ListItems.ProjectDefinitionListItemViewModel(project));
			// Asigna la ruta inicial
			if (_solution == null)
				PathTarget = SourceEditorViewModel.Instance.PathData;
			else
				PathTarget = System.IO.Path.GetDirectoryName(_solution.FullFileName);
		}

		/// <summary>
		///		Comprueba que los datos introducidos sean correctos
		/// </summary>
		private bool ValidateData()
		{
			bool validate = false;

				// Comprueba los datos introducidos
				if (Name.IsEmpty())
					SourceEditorViewModel.Instance.ControllerWindow.ShowMessage("Introduzca el nombre del archivo");
				else if (PathTarget.IsEmpty())
					SourceEditorViewModel.Instance.ControllerWindow.ShowMessage("Introduzca el directorio");
				else if (ProjectsDefinition.SelectedItem == null)
					SourceEditorViewModel.Instance.ControllerWindow.ShowMessage("Seleccione un tipo de proyecto");
				else
					validate = true;
				// Devuelve el valor que indica si los datos son correctos
				return validate;
		}

		/// <summary>
		///		Graba los datos
		/// </summary>
		protected override void Save()
		{
			if (ValidateData())
			{
				string targetPath = System.IO.Path.Combine(PathTarget, Name);
				string fileName = HelperFiles.CombineFileName(targetPath, HelperFiles.Normalize(Name),
																 ProjectsDefinition.SelectedItem.Project.Extension);
				ProjectModel project;

					// Crea el directorio
					HelperFiles.MakePath(targetPath);
					// Crea el proyecto (después de crear el directorio)
					project = new ProjectModel(_solution, ProjectsDefinition.SelectedItem.Project, fileName);
					// Crea el archivo de proyecto (vacío)
					HelperFiles.SaveTextFile(fileName, "");
					// Añade el proyecto a la solución
					if (_folder == null)
						_solution.Projects.Add(project);
					else
						_folder.Projects.Add(project);
					// Crea el proyecto
					SourceEditorViewModel.Instance.MessagesController.OpenFile(project.Definition, project, true);
					// Cierra el formulario
					RaiseEventClose(true);
			}
		}

		/// <summary>
		///		Nombre del archivo
		/// </summary>
		public string Name
		{
			get { return _name; }
			set { CheckProperty(ref _name, value); }
		}

		/// <summary>
		///		Directorio donde se va a crear el proyecto
		/// </summary>
		public string PathTarget
		{
			get { return _pathTarget; }
			set { CheckProperty(ref _pathTarget, value); }
		}

		/// <summary>
		///		Definiciones de proyectos
		/// </summary>
		public ListViewModel<ListItems.ProjectDefinitionListItemViewModel> ProjectsDefinition
		{
			get { return _definitions; }
			set { CheckObject(ref _definitions, value); }
		}
	}
}
