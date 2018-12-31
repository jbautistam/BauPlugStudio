using System;

using Bau.Libraries.Plugins.Views.HostView;
using Bau.Libraries.Plugins.Views.HostView.Controllers;

namespace Bau.Applications.BauPlugStudio
{
	/// <summary>
	///		Configuración
	/// </summary>
	public static class Globals
	{   
		// Variables privadas
		private static PluginsManager _pluginsManager;

		/// <summary>
		///		Inicializa los datos globales
		/// </summary>
		internal static void Initialize(MainWindow mainWindow, Xceed.Wpf.AvalonDock.DockingManager dockManager)
		{
			HostController = new HostPluginsController(ApplicationName,
													   new Controllers.HostViewsController(ApplicationName, mainWindow), 
													   new Controllers.HostViewModelController(ApplicationName),
													   mainWindow, dockManager);
		}

		/// <summary>
		///		Nombre de la aplicación
		/// </summary>
		internal static string ApplicationName
		{
			get { return "BauPlugStudio"; }
		}

		/// <summary>
		///		Controlador principal
		/// </summary>
		internal static HostPluginsController HostController { get; private set; }

		/// <summary>
		///		Manager para la composición de plugins
		/// </summary>
		internal static PluginsManager PluginsManager
		{
			get
			{ 
				// Crea el manager de plugins con MEF si no existía
				if (_pluginsManager == null)
					_pluginsManager = new PluginsManager();
				// Devuelve el manager de composición de plugins
				return _pluginsManager;
			}
		}
	}
}
