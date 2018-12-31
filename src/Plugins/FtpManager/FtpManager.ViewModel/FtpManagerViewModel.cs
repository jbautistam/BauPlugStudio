using System;

using Bau.Libraries.BauMvvm.ViewModels.Controllers;
using Bau.Libraries.Plugins.ViewModels;

namespace Bau.Libraries.FtpManager.ViewModel
{
	/// <summary>
	///		Configuración de los ViewModel
	/// </summary>
	public class FtpManagerViewModel : BaseControllerViewModel
	{
		public FtpManagerViewModel(string moduleName, IHostViewModelController hostController, 
								   IHostSystemController hostSystemController, IHostDialogsController hostDialogsController, 
								   Controllers.IViewsController viewsController) 
					: base(moduleName, hostController, hostSystemController, hostDialogsController)
		{ 
			// Crea la instancia estática
			Instance = this;
			// Inicializa los controladores
			ViewsController = viewsController;
			Messenger = new Controllers.MessagesController();
		}

		/// <summary>
		///		Inicializa el módulo
		/// </summary>
		public override void InitModule()
		{
		}

		/// <summary>
		///		Obtiene el directorio asociado a un parámetro
		/// </summary>
		private string GetPath(string parameterName, string pathAdditional)
		{
			string path = "";

				// Obtiene la configuración
				if (HostController != null)
				{ 
					// Obtiene el directorio
					path = GetParameter(parameterName);
					// Si no hay nada, crea el directorio por defecto
					if (string.IsNullOrEmpty(path))
						path = System.IO.Path.Combine(HostController.Configuration.PathBaseData, pathAdditional);
				}
				// Devuelve el directorio
				return path;
		}

		/// <summary>
		///		Controlador de formularios de la aplicación
		/// </summary>
		public Controllers.IViewsController ViewsController { get; }

		/// <summary>
		///		Controlador para las acciones desencadenadas desde el plugin de SourceStudio
		/// </summary>
		internal Controllers.SourceEditorPluginManager SourceEditorPluginManager { get; } = new Controllers.SourceEditorPluginManager();

		/// <summary>
		///		Instancia actual del ViewModel
		/// </summary>
		public static FtpManagerViewModel Instance { get; private set; }

		/// <summary>
		///		Controlador de mensajes
		/// </summary>
		private Controllers.MessagesController Messenger { get; }
	}
}
