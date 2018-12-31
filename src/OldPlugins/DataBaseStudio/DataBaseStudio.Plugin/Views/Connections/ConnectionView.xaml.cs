using System;
using System.Windows;

using Bau.Libraries.LibDataBaseStudio.ViewModel.Connections;

namespace Bau.Plugins.DataBaseStudio.Views.Connections
{
	/// <summary>
	///		Formulario para el mantenimiento de una conexión a base de datos
	/// </summary>
	public partial class ConnectionView : Window
	{
		public ConnectionView(ConnectionViewModel viewModel)
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
