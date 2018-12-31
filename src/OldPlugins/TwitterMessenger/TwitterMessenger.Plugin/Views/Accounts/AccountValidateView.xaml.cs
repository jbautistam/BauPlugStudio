using System;
using System.Windows;

using Bau.Libraries.LibRest.Authentication.Oauth;
using Bau.Libraries.LibTwitter;

namespace Bau.Plugins.TwitterMessenger.Views.Accounts
{
	/// <summary>
	///		Formulario para el mantenimiento de una cuenta
	/// </summary>
	public partial class AccountValidateView : Window
	{ 
		// Constantes privadas
		private const string UrlRequestToken = "https://api.twitter.com/oauth/request_token";
		private const string UrlAuthorize = "https://api.twitter.com/oauth/authorize";
		private const string UrlAccess = "https://api.twitter.com/oauth/access_token";
		// Variables privadas
		private oAuthAuthenticator _oAuthValidator = null;

		public AccountValidateView(TwitterAccount account)
		{ 
			// Inicializa los componentes
			InitializeComponent();
		}

		/// <summary>
		///		Inicializa el formulario
		/// </summary>
		private void InitForm()
		{
			// Inicializa el validador
			_oAuthValidator = new oAuthAuthenticator(TwitterMessengerPlugin.MainInstance.ViewModelManager.OAuthConsumerKey,
														TwitterMessengerPlugin.MainInstance.ViewModelManager.OAuthConsumerSecret);
			// Obtiene el token de validación
			if (_oAuthValidator.GetAuthorizationTokens(UrlRequestToken, "oob", out string oAuthToken, out string oAuthTokenSecret))
			{ 
				// Guarda los token
				OAuthToken = oAuthToken;
				OAuthSecretToken = oAuthTokenSecret;
				// Muestra el texto en el explorador
				wbBrowser.NavigateToString(GetHTML(oAuthToken));
			}
			else
				TwitterMessengerPlugin.MainInstance.HostPluginsController.ControllerWindow.ShowMessage("Error al intentar conectar con el servidor remoto");
		}

		/// <summary>
		///		Comprueba los datos introducidos
		/// </summary>
		private bool ValidateData()
		{
			bool validate = false;

				// Comprueba los datos
				if (string.IsNullOrEmpty(txtPIN.Text))
					TwitterMessengerPlugin.MainInstance.HostPluginsController.ControllerWindow.ShowMessage("Introduzca el código de autentificación de Twitter");
				else
					validate = true;
				// Devuelve el valor que indica si los datos son correctos
				return validate;
		}

		/// <summary>
		///		Graba los datos
		/// </summary>
		private void Save()
		{
			if (ValidateData())
			{
				// Asigna el token de autentificación para la solicitud
				_oAuthValidator.AccessToken = OAuthToken;
				// Obtiene los tokens de acceso a partir del PIN
				if (_oAuthValidator.GetAccessToken(UrlAccess, txtPIN.Text, out string oAuthToken, out string oAuthTokenSecret))
				{   
					// Recupera los tokens secretos
					OAuthToken = oAuthToken;
					OAuthSecretToken = oAuthTokenSecret;
					// Cierra el formulario
					DialogResult = true;
					Close();
				}
				else
					TwitterMessengerPlugin.MainInstance.HostPluginsController.ControllerWindow.ShowMessage("No se han podido verificar las credenciales del usuario");
			}
		}

		/// <summary>
		///		Obtiene la cadena HTML que se muestra en el explorador para solicitar la autorización
		///	a Twitter para la aplicación
		/// </summary>
		private string GetHTML(string oAuthToken)
		{
			string html = "<html lang='es'><head>";

				// Crea la cadena HTML
				html += " <meta http-equiv='Content-Language' content='en'>";
				html += " <meta charset='utf-8'>";
				html += " <title></title>";
				html += " <style type='text/css'>";
				html += " body {background-color:#5599BB;color:#ffffff;font-family:arial;}";
				html += " img{border:none}";
				html += " </style>";
				html += " </head>";
				html += " <body>";
				html += " <h1>Solicitud de verificación</h1>";
				html += " <p>Antes de utilizar esta aplicación debe iniciar sesión en Twitter.</p>";
				html += " <p>Pulse el botón de inicio de sesión situado debajo e inicie sesión en la página de Twitter.</p>";
				html += " <p>Una vez inicie la sesión, Twitter le proporcionara un PIN.</p>";
				html += " <p>Copie el PIN en el cuadro de texto del formulario y pulse Aceptar.</p>";
				html += " <p><a href='" + UrlAuthorize + "?oauth_token=" + oAuthToken + "'>";
				html += " <img src='https://g.twimg.com/dev/sites/default/files/images_documentation/sign-in-with-twitter-gray.png'";
				html += " alt='Autentificarse en Twitter'/></a></p>";
				html += " </body></html>";
				// Devuelve la cadena HTML
				return html;
		}

		/// <summary>
		///		Token de acceso
		/// </summary>
		public string OAuthToken { get; set; }

		/// <summary>
		///		Token secreto
		/// </summary>
		public string OAuthSecretToken { get; set; }

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			InitForm();
		}

		private void cmdValidate_Click(object sender, RoutedEventArgs e)
		{
			Save();
		}
	}
}