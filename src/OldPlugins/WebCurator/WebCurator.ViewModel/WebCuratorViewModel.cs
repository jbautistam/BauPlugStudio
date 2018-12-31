using System;

using Bau.Libraries.BauMvvm.ViewModels.Controllers;
using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.Plugins.ViewModels;

namespace Bau.Libraries.WebCurator.ViewModel
{
	/// <summary>
	///		Configuración de los ViewModel
	/// </summary>
	public class WebCuratorViewModel : BaseControllerViewModel
	{ 
		public WebCuratorViewModel(string moduleName, IHostViewModelController hostController, 
								   IHostSystemController hostSystemController,
								   IHostDialogsController hostDialogsController,
								   Controllers.IViewsController viewsController) : base(moduleName, hostController, hostSystemController, hostDialogsController)
		{ 
			// Crea la instancia estática
			Instance = this;
			// Asigna el manager de blogs y el controlador de vistas
			WebCuratorManager = new Application.WebCuratorManager();
			ViewsController = viewsController;
			MessagesController = new Controllers.MessagesController();
		}

		/// <summary>
		///		Inicializa el módulo
		/// </summary>
		public override void InitModule()
		{
			BotWebCurator = new Controllers.AutomaticProcessor(AutoStartCompile, MinutesBetweenCompile);
		}

		/// <summary>
		///		Manager de sitios Web
		/// </summary>
		public Application.WebCuratorManager WebCuratorManager { get; }

		/// <summary>
		///		Bot de generación de sitios Web
		/// </summary>
		internal Controllers.AutomaticProcessor BotWebCurator { get; private set; }

		/// <summary>
		///		Directorio de la biblioteca
		/// </summary>
		public string PathLibrary
		{
			get
			{
				string pathLibrary = GetParameter("PathLibrary");

					// Si no se ha encontrado ningún parámetro devuelve el directorio base
					if (pathLibrary.IsEmpty())
						pathLibrary = System.IO.Path.Combine(HostController.Configuration.PathBaseData, "WebCurator");
					// Devuelve el nombre de la base de datos
					return pathLibrary;
			}
			set { SetParameter("PathLibrary", value); }
		}

		/// <summary>
		///		Directorio de compilación de las Webs creadas con WebCurator
		/// </summary>
		public string PathGeneration
		{
			get
			{
				string pathLibrary = GetParameter("PathGeneration");

					// Si no se ha encontrado ningún parámetro devuelve el directorio base
					if (pathLibrary.IsEmpty())
						pathLibrary = System.IO.Path.Combine(HostController.Configuration.PathBaseData, "WebCurator\\Generate");
					// Devuelve el nombre de la base de datos
					return pathLibrary;
			}
			set { SetParameter("PathGeneration", value); }
		}

		/// <summary>
		///		Indica si se debe iniciar la compilación cuando se arranca el programa
		/// </summary>
		public bool AutoStartCompile
		{
			get { return GetParameter("AutoStartCompile").GetBool(false); }
			set { SetParameter("AutoStartCompile", value); }
		}

		/// <summary>
		///		Minutos que deben pasar entre los procesos de compilación
		/// </summary>
		public int MinutesBetweenCompile
		{
			get { return GetParameter("MinutesBetweenCompile").GetInt(60); }
			set { SetParameter("MinutesBetweenCompile", value); }
		}

		/// <summary>
		///		Controlador de vistas de aplicación
		/// </summary>
		public Controllers.IViewsController ViewsController { get; }

		/// <summary>
		///		Controlador de mensajes
		/// </summary>
		internal Controllers.MessagesController MessagesController { get; }

		/// <summary>
		///		Controlador para las acciones que vienen del plugin SourceEditor
		/// </summary>
		internal Controllers.SourceEditorPluginManager SourceEditorPluginManager { get; } = new Controllers.SourceEditorPluginManager();

		/// <summary>
		///		Instancia actual del ViewModel
		/// </summary>
		public static WebCuratorViewModel Instance { get; private set; }
	}
}
