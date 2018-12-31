using System;
using System.Windows;

using Bau.Libraries.BauMessenger.ViewModel.Connections;

namespace Bau.Plugins.BauMessenger.Views.Connections
{
	/// <summary>
	///		Formulario para dar de alta una conexión
	/// </summary>
	public partial class NewConnectionView : Window
	{
		public NewConnectionView()
		{
			ConnectionViewModel connectiontViewModel = new ConnectionViewModel();

				// Inicializa los componentes
				InitializeComponent();
				// Inicializa el viewmodel
				DataContext = connectiontViewModel;
				connectiontViewModel.Close += (sender, result) =>
															{
																DialogResult = result.IsAccepted;
																Close();
															};
		}

		private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
		{
			(DataContext as ConnectionViewModel).Password = (sender as System.Windows.Controls.PasswordBox)?.Password;
		}

		private void RepeatPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
		{
			(DataContext as ConnectionViewModel).RepeatPassword = (sender as System.Windows.Controls.PasswordBox)?.Password;
		}
	}
}
