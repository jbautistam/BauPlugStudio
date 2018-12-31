using System;
using System.Windows;

using Bau.Libraries.Plugins.ViewModels.Controllers;
using Bau.Libraries.BauMvvm.ViewModels.Controllers;
using Bau.Libraries.BauMvvm.Views.Controllers;

namespace Bau.Applications.BauPlugStudio.Controllers
{
	/// <summary>
	///		Controlador de ventanas
	/// </summary>
	internal class HostViewsController : IHostViewsController
	{
		internal HostViewsController(string applicationName, MainWindow mainWindow)
		{
			ApplicationName = applicationName;
			MainWindow = mainWindow;
		}

		/// <summary>
		///		Muestra un cuadro de diálogo
		/// </summary>
		public SystemControllerEnums.ResultType ShowDialog(Window view)
		{ 
			return ShowDialog(MainWindow, view);
		}

		/// <summary>
		///		Muestra un cuadro de diálogo
		/// </summary>
		public SystemControllerEnums.ResultType ShowDialog(Window owner, Window view)
		{ 
			// Si no se le ha pasado una ventana propietario, le asigna una
			if (owner == null)
				owner = MainWindow;
			// Muestra el formulario activo
			view.Owner = owner;
			view.ShowActivated = true;
			// Muestra el formulario y devuelve el resultado
			return ConvertDialogResult(view.ShowDialog());
		}

		/// <summary>
		///		Convierte el resultado de un cuadro de diálogo
		/// </summary>
		private SystemControllerEnums.ResultType ConvertDialogResult(bool? result)
		{
			if (result == null)
				return SystemControllerEnums.ResultType.Cancel;
			else if (result ?? false)
				return SystemControllerEnums.ResultType.Yes;
			else
				return SystemControllerEnums.ResultType.No;
		}

		/// <summary>
		///		Nombre de la aplicación
		/// </summary>
		public string ApplicationName { get; }

		/// <summary>
		///		Ventana principal de la aplicación
		/// </summary>
		internal Window MainWindow { get; }
	}
}