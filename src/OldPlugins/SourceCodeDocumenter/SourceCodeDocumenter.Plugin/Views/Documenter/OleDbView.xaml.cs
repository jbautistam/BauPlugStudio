using System;
using System.Windows;

using Bau.Libraries.LibSourceCodeDocumenter.ViewModel.Documenter;

namespace Bau.Plugins.SourceCodeDocumenter.Views.Documenter
{
	/// <summary>
	///		Formulario de definición de documentación de OleDb
	/// </summary>
	public partial class OleDbView : Window
	{
		public OleDbView(OleDbViewModel viewModel)
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
