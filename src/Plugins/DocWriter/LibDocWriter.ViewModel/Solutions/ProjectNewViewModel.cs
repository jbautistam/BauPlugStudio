using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibDocWriter.Model.Solutions;

namespace Bau.Libraries.LibDocWriter.ViewModel.Solutions
{
	/// <summary>
	///		ViewModel de <see cref="ProjectModel"/> para creación de un proyecto
	/// </summary>
	public class ProjectNewViewModel : BauMvvm.ViewModels.Forms.Dialogs.BaseDialogViewModel
	{ 
		// Variables privadas
		private SolutionModel _solution;
		private SolutionFolderModel _folder;
		private string _name, _pathTarget, _projectSource;

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
			PathTarget = _solution.Path;
		}

		/// <summary>
		///		Comprueba que los datos introducidos sean correctos
		/// </summary>
		private bool ValidateData()
		{
			bool validate = false;

				// Comprueba los datos introducidos
				if (Name.IsEmpty())
					DocWriterViewModel.Instance.ControllerWindow.ShowMessage("Introduzca el nombre del archivo");
				else if (PathTarget.IsEmpty() || !System.IO.Directory.Exists(PathTarget))
					DocWriterViewModel.Instance.ControllerWindow.ShowMessage("Introduzca el directorio");
				else if (!ProjectSource.IsEmpty() && (!ProjectSource.EndsWith(ProjectModel.FileName,
																			  StringComparison.CurrentCultureIgnoreCase) ||
													  !System.IO.File.Exists(ProjectSource)))
					DocWriterViewModel.Instance.ControllerWindow.ShowMessage("Seleccione un nombre de proyecto válido");
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
				ProjectModel newProject;
				string targetPath = System.IO.Path.Combine(PathTarget, Name);

					// Crea el proyecto
					newProject = new Application.Bussiness.Solutions.ProjectFactory().Create(_solution, _folder, targetPath);
					// Copia el proyecto origen
					if (!ProjectSource.IsEmpty() && System.IO.File.Exists(ProjectSource))
					{ 
						// Copia los archivos
						LibCommonHelper.Files.HelperFiles.CopyPath(System.IO.Path.GetDirectoryName(ProjectSource), targetPath);
						// Abre el proyecto que se ha creado
						newProject = new Application.Bussiness.Solutions.ProjectBussiness().Load(_solution, newProject.File.FullFileName);
						// Cambia el nombre del proyecto
						newProject.Name = Name;
						newProject.Title = Name;
						newProject.Description = null;
						newProject.KeyWords = null;
						newProject.URLBase = null;
					}
					// Graba el nuevo proyecto
					new Application.Bussiness.Solutions.ProjectBussiness().Save(newProject);
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
		///		Nombre del proyecto origen
		/// </summary>
		public string ProjectSource
		{
			get { return _projectSource; }
			set { CheckProperty(ref _projectSource, value); }
		}
	}
}
