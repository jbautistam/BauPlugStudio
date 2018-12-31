using System;
using System.Windows.Controls;

using Bau.Applications.BauPlugStudio.ViewModels.Tools.Errors;
using Bau.Libraries.BauMvvm.Common.Controllers.Messengers.Common;
using Bau.Libraries.BauMvvm.Views.Forms;

namespace Bau.Applications.BauPlugStudio.Views.Tools.Errors
{
	/// <summary>
	///		Ventana con la lista de errores
	/// </summary>
	public partial class ListErrorView : UserControl, IFormView
	{   
		public ListErrorView()
		{ 
			// Inicializa los componentes
			InitializeComponent();
			// Inicializa el DataContext
			lswLog.DataContext = ViewModel = new ErrorItemListViewModel();
			FormView = new BaseFormView(ViewModel);
			// Asigna los manejadores de eventos
			Globals.HostController.HostViewModelController.Messenger.Sent += (sender, evntArgs) =>
														{
															if (evntArgs.MessageSent is MessageError message)
																Dispatcher.Invoke(new Action(() => ViewModel.AddError(message)), null);
														};
		}

		/// <summary>
		///		Actualiza la lista
		/// </summary>
		internal void RefreshList()
		{
			ViewModel.RefreshList();
		}

		/// <summary>
		///		ViewModel de lista de log
		/// </summary>
		public BaseFormView FormView { get; }

		/// <summary>
		///		ViewModel de lista de log
		/// </summary>
		public ErrorItemListViewModel ViewModel { get; }
	}
}
