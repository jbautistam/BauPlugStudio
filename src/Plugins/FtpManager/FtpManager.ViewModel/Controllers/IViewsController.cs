using System;

using Bau.Libraries.BauMvvm.ViewModels.Controllers;
using Bau.Libraries.FtpManager.Model.Connections;

namespace Bau.Libraries.FtpManager.ViewModel.Controllers
{
	/// <summary>
	///		Interface con los controladores de las vistas
	/// </summary>
	public interface IViewsController
	{
		/// <summary>
		///		Abre el formulario de modificación de conexión
		/// </summary>
		SystemControllerEnums.ResultType OpenFormUpdateConnection(Connections.FtpConnectionViewModel viewModel);

		/// <summary>
		///		Abre una conexión a un servidor FTP
		/// </summary>
		void OpenFormFtp(FtpConnectionModel ftpConnection);

		/// <summary>
		///		Icono del archivo de proyecto
		/// </summary>
		string IconFileProject { get; }

		/// <summary>
		///		Icono del archivo de conexión
		/// </summary>
		string IconFileConnectionFtp { get; }

		/// <summary>
		///		Icono del menú abrir conexión FTP
		/// </summary>
		string IconMenuOpenFtpConnection { get; }
	}
}
