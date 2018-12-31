using System;

using Bau.Libraries.BauMvvm.ViewModels.Controllers;
using Bau.Libraries.DevConference.Admon.Application;
using Bau.Libraries.Plugins.ViewModels;

namespace Bau.Libraries.DevConferences.Admon.ViewModel
{
	/// <summary>
	///		Configuración de los ViewModel
	/// </summary>
	public class DevConferencesViewModel : BaseControllerViewModel
	{
		/// <summary>
		///		Inicializa los parámetros de ViewModel
		/// </summary>
		public DevConferencesViewModel(string moduleName, IHostViewModelController hostController, 
									   IHostSystemController hostSystemController, IHostDialogsController hostDialogsController,
									   Controllers.IViewsController viewsController) : base(moduleName, hostController, hostSystemController, hostDialogsController)
		{ 
			// Crea la instancia estática
			Instance = this;
			// Inicializa los datos
			ViewsController = viewsController;
		}

		/// <summary>
		///		Inicializa la aplicación
		/// </summary>
		public override void InitModule()
		{
			TrackManager = new AppTrackManager(PathTracks);
			TrackManager.Load();
		}

		/// <summary>
		///		Controlador de vistas de aplicación
		/// </summary>
		public Controllers.IViewsController ViewsController { get; }

		/// <summary>
		///		Instancia actual del ViewModel
		/// </summary>
		public static DevConferencesViewModel Instance { get; private set; }

		/// <summary>
		///		Manager de canales
		/// </summary>
		public AppTrackManager TrackManager { get; private set; }

		/// <summary>
		///		Directorio a partir del que se encuentran los datos de canales
		/// </summary>
		public string PathTracks
		{
			get
			{
				string pathTracks = GetParameter(nameof(PathTracks));

					// Si no se ha encontrado ningún parámetro crea el nombre del directorio
					if (string.IsNullOrWhiteSpace(pathTracks))
						pathTracks = System.IO.Path.Combine(HostController.Configuration.PathBaseData, "DevConferenceData");
					// Devuelve el directorio
					return pathTracks;
			}
			set
			{
				SetParameter(nameof(PathTracks), value);
				TrackManager.PathBase = value;
			}
		}
	}
}