using System;

using Bau.Libraries.BauMvvm.ViewModels.Controllers;
using Bau.Libraries.LibCommonHelper.Extensors;
using Bau.Libraries.Plugins.ViewModels;

namespace Bau.Libraries.LibMediaPlayer.ViewModel
{
	/// <summary>
	///		Configuración de los ViewModel
	/// </summary>
	public class MediaPlayerViewModel : BaseControllerViewModel
	{
		// Enumerados públicos
		/// <summary>
		///		Indice de iconos
		/// </summary>
		public enum IconIndex
		{
			/// <summary>Icono para la nueva carpeta</summary>
			NewFolder,
			/// <summary>Icono para el nuevo álbum</summary>
			NewAlbum
		}

		public MediaPlayerViewModel(string moduleName, IHostViewModelController hostController, 
								    IHostSystemController hostSystemController,
									IHostDialogsController hostDialogsController,
								    Controllers.IViewsController viewsController,
								    System.Collections.Generic.Dictionary<IconIndex, string> dctImagesRoutes)
										: base(moduleName, hostController, hostSystemController, hostDialogsController)
		{
			// Crea la instancia estática
			Instance = this;
			// Asigna el manager de blogs y el controlador de vistas
			MediaManager = new Application.MediaPlayerManager();
			ViewsController = viewsController;
			MessengerController = new Controllers.Messengers.MediaMessengerController(this);
			// Asigna las propiedades de configuración
			MediaManager.PathFiles = PathFiles;
			// Asigna las rutas a los iconos
			ImageRoutes = dctImagesRoutes;
		}

		/// <summary>
		///		Inicializa el módulo
		/// </summary>
		public override void InitModule()
		{
			MediaManager.Load();
		}

		/// <summary>
		///		Envía un mensaje de cambio de estado
		/// </summary>
		internal void SendMesageChangeStatus(object content = null)
		{
			HostController.Messenger.Send(new Controllers.Messengers.MessageChangeStatusMediaFiles(content));
		}

		/// <summary>
		///		Obtiene la ruta a un icono
		/// </summary>
		internal string GetIconRoute(IconIndex iconIndex)
		{
			if (ImageRoutes != null && ImageRoutes.TryGetValue(iconIndex, out string icon))
				return icon;
			else
				return null;
		}

		/// <summary>
		///		Manager de archivos multimedia
		/// </summary>
		internal Application.MediaPlayerManager MediaManager { get; private set; }

		/// <summary>
		///		Controlador de mensajes
		/// </summary>
		internal Controllers.Messengers.MediaMessengerController MessengerController { get; private set; }

		/// <summary>
		///		Directorio a partir del que se encuentran los datos de los archivos multimedia
		/// </summary>
		public string PathFiles
		{
			get
			{
				string pathFiles = GetParameter(nameof(PathFiles));

					// Si no se ha encontrado ningún parámetro crea el nombre del directorio
					if (pathFiles.IsEmpty())
						pathFiles = System.IO.Path.Combine(HostController.Configuration.PathBaseData, "MediaFiles");
					// Devuelve el directorio
					return pathFiles;
			}
			set
			{
				SetParameter(nameof(PathFiles), value);
				MediaManager.PathFiles = value;
			}
		}

		/// <summary>
		///		Controlador de vistas de aplicación
		/// </summary>
		public Controllers.IViewsController ViewsController { get; private set; }

		/// <summary>
		///		Rutas para las imágenes
		/// </summary>
		public System.Collections.Generic.Dictionary<IconIndex, string> ImageRoutes { get; private set; }

		/// <summary>
		///		Instancia actual del ViewModel
		/// </summary>
		public static MediaPlayerViewModel Instance { get; private set; }
	}
}
