using System;
using System.Windows.Controls;

using Bau.Libraries.BauMvvm.Views.Forms;
using Bau.Libraries.TwitterMessenger.ViewModel.Messages;

namespace Bau.Plugins.TwitterMessenger.Views.Messages
{
	/// <summary>
	///		Ventana para mostrar el panel con los mensajes de las cuentas de Twitter
	/// </summary>
	public partial class TwitterMessagesView : UserControl, IFormView
	{   
		public TwitterMessagesView()
		{ 
			// Inicializa los componentes
			InitializeComponent();
			// Inicializa el dataContext
			grdMain.DataContext = ViewModel = new TwitterAccountsViewModel();
			FormView = new BaseFormView(ViewModel);
			// Inicializa el explorador
			wbMessages.FunctionExecute += (sender, evntArgs) => ViewModel.TreatExplorerFunction(evntArgs.Parameters);
			ViewModel.MessagesHtmlChanged += (sender, evntArgs) => ShowHtml();
			// Muestra los mensajes
			ShowHtml();
		}

		/// <summary>
		///		Muestra el HTML
		/// </summary>
		private void ShowHtml()
		{
			wbMessages.ShowHtml(ViewModel.HtmlMessages);
		}

		/// <summary>
		///		Datos del formulario
		/// </summary>
		public BaseFormView FormView { get; }

		/// <summary>
		///		Datos del formulario
		/// </summary>
		public TwitterAccountsViewModel ViewModel { get; }
	}
}