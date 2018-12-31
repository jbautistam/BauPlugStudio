using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.BookLibrary.Model.Books;
//using Bau.Libraries.BookLibrary.Application.Services;

namespace Bau.Libraries.BookLibrary.ViewModel.Books.Content.eBook
{
	/// <summary>
	///		ViewModel para la compilación de libros
	/// </summary>
	public class BookCompileViewModel : BauMvvm.ViewModels.Forms.Dialogs.BaseDialogViewModel
	{   
		// Variables privadas
		private BookModel _book;
		private string _title, _description, _keyWords, _pathTarget, _content;

		public BookCompileViewModel(BookModel book)
		{ 
			// Inicializa los objetos
			_book = book;
			// Inicializa el formulario
			InitForm();
		}

		/// <summary>
		///		Inicializa el formulario
		/// </summary>
		private void InitForm()
		{
			Title = _book.Name;
		}

		/// <summary>
		///		Comprueba que los datos introducidos sean correctos
		/// </summary>
		private bool ValidateData()
		{
			bool validate = false;

				// Comprueba los datos introducidos
				if (Title.IsEmpty())
					BookLibraryViewModel.Instance.ControllerWindow.ShowMessage("Introduzca el título del libro");
				else if (PathTarget.IsEmpty())
					BookLibraryViewModel.Instance.ControllerWindow.ShowMessage("Introduzca el directorio donde se debe crear el libro");
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
			BookLibraryViewModel.Instance.ControllerWindow.ShowMessage("La compilación está comentada");
			//if (ValidateData())
			//{
			//	BookCompiler compiler = new BookCompiler();

			//		// Compila 
			//		if (!compiler.Compile(_book, Title, Description, KeyWords, Content, PathTarget, out string error))
			//			BookLibraryViewModel.Instance.ControllerWindow.ShowMessage(error);
			//		else
			//			BookLibraryViewModel.Instance.ControllerWindow.ShowMessage("Fin de la compilación del libro");
			//		// Cierra el formulario
			//		RaiseEventClose(true);
			//}
		}

		/// <summary>
		///		Título de la página a compilar
		/// </summary>
		public string Title
		{
			get { return _title; }
			set { CheckProperty(ref _title, value); }
		}

		/// <summary>
		///		Descripción de la página a compilar
		/// </summary>
		public string Description
		{
			get { return _description; }
			set { CheckProperty(ref _description, value); }
		}

		/// <summary>
		///		Palabras clave de la página
		/// </summary>
		public string KeyWords
		{
			get { return _keyWords; }
			set { CheckProperty(ref _keyWords, value); }
		}

		/// <summary>
		///		Directorio donde se va a compilar el libro
		/// </summary>
		public string PathTarget
		{
			get { return _pathTarget; }
			set { CheckProperty(ref _pathTarget, value); }
		}

		/// <summary>
		///		Contenido de la página
		/// </summary>
		public string Content
		{
			get { return _content; }
			set { CheckProperty(ref _content, value); }
		}
	}
}