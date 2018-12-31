using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.MVVM.ViewModels.ComboItems;
using Bau.Libraries.LibDocWriter.Model.Solutions;

namespace Bau.Libraries.LibDocWriter.ViewModel.Solutions
{
	/// <summary>
	///		ViewModel de <see cref="FileModel"/> para creación de un archivo
	/// </summary>
	public class FileNewViewModel : BauMvvm.ViewModels.Forms.Dialogs.BaseDialogViewModel
	{ 
		// Variables privadas
		private ProjectModel _project;
		private FileModel _folderParent;
		private string _name;

		public FileNewViewModel(ProjectModel project, FileModel folderParent)
		{
			_project = project;
			_folderParent = folderParent;
			InitProperties();
		}

		/// <summary>
		///		Inicializa las propiedades
		/// </summary>
		private void InitProperties()
		{
			ComboTypes = new Helper.ComboViewHelper(this).GetComboDocumentTypes("ComboTypes");
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
				else if (ComboTypes.SelectedID == null)
					DocWriterViewModel.Instance.ControllerWindow.ShowMessage("Seleccione el tipo del archivo");
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
				Application.Bussiness.Solutions.FileFactory factory = new Application.Bussiness.Solutions.FileFactory();

					// Crea el archivo
					File = factory.CreateFile(_project, _folderParent, Name, (FileModel.DocumentType) (ComboTypes.SelectedID ?? 0));
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
		///		Combo de tipos de archivo
		/// </summary>
		public ComboViewModel ComboTypes { get; private set; }

		/// <summary>
		///		Archivo creado
		/// </summary>
		public FileModel File { get; private set; }
	}
}
