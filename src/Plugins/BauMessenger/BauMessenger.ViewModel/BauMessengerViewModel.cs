using System;

using Bau.Libraries.BauMvvm.ViewModels.Controllers;
using Bau.Libraries.Plugins.ViewModels;

namespace Bau.Libraries.BauMessenger.ViewModel
{
	/// <summary>
	///		Configuración de los ViewModel
	/// </summary>
	public class BauMessengerViewModel : BaseControllerViewModel
	{
		/// <summary>
		///		Inicializa los parámetros de ViewModel
		/// </summary>
		public BauMessengerViewModel(string moduleName, IHostViewModelController hostController, 
									 IHostSystemController hostSystemController,
									 IHostDialogsController hostDialogsController,
									 Controllers.IViewsController viewsController) : base(moduleName, hostController, hostSystemController, hostDialogsController)
		{ 
			// Crea la instancia estática
			Instance = this;
			// Inicializa los datos
			BauMessenger = new Controllers.Xmpp.XmppManager();
			ViewsController = viewsController;
		}

		/// <summary>
		///		Inicializa la aplicación
		/// </summary>
		public override void InitModule()
		{
		}

		/// <summary>
		///		Manager de XMPP
		/// </summary>
		internal Controllers.Xmpp.XmppManager BauMessenger { get; }

		/// <summary>
		///		Nombre del archivo de cuentas
		/// </summary>
		public string FileAccounts
		{
			get { return System.IO.Path.Combine(Instance.HostController.Configuration.PathBaseData, "BauMessengerAccounts.xml"); }
		}

		/// <summary>
		///		Controlador de vistas de aplicación
		/// </summary>
		public Controllers.IViewsController ViewsController { get; }

		/// <summary>
		///		Instancia actual del ViewModel
		/// </summary>
		public static BauMessengerViewModel Instance { get; private set; }
	}
}