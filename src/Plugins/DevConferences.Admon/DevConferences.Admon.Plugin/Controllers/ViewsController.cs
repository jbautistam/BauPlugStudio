using System;

using Bau.Libraries.BauMvvm.ViewModels.Controllers;
using Bau.Libraries.DevConferences.Admon.ViewModel.Projects;
using Bau.Libraries.Plugins.Views.Host;
using Bau.Plugins.DevConferences.Admon.Views.Projects;

namespace Bau.Plugins.DevConferences.Admon.Controllers
{
	/// <summary>
	///		Controlador de ventanas de DevConference
	/// </summary>
	public class ViewsController : Libraries.DevConferences.Admon.ViewModel.Controllers.IViewsController
	{
		/// <summary>
		///		Abre el árbolo de conferencias
		/// </summary>
		public void OpenDevConferenceView()
		{
			DevConferencesPlugin.MainInstance.HostPluginsController.LayoutController.ShowDockPane("DEV_CONFERENCE_EXPLORER", LayoutEnums.DockPosition.Left,
																								  "DevConferences", new Views.Explorer.TreeTracksView());
		}

		/// <summary>
		///		Abre el formulario de modificación de un manager de canales
		/// </summary>
		public SystemControllerEnums.ResultType OpenPropertiesTrackManager(TrackManagerViewModel viewModel)
		{
			return DevConferencesPlugin.MainInstance.HostPluginsController.HostViewsController.ShowDialog(new ctlTrackManagerView(viewModel));
		}

		/// <summary>
		///		Abre el formulario de modificación de un canal
		/// </summary>
		public SystemControllerEnums.ResultType OpenPropertiesTrack(TrackViewModel viewModel)
		{
			return DevConferencesPlugin.MainInstance.HostPluginsController.HostViewsController.ShowDialog(new ctlTrackView(viewModel));
		}

		/// <summary>
		///		Abre el formulario de modificación de una categoría
		/// </summary>
		public SystemControllerEnums.ResultType OpenPropertiesCategory(CategoryViewModel viewModel)
		{
			return DevConferencesPlugin.MainInstance.HostPluginsController.HostViewsController.ShowDialog(new ctlCategoryView(viewModel));
		}

		/// <summary>
		///		Abre el formulario de modificación de una entrada
		/// </summary>
		public SystemControllerEnums.ResultType OpenPropertiesEntry(EntryViewModel viewModel)
		{
			return DevConferencesPlugin.MainInstance.HostPluginsController.HostViewsController.ShowDialog(new ctlEntryView(viewModel));
		}

		/// <summary>
		///		Abre una ventana del navegador
		/// </summary>
		public void ShowWebBrowser(string urlVideo)
		{
			DevConferencesPlugin.MainInstance.HostPluginsController.ShowWebBrowser(urlVideo);
		}

		/// <summary>
		///		Imagen de un servidor conectado
		/// </summary>
		public string ImageServerConnected
		{
			get { return "pack://application:,,,/DevConferences.Admon.Plugin;component/Resources/ServerConnected.png"; }
		}

		/// <summary>
		///		Imagen de un servidor desconectado
		/// </summary>
		public string ImageServerDisconnected
		{
			get { return "pack://application:,,,/DevConferences.Admon.Plugin;component/Resources/ServerDisconnected.png"; }
		}

		/// <summary>
		///		Imagen de un grupo
		/// </summary>
		public string ImageGroup
		{
			get { return "pack://application:,,,/DevConferences.Admon.Plugin;component/Resources/Group.png"; }
		}

		/// <summary>
		///		Imagen de un usuario pendiente de aprobar solicitud
		/// </summary>
		public string ImageUserPendingRequest
		{
			get { return "pack://application:,,,/DevConferences.Admon.Plugin;component/Resources/UserPendingRequest.png"; }
		}

		/// <summary>
		///		Imagen de un usuario en estado Away
		/// </summary>
		public string ImageUserStatusAway
		{
			get { return "pack://application:,,,/DevConferences.Admon.Plugin;component/Resources/UserStatusAway.png"; }
		}

		/// <summary>
		///		Imagen de un usuario en estado Chat
		/// </summary>
		public string ImageUserStatusChat
		{
			get { return "pack://application:,,,/DevConferences.Admon.Plugin;component/Resources/UserStatusChat.png"; }
		}

		/// <summary>
		///		Imagen de un usuario en estado Dnd
		/// </summary>
		public string ImageUserStatusDnd
		{
			get { return "pack://application:,,,/DevConferences.Admon.Plugin;component/Resources/UserStatusDnd.png"; }
		}

		/// <summary>
		///		Imagen de un usuario en estado Offline
		/// </summary>
		public string ImageUserStatusOffline
		{
			get { return "pack://application:,,,/DevConferences.Admon.Plugin;component/Resources/UserStatusOffline.png"; }
		}

		/// <summary>
		///		Imagen de un usuario en estado Online
		/// </summary>
		public string ImageUserStatusOnline
		{
			get { return "pack://application:,,,/DevConferences.Admon.Plugin;component/Resources/UserStatusOnline.png"; }
		}

		/// <summary>
		///		Imagen de un usuario en estado Xa
		/// </summary>
		public string ImageUserStatusXa
		{
			get { return "pack://application:,,,/DevConferences.Admon.Plugin;component/Resources/UserStatusXa.png"; }
		}
	}
}
