using System;
using System.Windows.Controls;

using Bau.Libraries.BauMessenger.ViewModel.Chat;
using Bau.Libraries.BauMvvm.Views.Forms;

namespace Bau.Plugins.BauMessenger.Views.Chat
{
	/// <summary>
	///		Ventana de visualización de Chat
	/// </summary>
	public partial class ChatView : UserControl, IFormView
	{
		public ChatView(ChatViewModel chatViewModel)
		{ 
			// Inicializa el componente
			InitializeComponent();
			// Inicializa la vista de datos
			grdData.DataContext = ViewModel = chatViewModel;
			FormView = new BaseFormView(ViewModel);
		}

		/// <summary>
		///		ViewModel asociado al formulario
		/// </summary>
		public BaseFormView FormView { get; }

		/// <summary>
		///		ViewModel
		/// </summary>
		public ChatViewModel ViewModel { get; }
	}
}
