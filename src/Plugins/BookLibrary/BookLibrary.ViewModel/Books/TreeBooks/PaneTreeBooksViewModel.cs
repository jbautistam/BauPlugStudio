using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.BauMvvm.ViewModels;
using Bau.Libraries.BookLibrary.Application.Bussiness;
using Bau.Libraries.BookLibrary.Model.Books;
using Bau.Libraries.BauMvvm.ViewModels.Controllers;

namespace Bau.Libraries.BookLibrary.ViewModel.Books.TreeBooks
{
	/// <summary>
	///		ViewModel para el árbol de libros
	/// </summary>
	public class PaneTreeBooksViewModel : BauMvvm.ViewModels.Forms.BasePaneViewModel
	{   
		// Variables privadas
		private TreeBooksViewModel _treeBooks;

		public PaneTreeBooksViewModel()
		{ 
			// Inicializa las propiedades
			TreeBooks = new TreeBooksViewModel();
			TreeBooks.LoadNodes();
			// Inicializa los comandos
			InitCommands();
			//// Inicializa los cambios de propiedades
			//PropertyChanged += (sender, args) =>
			//							{
			//								if (args.PropertyName == nameof(TreeBooksViewModel.SelectedNode))
			//								{
			//									NewLibraryCommand.OnCanExecuteChanged();
			//									NewBookCommand.OnCanExecuteChanged();
			//									DeleteCommand.OnCanExecuteChanged();
			//									OpenBookCommand.OnCanExecuteChanged();
			//									CompileCommand.OnCanExecuteChanged();
			//								}
			//							};
		}

		/// <summary>
		///		Inicializa los comandos
		/// </summary>
		private void InitCommands()
		{
			NewBookCommand = new BaseCommand("Nuevo libro", parameter => ExecuteAction(nameof(NewBookCommand), null),
											 parameter => CanExecuteAction(nameof(NewBookCommand), null))
									.AddListener(this, nameof(TreeBooksViewModel.SelectedNode));
			NewLibraryCommand = new BaseCommand("Nueva biblioteca", 
												parameter => ExecuteAction(nameof(PropertiesCommand), null),
												parameter => CanExecuteAction(nameof(PropertiesCommand), null))
									.AddListener(this, nameof(TreeBooksViewModel.SelectedNode));
			OpenBookCommand = new BaseCommand("Abrir libro", parameter => ExecuteAction(nameof(OpenBookCommand), null),
											  parameter => CanExecuteAction(nameof(OpenBookCommand), null))
									.AddListener(this, nameof(TreeBooksViewModel.SelectedNode));
			CompileCommand = new BaseCommand("Compilar libro", canExecute => ExecuteAction(nameof(CompileCommand), null),
											 parameter => CanExecuteAction(nameof(CompileCommand), null))
									.AddListener(this, nameof(TreeBooksViewModel.SelectedNode));
			DeleteCommand.AddListener(this, nameof(TreeBooksViewModel.SelectedNode));
		}

		/// <summary>
		///		Comprueba si se puede ejecutar una acción
		/// </summary>
		protected override void ExecuteAction(string action, object parameter)
		{
			switch (action)
			{
				case nameof(PropertiesCommand):
					if (!TreeBooks.IsSelectedBook)
						OpenFormUpdateLibrary(TreeBooks.GetSelectedLibrary(), TreeBooks.GetSelectedLibraryParent());
					break;
				case nameof(NewBookCommand):
						OpenFormUpdateBook(TreeBooks.GetSelectedBook(), TreeBooks.GetSelectedLibraryParent());
					break;
				case nameof(OpenBookCommand):
						OpenBook(TreeBooks.GetSelectedBook());
					break;
				case nameof(DeleteCommand):
						if (TreeBooks.SelectedNode != null)
						{
							if (TreeBooks.IsSelectedLibrary)
								DeleteLibrary(TreeBooks.GetSelectedLibrary());
							else if (TreeBooks.IsSelectedBook)
								DeleteBook(TreeBooks.GetSelectedBook());
						}
					break;
				case nameof(CompileCommand):
						OpenFormCompileBook(TreeBooks.GetSelectedBook());
					break;
				case nameof(RefreshCommand):
						Refresh();
					break;
			}
		}

