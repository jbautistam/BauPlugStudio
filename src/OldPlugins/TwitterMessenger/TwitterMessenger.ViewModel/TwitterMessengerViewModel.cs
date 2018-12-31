using System;
using Bau.Libraries.BauMvvm.ViewModels.Controllers;
using Bau.Libraries.Plugins.ViewModels;

namespace Bau.Libraries.TwitterMessenger.ViewModel
{
	/// <summary>
	///		Configuración de los ViewModel
	/// </summary>
	public class TwitterMessengerViewModel : BaseControllerViewModel
	{
		/// <summary>
		///		Inicializa los parámetros de ViewModel
		/// </summary>
		public TwitterMessengerViewModel(string moduleName, IHostViewModelController hostController, 
										 IHostSystemController hostSystemController, IHostDialogsController hostDialogsController, 
										 Controllers.IViewsController viewsController) 
						: base(moduleName, hostController, hostSystemController, hostDialogsController)
		{ 
			// Crea la instancia estática
			Instance = this;
			// Inicializa los datos
			TwitterMessenger = new TwitterBotManager();
			ViewsController = viewsController;
		}

		/// <summary>
		///		Inicializa la aplicación
		/// </summary>
		public override void InitModule()
		{
			TwitterMessenger.ManagerTwitter.OAuthConsumerKey = OAuthConsumerKey;
			TwitterMessenger.ManagerTwitter.OAuthConsumerSecret = OAuthConsumerSecret;
		}

		/// <summary>
		///		Manager de Twitter
		/// </summary>
		public TwitterBotManager TwitterMessenger { get; }

		/// <summary>
		///		Clave de OAuth
		/// </summary>
		public string OAuthConsumerKey
		{
			get { return GetParameter("OAuthConsumerKey"); }
			set
			{
				SetParameter("OAuthConsumerKey", value);
				TwitterMessenger.ManagerTwitter.OAuthConsumerKey = value;
			}
		}

		/// <summary>
		///		Secret de OAuth
		/// </summary>
		public string OAuthConsumerSecret
		{
			get { return GetParameter("OAuthConsumerSecret"); }
			set
			{
				SetParameter("OAuthConsumerSecret", value);
				TwitterMessenger.ManagerTwitter.OAuthConsumerSecret = value;
			}
		}

		/// <summary>
		///		Nombre del archivo de cuentas
		/// </summary>
		public string FileAccounts
		{
			get { return System.IO.Path.Combine(Instance.HostController.Configuration.PathBaseData, "AccountsTwitter.xml"); }
		}

		/// <summary>
		///		Controlador de vistas de aplicación
		/// </summary>
		public Controllers.IViewsController ViewsController { get; }

		/// <summary>
		///		Instancia actual del ViewModel
		/// </summary>
		public static TwitterMessengerViewModel Instance { get; private set; }
	}
}