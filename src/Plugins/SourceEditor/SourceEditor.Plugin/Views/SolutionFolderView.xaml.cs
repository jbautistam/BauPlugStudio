using System;
using System.Windows;

using Bau.Libraries.SourceEditor.ViewModel.Solutions;

namespace Bau.Libraries.SourceEditor.Plugin.Views
{
	/// <summary>
	///		Ventana para creación de una carpeta de proyecto en SourceEditor
	/// </summary>
	public partial class SolutionFolderView : Window
	{
		public SolutionFolderView(SolutionFolderViewModel viewModel)
		{ 
			// Inicializa los componentes
			InitializeComponent();
			// Inicializa el ViewModel
			DataContext = viewModel;
			viewModel.Close += (sender, result) =>
											{
												DialogResult = result.IsAccepted;
												Close();
											};
		}
	}
}