		/// <summary>
		///		Comprueba si se puede ejecutar una acción
		/// </summary>
		protected override bool CanExecuteAction(string action, object parameter)
		{
			switch (action)
			{
				case nameof(PropertiesCommand):
				case nameof(DeleteCommand):
					return TreeBooks.SelectedNode != null;
				case nameof(OpenBookCommand):
				case nameof(CompileCommand):
					return TreeBooks.IsSelectedBook;
				case nameof(NewBookCommand):
				case nameof(RefreshCommand):
					return true;
				default:
					return false;
			}
		}

		/// <summary>
		///		Actualiza el árbol
		/// </summary>
		private void Refresh()
		{
			TreeBooks.LoadNodes();
		}

		/// <summary>
		///		Abre el formulario de modificación / creación de una biblioteca
		/// </summary>
		private void OpenFormUpdateLibrary(LibraryModel library, LibraryModel libraryParent)
		{
			if (BookLibraryViewModel.Instance.ViewsController.OpenUpdateLibrary(new LibraryViewModel(library, libraryParent)) == SystemControllerEnums.ResultType.Yes)
				Refresh();
		}

		/// <summary>
		///		Abre el formulario de modificación / creación de un libro
		/// </summary>
		private void OpenFormUpdateBook(BookModel book, LibraryModel libraryParent)
		{
			string filter = "eBooks|*.epub|Cómic|*.cbr;*.cbz;*.cbt|Archivos PDF|*.pdf|Todos los archivos|*.*";
			string[] files = BookLibraryViewModel.Instance.DialogsController.OpenDialogLoadFilesMultiple(null, filter);

				// Si se ha seleccionado algún archivo
				if (files != null && files.Length > 0)
				{
					LibraryModel library = GetSelectedLibrarySearch();

						// Copia los libros
						CreateEBookNodes(library, files,
										 BookLibraryViewModel.Instance.ControllerWindow.ShowQuestion("¿Desea eliminar los archivos originales?"));
				}
		}

		/// <summary>
		///		Abre el formulario de compilación de un libro
		/// </summary>
		private void OpenFormCompileBook(BookModel book)
		{
			if (book != null)
			{
				if (book.FileName.IsEmpty() || !System.IO.File.Exists(book.FileName))
					BookLibraryViewModel.Instance.ControllerWindow.ShowMessage($"No se encuentra el archivo {book.FileName}");
				else
					BookLibraryViewModel.Instance.ViewsController.OpenFormCompile(new Content.eBook.BookCompileViewModel(book));
			}
		}

		/// <summary>
		///		Abre un libro
		/// </summary>
		private void OpenBook(BookModel book)
		{
			if (book != null)
				switch (BookLibraryViewModel.Instance.BooksManager.Configuration.BookFormats.GetBookType(book.FileName))
				{
					case BookModel.BookType.Book:
							BookLibraryViewModel.Instance.ViewsController.OpenBook(new Content.BookContentViewModel(book));
						break;
					case BookModel.BookType.Comic:
							BookLibraryViewModel.Instance.ViewsController.OpenBook(new Content.Comic.ComicContentViewModel(book));
						break;
					default:
							BookLibraryViewModel.Instance.ControllerWindow.ShowMessage("No se reconoce el formato del archivo");
						break;
				}
		}

		/// <summary>
		///		Borra una biblioteca
		/// </summary>
		private void DeleteLibrary(LibraryModel library)
		{
			if (library != null &&
				BookLibraryViewModel.Instance.ControllerWindow.ShowQuestion($"¿Realmente desea eliminar la biblioteca '{library.PathName}'?"))
			{ 
				// Borra la carpeta
				new LibraryBussiness().Delete(library);
				// Actualiza
				Refresh();
			}
		}

