using System;
using System.Windows;

using Bau.Libraries.LibDataBaseStudio.ViewModel.Reports;

namespace Bau.Plugins.DataBaseStudio.Views.Reports
{
	/// <summary>
	///		Formulario para el mantenimiento de los parámetros de ejecución de un informe
	/// </summary>
	public partial class ReportExecutionFileView : Window
	{
		public ReportExecutionFileView(ReportExecutionFileViewModel viewModel)
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
