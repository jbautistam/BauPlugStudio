using System;
using System.Windows;

using Bau.Libraries.LibSourceCodeDocumenter.ViewModel.Documenter;

namespace Bau.Plugins.SourceCodeDocumenter.Views.Documenter
{
	/// <summary>
	///		Formulario de definición de documentación de SQL Server
	/// </summary>
	public partial class SqlServerView : Window
	{
		public SqlServerView(SqlServerViewModel viewModel)
		{ 
			// Inicializa los componentes
			InitializeComponent();
			// Inicializa la vista de datos
			DataContext = viewModel;
			viewModel.Close += (sender, result) =>
											{
												DialogResult = result.IsAccepted;
												Close();
											};
		}
	}
}
