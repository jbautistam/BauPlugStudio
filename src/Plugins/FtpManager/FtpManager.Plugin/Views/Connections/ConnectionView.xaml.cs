using System;
using System.Windows;

using Bau.Libraries.FtpManager.ViewModel.Connections;

namespace Bau.Plugins.FtpManager.Views.Connections
{
	/// <summary>
	///		Formulario para el mantenimiento de una conexión a un servidor FTP
	/// </summary>
	public partial class ConnectionView : Window
	{
		public ConnectionView(FtpConnectionViewModel viewModel)
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
