using System;

using Bau.Libraries.BauMvvm.ViewModels.Controllers;

namespace Bau.Libraries.DevConferences.Admon.ViewModel.Controllers
{
	/// <summary>
	///		Interface con los controladores de las vistas
	/// </summary>
	public interface IViewsController 
	{
		/// <summary>
		///		Abre el panel de la aplicación
		/// </summary>
		void OpenDevConferenceView();

		/// <summary>
		///		Abre el formulario de modificación de un manager de canales
		/// </summary>
		SystemControllerEnums.ResultType OpenPropertiesTrackManager(Projects.TrackManagerViewModel viewModel);

		/// <summary>
		///		Abre el formulario de modificación de un canal
		/// </summary>
		SystemControllerEnums.ResultType OpenPropertiesTrack(Projects.TrackViewModel viewModel);

		/// <summary>
		///		Abre el formulario de modificación de una categoría
		/// </summary>
		SystemControllerEnums.ResultType OpenPropertiesCategory(Projects.CategoryViewModel viewModel);

		/// <summary>
		///		Abre el formulario de modificación de una entrada
		/// </summary>
		SystemControllerEnums.ResultType OpenPropertiesEntry(Projects.EntryViewModel viewModel);

		/// <summary>
		///		Abre una URL en el navegador
		/// </summary>
		void ShowWebBrowser(string urlVideo);

		/// <summary>
		///		Imagen de servidor desconectado
		/// </summary>
		string ImageServerDisconnected { get; }

		/// <summary>
		///		Imagen de servidor conectado
		/// </summary>
		string ImageServerConnected { get; }

		/// <summary>
		///		Imagen de grupo
		/// </summary>
		string ImageGroup { get; }

		/// <summary>
		///		Imagen de un usuario pendiente de aprobar solicitud
		/// </summary>
		string ImageUserPendingRequest { get; }

		/// <summary>
		///		Imagen de un usuario en estado Away
		/// </summary>
		string ImageUserStatusAway { get; }

		/// <summary>
		///		Imagen de un usuario en estado Chat
		/// </summary>
		string ImageUserStatusChat { get; }

		/// <summary>
		///		Imagen de un usuario en estado Dnd
		/// </summary>
		string ImageUserStatusDnd { get; }

		/// <summary>
		///		Imagen de un usuario en estado Offline
		/// </summary>
		string ImageUserStatusOffline { get; }

		/// <summary>
		///		Imagen de un usuario en estado Online
		/// </summary>
		string ImageUserStatusOnline { get; }

		/// <summary>
		///		Imagen de un usuario en estado Xa
		/// </summary>
		string ImageUserStatusXa { get; }
	}
}