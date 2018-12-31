using System;

using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.BauMvvm.ViewModels;
using Bau.Libraries.BookLibrary.Model.Books;
using Bau.Libraries.BauMvvm.ViewModels.Forms;

namespace Bau.Libraries.BookLibrary.ViewModel.Books.Content
{
	/// <summary>
	///		ViewModel para ver el contenido de un <see cref="BookModel"/>
	/// </summary>
	public abstract class BaseContentViewModel : BaseFormViewModel
	{   
		// Variables privadas
		private string _pathTarget;
		private int _actualPage, _pageIndex;

		public BaseContentViewModel(BookModel book) : base(false)
		{
			Book = book;
			InitViewModel();
		}

		/// <summary>
		///		Inicializa el ViewModel: propiedades, menús y comandos
		/// </summary>
		private void InitViewModel()
		{ 
			// Inicializa los comandos de página
			FirstPageCommand = CreateCommandForPage(nameof(FirstPageCommand));
			NextPageCommand = CreateCommandForPage(nameof(NextPageCommand));
			PreviousPageCommand = CreateCommandForPage(nameof(PreviousPageCommand));
			LastPageCommand = CreateCommandForPage(nameof(LastPageCommand));
			// Al cerrar el formulario, borrar el directorio temporal
			RequestClose += (sender, evntArgs) =>
										{
											if (!PathTarget.IsEmpty() && System.IO.Directory.Exists(PathTarget))
												LibCommonHelper.Files.HelperFiles.KillPath(PathTarget);
										};
		}

		/// <summary>
		///		Crea un comando asociado a la página HTML
		/// </summary>
		private BaseCommand CreateCommandForPage(string action)
		{
			return new BaseCommand(parameter => ExecuteAction(action, parameter),
								   parameter => CanExecuteAction(action, parameter))
							.AddListener(this, nameof(Pages)).AddListener(this, nameof(ActualPage));
		}

		/// <summary>
		///		Interpreta el libro
		/// </summary>
		public void Parse()
		{ 
			// Interpreta el libro
			EBookContent = new Application.Bussiness.BookBussiness().Parse(Book, PathTarget);
			Pages = EBookContent.CountPages();
			ActualPage = 1;
			// Inicializa el ViewModel
			InitBookView();
		}

		/// <summary>
		///		Inicializa la vista con el libro interpretado
		/// </summary>
		protected abstract void InitBookView();

		/// <summary>
		///		Muestra una página a partir del número de página
		/// </summary>
		protected void ShowPage(int pageIndex)
		{
			pageIndex--;
			if (pageIndex >= 0 && pageIndex < Pages)
				ShowPageReal(pageIndex);
		}

		/// <summary>
		///		Función para mostrar la página
		/// </summary>
		protected abstract void ShowPageReal(int pageIndex);

		/// <summary>
		///		Ejecuta una acción
		/// </summary>
		protected override void ExecuteAction(string action, object parameter)
		{
			switch (action)
			{
				case nameof(FirstPageCommand):
						ActualPage = 1;
					break;
				case nameof(PreviousPageCommand):
						ActualPage--;
					break;
				case nameof(LastPageCommand):
						ActualPage = Pages;
					break;
				case nameof(NextPageCommand):
						ActualPage++;
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
				case nameof(FirstPageCommand):
				case nameof(PreviousPageCommand):
					return ActualPage > 1;
				case nameof(LastPageCommand):
				case nameof(NextPageCommand):
					return ActualPage < Pages;
				default:
					return false;
			}
		}

		/// <summary>
		///		Libro
		/// </summary>
		internal BookModel Book { get; set; }

		/// <summary>
		///		Páginas del libro
		/// </summary>
		public BookPageModelCollection EBookContent { get; set; }

		/// <summary>
		///		Nombre de archivo
		/// </summary>
		public string FileName
		{
			get { return Book.FileName; }
		}

		/// <summary>
		///		Título del archivo
		/// </summary>
		public string Name
		{
			get { return Book.Name; }
		}

		/// <summary>
		///		Directorio destino
		/// </summary>
		public string PathTarget
		{
			get
			{ 
				// Obtiene un directorio temporal si no existía
				if (_pathTarget.IsEmpty())
					_pathTarget = System.IO.Path.Combine(System.IO.Path.GetTempPath(), LibCommonHelper.Files.HelperFiles.Normalize(Book.Name));
				// Devuelve el directorio temporal
				return _pathTarget;
			}
		}

		/// <summary>
		///		Página actual
		/// </summary>
		public int ActualPage
		{
			get { return _actualPage; }
			set
			{
				if (CheckProperty(ref _actualPage, value))
					ShowPage(_actualPage);
			}
		}

		/// <summary>
		///		Número de páginas
		/// </summary>
		public int Pages
		{
			get { return _pageIndex; }
			set { CheckProperty(ref _pageIndex, value); }
		}

		/// <summary>
		///		Comando para mostrar la primera página
		/// </summary>
		public BaseCommand FirstPageCommand { get; private set; }

		/// <summary>
		///		Comando para mostrar la siguiente página
		/// </summary>
		public BaseCommand NextPageCommand { get; private set; }

		/// <summary>
		///		Comando para mostrar la página anterior
		/// </summary>
		public BaseCommand PreviousPageCommand { get; private set; }

		/// <summary>
		///		Comando para mostrar la última página
		/// </summary>
		public BaseCommand LastPageCommand { get; private set; }
	}
}
