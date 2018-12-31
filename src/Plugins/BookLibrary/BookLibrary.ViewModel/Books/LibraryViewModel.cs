using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.BookLibrary.Model.Books;

namespace Bau.Libraries.BookLibrary.ViewModel.Books
{
	/// <summary>
	///		ViewModel de <see cref="LibraryModel"/>
	/// </summary>
	public class LibraryViewModel : BauMvvm.ViewModels.Forms.Dialogs.BaseDialogViewModel
	{ 
		// Variables privadas
		private LibraryModel _library, _libraryParent;
		private string _name;

		public LibraryViewModel(LibraryModel library, LibraryModel libraryParent)
		{
			_library = library;
			_libraryParent = libraryParent;
			if (_library == null)
				_library = new LibraryModel();
			InitProperties();
		}

		/// <summary>
		///		Inicializa las propiedades
		/// </summary>
		private void InitProperties()
		{
			Name = _library.PathName;
		}

		/// <summary>
		///		Comprueba que los datos introducidos sean correctos
		/// </summary>
		private bool ValidateData()
		{
			bool validate = false;

				// Comprueba los datos introducidos
				if (Name.IsEmpty())
					BookLibraryViewModel.Instance.ControllerWindow.ShowMessage("Introduzca el nombre de la biblioteca");
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
				string pathParent = BookLibraryViewModel.Instance.BooksManager.Configuration.PathLibrary;

					// Si hay un directorio padre, lo asigna
					if (_libraryParent != null)
						pathParent = _libraryParent.Path;
					// Graba el objeto
					new Application.Bussiness.LibraryBussiness().Save(pathParent, _library, Name);
					// Cierra el formulario
					RaiseEventClose(true);
			}
		}

		/// <summary>
		///		Nombre de la biblioteca
		/// </summary>
		public string Name
		{
			get { return _name; }
			set { CheckProperty(ref _name, value); }
		}
	}
}
