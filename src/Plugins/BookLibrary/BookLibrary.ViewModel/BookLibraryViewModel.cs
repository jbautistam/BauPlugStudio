using System;
using Bau.Libraries.BauMvvm.ViewModels.Controllers;
using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.Plugins.ViewModels;

namespace Bau.Libraries.BookLibrary.ViewModel
{
	/// <summary>
	///		Configuración de los ViewModel
	/// </summary>
	public class BookLibraryViewModel : BaseControllerViewModel
	{ 
		public BookLibraryViewModel(string moduleName, IHostViewModelController hostController, 
								    IHostSystemController hostSystemController, IHostDialogsController hostDialogsController,
									Controllers.IViewsController viewsController) : base(moduleName, hostController, hostSystemController, hostDialogsController)
		{ 
			// Crea la instancia estática
			Instance = this;
			// Asigna el controlador de vistas
			ViewsController = viewsController;
			// Asigna las propiedades de configuración
			BooksManager.Configuration.PathLibrary = PathLibrary;
		}

		/// <summary>
		///		Inicializa el módulo
		/// </summary>
		public override void InitModule()
		{ 
			// ... no hace nada, simplemente implementa la interface
		}

		/// <summary>
		///		Manager de libros
		/// </summary>
		internal Application.BookLibraryManager BooksManager { get; } = new Application.BookLibraryManager();

		/// <summary>
		///		Directorio de la biblioteca
		/// </summary>
		public string PathLibrary
		{
			get
			{
				string pathLibrary = GetParameter(nameof(PathLibrary));

					// Si no se ha encontrado ningún parámetro devuelve el directorio base
					if (pathLibrary.IsEmpty())
						pathLibrary = System.IO.Path.Combine(HostController.Configuration.PathBaseData, "eBooks");
					// Devuelve el nombre de la base de datos
					return pathLibrary;
			}
			set
			{
				SetParameter("PathLibrary", value);
				BooksManager.Configuration.PathLibrary = value;
			}
		}

		/// <summary>
		///		Controlador de vistas de aplicación
		/// </summary>
		public Controllers.IViewsController ViewsController { get; }

		/// <summary>
		///		Instancia actual del ViewModel
		/// </summary>
		public static BookLibraryViewModel Instance { get; private set; }
	}
}