		/// <summary>
		///		Borra un libro
		/// </summary>
		private void DeleteBook(BookModel book)
		{
			if (book != null &&
				BookLibraryViewModel.Instance.ControllerWindow.ShowQuestion($"¿Realmente desea eliminar el libro '{book.Name}'?"))
			{ 
				// Borra el libro
				new BookBussiness().Delete(book);
				// Actualiza
				Refresh();
			}
		}

		/// <summary>
		///		Crea los nodos de eBook correspondiente a los archivos pasados como parámetros
		/// </summary>
		public void CreateEBookNodes(BaseNodeViewModel nodeTarget, string[] fileNames, bool killSource)
		{
			LibraryModel library = null;

				// Obtiene la librería de un nodo
				if (nodeTarget is LibraryNodeViewModel libraryNode)
					library = libraryNode.Library;
				else if (nodeTarget is BookNodeViewModel bookNode)
					library = (bookNode.Parent as LibraryNodeViewModel).Library;
				// Añade los libros
				if (library != null)
					CreateEBookNodes(library, fileNames, killSource);
		}

		/// <summary>
		///		Crea los nodos de eBook correspondiente a los archivos pasados como parámetros
		/// </summary>
		private void CreateEBookNodes(LibraryModel library, string[] fileNames, bool killSource)
		{
			string pathTarget = null;

				// Obtiene el directorio destino
				if (library != null) // ... por los directorios raíz
					pathTarget = library.Path;
				// Si no se ha encontrado el directorio, recoge el directorio principal
				if (pathTarget.IsEmpty() || !System.IO.Directory.Exists(pathTarget))
					pathTarget = BookLibraryViewModel.Instance.BooksManager.Configuration.PathLibrary;
				// Guarda los libros en la librería
				foreach (string fileName in fileNames)
				{
					string fileTarget = LibCommonHelper.Files.HelperFiles.GetConsecutiveFileName(pathTarget, System.IO.Path.GetFileName(fileName));

						// Crea el directorio
						LibCommonHelper.Files.HelperFiles.MakePath(System.IO.Path.GetDirectoryName(fileTarget));
						// Copia el archivo
						if (!LibCommonHelper.Files.HelperFiles.CopyFile(fileName, fileTarget))
							BookLibraryViewModel.Instance.ControllerWindow.ShowMessage("No se ha podido copiar el archivo");
						else if (killSource) // Elimina el archivo origen si es necesario
							LibCommonHelper.Files.HelperFiles.KillFile(fileTarget);
				}
				// Actualiza
				Refresh();
		}

		/// <summary>
		///		Obtiene la biblioteca seleccionada (si está seleccionado un libro, se recoge la biblioteca padre)
		/// </summary>
		private LibraryModel GetSelectedLibrarySearch()
		{
			LibraryModel library = TreeBooks.GetSelectedLibrary();

				// Si está en un libro, se selecciona la biblioteca padre
				if (library == null)
					library = TreeBooks.GetSelectedLibraryParent();
				// Devuelve la biblioteca
				return library;
		}

		/// <summary>
		///		Arbol de bibliotecas / libros
		/// </summary>
		public TreeBooksViewModel TreeBooks
		{
			get { return _treeBooks; }
			set { CheckObject(ref _treeBooks, value); }
		}

		/// <summary>
		///		Comando de nueva librería
		/// </summary>
		public BaseCommand NewLibraryCommand { get; private set; }

		/// <summary>
		///		Comando de nuevo libro
		/// </summary>
		public BaseCommand NewBookCommand { get; private set; }

		/// <summary>
		///		Comando de nuevo libro
		/// </summary>
		public BaseCommand OpenBookCommand { get; private set; }

		/// <summary>
		///		Comando de compilar libro
		/// </summary>
		public BaseCommand CompileCommand { get; private set; }
	}
}
