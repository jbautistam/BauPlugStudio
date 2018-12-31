using System;

using Bau.Libraries.BauMvvm.ViewModels.Controllers;
using Bau.Libraries.Plugins.ViewModels;

namespace Bau.Libraries.DatabaseStudio.ViewModels
{
	/// <summary>
	///		ViewModel principal de la aplicación
	/// </summary>
	public class MainViewModel : BaseControllerViewModel
	{
		public MainViewModel(string moduleName, IHostViewModelController hostController, 
							 IHostSystemController hostSystemController, IHostDialogsController hostDialogsController, 
							 Controllers.IViewsController mainController)
					: base(moduleName, hostController, hostSystemController, hostDialogsController)
		{
			Instance = this;
			ViewsController = mainController;
		}

		/// <summary>
		///		Inicializa el módulo
		/// </summary>
		public override void InitModule()
		{
		}

		/// <summary>
		///		Instancia principal
		/// </summary>
		public static MainViewModel Instance { get; private set; }

		/// <summary>
		///		Controlador de ventanas
		/// </summary>
		public Controllers.IViewsController ViewsController { get; }

		/// <summary>
		///		Directorio base de los proyectos
		/// </summary>
		public string ProjectsPathBase 
		{ 
			get 
			{ 
				string path = HostController.Configuration.Parameters.GetValue(ModuleName, nameof(ProjectsPathBase));

					// Si no se ha configurado ningún directorio, se crea el predeterminado
					if (string.IsNullOrEmpty(path))
						path = System.IO.Path.Combine(HostController.Configuration.PathBaseData, "Database");
					// Devuelve el directorio
					return path;
			}
			set { HostController.Configuration.Parameters.SetValue(ModuleName, nameof(ProjectsPathBase), value); }
		}

		/// <summary>
		///		Ultimo directorio de proyectos
		/// </summary>
		public string LastProject 
		{ 
			get { return HostController.Configuration.Parameters.GetValue(ModuleName, nameof(LastProject)); }
			set { HostController.Configuration.Parameters.SetValue(ModuleName, nameof(LastProject), value); }
		}
	}
}
