using System;
using System.Windows;

using Bau.Libraries.LibDataBaseStudio.ViewModel.Reports;

namespace Bau.Plugins.DataBaseStudio.Views.Reports
{
	/// <summary>
	///		Formulario para el mantenimiento de los parámetros de ejecución de un informe
	/// </summary>
	public partial class ReportExecutionView : Window
	{
		public ReportExecutionView(ReportExecutionViewModel viewModel)
		{ 
			// Inicializa los componentes
			InitializeComponent();
			// Asigna el contexto de la ventana
			DataContext = viewModel;
			udtListConnections.InitControl(viewModel.Connections);
			udtListFiles.InitControl(viewModel.Files);
			viewModel.Close += (sender, result) =>
												{
													DialogResult = result.IsAccepted;
													Close();
												};
		}
	}
}
