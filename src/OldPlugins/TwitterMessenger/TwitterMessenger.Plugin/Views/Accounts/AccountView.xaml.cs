using System;
using System.Windows;

using Bau.Libraries.TwitterMessenger.ViewModel.Accounts;

namespace Bau.Plugins.TwitterMessenger.Views.Accounts
{
	/// <summary>
	///		Formulario para el mantenimiento de una cuenta
	/// </summary>
	public partial class AccountView : Window
	{
		public AccountView(AccountViewModel viewModel)
		{ 
			// Inicializa los componentes
			InitializeComponent();
			// Asigna el contexto de la ventana
			DataContext = viewModel;
			viewModel.Close += (sender, result) =>
											{
												DialogResult = result.IsAccepted;
												Close();
											};
		}
	}
}
