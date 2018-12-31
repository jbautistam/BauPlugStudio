using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.SourceEditor.Model.Solutions;

namespace Bau.Libraries.SourceEditor.ViewModel.Solutions
{
	/// <summary>
	///		ViewModel de <see cref="FileModel"/> para cambiar el nombre de un archivo
	/// </summary>
	public class RenameFileViewModel : BauMvvm.ViewModels.Forms.Dialogs.BaseDialogViewModel
	{ 
		// Variables privadas
		private FileModel _file;
		private string _newName;

		public RenameFileViewModel(FileModel file)
		{
			_file = file;
			NewName = file.Name;
		}

		/// <summary>
		///		Comprueba que los datos introducidos sean correctos
		/// </summary>
		private bool ValidateData()
		{
			bool validate = false;

				// Comprueba los datos introducidos
				if (NewName.IsEmpty())
					SourceEditorViewModel.Instance.ControllerWindow.ShowMessage("Introduzca el nuevo nombre del archivo");
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
				string newFileName = factory.RenameFile(_file, NewName);

					// Envía el mensaje de cambio de nombre
					SourceEditorViewModel.Instance.MessagesController.RenameFile(_file, newFileName, NewName);
					// Cierra el formulario
					RaiseEventClose(true);
			}
		}

		/// <summary>
		///		Nombre del archivo
		/// </summary>
		public string NewName
		{
			get { return _newName; }
			set { CheckProperty(ref _newName, value); }
		}
	}
}
