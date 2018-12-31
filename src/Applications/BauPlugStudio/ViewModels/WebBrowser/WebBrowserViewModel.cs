using System;

namespace Bau.Applications.BauPlugStudio.ViewModels.WebBrowser
{
	/// <summary>
	///		ViewModel para la ventana del navegador
	/// </summary>
	public class WebBrowserViewModel : Libraries.BauMvvm.ViewModels.Forms.BaseFormViewModel
	{   
		// Variables privadas
		private string _url;

		public WebBrowserViewModel(string url)
		{
			Url = url;
		}

		/// <summary>
		///		Ejecuta una acción
		/// </summary>
		protected override void ExecuteAction(string action, object parameter)
		{
			switch (action)
			{
				case nameof(RefreshCommand):
						Globals.HostController.ControllerWindow.ShowMessage("Actualizar el navegador");
					break;
			}
		}

		/// <summary>
		///		Comprueba si puede ejecutar una acción
		/// </summary>
		protected override bool CanExecuteAction(string action, object parameter)
		{
			switch (action)
			{
				case nameof(RefreshCommand):
					return true;
				default:
					return false;
			}
		}

		/// <summary>
		///		Url a presentar en el navegador
		/// </summary>
		public string Url
		{
			get { return _url; }
			set { CheckProperty(ref _url, value, "Url"); }
		}
	}
}
