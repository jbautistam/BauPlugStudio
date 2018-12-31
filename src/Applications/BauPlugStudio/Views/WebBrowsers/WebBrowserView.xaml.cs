using System;
using System.Windows.Controls;

using Bau.Applications.BauPlugStudio.ViewModels.WebBrowser;
using Bau.Libraries.BauMvvm.Views.Forms;

namespace Bau.Applications.BauPlugStudio.Views.WebBrowsers
{
	/// <summary>
	///		Ventana para presentar un WebBrowser
	/// </summary>
	public partial class WebBrowserView : UserControl, IFormView
	{
		public WebBrowserView(string url)
		{ 
			// Inicializa los componentes
			InitializeComponent();
			// Inicializa el DataContext
			grdData.DataContext = ViewModel = new WebBrowserViewModel(url);
			FormView = new BaseFormView(ViewModel);
			// Muestra una página en el explorador
			wbExplorer.ShowUrl(url);
		}

		/// <summary>
		///		ViewModel asociado al formulario
		/// </summary>
		public BaseFormView FormView { get; }

		/// <summary>
		///		ViewModel asociado al formulario
		/// </summary>
		public WebBrowserViewModel ViewModel { get; }

	}
}
