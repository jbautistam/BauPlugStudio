using System;
using System.Windows;

using Bau.Libraries.LibSourceCodeDocumenter.ViewModel.Documenter;

namespace Bau.Plugins.SourceCodeDocumenter.Views.Documenter
{
	/// <summary>
	///		Formulario de definición de la documentación
	/// </summary>
	public partial class SourceCodeView : Window
	{
		public SourceCodeView(SourceCodeViewModel viewModel)
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
