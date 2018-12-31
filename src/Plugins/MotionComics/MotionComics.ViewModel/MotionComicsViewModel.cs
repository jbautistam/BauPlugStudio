using System;

using Bau.Libraries.BauMvvm.ViewModels.Controllers;
using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.Plugins.ViewModels;

namespace Bau.Libraries.MotionComics.ViewModel
{
	/// <summary>
	///		Configuración de los ViewModel
	/// </summary>
	public class MotionComicsViewModel : BaseControllerViewModel
	{
		public MotionComicsViewModel(string moduleName, IHostViewModelController hostController, 
								     IHostSystemController hostSystemController,
									 IHostDialogsController hostDialogsController,
									 Controllers.IViewsController viewsController) : base(moduleName, hostController, hostSystemController, hostDialogsController)
		{ 
			// Crea la instancia estática
			Instance = this;
			// Inicializa los controladores
			ViewsController = viewsController;
			MessagesController = new Controllers.MessagesController();
		}

		/// <summary>
		///		Inicializa el módulo
		/// </summary>
		public override void InitModule()
		{
		}

		/// <summary>
		///		Controlador de formularios de la aplicación
		/// </summary>
		public Controllers.IViewsController ViewsController { get; private set; }

		/// <summary>
		///		Controlador de mensajes
		/// </summary>
		internal Controllers.MessagesController MessagesController { get; private set; }

		/// <summary>
		///		Controlador para las opciones que vienen del plugin SourceEditor
		/// </summary>
		internal Controllers.SourceEditorPluginManager SourceEditorPluginManager { get; } = new Controllers.SourceEditorPluginManager();

		/// <summary>
		///		Instancia actual del ViewModel
		/// </summary>
		public static MotionComicsViewModel Instance { get; private set; }

		/// <summary>
		///		Valor de configuración: indica si se muestran los adorners
		/// </summary>
		public bool ShowAdorners
		{
			get { return GetParameter("ShowAdorners").GetBool(); }
			set { SetParameter("ShowAdorners", value); }
		}
	}
}
