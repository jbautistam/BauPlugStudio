using System;

using Bau.Libraries.BauMvvm.ViewModels.Controllers;

namespace Bau.Libraries.TwitterMessenger.ViewModel.Controllers
{
	/// <summary>
	///		Interface con los controladores de las vistas
	/// </summary>
	public interface IViewsController
	{
		/// <summary>
		///		Abre el panel de mensajes de Twitter
		/// </summary>
		void OpenTwitterMessagesView();

		/// <summary>
		///		Abre el formulario de modificación de una cuenta
		/// </summary>
		SystemControllerEnums.ResultType OpenFormPropertiesAccount(Accounts.AccountViewModel viewModel);

		/// <summary>
		///		Abre el formulario de validación de cuenta
		/// </summary>
		SystemControllerEnums.ResultType OpenFormValidateAccount(LibTwitter.TwitterAccount account);

		/// <summary>
		///		Abre el navegador web
		/// </summary>
		void ShowWebBrowser(string url);
	}
}
