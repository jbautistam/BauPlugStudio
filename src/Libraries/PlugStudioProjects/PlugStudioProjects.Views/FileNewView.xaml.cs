using System;
using System.Windows;

using Bau.Libraries.PlugStudioProjects.ViewModels.Definitions;

namespace Bau.Libraries.PlugStudioProjects.Views
{
	/// <summary>
	///		Ventana para creación de un documento en SourceEditor
	/// </summary>
	public partial class FileNewView : Window
	{
		public FileNewView(SelectNewFileViewModel viewModel)
		{ 
			// Inicializa los componentes
			InitializeComponent();
			// Inicializa el ViewModel
			ViewModel = viewModel;
			DataContext = ViewModel;
			ViewModel.Close += (sender, result) =>
											{
												DialogResult = result.IsAccepted;
												Close();
											};
		}

		/// <summary>
		///		ViewModel de la ventana
		/// </summary>
		public SelectNewFileViewModel ViewModel { get; }
	}
}
