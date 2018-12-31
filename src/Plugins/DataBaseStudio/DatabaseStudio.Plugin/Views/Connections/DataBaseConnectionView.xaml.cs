using System;
using System.Windows;
using System.Windows.Controls;

using Bau.Libraries.DatabaseStudio.ViewModels.Projects.Connections;

namespace Bau.Libraries.FullDatabaseStudio.Plugin.Views.Connections
{
	/// <summary>
	///		Ventana con los datos de conexión a una base de datos
	/// </summary>
	public partial class DataBaseConnectionView : Window
	{
		public DataBaseConnectionView(DataBaseConnectionViewModel viewModel)
		{
			// Inicializa los componentes
			InitializeComponent();
			// Asocia el contexto
			DataContext = ViewModel = viewModel;
			txtPassword.Password = viewModel.Password; // ... txtPassword almacena un SecureString que no se puede asociar utilizando Binding
			ViewModel.Close += (sender, eventArgs) => 
									{
										DialogResult = eventArgs.IsAccepted; 
										Close();
									};
		}

		/// <summary>
		///		ViewModel
		/// </summary>
		private DataBaseConnectionViewModel ViewModel { get; }

		private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
		{	
			ViewModel.Password = (sender as PasswordBox)?.Password;
		}
	}
}
