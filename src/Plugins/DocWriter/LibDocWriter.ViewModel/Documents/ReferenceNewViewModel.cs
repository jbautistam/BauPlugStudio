using System;

using Bau.Libraries.LibDocWriter.Model.Solutions;

namespace Bau.Libraries.LibDocWriter.ViewModel.Documents
{
	/// <summary>
	///		ViewModel para crear referencias
	/// </summary>
	public class ReferenceNewViewModel : BauMvvm.ViewModels.Forms.Dialogs.BaseDialogViewModel
	{ 
		// Variables privadas
		private TreeDocumentsViewModel _treeFiles;
		private bool _isRecursive;

		public ReferenceNewViewModel(ProjectModel project, FileModel folderTarget)
		{ 
			// Inicializa las propiedades
			Project = project;
			FolderTarget = folderTarget;
			IsRecursive = true;
			// Inicializa el combo de tipos (al seleccionar un elemento, se cargará el combo de proyectos
			InitCombos();
			// Inicializa los manejadores de eventos (al principio)
			PropertyChanged += (sender, evntArgs) =>
									{
										if (evntArgs.PropertyName.Equals(nameof(ComboWebType)))
											LoadComboProjects(project);
										if (evntArgs.PropertyName.Equals(nameof(ComboProjects)))
											LoadTreeFiles();
									};
			// Carga por primera vez el combo de proyectos
			LoadComboProjects(project);
		}

		/// <summary>
		///		Inicializa los combos
		/// </summary>
		private void InitCombos()
		{
			ComboWebType = new Helper.ComboViewHelper(this).LoadComboWebTypes(nameof(ComboWebType));
			ComboWebType.SelectedID = (int) ProjectModel.WebDefinitionType.Template;
		}

		/// <summary>
		///		Carga el combo de proyectos
		/// </summary>
		private void LoadComboProjects(ProjectModel project)
		{
			int index = 0;

				// Crea el combo de proyectos
				ComboProjects = new MVVM.ViewModels.ComboItems.ComboViewModel(this, nameof(ComboProjects));
				// Carga los proyectos
				foreach (ProjectModel projectSolution in project.Solution.GetAllProjects())
					if (projectSolution.GlobalId != project.GlobalId &&
							projectSolution.WebType == (ProjectModel.WebDefinitionType) (ComboWebType.SelectedID ?? 0))
						ComboProjects.AddItem(index++, projectSolution.Name, projectSolution);
				// Selecciona el primer elemento del combo
				ComboProjects.SelectedID = 0;
		}

		/// <summary>
		///		Obtiene el proyecto seleccionado en el combo
		/// </summary>
		private ProjectModel GetSelectedProject()
		{
			if (ComboProjects.SelectedTag != null && ComboProjects.SelectedTag is ProjectModel)
				return ComboProjects.SelectedTag as ProjectModel;
			else
				return null;
		}

		/// <summary>
		///		Carga el árbol de archivo
		/// </summary>
		private void LoadTreeFiles()
		{
			ProjectModel project = GetSelectedProject();

				if (project != null)
				{
					if (TreeFiles != null && TreeFiles.Files != null)
						TreeFiles.Files.Clear();
					TreeFiles = new TreeDocumentsViewModel(this, FileModel.DocumentType.Unknown, project.File, null);
				}
		}

		/// <summary>
		///		Comprueba los datos de la ventana
		/// </summary>
		private bool ValidateData()
		{
			bool validate = false;

				// Comprueba los datos del formulario
				if (GetSelectedProject() == null)
					DocWriterViewModel.Instance.ControllerWindow.ShowMessage("Seleccione un proyecto");
				else if (TreeFiles.GetIsCheckedFiles().Count == 0)
					DocWriterViewModel.Instance.ControllerWindow.ShowMessage("Seleccione al menos un archivo");
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
				// Crea las referencias
				new Application.Bussiness.Documents.ReferenceBussiness().Create(Project, FolderTarget.Path, GetSelectedProject(), TreeFiles.GetIsCheckedFiles(), IsRecursive);
				// Cierra el formulario
				RaiseEventClose(true);
			}
		}

		/// <summary>
		///		Proyecto
		/// </summary>
		public ProjectModel Project { get; }

		/// <summary>
		///		Carpeta destino
		/// </summary>
		public FileModel FolderTarget { get; }

		/// <summary>
		///		Combo de tipos de sitios
		/// </summary>
		public MVVM.ViewModels.ComboItems.ComboViewModel ComboWebType { get; private set; }

		/// <summary>
		///		Combo de proyectos
		/// </summary>
		public MVVM.ViewModels.ComboItems.ComboViewModel ComboProjects { get; private set; }

		/// <summary>
		///		Arbol de archivos
		/// </summary>
		public Documents.TreeDocumentsViewModel TreeFiles
		{
			get { return _treeFiles; }
			private set { CheckObject(ref _treeFiles, value); }
		}

		/// <summary>
		///		Indica si se debe grabar de forma recursiva
		/// </summary>
		public bool IsRecursive
		{
			get { return _isRecursive; }
			set { CheckProperty(ref _isRecursive, value); }
		}
	}
}
