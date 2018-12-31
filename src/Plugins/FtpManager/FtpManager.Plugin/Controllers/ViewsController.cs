using System;

using Bau.Libraries.BauMvvm.ViewModels.Controllers;
using Bau.Libraries.FtpManager.ViewModel.Connections;

namespace Bau.Plugins.FtpManager.Controllers
{
	/// <summary>
	///		Controlador de ventanas de FtpManager
	/// </summary>
	public class ViewsController : Libraries.FtpManager.ViewModel.Controllers.IViewsController
	{
		/// <summary>
		///		Abre el formulario de modificación de una conexión
		/// </summary>
		public SystemControllerEnums.ResultType OpenFormUpdateConnection(FtpConnectionViewModel viewModel)
		{
			return FtpManagerPlugin.MainInstance.HostPluginsController.HostViewsController.ShowDialog(new Views.Connections.ConnectionView(viewModel));
		}

		/// <summary>
		///		Abre la conexión a un servidor FTP
		/// </summary>
		public void OpenFormFtp(Libraries.FtpManager.Model.Connections.FtpConnectionModel ftpConnection)
		{
			FtpManagerPlugin.MainInstance.HostPluginsController.LayoutController.ShowDocument($"FTP_EXPLORER_{ftpConnection.Server}", "Explorador FTP",
																							  new Views.FtpExplorer.FtpExplorerView(ftpConnection));
		}

		/// <summary>
		///		Icono del archivo de conexión
		/// </summary>
		public string IconFileConnectionFtp
		{
			get { return "pack://application:,,,/FtpManager.Plugin;component/Themes/Images/Connection.png"; }
		}

		/// <summary>
		///		Icono del menú para abrir una conexión
		/// </summary>
		public string IconMenuOpenFtpConnection
		{
			get { return "pack://application:,,,/FtpManager.Plugin;component/Themes/Images/Process.png"; }
		}

		/// <summary>
		///		Icono del archivo de proyecto
		/// </summary>
		public string IconFileProject
		{
			get { return "pack://application:,,,/FtpManager.Plugin;component/Themes/Images/Project.png"; }
		}
	}
}
