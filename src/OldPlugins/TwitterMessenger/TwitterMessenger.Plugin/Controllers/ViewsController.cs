using System;
using System.Windows;

using Bau.Libraries.BauMvvm.ViewModels.Controllers;
using Bau.Libraries.Plugins.Views.Host;
using Bau.Libraries.TwitterMessenger.ViewModel.Accounts;

namespace Bau.Plugins.TwitterMessenger.Controllers
{
	/// <summary>
	///		Controlador de ventanas de TwitterMessenger
	/// </summary>
	public class ViewsController : Libraries.TwitterMessenger.ViewModel.Controllers.IViewsController
	{
		/// <summary>
		///		Abre el panel de mensajes de Twitter
		/// </summary>
		public void OpenTwitterMessagesView()
		{
			TwitterMessengerPlugin.MainInstance.HostPluginsController.LayoutController.ShowDockPane("TWITTER_BOT_MESSAGES",
																									LayoutEnums.DockPosition.Right,
																									"Mensajes Twitter", new Views.Messages.TwitterMessagesView());
		}

		/// <summary>
		///		Abre el formulario de modificación de una cuenta
		/// </summary>
		public SystemControllerEnums.ResultType OpenFormPropertiesAccount(AccountViewModel viewModel)
		{
			return TwitterMessengerPlugin.MainInstance.HostPluginsController.HostViewsController.ShowDialog(new Views.Accounts.AccountView(viewModel));
		}

		/// <summary>
		///		Abre el formulario de validación de una cuenta
		/// </summary>
		public SystemControllerEnums.ResultType OpenFormValidateAccount(Libraries.LibTwitter.TwitterAccount account)
		{
			Views.Accounts.AccountValidateView frmValidate = new Views.Accounts.AccountValidateView(account);
			SystemControllerEnums.ResultType result = TwitterMessengerPlugin.MainInstance.HostPluginsController.HostViewsController.ShowDialog(frmValidate);

				// Si se ha validado el usuario, recoge los resultados
				if (result == SystemControllerEnums.ResultType.Yes)
				{   
					// Asigna los datos
					account.OAuthToken = frmValidate.OAuthToken;
					account.OAuthTokenSecret = frmValidate.OAuthSecretToken;
				}
				// Devuelve el resultado
				return result;
		}

		/// <summary>
		///		Muestra el browser
		/// </summary>
		public void ShowWebBrowser(string url)
		{
			TwitterMessengerPlugin.MainInstance.HostPluginsController.ShowWebBrowser(url);
		}
	}
}
