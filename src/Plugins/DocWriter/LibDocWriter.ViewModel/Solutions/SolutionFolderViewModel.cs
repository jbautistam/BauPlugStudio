using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.LibDocWriter.Model.Solutions;

namespace Bau.Libraries.LibDocWriter.ViewModel.Solutions
{
	/// <summary>
	///		ViewModel de <see cref="FileModel"/> para creación / modificación de una carpeta
	/// </summary>
	public class SolutionFolderViewModel : BauMvvm.ViewModels.Forms.Dialogs.BaseDialogViewModel
	{ 
		// Variables privadas
		private SolutionModel _solution;
		private SolutionFolderModel _folderParent, _folder;
		private string _name;

		public SolutionFolderViewModel(SolutionModel solution, SolutionFolderModel folderParent, SolutionFolderModel folder)
		{
			_solution = solution;
			_folderParent = folderParent;
			_folder = folder;
			InitProperties();
		}

		/// <summary>
		///		Inicializa las propiedades
		/// </summary>
		private void InitProperties()
		{
			if (_folder != null)
				Name = _folder.Name;
		}

		/// <summary>
		///		Comprueba que los datos introducidos sean correctos
		/// </summary>
		private bool ValidateData()
		{
			bool validate = false;

				// Comprueba los datos introducidos
				if (Name.IsEmpty())
					DocWriterViewModel.Instance.ControllerWindow.ShowMessage("Introduzca el nombre de la carpeta");
				else if (_folderParent == null && _solution.Folders.ExistsByName(Name))
					DocWriterViewModel.Instance.ControllerWindow.ShowMessage("Ya existe una carpeta con ese nombre en la solución");
				else if (_folderParent != null && _folderParent.Folders.ExistsByName(Name))
					DocWriterViewModel.Instance.ControllerWindow.ShowMessage("Ya existe una carpeta con ese nombre");
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
				// Crea / modifica el directorio
				if (_folder == null)
				{ 
					// Crea la carpeta y le asigna el nombre
					_folder = new SolutionFolderModel(_solution);
					_folder.Name = Name;
					// Añade la carpeta a la solución o a la carpeta padre
					if (_folderParent == null)
						_solution.Folders.Add(_folder);
					else
						_folderParent.Folders.Add(_folder);
				}
				else
					_folder.Name = Name;
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
