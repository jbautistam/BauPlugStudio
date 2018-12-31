using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibDocWriter.Model.Solutions;

namespace Bau.Libraries.LibDocWriter.ViewModel.Solutions
{
	/// <summary>
	///		ViewModel de <see cref="FileModel"/> para creación / modificación de una carpeta
	/// </summary>
	public class FolderViewModel : BauMvvm.ViewModels.Forms.Dialogs.BaseDialogViewModel
	{ 
		// Variables privadas
		private ProjectModel _project;
		private FileModel _folderParent, _folder;
		private string _name;

		public FolderViewModel(ProjectModel project, FileModel folderParent, FileModel folder)
		{
			_project = project;
			_folderParent = folderParent;
			_folder = folder;
			if (_folder == null)
				_folder = new FileModel(project);
			InitProperties();
		}

		/// <summary>
		///		Inicializa las propiedades
		/// </summary>
		private void InitProperties()
		{
			Name = _folder.Name;
		}

		/// <summary>
		///		Comprueba que los datos introducidos sean correctos
		/// </summary>
		private bool ValidateData(Application.Bussiness.Solutions.FileFactory factory)
		{
			bool validate = false;

				// Comprueba los datos introducidos
				if (Name.IsEmpty())
					DocWriterViewModel.Instance.ControllerWindow.ShowMessage("Introduzca el nombre de la carpeta");
				else if (_folderParent == null && _project.File.Files.ExistsByFullFileName(factory.GetPath(_project, _folderParent, Name)))
					DocWriterViewModel.Instance.ControllerWindow.ShowMessage("Ya existe un directorio con ese nombre en el proyecto");
				else if (_folderParent != null && _folderParent.Files.ExistsByFullFileName(factory.GetPath(_project, _folderParent, Name)))
					DocWriterViewModel.Instance.ControllerWindow.ShowMessage("Ya existe un directorio con ese nombre en la carpeta");
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
			Application.Bussiness.Solutions.FileFactory factory = new Application.Bussiness.Solutions.FileFactory();

				if (ValidateData(factory))
				{ 
					// Crea / modifica el directorio
					if (_folder.FullFileName.IsEmpty())
						factory.CreateFolder(_project, _folderParent, Name);
					else
						factory.UpdateFolder(_project, _folder, Name);
					// Cierra el formulario
					RaiseEventClose(true);
				}
		}

		/// <summary>
		///		Nombre de la carpeta
		/// </summary>
		public string Name
		{
			get { return _name; }
			set { CheckProperty(ref _name, value); }
		}
	}
}
